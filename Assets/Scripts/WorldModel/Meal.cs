using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Meal : Item
{
    public List<Ingredient> HeldIngredients = new List<Ingredient>();
    public MealState meal;
    public bool IsSpawned;
    public int CookDuration;
    public bool IsBurnt;
    public bool IsCooked;

    private Text MyText;
    private GameObject Model;
    private RectTransform Canvas;

    public Image ProgressBar;
    public GameObject OnionTracker;
    public TextMesh OnionCount;
    public GameObject MushroomTracker;
    public TextMesh MushroomCount;

    private void Awake()
    {
        MyText = GetComponentInChildren<Text>();
        Model = transform.Find("Model").gameObject;
        Canvas = transform.Find("Canvas").GetComponent<RectTransform>();
    }

    private void Start()
    {
        GetItemManager().RegisterMeal(this);
        UpdateVisuals();
    }

    private void Update()
    {
        int onions = 0;
        int mushrooms = 0;
        foreach (Ingredient ing in HeldIngredients)
        {
            ing.transform.position = transform.position + 5 * Vector3.down;
            float cookingProgress = meal.CurrentCookingProgress();
            float burningProgress = meal.CurrentBurningProgress();

            ProgressBar.fillAmount = cookingProgress;
            ProgressBar.color = Color.Lerp(Color.green, Color.red, burningProgress);
            if (burningProgress >= 1)
            {
                ProgressBar.color = Color.black;
            }

            if (ing.MyIngredientType == IngredientType.ONION)
                onions++;
            if (ing.MyIngredientType == IngredientType.MUSHROOM)
                mushrooms++;
        }

        OnionCount.text = onions.ToString();
        MushroomCount.text = mushrooms.ToString();
        
        OnionTracker.SetActive(onions != 0);
        MushroomTracker.SetActive(mushrooms != 0);
    }

    public override ItemState GetState()
    {
        List<int> ingIDs = HeldIngredients.Select(ing => ing.ID).ToList();
        return new MealState(ID, CookDuration, ingIDs);
    }

    public override void LoadState(ItemState state)
    {
        MealState mState = state as MealState;
        meal = mState;
        HeldIngredients.Clear();
        HeldIngredients.AddRange(mState.ContainedIngredientIDs.Select(id => GetItemManager().ItemList[id] as Ingredient));
        IsSpawned = mState.IsSpawned();
        CookDuration = mState.cookDuration;
        IsBurnt = mState.IsBurnt();
        IsCooked = mState.IsCooked();

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        MyText.text = "Time: " + CookDuration + "\n";
        MyText.text += "IDs: " + string.Join(", ", HeldIngredients.Select(ing => ing.ID.ToString()).ToArray()) + "\n";
        MyText.text += IsCooked ? "COOKED!\n" : "";
        MyText.text += IsBurnt ? "BURNT!\n" : "";
        Model.SetActive(IsSpawned);
        Canvas.gameObject.SetActive(IsSpawned);
    }
}
