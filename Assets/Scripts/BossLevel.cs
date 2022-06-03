using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossLevel : MonoBehaviour
{
    public List<GameObject> spawningCubes;
    private float _delay;
    [HideInInspector] public bool startSpawning;
    
    private List<Vector3> _spawnPositions = new List<Vector3>();

    private void Start()
    {
        GameObject[] tiles = InGameManager.instance.tiles;
        for (int i = 0; i < tiles.Length; i++)
        {
            _spawnPositions.Add(tiles[i].transform.position);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) startSpawning = true;
        if(startSpawning)
        {
            _delay += Time.deltaTime;
            if (_delay >= 2f)
            {
                _delay = 0;
                //List<GameObject> activeCubes = InGameManager.instance.GetActiveCubes();
                GameObject newCube = Instantiate(spawningCubes[Random.Range(0, spawningCubes.Count)],
                    _spawnPositions[Random.Range(0, _spawnPositions.Count)], quaternion.identity);
                newCube.transform.DOScale(Vector3.zero, 0.25f).From();
            }
        }
    }
}
