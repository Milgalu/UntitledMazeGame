using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public GameObject clearMessage; // 플레이어에게 표시할 클리어 메시지

    void Start()
    {
        // 초기에는 메시지를 비활성화
        clearMessage.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log("Player is close to the goal!");
                clearMessage.SetActive(true); // 클리어 메시지 활성화
            }
        }
    }
}
