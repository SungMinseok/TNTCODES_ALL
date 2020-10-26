using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class InGameVideo : MonoBehaviour
{
    public static InGameVideo instance;
    public VideoPlayer theVideo;
    [Header("0: 진엔딩, 1: 크레딧, 2: 끝로고, 3: 헨리영상")]
    public VideoClip[] videoClips;
    public GameObject rawImage;
    public GameObject backGround;
    
    public bool isPlaying;
    PlayerManager thePlayer;

    #region
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
        //instance = this;
    }
    #endregion

    void Start(){
        thePlayer = PlayerManager.instance;
    }
    public void StartVideo(string VideoName)
    {
        isPlaying = true;
        thePlayer.isWatching = true;
        backGround.SetActive(true);
        rawImage.SetActive(true);
        switch(VideoName){
            case "TrueEnding" : 
                theVideo.clip = videoClips[0];
                break;
            case "Credit" : 
                theVideo.clip = videoClips[1];
                break;
            case "LastLogo" : 
                theVideo.clip = videoClips[2];
                break;
            case "Henry" : 
                theVideo.clip = videoClips[3];
                break;
        }
        theVideo.gameObject.SetActive(true);
        theVideo.Play();
        theVideo.loopPointReached += OnMovieFinished;
    }
    void OnMovieFinished(VideoPlayer player)
    {
        //Debug.Log("Event for movie end called");
        isPlaying = false;
        player.Pause();
    }
    public void ExitVideo(){

        thePlayer.isWatching = false;
        backGround.SetActive(false);
        rawImage.SetActive(false);
        theVideo.gameObject.SetActive(false);
    }
}
