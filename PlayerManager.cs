using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerState{
    [Header ("신발")]public bool shoes;
}
public class PlayerManager : MovingObject
{
    public static PlayerManager instance; //static은 정적변수, 해당스크립트가 적용된 모든객체들은 static으로 선언된 변수의 값을 공유
    
    [HideInInspector]public Transform pointInMaze;
    public Animator exc;
    public SpriteRenderer shadow_normal;
    public SpriteRenderer shadow_laydown;
    public SpriteRenderer shadow_climbing;
    public GameObject shadow_swim;
    public BoxCollider2D normalCol;
    public BoxCollider2D fishCol;
    public int life;
    public bool onMud;
    public bool onPond;



    public bool isGameOver;
    public bool isDemoOver;
    public bool devMode;
    [HideInInspector]public bool finishGame;
    public bool notMove= true;
    public bool isInteracting=false;  //대화중 메뉴 안켜지게 하려고
    public int canInteractWith;  //실행 대기 트리거 넘버

    //[HideInInspector]public bool canInteract=false;  //트리거 박스 들어가면 true(중복실행 방지)

    [Header ("처음 시작 시 대사")]
    public Dialogue dialogue;

    [Header ("자동 저장 시 대사")]
    public Dialogue dialogue1;
    public string currentMapName;
    public string lastMapName;
    
    public bool isChased;
    public bool isTransporting; //언노운 같이 이동하기위해서.
    public bool isTransporting2; //이동효과를 위해서.
    public bool isHalting;
    public bool isGrabbing;
    public bool isWatching;
    public bool isClimbing;
    public bool isFalling;
    
    public bool underDarkness;//어둡게
    
    public Color shadowColor;
    
    public Color normalColor;
    public int mazeNum;//미로에서 위치



    public bool isPlayingPuzzle;    //퍼즐창 온이면 트루
    public bool isPlayingGame;    //퍼즐창 온이면 트루
    public bool autoSave; // 강제 저장...
    public bool isWakingup=true;
    // public bool movingX;
    // public bool movingY;

    [HideInInspector]public PositionRendererSorter sorter;
    //public int backUpSorter;
    
    private SaveNLoad theSL;
    private AudioManager theAudio;
    [HideInInspector]public SpriteRenderer spriteRenderer;
    //[HideInInspector]public bool debuggingMode;



    public int milkRemain;  //우유먹었을때 초기화 넘버
    public int shoe0Remain;  //우유먹었을때 초기화 넘버
    public int shoe1Remain;  //우유먹었을때 초기화 넘버
    public int transferMapCount;   //맵 이동 횟수.
    public int[] mapCheckList = new int[20];

    void Start(){
        theSL = FindObjectOfType<SaveNLoad>();
        theAudio = FindObjectOfType<AudioManager>();
        if(!DebugManager.instance.onDebug) boxCollider.enabled =false;
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //queue = new Queue<string>();
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }

