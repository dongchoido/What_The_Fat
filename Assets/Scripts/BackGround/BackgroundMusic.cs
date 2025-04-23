using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic backgroundmusic;
    void Awake()
    {
        if(backgroundmusic == null){
            backgroundmusic = this;
            DontDestroyOnLoad(backgroundmusic);
        }
        else{
            Destroy(gameObject);
        }
    }
   
}
