using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTex : MonoBehaviour
{
    Material mat;
    public float scrollSpeed;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        mat.mainTextureOffset -= new Vector2(scrollSpeed * Time.deltaTime, 0f);
    }

}
