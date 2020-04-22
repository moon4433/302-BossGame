using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootRaycast : MonoBehaviour
{

    public Transform ground;
    public Vector3 offset;
    
    void Start()
    {
        
    }

   
    void Update()
    {
        FindGround();   
    }

    void FindGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);

        Debug.DrawRay(ray.origin, ray.direction);

        if(Physics.SphereCast(ray, .1f, out RaycastHit hit, 1.5f))
        {
            // ray hit something
            transform.position = hit.point + offset;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        else
        {
            // ray doesn't hit something
            if(ground != null)
            {
                transform.position = new Vector3(transform.position.x, ground.position.y, transform.position.z) + offset;
                transform.localRotation = Quaternion.identity;
            }
        }
    }
}
