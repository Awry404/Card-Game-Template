using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class EnemySpeedDie : MonoBehaviour
{
    private Enemy parent;
    public int value = 0;
    bool mouseover = false;
    public SpeedDie clash_target;
    public Card selected_card;
    public GameObject canvas;
    public List<GameObject> cardObjects = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    //find parent librarian and call OnDieClick
        parent = GetComponentInParent<Enemy>();
        if (parent != null)
        {
            Debug.Log("parent found: " + parent.name);
        }
        else
        {
            Debug.LogError("No parent found!");
        }    
    }

    // Update is called once per frame
    void Update()
    {

       if (Input.GetMouseButtonDown(0))
        {
            if (mouseover == true)
            {
                if (parent != null)
                {
                    //this might have an error in the future where clash targets are a script and not object. idk if
                    GameManager.gm.selected_enemy_die = this;
                    if (GameManager.gm.selected_card != null)
                    {
                        
                        //assign clashes
                        SpeedDie temp = GameManager.gm.selected_die;
                        //clash_target = temp;
                        Card card = GameManager.gm.selected_card;
                        GameManager.gm.card_database.Add(temp.librarian.hand[card.locationinhand]);
                        Debug.Log("added card to database: " + GameManager.gm.card_database.Count);
                        GameManager.gm.CountCards();
                        
                        temp.clash_target = this;
                        //set selected card for player
                        temp.selected_card = GameManager.gm.card_database[GameManager.gm.card_database.Count - 1];
                        temp.librarian.discard(card);
                        GameManager.gm.selected_card = null;
                        
                    }
                    else
                    {
                        
                        parent.OnDieClick(selected_card);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (parent != null)
            {
                parent.KillCards();
                GameManager.gm.selected_enemy_die = null;
            }
        }
        if (GameManager.gm.selected_enemy_die != this)
        {
            parent.KillCards();
        }
    }


    void OnMouseOver()
    {
        //change color to red
        GetComponent<SpriteRenderer>().color = Color.red;
        mouseover = true;

    }

    void OnMouseExit()
    {
        //change color to white
        GetComponent<SpriteRenderer>().color = Color.white;
        mouseover = false;
    }
}
