using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    public List<IngredientSpawner> IngredientSpawners = new List<IngredientSpawner>();
    public List<Item> ItemList = new List<Item>();
    public List<int> IngredientIndexList = new List<int>();
    public List<int> TableIndexList = new List<int>();
    public List<int> BoardIndexList = new List<int>();
    public List<int> PotIndexList = new List<int>();
    public List<int> MealIndexList = new List<int>();
    public List<int> PlateIndexList = new List<int>();
    public Player PlayerObject;
    public SubmittedTable SubmittedTableRef;

    public Item GetItem(int id)
    {
        return ItemList[id];
    }

    private void Awake()
    {
        
    }

    public void RegisterIngredientSpawner(IngredientSpawner spawner)
    {
        IngredientSpawners.Add(spawner);
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

    public void RegisterBoard(Board board)
    {
        int index = ItemList.Count;
        ItemList.Add(board);
        BoardIndexList.Add(index);
        board.ID = index;
    }

    public void RegisterPot(Pot pot)
    {
        int index = ItemList.Count;
        ItemList.Add(pot);
        PotIndexList.Add(index);
        pot.ID = index;
    }

    public void RegisterMeal(Meal meal)
    {
        int index = ItemList.Count;
        ItemList.Add(meal);
        MealIndexList.Add(index);
        meal.ID = index;
    }

    public void RegisterPlate(Plate plate)
    {
        int index = ItemList.Count;
        ItemList.Add(plate);
        PlateIndexList.Add(index);
        plate.ID = index;
    }

    // Use this for initialization
    private void Start ()
    {
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
            PotStateIndexList = PotIndexList,
            PlateStateIndexList = PlateIndexList,
            MealStateIndexList = MealIndexList,
            BoardStateIndexList = BoardIndexList,
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
        

        PlayerObject.LoadState(state.CurrentPlayerState);
    }
    
}
