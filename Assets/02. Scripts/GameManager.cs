using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform[] points; // 스폰 포인트 배열
    public GameObject monsterPrefab; // 생성할 몬스터 프리팹
    public GameObject robotPrefab; // 생성할 로봇 프리팹 (현재 사용되지 않음)
    public float createTime = 2.0f; // 몬스터 생성 주기
    public int maxMonster = 10; // 최대 몬스터 수
    public bool isGameOver = false; // 게임 오버 상태

    int prevTime; // 이전 시간 저장 변수
    float countTime; // 시간 계산 변수

    public GameObject itemPrefab; // 생성할 아이템 프리팹
    int prevItemCheck; // 아이템 생성 시간 체크 변수

    public static GameManager instance = null; // 싱글톤 인스턴스

    private void Awake()
    {
        instance = this; // 싱글톤 인스턴스 초기화
    }

    // 첫 프레임 전에 호출
    void Start()
    {
        // 스폰 포인트를 찾아 배열에 할당
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
    }

    // 매 프레임마다 호출
    void Update()
    {
        if (isGameOver == false && points.Length > 0) // 게임 오버 상태가 아니고, 스폰 포인트가 있을 경우
        {
            countTime += Time.deltaTime; // 시간 증가

            int currTime = (int)(countTime % createTime); // 현재 시간 계산
            if (currTime == 0 && prevTime != currTime) // 특정 주기마다 몬스터 생성
            {
                int monsterCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length; // 현재 몬스터 수 계산

                if (monsterCount < maxMonster) // 최대 몬스터 수보다 적을 때
                {
                    int idx = Random.Range(1, points.Length); // 랜덤 스폰 포인트 선택

                    if (Random.Range(0, 9) < 8) // 일정 확률로 몬스터 생성
                    {
                        Instantiate(monsterPrefab, points[idx].position, points[idx].rotation); // 몬스터 생성
                    }
                }
            }
            prevTime = currTime; // 이전 시간 업데이트

            // 아이템 생성 로직
            int curItemTime = (int)(countTime % 5f); // 아이템 생성 주기 계산
            if (curItemTime == 0 && prevItemCheck != curItemTime) // 특정 주기마다 아이템 생성
            {
                Vector3 randpos = new Vector3(Random.Range(-25f, 25f), 3.0f, Random.Range(-25,25f)); // 랜덤 위치 설정

                Instantiate(itemPrefab, randpos, Quaternion.identity); // 아이템 생성
            }

            prevItemCheck = curItemTime; // 이전 아이템 생성 시간 업데이트
        }
    }
}
