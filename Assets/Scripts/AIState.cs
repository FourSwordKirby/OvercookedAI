using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ICloneable {
    List<IngredientState> IngredientStateList;
    List<PotState> PotStateList;
    List<PlateState> PlateStateList;
    List<SoupState> SoupStateList;
    TableState CurrentTableState;
    PlayerState CurrentPlayerState;

    public object Clone()
    {
        throw new NotImplementedException();
    }
}
