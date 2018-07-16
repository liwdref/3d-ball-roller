using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour {

    public float moveSpeed = 5.0f;
    public float drag = 0.5f;
    public float terminalRotationSpeed = 25.0f;

    public VirtualJoystick moveJoystick;

    public float boostSpeed = 5.0f;
    public float boostCooldown = 2.0f;
    private float lastBoost;

    private Rigidbody controller;
    private Transform camTransform;

    private void Start()
    {
        lastBoost = Time.time - boostCooldown;
        controller = GetComponent<Rigidbody>();
        controller.maxAngularVelocity = terminalRotationSpeed;
        controller.drag = drag;

        camTransform = Camera.main.transform;
    }

    private void Update()
    {
       Vector3 dir = Vector3.zero;

        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if (dir.magnitude > 1)
            dir.Normalize();
     

        if(moveJoystick.InputDirection != Vector3.zero)
        {
           dir = moveJoystick.InputDirection;
        }

        //Rotate with camera
        Vector3 rotatedDir = camTransform.TransformDirection(dir);
        rotatedDir = new Vector3(x: rotatedDir.x, y: 0, z: rotatedDir.z);
        rotatedDir = rotatedDir.normalized * dir.magnitude;

        controller.AddForce(rotatedDir * moveSpeed);
    }

    public void Boost()
    {
        if (Time.time - lastBoost > boostCooldown)
        {
            controller.AddForce(controller.velocity.normalized * boostSpeed, ForceMode.VelocityChange);
        }
    }
}
