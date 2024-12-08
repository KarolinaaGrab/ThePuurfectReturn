using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = this.transform.parent.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            anim.SetBool("goingUp", true);
        }
    }

    private void OnTriggerExit(Collider obj)
    {
        if (obj.CompareTag("Player"))
        {
            anim.SetBool("goingUp", false);
        }
    }
}
