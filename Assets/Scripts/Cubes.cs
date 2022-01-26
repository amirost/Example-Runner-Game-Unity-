using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubes : MonoBehaviour
{
    [SerializeField] private GameObject particles;
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public void EnableParticles() 
    {
        meshRenderer.enabled = false;
        particles.SetActive(true);
    }
}
