using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusScript : MonoBehaviour
{
    [SerializeField] private float motorForce;
    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;

    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider;


    private float verticalInput;

    // Update is called once per frame
    void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        UpdateWheels();
    }

    private void GetInput()
    {
        verticalInput = 1f;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheel);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheel);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheel);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheel);
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
            if (other.GetComponent<CheckPoint>().getName().Equals("Point_2")) Destroy(this.gameObject);
        }

    }
}
