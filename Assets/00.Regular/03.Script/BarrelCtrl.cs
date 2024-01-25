using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;
    private Transform tr;

    private int hitCount = 0;

    public AudioClip fireSfx;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        source = GetComponent<AudioSource>();

    }



    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "BULLET")
        {
            Destroy(coll.gameObject);
            if (++hitCount >= 3)
            {
                ExpBarrel();
                source.PlayOneShot(fireSfx, 0.9f);
            }
        }
    }

    void ExpBarrel()
    {
        GameObject explosion = Instantiate(expEffect, transform.position, Quaternion.identity);

        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);

        foreach(Collider coll in colls)
        {
            Rigidbody rbody = coll.GetComponent<Rigidbody>();
            if(rbody!= null)
            {
                rbody.mass = 1.0f;
                rbody.AddExplosionForce(1000.0f, tr.position, 10f, 300f);
            }
        }

        Destroy(explosion, explosion.GetComponent<ParticleSystem>().duration);
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
