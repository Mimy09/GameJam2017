using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CanvasListener : EventListener
{
    // variables [public]
    public float m_speed = 5.0f;
    public Image m_backgroundBlack;
    // variables [private]
    private Event m_currentEvent;
    private bool m_animationDone;
    private float m_animationCount;
    private float m_animationOffset;
    private float m_animationTarget;
    private float m_animationValue1;
    private float m_animationValue2;

    void Start()
    {
        // initialize variables
        m_currentEvent = Event.NONE;
        m_animationDone = true;
        m_animationTarget = 0.0f;
        m_animationValue2 = 0.0f;
        m_animationValue1 = m_backgroundBlack.color.a;

        HandleEvent(Event.START);
    }

    // Update is called once per frame
    void Update()
    {
        // check if animation complete
        if (!m_animationDone)
        {
            // check animation count
            if (m_animationCount > 0)
            {
                // update animation
                m_animationCount -= Time.deltaTime;
                Color color = m_backgroundBlack.color;
                color.a += m_animationOffset * Time.deltaTime;
                m_backgroundBlack.color = color;
            }
            else
            {
                // finalize animation
                m_animationDone = true;
                Color color = m_backgroundBlack.color;
                color.a = m_animationTarget;
                m_backgroundBlack.color = color;
                // check if gameover
                if (m_currentEvent == Event.GAMEOVER)
                {
                    // restart
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }

    override public void HandleEvent(Event e)
    {
        switch(e)
        {
            case Event.START:
            case Event.RESTART:
                m_animationDone = false;
                m_animationCount = m_speed - m_animationCount;
                m_animationOffset = (m_animationValue2 - m_backgroundBlack.color.a) / m_animationCount;
                break;
            case Event.DEATH:
            case Event.GAMEOVER:
                m_animationDone = false;
                m_animationCount = m_speed - m_animationCount;
                m_animationOffset = (m_animationValue1 - m_backgroundBlack.color.a) / m_animationCount;
                break;
        }
        // store event
        m_currentEvent = e;
    }
}
