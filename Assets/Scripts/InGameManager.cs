using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;
    
    Dictionary<int, string> cubeProperty = new Dictionary<int, string>()
    {
        {1, "red"},
        {2, "blue"},
        {3, "yellow"},
        {4, "green"},
        {5, "violet"},
    };

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
