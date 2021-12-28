using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField, Range(0f, 300f), Tooltip("the maximum speed of the player in m/s")]
    private float m_maxSpeed = 10f;
    [SerializeField, Range(0f, 300f), Tooltip("the acceleration of the player in m/s²")]
    private float m_maxAcceleration = 10f;

    /// <summary/> the current velocity of the player
    Vector3 m_velocity;
    
    /// <summary/> the player's rigidbody
    Rigidbody m_rigidBody;

    void Awake () => m_rigidBody = GetComponent<Rigidbody>();
    
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // Fetching the player's input
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        // Calculating the desired velocity based of the player input 
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * m_maxSpeed;
        
        m_velocity = m_rigidBody.velocity;
        
        // Calculating the maximum speed at which the velocity will interpolate
        float maxSpeedChange = m_maxAcceleration * Time.deltaTime;
        
        // Interpolating the current acceleration towards the desired velocity
        m_velocity.x = Mathf.MoveTowards(m_velocity.x, desiredVelocity.x, maxSpeedChange);
        m_velocity.z = Mathf.MoveTowards(m_velocity.z, desiredVelocity.z, maxSpeedChange);
        
        m_rigidBody.velocity = m_velocity;
    }
}
