using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointAt : MonoBehaviour
{

    public Transform target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 lookAt = target.position - transform.position;
        lookAt.Normalize();

        Quaternion targetRot = transform.rotation = Quaternion.LookRotation(lookAt, Vector3.up);
        transform.rotation = AnimMath.Lerp(transform.rotation, targetRot, .01f);
    }
}
