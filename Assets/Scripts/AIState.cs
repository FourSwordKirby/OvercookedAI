using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIState : ICloneable {
    public List<ItemState> ItemStateList;

    // TODO: Technically, these lists don't change. We can probably move them to somewhere outside the state.
    public List<int> IngredientStateIndexList;
    public List<int> PotStateIndexList;
    public List<int> PlateStateIndexList;
    public List<int> MealStateIndexList;
    public List<int> BoardStateIndexList;

    public TableState CurrentTableState;
    public PlayerState CurrentPlayerState;

    public int GValue;
    public bool IsClosed;
    public AIState Parent;
    public Action ParentAction;

    public object Clone()
    {
        return new AIState()
        {
            ItemStateList = this.ItemStateList.Select(itemState => itemState.Clone() as ItemState).ToList(),
            IngredientStateIndexList = this.IngredientStateIndexList,
            PotStateIndexList = this.PotStateIndexList,
            PlateStateIndexList = this.PlateStateIndexList,
            MealStateIndexList = this.MealStateIndexList,
            BoardStateIndexList = this.BoardStateIndexList,
            CurrentTableState = this.CurrentTableState.Clone() as TableState,
            CurrentPlayerState = this.CurrentPlayerState as PlayerState,
            GValue = int.MaxValue,
            IsClosed = false,
            Parent = null,
            ParentAction = null
        };
    }
}

public class AIStateComparator : IEqualityComparer<AIState>
{
    public bool Equals(AIState x, AIState y)
    {
        return x.ItemStateList.SequenceEqual(y.ItemStateList)
            && x.CurrentTableState.Equals(y.CurrentTableState)
            && x.CurrentPlayerState.Equals(y.CurrentPlayerState);
    }

    public int GetHashCode(AIState obj)
    {
        int seed = 487;
        int modifier = 31;

        unchecked
        {
            int hash = obj.ItemStateList.Aggregate(seed, (current, item) => (current * modifier) + item.GetHashCode());
            hash = (hash * modifier) + obj.CurrentTableState.GetHashCode();
            hash = (hash * modifier) + obj.CurrentPlayerState.GetHashCode();
            return hash;
        }
    }
}