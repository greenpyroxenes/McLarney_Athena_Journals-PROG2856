using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public class Enemy : MonoBehaviour
{
    public Player playerScript;
    public Transform playerPos;
    public float shipSpeed = 1f;
    float shipAcceleration;
    public float shipaAcelerationTime = 1f;
    public Vector3 shipVelocity;
    public Vector3 rocketVelocity;
    public GameObject enemyRocketPrefab;
    public bool spawnedRocket = false;
    public GameObject copyRocket;
    public float rocketSpeed = 1f;
    float rocketAcceleration;
    public float rocketAccelerationTime;
    public float time = 0f;
    public float rocketDist = 0.5f;
    public float rocketSpawnTime = 0f;

    void Start()
    {
        shipAcceleration = shipSpeed / shipaAcelerationTime;
        rocketAcceleration = rocketSpeed / rocketAccelerationTime;
    }

    private void Update()
    {
        rocketSpawnTime += Time.deltaTime;
        playerPos = playerScript.transform;
        ShipMovement(playerPos);
        if(spawnedRocket == false && rocketSpawnTime > 4f)
        {
            LaunchRocket();
            spawnedRocket = true;
        }
        if(spawnedRocket == true)
        {
            RocketMovement(playerPos);
            DestroyRocket();
        }
    }

    void ShipMovement(Transform inPlayerPos)
    {
        Vector3 direction = inPlayerPos.position - transform.position;
        direction = direction.normalized;

        shipVelocity += shipaAcelerationTime * Time.deltaTime * direction;
        shipVelocity = Vector3.ClampMagnitude(shipVelocity, shipSpeed);
        transform.position += shipVelocity * Time.deltaTime;
    }

    void LaunchRocket()
    {
        if (spawnedRocket == false)
        {
            copyRocket = Instantiate(enemyRocketPrefab, transform.position, Quaternion.identity);
        }
    }
    void RocketMovement(Transform inPlayerPos)
    {
        Vector3 direction = inPlayerPos.position - copyRocket.transform.position;
        direction = direction.normalized;

        rocketVelocity += rocketAcceleration * Time.deltaTime * direction;
        rocketVelocity = Vector3.ClampMagnitude(rocketVelocity, rocketSpeed);
        copyRocket.transform.position += rocketVelocity * Time.deltaTime;
    }

    void DestroyRocket()
    {
        if (rocketDist > Vector3.Distance(playerScript.transform.position, copyRocket.transform.position))
        {
            Destroy(copyRocket);
            spawnedRocket = false;
            rocketSpawnTime = 0;
            time = 0;
        }
        time += Time.deltaTime;
        if (time > 4f)
        {
            Destroy(copyRocket);
            spawnedRocket = false;
            time = 0;
            rocketSpawnTime = 0;
        }
    }
}
