using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Meal : Item
{
    public List<Ingredient> HeldIngredients = new List<Ingredient>();
    public bool IsSpawned;
    public bool IsSubmitted;
    public int CookDuration;
    public bool IsBurnt;
    public bool IsCooked;

    private Text MyText;
    private GameObject Model;
    private RectTransform Canvas;

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
    }

    public override ItemState GetState()
    {
        List<int> ingIDs = HeldIngredients.Select(ing => ing.ID).ToList();
        return new MealState(ID, IsSpawned, ingIDs);
    }

    public override void LoadState(ItemState state)
    {
        MealState mState = state as MealState;
        HeldIngredients.Clear();
        HeldIngredients.AddRange(mState.ContainedIngredientIDs.Select(id => GetItemManager().ItemList[id] as Ingredient));
        IsSpawned = mState.IsSpawned;
        CookDuration = mState.cookDuration;
        IsBurnt = mState.IsBurnt();
        IsCooked = mState.IsCooked();

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        MyText.text = "Time: " + CookDuration + "\n";
        MyText.text += "IDs: " + string.Join(", ", HeldIngredients.Select(ing => ing.ID.ToString()).ToArray());
        Model.SetActive(IsSpawned);
        Canvas.gameObject.SetActive(IsSpawned);
    }
}
