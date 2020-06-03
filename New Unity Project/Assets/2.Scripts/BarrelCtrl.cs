
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    //폭발 효과 프리팹을 저장할 변수
    public GameObject expEffect;

    //찌그러진 드럼통의 메쉬를 저장할 배열
    public Mesh[] meshes;
    
    //총알이 맞은 횟수
    int hitCount = 0;
    
    //Rigidbody 컴포넌트를 저장할 변수
    Rigidbody rb;

    //MeshFilter 컴포넌트를 저장할 변수
    MeshFilter meshFilter;

    //드럼통의 텍스처를 저장할 배열
    public Texture[] textures;
    //MeshRendderer 컴포넌트를 저장할 변수
    MeshRenderer _renderer;

    //AudioSource 컴포넌트를 저장할 변수
    AudioSource _audio;

    //폭발 반경
    public float expRadius = 10.0f;

    //폭발음 오디오 클립
    public AudioClip expSfx;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody 컴포넌트를 추출해 저장
        rb = GetComponent<Rigidbody>();
        //MeshFilter 컴포넌트를 추출해 저장
        meshFilter = GetComponent<MeshFilter>();
        //MeshRenderer 컴포넌트를 추출해 저장
        _renderer = GetComponent<MeshRenderer>();
        //난수를 발생시켜 불규칙적인 텍스처를 적용
        _renderer.material.mainTexture = textures[UnityEngine.Random.Range(0, textures.Length)];
        //AudioSource 컴포넌트를 추출해 저장
        _audio = GetComponent<AudioSource>();
    }

    //충돌이 발생했을 때 한 번 호출되는 콜백 함수
    private void OnCollisionEnter(Collision coll)
    {
           //충돌한 게임오브젝트의 태그를 비교
           if(coll.collider.CompareTag("BULLET"))
        {
            //총알의 충돌 횟수를 증가시키고 3발 이상 맞았는지 확인
            if(++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    //폭발 효과를 처리할 함수
    private void ExpBarrel()
    {
        //폭발 효과 프리팹을 동적으로 생성
        Instantiate(expEffect, transform.position, Quaternion.identity);
        //Rigidbody 컴포넌트의 mass를 1.0으로 수정해 무게를 가볍게 함
        rb.mass = 1.0f;
        //위로 솟구치는 힘을 가함
        rb.AddForce(Vector3.up * 1000.0f);

        //폭발력 생성
        IndirectDamage(transform.position);

        //난수를 발생
        int idx = UnityEngine.Random.Range(0, meshes.Length);
        //찌그러진 메쉬를 적용
        meshFilter.sharedMesh = meshes[idx];
        //폭발음 발생
        _audio.PlayOneShot(expSfx, 1.0f);

    }

    private void IndirectDamage(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);
        foreach(var coll in colls)
        {
            //폭발 범위에 포함된 드럼통의 Rigidbody 컴포넌트 추출
            var _rb = coll.GetComponent<Rigidbody>();
            //드럼통의 무게를 가볍게 함
            _rb.mass = 1.0f;
            _rb.AddExplosionForce(1200.0f, pos, expRadius, 1000.0f);
        }
    }
}
