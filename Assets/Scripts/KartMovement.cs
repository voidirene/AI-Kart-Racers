using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 2000;
    private float currentSpeed;
    [SerializeField] private float stirringSpeed = 150;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            Accelerate(Input.GetAxis("Vertical"));
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            Stir(Input.GetAxis("Horizontal"));
        }
    }

    public void Accelerate(float direction)
    {
        Vector3 force = ConvertToCameraSpace(new Vector3(direction, 0, 0), true);
        rb.AddForce(force * maxSpeed * Time.deltaTime);
    }

    public void Stir(float direction)
    {
        Vector3 force = ConvertToCameraSpace(new Vector3(0, direction, 0), false);
        gameObject.transform.Rotate(force * stirringSpeed * Time.deltaTime);
    }

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate, bool eliminateY)
    {
        // get the forward and right vectors of the camera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // set the y values to 0 to ignore up/down camera angles
        cameraForward.y = 0;
        cameraRight.y = 0;

        // normalize both vectors so they each have a magnitude of 1
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        // rotate the x and z VectorToRotate values to camera space
        Vector3 cameraForwardXProduct = vectorToRotate.x * cameraForward;
        Vector3 cameraRightZProduct = vectorToRotate.z * cameraRight;

        // vector3 in camera space = sum of both products
        Vector3 vectorRotatedToCameraSpace = cameraForwardXProduct + cameraRightZProduct;

        // add the original y value back in if required
        if (!eliminateY)
            vectorRotatedToCameraSpace.y = vectorToRotate.y;

        return vectorRotatedToCameraSpace;
    }
}
