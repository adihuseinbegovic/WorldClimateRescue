using Platformer.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private bool moving = false;
    public PlayerController player;
    private float speed = 50;
    public AnimationClip shoot;
    
    public GameObject ball;

    private float nextActionTime = 0.0f;
    public float period = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void shootBall()
    {
       // shoot.
        Vector3 ballpos = new Vector3(transform.position.x -2f , transform.position.y+1.5f , transform.transform.position.z);
        GameObject obj = (GameObject)Instantiate(ball, ballpos, transform.rotation);
       // shoot.Play();
        obj.GetComponent<Rigidbody2D>().velocity = new Vector2(-9f, -30f);
        // ball.GetComponent<Rigidbody2D>.velocity = new Vector3(0, 10, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (nextActionTime > 40)
        {
           
            shootBall();
            nextActionTime = 0.0f;
        }


        if (moving == true)
        {
            Debug.Log("Changing target ... value = " + (player.transform.position.x +5));
            float step = speed * Time.deltaTime; // calculate distance to move
            Vector3 awayFromPlayer = new Vector3(player.transform.position.x + 5000, player.transform.position.y, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, awayFromPlayer, step);
            moving = true;
        }
        nextActionTime += period;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            moving = true;
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            moving = false;
        }
    }
}
