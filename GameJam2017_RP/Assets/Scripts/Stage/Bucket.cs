using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    public int m_waterLimit = 5;
    public bool m_isComplete = false;
    public Door m_lockedDoorScript = null;

    public SpriteRenderer m_graphics;
    public Sprite m_bucketFull;
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        // check if complete
        if (m_isComplete) return;
        // check for player bullet
        if (collider.tag == "Bullet")
        {
            // update values
            m_waterLimit--;
            // check if empty
            if (m_waterLimit == 0)
            {
                // set as complete
                m_isComplete = true;
                // unlock the door
                m_lockedDoorScript.m_isUnlocked = true;

                m_graphics.sprite = m_bucketFull;
            }
        }
    }
}