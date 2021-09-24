using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class TruckController : MonoBehaviour
{
    [SerializeField] private float motorForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private Transform lookAtPos;

    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;
    [SerializeField] private Transform rearLeftWheel2, rearRightWheel2;
    [SerializeField] private Transform rearLeftWheel3, rearRightWheel3;


    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider2, rearRightWheelCollider2;
    [SerializeField] private WheelCollider rearLeftWheelCollider3, rearRightWheelCollider3;

    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking, isRestarted;
    private int motionCounter = 0;

    Vector3 startPosition;
    Quaternion startRotation;
    Rigidbody rb;

    private void Start()
    {
        PlayerPrefs.DeleteAll();
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        Restart();
    }

    private void Restart()
    {
        if (isRestarted)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
            rb.velocity = Vector3.zero;
        }
    }

    private void GetInput()
    {
        verticalInput = 0.7f;
        horizontalInput = 0.025f;
        motionCounter++;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider2.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider2.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider3.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider3.motorTorque = verticalInput * motorForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheel);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheel);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheel);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheel);
        UpdateSingleWheel(rearLeftWheelCollider2, rearLeftWheel2);
        UpdateSingleWheel(rearRightWheelCollider2, rearRightWheel2);
        UpdateSingleWheel(rearLeftWheelCollider3, rearLeftWheel3);
        UpdateSingleWheel(rearRightWheelCollider3, rearRightWheel3);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Checkpoint"))
        {
            if (other.GetComponent<CheckPoint>().getName().Equals("killer")) Destroy(this.gameObject);
        }

    }
}
