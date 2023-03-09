using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarHandler : MonoBehaviour
{
    public Vector3 offset;
    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(parent.position + offset);
    }
}
