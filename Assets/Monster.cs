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
        Debug.Log("������Ʈ �±�: " + objectTag);
        switch (objectTag)
        {
            case "WALL":
                monsterAI.CollisionWall();
                Debug.Log("���� �浹�߽��ϴ�!!!!!!!!!!!");
                break;
            case "PLAYER":
                monsterAI.CollisionPlayer();
                Debug.Log("�÷��̾ ��ҽ��ϴ�!");
                break;
            default:
                // ��Ÿ ������Ʈ�� �浹���� �� ó��
                Debug.Log("�� �� ���� ������Ʈ�� �浹�߽��ϴ�!");
                break;
        }
        // ĳ���� ���� �̵��� ���
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
        h = _X;       // ������
        v = _Y;          // ������

        // Point 2.
        transform.position += new Vector3(h, 0, v) * Speed * Time.deltaTime;
    }*/

}
