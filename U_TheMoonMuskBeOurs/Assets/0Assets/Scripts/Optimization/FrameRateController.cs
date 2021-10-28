#if UNITY_ANDROID

using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    [Header("FPS Control settings")]
    [SerializeField] bool debugFPS = false;
    [SerializeField] int debugStep = 15;

    [Space(5)]
    [Header("Debugging")]
    [SerializeField] GameObject canvas;
    [SerializeField] TMPro.TextMeshProUGUI fpsLabel;


    private void Awake() => SetApplicationTargetFrameRate(60);

    private void Start() => canvas.SetActive(false);


    public void Update()
    {
        if (debugFPS)
        {
            if(!canvas.activeInHierarchy) canvas.SetActive(true);
            if (Time.frameCount % debugStep == 0)
            {
                fpsLabel.text = string.Format("FPS: {0}", 1 / Time.deltaTime);
                //Debug.Log(string.Format("FPS: {0}", 1 / Time.deltaTime));
            }
        }
        else
            if (canvas.activeInHierarchy) canvas.SetActive(false);
    }

    public void SetApplicationTargetFrameRate(int targetFR) => Application.targetFrameRate = targetFR;

}

#endif
