using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CubeMovement : MonoBehaviour
{
    public bool canMove, isStatic, canMoveRight,canMoveLeft, canMoveForward, canMoveBackward;
    private float _maxDist;
    public Vector3 _moveDir;
    public float speed;
    private Rigidbody _rb;
    private Collider _collider;
    public int code;
    public TextMeshPro codeText;
    public Transform bone;

    public enum CubeState
    {
        IDLE,
        MOVING
    }

    public CubeState cubeState;
    private ParticleSystem _mergeParticle;
    private Animator _cubeEffectAnim;
    private TrailRenderer _trailRenderer;

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
        codeText.text = code.ToString();
        _mergeParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        _cubeEffectAnim = transform.GetChild(1).gameObject.GetComponent<Animator>();
        _trailRenderer = GetComponent<TrailRenderer>();
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
        Debug.DrawRay(transform.position, transform.right * _maxDist, Color.yellow);
        Debug.DrawRay(transform.position, -transform.right * _maxDist, Color.red);
        Debug.DrawRay(transform.position, transform.forward * _maxDist, Color.green);
        Debug.DrawRay(transform.position, -transform.forward * _maxDist, Color.blue);
        
        RaycastHit hit, rightHit, leftHit, forwardHit, backwardHit;
        if (Physics.Raycast(transform.position, transform.right, out rightHit, _maxDist))
        {
            if (rightHit.collider.CompareTag("border"))
            {
                canMoveRight = false;
            }else if (rightHit.collider.CompareTag("cube"))
            {
                
            }
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
        if (canMove && !InGameManager.instance.gameOver)
        {
            //transform.Translate(_moveDir * speed * Time.deltaTime);
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
        /*Debug.Log("Move Dir" + _moveDir);
                Debug.Log("Cubes Dir = " + (transform.position - other.transform.position));#1#*/
        if (other.gameObject.CompareTag("cube"))
        {
            if (code == other.gameObject.GetComponent<CubeMovement>().code)
            {
                //Debug.Log("Direction Mag = " + _moveDir.magnitude);
                if(_moveDir == Vector3.right)
                {
                    //Debug.Log("Same Cube collision");
                    if ((other.transform.position.x - transform.position.x) >
                        (transform.position.x - other.transform.position.x))
                    {
                        other.gameObject.SetActive(false);
                        code += 1;
                        PlayEffects();
                    }
                }
                else if(_moveDir == -Vector3.right)
                {
                    //Debug.Log("Same Cube collision");
                    if ((other.transform.position.x - transform.position.x) <
                        (transform.position.x - other.transform.position.x))
                    {
                        other.gameObject.SetActive(false);
                        code += 1;
                        PlayEffects();
                    }
                }
                else if(_moveDir == Vector3.forward)
                {
                    if ((other.transform.position.z - transform.position.z) >
                        (transform.position.z - other.transform.position.z))
                    {
                        other.gameObject.SetActive(false);
                        code += 1;
                        PlayEffects();
                    }
                }
                else if(_moveDir == -Vector3.forward)
                {
                    //Debug.Log("Same Cube collision");
                    if ((other.transform.position.z - transform.position.z) <
                        (transform.position.z - other.transform.position.z))
                    {
                        other.gameObject.SetActive(false);
                        code += 1;
                        PlayEffects();
                    }
                }
                
                codeText.text = code.ToString();
            }
        }

        if (other.gameObject.CompareTag("adder"))
        {
            code += other.gameObject.GetComponent<Adder>().adderValue;
            codeText.text = code.ToString();
            other.gameObject.SetActive(false);
            PlayEffects();
        }
    }

    void PlayEffects()
    {
        _mergeParticle.Play();
        _cubeEffectAnim.SetTrigger("effect");
        GetComponent<Renderer>().material = InGameManager.instance.GetUpdatedMaterial(code);
        StartCoroutine(CheckLevelComplete());

        Color matColor = GetComponent<Renderer>().material.color;
        _trailRenderer.startColor = matColor;
        _trailRenderer.endColor = matColor;
    }
    IEnumerator CheckLevelComplete()
    {
        yield return new WaitForSeconds(1);
        InputEventsManager.instance.StartLevelCompleteEvent();
    }

    int GetHigherAxis(Vector3 v)
    {
        int axisCode = 0;
        if (Mathf.Abs(v.x) > 0) axisCode = 1;
        if (Mathf.Abs(v.y) > 0) axisCode = 2;
        if (Mathf.Abs(v.z) > 0) axisCode = 3;
        return axisCode;
    }
    void MoveRight()
    {
        if (canMoveRight && cubeState == CubeState.IDLE)
        {
            _moveDir = transform.right;
            canMove = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
        
    }
    void MoveLeft()
    {
        if (canMoveLeft && cubeState == CubeState.IDLE)
        {
            _moveDir = -transform.right;
            canMove = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }
    void MoveForward()
    {
        if(canMoveForward && cubeState == CubeState.IDLE)
        {
            _moveDir = transform.forward;
            canMove = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            //bone.DOLocalMove(new Vector3(0, bone.position.y, 0.004f), 0.8f);
        }
    }
    void MoveBackward()
    {
        if(canMoveBackward && cubeState == CubeState.IDLE)
        {
            _moveDir = -transform.forward;
            canMove = true;
            _rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        }
    }
}
