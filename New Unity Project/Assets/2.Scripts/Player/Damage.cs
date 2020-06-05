using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private const string enemyTag = "ENEMY";

    private float initHp = 100.0f;
    public float currHp;

    //델리게이트 및 이벤트 선언
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    // Start is called before the first frame update
    void Start()
    {
        currHp = initHp;
    }

    //충돌한 Collider의 InTrigger 옵션이 체크 됐을 때 발쌩
     void OnTriggerEnter(Collider coll)
    {
        //충돌한 Collider 의 태그가 BULLET이면 Player의 CurrHP를 차감
        if(coll.tag ==bulletTag)
        {
            Destroy(coll.gameObject);

            currHp -= 5.0f;
            Debug.Log("Player HP =" + currHp.ToString());

            //player의 생명이 0 이하이면 사망 처리
            if(currHp<=0.0f)
            {
                playerDie();
            }
        }
    }
    //player의 사망 처리 루틴
    void playerDie()
    {
        OnPlayerDie();
        Debug.Log("playerDie !");
        //"ENEMY" 태그로 지정된 모든 적 캐릭터를 추출해 배열에 저장
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        //배열의 처음부터 순회하면서 적 캐릭터의 onPlayerDie 함수를 호출
        for(int i= 0;i<enemies.Length;i++)
        {
            enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }
    }

}
