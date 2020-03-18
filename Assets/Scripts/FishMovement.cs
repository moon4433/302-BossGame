using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{

    public Transform noseRotation;
    public Transform mid00Rotation;
    public Transform mid01Rotation;
    public Transform mid02Rotation;
    public Transform mid03Rotation;
    public Transform tailRotation;
    
    private float y;
    private bool rotateY;
    private float rotationSpeed;

    Quaternion rotCache;

    void Start()
    {
        
        y = 0.0f;
        rotateY = true;
        rotationSpeed = 75.0f;
    }

    void FixedUpdate()
    {

        if (rotateY == true)
        {
            y += Time.deltaTime * rotationSpeed;

            if (y > 25.0f)
            {
                y = 25.0f;
                rotateY = false;
            }
        }
        else
        {
            y -= Time.deltaTime * rotationSpeed;

            if (y < -25.0f)
            {
                y = -25.0f;
                rotateY = true;
            }
        }

        noseRotation.rotation = Quaternion.Euler(0, -y, 0);
        mid00Rotation.rotation = Quaternion.Euler(0, y, 0);
        mid01Rotation.rotation = Quaternion.Euler(0, -y, 0);
        mid02Rotation.rotation = Quaternion.Euler(0, y, 0);
        mid03Rotation.rotation = Quaternion.Euler(0, -y, 0);
        tailRotation.rotation = Quaternion.Euler(0, y, 0);
    }
}
