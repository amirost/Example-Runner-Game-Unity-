using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject particles;

    private MeshRenderer meshRenderer;
    private Color color;
    [SerializeField] private Material particlesMaterial;

    public AudioClip breakSound;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        color = meshRenderer.material.color;            // Set the particles color to the obstacle color
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))                 // if player hit obstacle
        {
            if (breakSound)
                AudioSource.PlayClipAtPoint(breakSound, transform.position);

            GetComponent<Collider>().enabled = false;   // disable collider of obstacle to avoid multiple trigger
            meshRenderer.enabled = false;               // disable the renderer of the obstacle
            particlesMaterial.color = color;            // set the color on the particles
            particles.SetActive(true);                  // enable particles
            Destroy(gameObject, 2f);                    // Destroy gameobject after 2 seconds

            
        }
    }
}
