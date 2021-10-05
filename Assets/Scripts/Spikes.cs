using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class Spikes : MonoBehaviour
{
    public GameObject m_Character;
    float invulnerable = 0;
    
    private void FixedUpdate()
    {

        invulnerable -= Time.deltaTime;

    }


    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player")&& invulnerable<=0)
        {
            getHurt(collider);
            invulnerable = 1.5f;
        }
    }
    void getHurt(Collider2D collider)
    {
        bool hit = false;

       
        if (!hit)
        {
            m_Character.SendMessage("Hit");
            Rigidbody2DExtension.AddExplosionForce(collider.GetComponent<Rigidbody2D>(),8 , m_Character.transform.position, 1, 0.1f);
            hit = true;
        }
        
    }


}
