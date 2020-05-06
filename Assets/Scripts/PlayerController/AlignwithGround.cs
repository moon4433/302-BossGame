using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignwithGround : MonoBehaviour
{

    void Update()
    {
        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        if(Physics.SphereCast(ray, .1f, out RaycastHit hit, 1.5f))
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.FromToRotation(transform.up,hit.normal) * transform.rotation,
                90 * Time.deltaTime
                );
        }
    }
}
