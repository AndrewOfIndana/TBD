using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;

    [Header("Movement Variables")]
    public float playerSpeed = 10f;
    Vector3 velocity;

    void Awake()
    {
        playerRb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        velocity.x = Input.GetAxis("Horizontal");
        velocity.z = Input.GetAxis("Vertical");

        playerRb.MovePosition(playerRb.position + velocity * playerSpeed * Time.deltaTime);

        
    }
}
