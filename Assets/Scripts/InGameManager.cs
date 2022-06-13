using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class InGameManager : MonoBehaviour
{
    public static InGameManager instance;

    public List<Material> cubeMaterials;
    public GameObject confettiBlast;
    [HideInInspector] public GameObject[] cubes;
    [HideInInspector] public GameObject[] tiles;
    [HideInInspector] public bool gameOver;
    private Color _currentMatColor;
    
    public int targetCubeCode;
    public bool bossLevel;

    private void Awake()
    {
        instance = this;
        cubes = GameObject.FindGameObjectsWithTag("cube");
        tiles = GameObject.FindGameObjectsWithTag("tile");
    }

    private void Start()
    {
        InputEventsManager.instance.LevelCompleteEvent += CheckLevelComplete;
        Vibration.Init();
        Transform cam = FindObjectOfType<Camera>().transform;
        cam.DOMove(new Vector3(cam.position.x - 0.35f, cam.position.y, cam.position.z), 0.2f).From();
    }

    void CheckLevelComplete()
    {
        if(!bossLevel)
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
        else
        {
            List<GameObject> allCubes = BossLevel.instance._gameCubes;
            for (int i = 0; i < allCubes.Count; i++)
            {
                if (allCubes[i].GetComponent<CubeMovement>().code == targetCubeCode)
                {
                    StartCoroutine(LevelWinCondition());
                    BossLevel.instance.startSpawning = false;
                    return;
                }
            }
        }
    }

    IEnumerator LevelWinCondition()
    {
        if(!gameOver)
        {
            TileEffect();
            SoundsManager.instance.PlayClip(SoundsManager.instance.win);
            confettiBlast.SetActive(true);
            gameOver = true;
            yield return new WaitForSeconds(1.5f);
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
    }

    public List<GameObject> GetActiveCubes()
    {
        List<GameObject> _activeCubes = new List<GameObject>();
        for (int i = 0; i < cubes.Length; i++)
        {
            if(cubes[i].activeInHierarchy)
                _activeCubes.Add(cubes[i]);
        }

        return _activeCubes;
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
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
