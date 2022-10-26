using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Attribute { fire, ice, lightning, poision, grow };// the diffrent types of cards
[CreateAssetMenu(menuName = "Deck/Card")]//creats Cards for Deck
public class Card : ScriptableObject
{
    public string Name; // name of card 
    public string Description; // a description of what the card does
    public Sprite cardImage; 
    public GameObject particle;
    public GameObject audioSource;
}