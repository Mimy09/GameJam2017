using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    // variables [private]
    private EventListener m_listener = null;

    void Awake()
    {
        // find event listener
        EventListener listener = GetComponentInParent<EventListener>();
        // check if null object
        if (listener == null) return;
        // check if not the same object
        if (listener.gameObject != gameObject)
        {
            // store listener
            m_listener = listener;
        }
    }

    public void SendEvent(Event e)
    {
        // handle event in listener
        if (m_listener) m_listener.HandleEvent(e);
    }

    virtual public void HandleEvent(Event e) { SendEvent(e); }
}
