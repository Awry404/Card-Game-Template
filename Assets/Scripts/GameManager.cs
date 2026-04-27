using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public List<Enemy> enemies = new List<Enemy>();
    public List<Librarian> librarians = new List<Librarian>();
    public List<GameObject> clashers = new List<GameObject>();
    public Librarian selectedl;
    public Enemy selectede;


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
        //find all enemies on screen
        enemies = new List<Enemy>(FindObjectsByType<Enemy>());
        librarians = new List<Librarian>(FindObjectsByType<Librarian>());
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
        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = 0; j < enemies[i].dice.Count-1; j++)
            {
                if (enemies[i].dice[j].clash_target != null)
                {
                    clashers.Add(enemies[i].dice[j].gameObject);
                }
            }
        }

        for (int i = 0; i < librarians.Count; i++)
        {
            for (int j = 0; j < librarians[i].dice.Count; j++)
            {
                if (librarians[i].dice[j].clash_target != null)
                {
                    clashers.Add(librarians[i].dice[j].gameObject);
                }
            }
        }
        
        for (int i = 0; i < clashers.Count; i++)
        {
            //move clasher towrads it's target
            float range = 0.5f; // Adjust this value as needed
            if (Vector3.Distance(clashers[i].transform.position, clashers[i].GetComponent<SpeedDie>().clash_target.transform.position) > range)
            {
                clashers[i].transform.position = Vector3.MoveTowards(clashers[i].transform.position, clashers[i].GetComponent<SpeedDie>().clash_target.transform.position, Time.deltaTime * 5);
            }
            else
            {
                //clash
                if (clashers[i].GetComponent<Librarian>() != null)
                {
                    // per librarian
                    for (int j = 0; j < clashers[i].GetComponent<Librarian>().dice.Count; j++)
                    {
                        //per card
                        selectedl = clashers[i].GetComponent<Librarian>();
                        selectede = clashers[i].GetComponent<Librarian>().dice[j].clash_target.GetComponent<Enemy>();
                        for  (int k = 0; k < selectedl.dice[j].selected_card.data.dice.Length; k++)
                        {
                            //per die
                            int temp1 = Random.Range(selectedl.dice[j].selected_card.data.dice[k].min, selectedl.dice[j].selected_card.data.dice[k].max);
                            int temp2 = Random.Range(selectede.dice[j].selected_card.data.dice[k].min, selectede.dice[j].selected_card.data.dice[k].max);
                            if (temp1 > temp2)
                            {
                                if (selectede.dice[j].selected_card.data.dice[k].type != "block")
                                {
                                    selectede.health -= (temp1 - temp2);
                                }
                                else if (selectedl.dice[j].selected_card.data.dice[k].type != "evade")
                                {
                                    //do nothing
                                }
                                else
                                {
                                    selectede.health -= temp1;
                                }
                               
                            }
                            else if (temp2 > temp1)
                            {
                                if (selectedl.dice[j].selected_card.data.dice[k].type != "block")
                                {
                                    selectedl.health -= (temp2 - temp1);
                                }
                                else if (selectede.dice[j].selected_card.data.dice[k].type != "evade")
                                {
                                    //do nothing
                                }
                                else
                                {
                                    selectedl.health -= temp2;
                                }
                            }
                            
                        }
                        
                    }

                }
                
            }
        }
        
        

    }
    



    
}
