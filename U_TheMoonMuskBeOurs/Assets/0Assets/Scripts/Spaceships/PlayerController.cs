using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private Camera mainCam;
        private Vector2 invalidInput;
        private Vector3? worldPos;

        #region === Monobehaviour Methods ===

        void Start()
        {
            invalidInput = Vector2.zero - Vector2.one;
        }

        void Update()
        {
            InputPointToWorldCoordinates();
            if (worldPos == null) return; //Keep current transform 
            transform.position = (Vector3) worldPos;
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
            inputCoords = Input.mousePosition; //z discarded
            if ((inputCoords.x < 0 || inputCoords.x > Screen.width)
                || (inputCoords.y < 0 || inputCoords.y > Screen.height))
                inputCoords = invalidInput;
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
        
        #endregion
    }
}

