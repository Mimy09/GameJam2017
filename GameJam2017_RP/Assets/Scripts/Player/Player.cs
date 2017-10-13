//////////////////////////////////////////
// Includes
using UnityEngine;
//////////////////////////////////////////

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    //////////////////////////////////////////
    // Public Variables

    /* ---- Player Vars ---- */
    public GameObject m_arm;

    /* ---- Player Movement Vars ---- */
    public int m_speed = 1;
    public int m_ladderSpeed = 10;
    public float m_jumpheight = 1;
    public LayerMask m_groundLayer;
    public LayerMask m_wallLayer;
    public LayerMask m_LadderLayer;
    public Transform m_groundCheck;
    public float m_groundRadius = 0.2f;
    //////////////////////////////////////////
    
    //////////////////////////////////////////
    // Private Variables
    private Rigidbody2D m_rigidbody2D;
    private bool m_canJump = true;
    private bool m_grounded = false;
    private bool m_collidingRight = false;
    private bool m_collidingLeft = false;
    private bool m_collidingLadder = false;
    private float move;
    //////////////////////////////////////////



    void Start() {
        m_rigidbody2D = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (m_arm != null) {
            //m_arm.transform.Rotate(Input.mousePosition);
        }

        //////////////////////////////////////////
        // Player Movement

        //* ---- Movement ---- */
        m_grounded = Physics2D.OverlapCircle(m_groundCheck.position, m_groundRadius, m_groundLayer);
        m_collidingRight = Physics2D.OverlapCircle((m_groundCheck.right * 0.7f) + transform.position, 0.2f, m_wallLayer);
        m_collidingLeft = Physics2D.OverlapCircle((-m_groundCheck.right * 0.7f) + transform.position, 0.2f, m_wallLayer);
        m_collidingLadder = Physics2D.OverlapCircle(m_groundCheck.position, m_groundRadius, m_LadderLayer);
        // Get movement
        move = Input.GetAxis("Horizontal");

        // Move left and right
        if (move < 0 && !m_collidingLeft) {
            m_rigidbody2D.velocity = new Vector2(move * m_speed, m_rigidbody2D.velocity.y); // Left
        } else if (move > 0 && !m_collidingRight) {
            m_rigidbody2D.velocity = new Vector2(move * m_speed, m_rigidbody2D.velocity.y); // Right
        }

        // Freeze the rigidbody in the Y when climbing ladder
        if (m_collidingLadder) {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY ^ RigidbodyConstraints2D.FreezeRotation;
        } else {
            m_rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            transform.Rotate(new Vector3(0, 0, 0));
        }
        
        /* ----  Ladder climbing ---- */
        if (Input.GetKey(KeyCode.W) && m_collidingLadder) { // UP
            transform.Translate(transform.up * m_ladderSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) && m_collidingLadder && !m_grounded) { // DOWN
            transform.Translate(-transform.up * m_ladderSpeed * Time.deltaTime);
        }

        /* ---- Arm Movement ---- */
        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 1;
        Vector3 object_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        object_pos = Camera.main.WorldToScreenPoint(m_arm.transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        m_arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

        
        /* ---- Jumping ---- */
        if (Input.GetKeyDown(KeyCode.Space)) { // JUMP
            if (m_grounded) m_rigidbody2D.AddForce(transform.up * m_jumpheight, ForceMode2D.Impulse);
        }

        //////////////////////////////////////////
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_groundCheck.position, m_groundRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere((m_groundCheck.right * 0.7f) + transform.position, 0.2f);
        Gizmos.DrawWireSphere((-m_groundCheck.right * 0.7f) + transform.position, 0.2f);
    }
}
