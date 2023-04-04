using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleController : MonoBehaviour
{
    [SerializeField] [Range(0.5f, 1)] float treeScale = .5f;
    [SerializeField] float current;
    float speed = 2f;
    private void Update()
    {
        if(Input.GetKey(KeyCode.C))
        {
            treeScale += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.V))
        {
            treeScale -= Time.deltaTime;
        }
        treeScale = Mathf.Clamp(treeScale, .5f, 1f);
        current = Mathf.MoveTowards(current, treeScale, speed * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(treeScale,treeScale,treeScale), current);
    }
    private void ScaleTree()
    {

    }
}
