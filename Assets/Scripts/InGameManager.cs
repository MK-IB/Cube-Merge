using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    public List<Material> cubeMaterials;
    public GameObject confettiBlast;
    [HideInInspector] public GameObject[] cubes;
    [HideInInspector] public GameObject[] tiles;
    private Color _currentMatColor;
 
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InputEventsManager.instance.LevelCompleteEvent += CheckLevelComplete;
        cubes = GameObject.FindGameObjectsWithTag("cube");
        tiles = GameObject.FindGameObjectsWithTag("tile");
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
            for (int i = 0; i < cubes.Length; i++)
            {
                if (cubes[i].activeInHierarchy)
                {
                    _currentMatColor = cubes[i].GetComponent<Renderer>().material.color;
                }
            }
            StartCoroutine(LevelWinCondition());
        }
        
        else activeCubesCounter = 0;
    }

    IEnumerator LevelWinCondition()
    {
        TileEffect();
        confettiBlast.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void TileEffect()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            SpriteRenderer spriteRenderer = tiles[i].GetComponent<SpriteRenderer>();
            spriteRenderer.DOColor(_currentMatColor, 0.5f).OnComplete(() =>
            {
                spriteRenderer.DOColor(new Color32(65, 65, 65, 255), 0.5f);
            });
        }
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
