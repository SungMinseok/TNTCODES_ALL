﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
public class Trig3 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    public int trigNum;
    
    
    public Select select_1;
    public Dialogue dialogue_1;
    public Dialogue dialogue_2;

    public SpriteRenderer off;
    public SpriteRenderer on;
    public Color color;
    public GameObject turtle;
    public GameObject col;
    //public Color color;

    public GameObject[] trigs;

    ///////////////////////////////////////////////////////////////////   Don't Touch
    private DialogueManager theDM;
    private SelectManager theSelect;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    //private ObjectManager theOB;

    ///////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전 //  bifer : 분기
    protected bool flag;
    
    private int bifur;
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    public bool autoEnable;
    public bool preserveTrigger;
    public bool onlyOnce = true;

    void Start()                                                            //Don't Touch
    {
        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        //theOB = ObjectManager.instance;
        
        if(theDB.trigOverList.Contains(11)){
            turtle.SetActive(true);
            
        }
        else turtle.SetActive(false);
        if(theDB.trigOverList.Contains(trigNum)){
            flag = true;
            off.GetComponent<SpriteRenderer>().enabled = false;
            on.GetComponent<SpriteRenderer>().enabled = true;
            col.SetActive(true);
        }
            
    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //외부에서 콜라이더에 첫 진입하거나(0) 자기 콜라이더 위에 있으면 해당 트리거 넘버 고정.
        if(!thePlayer.exc.GetBool("on")&&!flag&&(thePlayer.canInteractWith==0||thePlayer.canInteractWith==trigNum)){
            thePlayer.exc.SetBool("on",true);
            thePlayer.canInteractWith = trigNum;
        }


        //그 넘버만 실행함.
        if(!flag && !autoEnable && (Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&& !theDM.talking &&thePlayer.canInteractWith==trigNum){
            flag = true;
            thePlayer.exc.SetBool("on",false);
            //thePlayer.canInteractWith = 0;
            StartCoroutine(EventCoroutine());
            Debug.Log(trigNum+"번");
        }

        if(!flag && autoEnable&& !theDM.talking){
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }
    void OnTriggerExit2D(Collider2D collision){
        thePlayer.exc.SetBool("on",false);
        thePlayer.canInteractWith = 0;
    }


    /*IEnumerator FadeOutCoroutine(){
        
        color = sprite.color;
        //fadeFlag = true;
        while(color.a >0f){
            color.a -= 0.02f;
            sprite.color = color;
            Debug.Log("1");
            yield return new WaitForSeconds(0.01f);
        }
        
        //fadeFlag = false;
    }
*/
    private void Wait(){
        StartCoroutine(_Wait());
    }
    IEnumerator _Wait(){
        yield return new WaitForSeconds(0.01f);
    }
    IEnumerator EventCoroutine(){

        //theOrder.PreLoadCharacter();        
        //theOrder.Turn("Player","UP");

        theOrder.NotMove();                                                 //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
         
        theSelect.ShowSelect(select_1);
        yield return new WaitUntil(() => !theSelect.selecting);
                   //대화 끝날 때까지 대기 (마지막 제외 필수)
        
        if(theSelect.GetResult()==0){
            //StartCoroutine(FadeOutCoroutine());
            off.GetComponent<SpriteRenderer>().enabled = false;
            on.GetComponent<SpriteRenderer>().enabled = true;
            col.SetActive(true);
            
            AudioManager.instance.Play("magic0");
            //theOB.RemoveSlowly(sprite);
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(()=> !theDM.talking); 
            
            
            preserveTrigger=false;
            if(onlyOnce)
                theDB.trigOverList.Add(trigNum);
            
        }
        else{
            
            theDM.ShowDialogue(dialogue_2);
            yield return new WaitUntil(()=> !theDM.talking); 
        }
            
        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 끝

        theOrder.Move();                                                        //트리거 완료 후 이동가능



        if(preserveTrigger)
            flag=false;
    }


}

