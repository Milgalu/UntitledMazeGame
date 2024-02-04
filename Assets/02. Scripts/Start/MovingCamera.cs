using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    public float moveSpeed = 5f; // 카메라 이동 속도
    public float moveDistance = 2f; // 카메라 이동 거리

    private Vector3 initialPosition;

    void Start()
    {
        InitializeCameraMovement();
    }

    void Update()
    {
        MoveCameraUpDown();
    }

    void InitializeCameraMovement()
    {
        initialPosition = transform.position;
    }

    void MoveCameraUpDown()
    {
        float newY = initialPosition.y + moveDistance * Mathf.Sin(Time.time * moveSpeed);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
