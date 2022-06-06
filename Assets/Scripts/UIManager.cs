using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image squareTransitionImg;
    public Image circleTransitionImg;
    public GameObject bossLevelText;

    private void Start()
    {
        squareTransitionImg.color = GetRandomColor();
        circleTransitionImg.color = GetRandomColor();
        if(InGameManager.instance.bossLevel) bossLevelText.SetActive(true);
    }

    private Color32 GetRandomColor()
    {
        return new Color32(
            (byte)UnityEngine.Random.Range(0, 255),
            (byte)UnityEngine.Random.Range(0, 255),
            (byte)UnityEngine.Random.Range(0, 255),
            255
            );
    }
}
