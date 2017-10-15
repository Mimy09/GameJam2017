using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {

    public Enemy Boss;
    public RectTransform BossBar;
    public bool EnableBossBar;

    void Start() {
        BossBar.parent.gameObject.SetActive(false);
    }

    void Update() {
        if (EnableBossBar) {
            BossBar.parent.gameObject.SetActive(true);
        }
        BossBar.sizeDelta = new Vector2((100.4f / 3000) * Boss.m_health, 100);
    }
}
