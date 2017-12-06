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
        if(ingredients.Count > 3)
        {
            throw new System.Exception("All orders must be < size 3");
        }

		for(int i = 0; i < 3; i++)
        {
            if (i < ingredients.Count)
            {
                ingredientIcons[i].gameObject.SetActive(true);
                if (ingredients[i] == IngredientType.MUSHROOM)
                    ingredientIcons[i].sprite = mushroomSprite;
                else
                    ingredientIcons[i].sprite = onionSprite;
            }
            else
                ingredientIcons[i].gameObject.SetActive(false);
        }
    }
}
