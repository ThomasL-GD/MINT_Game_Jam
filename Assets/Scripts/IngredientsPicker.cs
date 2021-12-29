using System;
using System.Collections;
using System.Collections.Generic;
using Ingredients;
using UnityEngine;
using IngredientWithGo;
using UnityEngine.Serialization;

namespace IngredientWithGo {
    public struct Ingredient {
        public IngredientList type;
        public GameObject go;
    }
}

public class IngredientsPicker : MonoBehaviour {

    
    [SerializeField] private AudioSource m_ouchSound;
    [SerializeField] private AudioSource m_pickupSound;
    [SerializeField] private AudioSource m_putDownSound;
    [SerializeField] private Transform m_loadingPosition = null;

    [SerializeField, Tooltip("the rigidbodied prefabs")]
    private GameObject m_eggRbPrefab, m_chocolateRbPrefab;

    [FormerlySerializedAs("m_FlourRbPrefab")] [SerializeField, Tooltip("the rigidbodied prefabs")]
    private GameObject m_flourRbPrefab;

    [SerializeField, Tooltip("the rigidbodied prefabs")]
    private GameObject m_strawberryRbPrefab;

    [SerializeField, Tooltip("the particle effects")]
    private ParticleSystem m_eggParticle, m_chocolateParticle;

    [FormerlySerializedAs("m_FlourParticle")] [SerializeField, Tooltip("the particle effects")]
    private ParticleSystem m_flourParticle;

    [SerializeField, Tooltip("the particle effects")]
    private ParticleSystem m_strawberryParticle;

    [SerializeField] [Range(0.1f, 1f)] private float m_blinkTiming = 0.2f;
    [SerializeField] [Range(0.2f, 5f)] private float m_totalBlinkTime = 2f;
    private float m_elapsedBlinkingTime = 0f;

    private bool m_isBlinking = false;
    private bool m_isTransparent = false;

    [SerializeField] private SkinnedMeshRenderer m_mr = null;
    
    
    
    private List<Ingredient> m_ingredientsLoaded = new List<Ingredient>();

    private void Start() {
        GameManager.OnEndScene += ResetPhysics;
    }

    private void OnCollisionEnter(Collision p_other) {
        switch (p_other.gameObject.layer) {
            case 7: {
                    if(m_isBlinking) break;
                    m_isBlinking = true;
                    StartCoroutine(Blink());
                    m_ouchSound.Play();

                if (m_ingredientsLoaded.Count < 1) {
                    GameManager.singleton.ChangeHp(-1);
                    break;
                }
                else {
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
                }
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
                    ParticleSystem particleSystem = new ParticleSystem();
                    switch (type)
                    {
                        case IngredientList.Egg:
                        {
                            prefab = m_eggRbPrefab;
                            particleSystem = m_eggParticle;
                            break;
                        }
                        case IngredientList.Chocolate:
                        {
                            prefab = m_chocolateRbPrefab;
                            particleSystem = m_chocolateParticle;
                            break;
                        }
                        case IngredientList.Flour:
                        {
                            prefab = m_flourRbPrefab;
                            particleSystem = m_flourParticle;
                            break;
                        }
                        case IngredientList.Strawberry:
                        {
                            prefab = m_strawberryRbPrefab;
                            particleSystem = m_strawberryParticle;
                            break;
                        }default: return;
                    }

                    m_pickupSound.Play();
                    
                    GameObject ingredient = Instantiate(prefab, m_loadingPosition.position, prefab.transform.rotation);
                    Instantiate(particleSystem, p_other.transform.position, particleSystem.transform.rotation);
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
                    
                    m_putDownSound.Play();
                }
                break;
            }
        }
    }

    private IEnumerator Blink() {

        Physics.IgnoreLayerCollision(gameObject.layer, 7);
        
        yield return new WaitForSeconds(m_blinkTiming);
        m_isTransparent = !m_isTransparent;

        m_elapsedBlinkingTime += m_blinkTiming;
        m_mr.enabled = !m_isTransparent;
        
        if(m_elapsedBlinkingTime < m_totalBlinkTime)StartCoroutine(Blink());
        else {

            Physics.IgnoreLayerCollision(gameObject.layer, 7, false);
            m_isTransparent = false;
            m_mr.enabled = !m_isTransparent;
            m_elapsedBlinkingTime = 0f;
            m_isBlinking = false;
        }
    }

    private void ResetPhysics() {
        GameManager.OnEndScene -= ResetPhysics;
        Physics.IgnoreLayerCollision(gameObject.layer, 7, false);
    }
}
