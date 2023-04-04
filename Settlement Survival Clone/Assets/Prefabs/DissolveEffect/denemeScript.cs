using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class denemeScript : MonoBehaviour
{
    public float current=0;
    public float timeVariable=.05f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current = Mathf.MoveTowards(current, 1, timeVariable);
    }
}
