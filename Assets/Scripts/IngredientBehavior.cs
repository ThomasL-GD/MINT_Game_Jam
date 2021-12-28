using System;
using Ingredients;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IngredientBehavior : MonoBehaviour {

    public IngredientList m_ingredient;

    private void OnEnable() {
        if (gameObject.layer != 6) {
            Debug.LogWarning("Setting back this object to the layer 6", this);
            gameObject.layer = 6;
        }
    }
}
