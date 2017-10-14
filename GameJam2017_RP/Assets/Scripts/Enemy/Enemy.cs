using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // variables [public]
    public float m_speed = 3.0f;
    public float m_damage = 25.0f;
    public float m_health = 100.0f;
    public bool m_collisionResult = false;
    public float m_collisonRadius = 0.5f;
    public float m_collisonOffsetX = 1.5f;
    public float m_collisonOffsetY = -1.0f;
    public LayerMask m_collisionLayerMask = 0;
    // variables [private]
    private float m_moveDirection = 1.0f;
    private Vector3 m_colliderPosition;
    
    void Start ()
    {
        // validate variables
        OnValidate();
    }
	
	void Update ()
    {
        // check for direction collision
		if (Physics2D.OverlapCircle(transform.position + m_colliderPosition, m_collisonRadius, m_collisionLayerMask) == m_collisionResult)
        {
            // update direction
            m_moveDirection = -m_moveDirection;
            // update collision position
            m_colliderPosition.x = -m_colliderPosition.x;
        }
        else
        {
            // move enemy in direction
            transform.Translate(m_moveDirection * m_speed * Time.deltaTime, 0, 0);
        }
	}

    void OnValidate()
    {
        // initialize variables [public]
        m_speed = Mathf.Abs(m_speed);
        m_collisonRadius = Mathf.Abs(m_collisonRadius);
        m_collisonOffsetX = Mathf.Abs(m_collisonOffsetX);
        // initialize variables [private]
        m_colliderPosition = new Vector3(m_collisonOffsetX * m_moveDirection, m_collisonOffsetY, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = m_moveDirection < 0 ? Color.magenta : Color.red;
        Gizmos.DrawWireSphere(new Vector3(m_collisonOffsetX, m_collisonOffsetY, 0) + transform.position, m_collisonRadius);
        Gizmos.color = m_moveDirection < 0 ? Color.red : Color.magenta;
        Gizmos.DrawWireSphere(new Vector3(-m_collisonOffsetX, m_collisonOffsetY, 0) + transform.position, m_collisonRadius);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Bullet")
        {
            m_health -= m_damage;
            if (m_health <= 0.0f) Destroy(gameObject);
        }
    }
}
