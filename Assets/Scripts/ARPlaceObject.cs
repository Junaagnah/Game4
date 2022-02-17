using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceObject : MonoBehaviour
{
    public bool IsCubePlaced = false;

    [SerializeField]
    private GameObject _prefabToPlace;

    private ARRaycastManager _raycastManager;
    private ARAnchorManager _anchorManager;

    private static readonly List<ARRaycastHit> Hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Awake()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _anchorManager = GetComponent<ARAnchorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Touch touch;

        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) { return; }

        if (!IsCubePlaced && _raycastManager.Raycast(touch.position, Hits, TrackableType.Planes))
        {
            // Check for plane with lowest y
            ARRaycastHit lowestHit = Hits[0];

            Hits.ForEach(delegate (ARRaycastHit hit)
            {
                if (hit.pose.position.y < lowestHit.pose.position.y)
                {
                    lowestHit = hit;
                }
            });

            CreateAnchor(lowestHit);

            Debug.Log($"Instantiated on: {Hits[0].hitType}");
        }
    }

    ARAnchor CreateAnchor(in ARRaycastHit hit)
    {
        ARAnchor anchor = null;

        if (hit.trackable is ARPlane plane)
        {
            var planeManager = GetComponent<ARPlaneManager>();
            if (planeManager)
            {
                var oldPrefab = _anchorManager.anchorPrefab;
                _anchorManager.anchorPrefab = _prefabToPlace;
                anchor = _anchorManager.AttachAnchor(plane, hit.pose);
                _anchorManager.anchorPrefab = oldPrefab;

                IsCubePlaced = true;

                return anchor;
            }
        }

        return anchor;
    }
}
