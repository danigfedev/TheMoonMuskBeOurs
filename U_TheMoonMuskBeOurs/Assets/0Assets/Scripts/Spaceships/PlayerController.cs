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

    [SerializeField] private Camera mainCam;

    [Tooltip("The spaceship's max vertical rotation in degrees caused by horizontal strafe. " +
            "Positive values rotate the object to the left from camera's PoV. Negative values rotate to the right.")]
    [SerializeField] private float maxVerticalRotation_Left = 30;
    [SerializeField] private float rotationDuration = 1;


    private Vector2 invalidInput;
    private Vector3? worldPos;
    private Coroutine rotationCoroutine = null;

    #region === Monobehaviour Methods ===

    private void Start()
    {
        invalidInput = Vector2.zero - Vector2.one;
    }
       


        void Update()
        {

            InputPointToWorldCoordinates();
            if (worldPos == null) return; //Keep current transform 
            transform.position = (Vector3) worldPos;

#if UNITY_EDITOR || UNITY_STANDALONE
            RotationInPC();
#elif UNITY_ANDROID
    RotationAndroid();
#endif

            
        }

        #endregion

        #region === Private Methods ===

        /// <summary>
        /// Gets input's screen coordinates (pixel untis) and translates it into World coordinates
        /// </summary>
        /// <returns>Input position in World Coordinates</returns>
        private void InputPointToWorldCoordinates()
        {
            // Getting input in screen coordinates

            Vector2 inputCoords = invalidInput; //negative vector
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButton(0))
            {
                inputCoords = Input.mousePosition; //z discarded
                if ((inputCoords.x < 0 || inputCoords.x > Screen.width)
                    || (inputCoords.y < 0 || inputCoords.y > Screen.height))
                    inputCoords = invalidInput;
            }
            
#elif UNITY_ANDROID
            if(Input.touchCount > 0)
            {
                //Input.touches[0]
                Touch userTouch = Input.GetTouch(0);
                inputCoords= new Vector2(userTouch.position.x, userTouch.position.y);
            }
#endif

            //Conversion into Vector3. z value must be positive

            Vector3 touchPos = new Vector3(
                    inputCoords.x,
                    inputCoords.y,
                    -1 * mainCam.transform.position.z);

            //Calculate World position

            if (inputCoords.Equals(invalidInput))
            {
                worldPos = null;
                return;
            }

            worldPos = mainCam.ScreenToWorldPoint(touchPos);
        }

        float horizontalAmount = 0;
        float threshold = 25;
        int stationaryMaxCount = 15;
        int stationaryCount = 0;
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
                        float vertRot = (transform.localEulerAngles.y > 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
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

                        float vertRot = (transform.localEulerAngles.y > 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
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

                        float vertRot = (transform.localEulerAngles.y > 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
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
                    float vertRot = (transform.localEulerAngles.y > 180) ? transform.localEulerAngles.y - 360 : transform.localEulerAngles.y;
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
        Vector3 startRotation = transform.localEulerAngles;

        float t = 0;
        while (t <= 1)
        {
            //Clamp rotation value in the range (-180, 180] degrees
            float vertRot = startRotation.y;
            if (vertRot > 180) vertRot -= 360;

            float newRotation = Mathf.Lerp(vertRot, endAngle, t);
            transform.localEulerAngles = new Vector3(startRotation.x, newRotation, startRotation.z);

            t += Time.deltaTime / rotationDuration;

            yield return null;
        }

        transform.localEulerAngles = new Vector3(startRotation.x, endAngle, startRotation.z);

        rotationCoroutine = null;
    }



        

#endregion
}

