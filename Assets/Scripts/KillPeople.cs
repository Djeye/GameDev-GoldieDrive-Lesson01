using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPeople : MonoBehaviour
{
    Animator anim;
    bool entered = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
        if (PeopleSpawnerScript.available)
        {
            anim.SetBool("available", true);
        }
    }

    private void FixedUpdate()
    {
        if (PeopleSpawnerScript.available && !entered)
        {
            anim.SetBool("available", true);
            entered = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PeopleKiller")) Destroy(this.gameObject);
    }
}
