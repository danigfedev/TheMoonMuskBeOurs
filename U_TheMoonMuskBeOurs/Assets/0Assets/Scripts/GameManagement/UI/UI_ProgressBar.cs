using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ProgressBar : MonoBehaviour
{


    [SerializeField] Transform player;
    [SerializeField] Transform[] stages;
    //[SerializeField] Transform stage0Pos;
    //[SerializeField] Transform stage1Pos;
    //[SerializeField] Transform stage2Pos;
    //[SerializeField] Transform stage3Pos;
    //[SerializeField] Transform endPos;

    private int currentPos = 0;

    void OnEnable()
    {
        player.parent = stages[currentPos];
        player.localPosition = Vector3.zero;
    }

    public void MoveToNextStage()
    {
        currentPos++;
        player.parent = stages[currentPos];
        player.localPosition = Vector3.zero;
    }
}
