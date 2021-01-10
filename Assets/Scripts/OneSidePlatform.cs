using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityStandardAssets.CrossPlatformInput;

public class OneSidePlatform : MonoBehaviour
{

    public void jumpDown(GameObject character, float waitTime)
    {
        StartCoroutine(ignoreCollider2D(character,waitTime));
    }
    IEnumerator ignoreCollider2D(GameObject character,float waitTime)
    {
        var listCollider2d = character.GetComponents<Collider2D>();
        foreach( Collider2D collider in listCollider2d){
            Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>());
        }
        yield return new WaitForSeconds(.15f);
        Physics2D.IgnoreCollision(character.GetComponent<Collider2D>(), GetComponent<Collider2D>(),false);

    }

}
