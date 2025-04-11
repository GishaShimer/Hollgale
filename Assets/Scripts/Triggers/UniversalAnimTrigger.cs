using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class universalAnimTrigger : MonoBehaviour
{
    public PlayableDirector director;
 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            director.Play();
  
            gameObject.SetActive(false);
        }
    }
}
