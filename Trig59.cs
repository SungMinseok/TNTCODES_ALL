﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//해당 위치로 이동 후 키입력 받고 이벤트 진행
[RequireComponent(typeof(BoxCollider2D))]
public class Trig59 : MonoBehaviour
{
    //수정//////////////////////////////////////////////////////////////////    Dialogue or Select ( 각 항목 개수는 무제한이고 단 Select에서 선택지 4개가 최대 )
    //public static Trig27 instance;
    public static Trig59 instance;
    public int trigNum;

    // [Header ("텐드 진입 전")]
    // public Select select_1;
    // [Header ("거미줄 완료 후")]
    // public Select select_2;
    [Header ("처음 대사")]
    public Dialogue dialogue_0;
    public Select select_0;
    public Dialogue dialogue_1;
    [Header ("게임 성공 후")]
    public Dialogue dialogue_2;

    public GameObject[] mats = new GameObject[4];
    public GameObject centerView;
    //public GameObject centerView;
    public int refuseCount;
    

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
    protected bool flag;    // true 이면 다시 실행 안됨.
    
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
        instance = this;
        theDM = DialogueManager.instance;
        theSelect = SelectManager.instance;
        theOrder = OrderManager.instance;
        thePlayer = PlayerManager.instance;
        theDB = DatabaseManager.instance;
        theCamera = CameraMovement.instance;
        theMap= MapManager.instance;
        thePuzzle= PuzzleManager.instance;
        // if(theDB.trigOverList.Contains(11)){
        //     flag = false;
        //     trig37.SetActive(true);
        // }
        //else flag = true;

        
        if(theDB.trigOverList.Contains(trigNum)){//트리거 실행 후 맵에 다시 돌아왔을 때 DB list에 들어가 있으면 다시 실행 안됨.
            flag = true;
            if(repeatBifur!=0){
                flag = false;
                bifur = repeatBifur;
            }
        }
        // if(theDB.gameOverList.Contains(22)){
        // }
        // else 
        if(theDB.gameOverList.Contains(21)){
            mats[0].SetActive(false);
            mats[1].SetActive(false);
            mats[2].SetActive(false);
            mats[3].SetActive(true);
        }
        else if(theDB.gameOverList.Contains(20)){
            mats[0].SetActive(false);
            mats[1].SetActive(false);
            mats[2].SetActive(true);
        }
        else if(theDB.gameOverList.Contains(19)){
            mats[0].SetActive(false);
            mats[1].SetActive(true);
        }

    }

    /////////////////////////////////////////////////////////////////////   해당 위치에서 1. 실행전이고, 2. 키입력시 트리거 발생
    private void OnTriggerStay2D(Collider2D collision){
        //if(theDB.trigOverList.Contains(24)&&!GameManager.instance.playing&&!theDB.gameOverList.Contains(6)){

        if(!theDB.gameOverList.Contains(22)&&!thePlayer.isPlayingGame){

    
           
            if(!thePlayer.exc.GetBool("on")&&!flag&&!autoEnable&&(thePlayer.canInteractWith==0||thePlayer.canInteractWith==trigNum)){
                thePlayer.exc.SetBool("on",true);
                thePlayer.canInteractWith = trigNum;
            }
            if(collision.gameObject.name == "Player" && !flag && !autoEnable && (Input.GetKeyDown(KeyCode.Space)||thePlayer.getSpace)&& !theDM.talking &&thePlayer.canInteractWith==trigNum){
                flag = true;
                thePlayer.exc.SetBool("on",false);
                thePlayer.canInteractWith = 0;
                StartCoroutine(EventCoroutine());
            }

            if(collision.gameObject.name == "Player" && !flag && autoEnable&& !theDM.talking){
                flag = true;
                StartCoroutine(EventCoroutine());
            } 
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
    


        if(bifur==0){
        //theCamera.GetComponent<CameraMovement>().moveSpeed=5f;
        theCamera.GetComponent<CameraMovement>().target=centerView;
            theDM.ShowDialogue(dialogue_0);
            yield return new WaitUntil(()=> !theDM.talking);
            theSelect.ShowSelect(select_0);
            yield return new WaitUntil(()=> !theSelect.selecting);
            if(theSelect.GetResult()==0){
                bifur=1;
                theDM.ShowDialogue(dialogue_1);
                yield return new WaitUntil(()=> !theDM.talking);            
                Fade2Manager.instance.FadeOut();
                yield return new WaitForSeconds(1f);
                GameManager.instance.game19.SetActive(true);
                thePlayer.isPlayingGame = true;            
                Fade2Manager.instance.FadeIn();
        //theCamera.GetComponent<CameraMovement>().moveSpeed=5f; 
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
            else{
//#if ADD_ACH
            
                refuseCount ++;
                if(refuseCount==3){
                    Debug.Log("업적17");        
                    if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(17);
                }
            
//#endif
                thePlayer.notMove= false;
                    flag=false;
            }
        theCamera.GetComponent<CameraMovement>().target=thePlayer.gameObject;
            //trig40.SetActive(true);
        }
        else{
            Fade2Manager.instance.FadeOut();
            yield return new WaitForSeconds(1f);
            GameManager.instance.game19.SetActive(true);
            thePlayer.isPlayingGame = true;            
            Fade2Manager.instance.FadeIn();
                if(onlyOnce)
                    theDB.trigOverList.Add(trigNum);
                if(preserveTrigger)
                    flag=false;
                if(repeatBifur!=0){
                    theDB.trigOverList.Add(trigNum);
                    flag=false;
                    bifur=repeatBifur;
                }

            // theSelect.ShowSelect(select_0);
            // yield return new WaitUntil(()=> !theSelect.selecting);
            // if(theSelect.GetResult()==0){
            //     bifur=1;
            //     theDM.ShowDialogue(dialogue_1);
            //     yield return new WaitUntil(()=> !theDM.talking);            
            //     Fade2Manager.instance.FadeOut();
            //     yield return new WaitForSeconds(1f);
            //     GameManager.instance.game19.SetActive(true);
            //     thePlayer.isPlayingGame = true;            
            //     Fade2Manager.instance.FadeIn();
            // }
            // else{
            //     thePlayer.notMove= false;
            // }
            // Fade2Manager.instance.FadeOut();
            // yield return new WaitForSeconds(1f);
            // GameManager.instance.game19.SetActive(true);
            // thePlayer.isPlayingGame = true;
            // Fade2Manager.instance.FadeIn();
        }
        // else{
        //     theDM.ShowDialogue(dialogue_1);
        //     yield return new WaitUntil(()=> !theDM.talking);
        // }

        //theOrder.Move(); 
            

    }

    // public IEnumerator FinishGame(){
    //         //yield return new WaitForSeconds(1f);
    //         // theDM.ShowDialogue(dialogue_2);
    //         // yield return new WaitUntil(()=> !theDM.talking);
    //         PaperManager.instance.letter0.gameObject.SetActive(true);
    //         PaperManager.instance.letterBtnUpdate = true;//.SetBool("flash",true);
    //         BookManager.instance.ActivateUpdateIcon(0);
    //         yield return null;
    //         //PaperManager.instance.letterBtn.gameObject.SetActive(true);
    // }
    
}

