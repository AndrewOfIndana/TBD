using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Attribute { fire, ice, lightning, poision, grow };// the different types of cards
[CreateAssetMenu(menuName = "Deck/Card")]//creates Cards for Deck
public class Card : ScriptableObject
{
    public string Name; // name of card 
    public string Description; // a description of what the card does
    public Sprite cardImage; // the matching image of the card
    public GameObject particle;
    public GameObject audioSource;
    public int value;
}