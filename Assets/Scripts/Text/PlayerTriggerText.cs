using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTriggerText : MonoBehaviour
{
    public GameObject textPrefab;
    public Transform spawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        textLine Trigger = other.GetComponent<textLine>();
        if(Trigger != null)
        {
            ShowText(Trigger.GetText());
        }
    }

   private void ShowText(string Text)
    {
        GameObject text = Instantiate(textPrefab,spawnPoint.position,Quaternion.identity);
        text.GetComponentInChildren<TMP_Text>().text = Text;


        Destroy(text, 3f);
    }
}
