using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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
    public GameObject playerRocketPrefab;
    public GameObject playerShipPrefab;
    public List<GameObject> playerShips = new();
    public float bombAngle = 0f;
    bool spinBombSpawn = false;
    public float bombSpin = 10f;
    public float bombSpinRadius = 1f;
    public bool bladeSpawn = false;
    public int numOfBlades = 4;
    public float bladeDist = 1f;
    public float bladeSpeed = 1f;
    public float bladeRotate = 0f;
    public GameObject copyBomb;
    public float shipRotate = 1f;
    List<GameObject> cloneBladeList = new();
    public float collisionDist = 1f;
    public bool bombLaunch = false;
    public float bombLaunchSpeed = 10f;
    float bombExplodeDist = 1f;
    public float time = 0;
    public float bladeOrbitSpeed = 1f;
    bool collide = false;
    public float spinTime = 0;
    [Tooltip("Measured in Degrees per second.")]
    public float angularSpeed = 270f;

    void Start()
    {
        acceleration = maxSpeed / accelerationTime;
        deceleration = maxSpeed / decelerationTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (collide == false)
        {
            if (Keyboard.current.oKey.wasPressedThisFrame)
            {
                SpawnBombAtOffset(bombOffset);
            }
            if (Keyboard.current.tKey.wasPressedThisFrame)
            {
                SpawnBombTrail(bombTrailSpacing, numberOfBombs);
            }
            if (Keyboard.current.cKey.wasPressedThisFrame)
            {
                SpawnBombOnCorner(cornerDist);
            }
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                WarpPlayer(enemyScript.transform, ratio);
            }
            if (Keyboard.current.rKey.isPressed)
            {
                DetectAsteroids(maxRadar, asteroidTransforms);
            }
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                SpawnPowerUps(powerUpRadius, numOfPowerUp);
            }
            if (Keyboard.current.qKey.wasPressedThisFrame && bombLaunch == false && spinBombSpawn == false)
            {
                SpawnRotatingBomb();
                spinBombSpawn = true;
            }
            if (spinBombSpawn == true)
            {
                BombOrbit();
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    bombLaunch = true;
                    spinBombSpawn = false;
                }
            }
            if (bombLaunch == true)
            {
                LaunchBomb();
            }
            if (Keyboard.current.bKey.wasPressedThisFrame && bladeSpawn == false)
            {
                SpawnBlades(bladeDist, numOfBlades);
                bladeSpawn = true;
            }
            if (bladeSpawn == true)
            {
                BladeRotate(bladeSpeed);
            }
            PlayerMovement();
            PlayerRadar(radarRadius, numSides);
            playerRotation();
        }
        if (collide == false)
        {
            EnemyCollide();
        }
        if(collide == true)
        {
            ShipSpin();
        }
    }

    #region Bombs and Weapons
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
    void SpawnRotatingBomb()
    {
        Vector3 bombPos = transform.position;
        float bombPosY = bombPos.y + 2;
        float bombPosX = bombPos.x;
        float bombPosZ = bombPos.z;
        bombPos = new Vector3(bombPosX, bombPosY, bombPosZ);
        copyBomb = Instantiate(bombPrefab, bombPos, Quaternion.identity);
    }
    void BombOrbit()
    {
        bombAngle += bombSpin * Time.deltaTime;
        if (bombAngle > 360f)
        {
            bombAngle = 0f;
        }

        float xPos = Mathf.Cos(bombAngle * Mathf.Deg2Rad) * bombSpinRadius + transform.position.x;
        float yPos = Mathf.Sin(bombAngle * Mathf.Deg2Rad) * bombSpinRadius + transform.position.y;
        Vector3 orbitPoint = new Vector3(xPos, yPos, 0);
        copyBomb.transform.position = orbitPoint;
    }

    void LaunchBomb()
    {
        Vector3 launchAngle = new Vector3 (Mathf.Cos(bombAngle * Mathf.Deg2Rad), Mathf.Cos(bombAngle * Mathf.Deg2Rad));
        Debug.Log(launchAngle);
        Vector3 direction = launchAngle.normalized - Vector3.forward;
        copyBomb.transform.position += Time.deltaTime * bombLaunchSpeed * direction;
        if(Vector3.Distance(enemyScript.transform.position, copyBomb.transform.position) < bombExplodeDist)
        {
            Destroy(copyBomb);
            bombLaunch = false;
        }
        time += Time.deltaTime;
        if(time > 4)
        {
            Destroy(copyBomb);
            bombLaunch = false;
            time = 0;
        }
    }
    public void SpawnBlades(float inRadius, int inNumOfBlades)
    {
        float stepAngle = 360.0f / inNumOfBlades;

        stepAngle *= Mathf.Deg2Rad;
        float currentAngle = stepAngle;
        for (int i = 0; i < inNumOfBlades; i++)
        {
            float xPos = Mathf.Cos(currentAngle) * inRadius;
            float yPos = Mathf.Sin(currentAngle) * inRadius;

            Vector3 newPoint = new Vector3(xPos, yPos);
            playerShips.Add(playerShipPrefab);
            cloneBladeList.Add(playerShipPrefab);
            cloneBladeList[i] = Instantiate(playerShips[i], transform.position + newPoint, Quaternion.identity);
            currentAngle += stepAngle;
        }
    }
    void BladeRotate(float inBladeSpeed)
    {
        bladeRotate += inBladeSpeed * Time.deltaTime;
        if (bladeRotate > 360)
        {
            bladeRotate = 0;
        }
        for (int i = 0; i < cloneBladeList.Count; i++)
        {
            cloneBladeList[i].transform.Rotate(0, 0, bladeRotate);
        }
    }


    #endregion

    #region Movement
    void PlayerMovement()
    {
        Vector3 playerInput = Vector3.zero;

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            playerInput += Vector3.left;
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            playerInput += Vector3.right;
        }
        if (Keyboard.current.upArrowKey.isPressed)
        {
            playerInput += Vector3.up;
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
            playerInput += Vector3.down;
        }

        if (playerInput.sqrMagnitude > 0) //input detected
        {
            velocity += acceleration * Time.deltaTime * playerInput.normalized; //acelerate

            if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            }
        }
        else //not detected
        {
            float deltaVelocity = deceleration * Time.deltaTime;

            if (velocity.magnitude > deltaVelocity) //if we still need to slow down
            {
                velocity -= deceleration * Time.deltaTime * velocity.normalized; //decelerate
            }
            else
            {
                velocity = Vector3.zero; //complete stop
            }
        }
        transform.position += velocity * Time.deltaTime;
    }

    void playerRotation()
    {
        if (velocity.sqrMagnitude == 0) return; //not moving, nothing

        float deltaAngle = ComputeShortestAngle(transform.up, velocity);

        if (deltaAngle == 0) return; //locked on nothing

        float rotationDirection = Mathf.Sign(deltaAngle);
        float rotationStep = angularSpeed * Time.deltaTime;

        if (rotationStep < Mathf.Abs(deltaAngle)) //still need to rotate
        {
            transform.Rotate(0, 0, rotationDirection * rotationStep); //rotate relative to angular speed
        }
        else
        {
            transform.Rotate(0, 0, deltaAngle); //lock-on
        }
    }

    private float ComputeShortestAngle(Vector3 a, Vector3 b)
    {
        float angleA = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(b.y, b.x) * Mathf.Rad2Deg;

        return Mathf.DeltaAngle(angleA, angleB);
    }

    void EnemyCollide()
    {
        if(Vector3.Distance(enemyScript.transform.position, transform.position) < collisionDist)
        {
            collide = true;
        }
    }

    void ShipSpin()
    {
        spinTime += Time.deltaTime;
        if (spinTime < 2)
        {
            shipRotate += 0.5f * Time.deltaTime;
            if (shipRotate > 360)
            {
                shipRotate = 0;
            }
            transform.Rotate(0, 0, shipRotate);
            velocity -= deceleration * Time.deltaTime * velocity.normalized;
            transform.position += velocity * Time.deltaTime;
        }
        if(spinTime > 2)
        {
            spinTime = 0;
            collide = false;
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

    void EnemyCollision()
    {

    }
    #endregion
}
