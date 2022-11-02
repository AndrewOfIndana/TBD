using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//stores the cards scritable objects in array
public class CardDatabase
{
    public static Card[] Cards { get; private set; }
   
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Initialize() => Cards = Resources.LoadAll<Card>("Scriptable Objects/Cards/");
}
