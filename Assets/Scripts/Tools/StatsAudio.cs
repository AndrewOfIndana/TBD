using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //Allows user to create this 
public class StatsAudio : ScriptableObject
{
    /*  
        Name: Stats.cs
        Description: A ScriptableObject that holds a unit's audio clips
    
    */
    [Header("Audio Variables")]
    public AudioClip[] statSfx;
    public AudioClip deathSfx;

    public AudioClip GetRandomSfx()
    {
        return statSfx[Random.Range(0, statSfx.Length)];
    }
}
