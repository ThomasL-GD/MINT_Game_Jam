using System;
using System.Collections;
using System.Collections.Generic;
using Ingredients;
using UnityEngine;

public class IngredientsPicker : MonoBehaviour {

    [SerializeField] private Transform m_loadingPosition = null;

    private struct Ingredient {
        public IngredientList type;
        public GameObject go;
    }

    private List<Ingredient> m_ingredientsLoaded = new List<Ingredient>();

    private void OnCollisionEnter(Collision p_other) {
        if (p_other.gameObject.layer == 6) {
            if (p_other.gameObject.TryGetComponent(out IngredientBehavior script)) {
                m_ingredientsLoaded.Add(new Ingredient(){type = script.m_ingredient, go = p_other.gameObject});
                Destroy(script);
                p_other.gameObject.transform.position = m_loadingPosition.position;
                p_other.gameObject.AddComponent<Rigidbody>();
            }
        }
    }
}
