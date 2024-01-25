using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI
;
public class RobotControl : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3.0f;

    private Transform target;
    private float spawnRate;
    private float timerAfterSpawn;

    public int hp = 100;

    public bool isMoving = false;
    private NavMeshAgent nvAgent;
    Animator animator;

    public AudioClip fireClip;
    AudioSource fireAudio;

    private GameUI gameUI;



    // Start is called before the first frame update
    void Start()
    {
        timerAfterSpawn = 0f;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        StartCoroutine(MonsterAI());
        nvAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fireAudio = GetComponent<AudioSource>();

        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();

    }

    //�ڷ�ƾ���� ����� ���� AI

    IEnumerator MonsterAI()
    {
        while (hp > 0)
        {
            yield return new WaitForSeconds(0.2f);

            if (isMoving)
            {
                nvAgent.destination = target.position;
                nvAgent.isStopped = false;
                animator.SetBool("isMoving", true);
            }

            else
            {
                nvAgent.isStopped = true;
                animator.SetBool("isMoving", false);
            }
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
            return;

        timerAfterSpawn += Time.deltaTime;

        if (timerAfterSpawn >= spawnRate)
        {
            timerAfterSpawn = 0;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.transform.LookAt(target);
            transform.LookAt(target);
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
            fireAudio.PlayOneShot(fireClip);
        }
    }


    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "BULLET")
        {
            Destroy(coll.gameObject);

            hp -= coll.gameObject.GetComponent<Bullet>().damgage;

            if (hp <= 0)
            {
                MonsterDie();
            }
        }

    }


    void MonsterDie()
    {
        //���Ͱ� �״� ��� �±׸� ���Ͱ� �ƴѰ����� ����

        gameObject.tag = "Untagged";

        StopAllCoroutines();

        nvAgent.isStopped = true;
        animator.SetTrigger("Die");

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach (Collider coll in GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }

        gameUI.DispScore(100);
    }
}
