using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossLevel : MonoBehaviour
{
    public static BossLevel instance;
    
    public List<GameObject> spawningCubes;
    private float _delay;
    [HideInInspector] public bool startSpawning;
    private List<GameObject> _tiles = new List<GameObject>();
    public List<GameObject> _gameCubes;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       _tiles = InGameManager.instance.tiles.ToList();
       _gameCubes = InGameManager.instance.GetActiveCubes();
    }

    private List<Vector3> _spawnPositions;
    void CheckPosToSpawn()
    {
         _spawnPositions = new List<Vector3>();
         for (int i = 0; i < _gameCubes.Count; i++)
         {
             if(!_gameCubes[i].activeInHierarchy)
                 _gameCubes.RemoveAt(i);
         }
        for (int i = 0; i < _tiles.Count; i++)
        {
            for (int j = 0; j < _gameCubes.Count; j++)
            {
                if((_tiles[i].transform.position - _gameCubes[j].transform.position).magnitude >= 0.1f)
                    _spawnPositions.Add(_tiles[i].transform.position);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) startSpawning = true;
        if(startSpawning && !InGameManager.instance.gameOver)
        {
            _delay += Time.deltaTime;
            if (_delay >= 2f)
            {
                _delay = 0;
                CheckPosToSpawn();
                Vector3 spawnPos = _spawnPositions[Random.Range(0, _spawnPositions.Count)];
                spawnPos = new Vector3(spawnPos.x, 0, spawnPos.z);
                GameObject newCube = Instantiate(spawningCubes[Random.Range(0, spawningCubes.Count)],spawnPos, quaternion.identity);
                _gameCubes.Add(newCube);
            }
        }
    }
}
