using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public enum State
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }
    //상태를 저장할 변수
    public State state = State.PATROL;

    //주인공의 위치를 저장할 변수
    private Transform playerTr;
    //적 캐릭터의 위치를 저장할 변수
    private Transform enemyTr;
    //Animator 컴포넌트를 저장할 변수
    private Animator animator;

    //공격 사정거리
    public float attackDist = 5.0f;
    //추적 사정거리
    public float traceDist = 10.0f;

    //사망 여부를 판단할 변수
    public bool isDie = false;

    //코루틴에서 사용할 지연시간 변수
    private WaitForSeconds ws;
    //이동을 제어하는 MoveAgent 클래스를 저장할 변수
    private MoveAgent moveAgent;
    //애니메이터 컨트롤러에 정의한 파라미터의 해시값을 미리 추출
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");

    void Awake()
    {
        //주인공 게임 오브젝트 추출
        var player = GameObject.FindGameObjectWithTag("PLAYER");

        //주인공의 Transform 컴포넌트 추출
        if (player != null)
            playerTr = player.GetComponent<Transform>();

        //적 캐릭터의 Tranform 컴포넌트 추출
        enemyTr = GetComponent<Transform>();

        //Animator 컴포넌트 추출
        animator = GetComponent<Animator>();

        //이동을 제어하는 MoveAgent 클래스를 추출
        moveAgent = GetComponent<MoveAgent>();

        //코루틴의 지연시간 생성
        ws = new WaitForSeconds(0.3f);
       
    }
    void OnEnable()
    {
        //CheckState 코루틴 함수 실행
       StartCoroutine(CheckState());
        //Action 코루틴 함수 실행
        StartCoroutine(Action());
    }

    
    IEnumerator CheckState()
    {
        //적 캐릭터가 사망하기 전까지 도는 무한 루프
        while(!isDie)
        {
            //상태가 사망이면 코루틴 함수를 종료 시킴
            if (state == State.DIE) yield break;

            //주인공과 적 캐릭터 간의 거리를 계산
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            //공격 사정거리 이내인 경우
            if(dist<= attackDist)
            {
                state = State.ATTACK;
            }//추적 사정거리 이내인 경우
            else if (dist<=traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.PATROL;
            }
            //0.3초 동안 대기하는 동안 제어권을 양보
            yield return ws;
        }
    }
    IEnumerator Action()
    {
       
        //적 캐릭터가 사망할 떄까지 무한루프
        while(!isDie)
        {
            yield return ws;
            //상태에 따라 분기 처리
            switch (state)
            {
                case State.PATROL:
                    //순찰 모드를 활성화
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    //주인공의 위치를 넘겨 추적 모드로 변경
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, true);
                    break;
                case State.DIE:
                    moveAgent.Stop();
                    break;
            }
        }
    }
    void Update()
    {
        //Speed 파라미터에 이동 속도를 전달
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

}
