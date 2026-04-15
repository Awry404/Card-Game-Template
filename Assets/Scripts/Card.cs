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
    public Image BackgroundImage;
    public Color Rarity = Color.white;
    public Librarian parent;
    public Enemy enemy_parent;
    public int locationinhand;
    //bool mouseover = false;

        

    // Start is called before the first frame update
    void Start()
    {
        BackgroundImage = GetComponentInChildren<Image>();
        card_name = data.card_name;
        cost = data.cost;
        sprite = data.sprite;
        dice = data.dice[0];
        oustatus = data.oustatus;
        ouamount = data.ouamount;
        nameText.text = card_name;
        BackgroundImage.color = Rarity; // Set default background color
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
        else
        {
            statusText.text = "";
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
                {                    
                    diceTextString += "<color=orange>";
                    diceTextString += "Inflict " + die.samount.ToString() + " " + die.status + "\n";
                }
                else if (die.status == "bleed")
                {                    
                    diceTextString += "<color=red>";
                    diceTextString += "Inflict " + die.samount.ToString() + " " + die.status + "\n";
                }
                else if (die.status == "power down")
                {                    
                    diceTextString += "<color=blue>";
                    diceTextString += "Inflict " + die.samount.ToString() + " " + die.status + "\n";
                }
                else if (die.status == "Recover_light")
                {
                    diceTextString += "<color=yellow>";
                    diceTextString += "Recover " + die.samount.ToString() + " " + "Cost" + "\n";
                }

                //diceTextString += "inflict " + die.samount.ToString() + " " + die.status + "\n";
                diceTextString += "<color=white>";
                
            }
            diceText.text = diceTextString;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (parent != null)
            if (parent.clicked_card == this)
            {
                // Logic to target enemy die - implement this based on your game mechanics
                Debug.Log("Card is currently selected: " + card_name);
            }
    }

    public void HighlightCard()
    {
        if (BackgroundImage != null)
        {
            BackgroundImage.color += new Color(0.2f, 0.2f, 0.2f); // Slightly brighten the card
            transform.SetAsLastSibling(); // Bring to top layer
            //transform.localScale = Vector3.one * 1.1f; // Slightly enlarge the card
            transform.position += new Vector3(0, 50, 0); // Move up slightly
            Debug.Log("Highlighted card: " + card_name);
        }
        else
        {
            Debug.LogError("BackgroundImage is not assigned on card: " + card_name);
        }
    }

    public void UnhighlightCard()
    {
        if (BackgroundImage != null)
        {
            BackgroundImage.color = Rarity; // Reset to default color
            //transform.localScale = Vector3.one; // Reset scale
            transform.position += new Vector3(0, -50, 0); 
            Debug.Log("Unhighlighted card: " + card_name);
        }
        else
        {
            Debug.LogError("BackgroundImage is not assigned on card: " + card_name);
        }
    }

    public void OnCardClicked()
    {
        if (parent != null && enemy_parent == null)
        {
            parent.clicked_card = this;
            GameManager.gm.selected_card = this;
            // Logic to target enemy die - implement this based on your game mechanics
            Debug.Log("Card clicked: " + card_name);
            // For example: Find enemy die and apply effect
        }

    }
}