        ResetColor();

    }
    public void LetBegin()                          //게임 로드시 일어나는 애니메이션을 위해
    {
        //if(!debuggingMode)
        //if(currentMapName!="lakein")
            StartCoroutine(WakingUp());      //맨처음

    }
    
    void Update()//여기에는 input넣고 fixedupdate에는 움직임movement넣기
    {
        if(!notMove){
            if(autoSave){
                StartCoroutine(AutoSave());
            }
            if(isInteracting){
                notMove = true;
            }
            // if(BookManager.instance.onButton.activeSelf){
            //     BookManager.instance.onButton.GetComponent<Button>().interactable = false;
            // }
            // if(Input.GetAxisRaw("Vertical")==0) movingY = false;
            // if(Input.GetAxisRaw("Horizontal")==0) movingX = false;
            // if(Input.GetAxisRaw("Vertical")!=0) movingY = true;
            // if(Input.GetAxisRaw("Horizontal")!=0) movingX = true;
            
            // if(movingY && )

            // if(Input.GetAxisRaw("Horizontal")!=0 ||Input.GetAxisRaw("Vertical")!=0){
            //     movement.Set(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));


            //     //movement.Set(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            //     if(movement.x !=0) movement.Set(Input.GetAxisRaw("Horizontal"), 0);
            //     else if(movement.y !=0) movement.Set(0,Input.GetAxisRaw("Vertical"));
            // } 

            if(Input.GetAxisRaw("Vertical")!=0 /* || Input.GetAxisRaw("Vertical")!=0 && Input.GetAxisRaw("Horizontal")!=0 */){    //상하 키 누른 상태
                // if(Input.GetAxisRaw("Horizontal")!=0){
                        
                //     movement.x = Input.GetAxisRaw("Horizontal");
                //     movement.y = 0;
                //     Debug.Log("a");
                // }
                // else{
                    //movingY = true;

                movement.x = 0;
                movement.y = Input.GetAxisRaw("Vertical");
//                Debug.Log("b");
                // }
            }
            else if(Input.GetAxisRaw("Horizontal")!=0){
                // if(Input.GetAxisRaw("Vertical")!=0){
                        
                //     movement.x = 0;
                //     movement.y = Input.GetAxisRaw("Vertical");
                //     Debug.Log("c");
                // }
                // else{
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = 0;
                //Debug.Log("d");
                // }
            }




            if(Input.GetAxisRaw("Horizontal")==0&&Input.GetAxisRaw("Vertical")==0){
                movement.x = 0;
                movement.y = 0;
            }


            //animator.SetFloat("Speed", movement.sqrMagnitude);
            
            if(Input.GetKey(KeyCode.LeftShift) && movement!=Vector2.zero && !animator.GetBool("sad")){
                rb.MovePosition(rb.position + movement * runSpeed * Time.fixedDeltaTime);
                animator.SetFloat("Speed", 2f);
                // Debug.Log(movement);
                // Debug.Log(runSpeed);
                // Debug.Log(Time.fixedDeltaTime);
                // Debug.Log(movement * runSpeed * Time.fixedDeltaTime);
            }
            else if(movement!=Vector2.zero){
                rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                animator.SetFloat("Speed", 1f);
                // Debug.Log("1" +movement);
                // Debug.Log("2" +runSpeed);
                // Debug.Log("3" +Time.fixedDeltaTime);
                // Debug.Log(movement * runSpeed * Time.fixedDeltaTime);
            }
            else if(movement==Vector2.zero){
                animator.SetFloat("Speed", 0f);
            }
        }
        // else{

        //     if(BookManager.instance.onButton.activeSelf){
        //         BookManager.instance.onButton.GetComponent<Button>().interactable = true;
        //     }
        // }





        if(isGameOver){
            animator.SetFloat("Speed", 0);
        }

    }
    void FixedUpdate(){
        if(!notMove){

        // //이동시 위치 설정
        //     if(Input.GetKey(KeyCode.LeftShift)){
        //         rb.MovePosition(rb.position + movement * (speed*2) * Time.fixedDeltaTime);
        //     }
        //     else rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        
        //이동완료 후 방향 고정
            //movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            movementDirection = new Vector2(movement.x, movement.y);
            if (movementDirection != Vector2.zero){
                animator.SetFloat("Horizontal", movementDirection.x);
                animator.SetFloat("Vertical", movementDirection.y);
            }

        }

        // if(canInteract&&!exc.GetBool("on")){
        //     exc.SetBool("on",true);
        // }
    //번쩍임
        // if(currentMapName=="ch2"&&!underDarkness){
        //     underDarkness = true;
        //     spriteRenderer.color = shadowColor;
        // }
        // else{
        //     underDarkness = false;
        //     spriteRenderer.color = normalColor;
        // }
        // if(currentMapName=="ch2"){
        //     //underDarkness = true;
        //     spriteRenderer.color = shadowColor;
        // }
        
        // else{
        //     //underDarkness = false;
        //     spriteRenderer.color = normalColor;
        // }
        // switch(currentMapName){
        //     case "ch2" : 
        //         spriteRenderer.color = shadowColor;
        //         break;
        //     case "lakein" : 
        //         spriteRenderer.color = shadowColor;
        //         if(!animator.GetBool("onFish")){
        //             animator.SetBool("onFish", true);
        //             FadeManager.instance.fog0.SetActive(true);

        //         }
        //         break;
        //     case "lake" : 
        //         spriteRenderer.color = normalColor;
        //         break;
        //     case "lakeout" : 
        //         spriteRenderer.color = normalColor;
        //         break;
        //     default :
        //         spriteRenderer.color = normalColor;
        //         break;
        // }


        if(animator.GetBool("onFish")){
            shadow_swim.SetActive(true);
            shadow_normal.gameObject.SetActive(false);
            fishCol.gameObject.SetActive(true);
            //normalCol.enabled=false;
        }
        else if(isClimbing){
            
                shadow_normal.gameObject.SetActive(false);
            shadow_climbing.gameObject.SetActive(true);
        }
        else if(isFalling){
            
                shadow_climbing.gameObject.SetActive(false);
                shadow_normal.gameObject.SetActive(false);
        }
        else{
            shadow_swim.SetActive(false);
            fishCol.gameObject.SetActive(false);
            shadow_climbing.gameObject.SetActive(false);
            if(!isWakingup){

                //normalCol.enabled=true;
                shadow_normal.gameObject.SetActive(true);
            }

        }
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.grab")&&!isGrabbing)
        {            
           // Do something
           isGrabbing = true;
//           Debug.Log("정지");
           notMove = true;
           StartCoroutine(Grabbing());
        }        



        // if(isWakingup){
        //     boxCollider.enabled =false;
        // }

        if(isChased){
            canInteractWith = -10;
            exc.SetBool("on",false);
        }


        // if(nearMachine){
        //     theAudio.Play("machine0");
        // }
        // else{

        //     theAudio.Stop("machine0");
        // }
        // if(nearFire){
        //     theAudio.Play("bonfire");
        // }
        // else{

        //     theAudio.Stop("bonfire");
        // }

    }
    public IEnumerator ChangeColor(){
        yield return new WaitForSeconds(1f);
        switch(currentMapName){
            case "ch2" : 
                spriteRenderer.color = shadowColor;
                break;
            case "lakein" : 
                spriteRenderer.color = shadowColor;
                if(!animator.GetBool("onFish")){
                    animator.SetBool("onFish", true);
                    FadeManager.instance.fog0.SetActive(true);

                }
                break;
            case "lake" : 
                spriteRenderer.color = normalColor;
                break;
            case "lakeout" : 
                spriteRenderer.color = normalColor;
                break;
            default :
                spriteRenderer.color = normalColor;
                //if(animator.GetBool("onFish")){
                //}
                break;
        }
    }

    public IEnumerator WakingUp(){
        CheckPassive();
        ChangeColor();
        Fade2Manager.instance.FadeIn(0.01f);
        notMove = true;

        if(currentMapName!="lakein"){

            animator.SetBool("wake_up",true);
            yield return new WaitForSeconds(2f);
            shadow_laydown.GetComponent<Animator>().SetTrigger("off");
            shadow_normal.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.3f);
            animator.SetBool("wake_up",false);
            shadow_laydown.gameObject.SetActive(false);
        }
        else{
            shadow_laydown.gameObject.SetActive(false);
        }
        //CheckPassive();
        //exc.SetTrigger("on");

        if(!DatabaseManager.instance.doneIntro){                //아주 처음 대화창 띄우기
            DatabaseManager.instance.doneIntro=true;
            DialogueManager.instance.ShowDialogue(dialogue);
            yield return new WaitUntil(()=> !DialogueManager.instance.talking); 
            //DialogueManager.instance.RenderTest();
        }                             
        notMove = false;
        isWakingup = false;
        boxCollider.enabled =true;

        //HotkeyManager.instance.PopUpHelp();
    }

    public void walkSoundStart(){
        if(!isTransporting2){
            
            if(onMud){
                
                theAudio.Play("mud"+Random.Range(0,6).ToString());
            }

            else if(onPond){
                
                theAudio.Play("pond"+Random.Range(0,4).ToString());
            }

            else{
                        
                string temp = Random.Range(0,4).ToString();
                
                theAudio.Play("walkforest"+temp);
            }





        }
        //StartCoroutine(walkSoundCoroutine(temp));
    }
    public void ClimbSoundStart(){
        string temp = Random.Range(0,4).ToString();
        
        theAudio.Play("climb"+temp);
        //StartCoroutine(walkSoundCoroutine(temp));
    }
    public void SwimSoundStart(){
        string temp = Random.Range(0,4).ToString();
        
        theAudio.Play("swim"+temp);
        //StartCoroutine(walkSoundCoroutine(temp));
    }
    public void ClimbHalt(){
        isHalting = !isHalting;
    }
