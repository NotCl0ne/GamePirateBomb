using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class GameControl : MonoBehaviour
{
    public GameObject key;
    public int maxBomb;
    private static bool m_isCanSpawnBomb=true;
    // Start is called before the first frame update
    void Start()
    {
    }
    public bool isCanSpawnBomb
    {
        get { return m_isCanSpawnBomb; }
    }
    // Update is called once per frame
    void Update()
    {
        var bombs = GameObject.FindGameObjectsWithTag("Bomb");
        if (GameObject.FindGameObjectWithTag("Enemy") == null && key)
        {
            key.SetActive(true);
        }
        if (bombs.Length>=maxBomb)
        {
            m_isCanSpawnBomb = false;
        }
        else
            m_isCanSpawnBomb = true;
    }
}
