using UnityEngine;
using UnityEngine.InputSystem;

public class SquareSpawner : MonoBehaviour
{
    Vector3 mousePos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        float mousePosX = mousePos.x;
        float mousePosY = mousePos.y;
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 line1 = new Vector2(mousePosX, mousePosY + 1);
            Vector2 line2 = new Vector2(mousePosX + 1, mousePosY);
            Vector2 line3 = new Vector2(mousePosX + 1, mousePosY + 1);
            Vector2 line4 = new Vector2(mousePosX + 1, mousePosY + 1);
            Debug.DrawLine(mousePos, line1);
            Debug.DrawLine(mousePos, line2);
            Debug.DrawLine(line1, line3);
            Debug.DrawLine(line2, line4);
            
        }
    }
}
