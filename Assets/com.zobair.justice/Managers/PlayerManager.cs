using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
 

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerManager>();

                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<PlayerManager>();
                }
            }
            return _instance;
        }
    }


    
    public GameObject player;
    
    
     

    private void Awake()
    {
        if (_instance != null)
            Destroy(this);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        if (player == null)
            player = GameObject.Find("Player");
    }

    private void Start()
    {
         
    }
}
