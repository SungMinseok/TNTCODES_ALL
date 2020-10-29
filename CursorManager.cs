using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
     private void Awake()



    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else{
            Destroy(this.gameObject);
        }


        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 30;
#if !UNITY_EDITOR
        SetCursorState(2);
#endif
    }
    
    public Texture2D defaultCursor;
    public Texture2D activatedCursor;
    public Texture2D grabCursor;
    public Texture2D interactableCursor;
    public Texture2D interactableGrabCursor;
    public bool _default;       //true면 기본 커서
    public bool interactable;
    //public bool canGrab;

    //private bool onCheck;
    //private bool onUsing;
    // Start is called before the first frame update
    private DatabaseManager theDB;
    PlayerManager thePlayer;
    void Start()
    {
        theDB=DatabaseManager.instance;
        //thePlayer=PlayerManager.instance;
        _default = true;
        //Cursor.visible =false;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update(){
        if(thePlayer==null){
            thePlayer = PlayerManager.instance;
        }
        
        //if(theDB.OnActivated[0]!=null){
            if(Input.GetMouseButtonUp(1)&& !_default){  //템선택 상태에서 오른쪽 버튼 클릭하면 복구
                RecoverCursor();
                for(int i=0; i<theDB.OnActivated.Length; i++)
                    theDB.OnActivated[i]=false;
            }

            if(Input.GetMouseButtonDown(0)&&!_default){
                SetToCursor(grabCursor);
            }
            if(Input.GetMouseButtonUp(0)&&!_default){
                SetToCursor(activatedCursor);
            }

        //}
        if(interactable&&_default&&!thePlayer.isInteracting){
                
            //Cursor.SetCursor(interactableCursor, Vector2.zero, CursorMode.ForceSoftware);
            
            if(Input.GetMouseButton(0)){
                SetToCursor(interactableGrabCursor);
            }
            // else if(Input.GetMouseButtonUp(0)){
            //     SetToCursor(interactableCursor);
            // }
            else{

                SetToCursor(interactableCursor);
            }
        }
        else if(_default){

            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
    
    public void RecoverCursor(){
        _default = true;
                
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        //theDB.OnActivated[0] = false;
        theDB.DeactivateItem();
        Debug.Log("마우스커서 복구");
    }
    public void SetToCursor(Texture2D ct){
        //_default = true;
                
        Cursor.SetCursor(ct, Vector2.zero, CursorMode.ForceSoftware);

        //Debug.Log("마우스커서 복구");
    }
    public void SetCursorState(int num){
        if(num==0){//커서 정상
            Cursor.lockState = CursorLockMode.None;
        }
        else if(num==1){//커서 숨김
            Cursor.lockState = CursorLockMode.Locked;

        }
        else{//밖으로 못나가게
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    /*
    public void CursorWait(){
        StartCoroutine(CursorWaitCoroutine());
        
    }
    /*

    
    public void OnClickItemEvent(int num){
        //Debug.Log("??13131313????????");
        //Debug.Log(DatabaseManager.instance.OnActivated[num]);
        //onCheck = DatabaseManager.instance.OnActivated[num];
        //if(onCheck){
        //    Debug.Log("이건 어떄");
           //DatabaseManager.instance.OnActivated[0]=false;
           //DatabaseManager.instance.OnActivated[num]=false;
        //}
        /*
        
        if(DatabaseManager.instance.OnActivated!=null){
        if(DatabaseManager.instance.OnActivated[num])
            //theDB.ActivateItem(num)
           DatabaseManager.instance.OnActivated[0]=false;
           DatabaseManager.instance.OnActivated[num]=false;
           Debug.Log("아이템 사용중 클릭 인식 성공"); 

        }
        StartCoroutine(OnClickCoroutine(num));
    }

    IEnumerator OnClickCoroutine(int num){
        yield return new WaitForSeconds(0.2f);
        
        if(theDB.OnActivated[num]){
            theDB.OnActivated[num] = false;
            theDB.OnActivated[0] = false;
        }
    }*/
    // public void ActivateInteractable(){
    //     if(_default)
    //         Cursor.SetCursor(interactableCursor, Vector2.zero, CursorMode.ForceSoftware);
    // }
    // public void DeactivateInteractable(){
    //     if(_default)
    //         Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    // }
}
