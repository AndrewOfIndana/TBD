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

    

    


   

}
    

