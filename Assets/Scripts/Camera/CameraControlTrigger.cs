using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class CameraControlTrigger : MonoBehaviour
{
    public CameraManager _cameraManager;
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D coll;
    private LayerMask playerLayer;

    private void Awake()
    {
        _cameraManager = CameraManager.instance;
        coll = GetComponent<Collider2D>();
        playerLayer = 1 << LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            if (customInspectorObjects.panCameraOnContact)
            {
                _cameraManager.PanCameraOnContact(customInspectorObjects.panDistance,
                    customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
       
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            Vector2 exitDirection = (other.transform.position - coll.bounds.center).normalized;
            
            if (customInspectorObjects.swapCameras && customInspectorObjects.cameraOnLeft != null &&
                customInspectorObjects.cameraOnRight != null)
            {
                _cameraManager.SwapCamera(customInspectorObjects.cameraOnLeft,
                    customInspectorObjects.cameraOnRight, exitDirection);
            }
            
            if (customInspectorObjects.panCameraOnContact)
            {
                _cameraManager.PanCameraOnContact(customInspectorObjects.panDistance,
                    customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}

# region Custom
[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;
    
    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;
    
    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}

[CustomEditor((typeof(CameraControlTrigger)))]
public class MyScriptEditor : Editor
{
    private CameraControlTrigger cameraControlTrigger;

    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        CustomInspectorObjects customInspector = cameraControlTrigger.customInspectorObjects;
        
        DrawDefaultInspector();
        if (customInspector.swapCameras)
        {
            customInspector.cameraOnLeft = EditorGUILayout.ObjectField("Camera on Left",
                customInspector.cameraOnLeft, typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
            
            customInspector.cameraOnRight = EditorGUILayout.ObjectField("Camera on Right",
                customInspector.cameraOnRight, typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }

        if (customInspector.panCameraOnContact)
        {
            customInspector.panDirection =
                (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction", customInspector.panDirection);
            customInspector.panDistance = EditorGUILayout.FloatField("Pan Distance", customInspector.panDistance);
            customInspector.panTime = EditorGUILayout.FloatField("Pan Time", customInspector.panTime);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraControlTrigger);
        }
    }
}
#endregion



