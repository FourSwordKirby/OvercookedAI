using System.Linq;
using System.Collections.Generic;
using System;

public class SoupState : ItemState {

    const int MAX_INGREDIENTS_PER_SOUP = 3;

    public bool IsSpawned;
    public List<int> ContainedIngredientIDs;

    public SoupState(int id)
        : base(id)
    {
        IsSpawned = false;
        ContainedIngredientIDs = Enumerable.Repeat(Item.NOTHING_ID, MAX_INGREDIENTS_PER_SOUP).ToList();
    }

    public override object Clone()
    {
        throw new NotImplementedException();
    }
}
