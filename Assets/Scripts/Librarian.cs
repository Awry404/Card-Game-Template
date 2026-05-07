using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Librarian : MonoBehaviour
{
    //public static Librarian librarian;
    public List<Card> deck = new List<Card>();
    public List<Card> truedeck = new List<Card>();

    public List<Card> hand = new List<Card>();
    public new string name = "Librarian";
    public List<SpeedDie> dice = new List<SpeedDie>();
    public List<GameObject> cardObjects = new List<GameObject>();
    
    public GameObject canvas;
    public Vector3 Coffset;
    public Card clicked_card;
    public int health = 20;
    public int stagger = 10;
    public TextMeshProUGUI damageindicator;
    public Vector3 setlocation = new Vector3(0, 0, 0);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setlocation = transform.position;
        canvas = GameObject.Find("Canvas");
        damageindicator = GetComponentInChildren<TMPro.TextMeshProUGUI>(true);

        
        
        dice = new List<SpeedDie>(GetComponentsInChildren<SpeedDie>());
    }

    // Update is called once per frame
    void Update()
    {
        if (damageindicator != null && damageindicator.gameObject.activeInHierarchy)
        {
            damageindicator.transform.position = transform.position + new Vector3(0, 2.5f, 0);
        }
    }

    public void turnstart()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        for (int i = 0; i < dice.Count; i++)
        {
            dice[i].clashed = false;
        }
    }

    public void draw(int amount)
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

    public void discard(Card card)
    {
        hand.RemoveAt(card.locationinhand);
        cardObjects.Remove(card.gameObject);
        Destroy(card.gameObject);
        KillCards();
        OnDieClick();
        
    }

    public void OnDieClick()
    {
        Coffset = hand.Count/2 * new Vector3(100.0f, 0, 0);
        //show available cards in hand
       for (int i = 0; i < hand.Count; i++)
        {
            //Debug.Log("Card " + i + ": " + hand[i].name);
            Card card = Instantiate(hand[i], canvas.transform);
            card.parent = this;
            card.locationinhand = i;
            cardObjects.Add(card.gameObject);
            card.transform.position = new Vector3(i * 100.0f, -25, 0) - Coffset; // Position cards in a row
        }

    }

    public void KillCards()
    {
        //show available cards in hand
       for (int i = cardObjects.Count-1; i >= 0; i--)
        {
            //Debug.Log("Killed Card " + i + ": " + hand[i].name);
            Destroy(cardObjects[i]);
        }
        cardObjects.Clear();

    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void UpdateDI(string amt)
    {
        if (damageindicator == null)
        {
            damageindicator = GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
        }

        if (damageindicator != null)
        {
            damageindicator.text = amt;
            damageindicator.gameObject.SetActive(!string.IsNullOrEmpty(amt));
        }
        else
        {
            Debug.LogWarning("Librarian damageindicator is not assigned.");
        }
    }
}

