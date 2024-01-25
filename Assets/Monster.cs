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
        Debug.Log("플레이어 포지션: "  + playerPosition);
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
        h = _X;       // 가로축
        v = _Y;          // 세로축

        // Point 2.
        transform.position += new Vector3(h, 0, v) * Speed * Time.deltaTime;
    }
    public Vector3 GetPlayerPosition()
    {
        // Player 오브젝트를 가져옵니다.
        GameObject player = GameObject.Find("Player");

        // Player 오브젝트의 위치를 가져옵니다.
        Vector3 playerPosition = player.transform.position;

        // Player의 위치를 반환합니다.
        return playerPosition;
    }
    void AI(Vector3 playerPosition, Vector3 mosterPosition)
    {

    }
}
