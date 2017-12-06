using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderTrackerUI : MonoBehaviour {
    public OrderUI OrderPrefab;

    private List<List<IngredientType>> activeOrders = new List<List<IngredientType>>();
    private List<OrderUI> orderPanels = new List<OrderUI>();

    private void Update()
    {
        for(int i = 0; i < activeOrders.Count; i++)
        {
            if(i > orderPanels.Count-1)
            {
                OrderUI newPanel = Instantiate(OrderPrefab) as OrderUI;
                newPanel.transform.parent = this.gameObject.transform;
                orderPanels.Add(newPanel);
            }
            OrderUI panel = orderPanels[i];
            panel.ingredients = activeOrders[i];
            panel.transform.position = new Vector3(i%5 * 100 + 50, 450 - (i/5*100));
        }
    }

    public void updateRecipes(List<List<IngredientType>> goalRecipes)
    {
        activeOrders = goalRecipes;
    }
}
