using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartMovement : MonoBehaviour
{
    //TODO: ADD REVERSING
    //[SerializeField] private float maxSpeed = 2000;
    //private float currentSpeed;
    // [SerializeField] private float stirringSpeed = 150;

    // private Rigidbody rb;

    // private void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    // }

    private const string accelerationAxis = "Accelerating";
    private const string steeringAxis = "Steering";
    private float accelerationInput;
    private float steeringInput;
    private bool isBreaking;
    private float currentBreakForce;
    private float currentSteerAngle;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;

    private void FixedUpdate()
    {
        //GetInputs();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInputs()
    {
        accelerationInput = Input.GetAxis(accelerationAxis);
        steeringInput = Input.GetAxis(steeringAxis);
        isBreaking = Input.GetKey(KeyCode.LeftShift);
    }

    //for the AI
    public void SetInputs(float accelerating, float steering, float breaking)
    {
        accelerationInput = accelerating;
        steeringInput = steering;
        if (breaking > 0)
        {
            isBreaking = true;
        }
        else
        {
            isBreaking = false;
        }
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = accelerationInput * motorForce;
        frontRightWheelCollider.motorTorque = accelerationInput * motorForce;

        currentBreakForce = isBreaking ? breakForce : 0f;
        if (isBreaking)
        {
            ApplyBreaking();
        }
    }
    // private void HandleMotor2()
    // {
    //     float speed = frontLeftWheelCollider.rpm * frontLeftWheelCollider.radius * Mathf.PI * 0.12f;
    //     if (speed < maxSpeed)
    //     {
    //         frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
    //         frontRightWheelCollider.motorTorque = verticalInput * motorForce;
    //     }
    //     else
    //     {
    //         frontLeftWheelCollider.motorTorque = 0;
    //         frontRightWheelCollider.motorTorque = 0;
    //     }

    //     currentBreakForce = isBreaking ? breakForce : 0f;
    //     if (isBreaking)
    //     {
    //         ApplyBreaking();
    //     }
    // }

    private void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        backLeftWheelCollider.brakeTorque = currentBreakForce;
        backRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteeringAngle * steeringInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(backLeftWheelCollider, backLeftWheelTransform);
        UpdateSingleWheel(backRightWheelCollider, backRightWheelTransform);
    }
    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }

    // public void Accelerate(float direction)
    // {
    //     Vector3 force = ConvertToCameraSpace(new Vector3(direction, 0, 0), true);
    //     rb.AddForce(force * maxSpeed * Time.deltaTime);
    // }

    // public void Stir(float direction)
    // {
    //     Vector3 force = ConvertToCameraSpace(new Vector3(0, direction, 0), false);
    //     gameObject.transform.Rotate(force * stirringSpeed * Time.deltaTime);
    // }

    // private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate, bool eliminateY)
    // {
    //     // get the forward and right vectors of the camera
    //     Vector3 cameraForward = Camera.main.transform.forward;
    //     Vector3 cameraRight = Camera.main.transform.right;

    //     // set the y values to 0 to ignore up/down camera angles
    //     cameraForward.y = 0;
    //     cameraRight.y = 0;

    //     // normalize both vectors so they each have a magnitude of 1
    //     cameraForward = cameraForward.normalized;
    //     cameraRight = cameraRight.normalized;

    //     // rotate the x and z VectorToRotate values to camera space
    //     Vector3 cameraForwardXProduct = vectorToRotate.x * cameraForward;
    //     Vector3 cameraRightZProduct = vectorToRotate.z * cameraRight;

    //     // vector3 in camera space = sum of both products
    //     Vector3 vectorRotatedToCameraSpace = cameraForwardXProduct + cameraRightZProduct;

    //     // add the original y value back in if required
    //     if (!eliminateY)
    //         vectorRotatedToCameraSpace.y = vectorToRotate.y;

    //     return vectorRotatedToCameraSpace;
    // }
}
