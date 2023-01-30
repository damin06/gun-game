using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    private Collider col;
    private Rigidbody rb;
    private Animator ani;


    private Collider[] ragCol;
    private Rigidbody[] ragRB;

    private void Start()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        OffRagdoll();
    }

    public void OnRagdoll()
    {
        foreach (Collider coli in ragCol)
        {
            coli.enabled = true;
        }

        foreach (Rigidbody Rigd in ragRB)
        {
            Rigd.isKinematic = false;
        }

        col.enabled = false;
        rb.isKinematic = true;
        ani.enabled = false;
    }

    public void OffRagdoll()
    {
        GetragdollCompenet();

        foreach (Collider coli in ragCol)
        {
            coli.enabled = false;
        }

        foreach (Rigidbody Rigd in ragRB)
        {
            Rigd.isKinematic = true;
        }

        col.enabled = true;
        rb.isKinematic = false;
        ani.enabled = true;
    }

    private void GetragdollCompenet()
    {
        ragCol = GetComponentsInChildren<Collider>();
        ragRB = GetComponentsInChildren<Rigidbody>();
    }
}
