using System.Collections;
using UnityEngine;

public class BlinkHP : MonoBehaviour {

    public float m_blinkDuration = 2f;
    public float m_blinkTime = 0.2f;
    
    private bool m_isTransparent = false;

    private MeshRenderer m_mr = null;

    public void Initialize(bool p_mustDisappear) {
        m_mr = GetComponent<MeshRenderer>();
        StartCoroutine(Blink());
        StartCoroutine(DeathCountDown(p_mustDisappear));
    }

    private IEnumerator Blink() {
        yield return new WaitForSeconds(m_blinkTime);
        m_isTransparent = !m_isTransparent;

        m_mr.enabled = !m_isTransparent;
        StartCoroutine(Blink());
    }

    private IEnumerator DeathCountDown(bool p_mustDisappear) {
        yield return new WaitForSeconds(m_blinkDuration);
        
        gameObject.SetActive(!p_mustDisappear);
        Destroy(this);
    }
}