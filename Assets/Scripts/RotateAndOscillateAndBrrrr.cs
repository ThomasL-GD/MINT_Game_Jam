using UnityEngine;

public class RotateAndOscillateAndBrrrr : MonoBehaviour
{
    [SerializeField, Tooltip("the rotation speed of the floating object")]
    private float m_rotateSpeed = 180f;
    
    [SerializeField,Tooltip("the angle at which the object will spin")] 
    private float m_spinAngle = 30f;

    [SerializeField, Tooltip("the offset at which the object will spin")]
    private Vector3 m_offset = Vector3.zero;
    
    [Header("oscillation"),SerializeField, Tooltip("Ascillation frequency and intensity")]
    private float m_frequency = 1f, m_intensity = 1f;

    private Vector3 startPos;
    private void Start()
    {
        transform.localRotation = Quaternion.Euler(0f,0f,m_spinAngle);
        startPos = transform.localPosition;
    }

    private void Update()
    {
        transform.RotateAround(startPos + m_offset, Vector3.up, m_rotateSpeed * Time.deltaTime);
        transform.position += Vector3.up * (Mathf.Sin(Time.unscaledTime * m_frequency) * m_intensity * Time.deltaTime);
    }
}
