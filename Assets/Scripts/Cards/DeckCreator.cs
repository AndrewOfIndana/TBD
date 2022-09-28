using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckCreator : MonoBehaviour
{
    public Card testCard;
    public Card[] deck;
    public int index;
    public Player[] players;
    public Card[] gameBoard;
    // Start is called before the first frame update
    void Start()
    {
        gameBoard = new Card[0];
        players = new Player[1];
        Card[] tempHand = new Card[0];
        for (int i = 0; i < players.Length; i++)
        {
            Player p= new Player();
            p.hand = tempHand;
            p.index = i;
            players[i] = p;
        }
       buildDeck();
        Shuffle();
       
        for (int i = 0; i <players.Length; i++)
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

            Card a = deck[A];
            Card b = deck[B];
            Card c = deck[A];
            a = b;
            b = c;
            deck[A] = a;
            deck[B] = b;
        }
    }
    public void Deal(Player p)
    {
        //deals firts three cards to player
        Card[] afterDraw = new Card[p.hand.Length + 1];
        p.hand.CopyTo(afterDraw, 0);
        afterDraw[p.hand.Length] = deck[0];
        p.hand = afterDraw;
        Card[] tempDeck = new Card[deck.Length - 1];
        for (int i=1;i<deck.Length; i++)
        {
            tempDeck[i-1] = deck[i];
        }
        deck = tempDeck;
    
    }
    public void playCard(Player p, int selectedCard)
    {
        Card selection = p.hand[selectedCard];
        Card[] tempGameBoard = new Card[gameBoard.Length + 1];
        gameBoard.CopyTo(tempGameBoard, 1);
        //grabs card from player
        tempGameBoard[0] = selection;
        gameBoard = tempGameBoard;

        Card[] tempHand = new Card[p.hand.Length - 1];
        for (int i = 0; i < p.hand.Length; i++)
        {
            if (i < selectedCard) {
                tempHand[i] = p.hand[i];
            }
            if (i > selectedCard)
            {
                tempHand[i-1] = p.hand[i]; 
            }
        }
        {

        }
    }
    
    public void buildDeck() {
        //constructs the deck
        deck = new Card[9];

        Card temp = new Card();
        temp.name = "Fire Attack";
        temp.effect = 1;
        temp.upgrade = 0;
        temp.att = Card.Attribute.fire;
        deck[0] = temp;

        Card temp2 = new Card();
        temp2.name="Fire Upgrade";
        temp2.effect = 0;
        temp2.upgrade = 1;
        temp2.att= Card.Attribute.fire;
        deck[1] = temp2;
        Card temp3 = new Card();

        temp3.name= "Ice Attack";
        temp3.effect = 1;
        temp3.upgrade= 0;
        temp3.att= Card.Attribute.ice;

        deck[2] = temp3;
        Card temp4 = new Card();
        temp4.name= "Ice Upgrade";
        temp4.effect= 1;
        temp4.upgrade = 0;
        temp4.att= Card.Attribute.ice;
        deck[3] = temp4;
        Card temp5 = new Card();

        temp5.name= "Lightning Attack";
        temp5.effect= 1;
        temp5.upgrade = 0;
        temp5.att= Card.Attribute.lightning;
        deck[4] = temp5;
        Card temp6 = new Card();

        temp6.name= "Lighting Upgrade";
        temp6.effect= 0;
        temp6.upgrade= 1;
        temp6.att= Card.Attribute.lightning;
        deck[5] = temp6;
        Card temp7 = new Card();

        temp7.name= "Poision Attack";
        temp7.effect= 1;
        temp7.upgrade= 0;
        temp7.att= Card.Attribute.poision;
        deck[6] = temp7;
        Card temp8 = new Card();
        
        temp8.name= "Poision Upgrade";
        temp8.effect= 0;
        temp8.upgrade= 1;
        temp8.att=Card.Attribute.poision;
        deck[7] = temp8;
        Card temp9 = new Card();

        temp9.name= "Grow Upgrade";
        temp9.effect= 0;
        temp9.upgrade= 1;
        temp9.att= Card.Attribute.grow;
        deck[8] = temp9;

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
    public class Card
{
    public string name;
    public int value;
    public int effect;
    public int upgrade;
    public enum Attribute{fire,ice,lightning,poision,grow};
    public Attribute att;
}
[Serializable]
public class Player
{
    public Card[] hand;
    public int index;
}
