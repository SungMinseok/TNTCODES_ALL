using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
public class SteamAchievement : MonoBehaviour
{
    public static SteamAchievement instance;
    public bool unlockTest = false;
    public CGameID m_GameID;
    public AppId_t appID;
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
    }
    void Start()
    {
        appID = SteamUtils.GetAppID();
        m_GameID = new CGameID(SteamUtils.GetAppID());
    }
    // public void UnlockSteamAchievement(int num){
    //     TestSteamAchievement(num);
    //     if(!unlockTest){

    //     }
    // }

    public void ApplyAchievements(int num){
        if(!SteamManager.Initialized) { return ; }

        TestSteamAchievement(num);
        if(!unlockTest){
                    
            SteamUserStats.SetAchievement("NEW_ACHIEVEMENT_1_"+num.ToString());
            SteamUserStats.StoreStats();
        }
    }
    public void DEBUG_LockSteamAchievement(int num){
        
        TestSteamAchievement(num);
        if(unlockTest){
            SteamUserStats.ClearAchievement("NEW_ACHIEVEMENT_1_"+num.ToString());
        }
    }
    public bool GetSteamAchievementStatus(int num){
        TestSteamAchievement(num);
        return unlockTest;
    }

    void Update(){
        //if(!m_bInitialized){

        //}
        SteamAPI.RunCallbacks();
    }

    void TestSteamAchievement(int num){
        SteamUserStats.GetAchievement("NEW_ACHIEVEMENT_1_"+num.ToString(), out unlockTest);
    }
}
