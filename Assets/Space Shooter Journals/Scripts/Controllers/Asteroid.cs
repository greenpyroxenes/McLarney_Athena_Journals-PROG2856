using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float arrivalDistance;
    public float maxFloatDistance = 2f;
    public Vector3 randomPos;

    // Start is called before the first frame update
    void Start()
    {
        CreateRandomPos();
    }

    // Update is called once per frame
    void Update()
    {
        AsteroidMovement(maxFloatDistance);
    }

    void AsteroidMovement(float inMaxDist)
    {
        if (inMaxDist > Vector3.Distance(transform.position, randomPos))
        {
            transform.position += moveSpeed * randomPos.normalized * Time.deltaTime;
        }
        if(arrivalDistance < Vector3.Distance(transform.position, randomPos))
        {
            CreateRandomPos();
        }
    }

    void CreateRandomPos()
    {
        randomPos = Random.insideUnitCircle * (maxFloatDistance - 0.5f);
    }
}
