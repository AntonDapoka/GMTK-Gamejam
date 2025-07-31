using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTextShakingScript : MonoBehaviour
{
    [Header("Angle Range (degrees)")]
    public Vector2 angleRange = new Vector2(5f, 20f);

    [Header("Speed Range")]
    public Vector2 speedRange = new Vector2(0.5f, 2f);

    [Header("Swing Curve")]
    public AnimationCurve swingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float angle;
    private float speed;
    private float timeOffset;
    private Quaternion startRotation;

    private void Start()
    {
        startRotation = transform.localRotation;
        angle = Random.Range(angleRange.x, angleRange.y);
        speed = Random.Range(speedRange.x, speedRange.y);

        timeOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        float t = Mathf.Repeat(Time.time * speed + timeOffset, 1f);
        float curveValue = swingCurve.Evaluate(t); 

        float swing = Mathf.Lerp(-1f, 1f, curveValue);
        float z = swing * angle;

        transform.localRotation = startRotation * Quaternion.Euler(0, 0, z);
    }
}