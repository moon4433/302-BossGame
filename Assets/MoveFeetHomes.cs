using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFeetHomes : MonoBehaviour
{

    public Transform feetHomes;
    public Transform body;

    public bool isMovingForward;
    public bool isMovingBackward;
    public Vector3 LastPOS;
    public Vector3 NextPOS;

    void Start()
    {
        body = GetComponent<Transform>();
    }

    void LateUpdate()
    {


        NextPOS.x = body.position.x;



        if (LastPOS.x < NextPOS.x)
        {
            isMovingForward = true;
            isMovingBackward = false;
            Debug.Log("forward");
        }
        if (LastPOS.x > NextPOS.x)
        {
            isMovingBackward = true;
            isMovingForward = false;
            Debug.Log("backward");
        }
        else if (LastPOS.x == NextPOS.x)
        {
            isMovingForward = false;
            isMovingBackward = false;
            Debug.Log("still");
        }

        LastPOS.x = NextPOS.x;
    }
}
