using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;
    public float drawingTime;

    private Vector3 currentPosition;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public AnimationCurve curve;

    // Update is called once per frame
    void Update()
    {
        drawingTime += Time.deltaTime;
        if(drawingTime > 1)
        {
            drawingTime = 0;
        }
        DrawConstellation();
    }

    private void DrawConstellation()
    {
        for (int i = 0; i < starTransforms.Count; i++)
        {
            startPosition = starTransforms[i].position;
            endPosition = starTransforms[i + 1].position;
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, curve.Evaluate(drawingTime));
            Debug.DrawLine(startPosition, currentPosition);
        }
    }
}
