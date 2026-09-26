using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public class Enemy : MonoBehaviour
{
    public Player playerScript;
    public Transform playerPos;
    public float speed = 1f;
    float acceleration;
    public float accelerationTime = 1f;
    public Vector3 velocity;

    void Start()
    {
        acceleration = speed / accelerationTime;
    }

    private void Update()
    { 
        ShipMovement(playerPos);
    }

    void ShipMovement(Transform inPlayerPos)
    {
        inPlayerPos = playerScript.transform;

        Vector3 direction = inPlayerPos.position - transform.position;
        direction = direction.normalized;

        velocity += acceleration * Time.deltaTime * direction;
        velocity = Vector3.ClampMagnitude(velocity, speed);
        transform.position += velocity * Time.deltaTime;
    }
}
