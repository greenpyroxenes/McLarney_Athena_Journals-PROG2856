using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;
    public Vector3 bombOffset = new Vector3(0f, 1f, 0f);
    public float bombTrailSpacing;
    public int numberOfBombs;
    public float cornerDist;
    public Enemy enemyScript;
    public float ratio;
    public float maxRadar;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            SpawnBombAtOffset(bombOffset);
        }
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            SpawnBombTrail(bombTrailSpacing, numberOfBombs);
        }
        if(Keyboard.current.cKey.wasPressedThisFrame)
        {
            SpawnBombOnCorner(cornerDist);
        }
        if(Keyboard.current.wKey.wasPressedThisFrame)
        {
            WarpPlayer(enemyScript.transform, ratio);
        }
        if(Keyboard.current.rKey.isPressed)
        {
            DetectAsteroids(maxRadar, asteroidTransforms);
        }    
    }

    void SpawnBombAtOffset(Vector3 inOffset)
    {
        Instantiate(bombPrefab, transform.position + inOffset, Quaternion.identity);
    }

    void SpawnBombTrail(float bombSpacing, int numberOfBombs)
    {
        Vector2 bombPos = transform.position;
        for (int i = 0; i < numberOfBombs; i++)
        {
            bombPos.y -= bombSpacing;
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
            
        }
    }

    void WarpPlayer(Transform target, float ratio)
    {
        if(ratio > 1)
        {
            ratio = 1;
        }
        Vector3 direction = target.position - transform.position;
        transform.position = direction.normalized * ratio;
    }

    void DetectAsteroids(float inMaxRange, List<Transform> inAsteroids)
    {
        
        for(int  i = 0;i < inAsteroids.Count;i++)
        {
            
            if (inMaxRange > Vector3.Distance(transform.position, inAsteroids[i].position))
            {
                Vector3 direction = inAsteroids[i].position - transform.position;
                Debug.Log(direction);
                Debug.DrawLine(transform.position, inAsteroids[i].position);
            }
        }
    }

    void SpawnBombOnCorner(float inputDistance)
    {
        int corner = Random.Range(1, 5);
        if(corner == 1)
        {
            Vector2 bombPos = transform.position;
            float bombPosX = bombPos.x + inputDistance;
            float bombPosY = bombPos.y + inputDistance;
            bombPos = new Vector2(bombPosX, bombPosY);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
        }
        if(corner == 2)
        {
            Vector2 bombPos = transform.position;
            float bombPosX = bombPos.x - inputDistance;
            float bombPosY = bombPos.y + inputDistance;
            bombPos = new Vector2(bombPosX, bombPosY);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
        }
        if(corner == 3)
        {
            Vector2 bombPos = transform.position;
            float bombPosX = bombPos.x + inputDistance;
            float bombPosY = bombPos.y - inputDistance;
            bombPos = new Vector2(bombPosX, bombPosY);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
        }
        if(corner == 4)
        {
            Vector2 bombPos = transform.position;
            float bombPosX = bombPos.x - inputDistance;
            float bombPosY = bombPos.y - inputDistance;
            bombPos = new Vector2(bombPosX, bombPosY);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
        }
    }
}
