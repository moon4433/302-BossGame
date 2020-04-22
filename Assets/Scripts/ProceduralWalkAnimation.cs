using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWalkAnimation : MonoBehaviour
{
    public float sinWaveOffset = 0;
    public float sinWaveSpeed = 1;

    // how far apart should the feet be?
    public float scaleX = 1;
    // how high the foot comes off the ground
    public float distanceY = 1;
    // how far forward / backward to move the foot
    public float distanceZ = 1;

    Vector3 startingPosition;
    PlayerController player;

    void Start()
    {
        startingPosition = transform.localPosition;
        player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {

        float time = (Time.time + sinWaveOffset * Mathf.PI) * sinWaveSpeed;

        float offsetZ = Mathf.Sin(time);
        float offsetY = Mathf.Cos(time);
        if (offsetY < 0) offsetY = 0;

        Vector3 finalPosition = startingPosition;

        finalPosition.x *= scaleX;
        finalPosition.y += offsetY * distanceY; // move final position up / down
        //finalPosition.z += offsetZ * distanceZ; // move final position forward / backward

        // get direction player is trying to move,
        // convert to local coordinates:
        //Vector3 walkDir = transform.InverseTransformDirection(player.walkDir);
        Vector3 walkDir = transform.InverseTransformDirection(player.walkDir);
        // move foot forward / backward
        finalPosition += walkDir * offsetZ * distanceZ;

        float p = walkDir.magnitude;

        transform.localPosition = Vector3.Lerp(startingPosition, finalPosition, p);


    }
}
