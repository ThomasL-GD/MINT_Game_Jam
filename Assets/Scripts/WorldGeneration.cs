using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using static UnityEditor.EditorGUILayout;

public class WorldGeneration : EditorWindow {
    
    [MenuItem("Blue's messing with Satan/My WorldGeneration")]
    static void Initialization() {
        // Get existing open window or if none, make a new one:
        WorldGeneration window = (WorldGeneration)EditorWindow.GetWindow(typeof(WorldGeneration));
        window.Show();
    }
    

    /*[Header("Dimensions")]/**/
    /*[SerializeField] [Range(1, 100)] /**/private int m_numberOfXTiles = 20;
    /*[SerializeField] [Range(1, 100)] /**/private int m_numberOfYTiles = 20;
    
    /*[SerializeField] [Range(0.1f, 10f)] /**/private float m_sizeOfATile = 0.5f;
    /*[SerializeField] /**/private Vector3 m_center = Vector3.zero;

    /*[Header(" ")] [Header("Tiles")]/**/
    /*[SerializeField] /**/private GameObject m_prefabWhiteTile = null;
    /*[SerializeField] /**/private GameObject m_prefabBlackTile = null;

    private Transform m_parentToObjects = null;
    private int m_spaceWidth = 5;
    private GameManager m_gameManager = null;


    private bool m_alternateBAndW = false;

    private void OnGUI() {
        m_spaceWidth = IntSlider("Space width", m_spaceWidth, 0, 100);
        
        Space(m_spaceWidth);
        
        m_numberOfXTiles = IntSlider("Number of X tiles", m_numberOfXTiles, 1, 100);
        m_numberOfYTiles = IntSlider("Number of Y tiles", m_numberOfYTiles, 1, 100);
        
        Space(m_spaceWidth);

        m_sizeOfATile = Slider("The size of a Tile", m_sizeOfATile, 0.1f, 10f);
        m_center = Vector3Field("Center of the grid", m_center);
        
        Space(m_spaceWidth);

        m_prefabWhiteTile = (GameObject)ObjectField("White tile Prefab", m_prefabWhiteTile, typeof(GameObject), false);
        m_prefabBlackTile = (GameObject)ObjectField("Black tile Prefab", m_prefabBlackTile, typeof(GameObject), false);
        
        Space(m_spaceWidth);
        
        m_gameManager = (GameManager)ObjectField("The game manager", m_gameManager, typeof(GameManager), true);
        
        Space(m_spaceWidth);
        
        m_parentToObjects = (Transform)ObjectField("Parent of every instance", m_parentToObjects, typeof(Transform), true);
        
        Space(m_spaceWidth);
        
        if (GUILayout.Button("Generate")) {
            if(m_parentToObjects != null) {
                foreach (Transform child in m_parentToObjects) {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.delayCall += () => {
                        DestroyImmediate( child.gameObject );
                    }; 
#endif
                }
            }
            
            CreateGrid();
                
            if (m_gameManager != null)  m_gameManager.SetTileValues(m_numberOfXTiles, m_numberOfYTiles, m_sizeOfATile, m_center); // We give all the values to the GameManager if there's one serialized
        }
        
    }

    private void CreateGrid() {
        
        for(int i = 0; i < m_numberOfYTiles; i++) {
            for(int j = 0; j < m_numberOfXTiles; j++) {
                CreateTile(j,i);
            }
            if(m_numberOfXTiles%2 == 0)m_alternateBAndW = !m_alternateBAndW;
        }
        
    }

    private void CreateTile(int p_x, int p_y) {
        m_alternateBAndW = !m_alternateBAndW;
        GameObject prefabToSpawn;
        switch (m_alternateBAndW) {
            case true :
                prefabToSpawn = m_prefabWhiteTile;
                break;
            
            case false :
                prefabToSpawn = m_prefabBlackTile;
                break;
        }
        
        float x = (m_center.x - ((m_numberOfXTiles * m_sizeOfATile) / 2f))  +  p_x * m_sizeOfATile  +  m_sizeOfATile/2f;
        float z = (m_center.z - ((m_numberOfYTiles * m_sizeOfATile) / 2f))  +  p_y * m_sizeOfATile  +  m_sizeOfATile/2f;
        
        
        //Get path to nearest (in case of nested) prefab from this gameObject in the scene
        string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(prefabToSpawn);
        //Get prefab object from path
        Object prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(Object));
        //Instantiate the prefab in the scene, as a sibling of current gameObject
        Object prefabSpawned = PrefabUtility.InstantiatePrefab(prefab, m_parentToObjects);

        if (prefabSpawned is GameObject) {
            Transform newTransform = ((GameObject) prefabSpawned).transform;
            newTransform.position = new Vector3(x, m_center.y, z);
        }

    }
}
