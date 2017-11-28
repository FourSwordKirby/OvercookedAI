using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ICloneable {
    public List<ItemState> ItemStateList;

    public List<int> IngredientStateIndexList;
    public List<int> PotStateIndexList;
    public List<int> PlateStateIndexList;
    public List<int> MealStateIndexList;
    public List<int> BoardStateIndexList;

    public TableState CurrentTableState;
    public PlayerState CurrentPlayerState;

    public object Clone()
    {
        throw new NotImplementedException();
    }
}
