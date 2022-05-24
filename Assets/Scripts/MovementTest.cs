using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTest : MonoBehaviour
{
    public bool canMove, isStatic, canMoveRight, canMoveLeft, canMoveForward, canMoveBackward;
    private float _maxDist;
    private Vector3 _moveDir;
    public float speed;
    private Rigidbody _rb;
    private Collider _collider;

    public enum CubeState1
    {
        IDLE,
        MOVING
    }

    public CubeState1 cubeState1;

    private void Start()
    {
        InputEventsManager.instance.SwipeRightEvent += MoveRight;
        InputEventsManager.instance.SwipeLeftEvent += MoveLeft;
        InputEventsManager.instance.SwipeUpEvent += MoveForward;
        InputEventsManager.instance.SwipeDownEvent += MoveBackward;
        _maxDist = transform.localScale.x / 2 + 0.01f;
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        cubeState1 = CubeState1.IDLE;
    }

    private void OnDisable()
    {
        InputEventsManager.instance.SwipeRightEvent -= MoveRight;
        InputEventsManager.instance.SwipeLeftEvent -= MoveLeft;
        InputEventsManager.instance.SwipeUpEvent -= MoveForward;
        InputEventsManager.instance.SwipeDownEvent -= MoveBackward;
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.right * _maxDist, Color.yellow);
        Debug.DrawLine(transform.position, -transform.right * _maxDist, Color.red);
        Debug.DrawLine(transform.position, transform.forward * _maxDist, Color.green);
        Debug.DrawLine(transform.position, -transform.forward * _maxDist, Color.blue);

        RaycastHit hit, rightHit, leftHit, forwardHit, backwardHit;
        if (Physics.Raycast(transform.position, transform.right, out rightHit, _maxDist))
        {
            if (rightHit.collider.CompareTag("border") && _moveDir == transform.right)
            {
                canMoveRight = false;
                canMove = false;
            }
            else canMoveRight = true;
        }
        else canMoveRight = true;

        if (Physics.Raycast(transform.position, -transform.right, out leftHit, _maxDist))
        {
            if (leftHit.collider.CompareTag("border") && _moveDir == -transform.right)
            {
                canMoveLeft = false;
                canMove = false;
            }
            else canMoveLeft = true;
        }
        else canMoveLeft = true;

        if (Physics.Raycast(transform.position, transform.forward, out forwardHit, _maxDist))
        {
            if (forwardHit.collider.CompareTag("border") && _moveDir == transform.forward)
            {
                canMoveForward = false;
                canMove = false;
            }
            else canMoveForward = true;
        }
        else canMoveForward = true;

        if (Physics.Raycast(transform.position, -transform.forward, out backwardHit, _maxDist))
        {
            if (backwardHit.collider.CompareTag("border") && _moveDir == -transform.forward)
            {
                canMoveBackward = false;
                canMove = false;
            }
            else canMoveBackward = true;
        }
        else canMoveBackward = true;
    }

    void ChangeToIdleState(Transform hitObj)
    {
        if (hitObj.CompareTag("border") /* && cubeState1 == CubeState1.MOVING*/)
        {
            cubeState1 = CubeState1.IDLE;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            //   transform.Translate(_moveDir * speed * Time.deltaTime);
            _rb.AddForce(_moveDir * speed * Time.fixedDeltaTime, ForceMode.Impulse);
            cubeState1 = CubeState1.MOVING;
        }

        if (_rb.velocity.magnitude <= 2.5f)
        {
            isStatic = true;
            cubeState1 = CubeState1.IDLE;
        }
        else
        {
            isStatic = false;
            cubeState1 = CubeState1.MOVING;
        }
    }
    


    void MoveRight()
    {
        _moveDir = transform.right;
        canMove = true;
    }

    void MoveLeft()
    {
        _moveDir = -transform.right;
        canMove = true;
    }

    void MoveForward()
    {
        _moveDir = transform.forward;
        canMove = true;
    }

    void MoveBackward()
    {
        _moveDir = -transform.forward;
        canMove = true;
    }
}