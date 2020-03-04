using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCurve : MonoBehaviour
{

    public BezierCurve curve;
    [Range(0, 1)] public float percent = 0;

    public bool shouldAnimate = true;

    public AnimationCurve speed;
    public float animationLength = 5;

    float timeCurrent = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (shouldAnimate)
        {
            timeCurrent += Time.deltaTime;
            percent = timeCurrent / animationLength;
            percent = Mathf.Clamp(percent, 0, 1);
        }

        SetPositionToCurve();
    }

    private void SetPositionToCurve()
    {
        if (curve)
        {
            float p = speed.Evaluate(percent);
            transform.position = curve.FindPositionAt(p);
        }
    }

    void OnValidate()
    {
        SetPositionToCurve();
    }
}
