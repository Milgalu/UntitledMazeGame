using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text txtScore;

    public Text besttxtScore;

    private int totScore = 0;

    private int bestScore; // 최고 점수 알려주는 변수를 추가

    // Start is called before the first frame update
    void Start()
    {
        //시작 시 bestScore에 점수 저장
        bestScore = PlayerPrefs.GetInt("TOT_SCORE", 0);
        
        DispScore(0);    
    }

    public void DispScore(int score)
    {
        totScore += score;
        txtScore.text = "score <color=#ff0000>" + totScore + "</color>";

        besttxtScore.text = "best score <color=#ff0000>" + bestScore + "</color>";
        //이전 최고기록 보다 현재 최고 기록이 높다면
        if(totScore> bestScore)
        {
            bestScore = totScore;
            PlayerPrefs.SetInt("TOT_SCORE", totScore);
        }
        
    }
}
