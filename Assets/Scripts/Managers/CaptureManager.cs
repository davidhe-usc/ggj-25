using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureManager : MonoBehaviour
{
    [SerializeField] private Camera camera;

    [SerializeField] private GameObject captureHead;
    [SerializeField] private CaptureLine captureLine;
    private CaptureLine currentLine;
    private ParticleSystem trails;
    private bool _canCapture;
    public bool canCapture
    {
        get { return _canCapture; }
        set
        {
            _canCapture = value;
            toggleParticles(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canCapture = false;
        trails = captureHead.GetComponentInChildren<ParticleSystem>();
        if (trails != null)
        {
            trails.Stop();
        }   
    }

    // Update is called once per frame
    void Update()
    {
        if (canCapture)
        {
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            captureHead.transform.position = mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                currentLine = Instantiate(captureLine, mousePosition, Quaternion.identity);
            }

            else if (Input.GetMouseButton(0) && currentLine != null)
            {
                currentLine.SetPosition(mousePosition);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            GameObject[] captureLines = GameObject.FindGameObjectsWithTag("Line");
            foreach (GameObject line in captureLines)
            {
                GameObject.Destroy(line);
            }
        }

    }
    public void toggleParticles(bool input)
    {
        if (trails != null)
        {
            if (input) {
                trails.Play();

            }
            else {
                trails.Stop(); 
            }
        }
    }
}

