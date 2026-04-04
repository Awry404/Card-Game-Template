using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Card_data data;

    public string card_name;
    public int cost;
    public string oustatus;
    public int ouamount;
    public Die_data dice;
    public Sprite sprite;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI statusText;

    public TextMeshProUGUI diceText;
    public string diceTextString = "";
    public Image spriteImage;
    bool mouseover = false;

        

    // Start is called before the first frame update
    void Start()
    {
        card_name = data.card_name;
        cost = data.cost;
        sprite = data.sprite;
        dice = data.dice[0];
        oustatus = data.oustatus;
        ouamount = data.ouamount;
        nameText.text = card_name;
        if (oustatus != "none"){
            if (oustatus == "burn")
            {
                statusText.text = "<color=orange>";
            }
            else if (oustatus == "bleed")
            {                    
                 statusText.text = "<color=red>";
            }
            else if (oustatus == "power down")
            {                    
                 statusText.text = "<color=blue>";
            }
            statusText.text += "inflict " + ouamount.ToString() + " " + oustatus;
        }
        //diceText.text = ouamount.ToString();
        //healthText.text = health.ToString();
        costText.text = cost.ToString();
        spriteImage.sprite = sprite;

        //list dice
        foreach (Die_data die in data.dice)
        {
            diceTextString += "<color=white>" + die.type + " " + die.min.ToString() + "-" + die.max.ToString() + "\n";
            if (die.status != "none")
            {
                //change color of status text based on type of status
                if (die.status == "burn")
                {                    diceTextString += "<color=orange>";
                }
                else if (die.status == "bleed")
                {                    diceTextString += "<color=red>";
                }
                else if (die.status == "power down")
                {                    
                    diceTextString += "<color=blue>";
                }

                diceTextString += "inflict " + die.samount.ToString() + " " + die.status + "\n";
                diceTextString += "<color=white>";
                
            }
            diceText.text = diceTextString;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && mouseover)
        {
            //figure out how to target enemy die
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
