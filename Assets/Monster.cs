using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float Speed = 0.8f;
    float h, v;
    static int cnt = 0;
    private MonsterAI monsterAI;

    void Start()
    {
        monsterAI = new MonsterAI();
    }

    // Update is called once per frame
    void Update()
    {
        if (cnt == 100)
        {
            monsterAI.Run();
            cnt = 0;
        }
        cnt++;
    }
    void OnCollisionEnter(Collision collision)
    {
        string objectTag = collision.gameObject.tag;
        Debug.Log("오브젝트 태그: " + objectTag);
        switch (objectTag)
        {
            case "WALL":
                monsterAI.CollisionWall();
                Debug.Log("벽과 충돌했습니다!!!!!!!!!!!");
                break;
            case "PLAYER":
                monsterAI.CollisionPlayer();
                Debug.Log("플레이어를 잡았습니다!");
                break;
            default:
                // 기타 오브젝트와 충돌했을 때 처리
                Debug.Log("알 수 없는 오브젝트와 충돌했습니다!");
                break;
        }
        // 캐릭터 이전 이동을 취소
    }
    /*
    void FixedUpdate()
    {
        if (cnt == 10)
        {
            _X = Random.Range(-0.5f, 0.5f);
            _Y = Random.Range(-0.5f, 0.5f);
            cnt = 0;
        }
        cnt++;
        // Point 1.
        h = _X;       // 가로축
        v = _Y;          // 세로축

        // Point 2.
        transform.position += new Vector3(h, 0, v) * Speed * Time.deltaTime;
    }*/

}
