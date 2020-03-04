using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCam : MonoBehaviour
{

    public float yaw { get; private set; }
    public float pitch = 0;

    public float lookSensitivityX = 1;
    public float lookSensitivityY = 1;

    void Start()
    {
        
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X"); // how many pixels the mouse has moved left/right
        float mouseY = Input.GetAxis("Mouse Y"); // how many pixels the mouse has moved up/down1

        yaw += mouseX * lookSensitivityX;
        pitch += mouseY * lookSensitivityY;

        pitch = Mathf.Clamp(pitch, 0, 89);

        Quaternion targetRot = Quaternion.Euler(pitch, yaw, 0);

        transform.rotation = AnimMath.Dampen(transform.rotation, targetRot, .001f);

    }
}
