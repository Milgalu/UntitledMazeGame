using UnityEngine;

public class RepeatMovement : MonoBehaviour
{
    public Vector3 startLocation; // 시작 좌표
    public Vector3 endLocation;   // 끝 좌표
    public float speed = 5f;      // 이동 속도
    public float teleportDelay = 1f; // 텔레포트 딜레이

    private bool movingToEnd = true; // 시작에서 끝으로 이동 중인지 여부

    void Update()
    {
        // 현재 방향으로 이동
        float step = speed * Time.deltaTime;
        if (movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endLocation, step);

            // 끝 좌표에 도달하면 텔레포트 딜레이 후 시작 좌표로 텔레포트
            if (Vector3.Distance(transform.position, endLocation) < 0.001f)
            {
                Invoke("TeleportToStart", teleportDelay);
                movingToEnd = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startLocation, step);
        }
    }

    void TeleportToStart()
    {
        // 텔레포트하여 시작 좌표로 이동
        transform.position = startLocation;
        movingToEnd = true;
    }
}
