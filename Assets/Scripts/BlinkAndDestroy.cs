using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlinkAndDestroy : MonoBehaviour {

    public float m_lifeTime = 2f;
    public float m_blinkTime = 0.2f;
    private Vector3 m_spreadDirection = Vector3.up;
    public float m_speed = 1f;
    
    private bool m_isTransparent = false;

    private MeshRenderer m_mr = null;

    private void Start() {
        m_mr = GetComponent<MeshRenderer>();
        StartCoroutine(Blink());
        StartCoroutine(DeathCountDown());

        m_spreadDirection = Random.insideUnitSphere;
    }

    private void Update() {
        transform.Translate(m_speed * Time.deltaTime * m_spreadDirection);
    }

    private IEnumerator Blink() {
        yield return new WaitForSeconds(m_blinkTime);
        m_isTransparent = !m_isTransparent;

        m_mr.enabled = !m_isTransparent;
        StartCoroutine(Blink());
    }

    private IEnumerator DeathCountDown() {
        yield return new WaitForSeconds(m_lifeTime);
        
        Destroy(gameObject);
    }
}
