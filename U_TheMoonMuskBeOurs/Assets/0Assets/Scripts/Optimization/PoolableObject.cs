using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    private SimpleEventSO destroyEventSO; //This should only be assigned if pooled object is an enemy

    public void SetDestroyEventSO(SimpleEventSO soEvent) => destroyEventSO = soEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != TagList.gameLimitTag) return;

        StartCoroutine(ResetAndHide());
    }

    private IEnumerator ResetAndHide()
    {
        yield return new WaitForEndOfFrame();

        if (destroyEventSO != null) destroyEventSO.RaiseEvent();

        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
}
