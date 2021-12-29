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

    [SerializeField, Tooltip("the rigidbodied prefabs")]
    private GameObject m_eggRbPrefab, m_chocolateRbPrefab, m_FlourRbPrefab, m_strawberryRbPrefab;

    [SerializeField] [Range(0.1f, 1f)] private float m_blinkTiming = 0.2f;
    [SerializeField] [Range(0.2f, 5f)] private float m_totalBlinkTime = 2f;
    private float m_elapsedBlinkingTime = 0f;

    private bool m_isBlinking = false;
    private bool m_isTransparent = false;

    [SerializeField] private SkinnedMeshRenderer m_mr = null;
    
    
    
    private List<Ingredient> m_ingredientsLoaded = new List<Ingredient>();

    private void OnCollisionEnter(Collision p_other) {
        switch (p_other.gameObject.layer) {
            case 7: {
                if(m_isBlinking) break;
                    m_isBlinking = true;
                    StartCoroutine(Blink());

                if (m_ingredientsLoaded.Count < 1) break;
                foreach (Ingredient ingredient in m_ingredientsLoaded) {
                    Destroy(ingredient.go.GetComponent<Rigidbody>());
                    Collider[] colliders = ingredient.go.GetComponents<Collider>();
                    foreach (Collider col in colliders) {
                        Destroy(col);
                    }

                    ingredient.go.AddComponent<BlinkAndDestroy>();
                    
                    GameManager.singleton.SpawnIngredient(ingredient.type);
                }
                m_ingredientsLoaded.Clear();
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider p_other) {
        
        switch (p_other.gameObject.layer) {
            case 6: {
                if (p_other.gameObject.TryGetComponent(out IngredientBehavior script))
                {
                    
                    IngredientList type = script.m_ingredient;

                    GameObject prefab = new GameObject();
                    
                    switch (type)
                    {
                        case IngredientList.Egg:
                        {
                            prefab = m_eggRbPrefab;
                            break;
                        }
                        case IngredientList.Chocolate:
                        {
                            prefab = m_chocolateRbPrefab;
                            break;
                        }
                        case IngredientList.Flour:
                        {
                            prefab = m_FlourRbPrefab;
                            break;
                        }
                        case IngredientList.Strawberry:
                        {
                            prefab = m_strawberryRbPrefab;
                            break;
                        }default: return;
                    }

                    GameObject ingredient = Instantiate(prefab, m_loadingPosition.position, prefab.transform.rotation);
                    
                    m_ingredientsLoaded.Add(new Ingredient(){type = type, go = ingredient});
                    
                    Destroy(p_other.gameObject);
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

    private IEnumerator Blink() {
        yield return new WaitForSeconds(m_blinkTiming);
        m_isTransparent = !m_isTransparent;

        m_elapsedBlinkingTime += m_blinkTiming;
        m_mr.enabled = !m_isTransparent;
        
        if(m_elapsedBlinkingTime < m_totalBlinkTime)StartCoroutine(Blink());
        else {
            m_isTransparent = false;
            m_mr.enabled = !m_isTransparent;
            m_elapsedBlinkingTime = 0f;
            m_isBlinking = false;
        }
    }
}
