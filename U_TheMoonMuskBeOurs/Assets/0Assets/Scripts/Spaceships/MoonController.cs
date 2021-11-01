using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonController : EnemyController_Base
{
    [SerializeField] SimpleEventSO victoryEventSO;
    [SerializeField] float waitForVictory = 1;
    private Vector3 linearStartPosition;
    private Vector3 linearEndPosition;
    private bool enableMovement = true;

    public override void Start()
    {
        Move();
    }

    public void FixedUpdate()
    {
        Move();
    }

    public void SetMotionParameters(Vector3 start, Vector3 end)
    {
        linearStartPosition = start;
        linearEndPosition = end;
    }


    float t = 0;
    public override void Move()
    {

        //Linear movement:
        if (t <= 1.1f)
        {
            Vector3 newPosition = Vector3.Lerp(linearStartPosition, linearEndPosition, t);
            enemyRigidbody.MovePosition(newPosition);

            t += Time.fixedDeltaTime * speed;
            return;
        }

        StartCoroutine(WaitForVictory());

    }

    private IEnumerator WaitForVictory()
    {
        yield return new WaitForSeconds(waitForVictory);
        victoryEventSO.RaiseEvent();
    }
}
