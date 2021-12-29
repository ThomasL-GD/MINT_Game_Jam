using System;
using System.Collections;
using System.Collections.Generic;
using Ingredients;
using UnityEngine;
using IngredientWithGo;

namespace IngredientWithGo {
    public struct Ingredient {
        public IngredientList type;
        public GameObject go;
    }
}

public class IngredientsPicker : MonoBehaviour {

    [SerializeField] private Transform m_loadingPosition = null;

    private List<Ingredient> m_ingredientsLoaded = new List<Ingredient>();

    private void OnCollisionEnter(Collision p_other) {
        switch (p_other.gameObject.layer) {
            case 6: {
                if (p_other.gameObject.TryGetComponent(out IngredientBehavior script)) {
                    m_ingredientsLoaded.Add(new Ingredient(){type = script.m_ingredient, go = p_other.gameObject});
                    Destroy(script);
                    p_other.gameObject.transform.position = m_loadingPosition.position;
                    p_other.gameObject.AddComponent<Rigidbody>();
                }
                break;
            }
            
            case 7: {
                if(m_ingredientsLoaded.Count < 1) break;
                foreach (Ingredient ingredient in m_ingredientsLoaded) {
                    Destroy(ingredient.go.GetComponent<Rigidbody>());
                    Collider[] colliders = ingredient.go.GetComponents<Collider>();
                    foreach (Collider col in colliders) {
                        Destroy(col);
                    }

                    m_ingredientsLoaded.Remove(ingredient);
                    ingredient.go.AddComponent<BlinkAndDestroy>();
                }
                break;
            }
            
            case 8: {
                if (p_other.gameObject.TryGetComponent(out OvenBehavior script)) {
                    Ingredient[] ingredientsToGive = new Ingredient[m_ingredientsLoaded.Count];
                    for (int i = 0; i < m_ingredientsLoaded.Count; i++) {
                        ingredientsToGive[i] = m_ingredientsLoaded[i];
                        Destroy(m_ingredientsLoaded[i].go.GetComponent<Rigidbody>());
                        Collider[] colliders = m_ingredientsLoaded[i].go.GetComponents<Collider>();
                        foreach (Collider col in colliders) Destroy(col);
                    }
                    script.AddIngredients(ingredientsToGive);
                    m_ingredientsLoaded.Clear();
                    
                    
                }
                break;
            }
        }
    }
}
