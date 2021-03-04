using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sampling_item : MonoBehaviour
{
    public bool sampling_available = false;
    public GameObject sampling_prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sampling_available == true)
        {
            if (Input.GetKey(KeyCode.T))
            {
                GameObject apo = GameObject.FindGameObjectsWithTag("apocalype_meter")[0];
                apo.GetComponent<Healthbar>().GainHealth(30);
                sampling_available = false;
                PlayerController player = gameObject.GetComponent<PlayerController>();
                Instantiate(sampling_prefab, player.transform.position, Quaternion.identity);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "sapling_item")
        {
            //Debug.Log("made contact with jetpack");
            collider.gameObject.SetActive(false);

            sampling_available = true;
        }
    }
}
