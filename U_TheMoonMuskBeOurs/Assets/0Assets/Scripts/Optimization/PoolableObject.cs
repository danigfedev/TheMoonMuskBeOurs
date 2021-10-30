using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != TagList.gameLimitTag) return;

        StartCoroutine(ResetAndHide());
    }

    private IEnumerator ResetAndHide()
    {
        yield return new WaitForEndOfFrame();
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}
