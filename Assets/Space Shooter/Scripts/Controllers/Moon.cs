using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Moon : MonoBehaviour
{
    public Transform planetTransform;
    public float orbitRadius;
    public float speed;
    public float angle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OrbitalMotion(orbitRadius, speed, planetTransform);
    }

    void OrbitalMotion(float inRadius, float inSpeed, Transform inTarget)
    {
        angle += inSpeed * Time.deltaTime;
        if(angle > 360f)
        {
            angle = 0f;
        }

        float xPos = Mathf.Cos(angle * Mathf.Deg2Rad) * inRadius + inTarget.position.x;
        float yPos = Mathf.Sin(angle * Mathf.Deg2Rad) * inRadius + inTarget.position.y;
        Vector3 orbitPoint = new Vector3(xPos , yPos, 0);
        transform.position = orbitPoint;
         
    }
}
