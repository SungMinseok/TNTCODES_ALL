using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game26 : MonoBehaviour
{
    public static game26 instance;
    public BoxCollider2D[] bounds;
    public int moveCount;

    void Start(){
        instance = this;

        CameraMovement.instance.SetBound(bounds[PlayerManager.instance.mazeNum]);
    }

    public void SetBoundInMaze(){
        
        if(PlayerManager.instance.mazeNum!=0){
        }
    }
}
