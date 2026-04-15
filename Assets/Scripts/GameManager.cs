using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public List<Card> player_deck = new List<Card>();
    public List<Card> ai_deck = new List<Card>();
    public List<Card> player_hand = new List<Card>();
    public List<Card> ai_hand = new List<Card>();
    public List<Card> discard_pile = new List<Card>();
    public int initial_hand_size = 4;
    public SpeedDie selected_die;
    public EnemySpeedDie selected_enemy_die;
    public Card selected_card;
    public List<Card> card_database = new List<Card>();

    private void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Deal(int damount)
    {
        Shuffle();
        for (int i = 0; i < initial_hand_size; i++)
        {
            player_hand.Add(player_deck[0]);
            player_deck.RemoveAt(0);
            ai_hand.Add(ai_deck[0]);
            ai_deck.RemoveAt(0);
        }
    }

    void Shuffle()
    {

    }

    void AI_Turn()
    {

    }

    public void CountCards()
    {
        Debug.Log("counted cards");
        for (int i = 0; i < card_database.Count; i++)
        {
           card_database[i].locationinhand = i;
        }
        
    }

    public void Clash()
    {
        //figure
    }
    



    
}
