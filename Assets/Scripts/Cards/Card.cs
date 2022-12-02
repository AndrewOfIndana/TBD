using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Attribute { fire, ice, lightning, poision, grow };// the different types of cards
[CreateAssetMenu(menuName = "Deck/Card")]//creates Cards for Deck
public class Card : ScriptableObject
{
    public string Name; // name of card 
    public Sprite cardImage; // the matching image of the card
    public CardEffects value;
}