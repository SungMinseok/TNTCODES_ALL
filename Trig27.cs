﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
[RequireComponent(typeof(BoxCollider2D))]
public class Trig27 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    //public static Trig27 instance;
    public int trigNum;

    // [Header ("텐드 진입 전")]
    // public Select select_1;
    // [Header ("거미줄 완료 후")]
    // public Select select_2;
    [Header ("대사")]
    public Dialogue dialogue_1;
    [Header ("대사2")]
    public Dialogue dialogue_2;
    [Header ("대사3")]
    public Dialogue dialogue_3;
    [Header ("대사4")]
    public Dialogue dialogue_4;
    [Header ("선택1")]
    public Select select_1;
    [Header ("선택2")]
    public Select select_2;
    [Header ("선택3")]
    public Select select_3;

    
    
    //public GameObject centerView;
    

    ///////////////////////////////////////////////////////////////////   Don't Touch
    private DialogueManager theDM;
    private SelectManager theSelect;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private DatabaseManager theDB;
    //private BookManager theBook;
    private MapManager theMap; 
    private CameraMovement theCamera;
    private PuzzleManager thePuzzle;
    private int temp;


    ///////////////////////////////////////////////////////////////////   flag는 실행여부 파악 : true이면 실행중/실행완료, false이면 실행전 //  bifer : 분기
    public bool flag;    // true 이면 다시 실행 안됨.
    
    private int bifur;
    
    /////////////////////////////////////////////////////////////////////   inspector에서 체크 가능. 1: 닿으면 자동 실행, 2: 체크시 해당 트리거 무한 반복.
    
    [Header ("실행시 바라보는 방향")]public string turn;
    [Header ("트리거 진입 시 자동 실행")]public bool autoEnable;
    
    [Header ("여러번 실행 가능")]public bool preserveTrigger;
    
    [Header ("게임 중 딱 한번만 실행(영원히)")]public bool onlyOnce= true;
    [Header ("특정 분기만(부터) 반복")]public int repeatBifur;

    void Start()                                                            //Don't Touch
    {
        this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        //instance = this;
        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        theCamera = CameraMovement.instance;
        theMap= MapManager.instance;
        thePuzzle= PuzzleManager.instance;

        

        if(trigNum==37){
            if(theDB.trigOverList.Contains(11)){
                flag = false;
            }
            else flag = true;
        }
        else if(trigNum==40){
            if(theDB.gameOverList.Contains(18)){
                flag = false;
            }
            else flag = true;
        }

        else if(trigNum==76){
            if(theDB.trigOverList.Contains(11)){
                flag = false;
                
                if(theDB.trigOverList.Contains(trigNum)){//원래 맨 밑.
                    bifur = repeatBifur;
                }
                
            }
            else{
                flag = true;
            }
        }

        if(theDB.trigOverList.Contains(trigNum)&&trigNum!=76){//원래 맨 밑.
            flag = true;
            if(repeatBifur!=0){
                flag = false;
                bifur = repeatBifur;
            }
        }

    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //if(theDB.trigOverList.Contains(24)&&!GameManager.instance.playing&&!theDB.gameOverList.Contains(6)){
            // if(!thePlayer.exc.GetBool("on")&&!flag&&!autoEnable){
            //     thePlayer.exc.SetBool("on",true);
            //     thePlayer.canInteractWith = trigNum;
            // }
            // if(collision.gameObject.name == "Player" && !flag && !autoEnable && Input.GetKeyDown(KeyCode.Space)&& !theDM.talking){
            //     flag = true;
            //     thePlayer.exc.SetBool("on",false);
            //     thePlayer.canInteractWith = 0;
            //     StartCoroutine(EventCoroutine());
            // }

            // if(collision.gameObject.name == "Player" && !flag && autoEnable&& !theDM.talking){
            //     flag = true;
            //     StartCoroutine(EventCoroutine());
            // } 
        //}

        if(!thePlayer.exc.GetBool("on")&&!flag&&!autoEnable&&(thePlayer.canInteractWith==0||thePlayer.canInteractWith==trigNum)){
            thePlayer.exc.SetBool("on",true);
            thePlayer.canInteractWith = trigNum;
        }


        //그 넘버만 실행함.
        if(!flag && !autoEnable && Input.GetKeyDown(KeyCode.Space)&& !theDM.talking &&thePlayer.canInteractWith==trigNum){
            flag = true;
            thePlayer.exc.SetBool("on",false);
            StartCoroutine(EventCoroutine());
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


    IEnumerator EventCoroutine(){

        theOrder.NotMove();  
        theOrder.PreLoadCharacter();  
        if(turn!="")      
            theOrder.Turn("Player",turn);
                                               //트리거 중 이동불가


        //////////////////////////////////////////////////////////////////////트리거마다 수정해야하는 부분 시작
    

        if(trigNum==29){
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(()=> !theDM.talking);
            AudioManager.instance.Play("getitem2");
            
            Inventory.instance.GetItem(11);
        }    
        else if(trigNum==30){

            //BGMManager.instance.FadeinMusic();
            theDB.progress=4;
            BookManager.instance.ActivateUpdateIcon(2);
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(()=> !theDM.talking);

        }    
        else if(trigNum==37){
            
        // AudioManager.instance.Play("lockdoor");
        //     theOrder.Turn("Player","UP");
        // theDM.ShowDialogue(dialogue_2);
        // yield return new WaitUntil(()=> !theDM.talking);
            theOrder.Turn("Player","LEFT");
        theDM.ShowDialogue(dialogue_1);
        yield return new WaitUntil(()=> !theDM.talking);

        }
        else if(trigNum==76){
            if(bifur==0){

                theDM.ShowDialogue(dialogue_1);
                yield return new WaitUntil(()=> !theDM.talking);
                
                theSelect.ShowSelect(select_1);//물어볼래?3가지
                yield return new WaitUntil(() => !theSelect.selecting);
                if(theSelect.GetResult()==0){
                    theDM.ShowDialogue(dialogue_2);
                    yield return new WaitUntil(()=> !theDM.talking);
                }
                else if(theSelect.GetResult()==1){
                    theDM.ShowDialogue(dialogue_3);
                    yield return new WaitUntil(()=> !theDM.talking);
                }
                else if(theSelect.GetResult()==2){
                    theDM.ShowDialogue(dialogue_4);
                    yield return new WaitUntil(()=> !theDM.talking);
                }
                
                theSelect.ShowSelect(select_2);//더물어볼래?
                yield return new WaitUntil(() => !theSelect.selecting);
                if(theSelect.GetResult()==0){//네
                    //질문 무한 반복
                    while(true){
                        theSelect.ShowSelect(select_3);//그래 더물어봐
                        yield return new WaitUntil(() => !theSelect.selecting);
                        if(theSelect.GetResult()==0){
                            theDM.ShowDialogue(dialogue_2);
                            yield return new WaitUntil(()=> !theDM.talking);
                        }
                        else if(theSelect.GetResult()==1){
                            theDM.ShowDialogue(dialogue_3);
                            yield return new WaitUntil(()=> !theDM.talking);
                        }
                        else if(theSelect.GetResult()==2){
                            theDM.ShowDialogue(dialogue_4);
                            yield return new WaitUntil(()=> !theDM.talking);
                        }
                        
                        theSelect.ShowSelect(select_2);//더물어볼래?
                        yield return new WaitUntil(() => !theSelect.selecting);
                        if(theSelect.GetResult()==0){//네
                        }
                        else if(theSelect.GetResult()==1){//아니오
                            break;
                        }
                    }
                }
                else if(theSelect.GetResult()==1){//아니오
                }

            }
            else{
                theSelect.ShowSelect(select_2);//더물어볼래?
                yield return new WaitUntil(() => !theSelect.selecting);
                if(theSelect.GetResult()==0){//네
                    //질문 무한 반복
                    while(true){
                        theSelect.ShowSelect(select_3);//그래 더물어봐
                        yield return new WaitUntil(() => !theSelect.selecting);
                        if(theSelect.GetResult()==0){
                            theDM.ShowDialogue(dialogue_2);
                            yield return new WaitUntil(()=> !theDM.talking);
                        }
                        else if(theSelect.GetResult()==1){
                            theDM.ShowDialogue(dialogue_3);
                            yield return new WaitUntil(()=> !theDM.talking);
                        }
                        else if(theSelect.GetResult()==2){
                            theDM.ShowDialogue(dialogue_4);
                            yield return new WaitUntil(()=> !theDM.talking);
                        }
                        
                        theSelect.ShowSelect(select_2);//더물어볼래?
                        yield return new WaitUntil(() => !theSelect.selecting);
                        if(theSelect.GetResult()==0){//네
                        }
                        else if(theSelect.GetResult()==1){//아니오
                            break;
                        }
                    }
                }
                else if(theSelect.GetResult()==1){//아니오
                }
            }

        }
        
        else{
            theDM.ShowDialogue(dialogue_1);
            yield return new WaitUntil(()=> !theDM.talking);

        }

        theOrder.Move(); 
            

        if(onlyOnce)
            theDB.trigOverList.Add(trigNum);
        if(preserveTrigger)
            flag=false;
        if(repeatBifur!=0){
            theDB.trigOverList.Add(trigNum);
            flag=false;
            bifur=repeatBifur;
        }
    }
}

