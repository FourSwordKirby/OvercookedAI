using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StephenTestScene : MonoBehaviour
{

    public ItemManager IM;
    public AIState StartState;
    public AIState CurrentState;
    public List<AIState> States = new List<AIState>();

    // Use this for initialization
    void Start()
    {
        IM = FindObjectOfType<ItemManager>();
        StartCoroutine(LateStart());
    }
    
    IEnumerator LateStart()
    {
        yield return new WaitForFixedUpdate();
        StartState = IM.GetWorldState();
        CurrentState = StartState;
        States.Add(CurrentState);
        Debug.Log("Pushed start state.");
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.O))
        {
            ApplyAction(new SpawnAction(IngredientType.ONION));
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ApplyAction(new SpawnAction(IngredientType.MUSHROOM));
        }

        if (!IM.PlayerObject.IsHolding)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Pickup on table 0");
                ApplyAction(new PickUpAction(IM.GetTable(0).ItemIDOnTable));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Pickup on table 1");
                ApplyAction(new PickUpAction(IM.GetTable(1).ItemIDOnTable));
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("Dropoff on table 0");
                ApplyAction(new DropOffAction(IM.GetTable(0).ID));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("Dropoff on table 1");
                ApplyAction(new DropOffAction(IM.GetTable(1).ID));
            }
        }

    }

    private void ApplyAction(Action a)
    {
        if (!a.isValid(CurrentState))
        {
            Debug.Log("Action is not valid. Ignoring.");
            return;
        }

        CurrentState = a.ApplyAction(CurrentState);
        States.Add(CurrentState);
        IM.LoadWorldState(CurrentState);
        Debug.Log("Action applied. History size: " + States.Count);
    }
}
