using System;
using UnityEditor;
using UnityEngine;
using Ingredients;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        ShowArrayProperty(serializedObject.FindProperty("m_ingredientsPrefabs"));
    }

    public void ShowArrayProperty(SerializedProperty list) {
        
        for (int i = 0; i < list.arraySize; i++) {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent (Enum.GetName(typeof(IngredientList), i)), true); 
        }
        
        serializedObject.ApplyModifiedProperties();
    }
    
}
