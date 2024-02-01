using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float Speed = 0.8f;
    float h, v;
    float _X = 0.0f;
    float _Y = 0.0f;
    static int cnt = 0;
    bool isStunned = false;  // 스턴 상태 여부를 나타내는 변수
    float stunDuration = 3f;  // 스턴 지속 시간

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStunned)
        {
            Vector3 playerPosition = GetPlayerPosition();
            Debug.Log("플레이어 위치: " + playerPosition);
            AI(playerPosition, transform.position);
        }
    }

    void FixedUpdate()
    {
        if (cnt == 10)
        {
            _X = Random.Range(-0.5f, 0.5f);
            _Y = Random.Range(-0.5f, 0.5f);
            cnt = 0;
        }
        cnt++;

        if (!isStunned)
        {
            // Point 1.
            h = _X;       // 좌우 이동
            v = _Y;       // 전후 이동

            // Point 2.
            transform.position += new Vector3(h, 0, v) * Speed * Time.deltaTime;
        }
    }

    public Vector3 GetPlayerPosition()
    {
        // Player 오브젝트를 찾음
        GameObject player = GameObject.Find("Player");

        // Player 오브젝트의 위치를 가져옴
        Vector3 playerPosition = player.transform.position;

        // Player의 위치를 반환
        return playerPosition;
    }

    void AI(Vector3 playerPosition, Vector3 monsterPosition)
    {
        // 몬스터의 AI 동작을 구현
        // 여기에 몬스터의 행동 및 공격 등을 추가
    }

    // 스턴 상태로 변경하는 메서드
    void StunMonster()
    {
        isStunned = true;
        StartCoroutine(RecoverFromStun());  // 스턴 상태에서 회복하는 코루틴 시작
    }

    // 스턴 상태에서 일정 시간이 지난 후에 원래 상태로 복구하는 코루틴
    IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }

    // 스턴 총알과 충돌했을 때 호출되는 메서드
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StunBullet"))
        {
            StunMonster();  // 몬스터를 스턴시킴
        }
    }
}
