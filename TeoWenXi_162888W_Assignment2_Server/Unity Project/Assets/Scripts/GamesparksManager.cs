﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesparksManager : MonoBehaviour
{
    public static GamesparksManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
