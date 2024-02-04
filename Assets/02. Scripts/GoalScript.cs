using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public GameObject clearMessage; // 플레이어에게 표시할 클리어 메시지
    public GameObject itemUI; // 플레이어에게 표시할 클리어 메시지
    
    public FirstPersonController firstPersonController;

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
                itemUI.SetActive(false); // 클리어 메시지 활성화
                Time.timeScale = 0f; // 게임 일시 정지
                Cursor.visible = true; // 마우스 커서 보이게 함
                Cursor.lockState = CursorLockMode.None; // 마우스 커서 고정 해제
                // 옵션 메뉴가 활성화되면 플레이어의 움직임을 멈춤
                firstPersonController.cameraCanMove = false;
                // 옵션 메뉴가 활성화되면 아이템 메뉴를 비활성화
                itemUI.SetActive(false);
            }
        }
    }
}
