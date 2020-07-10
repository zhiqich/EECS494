//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GameAnalyticsSDK;

//public class DataAnalytics : MonoBehaviour
//{
//    // Start is called before the first frame update

//    [HideInInspector] public bool isAllowed = false;
//    [HideInInspector] public bool isInitialized = false;

//    string[] characters = {"GA:Miner", "GA:Witch", "GA:Thief", "GA:Boomer"};

//    private void Awake()
//    {
//        Initialize();
//        // if (instance == null)
//        // {
//        //     instance = this;
//        // }
//        // else if (instance != this)
//        // {
//        //     Destroy(gameObject);
//        //     return;
//        // }
//        // DontDestroyOnLoad(gameObject);
//    }

//    public void Initialize()
//    {
//        GameAnalytics.Initialize();
//        isAllowed = !isAllowed;
//        // if (!instance.isInitialized) 
//        // {
//        //     GameAnalytics.Initialize();
//        //     instance.isInitialized = true;
//        // }
//        // instance.isAllowed = !instance.isAllowed;
//    }

//    public void SendData(int character, int score)
//    {
//        if (isAllowed)
//        {
//            GameAnalytics.NewDesignEvent(characters[character], score);
//            Debug.Log("send data to server");
//        }
//    }
//}
