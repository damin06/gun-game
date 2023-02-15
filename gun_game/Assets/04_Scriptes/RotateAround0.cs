using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private List<GameObject> target;
    [SerializeField] private float Speed;
    [SerializeField] private Vector3 axis;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject targetOBJ in target)
        {
            targetOBJ.transform.RotateAround(gameObject.transform.position, axis, Speed * Time.deltaTime);
        }
    }
}
