using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        // �� �Լ���ȣ��Ǹ� ����Ƽ UI ȭ�鿡�� Start ��ư�� ���� ������
        // �׷��� ������ ���⼭ �� ȭ���� scPlay ȭ������ ��ȯ�ؾ� �Ѵ�!
        Debug.Log("��ŸƮ ��ư�� Ŭ��!");

        SceneManager.LoadScene("scPlay");
    }
}
