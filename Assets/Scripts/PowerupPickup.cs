using UnityEngine;
using System.Collections;

public class PowerupPickup : MonoBehaviour {

    public AudioClip clip;

    void Start()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            // because we destroy the object, we cannot use an attached AudioSource and Play().
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            print("PICKUP - Diamond");            
            Destroy(gameObject);
    }


}
