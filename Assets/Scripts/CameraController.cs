using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float yHeight = 0;
    [SerializeField] private float smoothTime;

    void Update()
    {
        Vector3 newPosition = new Vector3(0, 0, -10);
        newPosition.x = Mathf.Lerp(transform.position.x, playerTransform.position.x, smoothTime);
        newPosition.y = Mathf.Lerp(transform.position.y, yHeight, smoothTime);
        transform.position = newPosition;
    }
}
