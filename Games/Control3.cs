using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Control3 : MonoBehaviour
{
    public game5 game5;
    public bool activateRelocating;
    public Block3[] blocks;
    public Slot3[] slots;
    public GameObject[] vessels;

    public bool[] checkAll;

    public int temp;   //잡은 블록 번호

    //private byte t = 15;

    void Start()
    {
        checkAll = new bool[slots.Length];
        for(int i=0; i<25;i++){   //찐 블록 num = 0~24
            //blocks[i].num = i;
            slots[i].num = i;
            //slots[i].transform.position = blocks[i].transform.position;
        }
        for(int j=0; j<4; j++){
            blocks[j].num = j;
        }

            for(int a=0; a<blocks.Length; a++){
                //blocks[a].RelocateAtFirst();
                blocks[a].Relocate();
            }
        

    }
    // void FixedUpdate()
    // {
    //     for(int i=0; i<slots.Length; i++){
    //         checkAll[i]=slots[i].check;
    //     }
    // }

    public void CheckGame(){
        // for(int i=0; i<slots.Length; i++){
        //     if(slots[i].check){

        //         checkAll[i] = slots[i].check;
        //     }
        //     else return;
            
        // }
        if(blocks[0].nowNum == 5 &&blocks[1].nowNum == 3 &&blocks[2].nowNum == 17 &&blocks[3].nowNum == 24 ){

            Debug.Log("퍼즐 완성");

            //game5.passGame();
            AudioManager.instance.Play("success0");
            GameManager.instance.GameSuccessTrig();
            game5.Invoke("passGame",GameManager.instance.successWaitTime);
        }
//#if ADD_ACH
        else if(!DatabaseManager.instance.gameOverList.Contains(29)&&blocks[0].nowNum == 1 &&blocks[1].nowNum == 15 &&blocks[2].nowNum == 13 &&blocks[3].nowNum == 24 ){
            DatabaseManager.instance.gameOverList.Add(29);
            Debug.Log("업적10");
            if(SteamAchievement.instance!=null) SteamAchievement.instance.ApplyAchievements(10);
        }
//#endif

    }
}
