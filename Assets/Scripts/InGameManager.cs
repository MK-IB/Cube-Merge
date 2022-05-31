using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    public List<Material> cubeMaterials;

    private void Awake()
    {
        instance = this;
    }

    public Material GetUpdatedMaterial(int code)
    {
        return cubeMaterials[code - 1];
    }
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
