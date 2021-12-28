using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGoesBrrrrrrrrrr : MonoBehaviour {

    [SerializeField] private Transform m_transformToFollow = null;
    
    private NavMeshAgent m_meshAgent = null;
    private bool m_ismTransformToFollowNull;

    private void Start() {
        m_ismTransformToFollowNull = m_transformToFollow == null;
        m_meshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if(m_ismTransformToFollowNull) return;
        m_meshAgent.destination = m_transformToFollow.position;
    }
}
