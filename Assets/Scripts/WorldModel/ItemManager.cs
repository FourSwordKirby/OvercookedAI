using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public List<Item> ItemList;
    public Item TableItem;

    public Item GetItem(int id)
    {
        return ItemList[id];
    }

    private void Awake()
    {
        
    }

    private void GetAllItems()
    {
        ItemList = new List<Item>();
        foreach (IngredientSpawner spawner in FindObjectsOfType<IngredientSpawner>())
        {
            ItemList.AddRange(spawner.SpawnedIngredients);
        }

        TableItem = FindObjectOfType<Table>();
    }

    // Use this for initialization
    private void Start ()
    {
        GetAllItems();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
