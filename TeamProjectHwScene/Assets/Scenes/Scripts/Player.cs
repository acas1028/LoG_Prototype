using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 3f;

    private Rigidbody playerRigidbody;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        
        
            
        
       


        float inputX = Input.GetAxis("Horizontal");

        float inputZ = Input.GetAxis("Vertical");

        float fallspeed = playerRigidbody.velocity.y;

        
        Vector3 velocity = new Vector3(inputX, 0, inputZ);

        velocity.y = fallspeed;

        velocity.x = velocity.x * speed;
        velocity.z = velocity.z * speed;

        playerRigidbody.velocity = velocity;
    }
}
