using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class DataAdapter : MonoBehaviour
{
    public static DataAdapter GameData;
    public GameController gameController;
    public List<Button> buttons;
    public GameDifficulty difficulty;
    public PlayerType enemyType;
    void Awake()
    {
        if(GameData==null)
        {
            DontDestroyOnLoad(gameObject);
            GameData = this;
        }
        else if(GameData!=this)
        {
            Destroy(gameObject);
        }
    }
}
