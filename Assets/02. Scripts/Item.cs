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

        // Item  효과 주기 위해 회전
        transform.Rotate(0f, 15f * Time.deltaTime, 0f);

    }
}
