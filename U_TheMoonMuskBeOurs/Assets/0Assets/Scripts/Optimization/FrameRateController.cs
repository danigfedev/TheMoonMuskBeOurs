#if UNITY_ANDROID

using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    [SerializeField] bool debugFPS = false;
    [SerializeField] int debugStep = 15;

    private void Awake() => SetApplicationTargetFrameRate(60);

    public void Update()
    {
        if (debugFPS)
        {
            if (Time.frameCount % debugStep == 0)
            {
                Debug.Log(string.Format("FPS: {0}", 1 / Time.deltaTime));
            }
        }
    }

    public void SetApplicationTargetFrameRate(int targetFR) => Application.targetFrameRate = targetFR;

}

#endif
