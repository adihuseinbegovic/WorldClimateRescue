using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetpack : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
       

        if(collision.gameObject.name == "jetpack_item")
        {
            //Debug.Log("made contact with jetpack");
            collision.gameObject.SetActive(false);
            PlayerController playerScript = gameObject.GetComponent<PlayerController>();
            playerScript.jumpsLeft = 5;

            GameObject jetpack = this.gameObject.transform.GetChild(0).gameObject;
            Debug.Log("Gameobject of type " + jetpack.tag);
            jetpack.SetActive(true);
            jetpack.GetComponent<SpriteRenderer>().enabled = true;

        }
        //Debug.Log("made contact with an object");
    }

    private void Update()
    {
    }
}
