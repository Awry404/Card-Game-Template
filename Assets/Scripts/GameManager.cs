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
    public int turn = 0;


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
    IEnumerator Start()
    {
        Debug.Log("Game Manager Started");
        //find all enemies on screen
        enemies = new List<Enemy>(FindObjectsByType<Enemy>());
        librarians = new List<Librarian>(FindObjectsByType<Librarian>());
        turn = 0;
        //wait a frame and then start combat
        yield return new WaitForSeconds(0.1f);
        startcombat();
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
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

    public void startcombat()
    {
        enemies = new List<Enemy>(FindObjectsByType<Enemy>());
        librarians = new List<Librarian>(FindObjectsByType<Librarian>());
        
        for (int i = 0; i < librarians.Count; i++)
        {
            librarians[i].transform.position = librarians[i].setlocation;
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].transform.position = enemies[i].setlocation;
        }

        if (turn == 0)
        {
            for (int i = 0; i < librarians.Count; i++)
            {
                librarians[i].hand.Clear();
                if (librarians[i].deck.Count == 0 && librarians[i].truedeck.Count > 0)
                {
                    librarians[i].deck = new List<Card>(librarians[i].truedeck);
                }
                librarians[i].ShuffleDeck();
                librarians[i].draw(4);
                librarians[i].turnstart();
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].hand.Clear();
                if (enemies[i].deck.Count == 0 && enemies[i].truedeck.Count > 0)
                {
                    enemies[i].deck = new List<Card>(enemies[i].truedeck);
                }
                enemies[i].ShuffleDeck();
                enemies[i].draw(4);
                enemies[i].turnstart();
            }
        }       
        else
        {
            for (int i = 0; i < librarians.Count; i++)
            {
                librarians[i].KillCards();
                librarians[i].draw(1);
                librarians[i].turnstart();
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].KillCards();
                enemies[i].draw(1);
                enemies[i].turnstart();
            }
        }
    }

    public IEnumerator Clash()
    {
        //figure
        clashers.Clear();
        for (int i = 0; i < enemies.Count; i++)
        {
            for (int j = 0; j < enemies[i].dice.Count; j++)
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
            //move clasher and its parent towards the clash target
            var speedDie = clashers[i].GetComponent<SpeedDie>();
            var enemySpeedDie = clashers[i].GetComponent<EnemySpeedDie>();
            if ((speedDie != null && speedDie.clashed == false) || (enemySpeedDie != null && enemySpeedDie.clashed == false))
            {
                float moveSpeed = 100f;
                float stopDistance = 50f;
                Vector3 targetPosition;
                Transform parentTransform;
                Vector3 dieWorldOffset;

                if (speedDie != null)
                {
                    targetPosition = speedDie.clash_target.transform.position;
                    parentTransform = speedDie.librarian.transform;
                }
                else
                {
                    targetPosition = enemySpeedDie.clash_target.transform.position;
                    parentTransform = enemySpeedDie.GetComponentInParent<Enemy>().transform;
                }

                dieWorldOffset = clashers[i].transform.position - parentTransform.position;
                Vector3 directionToTarget = targetPosition - clashers[i].transform.position;
                if (directionToTarget == Vector3.zero)
                {
                    directionToTarget = Vector3.forward;
                }
                directionToTarget = directionToTarget.normalized;
                Vector3 dieTargetPosition = targetPosition - directionToTarget * stopDistance;
                Vector3 parentTargetPosition = dieTargetPosition - dieWorldOffset;

                while (Vector3.Distance(clashers[i].transform.position, dieTargetPosition) > 0.05f)
                {
                    parentTransform.position = Vector3.MoveTowards(parentTransform.position, parentTargetPosition, Time.deltaTime * moveSpeed);
                    yield return null;
                }

                parentTransform.position = parentTargetPosition;

                //clash librarians
                if (clashers[i].GetComponentInParent<Librarian>() != null)
                {
                    
                     
                    // per clasher
                    
                    //per card played
                    selectedl = clashers[i].GetComponentInParent<Librarian>();
                    selectede = clashers[i].GetComponent<SpeedDie>().clash_target.GetComponentInParent<Enemy>();
                    SpeedDie playerDie = clashers[i].GetComponent<SpeedDie>();
                    EnemySpeedDie enemyDie = playerDie.clash_target;
                    int playerDiceCount = playerDie.selected_card.data.dice.Length;
                    int enemyDiceCount = enemyDie.selected_card.data.dice.Length;
                    int maxDice = Mathf.Max(playerDiceCount, enemyDiceCount);
                    for  (int k = 0; k < maxDice; k++)
                    {
                        //per die on card
                        int temp1 = 0;
                        if (k < playerDiceCount)
                        {
                            temp1 = Random.Range(playerDie.selected_card.data.dice[k].min, playerDie.selected_card.data.dice[k].max);
                        }
                        int temp2 = 0;
                        if (k < enemyDiceCount && enemyDie.clash_target == playerDie)
                        {
                            temp2 = Random.Range(enemyDie.selected_card.data.dice[k].min, enemyDie.selected_card.data.dice[k].max);
                        }
                        
                        selectedl.UpdateDI(temp1.ToString());
                        selectede.UpdateDI(temp2.ToString());
                        yield return new WaitForSeconds(1);
                        selectedl.UpdateDI("");
                        selectede.UpdateDI("");

                        
                        
                        if (temp1 > temp2)
                        {
                            if (k < enemyDiceCount && enemyDie.selected_card.data.dice[k].type == "block")
                            {
                                selectede.health -= temp1 - temp2;
                                selectedl.UpdateDI((temp1 - temp2).ToString());
                            }
                            else if (k < enemyDiceCount && enemyDie.selected_card.data.dice[k].type == "evade")
                            {
                                //do nothing
                                selectedl.UpdateDI("Missed");
                            }
                            else
                            {
                                selectede.health -= temp1;
                                selectedl.UpdateDI(temp1.ToString());
                            }
                           
                        }
                        else if (temp2 > temp1)
                        {
                            if (k < playerDiceCount && playerDie.selected_card.data.dice[k].type == "block")
                            {
                                selectedl.health -= temp2 - temp1;
                                selectede.UpdateDI((temp2 - temp1).ToString());
                            }
                            else if (k < playerDiceCount && playerDie.selected_card.data.dice[k].type == "evade")
                            {
                                //do nothing
                                selectede.UpdateDI("Missed");
                            }
                            else
                            {
                                selectedl.health -= temp2;
                                selectede.UpdateDI(temp2.ToString());
                            }
                        }
                        //wait and then reset indicators
                        yield return new WaitForSeconds(1);
                        selectedl.UpdateDI("");
                        selectede.UpdateDI("");
                        
                    }
                    clashers[i].GetComponent<SpeedDie>().clashed = true;
                    clashers[i].GetComponent<SpeedDie>().clash_target.GetComponent<EnemySpeedDie>().clashed = true;
                    

                }
                //clash enemies
                else if (clashers[i].GetComponentInParent<Enemy>() != null)
                {
                    // per clasher

                    //per card played
                    selectede = clashers[i].GetComponentInParent<Enemy>();
                    selectedl = clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponentInParent<Librarian>();
                    EnemySpeedDie enemyDie = clashers[i].GetComponent<EnemySpeedDie>();
                    SpeedDie playerDie = enemyDie.clash_target;
                    int enemyDiceCount = enemyDie.selected_card.data.dice.Length;
                    int playerDiceCount = playerDie.selected_card.data.dice.Length;
                    int maxDice = Mathf.Max(enemyDiceCount, playerDiceCount);
                    for (int k = 0; k < maxDice; k++)
                    {
                        //per die on card
                        int temp1 = 0;
                        if (k < enemyDiceCount)
                        {
                            temp1 = Random.Range(enemyDie.selected_card.data.dice[k].min, enemyDie.selected_card.data.dice[k].max);
                        }
                        int temp2 = 0;
                        if (k < playerDiceCount && playerDie.clash_target == enemyDie)
                        {
                            temp2 = Random.Range(playerDie.selected_card.data.dice[k].min, playerDie.selected_card.data.dice[k].max);
                        }

                        selectede.UpdateDI(temp1.ToString());
                        selectedl.UpdateDI(temp2.ToString());
                        yield return new WaitForSeconds(1);
                        selectede.UpdateDI("");
                        selectedl.UpdateDI("");

                        if (temp1 > temp2)
                        {
                            if (k < playerDiceCount && playerDie.selected_card.data.dice[k].type == "block")
                            {
                                selectedl.health -= temp1 - temp2;
                                selectede.UpdateDI((temp1 - temp2).ToString());
                            }
                            else if (k < playerDiceCount && playerDie.selected_card.data.dice[k].type == "evade")
                            {
                                //do nothing
                                selectede.UpdateDI("Missed");
                            }
                            else
                            {
                                selectedl.health -= temp1;
                                selectede.UpdateDI(temp1.ToString());
                            }
                        }
                        else if (temp2 > temp1)
                        {
                            if (k < enemyDiceCount && enemyDie.selected_card.data.dice[k].type == "block")
                            {
                                selectede.health -= temp2 - temp1;
                                selectedl.UpdateDI((temp2 - temp1).ToString());
                            }
                            else if (k < enemyDiceCount && enemyDie.selected_card.data.dice[k].type == "evade")
                            {
                                //do nothing
                                selectedl.UpdateDI("Missed");
                            }
                            else
                            {
                                selectede.health -= temp2;
                                selectedl.UpdateDI(temp2.ToString());
                            }
                        }
                        //wait and then reset indicators
                        yield return new WaitForSeconds(1);
                        selectede.UpdateDI("");
                        selectedl.UpdateDI("");
                    }
                    clashers[i].GetComponent<EnemySpeedDie>().clashed = true;
                    clashers[i].GetComponent<EnemySpeedDie>().clash_target.GetComponent<SpeedDie>().clashed = true;
                }
                    
                

            }
        }
        
    //remove selected cards
    for (int i = 0; i < clashers.Count; i++)
    {
        if (clashers[i].GetComponent<SpeedDie>() != null)
        {
            clashers[i].GetComponent<SpeedDie>().selected_card = null;
            clashers[i].GetComponent<SpeedDie>().clash_target = null;
        }
        else if (clashers[i].GetComponent<EnemySpeedDie>() != null)
        {
            clashers[i].GetComponent<EnemySpeedDie>().selected_card = null;
            clashers[i].GetComponent<EnemySpeedDie>().clash_target = null;
        }
    }
    
    //next turn
    card_database.Clear();
    turn += 1;
    startcombat();
    }
    



    
}
