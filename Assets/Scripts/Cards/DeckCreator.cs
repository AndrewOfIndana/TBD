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
    public int Height => Cards.GetLength(0);
    public int Width => Cards.GetLength(1);
    private readonly List<CardDisplay> _selection = new List<CardDisplay>();
    public int deckSize;
    public List<Card> testDeck = new List<Card> { };
    private void Awake() => Instance = this;
   
    //public Player[] players;

    // Start is called before the first frame update
    void Start()
    {
        deckSize = 5;
        buildDeck();
    }

    public void buildDeck()
    {
        bool handfull = false;
        /*

        for(int i = 0; i <deckSize; i++)
        {
            x = UnityEngine.Random.Range(0,CardDatabase.Cards.Length);
            testDeck[i] = CardDatabase.Cards[x];
        }
        */
        //counts how many cards in a deck and applies the images to the card
        Cards = new CardDisplay[decks.Max(hand => hand.deck.Length), 3];
        if(handfull = false) { }
        for (var y = 0; y < Height; y++)
        {
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

    public void Select(CardDisplay card) { 
        


    }

    


   

}
    
/*
        players = new Player[1];
        CardDisplay[] tempHand = new CardDisplay[0];
        for (int i = 0; i < players.Length; i++)
        {
            Player p = new Player();
            p.hand = tempHand;
            p.index = i;
            players[i] = p;
        }
        //    buildDeck();
        Shuffle();

        for (int i = 0; i < players.Length; i++)
        {
            Deal(players[i]);
            Deal(players[i]);
            Deal(players[i]);
        }
    }
    public void Shuffle()
    {
        //shuffles the deck for players to draw cards
        int replacement = UnityEngine.Random.Range(100, 1000);
        for (int i = 0; i < replacement; i++)
        {
            int A = UnityEngine.Random.Range(0, 9);
            int B = UnityEngine.Random.Range(0, 9);

            CardDisplay a = deck[A];
            CardDisplay b = deck[B];
            CardDisplay c = deck[A];
            a = b;
            b = c;
            deck[A] = a;
            deck[B] = b;
        }
    }
    public void Deal(Player p)
    {
        //deals first three cards to player
        CardDisplay[] afterDraw = new CardDisplay[p.hand.Length + 1];
        p.hand.CopyTo(afterDraw, 0);
        afterDraw[p.hand.Length] = deck[0];
        p.hand = afterDraw;
        CardDisplay[] tempDeck = new CardDisplay[deck.Length - 1];
        for (int i = 1; i < deck.Length; i++)
        {
            tempDeck[i - 1] = deck[i];
        }
        deck = tempDeck;

    }
    /*
    public void playCard(Player p, int selectedCard)
    {
        CardDisplay selection = p.hand[selectedCard];
        CardDisplay[] tempGameBoard = new CardDisplay[gameBoard.Length + 1];
        gameBoard.CopyTo(tempGameBoard, 1);
        //grabs card from player
       // tempGameBoard[0] = selection;
       // gameBoard = tempGameBoard; 

        CardDisplay[] tempHand = new CardDisplay[p.hand.Length - 1];
        for (int i = 0; i < p.hand.Length; i++)
        {
            if (i < selectedCard) {
                tempHand[i] = p.hand[i];
            }
            if (i > selectedCard)
            {
                tempHand[i - 1] = p.hand[i];
            }
        }
        {

        }
    }   
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playCard(players[0], 1);
        }
    }
}
    
}

[Serializable]
public class Player
{
    public CardDisplay[] hand;
    public int index;
   
}
*/
