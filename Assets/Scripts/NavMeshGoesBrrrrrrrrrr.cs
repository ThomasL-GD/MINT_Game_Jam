using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGoesBrrrrrrrrrr : MonoBehaviour {

    [SerializeField] public Transform m_transformToFollow = null;

    [Header("Jump")]
    [SerializeField] [Range(0.1f, 10f)] private float m_jumpCooldown = 1f;
    [SerializeField] [Range(0.1f, 1f)] private float m_jumpDuration = 0.2f;
    [SerializeField] [Range(0.1f, 10f)] private float m_jumpDistance = 2f;

    private float m_jumpElapsedTime = 0f;
    private bool m_isJumping = false;
    
    private Vector3 m_jumpOrigin = Vector3.zero;
    private Vector3 m_jumpTargetPos = Vector3.zero;

    [Header("NavMesh being demanding")]
    [SerializeField] [Range(0.001f, 1f)] private float m_ignoreStep = 0.2f;
    
    private NavMeshAgent m_meshAgent = null;
    private bool m_ismTransformToFollowNull;

    private Animator m_animator;
    private static readonly int JumpDuration = Animator.StringToHash("Jump_Duration");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private void Start() {
        m_ismTransformToFollowNull = m_transformToFollow == null;
        m_meshAgent = GetComponent<NavMeshAgent>();
        m_meshAgent.updateRotation = true;
        m_animator = GetComponent<Animator>();
        m_animator.SetFloat(JumpDuration, 1f/m_jumpDuration);
        StartCoroutine(JumpTimer());
    }

    private void Update() {
        if (m_ismTransformToFollowNull) return;
        
        if (!m_isJumping) return;

        m_jumpElapsedTime += Time.deltaTime;

        if (m_jumpElapsedTime >= m_jumpDuration) {
            m_isJumping = false;
            transform.position = m_jumpTargetPos;
            return;
        }

        float ratio = m_jumpElapsedTime / m_jumpDuration;
        transform.position = (Mathf.Sqrt(ratio) * (m_jumpTargetPos - m_jumpOrigin)) + m_jumpOrigin;
    }

    private IEnumerator JumpTimer() {

        yield return new WaitForSeconds(m_jumpCooldown);
        
        m_animator.SetTrigger(Jump);

        NavMeshPath path = new NavMeshPath();
        bool pathFound = m_meshAgent.CalculatePath(m_transformToFollow.position, path);
        for(int i = 0; i < path.corners.Length -1; i++) Debug.DrawLine(path.corners[i], path.corners[i+1], Color.magenta, m_jumpCooldown);
        
        if(pathFound) {
            m_jumpOrigin = transform.position;
            int desiredIndex;
            for(desiredIndex = 1; desiredIndex < path.corners.Length ; desiredIndex++) {
                if ((path.corners[0] - path.corners[desiredIndex]).magnitude > m_ignoreStep) {
                    break;
                }
            }

            Vector3 cornerPos = path.corners[desiredIndex];

            Vector3 position = transform.position;
            m_jumpTargetPos = position + (cornerPos - position).normalized * m_jumpDistance;
            //Debug.Log($"desired index : {desiredIndex}    desired position : {cornerPos}   reference position : {transform.position}    normalized direction : {(cornerPos - transform.position)}");
            Debug.DrawLine(m_jumpOrigin, m_jumpTargetPos, Color.red, m_jumpCooldown);
            transform.rotation = Quaternion.LookRotation((cornerPos - position).normalized, Vector3.up);
            m_isJumping = true;
            m_jumpElapsedTime = 0f;
        }
        
        StartCoroutine(JumpTimer());
        
    }

    /*
    private void OnCollisionEnter(Collision p_other) {
        Debug.Log("1");
        if (p_other.gameObject.layer == 8) {
            Debug.Log("2");
            if (p_other.gameObject.TryGetComponent(out OvenBehavior script)) {
                Debug.Log("3");
                script.RunAway(true);
                
            }
        }
    }*/
}