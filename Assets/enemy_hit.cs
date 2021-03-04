using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_hit : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collision");
        if (collider.tag == "enemy")
        {
            //Debug.Log("made contact with jetpack");
            PlayerController playerScript = gameObject.GetComponent<PlayerController>();
            playerScript.health.Decrement();
        }
    }
}
