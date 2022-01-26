using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{

    [SerializeField] private float Speed;
    [SerializeField] private float distanceMovement;

    private Vector3 startingPosition;
    [SerializeField] private bool moveLeft;
    [SerializeField] private bool moveRight;
    
    private float min;
    private float max;
    // Use this for initialization
    void Start()
    {
        startingPosition = transform.position;
        SetDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if(Speed > 0)
        Move();
    }

    private void Move()
    {
        Vector3 vector = startingPosition;
        vector.x += distanceMovement * Mathf.Sin(Time.time * Speed) + distanceMovement;
        transform.position = vector;
    }

    void SetDirection() 
    {
        if (moveRight)
        {
            
        }
        else if (moveLeft) 
        {
            distanceMovement = -distanceMovement;
        }
    }
}
