//////////////////////////////////////////
// Includes
using UnityEngine;
using UnityEngine.SceneManagement;
//////////////////////////////////////////

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    //////////////////////////////////////////
    // Public Variables

    /* ---- Player Vars ---- */
    public GameObject m_arm;
    public GameObject m_body;

    public float m_health = 100;
    public float m_healthWaterDamage = 10;

    /* ---- Player Movement Vars ---- */
    public int m_speed = 1;
    public int m_ladderSpeed = 10;
    public float m_jumpheight = 1;
    public float m_damageVelocity = 35;
    public LayerMask m_groundLayer;
    public LayerMask m_wallLayer;
    public LayerMask m_LadderLayer;
    public LayerMask m_water;
    public LayerMask m_toxicWater;
    public LayerMask m_toxicWaterPool;
    public LayerMask m_BossRoomCheck;
    public LayerMask m_EndGameCheck;
    public Transform m_groundCheck;
    public float m_groundRadius = 0.2f;
    public GameObject m_graphics;
    public Vector2 m_wallCollider;
    public float m_wallColliderOffst;
    public Door m_bossDoor;
    public Door m_endDoor;
    public Enemy m_boss;
    public RectTransform m_healthBar;
    //////////////////////////////////////////

    //////////////////////////////////////////
    // Private Variables
    private Rigidbody2D m_rigidbody2D;
    private Animator m_animator;
    private bool m_canJump = true;
    private bool m_grounded = false;
    private bool m_collidingRight = false;
    private bool m_collidingLeft = false;
    private bool m_collidingLadder = false;
    private bool m_fliped = false;
    private bool m_isDead = false;
    private bool m_isInBossRoom = false;
    private bool m_isFinished = false;
    private float move;
    private float angle;
    private GameObject m_canvas;
    //////////////////////////////////////////

    public float ArmAngle {
        get { return angle; }
    }

    void Start() {
        // Get the rigidbody
        m_rigidbody2D = this.GetComponent<Rigidbody2D>();
        m_animator = m_body.GetComponent<Animator>();
        m_canvas = GameObject.FindGameObjectWithTag("Canvas");
    }

    void Update() {
        //////////////////////////////////////////
        // Player Movement

        m_healthBar.sizeDelta = new Vector2((231.0f / 100) * m_health, 59);

        //  Damage / restart scene
        if (Physics2D.OverlapCircle(m_groundCheck.position - transform.up * 1.5f, m_groundRadius) && m_rigidbody2D.velocity.y < m_damageVelocity) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (m_health <= 0 && !m_isDead) {
            m_isDead = true;
            if (m_isInBossRoom) {
                m_canvas.GetComponent<CanvasListener>().m_speed = 0.5f;
                m_canvas.GetComponent<CanvasListener>().HandleEvent(Event.FADEOUT);
            } else {
                m_canvas.GetComponent<CanvasListener>().m_speed = 1;
                m_canvas.GetComponent<CanvasListener>().HandleEvent(Event.GAMEOVER);
            }
        }

        if (m_isInBossRoom && m_canvas.GetComponent<CanvasListener>().m_animationDone && m_isDead) {
            transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
            m_health = 100;
            m_isDead = false;
            m_bossDoor.m_isUnlocked = true;
            m_canvas.GetComponent<CanvasListener>().HandleEvent(Event.FADEIN);
        }

        if (Physics2D.OverlapBox(transform.position, new Vector2(0.8f, 1), 0, m_toxicWaterPool)) {
            m_health -= m_healthWaterDamage * Time.deltaTime;
        }
        if (Physics2D.OverlapBox(transform.position, new Vector2(0.8f, 1), 0, m_toxicWater)) {
            if (angle < 110 && angle > 70) {
            if (!Input.GetMouseButton(1)) m_health -= m_healthWaterDamage * Time.deltaTime;
            } else {
                m_health -= m_healthWaterDamage * Time.deltaTime;
            }
        }

        if (Physics2D.OverlapBox(transform.position, new Vector2(0.8f, 1), 0, m_BossRoomCheck)) {
            m_canvas.GetComponent<BossHealth>().EnableBossBar = true;
            m_bossDoor.m_isUnlocked = false;
            m_isInBossRoom = true;
        }

        if (m_boss.m_health <= 0) {
            m_endDoor.m_isUnlocked = true;
        }

        if (Physics2D.OverlapBox(transform.position, new Vector2(0.8f, 1), 0, m_EndGameCheck) && !m_isFinished) {
            m_isFinished = true;
            m_canvas.GetComponent<CanvasListener>().m_speed = 8;
            m_canvas.GetComponent<CanvasListener>().HandleEvent(Event.GAMEOVER);
        }

        //* ---- Movement ---- */
        m_grounded = Physics2D.OverlapCircle(m_groundCheck.position, m_groundRadius, m_groundLayer);
        m_collidingRight = Physics2D.OverlapBox((m_groundCheck.right * m_wallColliderOffst) + transform.position, m_wallCollider, 0, m_wallLayer);
        m_collidingLeft = Physics2D.OverlapBox((-m_groundCheck.right * m_wallColliderOffst) + transform.position, m_wallCollider, 0, m_wallLayer);
        m_collidingLadder = Physics2D.OverlapCircle(m_groundCheck.position, m_groundRadius, m_LadderLayer);
        // Get movement
        move = Input.GetAxis("Horizontal");

        // Animations
        if (move != 0) m_animator.SetBool("Walking", true);
        else m_animator.SetBool("Walking", false);

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
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
        if (angle < 90 && angle > -90)
            m_arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        else
            m_arm.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));

        /* ---- Player Flipping ---- */
        if (angle < 90 && angle > -90) {
            // m_arm.GetComponent<SpriteRenderer>().flipY = m_fliped = false;

            Vector3 scale = m_graphics.transform.localScale;
            scale.x = 1;
            m_graphics.transform.localScale = scale;

            //m_body.GetComponent<SpriteRenderer>().flipX = false;
        } else {
            // m_arm.GetComponent<SpriteRenderer>().flipY = m_fliped = true;

            Vector3 scale = m_graphics.transform.localScale;
            scale.x = -1;
            m_graphics.transform.localScale = scale;

            //m_body.GetComponent<SpriteRenderer>().flipX = true;
        }

        /* ---- Jumping ---- */
        if (Input.GetKeyDown(KeyCode.Space)) { // JUMP
            if (m_grounded) m_rigidbody2D.AddForce(transform.up * m_jumpheight, ForceMode2D.Impulse);
        }

        //////////////////////////////////////////
    }
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_groundCheck.position, m_groundRadius);
        Gizmos.DrawWireSphere(m_groundCheck.position - transform.up * 1.5f, m_groundRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((m_groundCheck.right * m_wallColliderOffst) + transform.position, new Vector3(m_wallCollider.x, m_wallCollider.y, 0));
        Gizmos.DrawWireCube((-m_groundCheck.right * m_wallColliderOffst) + transform.position, new Vector3(m_wallCollider.x, m_wallCollider.y, 0));
    }
}
