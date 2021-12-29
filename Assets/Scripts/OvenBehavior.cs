using System;
using System.Collections;
using System.Collections.Generic;
using Ingredients;
using IngredientWithGo;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class OvenBehavior : MonoBehaviour {

    [SerializeField] private GameObject m_prefabCake = null;
    [SerializeField] [Range(1,5)] private int m_numberOfCakeToSpawn = 3;
    //[SerializeField] [Range(0.5f,3f)] private float m_spawnDistance = 1.5f;
    [SerializeField] [Range(0.1f,3f)] private float m_spawnInterval = 0.5f;
    //[SerializeField] [Range(1f,50f)] private float m_randomMoveInterval = 10f;
    //[SerializeField] private Vector3[] m_potentialSpawnPos = null;
    
    [SerializeField] private Vector3[] m_ingredientLocalPosition = null;

    private NavMeshAgent m_navMeshAgent;
    private int m_cakesSpawned = 0;

    private bool m_isSpawning = false;
    

    private bool[] m_ingredientsValidated = null;
    private List<Ingredient> m_ingredientsIn = new List<Ingredient>();

    private Animator m_animator = null;
    private static readonly int NomNom = Animator.StringToHash("NomNom");

    private void OnEnable() {
        m_ingredientsValidated = new bool[Enum.GetValues(typeof(IngredientList)).Length];
        for (int i = 0; i < m_ingredientsValidated.Length; i++) m_ingredientsValidated[i] = false;
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();

        //if(m_numberOfCakeToSpawn > m_potentialSpawnPos.Length) Debug.LogWarning("Not enough spawn positions for the amount of cakes you want to spawn...");
    }

    private void AddIngredient(Ingredient p_ingredient, bool p_checkForCakeCompletion = false) {
        m_ingredientsValidated[(int) p_ingredient.type] = true;
        m_ingredientsIn.Add(p_ingredient);
        p_ingredient.go.transform.SetParent(transform);
        p_ingredient.go.transform.localPosition = m_ingredientLocalPosition[(int)p_ingredient.type];
        
        m_animator.SetTrigger(NomNom);

        if (p_checkForCakeCompletion) CheckForCake();
    }
    
    public void AddIngredients(Ingredient[] p_ingredients) {
        foreach (Ingredient ing in p_ingredients) {
            AddIngredient(ing);
        }
        if (!CheckForCake()) RunAway(true);
    }

    public void RunAway(bool p_forceChangePath) {
        if(!p_forceChangePath && !(m_navMeshAgent.remainingDistance > m_navMeshAgent.stoppingDistance || m_navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)) return;
        SetRandomPathToGo();
    }

    private bool CheckForCake() {
        foreach (bool bo in m_ingredientsValidated) {
            if(!bo) return false;
        }
        
        GameManager.singleton.ChangeHp(1);
        GameManager.singleton.RegenerateWalls();
        GameManager.singleton.SpawnAllIngredient();
        StartBaking();
        return true;
    }

    private void StartBaking() {
        
        for (int i = 0; i < m_ingredientsValidated.Length; i++) m_ingredientsValidated[i] = false;
        foreach (Ingredient ing in m_ingredientsIn) Destroy(ing.go);
        m_ingredientsIn.Clear();
        
        //List<Vector3> availblePos = m_potentialSpawnPos.ToList();
        // for (int i = 0; i < m_numberOfCakeToSpawn; i++) { Instantiate(m_prefabCake, availblePos[Random.Range(0, availblePos.Count)], m_prefabCake.transform.rotation);}
        m_cakesSpawned = 0;
        m_isSpawning = true;
        StartCoroutine(BakingWhileRunning());
    }

    private IEnumerator BakingWhileRunning() {

        SetRandomPathToGo();

        yield return new WaitForSeconds(m_spawnInterval);

        var transform1 = transform;
        GameObject go = Instantiate(m_prefabCake, transform1.position, transform1.rotation);
        go.GetComponent<NavMeshGoesBrrrrrrrrrr>().m_transformToFollow = GameManager.singleton.m_chefTransform;
        m_cakesSpawned++;
        GameManager.singleton.ChangeScore(1);

        if (m_cakesSpawned < m_numberOfCakeToSpawn) StartCoroutine(BakingWhileRunning());
        else m_isSpawning = false;
    }

    /*private IEnumerator RandomMoveTick() {
        yield return new WaitForSeconds(m_randomMoveInterval);

        StartCoroutine(RandomMoveTick());
    }*/

    private void SetRandomPathToGo() {
        Vector2 firstCorner = GameManager.singleton.wolrdPosOfFirstCorner;
        Vector2 lastCorner = GameManager.singleton.wolrdPosOfLastCorner;
        m_navMeshAgent.destination = new Vector3(Random.Range(firstCorner.x, lastCorner.x), transform.position.y, Random.Range(firstCorner.y, lastCorner.y));
    }

    private int NumberOfIngredientsValidated() {
        int n = 0;
        foreach(bool bo in m_ingredientsValidated) if (bo) n++;
        return n;
    }
}
