using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SOs/Tile Values")]
public class SOTileValues : ScriptableObject {
    
    public int m_numberOfXTiles = 20;
    public int m_numberOfYTiles = 20;
    
    public float m_sizeOfATile = 1;
    public Vector3 m_center = Vector3.zero;
}