/*
    IEnumerator walkSoundCoroutine(int temp){
        string num = temp.ToString();
        theAudio.Play("walkforest"+num);
        yield return new WaitForSeconds(0.5f);
        walkSoundStart();
    }
    public void walkSoundStop(){
        StopCoroutine(walkSoundCoroutine(0));
        //theAudio.Stop(walkSound_1);
    }
    */


    IEnumerator Grabbing(){
        yield return new WaitForSeconds(1.5f);
        if(!isInteracting) notMove = false;
        isGrabbing = false;
    }

    public void ResetPlayer(){
        lastMapName = "";
        //ResetTerrainState();
        onMud = false;
        onPond = false;
        theAudio.Stop("machine0");
        theAudio.Stop("bonfire");
        
    }

    IEnumerator AutoSave(){
        autoSave = false;
        DialogueManager.instance.ShowDialogue(dialogue1);
        yield return new WaitUntil(()=> !DialogueManager.instance.talking); 
        
        BookManager.instance.BookOn();
        BookManager.instance.OnSetting();
    }

    public void CheckPassive(){
        if(currentMapName=="lakein"){
            animator.SetBool("onFish",true);
            ObjectManager.instance.ImageFadeIn(FadeManager.instance.fog0.GetComponent<Image>(),0.015f);
        }
        else{
            
            animator.SetBool("onFish", false);
            FadeManager.instance.fog0.SetActive(false);
        }
        if(Inventory.instance.SearchItem(14)){
            life =1;
        }
        if(DatabaseManager.instance.caughtCount>=5) Inventory.instance.GetItem(16);
    }

    public void ResetColor(){
        shadowColor = new Color(0.7f,0.7f,0.7f,1f);
        normalColor = new Color(1f,1f,1f,1f);
    }

    public void MapCountCheck(string mapName){
        
        switch(mapName){
            case "start" :
                mapCheckList[0] +=1;
                break;
            case "cabin" :
                mapCheckList[1] +=1;
                break;
            case "catwood" :
                mapCheckList[2] +=1;
                break;
            case "catwood2" :
                mapCheckList[3] +=1;
                break;
            case "ch2" :
                mapCheckList[4] +=1;
                break;
            case "ch3" :
                mapCheckList[5] +=1;
                break;
            case "cornerwood" :
                mapCheckList[6] +=1;
                break;
            case "camp" :
                mapCheckList[7] +=1;
                break;
            case "middlewood" :
                mapCheckList[8] +=1;
                break;
            case "village" :
                mapCheckList[9] +=1;
                break;
            case "lake" :
                mapCheckList[10] +=1;
                break;
            case "lakein" :
                mapCheckList[11] +=1;
                break;
            case "lakeout" :
                mapCheckList[12] +=1;
                break;
            case "rainingforest" :
                mapCheckList[13] +=1;
                break;
            case "parrothidden" :
                mapCheckList[14] +=1;
                break;
            case "thunderingforest" :
                mapCheckList[15] +=1;
                break;
            case "mazein" :
                mapCheckList[16] +=1;
                break;
            case "maze" :
                mapCheckList[17] +=1;
                break;
            case "mazeout" :
                mapCheckList[18] +=1;
                break;
            case "end" :
                mapCheckList[19] +=1;
                break;
            
        }
    }

}
