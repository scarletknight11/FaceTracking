using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARFace))]
public class EyeTracker : MonoBehaviour {

    [SerializeField]
    private GameObject eyePrefab;

    private GameObject leftEye;
    private GameObject rightEye;

    private ARFace arface;

    // Start is called before the first frame update
    void Start()
    {
        arface = GetComponent<ARFace>();
    }

    void OnEnable()
    {
        ARFaceManager faceManager = FindObjectOfType<ARFaceManager>();
        if (faceManager != null && faceManager.subsystem != null && faceManager.subsystem.subsystemDescriptor.supportsEyeTracking)
        {
            arface.updated += OnUpdated;
            Debug.LogError("Eye Tracking is supported");
        }
        else
        {
            Debug.LogError("Eye Tracking is not supported on this device");
        }
    }

    void OnDisable() 
    {
        arface.updated -= OnUpdated;
        SetVisibility(false);
    }

    void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
    {
        if(arface.leftEye != null && leftEye == null)
        {
            leftEye = Instantiate(eyePrefab, arface.leftEye);
            leftEye.SetActive(false);
        }
        if (arface.rightEye != null && rightEye == null)
        {
            rightEye = Instantiate(eyePrefab, arface.rightEye);
            rightEye.SetActive(false);
        }
        
        bool shouldBeVisible = (arface.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready);

        // set visibilities
        SetVisibility(shouldBeVisible);
    }

    void SetVisibility(bool isVisible)
    {
        if(leftEye != null && rightEye != null)
        {
            leftEye.SetActive(isVisible);
            rightEye.SetActive(isVisible);
        }
    }

     
}
