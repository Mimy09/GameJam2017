//////////////////////////////////////////
// Includes
using UnityEngine;
//////////////////////////////////////////

[RequireComponent(typeof(Player))]
public class Weapon : MonoBehaviour {

    //////////////////////////////////////////
    // PUBLIC VARIABLES
    public float ammo = 0;
    public float m_ammoMax = 10;
    public float m_ammoDrain = 0.5f;
    public float m_ammoGain = 2;
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
        if (ammo > 0) ammo -= (m_ammoDrain * Time.deltaTime);
        if (ammo < 1) ammo = 0;

        if (Input.GetMouseButtonDown(1)) {
            if (ammo > 0) ammo -= 1;
        }
    }
}
