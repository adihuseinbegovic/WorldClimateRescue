﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;
        public Sprite jetpack_off, jetpackk_on, jetpack_falling; 
        public int jumpsLeft = 0;
        public float jumpBoost = 1;
        public bool jetPackEnabled = true; 
        Vector3 posJetpackLeft = new Vector3(-1.24f, -0.25f, -0.964f);
        Vector3 posJetpackRight = new Vector3(0.8f, -0.25f, -0.964f);
        GameObject jetpack;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            jetpack = GameObject.FindWithTag("jetpack");
            jetpack.SetActive(false);
        }

        protected override void Update()
        {
            //APOCALYPSE METER = 0
            //YOU LOST
            if(GameObject.FindGameObjectWithTag("apocalype_meter").GetComponent<Healthbar>().healthPercentage == 0)
            {
                Debug.Log("Apocalypse Meter == 0; YOU LOST!");
                SceneManager.LoadScene("StartScene");
            }
            if (GameObject.FindGameObjectWithTag("healthbar").GetComponent<Healthbar>().healthPercentage == 0)
            {
                Debug.Log("Health Meter == 0; YOU LOST!");
                SceneManager.LoadScene("StartScene");
            }

            if (Input.GetKey(KeyCode.J))
            {
                    if (jetPackEnabled == false)
                    {
                        Debug.Log("set jetpack level true");
                        jetPackEnabled = true;
                        jetpack.GetComponent<SpriteRenderer>().sprite = jetpack_falling;
                    }
                    else if (jetPackEnabled == true)
                    {
                        Debug.Log("set jetpack level false");
                        jetPackEnabled = false;
                        jetpack.GetComponent<SpriteRenderer>().sprite = jetpack_off;
                    }
            }

            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
               
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                        jetpack.GetComponent<SpriteRenderer>().sprite = jetpackk_on;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                        if(jetPackEnabled == true)
                        {
                            jetpack.GetComponent<SpriteRenderer>().sprite = jetpack_falling;
                        }
                        else
                        {
                            jetpack.GetComponent<SpriteRenderer>().sprite = jetpack_off;
                        }
                        
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {

                if (jumpsLeft > 0)
                {
                    if(jetPackEnabled == true)
                    {
                        jumpBoost = 1.5f;
                        jumpsLeft--;
                        GameObject apo = GameObject.FindGameObjectsWithTag("apocalype_meter")[0];
                        apo.GetComponent<Healthbar>().TakeDamage(10);
                    } else
                    {
                        jumpBoost = 1;
                    }
                }
                if (jumpsLeft <= 0)
                {
                    jumpBoost = 1;
                    //TODO: set jetpack invisible
                    GameObject jetpack2 = this.gameObject.transform.GetChild(0).gameObject;

                    jetpack2.SetActive(false);
                }
                velocity.y = jumpTakeOffSpeed * model.jumpModifier * jumpBoost;
                jump = false;

                GameObject jetpack = this.gameObject.transform.GetChild(0).gameObject;

                //TODO if jetpack available set animation to flying

            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                    //TODO if jetpack available set animation to stopping
                }
            }

            if (move.x > 0.01f)
            {
                spriteRenderer.flipX = false;
                GameObject jetpack = this.gameObject.transform.GetChild(0).gameObject;
                jetpack.GetComponent<SpriteRenderer>().flipX = false;
                jetpack.transform.localPosition = posJetpackLeft;
            }
                


            else if (move.x < -0.01f)
            {
                spriteRenderer.flipX = true;
                GameObject jetpack = this.gameObject.transform.GetChild(0).gameObject;
                jetpack.GetComponent<SpriteRenderer>().flipX = true;

                jetpack.transform.localPosition = posJetpackRight;
            }
                

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);


            //ONLY FOR MOUNTAIN LEVEL 
            GameObject apometer = GameObject.FindGameObjectWithTag("apocalype_meter");
            float percentage = apometer.GetComponent<Healthbar>().healthPercentage /100f;
            targetVelocity = move * maxSpeed *percentage;
           
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}