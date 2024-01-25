using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 레거시 애니메이션 코드입니다 사용하지 않을 예정!
//[System.Serializable]
//public class Anim
//{
//    public AnimationClip idle;
//    public AnimationClip runForward;
//    public AnimationClip runBackward;
//    public AnimationClip runRight;
//    public AnimationClip runLeft;
//}
public class PlayerCtrl : MonoBehaviour
{

    private float h = 0.0f;
    private float v = 0.0f;
    private Transform tr;
    public float moveSpeed = 10.0f;
    public float rotSpeed = 100.0f;

    // 이제 레거시 애니메이션은 사용하지 않습니다!
    //public Anim anim;
    //Animation _animation;
    private Animator animator;

    private int hp = 100;
    public bool isDie = false;

    private int initHp;
    public Image imgHpbar;

    GameManager gameMgr;


    // Start is called before the first frame update
    void Start()
    {

        initHp = hp;

        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();

        //레거시 애니메이션 코드 : 더이상 사용하지 않습니다!
        //_animation = GetComponentInChildren<Animation>();
        //_animation.clip = anim.idle;
        //_animation.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
            return;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //Debug.Log("h=" + h.ToString());
        //Debug.Log("v=" + v.ToString());

       
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // translate 함수 자체를 분기 해서 1.5배 속도/0.5배 속도/일반 속도로 이동할수 잇도록 함
        if (Input.GetKey(KeyCode.LeftControl))
        {
            tr.Translate(moveDir.normalized * moveSpeed*1.5f * Time.deltaTime, Space.Self);

        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            tr.Translate(moveDir.normalized * moveSpeed*0.5f * Time.deltaTime, Space.Self);
        }
        else
        {
            tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
        }


        //간단한 충돌처리 로직 (벽의 길이가 -25에서 25사이 이므로
        // 그 사이를 넘지 못하면 못지나가게 한다.

     
        Vector3 pos = tr.position;
        Debug.Log("pos:" + pos);

        if (pos.x < -24f)
            pos.x = -24f;
        else if (pos.x >24f)
            pos.x = 24f;

        if (pos.z < -24f)
            pos.z = -24f;
        else if (pos.z >24f)
            pos.z = 24f;

        //벽의 가장자리에서 더이상 못지나가도록 한다.
        tr.position = pos;


        tr.Rotate(Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis("Mouse X"));
        
        if(moveDir.magnitude > 0)
        {
            animator.SetFloat("Speed", 1.0f);
        }
        else
        {
            animator.SetFloat("Speed", 0.0f);
        }
        //레거시 애니메이션에서 사용되던 코드 입니다. 더이상 사용안함!
        //if(v >= 0.1f)
        //{
        //    _animation.CrossFade(anim.runForward.name, 0.3f);

        //}
        //else if(v <= -0.1f)
        //{
        //    _animation.CrossFade(anim.runBackward.name, 0.3f);
        //}
        //else if (h >= 0.1f)
        //{
        //    _animation.CrossFade(anim.runRight.name, 0.3f);
        //}
        //else if (h <= -0.1f)
        //{
        //    _animation.CrossFade(anim.runLeft.name, 0.3f);
        //}
        //else
        //{
        //    _animation.CrossFade(anim.idle.name, 0.3f);
        //}

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }


 
    private void OnTriggerEnter(Collider coll)
    {
        if (isDie)
            return;

        if(coll.gameObject.tag == "PUNCH")
        {
            hp -= 10;

            imgHpbar.fillAmount = (float)hp / (float)initHp;
            Debug.Log("Player HP = " + hp);
            if(hp <= 0)
            {
                PlayerDie();
            }
            else
            {
                animator.SetTrigger("Hit");
            }
        }

        else if(coll.gameObject.tag == "ENEMYBULLET")
        {
            Destroy(coll.gameObject);
            hp -= coll.gameObject.GetComponent<Bullet>().damgage;

            imgHpbar.fillAmount = (float)hp / (float)initHp;
            Debug.Log("Player HP = " + hp);
            if (hp <= 0)
            {
                PlayerDie();
            }
            else
            {
                animator.SetTrigger("Hit");
            }


        }

        else if (coll.gameObject.tag == "ITEM")
        {
            Item item = coll.gameObject.GetComponent<Item>();

            if(item != null)
            {
                hp += item.healAmount;

                if (hp > 100)
                {
                    hp = 100;
                }
                imgHpbar.fillAmount = (float)hp / (float)initHp;
            }
            Destroy(coll.gameObject);
        }
    }


    void PlayerDie()
    {
        Debug.Log("Player Die!");
        animator.SetTrigger("Die");
        isDie = true;

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");
        
        foreach(GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }

        //gameMgr.isGameOver = true;
        GameManager.instance.isGameOver = true;
    }
}
