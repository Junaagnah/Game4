using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Canvas BeginText;
    [SerializeField]
    private Canvas Sliders;
    [SerializeField]
    private ARSessionOrigin Session;

    private ARPlaceObject ARSessionScript;
    private bool CanEditCube = false;

    private void Awake()
    {
        Sliders.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get script from component
        ARSessionScript = Session.GetComponent<ARPlaceObject>();
    }

    // Update is called once per frame
    void Update()
    {

        // Check if cube has been placed (would be better with events but I have big flemme
        if (!CanEditCube && ARSessionScript.IsCubePlaced)
        {
            BeginText.enabled = false;
            Sliders.enabled = true;
            CanEditCube = true; // Avoid checking variable from session origin each frame
        }

        if (CanEditCube)
        {
            // Get RVB values from sliders
            float red = GameObject.FindGameObjectWithTag("REDSLIDER").GetComponent<Slider>().value;
            float green = GameObject.FindGameObjectWithTag("GREENSLIDER").GetComponent<Slider>().value;
            float blue = GameObject.FindGameObjectWithTag("BLUESLIDER").GetComponent<Slider>().value;

            GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>().material.color = new Color(red, green, blue);
        }
    }
}
