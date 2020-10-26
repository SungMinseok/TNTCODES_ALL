using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class Slot3 : MonoBehaviour, IDropHandler
{    
    public Control3 control;
    public int num;
    public bool check;

    public void OnDrop(PointerEventData eventData){


            control.blocks[control.temp].check = true;  //퍼즐에 들어갔는지 체크
        //1 들어온 블록에 연결된 번호 일단 저장
        // getNum = control.blocks[control.temp].linkedNum;    //  0, 1, 2

        
            control.blocks[control.temp].nowNum = num;
           
        //2 블록이 들어간 퍼즐 번호 다시 지정해주고, 체크 온    (여기서 인덱스 초과 오류)

    //맞는 블록이 들어오면 트루.
        
        //if(num==control.temp) control.slots[num].check = true;    //해당 슬롯 채워짐


        if(eventData.pointerDrag != null){
            //eventData.pointerDrag.transform.position = transform.position;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition
            =GetComponent<RectTransform>().anchoredPosition;
        }
        int ranNum = UnityEngine.Random.Range(0,4);
        
        AudioManager.instance.Play("hammer"+ranNum.ToString());
        control.CheckGame();

    }
}
