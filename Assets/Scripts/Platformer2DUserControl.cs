using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]

    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;

        private GameObject powerBar;

        private bool m_Jump;
        public GameObject bombPrefab;
        public const float MAX_FORCE= 7f;
        public const float MAX_TIME_HOLD = 1f;
        private float holdDownTime_Start = 0;
        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            powerBar = GameObject.Find("Force Bar");
            powerBar.SetActive(false);

        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");



            }

            if (CrossPlatformInputManager.GetButtonDown("Fire1")) {
                holdDownTime_Start = Time.time;
               
                
            }
            if (CrossPlatformInputManager.GetButton("Fire1"))
            {
                if (powerBar.active == false)
                    powerBar.SetActive(true);

                powerBar.GetComponent<Animator>().SetBool("ButtonDown", true);
            }
            if (CrossPlatformInputManager.GetButtonUp("Fire1"))
            {
                float holdDownTime = Time.time - holdDownTime_Start;
                powerBar.SetActive(false);
                powerBar.GetComponent<Animator>().SetBool("ButtonDown", false);

                GameObject bomb = Instantiate(bombPrefab);
                bomb.transform.position = transform.position;
                bomb.GetComponent<Bomb>().Launch(calculateForce(holdDownTime),m_Character.m_FacingRight);
                
            }
        }

        private float calculateForce(float holdDownTime)
        {
            float maxTimeToHold = MAX_TIME_HOLD;
            float holdTimeNormalized = Mathf.Clamp01(holdDownTime / maxTimeToHold);
            float force = holdTimeNormalized * MAX_FORCE;
            return force;
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, m_Jump);
            m_Jump = false;
        }
    }
}
