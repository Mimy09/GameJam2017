using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float m_timer = 2;

	void Update () {
        m_timer -= Time.deltaTime;
        if (m_timer < 0) Destroy(this.gameObject);
    }
    void OnTriggerEnter2D(Collider2D c) {
        if (c.tag != "Player") {
            if (c.tag != "Water") {
                Destroy(this.gameObject);
            }
        }
    }
}
