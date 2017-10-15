//////////////////////////////////////////
// Includes
using UnityEngine;
//////////////////////////////////////////

[RequireComponent(typeof(Player))]
public class Weapon : MonoBehaviour {

    //////////////////////////////////////////
    // PUBLIC VARIABLES
    public float m_ammo = 0;
    public float m_ammoMax = 10;
    public float m_ammoDrain = 0.5f;
    public float m_ammoGain = 2;
    public float m_bulletForce = 50;
    public GameObject m_bullet;
    public LayerMask m_waterLayerMask;
    public Animator m_armAnimator;
    public GameObject m_partical;

    public RectTransform m_ammoBar;
    //////////////////////////////////////////


    //////////////////////////////////////////
    // PRIVATE VARIABLE
    private Player m_ply;
    private float m_timer;
    
    //////////////////////////////////////////

    void Start() {
        m_ply = this.GetComponent<Player>();
    }

    void Update() {
        if (m_ammo > 0) m_ammo -= (m_ammoDrain * Time.deltaTime);
        if (m_ammo < 0) m_ammo = 0;

        m_ammoBar.sizeDelta = new Vector2((231.0f / m_ammoMax) * m_ammo, 59);
        
        if (Input.GetMouseButton(1)) {
            m_armAnimator.SetBool("IsUp", true);
            if (m_ammo > m_ammoMax) m_ammo = m_ammoMax;
            if (Physics2D.OverlapBox(transform.position, new Vector2(0.8f, 1), 0, m_waterLayerMask) && m_ply.ArmAngle < 110 && m_ply.ArmAngle > 70) {
                if (m_ammo < m_ammoMax) m_ammo += (m_ammoGain * Time.deltaTime);
                m_partical.SetActive(true);
            } else {
                m_partical.SetActive(false);
            }
        } else {
            m_partical.SetActive(false);
            m_armAnimator.SetBool("IsUp", false);
        }
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButton(1)) {
            if (m_ammo > 0) {
                m_ammo -= 1;
                GameObject.FindGameObjectWithTag("Canvas").GetComponent<AudioManager>().PlayClip(1);
                GameObject temp = Instantiate(m_bullet, transform.position, Quaternion.Euler(0, 0, m_ply.ArmAngle - 90));
                temp.GetComponent<Rigidbody2D>().AddForce(temp.transform.up * m_bulletForce, ForceMode2D.Impulse);
            }
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, new Vector3(0.8f, 1, 0));
    }
}