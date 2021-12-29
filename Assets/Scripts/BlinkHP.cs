using System;
using System.Collections;
using UnityEngine;

public class BlinkHP : MonoBehaviour {

    public float m_blinkDuration = 2f;
    public float m_blinkTime = 0.2f;
    
    private bool m_isTransparent = false;

    private MeshRenderer m_mr = null;

    private bool m_isBlinking = false;

    private void Start() {
        m_mr = GetComponent<MeshRenderer>();
    }

    public void Initialize(bool p_mustDisappear) {
        m_isBlinking = true;
        StartCoroutine(Blink());
        StartCoroutine(DeathCountDown(p_mustDisappear));
    }

    private IEnumerator Blink() {
        yield return new WaitForSeconds(m_blinkTime);

        if (!m_isBlinking) yield break;
        
        m_isTransparent = !m_isTransparent;

        m_mr.enabled = !m_isTransparent;
        StartCoroutine(Blink());
    }

    private IEnumerator DeathCountDown(bool p_mustDisappear) {
        yield return new WaitForSeconds(m_blinkDuration);

        m_isBlinking = false;
        m_mr.enabled = !p_mustDisappear;
        gameObject.SetActive(!p_mustDisappear);
    }
}