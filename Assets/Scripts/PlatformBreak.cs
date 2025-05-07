using System.Collections;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    private bool isBreaking = false;
    private Collider2D col;
    private Renderer rend;

    SoundManager audioManager;
   


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        col = GetComponent<Collider2D>();
        rend = GetComponent<Renderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBreaking)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    isBreaking = true;
                    StartCoroutine(BreakAndRespawn());
                    break;
                }
            }
        }
    }

    private IEnumerator BreakAndRespawn()
    {
        yield return new WaitForSeconds(1f);
        audioManager.PlaySFX(audioManager.platformBreak, 1f);

        col.enabled = false;
        rend.enabled = false;
        yield return new WaitForSeconds(2f);
        col.enabled = true;
        rend.enabled = true;
        isBreaking = false;
    }
}
