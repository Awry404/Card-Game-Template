using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Die_data", menuName = "Die/Die_data", order = 1)]
public class Die_data : ScriptableObject
{
    public string type;
    public int min;
    public int max;
    public string status;
    public int samount;


}
