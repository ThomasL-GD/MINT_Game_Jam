using UnityEngine;

public class DisableChildren : MonoBehaviour
{
    [SerializeField] private GameObject[] m_gos;
    
    void Awake()
    {
        foreach (var go in m_gos)
        {
            if (Random.Range(0, 2) == 1)
            {
                Destroy(go);
            }
        }
    }
}
