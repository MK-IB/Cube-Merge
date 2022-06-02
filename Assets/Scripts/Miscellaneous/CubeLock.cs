using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CubeLock : MonoBehaviour
{
    public GameObject mergeEffect;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("key"))
        {
            other.gameObject.SetActive(false);
            Instantiate(mergeEffect, transform.position, quaternion.identity).GetComponent<ParticleSystem>().Play();
            gameObject.SetActive(false);
        }
    }
}
