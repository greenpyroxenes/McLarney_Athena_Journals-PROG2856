using UnityEngine;
using UnityEngine.InputSystem;

public class Pipeline : MonoBehaviour
{

    public Vector2 storedPos;
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
            storedPos = mousePos;
            Debug.Log(storedPos);
        }
    }
}
