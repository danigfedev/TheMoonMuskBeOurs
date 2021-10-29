using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Shield : MonoBehaviour
{
    [SerializeField] float shieldDuration;
    [SerializeField] float shiedlFadeTime;
    [SerializeField] [Range(0,1)]float fadePctg = 0.25f;
    [SerializeField] Material shieldMaterial; 

    private Transform playerShip;
    private Rigidbody shieldRB;
    private float shieldVisibleTime;

    public Transform PlayerShip
    {
        set { playerShip = value; }
    }

    private void Awake()
    {
        shieldRB = GetComponent<Rigidbody>();
        shieldVisibleTime = shieldDuration - shiedlFadeTime;
    }


    private void OnEnable()
    {
        StartCoroutine(DisableShield());
    }

    private void FixedUpdate()
    {
        if (playerShip != null)
            shieldRB.MovePosition(playerShip.position);
    }

    private IEnumerator DisableShield()
    {
        yield return new WaitForSeconds(shieldVisibleTime);

        float initialValue = shieldMaterial.GetFloat("Shield_Intensity");
        float t = 0;
        while (t < 1)
        {
            float newValue = Mathf.Lerp(initialValue, 0, t);
            shieldMaterial.SetFloat("Shield_Intensity", newValue);
            t += Time.deltaTime/ shiedlFadeTime;
            
            yield return null;
        }
        shieldMaterial.SetFloat("Shield_Intensity", initialValue);
        gameObject.SetActive(false);
    }
}
