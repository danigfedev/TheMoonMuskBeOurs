using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum RotationDirections
    {
        LEFT = 0,
        MIDDLE,
        RIGHT
    }

    [Header("== TESTING ==")]
    [SerializeField] bool enableControlOnAwake = false;
    [Space(10)]

    [Header("== Spaceship Motion ==")]
    [Tooltip("The spaceship's max vertical rotation in degrees caused by horizontal strafe. " +
            "Positive values rotate the object to the left from camera's PoV. Negative values rotate to the right.")]
    [SerializeField] float maxVerticalRotation_Left = 30;
    [SerializeField] float rotationDuration = 1;
    
    [Space(10)]
    [Header("== Spaceship structure ==")]
    [SerializeField] Transform spaceshipBody;
    [SerializeField] GameObject fireObject;
    

    [Space(10)]
    [Header("== SO Events ==")]
    //SimpleEventSO dieEventSO;

    private Camera mainCam;
    private Rigidbody spaceshipRB;

    private bool controlEnabled = false;

    private float horizontalAmount = 0;
    private float threshold = 25;
    private int stationaryMaxCount = 15;
    private int stationaryCount = 0;
    private Coroutine rotationCoroutine = null;

    #region === MonoBehaviour Methods ===

    private void Awake()
    {
        mainCam = Camera.main;
        spaceshipRB = gameObject.GetComponent<Rigidbody>();
        fireObject.SetActive(false);
        EnableControl(enableControlOnAwake);
    }

    private void FixedUpdate()
    {
        if (!controlEnabled) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        MovePC();
        RotationInPC();
#elif UNITY_ANDROID
        MoveAndroid();
        RotationAndroid();
#endif
    }

    #endregion



    #region === Player Movement ===

    private void MovePC()
    {
        Move(GetMovementInputPC());
    }

    private void MoveAndroid()
    {
        Move(GetMovementInputAndroid());
    }


    private void Move(Vector3 newPosition)
    {
        spaceshipRB.MovePosition(newPosition);
    }

    private Vector3 GetMovementInputPC()
    {
        if (Input.GetMouseButton(0)) //MLB pressed
        {
            Vector3 mousePos = Input.mousePosition;

            //Correct z:
            //mousePos = new Vector3(
            //        mousePos.x,
            //        mousePos.y,
            //        -1 * mainCam.transform.position.z);

            mousePos = mainCam.ScreenToWorldPoint(mousePos);
            return new Vector3(mousePos.x, mousePos.y, transform.position.z);
        }

        return transform.position;
    }

    private Vector3 GetMovementInputAndroid()
    {
        if (Input.touches.Length > 0)
        {
            Touch mainTouch = Input.GetTouch(0);

            if (mainTouch.phase == TouchPhase.Began || mainTouch.phase == TouchPhase.Moved)
            {
                Vector3 inputPos = mainTouch.position; //Vector2 -> Vector3 ==> z = 0
                inputPos = mainCam.ScreenToWorldPoint(inputPos);
                return new Vector3(inputPos.x, inputPos.y, transform.position.z);
            }
        }

        return transform.position;
    }

    #endregion


    #region === Player Rotation ===


#if UNITY_EDITOR || UNITY_STANDALONE
        Vector2 currentInput;
        Vector2 prevInput;

        private void RotationInPC()
        {
            if (Input.GetMouseButton(0))
            {
                currentInput = Input.mousePosition;

                if (Input.GetMouseButtonDown(0))
                {
                    //Click
                    prevInput = currentInput;
                }



                //DO Stuff
                if (currentInput == prevInput)
                {
                    stationaryCount++;
                    if (stationaryCount % stationaryMaxCount == 0)
                    {
                        //Stationary
                        stationaryCount = 0;

                        horizontalAmount = 0;
                        //float vertRot = (transform.localEulerAngles.y > 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
                        float vertRot = GetVerticalRotation();
                        if (vertRot != 0 && rotationCoroutine == null)
                            RotateVertically(RotationDirections.MIDDLE);
                    }
                }
                else
                {
                    //Movement
                    horizontalAmount += (currentInput.x - prevInput.x);

                    if (rotationCoroutine != null)
                    {
                        prevInput = currentInput;
                        return;
                    }

                    if (Mathf.Abs(horizontalAmount) > threshold)
                    {
                        int direction = 0;
                        if (horizontalAmount > 0) direction = -1; //right: -30
                        else if (horizontalAmount < 0) direction = 1; //left 30

                        //float vertRot = (transform.localEulerAngles.y > 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
                        float vertRot = GetVerticalRotation();
                    //Debug.Log(vertRot);
                    if (direction == 1 && vertRot != maxVerticalRotation_Left)
                            RotateVertically(RotationDirections.LEFT);
                        else if (direction == -1 && vertRot != -maxVerticalRotation_Left)
                            RotateVertically(RotationDirections.RIGHT);
                    }
                }

                prevInput = currentInput;
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    //Release
                    horizontalAmount = 0;
                    currentInput = Vector2.zero;
                    prevInput = Vector2.zero;

                    RotateVertically(RotationDirections.MIDDLE);
                }
            }
        }

