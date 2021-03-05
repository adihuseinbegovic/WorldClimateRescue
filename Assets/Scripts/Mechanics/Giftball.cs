using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giftball : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collide!");

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hit it!");
            GameObject.FindGameObjectWithTag("healthbar").GetComponent<Healthbar>().TakeDamage(30);
            gameObject.SetActive(false);
        }
    }
}