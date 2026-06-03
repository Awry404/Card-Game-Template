using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using TMPro;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


public class deckmaker : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public string deckFileName;
    public bool mouseover = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveDeck();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mouseover == true)
            {
                Debug.Log("started recording");
                RecordDeck();
            }
        }
    }

    public void addCard(Card card)
    {
        if (deck.Count < 30)
        {
            deck.Add(card);
            Debug.Log($"Added card '{card.data.card_name}' to deck (total: {deck.Count})");
        }
        else
        {
            Debug.Log("Deck is full! Cannot add more cards.");
        }
    }

    public void RecordDeck()
    {
        
    }

    public void saveDeck()
    {
        string fileName = string.IsNullOrWhiteSpace(deckFileName) ? gameObject.name : deckFileName;
        string deckFolder = "Decks";
        if (GameManager.gm != null)
        {
            deckFolder = GameManager.gm.deckFolder;
        }

        string folderPath = Path.Combine(Application.streamingAssetsPath, deckFolder);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, fileName + ".txt");

        var lines = deck.Where(c => c != null && c.data != null)
                        .Select(c => c.data.card_name)
                        .ToArray();

        File.WriteAllLines(filePath, lines);
        Debug.Log($"Saved deck '{fileName}' to {filePath} ({lines.Length} cards)");
    }

    public void OnMouseOver()
    {
        

        mouseover = true;
    }

    public void OnMouseExit()
    {
        mouseover = false;
    }
}
