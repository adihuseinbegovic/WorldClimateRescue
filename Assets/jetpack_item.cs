using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetpack_item : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collision");
        if(collider.name == "jetpack_item")
        {
            //Debug.Log("made contact with jetpack");
            collider.gameObject.SetActive(false);
            PlayerController playerScript = gameObject.GetComponent<PlayerController>();
            playerScript.jumpsLeft = 5;

            GameObject jetpack = this.gameObject.transform.GetChild(0).gameObject;
            Debug.Log("Gameobject of type " + jetpack.tag);
            jetpack.SetActive(true);
        }
    }
}