#elif UNITY_ANDROID
        private void RotationAndroid()
        {
            if (Input.touches.Length > 0)
            {
                Touch mainTouch = Input.GetTouch(0);

                if(mainTouch.phase == TouchPhase.Moved)
                {
                    horizontalAmount += mainTouch.deltaPosition.x;


                    if (rotationCoroutine != null) return;

                    if (Mathf.Abs(horizontalAmount) > threshold)
                    {
                        int direction = 0;
                        if (horizontalAmount > 0) direction = -1; //right: -30
                        else if (horizontalAmount < 0) direction = 1; //left 30

                        //float vertRot = (transform.localEulerAngles.y > 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
                        float vertRot = GetVerticalRotation();
                        //Debug.Log(vertRot);
                        if (direction == 1 && vertRot != maxVerticalRotation_Left)
                            RotateVertically(RotationDirections.LEFT);
                        else if (direction == -1 && vertRot != -maxVerticalRotation_Left)
                            RotateVertically(RotationDirections.RIGHT);
                    }
                }
                else if(mainTouch.phase == TouchPhase.Stationary)
                {
                    horizontalAmount = 0;
                    //float vertRot = (transform.localEulerAngles.y > 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
                    float vertRot = GetVerticalRotation();
                    if (vertRot != 0 && rotationCoroutine == null)
                        RotateVertically(RotationDirections.MIDDLE);
                }
                else if(mainTouch.phase == TouchPhase.Ended || mainTouch.phase == TouchPhase.Canceled)
                {
                    horizontalAmount = 0;
                    RotateVertically(RotationDirections.MIDDLE);
                }
            }
        }
#endif

    private float GetVerticalRotation()
    {
        //return (transform.localEulerAngles.y > 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
        return (spaceshipBody.localEulerAngles.y > 180) ? spaceshipBody.localEulerAngles.y - 360 : spaceshipBody.localEulerAngles.y;
    }

    /// <summary>
    /// Handles spaceship's vertical rotation based on user input
    /// </summary>
    /// <param name="rotDirection">Player's input direction</param>
    private void RotateVertically(RotationDirections rotDirection)
    {
        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        //Right: -30 deg
        //Middle: 0 deg
        //Left: 30 deg
        if (rotDirection == RotationDirections.LEFT)
            rotationCoroutine = StartCoroutine(RotateToReachAngle(maxVerticalRotation_Left));
        else if (rotDirection == RotationDirections.MIDDLE)
            rotationCoroutine = StartCoroutine(RotateToReachAngle(0));
        else if (rotDirection == RotationDirections.RIGHT)
            rotationCoroutine = StartCoroutine(RotateToReachAngle(-1 * maxVerticalRotation_Left));

    }

    /// <summary>
    /// Handles spaceship vertical rotation
    /// </summary>
    /// <param name="endAngle">The destination angle</param>
    /// <returns></returns>
    private IEnumerator RotateToReachAngle(float endAngle)
    {
        //Get current vertical rotation
        //Vector3 startRotation = transform.localEulerAngles;
        Vector3 startRotation = spaceshipBody.localEulerAngles;

        float t = 0;
        while (t <= 1)
        {
            //Clamp rotation value in the range (-180, 180] degrees
            float vertRot = startRotation.y;
            if (vertRot > 180) vertRot -= 360;

            float newRotation = Mathf.Lerp(vertRot, endAngle, t);
            //transform.localEulerAngles = new Vector3(startRotation.x, newRotation, startRotation.z);
            spaceshipBody.localEulerAngles = new Vector3(startRotation.x, newRotation, startRotation.z);

            t += Time.deltaTime / rotationDuration;

            yield return null;
        }

        //transform.localEulerAngles = new Vector3(startRotation.x, endAngle, startRotation.z);
        spaceshipBody.localEulerAngles = new Vector3(startRotation.x, endAngle, startRotation.z);

        rotationCoroutine = null;
    }
    
    #endregion

    #region === Public Methods ===

    public void EnableControl(bool status) => controlEnabled = status;

    public void StartEngines() => fireObject.SetActive(true);

    #endregion
}

