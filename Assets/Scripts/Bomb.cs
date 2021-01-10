using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public float radius = 2;
    public float power = 50f;
    public float uplift = 0.1f;

    public float damageRadius = 2;

    public LayerMask whoIsEffected;

    public bool lit = true;
    static float shakeDuration = 0f;

    private float shakeMagnitude = 0.5f;

    private float dampingSpeed = 1f;

    private bool canExplode = false;
    private bool isDoneWaiting = false;
    private SoundController soundController;
    public AudioClip explodeSound;

    Vector3 cameraInitialPosition;
    private AudioSource audioSource;
    void Start()
    {
        cameraInitialPosition = Camera.main.transform.localPosition;
        StartCoroutine(waitExplode());
        soundController = GetComponent<SoundController>();
    }



    IEnumerator waitExplode()
    {

        yield return new WaitForSeconds(4f);
        isDoneWaiting = true;

    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.7f);
        Destroy(gameObject);
    }
    void Explode()
    {
        if (lit &&isDoneWaiting&& canExplode)
        {
            soundController.PlaySound(explodeSound);
            GetComponent<Animator>().SetTrigger("Explode");
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            ExplosiveForce();
            shakeDuration = 0.2f;
            CheckDamage();
            canExplode = false;
            StartCoroutine(Destroy());
        }
    }
    void ExplosiveForce()
    {
        Vector3 explosionPos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);

        List<GameObject> hitPrev = new List<GameObject>();
        foreach (Collider2D hit in colliders)
        {
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

            if (rb != null && !hitPrev.Contains(hit.gameObject))
                Rigidbody2DExtension.AddExplosionForce(rb, power, explosionPos, radius, uplift);
            hitPrev.Add(hit.gameObject);
        }
    }

    void CheckDamage()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius, whoIsEffected);

        List<GameObject> hitPrev = new List<GameObject>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject && !hitPrev.Contains(colliders[i].gameObject))
            {
                colliders[i].gameObject.SendMessage("Hit");
            }
            hitPrev.Add(colliders[i].gameObject);
        }
    }

    public void BlowOut()
    {
        StartCoroutine(WaitAndBlowOut());
    }

    IEnumerator WaitAndBlowOut()
    {
        yield return new WaitForSeconds(0.6f);
        GetComponent<Animator>().SetTrigger("TurnedOff");
        Destroy(gameObject, 0.5f);
        lit = false;
    }

    public void Eat()
    {
        Invoke("GetEaten", 0.5f);
    }

    void GetEaten()
    {
        Destroy(gameObject);
    }

    void Update()
    {
       Explode();

        if (shakeDuration > 0)
        {
            Camera.main.transform.localPosition = cameraInitialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            Camera.main.transform.localPosition = cameraInitialPosition;
        }

    }

    public void Launch(float force, bool isFacingRight)
    {
        float degrees = 45.0f;

        if (isFacingRight == false)
        {
            degrees = -45.0f;
            force *= -1;
        }
        float radians = degrees * Mathf.Deg2Rad;

        Vector2 vec2 = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        transform.GetComponent<Rigidbody2D>().velocity = vec2 * force;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player")|| collision.gameObject.CompareTag("Enemy")|| collision.gameObject.CompareTag("Ground"))
            canExplode=true;

    }

    void OnCollisionExit2D(Collision2D collision)
    {
            canExplode = false;
    }
}

