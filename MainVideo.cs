using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class MainVideo : MonoBehaviour
{
    // Start is called before the first frame up
    public GameObject menu;
    public VideoPlayer theVideo;
    public Animator animator;
    public int playMusicTrack;



    BGMManager BGM;
    void Start()
    {
        menu.SetActive(false);
        //theVideo.Play();
        StartCoroutine(Wait());    
        BGM = FindObjectOfType<BGMManager>();

    }

    IEnumerator Wait(){
        yield return new WaitForSeconds(0.01f);
        //Fade2Manager.instance.FadeIn(0.01f);
        // if(MainMenu.instance.logoOn){
        //     MainMenu.instance.logo.SetActive(true);
        //     yield return new WaitForSeconds(5f);
        // }
        theVideo.Play();
        yield return new WaitForSeconds(0.1f);
        BGM.Play(playMusicTrack);

        yield return new WaitForSeconds(4.2f);
        menu.SetActive(true);
        animator.SetBool("Appear",true);
        //yield return new WaitForSeconds(1.5f);
        
        //animator.SetBool("Swing",true);
        
        // yield return new WaitForSeconds(15f);
        // theVideo.Pause();
        
    }

}
