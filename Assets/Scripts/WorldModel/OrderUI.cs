using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {

    public Sprite onionSprite;
    public Sprite mushroomSprite;

    public List<Image> ingredientIcons;
    public List<IngredientType> ingredients;

	// Update is called once per frame
	void Update () {
        if(ingredientIcons.Count != ingredients.Count || ingredients.Count != 3)
        {
            throw new System.Exception("All orders must be of size 3");
        }

		for(int i = 0; i < ingredients.Count; i++)
        {
            if (ingredients[i] == IngredientType.MUSHROOM)
                ingredientIcons[i].sprite = mushroomSprite;
            else
                ingredientIcons[i].sprite = onionSprite;
        }
    }
}
