using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ICloneable {
    public List<IngredientState> IngredientStateList;
    public List<PotState> PotStateList;
    public List<PlateState> PlateStateList;
    public List<MealState> MealStateList;
    public List<BoardState> BoardStateList;
    public TableState CurrentTableState;
    public PlayerState CurrentPlayerState;

    public object Clone()
    {
        throw new NotImplementedException();
    }
}
