using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public List<Item> ItemList = new List<Item>();
    public List<int> IngredientIndexList = new List<int>();
    public Item TableItem;
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

    // Use this for initialization
    private void Start ()
    {
        TableItem = FindObjectOfType<Table>();
        PlayerObject = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
