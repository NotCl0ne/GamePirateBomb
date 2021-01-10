using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class GameControl : MonoBehaviour
{
    public GameObject key;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameObject.Find("BigGuy").GetComponent<PlatformerCharacter2D>().isAlive&&key)
        {
            key.SetActive(true);
        }
    }
}
