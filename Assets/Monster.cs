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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cnt == 10)
        {
            Vector3 TF = transform.position;
            //MosterAI moster;
            
        }
        Vector3 playerPosition = GetPlayerPosition();
        Debug.Log("�÷��̾� ������: "  + playerPosition);
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
        // Point 1.
        h = _X;       // ������
        v = _Y;          // ������

        // Point 2.
        transform.position += new Vector3(h, 0, v) * Speed * Time.deltaTime;
    }
    public Vector3 GetPlayerPosition()
    {
        // Player ������Ʈ�� �����ɴϴ�.
        GameObject player = GameObject.Find("Player");

        // Player ������Ʈ�� ��ġ�� �����ɴϴ�.
        Vector3 playerPosition = player.transform.position;

        // Player�� ��ġ�� ��ȯ�մϴ�.
        return playerPosition;
    }
    void AI(Vector3 playerPosition, Vector3 mosterPosition)
    {

    }
}
