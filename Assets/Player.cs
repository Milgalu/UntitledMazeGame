using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 10.0f;
    public float h, v;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        // Point 1.
        h = Input.GetAxis("Horizontal");        // 가로축
        v = Input.GetAxis("Vertical");          // 세로축

        // Point 2.
        transform.position += new Vector3(h, 0, v) * Speed * Time.deltaTime;
    }
    
}
