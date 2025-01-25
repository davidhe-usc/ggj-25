using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureManager : MonoBehaviour
{
    public Camera camera;

    public CaptureLine captureLine;
    private CaptureLine currentLine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            currentLine = Instantiate(captureLine, mousePosition, Quaternion.identity);
        }

        if (Input.GetMouseButton(0))
        {
            currentLine.SetPosition(mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            GameObject[] captureLines = GameObject.FindGameObjectsWithTag("Line");
            foreach(GameObject line in captureLines)
            {
                GameObject.Destroy(line);
            }
        }
    }
}
