using UnityEngine;

public class RiseWhenBorn : MonoBehaviour {
    
    private float m_originalHeight = 0f;
    private float m_targetHeightGain = 0f;
    private float m_elapsedTime = 0f;
    private float m_maxTime = 0f;
    private bool m_isDone = false;
    
    private void OnEnable() {
        m_maxTime = GameManager.singleton.m_wallRiseTime;
        m_originalHeight = transform.position.y;
        m_targetHeightGain = GetComponent<MeshFilter>().mesh.bounds.size.z;
        Debug.Log(m_targetHeightGain);
    }

    private void Update() {

        if (m_isDone) return;

        m_elapsedTime += Time.deltaTime;

        if (m_elapsedTime > m_maxTime) {
            Transform myTransform = transform;
            Vector3 position = myTransform.position;
            position = new Vector3(position.x, m_originalHeight + m_targetHeightGain, position.z);
            myTransform.position = position;

            m_isDone = true;
            return;
        }
        
        float ratio = m_elapsedTime / m_maxTime;
        
        Transform transform1 = transform;
        Vector3 position1 = transform1.position;
        position1 = new Vector3(position1.x, m_originalHeight + (m_targetHeightGain * ratio), position1.z);
        transform1.position = position1;
    }
}
