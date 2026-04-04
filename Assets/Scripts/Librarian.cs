using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Librarian : MonoBehaviour
{
    //public static Librarian librarian;
    public List<Card> deck = new List<Card>();
    public List<Card> truedeck = new List<Card>();

    public List<Card> hand = new List<Card>();
    public string name = "Librarian";
    
    public GameObject canvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.Find("Canvas");

        hand.Clear();
        if (deck.Count == 0 && truedeck.Count > 0)
        {
            deck = new List<Card>(truedeck);
        }
        draw(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void draw(int amount)
    {
        if (deck.Count == 0 && truedeck.Count > 0)
        {
            deck = new List<Card>(truedeck);
        }

        for (int i = 0; i < amount; i++)
        {
            if (deck.Count == 0)
            {
                Debug.Log("Deck is empty!");
                break;
            }

            hand.Add(deck[0]);
            deck.RemoveAt(0);

            if (deck.Count == 0 && truedeck.Count > 0 && i < amount - 1)
            {
                deck = new List<Card>(truedeck);
            }
        }
    }

    public void OnDieClick()
    {
        //show available cards in hand
       for (int i = 0; i < hand.Count; i++)
        {
            Debug.Log("Card " + i + ": " + hand[i].name);
            Card card = Instantiate(hand[i], canvas.transform);
            card.transform.position = new Vector3(i * 50.0f, -25, 0); // Position cards in a row
        }

    }
}

