using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RowGeneration : MonoBehaviour
{
    public int numberOfSquares;
    public string number;
    public int result;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateSquareNumber(string num)
    {
        if (int.TryParse(num, out result))
        {
            numberOfSquares = result;
        }
        Debug.Log(numberOfSquares);
    }
}
