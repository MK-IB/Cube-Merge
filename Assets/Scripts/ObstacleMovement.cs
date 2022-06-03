using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public bool canMove, isStatic, canMoveRight, canMoveLeft, canMoveForward, canMoveBackward;
    public Vector3 _moveDir;
    private Rigidbody _rb;


    void Start()
    {
        InputEventsManager.instance.SwipeRightEvent += MoveRight;
        InputEventsManager.instance.SwipeLeftEvent += MoveLeft;
        InputEventsManager.instance.SwipeUpEvent += MoveForward;
        InputEventsManager.instance.SwipeDownEvent += MoveBackward;
        _rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        InputEventsManager.instance.SwipeRightEvent -= MoveRight;
        InputEventsManager.instance.SwipeLeftEvent -= MoveLeft;
        InputEventsManager.instance.SwipeUpEvent -= MoveForward;
        InputEventsManager.instance.SwipeDownEvent -= MoveBackward;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            //transform.Translate(_moveDir * speed * Time.deltaTime);
            _rb.AddForce(_moveDir * 200 * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    void MoveRight()
    {
        _moveDir = transform.right;
        canMove = true;
        _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                          RigidbodyConstraints.FreezeRotation;
    }

    void MoveLeft()
    {
        _moveDir = -transform.right;
        canMove = true;
        _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ |
                          RigidbodyConstraints.FreezeRotation;
    }

    void MoveForward()
    {
        _moveDir = transform.forward;
        canMove = true;
        _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX |
                          RigidbodyConstraints.FreezeRotation;
    }

    void MoveBackward()
    {
        _moveDir = -transform.forward;
        canMove = true;
        _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX |
                          RigidbodyConstraints.FreezeRotation;
    }
}