using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFatOnInstantiate : MonoBehaviour
{
    [SerializeField] private float m_growingSpeed = 3;

    private Vector3 m_ogScale;
    // Start is called before the first frame update
    void Awake()
    {
        m_ogScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Vector3.one * (m_growingSpeed * Time.deltaTime);

        if (transform.localScale.magnitude >= m_ogScale.magnitude)
        {
            transform.localScale = m_ogScale;
            Destroy(this);
        }
    }
}
