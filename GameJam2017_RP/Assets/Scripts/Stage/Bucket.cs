using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    public int m_waterLimit = 5;
    public bool m_isComplete = false;
    public Door m_lockedDoorScript = null;
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("sup!");
        // check if complete
        if (m_isComplete) return;
        // check for player bullet
        if (collider.tag == "Bullet")
        {
            Debug.Log("YESSSSSSSS!");
            // update values
            m_waterLimit--;
            // check if empty
            if (m_waterLimit == 0)
            {
                // set as complete
                m_isComplete = true;
                // unlock the door
                m_lockedDoorScript.m_isUnlocked = true;
            }
        }
    }
}