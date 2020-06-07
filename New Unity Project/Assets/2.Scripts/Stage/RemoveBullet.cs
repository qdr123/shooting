//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{

    //스파크 프리팹을 저장할 변수
    public GameObject sparkEffect;
    //충돌이 시작할때 발생하는 이벤트
    private void OnCollisionEnter(Collision coll)
    {
        //충돌한 게임오브젝트의 태그값 비교
        if(coll.collider.tag=="BULLET")
        {
            //스파크 효과 함수 호출
            showEffect(coll);
            //충돌한 게임오브젝트 삭제
            //Destroy(coll.gameObject);
            coll.gameObject.SetActive(false);

        }
    }

    private void showEffect(Collision coll)
    {
        //출동 지점의 정보를 추출
        ContactPoint contact = coll.contacts[0];
        //법선 벡터가 이루는 회전각도를 추출
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        //스파크 효과를 생성
        Instantiate(sparkEffect, contact.point, rot);
    }
}
