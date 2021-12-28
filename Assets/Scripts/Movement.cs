using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField, Range(0f, 300f), Tooltip("the maximum speed of the player in m/s")]
    private float m_maxSpeed = 10f;
    [SerializeField, Range(0f, 300f), Tooltip("the acceleration of the player in m/s²")]
    private float m_maxAcceleration = 10f;
    
    [SerializeField, Range(0f, 300f), Tooltip("the maximum speed of the player in m/s")]
    private float m_maxSpeedSprint = 10f;
    [SerializeField, Range(0f, 300f), Tooltip("the acceleration of the player in m/s²")]
    private float m_maxAccelerationSprint = 10f;
    
    [SerializeField, Range(0f, 1440f), Tooltip("the rotation of the player in °/s")]
    private float m_rotationSpeed = 360f;

    [SerializeField,Tooltip("the sprinting key")] private KeyCode m_sprintKey;
    /// <summary/> the player's rigidbody
    Rigidbody m_rigidBody;

    void Awake () => m_rigidBody = GetComponent<Rigidbody>();


    /// <summary/> Update is called once per frame
    void Update() => m_rigidBody.velocity = SetVelocityFromInput(m_rigidBody.velocity);

    private Vector3 SetVelocityFromInput(Vector3 p_currentVelocity)
    {
        bool sprinting = Input.GetKey(m_sprintKey);
        float maxSpeed = sprinting?m_maxSpeedSprint:m_maxSpeed;
        float maxAcceleration = sprinting?m_maxAccelerationSprint:m_maxAcceleration;

        // Fetching the player's input
        Vector2 playerInput = Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")), 1f);

        // Calculating the desired velocity based of the player input 
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        // Calculating the maximum speed at which the velocity will interpolate
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        
        // Interpolating the current acceleration towards the desired velocity
        p_currentVelocity.x = Mathf.MoveTowards(p_currentVelocity.x, desiredVelocity.x, maxSpeedChange);
        p_currentVelocity.z = Mathf.MoveTowards(p_currentVelocity.z, desiredVelocity.z, maxSpeedChange);

        
        
        RotatePlayerTowardsInput(new Vector3(playerInput.x,0,playerInput.y));
        // Returning the calculated Velocity
        return p_currentVelocity;
    }

    private void RotatePlayerTowardsInput(Vector3 p_playerInput)
    {
        if(p_playerInput.magnitude < .01f)
        {
            PlayIdleAnimation();
            return;
        }
        
        PlayRunningAnimation();
        
        Quaternion targetRotation = Quaternion.LookRotation(p_playerInput);
 
        if(Quaternion.Angle(transform.rotation,targetRotation) < 15f)
        {
            transform.rotation = targetRotation;
            transform.rotation *= Quaternion.Euler(Vector3.up);
            return;
        }
        //Rotate smoothly to this target:
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);

        transform.rotation *= Quaternion.Euler(Vector3.up);
    }
    
    private void PlayRunningAnimation()
    {
        
    }
    private void PlayIdleAnimation()
    {
        
    }
}
