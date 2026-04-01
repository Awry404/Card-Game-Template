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

    public Image spriteImage;
        

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
        statusText.text = "inflict " + ouamount.ToString() + " " + oustatus;
        //diceText.text = ouamount.ToString();
        //healthText.text = health.ToString();
        costText.text = cost.ToString();
        spriteImage.sprite = sprite;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
