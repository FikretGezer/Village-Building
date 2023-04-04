using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public static List<GameObject> players = new List<GameObject>();
    public int playerCount;

    private void Update()
    {
        playerCount = players.Count;
    }

}
