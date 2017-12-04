using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour {

    public const int MAX_NUMBER_OF_INGREDIENTS = 5;
    public IngredientType MyIngredientType;
    public Ingredient IngredientPrefab;
    public List<Item> SpawnedIngredients;

    private void Awake()
    {
        SpawnedIngredients = new List<Item>();
        for (int i = 0; i < MAX_NUMBER_OF_INGREDIENTS; ++i)
        {
            Ingredient ing = Instantiate<Ingredient>(IngredientPrefab, transform);
            ing.transform.position = transform.position;
            ing.MyIngredientType = MyIngredientType;
            SpawnedIngredients.Add(ing);
        }
    }

    private void Start()
    {
        FindObjectOfType<ItemManager>().RegisterIngredientSpawner(this);
    }
}
