using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HP : MonoBehaviour
{
    public GameObject optionUI;
    public GameObject gameoverUI;
    public GameObject itemUI;

    private FirstPersonController firstPersonController;
    private Transform tr;
  
    private Animator animator;

    private int hp = 100;
    public bool isDie = false;

    private int initHp;
    public Image imgHpbar;

    GameManager gameMgr;

    public Image lightImage; // 플레이어의 이미지 컴포넌트


    // Start is called before the first frame update
    void Start()
    {
        initHp = hp;

        tr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Option 스크립트가 있는 게임 오브젝트의 CameraMovement 컴포넌트를 가져옵니다.
        firstPersonController = GetComponent<FirstPersonController>();
        // 시작 시에는 옵션 캔버스를 비활성화
        gameoverUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (isDie)
            return;

        if (coll.gameObject.tag == "PUNCH")
        {
            hp -= 10;

            // HP에 따라 HP 바 업데이트
            imgHpbar.fillAmount = (float)hp / (float)initHp;

            // HP에 따라 이미지의 투명도 업데이트
            float alpha = Mathf.Lerp(0.0f, 0.2f, 1f - (float)hp / (float)initHp); // 역으로 계산하여 피가 적을수록 투명도가 높아지도록
            Color imageColor = lightImage.color;
            imageColor.a = alpha;
            lightImage.color = imageColor;

            Debug.Log("Player HP = " + hp);
            if (hp <= 0)
            {
                PlayerDie();
            }
        }


        //ü�¾�����
        
        else if (coll.gameObject.tag == "HPPill")
        {
            Heal heal = coll.gameObject.GetComponent<Heal>();

            if (heal != null)
            {
                hp += heal.healAmount;

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
        isDie = true;

        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        foreach (GameObject monster in monsters)
        {
            monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }

        GameManager.instance.isGameOver = true;

        gameoverUI.SetActive(true);

        // 마우스 커서를 보이게 함
        Cursor.visible = true;

        // 마우스 커서를 고정/해제
        Cursor.lockState = CursorLockMode.None;

        // 옵션 메뉴가 활성화되면 플레이어의 움직임을 멈춤
        firstPersonController.cameraCanMove = false;
        firstPersonController.walkSpeed = 0.0f;

        // 옵션 메뉴가 활성화되면 아이템 메뉴를 비활성화
        itemUI.SetActive(false);

        // 오브젝트 제거
        Destroy(optionUI, 0.0f);
    }
}
