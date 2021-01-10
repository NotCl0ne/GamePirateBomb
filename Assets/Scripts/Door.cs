using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets._2D;

public class Door : MonoBehaviour
{
    private GameObject m_player;
    private bool isOpen = false;
    public bool Open;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindWithTag("Player");
        if (Open)
        {
            GetComponent<Animator>().SetTrigger("Open");
            GetComponent<Collider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player")&&isOpen)
        {
            StartCoroutine(goInDoor());

        }
    }
    IEnumerator goInDoor()
    {
        yield return new WaitForSeconds(0.2f);
        m_player.GetComponent<PlatformerCharacter2D>().Move(0, false);
        m_player.GetComponent<Platformer2DUserControl>().enabled = false;

        m_player.GetComponent<Animator>().SetTrigger("DoorIn");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void openDoor()
    {
        isOpen = true;
    }
}
