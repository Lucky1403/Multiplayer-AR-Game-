using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick joystick;
    public float speed = 2f;
    private Vector3 velocityVector = Vector3.zero; //-> Indicating Initial Velocity of the Spinner
    public float maxVelocityChange = 4f;
    private Rigidbody rb;
    public float tiltAmount = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float _xMovementInput = joystick.Horizontal;
        float _zMovementInput = joystick.Vertical;
        Vector3 _movementHorizontal = transform.right * _xMovementInput;
        Vector3 _movementVertical = transform.forward * _zMovementInput;

        Vector3 _movementVelocityVector = (_movementHorizontal + _movementVertical).normalized * speed;

        Move(_movementVelocityVector);

        transform.rotation = Quaternion.Euler(joystick.Vertical * speed * tiltAmount, 0, -1 * joystick.Horizontal * speed * tiltAmount);
    }

    void Move(Vector3 movementVelocityVector)
    {
        velocityVector = movementVelocityVector;
    }

    void FixedUpdate()
    {
        if (velocityVector != Vector3.zero)
        {
            Vector3 velocity = rb.velocity;
            Vector3 changeInVelocity = (velocityVector - velocity);

            changeInVelocity.x = Mathf.Clamp(changeInVelocity.x, -maxVelocityChange, maxVelocityChange);
            changeInVelocity.z = Mathf.Clamp(changeInVelocity.z, -maxVelocityChange, maxVelocityChange);
            changeInVelocity.y = 0f;


            rb.AddForce(changeInVelocity, ForceMode.Acceleration);
        }
    }
}
