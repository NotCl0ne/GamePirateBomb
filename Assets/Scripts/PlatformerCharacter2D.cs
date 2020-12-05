using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private float m_JumpCooldown = 0.8f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        public bool m_FacingRight = true;  // For determining which way the player is currently facing.

        public bool isPlayer = false;
        public float hitForce = 300;
        public float hitRadius = 1f;

        bool isHit = false;
        public GameObject particlePrefab;
        public Transform RunParticle;
        bool isRunParticleExist = false;


        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

        }

        bool prevGrounded = false;
        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                }
            }

            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            if(!m_Grounded)
                m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

            if (m_Grounded && !prevGrounded)
            {
                // Landed
                m_Anim.SetFloat("vSpeed", 0f);
                StartCoroutine(landParticle());

            }
            else if (!m_Grounded && prevGrounded)
            {
                // Jumped
            }
            prevGrounded = m_Grounded;

            m_JumpCooldown -= 1 * 0.03f;
        }




        Vector2 oldPosition; 
        public void Move(float move, bool jump)
        {
            if (isHit) return;

            var trueVelocity = (new Vector2(transform.position.x, transform.position.y) - new Vector2(oldPosition.x, transform.position.y)).magnitude / Time.fixedDeltaTime;
            oldPosition = transform.position;
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(trueVelocity));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
                if (move != 0 && m_Grounded && m_JumpCooldown < 0)
                {
                    StartCoroutine(runParticle());
                }
                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground") && m_JumpCooldown < 0)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                if(m_Anim.GetFloat("vSpeed")>=0)
                    StartCoroutine(jumpParticle());

                m_JumpCooldown = 0.8f;
            }

        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        public void Hit()
        {
            m_Anim.SetTrigger("Hit");
            isHit = true;
            Invoke("ResetHit", 0.2f);
            if(isPlayer)
                Lives.manager.LoseLife();
        }

        void ResetHit()
        {
            isHit = false;
        }

        public void Die()
        {
            if (isPlayer)
                GetComponent<Platformer2DUserControl>().enabled = false;
            else
            {
                GetComponent<Enemy>().RemoveDialog();
                GetComponent<Enemy>().enabled = false;
            }

            m_Anim.SetBool("Dead", true);
            m_Anim.SetTrigger("Die");
            GetComponent<Collider2D>().sharedMaterial = null;
            this.enabled = false;
        }

        public void Attack(bool bomb=false)
        {
            if(bomb)
            {
               m_Anim.SetTrigger("Special");
                Invoke("CheckHitBomb", 0.2f);
            }
            else
            {
                m_Anim.SetTrigger("Attack");
                Invoke("CheckHitPlayer", 0.1f);
            }


        }

        void CheckHitPlayer()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitRadius);

            bool hit = false;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Player" && !hit)
                {
                    colliders[i].gameObject.SendMessage("Hit");
                    Rigidbody2DExtension.AddExplosionForce(colliders[i].GetComponent<Rigidbody2D>(), hitForce/2, transform.position, 1, 0.1f);
                    hit = true;
                }
            }
        }

        void CheckHitBomb()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, hitRadius);

            bool hit = false;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Bomb" && !hit)
                {
                    Rigidbody2DExtension.AddExplosionForce(colliders[i].GetComponent<Rigidbody2D>(), hitForce, transform.position, 1, 0.05f);
                    hit = true;
                }
            }
        }

        IEnumerator jumpParticle()
        {
            GameObject jumpParticles = Instantiate(particlePrefab);
            jumpParticles.transform.position = new Vector2(transform.position.x, transform.position.y - 0.26f);
            jumpParticles.GetComponent<Animator>().SetTrigger("Jump");
            yield return new WaitForSeconds(.5f);
            Destroy(jumpParticles);
        }
        IEnumerator landParticle()
        {
            GameObject landParticles = Instantiate(particlePrefab, new Vector2(transform.position.x, transform.position.y - 0.55f), Quaternion.identity);
            landParticles.GetComponent<Animator>().SetTrigger("Land");
            yield return new WaitForSeconds(.5f);
            Destroy(landParticles);
        }
        IEnumerator runParticle()
        {
            if (!isRunParticleExist)
            {
                GameObject runParticles = Instantiate(particlePrefab);
                if(!m_FacingRight)
                {         
                    Vector3 theScale = runParticles.transform.localScale;
                    theScale.x *= -1;
                    runParticles.transform.localScale = theScale;
                }
                runParticles.transform.position = RunParticle.position;
                runParticles.GetComponent<Animator>().SetTrigger("Run");
                isRunParticleExist = true;
                yield return new WaitForSeconds(.45f);
                Destroy(runParticles);
                isRunParticleExist = false;

            }
        }
    }
}
