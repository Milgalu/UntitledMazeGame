using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public int healAmount = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Item  ȿ�� �ֱ� ���� ȸ��
        transform.Rotate(0f, 15f * Time.deltaTime, 0f);

    }
}
