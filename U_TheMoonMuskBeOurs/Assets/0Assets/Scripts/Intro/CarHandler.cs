using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarHandler : MonoBehaviour
{
    [Header("Child objects")]
    [SerializeField] private Transform carBody;
    [SerializeField] private Transform frontTires;
    [SerializeField] private Transform backTires;
    
    [Space(5)]
    [Header("Car Animation parameters")]
    [SerializeField] private AnimationCurve carMotionpattern;
    [SerializeField] private float carDisplacement = 0.5f;
    [SerializeField] private float animationSpeed = 1;


    private float carStartSpeed = 2.5f;

    private float animationTime = 0;
    private Rigidbody carRB;
    private bool animate = false;

    private void Awake()
    {
        carRB = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        ResetCar();
        StartCar();
    }

    void Update()
    {
        if (!animate) return;
        AnimateCarBody();
        AnimateCarTires();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(TagList.carTriggerTag))
            StopCar(2.18f); //Car's horizontal extents
    }

    private void ResetCar()
    {
        carBody.localPosition = Vector3.zero;
        backTires.rotation = Quaternion.identity;
        frontTires.rotation = Quaternion.identity;
        animate = false;
    }
    /// <summary>
    /// Handles car's body up and down movement
    /// </summary>
    private void AnimateCarBody()
    {
        if (animationTime >= 1)
            animationTime = 0;

        float curveValue = (carMotionpattern.Evaluate(animationTime) - 0.5f) * 2;
        transform.GetChild(0).localPosition = Vector3.up * curveValue * carDisplacement;
        animationTime += Time.deltaTime * animationSpeed;
    }

    /// <summary>
    /// Handles car's tires rotation
    /// </summary>
    private void AnimateCarTires()
    {
        frontTires.transform.Rotate(transform.forward, 1);
        backTires.transform.Rotate(transform.forward, 1);
    }

    private void StartCar()
    {
        animate = true;
        carRB.velocity = transform.right * carStartSpeed;
    }

    private void StopCar(float breakDistance)
    {
        float T = breakDistance / carStartSpeed;
        StartCoroutine(StopCarCoroutine(T));

    }

    private IEnumerator StopCarCoroutine(float totalBreakTime)
    {
        float time = 0;
        while (time <= 1)
        {
            carRB.velocity = transform.right * Mathf.Lerp(carStartSpeed, 0, time);
            time += Time.deltaTime;
            yield return null;
        }


        animate = false;
        carRB.velocity = Vector3.zero;
        ResetCar();
    }
}
