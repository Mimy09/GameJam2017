using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAction : MonoBehaviour
{
    // variables [public]
	public Event m_event = Event.NONE;
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
	
    public void SendEvent()
    {
        // send stored event
        SendEvent(m_event);
    }

    public void SendEvent(Event e)
    {
        // handle event in listener
        if (m_listener) m_listener.HandleEvent(e);
    }
}
