using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MobileControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerClickHandler
{
    
#if UNITY_ANDROID || UNITY_IOS
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform bg;
    [SerializeField] private RectTransform js;
    public GameObject shiftBtn;
    public GameObject spaceBtn;
    public bool isTouch;
    private float r;
    public PlayerManager thePlayer;
    public GameObject mobileController;
    public bool isJoystick;
    public bool isShift;
    public bool isSpace;
    void Start()
    {
        //thePlayer = PlayerManager.instance;
        mobileController.SetActive(true);
        r = bg.rect.width * 0.5f;
    }

    void Update()
    {
        if(isJoystick){
            if(!thePlayer.notMove){
                
                if (isTouch)
                {
                    if(!thePlayer.isRunning){
                        thePlayer.rb.MovePosition(thePlayer.rb.position + thePlayer.movement * thePlayer.speed * Time.fixedDeltaTime);
                        thePlayer.animator.SetFloat("Speed", 1f);
                    }
                    else if(thePlayer.isRunning){
                        thePlayer.rb.MovePosition(thePlayer.rb.position + thePlayer.movement * thePlayer.runSpeed * Time.fixedDeltaTime);
                        thePlayer.animator.SetFloat("Speed", 2f);
                    }
                }
                else{
                    thePlayer.animator.SetFloat("Speed", 0f);

                }
            }
            else if(thePlayer.notMove){
                
                isTouch = false;
                js.localPosition = Vector3.zero;
                //thePlayer.animator.SetFloat("Speed", 0f);
            }
        }

    }
    public void OnDrag(PointerEventData eventData)
    {
        if(isJoystick){
            if(!thePlayer.notMove){
                js.anchoredPosition += eventData.delta / canvas.scaleFactor;
                js.anchoredPosition = Vector2.ClampMagnitude(js.anchoredPosition, r);

                Vector3 v = js.localPosition.normalized;

                if (v.y < 0.7 && v.y > 0)
                {
                    if (js.localPosition.x > 0)
                    {
                        thePlayer.movement.x = 1f;
                        thePlayer.movement.y = 0;
                    }
                    else if (js.localPosition.x < 0)
                    {
                        thePlayer.movement.x = -1f;
                        thePlayer.movement.y = 0;
                    }
                }
                else if (v.y >= 0.7)
                {

                    thePlayer.movement.x = 0;
                    thePlayer.movement.y = 1f;
                }

                else if (v.y > -0.7 && v.y < 0)
                {
                    if (js.localPosition.x > 0)
                    {
                        thePlayer.movement.x = 1f;
                        thePlayer.movement.y = 0;
                    }
                    else if (js.localPosition.x < 0)
                    {
                        thePlayer.movement.x = -1f;
                        thePlayer.movement.y = 0;
                    }
                }
                else if (v.y <= -0.7)
                {

                    thePlayer.movement.x = 0;
                    thePlayer.movement.y = -1f;
                }

                thePlayer.animator.SetFloat("Horizontal", thePlayer.movement.x);
                thePlayer.animator.SetFloat("Vertical", thePlayer.movement.y);

            }
        }
        

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(isJoystick){

            isTouch = true;
        }
        else if(isShift){
            thePlayer.isRunning = true;
        }
        else if(isSpace){
            thePlayer.getSpace = true;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if(isJoystick){
            isTouch = false;
            js.localPosition = Vector3.zero;
            thePlayer.animator.SetFloat("Speed", 0f);
        }
        else if(isShift){
            thePlayer.isRunning = false;
        }
        else if(isSpace){
            thePlayer.getSpace = false;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
    }
#endif

}
