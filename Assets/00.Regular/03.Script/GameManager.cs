using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform[] points;
    public GameObject monsterPrefab;
    public GameObject robotPrefab;
    public float createTime = 2.0f;
    public int maxMonster = 10;
    public bool isGameOver = false;

    int prevTime;
    float countTime;

    public GameObject itemPrefab;// 아이템 변수
    int prevItemCheck; //아이템 계싼을 위한 변수 

    public static GameManager instance = null;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();    
    }

    // Update is called once per frame
    void Update()
    {
        

        if(isGameOver == false && points.Length > 0)
        {
            countTime += Time.deltaTime;

            int currTime = (int)(countTime % createTime);
            if(currTime == 0 && prevTime != currTime)
            {
                int monster1Count = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length;
                int monster2Count = (int)GameObject.FindGameObjectsWithTag("ROBOT").Length;
                int monsterCount = monster1Count + monster2Count;

                if (monsterCount < maxMonster)
                {
                    int idx = Random.Range(1, points.Length - 1);


                    if (Random.Range(0, 9) < 8)
                    {
                        Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
                    }
                    else
                    {
                        GameObject robot = Instantiate(robotPrefab, points[idx].position, points[idx].rotation);

                        if (Random.Range(0, 10) <= 5)
                        {
                            robot.GetComponent<RobotControl>().isMoving = true;

                        }

                        else
                        {
                            robot.GetComponent<RobotControl>().isMoving = false;
                        }                    
                    }
                }
                
            }
            prevTime = currTime;

            //아이템 체크
            int curItemTime = (int)(countTime % 5f);
            if(curItemTime == 0 && prevItemCheck != curItemTime)
            {
                Vector3 randpos = new Vector3(Random.Range(-25f, 25f), 0.5f, Random.Range(-25, 25f));

                GameObject item = Instantiate(itemPrefab, randpos, Quaternion.identity);
            }

            prevItemCheck = curItemTime;
        }
    }
}
