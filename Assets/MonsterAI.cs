using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    List<float> actionSpace = new List<float>() { 0.0f, 1.0f, 2.0f, 3.0f };
    float learningRate = 0.1f;
    float discountFactor = 0.99f;
    int numEpisodes = 1;
    int T = 0;
    PPO ppo;
    public MonsterAI() 
    {
        ppo = new PPO(learningRate, discountFactor);
        Debug.Log("생성자 실행");
    }
    
    public void Run()
    {
        ppo.SetStateSpace();
        ppo.SetActionSpace(actionSpace);
        ppo.Train(numEpisodes);
        T = Random.Range(1, 10);
    }

    public void CollisionWall()
    {
        ppo.CollisionWall();
    }
    public void CollisionPlayer()
    {

    }
}
