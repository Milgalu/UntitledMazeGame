using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���Ž� �ִϸ��̼� �ڵ��Դϴ� ������� ���� ����!
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

    // ���� ���Ž� �ִϸ��̼��� ������� �ʽ��ϴ�!
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

        //���Ž� �ִϸ��̼� �ڵ� : ���̻� ������� �ʽ��ϴ�!
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

        // translate �Լ� ��ü�� �б� �ؼ� 1.5�� �ӵ�/0.5�� �ӵ�/�Ϲ� �ӵ��� �̵��Ҽ� �յ��� ��
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


        //������ �浹ó�� ���� (���� ���̰� -25���� 25���� �̹Ƿ�
        // �� ���̸� ���� ���ϸ� ���������� �Ѵ�.

     
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

        //���� �����ڸ����� ���̻� ������������ �Ѵ�.
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
        //���Ž� �ִϸ��̼ǿ��� ���Ǵ� �ڵ� �Դϴ�. ���̻� ������!
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
