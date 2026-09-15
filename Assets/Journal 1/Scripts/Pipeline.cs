using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
using UnityEngine.InputSystem;

public class Pipeline : MonoBehaviour
{

    public Vector2 newPos;
    public Vector2 oldPos;
    public Vector2 firstPos;
    public float time;
    public bool posSet = false;
    public bool firstLine = false;
    public List<Vector2> points;
    public bool held = false;
    public int pointNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //Debug.Log(mousePos);
        if(Mouse.current.leftButton.IsPressed())
        {
            if (posSet == false)
            {
                oldPos = mousePos;
                posSet = true;
            }
            held = true;
            time += Time.deltaTime;
            if(time > 0.1)
            {
                
                //newPos = mousePos;
                //if (firstLine == false)
                //{
                //    firstPos = oldPos;
                //    firstLine = true;
                //}
                points.Add(oldPos);
                posSet = false;
                time = 0;

            }

        }
        
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            held = false;
            Vector2 final1 = new Vector2(points[0].x, points[0].y);
            Vector2 final2 = new Vector2(points[pointNumber].x, points[pointNumber].y);
            Vector2 finalVector = final1 + final2;
            float squareX = finalVector.x * finalVector.x;
            float squareY = finalVector.y * finalVector.y;
            float magOfFinal = Mathf.Sqrt(squareX + squareY);
            print(Mathf.Abs(magOfFinal));
        }
        if (held == true)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Debug.DrawLine(points[i], points[i + 1], Color.white);
                pointNumber = i;
            }
        }
    }
}
