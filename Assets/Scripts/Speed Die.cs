using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class SpeedDie : MonoBehaviour
{
    private Librarian librarian;
    public int value = 0;
    bool mouseover = false;
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

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (librarian != null)
        //    {
        //        //check if mouse is over this object
        //        
        //        librarian.OnDieClick();
        //    }
        //}
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && mouseover)
        {
            if (librarian != null)
            {
                librarian.OnDieClick();
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (librarian != null)
            {
                //figure out how to remove the cards idk
            }
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
