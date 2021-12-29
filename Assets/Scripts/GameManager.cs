using System;
using System.Collections.Generic;
using Ingredients;
using TMPro;
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

    [SerializeField] public Transform m_chefTransform = null;
    
    [SerializeField] private GameObject m_prefabWall = null;

    [SerializeField] [Range(0.1f, 2f)] public float m_wallRiseTime = 0.5f;
    
    [SerializeField] private SOTileValues m_tileValues = null;

    [SerializeField] private GameObject[] m_ingredientsPrefabs;

    [SerializeField, Range(0f, 5f)] private float m_ingredientYOffset = 1f;

    [SerializeField] private WallsToBuild[] m_firstWallsToBuild;

    [SerializeField, Range(1,10)] private int m_startHealth = 3;

    [SerializeField] private GameObject[] m_visualHealthPoints;

    [SerializeField] private TextMeshProUGUI m_scoreMesh = null;
    private int m_score = 0;
    
    [HideInInspector] private int m_currentHealth = 3;
    

    private bool[,] m_walls = null;
    private Transform m_wallsParent = null;

    public Vector2 wolrdPosOfFirstCorner;
    public Vector2 wolrdPosOfLastCorner;

    public delegate void DestroyDelegator();
    public static DestroyDelegator DestroyEveryWall;


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

        m_currentHealth = m_startHealth;
        
        if(m_visualHealthPoints.Length < m_startHealth)Debug.LogError("There's not enough visual health points !");
        else {
            //We set the correct amount of health point in case there are too many
            for (int i = m_visualHealthPoints.Length - 1; i >= m_startHealth ; i--) {
                m_visualHealthPoints[i].SetActive(false);
            }
        }
    }

    private void Start() {
        
        foreach (WallsToBuild walls in m_firstWallsToBuild) {
            if(walls.isHorizontal) BuildLineOfWallsX(walls.otherAxeCoordinate, walls.begin, walls.end);
            else BuildLineOfWallsY(walls.otherAxeCoordinate, walls.begin, walls.end);
        }

        wolrdPosOfFirstCorner = new Vector2(m_tileValues.m_center.x - ((m_tileValues.m_numberOfXTiles * m_tileValues.m_sizeOfATile) / 2f), (m_tileValues.m_center.z - ((m_tileValues.m_numberOfYTiles * m_tileValues.m_sizeOfATile) / 2f)));
        wolrdPosOfLastCorner = new Vector2(wolrdPosOfFirstCorner.x + 2*(m_tileValues.m_center.x - wolrdPosOfFirstCorner.x), wolrdPosOfFirstCorner.y + 2*(m_tileValues.m_center.z - wolrdPosOfFirstCorner.y));

        SpawnAllIngredient();
    }

    private void OnValidate() {
        if (m_ingredientsPrefabs.Length != Enum.GetNames(typeof(IngredientList)).Length) {
            Debug.LogWarning("Don't change the length of this array, there's nothing more or less to show !");
            m_ingredientsPrefabs = new GameObject[Enum.GetNames(typeof(IngredientList)).Length];
        }
    }

    public void RegenerateWalls() {
        DestroyAllWalls();

        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 3; j++) {
                bool rDir = Random.Range(0, 2) == 1;
                if (rDir) BuildLineOfWallsX(1 + (j * 4) + Random.Range(0, 3), 1+ (i*6), 4+ (i*6));
                else BuildLineOfWallsY(1+ (i*6) + Random.Range(0, 4), 1 + (j*4), 3 + (j*4));
            }
        }
    }

    private void BuildLineOfWallsX(int p_y, int p_begin, int p_end) {
        Debug.Log($"X wall : {p_y}       {p_begin}  {p_end}");
        for (int i = 0; i <= Mathf.Abs(p_end - p_begin); i++) {
            BuildAWall(p_begin + i, p_y);
        }
    }

    private void BuildLineOfWallsY(int p_x, int p_begin, int p_end) {
        Debug.Log($"Y wall : {p_x}       {p_begin}  {p_end}");
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

        GameObject go = Instantiate(m_prefabWall, new Vector3(posX, m_tileValues.m_center.y, posY), m_prefabWall.transform.rotation, m_wallsParent);
        go.AddComponent<WallBehavior>();
    }

    private void DestroyAllWalls() {
        DestroyEveryWall?.Invoke();
        for (int i = 0; i < m_walls.GetLength(0); i++) {
            for (int j = 0; j < m_walls.GetLength(0); j++) {
                m_walls[i, j] = false;
            }
        }
    }
    
    public void SpawnAllIngredient() {
        SpawnIngredient(IngredientList.Egg);
        SpawnIngredient(IngredientList.Strawberry);
        SpawnIngredient(IngredientList.Flour);
        SpawnIngredient(IngredientList.Chocolate);
    }
    
    public void SpawnIngredient() {
        Vector2Int rand = GetRandomAvailableTile();
        SpawnIngredient(rand.x, rand.y);
    }
    
    public void SpawnIngredient(IngredientList p_ingredient) {
        Vector2Int rand = GetRandomAvailableTile();
        SpawnIngredient(rand.x, rand.y, p_ingredient);
    }

    private Vector2Int GetRandomAvailableTile() {
        List<Vector2Int> availablePos = new List<Vector2Int>();
        for (int i = 0; i < m_walls.GetLength(0); i++) {
            for (int j = 0; j < m_walls.GetLength(1); j++) {
                if(!m_walls[i,j])availablePos.Add(new Vector2Int(i,j));
            }
        }
        return availablePos[Random.Range(0, availablePos.Count)];
    }
    
    public void SpawnIngredient(int p_x, int p_y) {
        Array possibleIng = Enum.GetValues(typeof(IngredientList));
        SpawnIngredient(p_x, p_y, (IngredientList) possibleIng.GetValue(Random.Range(0, possibleIng.Length)));
    }

    private void SpawnIngredient(int p_x, int p_y, Ingredients.IngredientList p_ingredient) {
        float posX;
        float posY;
        IndexesToPositions(p_x, p_y, out posX, out posY);

        Instantiate(m_ingredientsPrefabs[(int)p_ingredient], new Vector3(posX, m_tileValues.m_center.y + m_ingredientYOffset, posY), m_prefabWall.transform.rotation);
    }

    private void IndexesToPositions(int p_x, int p_y, out float x, out float y) {
        x = (m_tileValues.m_center.x - ((m_tileValues.m_numberOfXTiles * m_tileValues.m_sizeOfATile) / 2f))  +  p_x * m_tileValues.m_sizeOfATile  +  (m_tileValues.m_sizeOfATile/2f);
        y = (m_tileValues.m_center.z - ((m_tileValues.m_numberOfYTiles * m_tileValues.m_sizeOfATile) / 2f))  +  p_y * m_tileValues.m_sizeOfATile  +  (m_tileValues.m_sizeOfATile/2f);
    }

    public void SetTileValues(int p_numberOfXTiles, int p_numberOfYTiles, float p_sizeOfATile, Vector3 p_center) {
        m_tileValues.m_numberOfXTiles = p_numberOfXTiles;
        m_tileValues.m_numberOfYTiles = p_numberOfYTiles;
        m_tileValues.m_sizeOfATile = p_sizeOfATile;
        m_tileValues.m_center = p_center;
    }

    public void LoseHp() {
        m_currentHealth --;

        m_visualHealthPoints[m_currentHealth].SetActive(false);
    }

    public void ChangeScore(int p_scoreToAdd) {
        m_score += p_scoreToAdd;
        m_scoreMesh.text = "x " + m_score.ToString();
    }

    
    private void OnDrawGizmos() {
        if (m_firstWallsToBuild.Length < 1) return;
    }
}