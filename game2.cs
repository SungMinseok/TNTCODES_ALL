using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class game2 : MonoBehaviour
{
    
    public Puzzle1 Puzzle1;
    public Control control;
    //private Puzzle1 thePuzzle;
    private GameManager theGame;
    private DatabaseManager theDB;
    private PuzzleManager thePuzzle;
    private PlayerManager thePlayer;
    void Start()               
    {
        //instance = this;
        theGame= GameManager.instance;
        theDB = DatabaseManager.instance;
        thePuzzle = PuzzleManager.instance;
        thePlayer = PlayerManager.instance;
        //thePuzzle = Puzzle1.instance;

        thePlayer.isPlayingGame = true;

    }
    public void exitGame(){
        Puzzle1.instance.inMain = true;
        if(theDB.puzzleOverList.Contains(1)) Puzzle1.instance.SpritesOn();
            AudioManager.instance.Play("button20");
        gameObject.SetActive(false);
        //thePuzzle.treeFace.SetActive(true);
        Puzzle1.buttonOn();
        //thePuzzle.StopAllCoroutines();

    }
    public void passGame(){

        Puzzle1.instance.inMain = true;
        Puzzle1.buttonOn();
        //thePuzzle.treeFace.SetActive(true);
        
        gameObject.SetActive(false);
            thePlayer.isPlayingGame = false;
            //theGame.pass[0] = true;
            //theDB.gameOverList.Add(0);
        //theGame.pass[2] = true;
        
        theDB.gameOverList.Add(2);

        Puzzle1.FinishGame();
    }
    public void ResetGame(){
            AudioManager.instance.Play("button20");
        for(int i=0;i<control.blocks.Length;i++){
            control.blocks[i].enabled = true;
            //if(control.blocks[i].nowNum!=-1){
                    
                control.blocks[i].Relocate();
                control.blocks[i].nowNum=-1;
                control.blocks[i].check=false;
            //}

            if(!control.blocks[i].fixedBlock){
                
                control.slots[i].check=false;
            }
            else{
                
                control.blocks[i].enabled = false;
            }
        }
    }
}
