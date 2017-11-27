using System.Linq;
using System.Collections.Generic;
using System;

public abstract class MealState : ItemState {

    public bool IsSpawned;
    public List<int> ContainedIngredientIDs;

    public MealState(int id)
        : base(id)
    {
    }
}
