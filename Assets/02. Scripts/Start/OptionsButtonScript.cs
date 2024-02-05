using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsButtonScript : MonoBehaviour
{
    
    public GameObject optionUI;
    public GameObject startUI;
    public string nextScene;
    public void Options()
    {
        // 옵션 버튼이 클릭되었을 때 실행되는 함수
        Debug.Log("옵션 메뉴를 엽니다.");
        // 옵션 메뉴 로직을 추가하려면 여기에 추가
        optionUI.SetActive(true);
        startUI.SetActive(false);
    }
    public void StartGame()
    {
        // 시작 버튼이 클릭되었을 때 실행되는 함수
        Debug.Log("게임을 시작합니다!");
        // 게임 시작 로직을 추가하려면 여기에 추가
        
        // SceneManager를 사용하여 씬 변경
        SceneManager.LoadScene(nextScene);
    }

    public void QuitGame()
    {
        // 게임 종료
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void BackToStart()
    {
        // 옵션 메뉴에서 뒤로 가기 버튼이 클릭되었을 때 실행되는 함수
        Debug.Log("시작 화면으로 돌아갑니다.");
        // 옵션 메뉴 로직을 추가하려면 여기에 추가
        optionUI.SetActive(false);
        startUI.SetActive(true);
    }
}
