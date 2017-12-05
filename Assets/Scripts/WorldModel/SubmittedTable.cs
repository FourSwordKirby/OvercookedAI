using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubmittedTable : MonoBehaviour {

    public List<Transform> Locations = new List<Transform>();

    private void Awake()
    {
        foreach(Transform child in transform.Find("Locations"))
        {
            Locations.Add(child);
        }
    }
}
