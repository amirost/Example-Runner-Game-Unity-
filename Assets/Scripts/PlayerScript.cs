using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerScript : MonoBehaviour
{   
    // THIS SCRIPT HANDLE THE PLAYER ANIMATIONS WHEN PICKING COINS AND WHEN HITTTING OBSTACLES
    private Animator animator;                                                                  
    private int sizePlayer;         // The Size of the player increase each time he picks a coin and decrease each time hits an obstacle

    [SerializeField] private GameObject player3DModel;
    [SerializeField] private ParticleSystem explosionParticles;
    private Material material;

    private int coins;              // number of coins picked during the current game

    void Awake()
    {
        animator = GetComponent<Animator>();
        material = player3DModel.GetComponent<SkinnedMeshRenderer>().sharedMaterials[0];    
        material.color = Color.white;       
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Obstacle"))
        {
            animator.SetTrigger("damage");              

            if (sizePlayer > 0)     
            {
                sizePlayer--;       // Decrease the size player of the player by one
                ApplySizePlayer();  
            }
            else if (sizePlayer == 0)
            {
                sizePlayer--;                           
                StartCoroutine(ExplosionBlink());   // if the player hit an obstacle while size of the player equal 0 (which means the default size), Start Critic Damage animation
            }
            else 
            {
                Explode();      // if sizePlayer = -1 and hits obstacle then explode
            }

        }
        if (collider.gameObject.CompareTag("Coin"))
        {
            Vibration.Vibrate(30);
            coins++;            
            if (sizePlayer < 5)         // Player can increase size only five times
            {
                sizePlayer++;
                StopBlink();            // Stop critic Damage animation
                ApplySizePlayer();
            }

        }
        if (collider.gameObject.CompareTag("FinishLine"))
        {   // WHEN REACHING THE FINISH LINE MAKE THE PLAYER EXPLODE
            StopBlink();
            if (sizePlayer < 0)     // in case of sizePlayer = -1 (Critical Damage aniamtion enabled)
            {                       // then set the sizePlayer to 0 to avoid Lose Screen
                sizePlayer = 0;
            }
            Explode();
        }
    }

    void ApplySizePlayer() 
    {
        // Scale the player
        float tempSize = sizePlayer / 1.5f;
        Vector3 newSize = new Vector3(1f + tempSize, 1f + tempSize, 1f + tempSize);
        transform.DOScale(newSize, 0.5f);
    }

    IEnumerator ExplosionBlink() 
    {
        // Switch between Red and White Color on the player for the critical damage animation
        material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        material.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(ExplosionBlink());
    }

    void StopBlink() 
    {
        // Stop Critical Damage aniamtion
        StopAllCoroutines();
        material.color = Color.white;
    }

    void Explode() 
    {
        StopBlink();
        var main = explosionParticles.main;                     // Increase the size of the explosion particles according to the player model size
        main.startSizeX = sizePlayer;
        main.startSizeY = sizePlayer;
        animator.SetTrigger("attack01");                        // Trigger Explode animation
        Vibration.Vibrate(240);
        Destroy(GetComponent<PlayerController>());              // To Stop moving the player
        GameManager.gm.currentGameScore = coins;
        if (sizePlayer < 0)                                     // if sizePlayer = -1 trigger lose screen
        {
            StartCoroutine(GameManager.gm.LoseGame());
        }   
        else {                                                  // else win screen
            StartCoroutine(GameManager.gm.WinGame(sizePlayer));
        }
        
    }
}
