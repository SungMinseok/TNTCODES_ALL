using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideo : MonoBehaviour
{
    public VideoPlayer theVideo;
    public GameObject skipBtn;
    public GameObject skipBtn_DEV;
    [Header("0: 도망, 1: 눈")]
    public VideoClip[] videoClips;
    public bool isPlaying;
    void Start()
    {   
        if(DebugManager.instance!=null&&DebugManager.playerInfo.count_clear>=1){
            skipBtn.SetActive(true);
        }
        else{
            skipBtn.SetActive(false);
        }
#if DEV_MODE

        skipBtn_DEV.SetActive(true);
#endif
        StartCoroutine(Wait()); 
    }


    IEnumerator Wait(){
        StartVideo(0);
        
        yield return new WaitUntil(()=> !theVideo.isPrepared);
        Fade2Manager.instance.FadeIn(0.03f);

        yield return new WaitUntil(()=> !isPlaying);
        Fade2Manager.instance.FadeOut(0.03f);
            yield return new WaitForSeconds(2f);
        StartVideo(1);
        
        yield return new WaitUntil(()=> !theVideo.isPrepared);
        Fade2Manager.instance.FadeIn(0.03f);

        yield return new WaitUntil(()=> !isPlaying);
        Fade2Manager.instance.FadeOut(0.03f);
            yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Loading");
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)&&Input.GetKeyDown(KeyCode.Q)){
            SkipButton();
        }
    }

    public void SkipButton(){
        theVideo.Stop();
        AudioManager.instance.Play("button22");
        SceneManager.LoadScene("Loading");
    }
    public void SkipButtonDEVMODE(){
        theVideo.Stop();
        AudioManager.instance.Play("button22");
        SceneManager.LoadScene("start");
    }
    public void StartVideo(int videoNum)
    {
        isPlaying = true;
        if(videoClips[videoNum]!=null) theVideo.clip = videoClips[videoNum];
        theVideo.Play();
        theVideo.loopPointReached += OnMovieFinished;
    }
    void OnMovieFinished(VideoPlayer player)
    {
        player.Pause();
        isPlaying = false;
    }
}
