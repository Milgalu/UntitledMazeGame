using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text txtScore;

    public Text besttxtScore;

    private int totScore = 0;

    private int bestScore; // �ְ� ���� �˷��ִ� ������ �߰�

    // Start is called before the first frame update
    void Start()
    {
        //���� �� bestScore�� ���� ����
        bestScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        
        DispScore(0);    
    }

    public void DispScore(int score)
    {
        totScore += score;
        txtScore.text = "score <color=#ff0000>" + totScore + "</color>";

        besttxtScore.text = "best score <color=#ff0000>" + bestScore + "</color>";
        //���� �ְ��� ���� ���� �ְ� ����� ���ٸ�
        if(totScore> bestScore)
        {
            bestScore = totScore;
            PlayerPrefs.SetInt("TOT_SCORE", totScore);
        }
        
    }
}
