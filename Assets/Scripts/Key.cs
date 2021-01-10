using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject m_Door;
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
        if (collider.gameObject.CompareTag("Player"))
        {
            m_Door.GetComponent<Door>().openDoor();
            m_Door.GetComponent<Animator>().SetTrigger("Open");
            Destroy(gameObject);
        }
    }
}
