using UnityEngine;

public class MovementTest : MonoBehaviour
{
    public bool canMove, isStatic, canMoveRight, canMoveLeft, canMoveForward, canMoveBackward;
    private float _maxDist;
    private Vector3 _moveDir;
    public float speed;
    private Rigidbody _rb;
    private Collider _collider;

    public int code;
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
        _maxDist = 10;
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

    private Vector3 _maxMovePos;
    private void Update()
    {
        if(cubeState1 == CubeState1.IDLE)
        {
            Debug.DrawRay(transform.position, transform.right * _maxDist, Color.yellow);
            Debug.DrawRay(transform.position, -transform.right * _maxDist, Color.red);
            Debug.DrawRay(transform.position, transform.forward * _maxDist, Color.green);
            Debug.DrawRay(transform.position, -transform.forward * _maxDist, Color.blue);
        }

        RaycastHit hit, rightHit, leftHit, forwardHit, backwardHit;

        #region RIGHT_RAY
        if (Physics.Raycast(transform.position, transform.right, out rightHit))
        {
            GameObject hitObject = rightHit.collider.gameObject;
            if (hitObject.CompareTag("cube") && cubeState1 == CubeState1.IDLE)
            {
                if (hitObject.transform.GetComponent<MovementTest>().code != code)
                {
                    if (Mathf.Abs((hitObject.transform.position.x - 1) - transform.position.x) <= 0)
                        canMoveRight = false;
                    else
                    {
                        _maxMovePos = new Vector3(hitObject.transform.position.x - 1, 0, transform.position.z);
                        canMoveRight = true;
                    }
                }
                else 
                {
                    hitObject.layer = 2;  
                }
            }
            if (hitObject.CompareTag("border") && cubeState1 == CubeState1.IDLE)
            {
                float xDist = Mathf.Abs((hitObject.transform.position.x - 1) - transform.position.x);
                if (xDist <= 0)
                {
                    canMoveRight = false;
                    //Debug.Log("can't move");
                }
                else
                {
                    //Debug.Log("Can Move");
                    _maxMovePos = new Vector3(hitObject.transform.position.x - 1,0, transform.position.z);
                    canMoveRight = true;
                }    
            }
        }
        #endregion
        #region LEFT_RAY
        if (Physics.Raycast(transform.position, -transform.right, out leftHit))
        {
            GameObject hitObject = leftHit.collider.gameObject;
            if (hitObject.CompareTag("cube") && cubeState1 == CubeState1.IDLE)
            {
                if (hitObject.transform.GetComponent<MovementTest>().code != code)
                {
                    if (Mathf.Abs((hitObject.transform.position.x + 1) - transform.position.x) <= 0)
                        canMoveLeft = false;
                    else
                    {
                        _maxMovePos = new Vector3(hitObject.transform.position.x + 1, 0, transform.position.z);
                        canMoveLeft = true;
                    }
                         
                }
                else 
                {
                    hitObject.layer = 2;  
                }
            }
            if (hitObject.CompareTag("border") && cubeState1 == CubeState1.IDLE)
            {
                float xDist = Mathf.Abs((hitObject.transform.position.x + 1) - transform.position.x);
                //Debug.Log("x dist = " + xDist + " of " + transform.name);
                if (xDist <= 0)
                {
                    canMoveLeft = false;
                    //Debug.Log("can't move");
                }
                else
                {
                    //Debug.Log("Can Move");
                    _maxMovePos = new Vector3(hitObject.transform.position.x + 1,0, transform.position.z);
                    canMoveLeft = true;
                }    
            }
        }
        #endregion 
        if (canMove)
        {
            transform.Translate(_moveDir * speed * Time.deltaTime);
            cubeState1 = CubeState1.MOVING;
            if(Vector3.Distance(transform.position, _maxMovePos) <= 0.1f )
                StopMovement();
        }
        
    }

    void StopMovement()
    {
        canMove = false;
        transform.position = _maxMovePos;
        cubeState1 = CubeState1.IDLE;
    }
    private void FixedUpdate()
    {
        /*if (canMove)
        {
            //_rb.AddForce(_moveDir * speed * Time.fixedDeltaTime, ForceMode.Impulse);
            cubeState1 = CubeState1.MOVING;
        }*/
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cube") && cubeState1 == CubeState1.MOVING)
        {
            if(other.GetComponent<MovementTest>().code == code)
                other.gameObject.SetActive(false);
        }
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
        if(canMoveRight)
        {
            _moveDir = transform.right;
            canMove = true;
        }
    }

    void MoveLeft()
    {
        if(canMoveLeft)
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