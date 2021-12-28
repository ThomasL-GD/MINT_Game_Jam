using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Dimensions")]
    [SerializeField] [Range(1, 100)] private int m_numberOfXTiles = 20;
    [SerializeField] [Range(1, 100)] private int m_numberOfYTiles = 20;
    
    [SerializeField] [Range(0.1f, 10f)] private float m_sizeOfATile = 20;
    [SerializeField] private Vector3 m_center = Vector3.zero;

    [Header(" ")] [Header("Tiles")]
    [SerializeField] private GameObject[] m_tileDataBase = null;
    
    
    //Singleton time ! 	(˵ ͡° ͜ʖ ͡°˵)
    public static GameManager singleton { get; private set; }
        
    /// <summary>
    /// Is that... a singleton setup ?
    /// *Pokédex's voice* A singleton, a pretty common pokécode you can find in a lot of projects, it allows anyone to
    /// call it and ensure there is only one script of this type in the entire scene !
    /// </summary>
    private void Awake() {
        //base.Awake();
        // if the singleton hasn't been initialized yet
        if (singleton != null && singleton != this)
        {
            gameObject.SetActive(this);
            Debug.LogWarning("BROOOOOOOOOOOOOOOOOOO ! There are too many Singletons broda", this);
            return;
        }
 
        singleton = this;
    }


    private void CreateGrid() {
        
        for(int i = 0; i < m_numberOfYTiles; i++) {
            for(int j = 0; j < m_numberOfXTiles; j++) {
                CreateTile(j,i);
            }
        }
        
    }

    private void CreateTile(int p_x, int p_y) {
        float x = (m_center.x - ((m_numberOfXTiles * m_sizeOfATile) / 2f))  +  p_x * m_sizeOfATile;
    }
}
