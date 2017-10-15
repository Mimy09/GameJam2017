using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartAnimationController : MonoBehaviour {

    public Animator m_animator;
    private CanvasListener cl;
    private bool m_isDone = false;
	// Use this for initialization
	void Start () {
        m_animator = GetComponent<Animator>();
        cl = GetComponent<CanvasListener>();
    }
	
	// Update is called once per frame
	void Update () {
		if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("End") && !m_isDone)
        {
            cl.m_speed = 1;
            m_isDone = true;
            cl.HandleEvent(Event.FADEOUT);
            
        }
        if (cl.m_animationDone && m_isDone)
        {
            SceneManager.LoadScene(1);
        }
    }
}
