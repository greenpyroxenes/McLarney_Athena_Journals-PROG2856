using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    public Vector3 velocity;
    public float accelerationTime = 1f;
    public float decelerationTime = 1f;
    private float acceleration;
    private float deceleration;
    public float maxSpeed = 1f;
    public float radarRadius = 3f;
    public int numSides = 8;
    public Color radarColor = Color.green;
    public float powerUpRadius = 2f;
    public int numOfPowerUp = 4;
    public GameObject powerupPrefab;

    void Start()
    {
        acceleration = maxSpeed / accelerationTime;
        deceleration = maxSpeed / decelerationTime;
    }

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
        if(Keyboard.current.pKey.wasPressedThisFrame)
        {
            SpawnPowerUps(powerUpRadius, numOfPowerUp);
        }
        PlayerMovement();
        PlayerRadar(radarRadius, numSides);
    }

    #region Bomb
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

    void SpawnBombOnCorner(float inputDistance)
    {
        int corner = Random.Range(1, 5);
        if (corner == 1)
        {
            Vector2 bombPos = transform.position;
            float bombPosX = bombPos.x + inputDistance;
            float bombPosY = bombPos.y + inputDistance;
            bombPos = new Vector2(bombPosX, bombPosY);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
        }
        if (corner == 2)
        {
            Vector2 bombPos = transform.position;
            float bombPosX = bombPos.x - inputDistance;
            float bombPosY = bombPos.y + inputDistance;
            bombPos = new Vector2(bombPosX, bombPosY);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
        }
        if (corner == 3)
        {
            Vector2 bombPos = transform.position;
            float bombPosX = bombPos.x + inputDistance;
            float bombPosY = bombPos.y - inputDistance;
            bombPos = new Vector2(bombPosX, bombPosY);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
        }
        if (corner == 4)
        {
            Vector2 bombPos = transform.position;
            float bombPosX = bombPos.x - inputDistance;
            float bombPosY = bombPos.y - inputDistance;
            bombPos = new Vector2(bombPosX, bombPosY);
            Instantiate(bombPrefab, bombPos, Quaternion.identity);
        }
    }
    #endregion

    #region Movement
    void PlayerMovement()
    {
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            velocity += acceleration * Time.deltaTime * Vector3.left;
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            velocity += acceleration * Time.deltaTime * Vector3.right;
        }
        if (Keyboard.current.upArrowKey.isPressed)
        {
            velocity += acceleration * Time.deltaTime * Vector3.up;
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
            velocity += acceleration * Time.deltaTime * Vector3.down;
        }
        if(!Keyboard.current.leftArrowKey.isPressed && !Keyboard.current.rightArrowKey.isPressed && !Keyboard.current.upArrowKey.isPressed && !Keyboard.current.downArrowKey.isPressed)
        {
            velocity -= deceleration * Time.deltaTime * velocity.normalized;
        }

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
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
    #endregion

    #region Misc

    void SpawnPowerUps(float inRadius, int numPowerUps)
    {
        float stepAngle = 360.0f / numPowerUps;
        List<GameObject> powerUps = new();

        stepAngle *= Mathf.Deg2Rad;
        float currentAngle = stepAngle;

        for (int i = 0; i < numPowerUps; i++)
        {
            float xPos = Mathf.Cos(currentAngle) * inRadius;
            float yPos = Mathf.Sin(currentAngle) * inRadius;

            Vector3 newPoint = new Vector3(xPos, yPos);
            powerUps.Add(powerupPrefab);
            Instantiate(powerUps[i], transform.position + newPoint, Quaternion.identity);
            currentAngle += stepAngle;
        }
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

    void PlayerRadar(float inRadius, int inNumSides)
    {
        float stepAngle = 360.0f / inNumSides;
        List<Vector3> points = new();

        stepAngle *= Mathf.Deg2Rad;
        float currentAngle = stepAngle;

        for (int i = 0; i < inNumSides; i++)
        {
            float xPos = Mathf.Cos(currentAngle) * inRadius;
            float yPos = Mathf.Sin(currentAngle) * inRadius;

            Vector3 newPoint = new Vector2(xPos, yPos);
            points.Add(newPoint);
            currentAngle += stepAngle;
        }

        for (int i = 0; i < inNumSides - 1; i++)
        {
            Vector3 startPoint = transform.position + points[i];
            Vector3 endPoint = transform.position + points[i + 1];

            Debug.DrawLine(startPoint, endPoint, radarColor);

            if (i == inNumSides - 2)
            {
                startPoint = transform.position + points[i + 1];
                endPoint = transform.position + points[0];

                Debug.DrawLine(startPoint, endPoint, radarColor);
            }
        }
        float radarDist = inRadius;
        if(Vector3.Distance(enemyScript.transform.position, transform.position) < radarDist)
        {
            radarColor = Color.red;
        }
        else
        {
            radarColor = Color.green;
        }
    }
    #endregion
}
