using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class EnemySpeedDie : MonoBehaviour
{
    private Enemy parent;
    public int value = 0;
    bool mouseover = false;
    public Librarian clash_target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                    //parent.KillCards();
                    //parent.OnDieClick();
                    //GameManager.gm.selected_enemy_die = this;
                    if (GameManager.gm.selected_card != null)
                    {
                        //set clash target in enemy and player; figure out how to do combat start ig
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
