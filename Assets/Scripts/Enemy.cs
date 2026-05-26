using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Enemy : MonoBehaviour
{
    //public static Librarian librarian;
    public List<Card> deck = new List<Card>();
    public List<Card> truedeck = new List<Card>();

    public List<Card> hand = new List<Card>();
    public new string name = "Enemy";
    public List<EnemySpeedDie> dice = new List<EnemySpeedDie>();
    public List<GameObject> cardObjects = new List<GameObject>();
    
    public GameObject canvas;
    public Vector3 Coffset;
    public Card clicked_card;
    public List<SpeedDie> player_dice = new List<SpeedDie>();
    public int index = 0;
    public int health = 20;
    public int maxhealth = 20;
    public int stagger = 10;
    public TextMeshProUGUI damageindicator;
    public Vector3 setlocation = new Vector3(0, 0, 0);
    public SpriteRenderer spriterenderer;
    public Sprite normal;
    public Sprite staggered;
    public Sprite pierce;
    public Sprite blunt;
    public Sprite slash;
    public Sprite guard;
    public Sprite move;
    public Vector3 DIOffset = new Vector3(10, -50f, 0);
    public Color tint = new Color(1f, 1f, 1f, 1f);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setlocation = transform.position;
        canvas = GameObject.Find("Canvas");
        damageindicator = GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
        
        dice = new List<EnemySpeedDie>(GetComponentsInChildren<EnemySpeedDie>());
        spriterenderer = GetComponent<SpriteRenderer>();
        //for (int i = 0; i < dice.Count; i++)
        //{
        //    dice[i].selected_card = hand[1];
        //    hand.RemoveAt(1);
        //    //find all player dice
        //    player_dice = new List<SpeedDie>(FindObjectsByType<SpeedDie>());
        //    index = Random.Range(0, player_dice.Count-1);
        //    dice[i].clash_target = player_dice[index];
        //}
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (damageindicator != null && damageindicator.gameObject.activeInHierarchy)
        {
            damageindicator.transform.position = transform.position + DIOffset;
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

    public void OnDieClick(Card selected_card)
    {
        KillCards();
        Coffset = hand.Count/2 * new Vector3(100.0f, 0, 0);
        Card card = Instantiate(selected_card, canvas.transform);
        card.enemy_parent = this;
        cardObjects.Add(card.gameObject);
        card.transform.position = new Vector3(-450, 600, 0);

    }

    public void turnstart()
    {
        tint = new Color(1f * (health / (float)maxhealth), 1f * (health / (float)maxhealth), 1f * (health / (float)maxhealth), 1f);
        spriterenderer.sprite = normal;
        spriterenderer.color = tint;
        transform.localScale = new Vector3(20, 20, 20);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        for (int i = 0; i < dice.Count; i++)
        {
            dice[i].clashed = false;
            dice[i].selected_card = hand[0];
            hand.RemoveAt(0);
            //find all player dice
            player_dice = new List<SpeedDie>(FindObjectsByType<SpeedDie>());
            index = Random.Range(0, player_dice.Count);
            dice[i].clash_target = player_dice[index];
        }
    }

    public void KillCards()
    {
        //show available cards in hand
       for (int i = cardObjects.Count-1; i >= 0; i--)
        {
            Debug.Log("Killed Card " + i + ": " + hand[i].name);
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
            Debug.LogWarning("Enemy damageindicator is not assigned.");
        }
    }
}

