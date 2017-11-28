using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour {

    public const int MAX_NUMBER_OF_INGREDIENTS = 5;
    public Item IngredientPrefab;
    public List<Item> SpawnedIngredients;

    private void Awake()
    {
        SpawnedIngredients = Enumerable.Repeat(Instantiate<Item>(IngredientPrefab, transform), MAX_NUMBER_OF_INGREDIENTS).ToList();
    }
}
