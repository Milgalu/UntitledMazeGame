using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform[] points; // ���� ����Ʈ �迭
    public GameObject monsterPrefab; // ������ ���� ������
    public GameObject robotPrefab; // ������ �κ� ������ (���� ������ ����)
    public float createTime = 2.0f; // ���� ���� �ֱ�
    public int maxMonster = 10; // �ִ� ���� ��
    public bool isGameOver = false; // ���� ���� ����

    int prevTime; // ���� �ð� ���� ����
    float countTime; // �ð� ��� ����

    public GameObject itemPrefab; // ������ ������ ������
    int prevItemCheck; // ������ ���� �ð� üũ ����

    public static GameManager instance = null; // �̱��� �ν��Ͻ�

    private void Awake()
    {
        instance = this; // �̱��� �ν��Ͻ� �ʱ�ȭ
    }

    // ù ������ ���� ȣ��
    void Start()
    {
        // ���� ����Ʈ�� ã�� �迭�� �Ҵ�
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
    }

    // �� �����Ӹ��� ȣ��
    void Update()
    {
        if (isGameOver == false && points.Length > 0) // ���� ���� ���°� �ƴϰ�, ���� ����Ʈ�� ���� ���
        {
            countTime += Time.deltaTime; // �ð� ����

            int currTime = (int)(countTime % createTime); // ���� �ð� ���
            if (currTime == 0 && prevTime != currTime) // Ư�� �ֱ⸶�� ���� ����
            {
                int monsterCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length; // ���� ���� �� ���

                if (monsterCount < maxMonster) // �ִ� ���� ������ ���� ��
                {
                    int idx = Random.Range(1, points.Length); // ���� ���� ����Ʈ ����

                    if (Random.Range(0, 9) < 8) // ���� Ȯ���� ���� ����
                    {
                        Instantiate(monsterPrefab, points[idx].position, points[idx].rotation); // ���� ����
                    }
                }
            }
            prevTime = currTime; // ���� �ð� ������Ʈ

            // ������ ���� ����
            int curItemTime = (int)(countTime % 5f); // ������ ���� �ֱ� ���
            if (curItemTime == 0 && prevItemCheck != curItemTime) // Ư�� �ֱ⸶�� ������ ����
            {
                Vector3 randpos = new Vector3(Random.Range(-25f, 25f), 3.0f, Random.Range(-25,25f)); // ���� ��ġ ����

                Instantiate(itemPrefab, randpos, Quaternion.identity); // ������ ����
            }

            prevItemCheck = curItemTime; // ���� ������ ���� �ð� ������Ʈ
        }
    }
}
