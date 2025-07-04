using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int MaxNumberOfShots = 3;
    private int _usedNumberOfShots;
    public static GameManager instance;

    public void Awake(){
        if(instance == null) instance = this;
    }

    public void UseShot(){
        _usedNumberOfShots++;
    }

    public bool hasEnoughShots(){
        return _usedNumberOfShots < MaxNumberOfShots;
    }
}
