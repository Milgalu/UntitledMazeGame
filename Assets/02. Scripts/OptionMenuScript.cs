using UnityEngine;
using UnityEngine.SceneManagement;
public class OptionMenuScript : MonoBehaviour
{
    public GameObject optionUI;
    public GameObject itemUI;
    private bool isCanvasActive = false;
    public string nextScene = "Start";

    // 추가된 부분
    private FirstPersonController firstPersonController;

    void Start()
    {
        // Option 스크립트가 있는 게임 오브젝트의 CameraMovement 컴포넌트를 가져옵니다.
        firstPersonController = GetComponent<FirstPersonController>();
        // 시작 시에는 옵션 캔버스를 비활성화
        optionUI.SetActive(false);
    }

    void Update()
    {
        // ESC 키를 누르면 옵션 캔버스의 활성/비활성을 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionMenu();
        }
    }

    public void ToggleOptionMenu()
    {
        // 옵션 캔버스의 활성/비활성을 토글
        isCanvasActive = !isCanvasActive;
        optionUI.SetActive(isCanvasActive);

        // 게임 진행 중에 일시 정지 상태로 변경
        Time.timeScale = isCanvasActive ? 0f : 1f;

        // 마우스 커서를 보이게 함
        Cursor.visible = isCanvasActive;

        // 마우스 커서를 고정/해제
        Cursor.lockState = isCanvasActive ? CursorLockMode.None : CursorLockMode.Locked;

        // 옵션 메뉴가 활성화되면 플레이어의 움직임을 멈춤
        firstPersonController.cameraCanMove = isCanvasActive ? false : true;

        // 옵션 메뉴가 활성화되면 아이템 메뉴를 비활성화
        itemUI.SetActive(!isCanvasActive);
    }

    public void ResumeGame()
    {
        // Resume 버튼이 눌렸을 때의 동작
        ToggleOptionMenu();
    }

    public void RestartGame()
    {
        // Restart 버튼이 눌렸을 때의 동작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ToggleOptionMenu();
    }

    public void ExitGame()
    {
        // SceneManager를 사용하여 씬 변경
        SceneManager.LoadScene(nextScene);
        Time.timeScale = 1f; // 게임 일시 정지 해제
        // 마우스 커서를 보이게 함
        Cursor.visible = true;
        // 마우스 커서를 고정/해제
        Cursor.lockState = CursorLockMode.None ;
    }
}
