using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // variables [public]
    public bool m_isUnlocked = false;
    [Range(0, 2)] public float m_speed = 0.5f; 
    // variables [private]
    private bool m_isOpen = false;
    private float m_frameCount = 0;
    private Vector3 m_offset;
    private Vector3 m_position1;
    private Vector3 m_position2;
    private Vector3 m_targetPosition;
    private bool m_isAnimationFinished = true;

    void Start ()
    {
        // initialize variables
        m_offset = new Vector3();
        m_position1 = transform.position;
        m_position2 = transform.position + Vector3.up * transform.localScale.y * 2;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // check if animation finished
		if (!m_isAnimationFinished)
        {
            // check frame count
            if (m_frameCount > 0)
            {
                // translate door
                m_frameCount -= Time.deltaTime;
                transform.Translate(m_offset * Time.deltaTime);
            }
            else
            {
                // stop animation
                m_isAnimationFinished = true;
                transform.position = m_targetPosition;
            }
        }
        else if (m_isOpen && !m_isUnlocked)
        {
            // set boolean
            m_isOpen = false;
            m_isAnimationFinished = false;
            // calculate animation info
            m_frameCount = m_speed - m_frameCount;
            m_targetPosition = m_position1;
            m_offset = (m_position1 - transform.position) / m_frameCount;
        }
	}

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag != "Player") return;
        // check if unlocked
        if (!m_isUnlocked || !m_isOpen) return;
        // set boolean
        m_isOpen = false;
        m_isAnimationFinished = false;
        // calculate animation info
        m_frameCount = m_speed - m_frameCount;
        m_targetPosition = m_position1;
        m_offset = (m_position1 - transform.position) / m_frameCount;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "Player") return;
        // check if unlocked
        if (!m_isUnlocked || m_isOpen) return;
        // set boolean
        m_isOpen = true;
        m_isAnimationFinished = false;
        // calculate animation info
        m_frameCount = m_speed - m_frameCount;
        m_targetPosition = m_position2;
        m_offset = (m_position2 - transform.position) / m_frameCount;
    }
}
