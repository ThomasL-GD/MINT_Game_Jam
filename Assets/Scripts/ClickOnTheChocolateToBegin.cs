using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickOnTheChocolateToBegin : MonoBehaviour
{
    [SerializeField] private int m_firstScene;
    private void OnMouseDown()
    {
        SceneManager.LoadScene(m_firstScene);
    }
}
