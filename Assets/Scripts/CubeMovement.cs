using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CubeMovement : MonoBehaviour
{
    public bool canMove, isStatic, canMoveRight,canMoveLeft, canMoveForward, canMoveBackward;
    private float _maxDist;
    private Vector3 _moveDir;
    public float speed;
    private Rigidbody _rb;
    private Collider _collider;

    public enum CubeState
    {
        IDLE,
        MOVING
    }

    public CubeState cubeState;

    private void Start()
    {
        InputEventsManager.instance.SwipeRightEvent += MoveRight;
        InputEventsManager.instance.SwipeLeftEvent += MoveLeft;
        InputEventsManager.instance.SwipeUpEvent += MoveForward;
        InputEventsManager.instance.SwipeDownEvent += MoveBackward;
        _maxDist = transform.localScale.x / 2 + 0.01f;
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        cubeState = CubeState.IDLE;
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
            if (rightHit.collider.CompareTag("border"))
            {
                canMoveRight = false;    
            }else canMoveRight = true;
            
        }else canMoveRight = true;
        if (Physics.Raycast(transform.position, -transform.right, out leftHit, _maxDist))
        {
            if (leftHit.collider.CompareTag("border"))
            {
                canMoveLeft = false;    
            }else canMoveLeft = true;
        }else canMoveLeft = true;
        
        if (Physics.Raycast(transform.position, transform.forward, out forwardHit, _maxDist))
        {
            if (forwardHit.collider.CompareTag("border"))
            {
                canMoveForward = false;    
            }else canMoveForward = true;
        }else canMoveForward = true;

        if (Physics.Raycast(transform.position, -transform.forward, out backwardHit, _maxDist))
        {
            if (backwardHit.collider.CompareTag("border"))
            {
                canMoveBackward = false;    
            }else canMoveBackward = true; 
        }else canMoveBackward = true; 
    }

    void ChangeToIdleState(Transform hitObj)
    {
        if (hitObj.CompareTag("border")/* && cubeState1 == CubeState1.MOVING*/)
        {
            cubeState = CubeState.IDLE;
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            //   transform.Translate(_moveDir * speed * Time.deltaTime);
            _rb.AddForce(_moveDir * speed * Time.fixedDeltaTime, ForceMode.Impulse);
            cubeState = CubeState.MOVING;
        }
        if (_rb.velocity.magnitude <= 2.5f)
        {
            isStatic = true;
            cubeState = CubeState.IDLE;
        }
        else
        {
            isStatic = false;
            cubeState = CubeState.MOVING;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("cube"))
        {
            CubeMovement cubeMovement = other.transform.GetComponent<CubeMovement>();
            if (cubeMovement.cubeState == CubeState.IDLE && cubeState == CubeState.MOVING)
            {
                other.gameObject.SetActive(false);
            }else if (cubeMovement.cubeState == CubeState.IDLE && cubeState == CubeState.IDLE)
            {
                Debug.Log("Idle cube = " + other.gameObject.name);
                if (_moveDir.normalized == (transform.position - other.transform.position).normalized)
                {
                    gameObject.SetActive(false);
                    
                }
            }
        }
    }

    void MoveRight()
    {
        if (canMoveRight)
        {
            _moveDir = transform.right;
            canMove = true;    
        }
        
    }
    void MoveLeft()
    {
        if (canMoveLeft)
        {
            _moveDir = -transform.right;
            canMove = true;    
        }
    }
    void MoveForward()
    {
        if(canMoveForward)
        {
            _moveDir = transform.forward;
            canMove = true;
        }
    }
    void MoveBackward()
    {
        if(canMoveBackward)
        {
            _moveDir = -transform.forward;
            canMove = true;
        }
    }
}
