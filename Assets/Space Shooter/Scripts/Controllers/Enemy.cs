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

    void Start()
    {
        shipAcceleration = shipSpeed / shipaAcelerationTime;
        rocketAcceleration = rocketSpeed / rocketAccelerationTime;
    }

    private void Update()
    {
        playerPos = playerScript.transform;
        ShipMovement(playerPos);
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            LaunchRocket(playerPos);
            spawnedRocket = true;
        }
        if(spawnedRocket == true)
        {
            RocketMovement(playerPos);
            StartCoroutine(explodeRocket());
        }
        Debug.Log(enemyRocketPrefab.transform.position);
    }

    void ShipMovement(Transform inPlayerPos)
    {
        Vector3 direction = inPlayerPos.position - transform.position;
        direction = direction.normalized;

        shipVelocity += shipaAcelerationTime * Time.deltaTime * direction;
        shipVelocity = Vector3.ClampMagnitude(shipVelocity, shipSpeed);
        transform.position += shipVelocity * Time.deltaTime;
    }

    void LaunchRocket(Transform inPlayerPos)
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

    void DestroyRocket(GameObject inRocket)
    {
        Destroy(inRocket);
        spawnedRocket = false;
    }

    IEnumerator explodeRocket()
    {

        yield return new WaitForSeconds(5f);
        DestroyRocket(copyRocket);
        
    }
}
