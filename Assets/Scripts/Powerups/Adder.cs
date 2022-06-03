using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Adder : MonoBehaviour
{
    public TextMeshPro codeText;
    public int adderValue;

    private void Start()
    {
        codeText.text = "+" + adderValue.ToString();
    }
}
