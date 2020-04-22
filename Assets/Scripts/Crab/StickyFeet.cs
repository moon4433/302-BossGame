using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyFeet : MonoBehaviour
{

    public Transform homeLocation;
    public float moveDistanceThreshold = 0.5f;

    public StickyFeet[] otherFeetToCheckFirst;

    [Header("Foot Animation:")]
    public AnimationCurve footLateralEase;
    public AnimationCurve footVerticalRaise;
    public float timeToMoveFoot = 0.25f;

    float footMoveTimer = 0;
    Vector3 plantedPositionLast;
    Vector3 plantedPositionNext;


    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckIfCanMoveFoot();
        AnimateFoot();
    }

    void AnimateFoot()
    {

        if (!IsAnimating())
        { // if not animating
            transform.position = plantedPositionNext; // keep foot planted
            return;
        }

        footMoveTimer += Time.deltaTime;

        float p = footMoveTimer / timeToMoveFoot;
        p = Mathf.Clamp(p, 0, 1);

        Vector3 pos = Vector3.Lerp(plantedPositionLast, plantedPositionNext, footLateralEase.Evaluate(p));
        pos.y += footVerticalRaise.Evaluate(p);

        transform.position = pos;

    }

    void CheckIfCanMoveFoot()
    {

        if (IsAnimating()) return;

        foreach(StickyFeet foot in otherFeetToCheckFirst)
        {
            if (foot.IsAnimating()) return;
        }

        float d2 = (transform.position - homeLocation.position).sqrMagnitude;

        if(d2 > moveDistanceThreshold * moveDistanceThreshold)
        {
            DoRayCast();
        }
        
    }

    void DoRayCast()
    {
        Ray ray = new Ray(homeLocation.position + Vector3.up * 2.5f, Vector3.down);

        if(Physics.Raycast(ray, out RaycastHit hit, 5))
        {
            footMoveTimer = 0;
            plantedPositionLast = transform.position;
            plantedPositionNext = hit.point;
        }
    }

    bool IsAnimating()
    {
        return (footMoveTimer < timeToMoveFoot);
    }
}
