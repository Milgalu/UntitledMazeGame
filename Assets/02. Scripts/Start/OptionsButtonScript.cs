using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsButtonScript : MonoBehaviour
{
    public string nextScene;
    public void Options()
    {
        // 옵션 버튼이 클릭되었을 때 실행되는 함수
        Debug.Log("옵션 메뉴를 엽니다.");
        // 옵션 메뉴 로직을 추가하려면 여기에 추가
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
}
