using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Card_data", menuName = "Cards/Card_data", order = 1)]
public class Card_data : ScriptableObject
{
    public string card_name;
    public int cost;
    public string oustatus;
    public int ouamount;
    [SerializeField] public Die_data[] dice;
    public Sprite sprite;


}
