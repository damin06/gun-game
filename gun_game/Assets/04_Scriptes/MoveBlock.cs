using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    [SerializeField] private Transform From;
    [SerializeField] private Transform To;
    [SerializeField] private float Speed;
    private bool isTo = true;

    void Update()
    {
        if (isTo)
        {
            if (Vector3.Distance(gameObject.transform.position, From.transform.position) == 0)
            {
                isTo = false;
            }

            transform.position = Vector3.MoveTowards(transform.position, From.position, Speed * Time.deltaTime);
        }
        else
        {
            if (Vector3.Distance(gameObject.transform.position, To.transform.position) == 0)
            {
                isTo = true;
            }

            transform.position = Vector3.MoveTowards(transform.position, To.position, Speed * Time.deltaTime);
        }
    }
}
