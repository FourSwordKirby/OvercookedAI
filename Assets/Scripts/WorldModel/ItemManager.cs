using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public List<Item> ItemList = new List<Item>();
    public List<int> IngredientIndexList = new List<int>();
    public List<int> TableIndexList = new List<int>();
    public Table TableItem;
    public Player PlayerObject;

    public Item GetItem(int id)
    {
        return ItemList[id];
    }

    private void Awake()
    {
        
    }

    public void RegisterIngredientItem(Ingredient ingredient)
    {
        int index = ItemList.Count;
        ItemList.Add(ingredient);
        IngredientIndexList.Add(index);
        ingredient.ID = index;
    }

    public void RegisterTableItem(Table table)
    {
        int index = ItemList.Count;
        ItemList.Add(table);
        TableIndexList.Add(index);
        table.ID = index;
    }

    // Use this for initialization
    private void Start ()
    {
        TableItem = FindObjectOfType<Table>();
        PlayerObject = FindObjectOfType<Player>();
	}

    public Table GetTable(int index)
    {
        return ItemList[TableIndexList[index]] as Table;
    }
	
	public AIState GetWorldState()
    {
        return new AIState()
        {
            ItemStateList = ItemList.Select(item => item.GetState()).ToList(),
            IngredientStateIndexList = IngredientIndexList,
            PotStateIndexList = new List<int>(),
            PlateStateIndexList = new List<int>(),
            MealStateIndexList = new List<int>(),
            BoardStateIndexList = new List<int>(),
            TableStateIndexList = TableIndexList,
            CurrentPlayerState = PlayerObject.GetPlayerState(),
            onionSpawnCount = 0,
            mushroomSpawnCount = 0
        };
    }

    public void LoadWorldState(AIState state)
    {
        for (int i = 0; i < state.ItemStateList.Count; ++i)
        {
            ItemList[i].LoadState(state.ItemStateList[i]);
        }

        foreach (int tableId in TableIndexList)
        {
            Table table = ItemList[tableId] as Table;
            if (table.ItemIDOnTable != Item.NOTHING_ID)
            {
                ItemList[table.ItemIDOnTable].transform.position = table.HoldingPosition;
            }
        }

        PlayerObject.LoadState(state.CurrentPlayerState);
        if (state.CurrentPlayerState.HoldingItemID != Item.NOTHING_ID)
        {
            ItemList[state.CurrentPlayerState.HoldingItemID].transform.position = PlayerObject.HoldPosition;
        }
    }
}
