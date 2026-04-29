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
            float range = 50f; // Adjust this value as needed
            if (clashers[i].GetComponent<SpeedDie>() && clashers[i].GetComponent<SpeedDie>().clashed == false || clashers[i].GetComponent<EnemySpeedDie>() && clashers[i].GetComponent<EnemySpeedDie>().clashed == false)
            {
                if (clashers[i].GetComponent<SpeedDie>())
                {
                    while (Vector3.Distance(clashers[i].transform.position, clashers[i].GetComponent<SpeedDie>().clash_target.transform.position) > range)
                    {
                        Debug.Log("moving clashers");
                        clashers[i].transform.position = Vector3.MoveTowards(clashers[i].transform.position, clashers[i].GetComponent<SpeedDie>().clash_target.transform.position, Time.deltaTime * 5);
                        clashers[i].GetComponentInParent<Librarian>().transform.position = Vector3.MoveTowards(clashers[i].transform.position, clashers[i].GetComponent<SpeedDie>().clash_target.GetComponentInParent<Enemy>().transform.position, Time.deltaTime * 5);
                    }
                }
                else if (clashers[i].GetComponent<EnemySpeedDie>())
                {
                    
                }
                
                    
                //clash
                if (clashers[i].GetComponentInParent<Librarian>() != null)
                {
                    
                     
                    // per clasher
                    
                    //per card played
                    selectedl = clashers[i].GetComponentInParent<Librarian>();
                    selectede = clashers[i].GetComponent<SpeedDie>().clash_target.GetComponentInParent<Enemy>();
                    for  (int k = 0; k < clashers[i].GetComponent<SpeedDie>().selected_card.data.dice.Length; k++)
                    {
                        //per die on card
                        int temp1 = Random.Range(clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].min, clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].max);
                        int temp2 = 0;

                        if (clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().selected_card.data.dice.Length > k)
                        {
                            temp2 = Random.Range(clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().selected_card.data.dice[k].min, clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().selected_card.data.dice[k].max);
                        }
                        
                        if (temp1 > temp2)
                        {
                            if (clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].type != "block")
                            {
                                selectede.health -= temp1 - temp2;
                            }
                            else if (clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].type != "evade")
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
                            if (clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].type != "block")
                            {
                                selectedl.health -= temp2 - temp1;
                            }
                            else if (clashers[i].GetComponent<SpeedDie>().selected_card.data.dice[k].type != "evade")
                            {
                                //do nothing
                            }
                            else
                            {
                                selectedl.health -= temp2;
                            }
                        }
                        
                    }
                    clashers[i].GetComponent<SpeedDie>().clashed = true;
                    clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().clashed = true;
                        
                    
                    //clashers[i].GetComponent<SpeedDie>().clashed = true;
                    //clashers[i].GetComponent<EnemySpeedDie>().clashed = true;

                }
                    
                

            }
        }
        
        

    }
    



    
}
