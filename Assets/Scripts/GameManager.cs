using System;
using Ingredients;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ingredients {
    public enum IngredientList {
        Egg = 0,
        Flour = 1,
        Strawberry = 2,
        Chocolate = 3,
    }
}

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject m_prefabWall = null;

    [SerializeField] [Range(0.1f, 2f)] public float m_wallRiseTime = 0.5f;

    [SerializeField] [Tooltip("For testing purposes only")] private int m_wallXIndex = 0;
    [SerializeField] [Tooltip("For testing purposes only")] private int m_wallYIndex = 0;
    
    [SerializeField] private SOTileValues m_tileValues = null;

    [SerializeField] private GameObject[] m_ingredientsPrefabs;

    [SerializeField, Range(0f, 5f)] private float m_ingredientYOffset = 1f;

    [SerializeField] private WallsToBuild[] m_firstWallsToBuild;

    private bool[,] m_walls = null;
    private Transform m_wallsParent = null;
    

    [Serializable]
    private struct WallsToBuild {
        public bool isHorizontal;
        public int otherAxeCoordinate;
        public int begin;
        public int end;
    }
    
    

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

        m_wallsParent = Instantiate(new GameObject(name = "Walls")).transform;
        m_walls = new bool[m_tileValues.m_numberOfXTiles, m_tileValues.m_numberOfYTiles];
    }

    private void Start() {
        
        foreach (WallsToBuild walls in m_firstWallsToBuild) {
            if(walls.isHorizontal) BuildLineOfWallsX(walls.otherAxeCoordinate, walls.begin, walls.end);
            else BuildLineOfWallsY(walls.otherAxeCoordinate, walls.begin, walls.end);
        }
        
        Raoul();
        Raoul();
        Raoul();
        Raoul();
        Raoul();
        Raoul();
        Raoul();
    }

    private void OnValidate() {
        if (m_ingredientsPrefabs.Length != Enum.GetNames(typeof(IngredientList)).Length) {
            Debug.LogWarning("Don't change the length of this array, there's nothing more or less to show !");
            m_ingredientsPrefabs = new GameObject[Enum.GetNames(typeof(IngredientList)).Length];
        }
    }

    [ContextMenu("Bob, build !")]
    public void Bob() {
        BuildAWall(m_wallXIndex, m_wallYIndex);
    }

    private void BuildLineOfWallsX(int p_y, int p_begin, int p_end) {
        for (int i = 0; i <= Mathf.Abs(p_end - p_begin); i++) {
            BuildAWall(p_begin + i, p_y);
        }
    }

    private void BuildLineOfWallsY(int p_x, int p_begin, int p_end) {
        for (int i = 0; i <= Mathf.Abs(p_end - p_begin); i++) {
            BuildAWall(p_x, p_begin + i);
        }
    }

    private void BuildAWall(int p_x, int p_y) {
        if (m_walls[p_x, p_y]) {
            Debug.LogWarning($"there's already a wall here ! {p_x} {p_y}", this);
            return;
        }

        m_walls[p_x, p_y] = true;
        float posX;
        float posY;
        
        IndexesToPositions(p_x, p_y, out posX, out posY);

        Instantiate(m_prefabWall, new Vector3(posX, m_tileValues.m_center.y, posY), m_prefabWall.transform.rotation, m_wallsParent);
    }

    [ContextMenu("Raoul, eat !")]
    public void Raoul() {
        SpawnIngredient(Random.Range(0,m_tileValues.m_numberOfXTiles), Random.Range(0,m_tileValues.m_numberOfYTiles), (IngredientList)Random.Range(0,4));
    }

    private void SpawnIngredient(int p_x, int p_y, Ingredients.IngredientList p_ingredient) {
        float posX;
        float posY;
        IndexesToPositions(p_x, p_y, out posX, out posY);

        Instantiate(m_ingredientsPrefabs[Random.Range(0, m_ingredientsPrefabs.Length)], new Vector3(posX, m_tileValues.m_center.y + m_ingredientYOffset, posY), m_prefabWall.transform.rotation);
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
