using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image squareTransitionImg;
    public Image circleTransitionImg;
    public TextMeshProUGUI levelNumText;
    public GameObject bossLevelText;

    private void Start()
    {
        squareTransitionImg.color = GetRandomColor();
        circleTransitionImg.color = GetRandomColor();
        if(InGameManager.instance.bossLevel) bossLevelText.SetActive(true);
        levelNumText.SetText("LEVEL " + (SceneManager.GetActiveScene().buildIndex + 1).ToString());
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
