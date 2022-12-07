using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;

public class DeckCreator : MonoBehaviour
{
    public static DeckCreator Instance { get; private set; }
    public PlayerDeck[] decks;
    public CardDisplay[,] Cards { get; private set; }
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    public int Height => Cards.GetLength(0);
    public int Width => Cards.GetLength(1);
    private readonly List<CardDisplay> _selection = new List<CardDisplay>();
    public int deckSize;
    // public List<Card> testDeck = new List<Card> { };
    public PlayerController playerController;
    public float coolDownTime = 30f;
    public float startUpTime = 30f;

    private void Awake() => Instance = this;
   
    private IEnumerator coroutine;
  
    public void buildDeck()
    {
        //counts how many cards in a deck and applies the images to the card
        Cards = new CardDisplay[decks.Max(hand => hand.deck.Length), 3];
        for(int i =0; i < Width; i++)
        {
            decks[0].deck[i].gameObject.SetActive(false);
        }
      // deals the cards to the player
        StartCoroutine(SetDown(startUpTime, x));
      
    }
    private IEnumerator SetDown(float time, int pos)
    {
        if(pos < Width)
        {
            yield return new WaitForSeconds(time);
            decks[0].deck[pos].gameObject.SetActive(true);
            var card = decks[y].deck[pos];
            card.x = pos;
            card.y = pos;
            Cards[y, x] = card;
            //sets the card image from a random scritpable object in the database
            card.Card = CardDatabase.Cards[UnityEngine.Random.Range(0, CardDatabase.Cards.Length)];
            playerController.AddCard(card.Card.value);
            StartCoroutine(SetDown(startUpTime, pos + 1));
        }
    }


    private IEnumerator CoolDown(float time, int pos)
    {
        var card = decks[y].deck[x + pos];
        decks[0].deck[pos].gameObject.SetActive(false);

        playerController.ActivateCard(card.Card.value, pos);
        yield return new WaitForSeconds(time);
        decks[0].deck[pos].gameObject.SetActive(true);
        card.Card = CardDatabase.Cards[UnityEngine.Random.Range(0, CardDatabase.Cards.Length)];
        playerController.ReplaceCard(card.Card.value, pos);
    }
    public void Card1()
    {
        decks[0].deck[0].HideEffect();
        StartCoroutine(CoolDown(coolDownTime, 0));
    }
    public void Card2()
    {
        decks[0].deck[1].HideEffect();
        StartCoroutine(CoolDown(coolDownTime, 1));
    }
    public void Card3()
    {
        decks[0].deck[2].HideEffect();
        StartCoroutine(CoolDown(coolDownTime, 2));
    }

    public void Select(CardDisplay card)
    {
        if (!_selection.Contains(card)) _selection.Add(card);//adds the card
        if (_selection.Count < 3) return;
    }

}
    

