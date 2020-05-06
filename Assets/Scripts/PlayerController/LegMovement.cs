using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegMovement : MonoBehaviour
{

    public Transform leftFoot;
    public Transform rightFoot;

    public bool isAnimating = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AnimateThoseLegs();
    }

    void AnimateThoseLegs()
    {

        float pitch = 0;
        float secondPitch = 0;

        if (isAnimating)
        {
            pitch = Mathf.Sin(Time.time * 5) * .2f;
            secondPitch = Mathf.Cos(Time.time * 5) * .2f;
        }

        leftFoot.position = new Vector3(leftFoot.position.x, -secondPitch + .25f, pitch);
        rightFoot.position = new Vector3(rightFoot.position.x, secondPitch + .25f, -pitch);
        
    }
}
