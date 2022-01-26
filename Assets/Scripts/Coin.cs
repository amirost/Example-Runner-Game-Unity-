using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private AudioClip collectSound;

    void Update()
    {
        Rotate();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player")) 
        {
            if(collectSound)
                AudioSource.PlayClipAtPoint(collectSound, transform.position);

            Destroy(gameObject);
        }
    }

    void Rotate() 
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
