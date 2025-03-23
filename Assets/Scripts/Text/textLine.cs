
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textLine : MonoBehaviour
{
    public string text;
    public bool oneTimeUse = true;



  
        public string GetText()
    {
        if(oneTimeUse)
        {
            gameObject.SetActive(false);
        }
        return text;
    }
}
