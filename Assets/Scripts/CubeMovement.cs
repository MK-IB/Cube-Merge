using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    private void OnEnable()
    {
        InputEventsManager.instance.SwipeRightEvent += MoveRight;
        InputEventsManager.instance.SwipeLeftEvent += MoveLeft;
        InputEventsManager.instance.SwipeUpEvent += MoveUp;
        InputEventsManager.instance.SwipeDownEvent += MoveDown;
    }
    private void OnDisable()
    {
        InputEventsManager.instance.SwipeRightEvent -= MoveRight;
        InputEventsManager.instance.SwipeLeftEvent -= MoveLeft;
        InputEventsManager.instance.SwipeUpEvent -= MoveUp;
        InputEventsManager.instance.SwipeDownEvent -= MoveDown;
    }

    private void Update()
    {
        RaycastHit hit;
        Debug.DrawLine(transform.position, transform.right * 1, Color.yellow);
        if (Physics.Raycast(transform.position, transform.right * 1))
        {
            
        }
    }

    void MoveRight()
    {
        
    }
    void MoveLeft()
    {
        
    }
    void MoveUp()
    {
        
    }
    void MoveDown()
    {
        
    }
}
