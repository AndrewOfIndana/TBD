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
    public int x;
    public int y;
    public int Height => Cards.GetLength(0);
    public int Width => Cards.GetLength(1);
    private readonly List<CardDisplay> _selection = new List<CardDisplay>();
    public int deckSize;
    public List<Card> testDeck = new List<Card> { };
    private void Awake() => Instance = this;
   
    private IEnumerator coroutine;
  
    // Start is called before the first frame update
    void Start()
    {
        buildDeck();
    }
    
    public void buildDeck()
    {
        //counts how many cards in a deck and applies the images to the card
        Cards = new CardDisplay[decks.Max(hand => hand.deck.Length), 3];
      // deals the cards to the player
            for (var x = 0; x < Width; x++)
            {
                //gets the card location 
                var card = decks[y].deck[x];
                card.x = x;
                card.y = y;
                Cards[y, x] = card;
                //sets the card image from a random scritpable object in the database
                card.Card = CardDatabase.Cards[UnityEngine.Random.Range(0, CardDatabase.Cards.Length)];
            }
      
    }
    IEnumerator  CoolDown(float time, int pos)
    {
        var card = decks[y].deck[x + pos];
        decks[0].deck[pos].gameObject.SetActive(false);
        

        yield return new WaitForSeconds(time);
        decks[0].deck[pos].gameObject.SetActive(true);
        Debug.Log(card.Card.value);
        card.Card = CardDatabase.Cards[UnityEngine.Random.Range(0, CardDatabase.Cards.Length)];
    }
    public void Card1()
    {
        StartCoroutine(CoolDown(2f, 0));
      
    }


    public void Card2()
    {
        StartCoroutine(CoolDown(2f, 1));
    }
    public void Card3()
    {
        StartCoroutine(CoolDown(2f, 2));

    }

    public async void Select(CardDisplay card)
    {
        if (!_selection.Contains(card)) _selection.Add(card);//adds the card
        if (_selection.Count < 3) return;
        Debug.Log($"Selected tiles at ({_selection[0].x},{_selection[0].y}) and ({_selection[1].x}, {_selection[1].y})and ({_selection[2].x},{_selection[2].y}");

    }

}
    

