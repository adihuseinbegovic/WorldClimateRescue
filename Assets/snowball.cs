using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snowball : MonoBehaviour
{
    // Start is called before the first frame update
   // public Animation platsch;
   // public Animation spawn;


    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // platsch.Play();

        
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("healthbar").GetComponent<Healthbar>().TakeDamage(20);
            gameObject.SetActive(false);
        }
    }
}
