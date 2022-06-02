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
    public GameObject confettiBlast;
    [HideInInspector] public GameObject[] cubes;
 
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InputEventsManager.instance.LevelCompleteEvent += CheckLevelComplete;
        cubes = GameObject.FindGameObjectsWithTag("cube");
    }

    void CheckLevelComplete()
    {
        int activeCubesCounter = 0;
        for (int i = 0; i < cubes.Length; i++)
        {
            if (cubes[i].activeInHierarchy)
                activeCubesCounter++;
        }

        if (activeCubesCounter == 1)
        {
            confettiBlast.SetActive(true);
            StartCoroutine(LoadNextLevel());
        }
        
        else activeCubesCounter = 0;
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public Material GetUpdatedMaterial(int code)
    {
        return cubeMaterials[code - 1];
    }
    private void Update()
    {
        
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
