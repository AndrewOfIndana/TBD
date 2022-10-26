using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckCreator : MonoBehaviour
{
    public CardDisplay[] deck;
    
    public int index;
    public Player[] players;
    public Card[] gameBoard;
    // Start is called before the first frame update
    void Start()
    {
        gameBoard = new Card[0];
        players = new Player[1];
        CardDisplay[] tempHand = new CardDisplay[0];
        for (int i = 0; i < players.Length; i++)
        {
            Player p = new Player();
            p.hand = tempHand;
            p.index = i;
            players[i] = p;
        }
      //  buildDeck();
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
        //deals firts three cards to player
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


[Serializable]
public class Player
{
    public CardDisplay[] hand;
    public int index;
   
}
