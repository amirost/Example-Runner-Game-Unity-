using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private bool startFollowPlayer = true;
    [SerializeField] private GameObject newCameraPosition;

    private Vector3 position;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;        // Get thedistance betweent he camera and the player to always let camera this further from the player
    }

    // Update is called once per frame
    void LateUpdate()
    {
        FollowPlayer();                 
    }

    void FollowPlayer() 
    {
        if (startFollowPlayer)                  
        {
            transform.position = offset + player.transform.position;    
            position.x = Mathf.Clamp(transform.position.x, -2f, 2f);    
            transform.position = new Vector3(position.x, transform.position.y, transform.position.z);
        }
    }

    public void LookAtCubes() 
    {
        startFollowPlayer = false;
        transform.DOMove(newCameraPosition.transform.position, 1f);
    }

}
