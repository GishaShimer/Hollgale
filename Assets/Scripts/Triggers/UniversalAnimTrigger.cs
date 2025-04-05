using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class universalAnimTrigger : MonoBehaviour
{
    public string nameTrigger;
    public string animatorName;
    Animator anim;
    private bool hasTriggered = false;
    private void Start()
    {
        if(animatorName != null)
            anim = GameObject.Find(animatorName).GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            anim.SetTrigger(nameTrigger);
            hasTriggered = true;
        }
    }
}
