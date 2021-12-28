using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject m_prefabWall = null;

    [SerializeField] [Range(0.1f, 2f)] public float m_wallRiseTime = 0.5f;

    [SerializeField] [Tooltip("For testing purposes only")] private int m_wallXIndex = 0;
    [SerializeField] [Tooltip("For testing purposes only")] private int m_wallYIndex = 0;
    
    [SerializeField] private SOTileValues m_tileValues = null;
    
    

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

    [ContextMenu("Bob, build !")]
    public void Bob() {
        BuildAWall(m_wallXIndex, m_wallYIndex);
    }

    private void BuildAWall(int p_x, int p_y) {
        float posX;
        float posY;
        
        IndexesToPositions(p_x, p_y, out posX, out posY);

        Instantiate(m_prefabWall, new Vector3(posX, m_tileValues.m_center.y, posY), m_prefabWall.transform.rotation);
    }

    private void IndexesToPositions(int p_x, int p_y, out float x, out float y) {
        x = (m_tileValues.m_center.x - ((m_tileValues.m_numberOfXTiles * m_tileValues.m_sizeOfATile) / 2f))  +  p_x * m_tileValues.m_sizeOfATile  +  m_tileValues.m_sizeOfATile/2f;
        y = (m_tileValues.m_center.y - ((m_tileValues.m_numberOfYTiles * m_tileValues.m_sizeOfATile) / 2f))  +  p_y * m_tileValues.m_sizeOfATile  +  m_tileValues.m_sizeOfATile/2f;
    }

    public void SetTileValues(int p_numberOfXTiles, int p_numberOfYTiles, float p_sizeOfATile, Vector3 p_center) {
        m_tileValues.m_numberOfXTiles = p_numberOfXTiles;
        m_tileValues.m_numberOfYTiles = p_numberOfYTiles;
        m_tileValues.m_sizeOfATile = p_sizeOfATile;
        m_tileValues.m_center = p_center;
    }
}
