using System;
using System.Collections.Generic;
using System.Linq;
using Ingredients;
using UnityEngine;
using Random = UnityEngine.Random;

public class OvenBehavior : MonoBehaviour {

    [SerializeField] private GameObject m_prefabCake = null;
    [SerializeField] [Range(1,5)] private int m_numberOfCakeToSpawn = 3;
    [SerializeField] private Vector3[] m_potentialSpawnPos = null;

    private bool[] m_ingredientsIn = null;

    private void OnEnable() {
        m_ingredientsIn = new bool[Enum.GetValues(typeof(IngredientList)).Length];
        for (int i = 0; i < m_ingredientsIn.Length; i++) m_ingredientsIn[i] = false;
        
        if(m_numberOfCakeToSpawn > m_potentialSpawnPos.Length) Debug.LogWarning("Not enough spawn positions for the amount of cakes you want to spawn...");
    }

    public void AddIngredient(IngredientList p_ingredient) {
        m_ingredientsIn[(int) p_ingredient] = true;
        CheckForCake();
    }
    
    public void AddIngredients(IngredientList[] p_ingredients) {
        foreach (IngredientList ing in p_ingredients) {
            m_ingredientsIn[(int) ing] = true;
        }
        CheckForCake();
    }

    private void CheckForCake() {
        foreach (bool bo in m_ingredientsIn) {
            if(!bo) return;
        }
        
        BakeCake();
    }

    private void BakeCake() {
        List<Vector3> availblePos = m_potentialSpawnPos.ToList();
        for (int i = 0; i < m_numberOfCakeToSpawn; i++) {
            Instantiate(m_prefabCake, availblePos[Random.Range(0, availblePos.Count)], m_prefabCake.transform.rotation);
        }
    }
}
