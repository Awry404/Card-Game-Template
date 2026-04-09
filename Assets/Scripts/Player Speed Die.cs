using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class SpeedDie : MonoBehaviour
{
    public Librarian librarian;
    public int value = 0;
    bool mouseover = false;
    public EnemySpeedDie clash_target;
    public Card selected_card;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    //find parent librarian and call OnDieClick
        librarian = GetComponentInParent<Librarian>();
        if (librarian != null)
        {
            Debug.Log("librarian found: " + librarian.name);
        }
        else
        {
            Debug.LogError("No parent librarian found!");
        }    
    }

    // Update is called once per frame
    void Update()
    {

       if (Input.GetMouseButtonDown(0))
        {
            if (mouseover == true)
            {
                if (librarian != null)
                {
                    librarian.KillCards();
                    librarian.OnDieClick();
                    GameManager.gm.selected_die = this;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (librarian != null)
            {
                librarian.KillCards();
                GameManager.gm.selected_die = null;
            }
        }
        if (GameManager.gm.selected_die != this)
        {
            librarian.KillCards();
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
