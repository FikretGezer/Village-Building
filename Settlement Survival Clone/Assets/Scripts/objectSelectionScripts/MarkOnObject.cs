using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MarkOnObject : MonoBehaviour
{
    public static event Action<MarkOnObject> OnMarkAdded;
    public static event Action<MarkOnObject> OnMarkRemoved;

    private void OnEnable()
    {
        OnMarkAdded(this);
    }
    private void OnDisable()
    {
        OnMarkRemoved(this);   
    }
    private void Update()
    {
        
    }
}
