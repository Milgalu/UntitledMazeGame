    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    public enum MonsterState { idle, trace, attack, die};
    public MonsterState monsterState = MonsterState.idle;

    private Transform monsterTr;
    private Transform playerTr;
    private UnityEngine.AI.NavMeshAgent nvAgent;
    private Animator animator;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;
    private bool isDie = false;

    private int hp = 100;

    public GameObject bloodEffect;
    public GameObject bloodDecal;

    private GameUI gameUI;

    // Start is called before the first frame update
    void Start()
    {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        //nvAgent.destination = playerTr.position;

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());

    }

    IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if(dist <= attackDist)
            {
                monsterState = MonsterState.attack;
            }
            else if(dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }

        }
    }

    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            

            switch(monsterState)
            {
                case MonsterState.idle:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsTrace", false);
                    break;
                case MonsterState.trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = false;
                    animator.SetBool("IsTrace", true);
                    animator.SetBool("IsAttack", false);
                    break;
                case MonsterState.attack:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsAttack", true);
                    break;

            }

            yield return new WaitForSeconds(0.2f);

        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "BULLET")
        {
            Destroy(coll.gameObject);

            hp -= coll.gameObject.GetComponent<Bullet>().damgage;
            CreatBloodEffect(coll.transform.position);
            if (hp <= 0)
            {
                MonsterDie();
            }
            else
            {
                animator.SetTrigger("IsHit");
            }
        }
    }

    void MonsterDie()
    {
        StopAllCoroutines();
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.isStopped = true;
        animator.SetTrigger("IsDie");

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach (Collider coll in GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }


        gameUI.DispScore(50);
    }

    private void Update()
    {
        
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();
        //  이렇게 해도 코루틴이 안끝나는 경우를 위해서
        //isDie = true;
        nvAgent.isStopped = true;
        animator.SetTrigger("IsPlayerDie");
    }

    void CreatBloodEffect(Vector3 pos)
    {
        // 혈흔 효과 생성
        GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(blood1, 1.0f);

        Vector3 decalPos = monsterTr.position + (Vector3.up * 0.05f);
        Quaternion decalRot = Quaternion.Euler(90, 0, Random.Range(0, 360));
        GameObject blood2 = (GameObject)Instantiate(bloodDecal, decalPos, decalRot);
        float scale = Random.Range(1.5f, 3.5f);
        blood2.transform.localScale = Vector3.one * scale;


        Destroy(blood2, 5f);
    }
}
