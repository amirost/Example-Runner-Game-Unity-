using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float controlSpeed; // Horizontal move speed
    [SerializeField] private float PlayerSpeed; // Character Speed
    [HideInInspector] public bool gameStarted;  

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            MoveForward();                  // Move the player forward
            TouchControls();    

            #if (UNITY_EDITOR)
            ButtonControls();
            #endif
        }
    }

    void MoveForward() 
    {
        transform.Translate(Vector3.forward * Time.deltaTime * PlayerSpeed, Space.World);
    }

    void TouchControls()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.Translate(new Vector3(touch.deltaPosition.x, 0, 0) * Time.deltaTime * controlSpeed, Space.World);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3.5f, 3.5f), transform.position.y, transform.position.z);       // Limit the X position of the player between -3.5f & 3.5f
            }

        }
    }

    public void StartGame()                 // Function Called by the GameManager.cs Script when launching a game
    {   
        gameStarted = true;                 
        animator.SetBool("walk", true);     // Start Walk Animation
    }



    void ButtonControls()
    {
    
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 pos = new Vector3(x, 0, z);
        transform.Translate(pos * Time.deltaTime * controlSpeed, Space.World);
        transform.position = new Vector3( Mathf.Clamp(transform.position.x, -3.5f, 3.5f), transform.position.y, transform.position.z);
    
    }
}
