using UnityEngine;

public class UI_Score : MonoBehaviour
{

    [SerializeField] TMPro.TextMeshProUGUI scoreText;
    [SerializeField] int initialValue = 0;

    void OnEnable()
    {
        UpdateScore(initialValue); //Hardcoded, as there is no control over Unity's event execution order.
        //On GameStart (GameManager), an int SO event and a bunch of standard events are triggered.
    }

    public void UpdateScore(int updatedScore)
    {
        scoreText.text = updatedScore.ToString();
    }
}
