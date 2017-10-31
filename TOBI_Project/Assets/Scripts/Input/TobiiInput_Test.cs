using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tobii = Tobii.Gaming.TobiiAPI;

using System.Reflection;

public class TobiiInput_Test : MonoBehaviour {

    private Vector2 m_tobiiViewPort = new Vector2(0.5f, 0.5f);
    private Vector2 m_tobiiScreenPoint = Vector2.zero;
    private Vector2 m_tobiiScreenPointUI = Vector2.zero;
    private Quaternion m_tobiiHeadRotation = Quaternion.identity;
    private Vector3 m_tobiiHeadPosition = Vector3.zero;

    private Camera m_tobiiViewPortCamera = null;


    private int callsPerFrame_tobiiViewPort = 0;        private bool wasUpdated_tobiiViewPort = false;
    private int callsPerFrame_tobiiScreenPoint = 0;     private bool wasUpdated_tobiiScreenPoint = false;
    private int callsPerFrame_tobiiScreenPointUI = 0;   private bool wasUpdated_tobiiScreenPointUI = false;
    private int callsPerFrame_tobbiHeadRotation = 0;    private bool wasUpdated_tobiiHeadRotation = false;
    private int callsPerFrame_tobiiHeadPosition = 0;    private bool wasUpdated_tobiiHeadPosition = false;

    private const int CALLS_PER_FRAME_TO_UPDATE_IN_FUNCTION = 2;

    [SerializeField] private bool trySmartUpdating = true;


   	private void Awake () {
        tobii.SetCurrentUserViewPointCamera((m_tobiiViewPortCamera == null ? Camera.main : m_tobiiViewPortCamera));
        tobii.SubscribeGazePointData();
        tobii.SubscribeHeadPoseData();

        Debug.Log(NameSpaceFound("Tobii.Gaming.TobiiAPI") ? "f" : "nf");

        try {
            System.Type t = System.Type.GetType("Tobii.Gaming.TobiiAPI");
            if(t.Namespace == "Tobii.Gaming") {
                Debug.Log("Found namespace");
            }
            if(t != null) {
                Debug.Log("Found");
            }
            else {
                Debug.Log("not Found");
            }
        }
        catch {
            Debug.Log("not Found");
        }
	}
	

    private void FixedUpdate() {
        if (trySmartUpdating)
        {
            callsPerFrame_tobiiViewPort = 0;
            callsPerFrame_tobiiScreenPoint = 0;
            callsPerFrame_tobiiScreenPointUI = 0;
            callsPerFrame_tobbiHeadRotation = 0;
            callsPerFrame_tobiiHeadPosition = 0;

            wasUpdated_tobiiViewPort = false;
            wasUpdated_tobiiScreenPoint = false;
            wasUpdated_tobiiScreenPointUI = false;
            wasUpdated_tobiiHeadRotation = false;
            wasUpdated_tobiiHeadPosition = false;
        }
    }


	private void Update () {
	    if(tobii.GetUserPresence() == Tobii.Gaming.UserPresence.Present || tobii.GetUserPresence() == Tobii.Gaming.UserPresence.Unknown) {
            if (UpdateInFunction(callsPerFrame_tobiiViewPort))          { m_tobiiViewPort = tobii.GetGazePoint().Viewport;
                                                                          wasUpdated_tobiiViewPort = true; }
            if (UpdateInFunction(callsPerFrame_tobiiScreenPoint))       { m_tobiiScreenPoint = tobii.GetGazePoint().Screen; 
                                                                          wasUpdated_tobiiScreenPoint = true; }
            if (UpdateInFunction(callsPerFrame_tobiiScreenPointUI))     { m_tobiiScreenPointUI = tobii.GetGazePoint().GUI; 
                                                                          wasUpdated_tobiiScreenPointUI = true; }
            if (UpdateInFunction(callsPerFrame_tobbiHeadRotation))      { m_tobiiHeadRotation = tobii.GetHeadPose().Rotation;
                                                                          wasUpdated_tobiiHeadRotation = true; }
            if (UpdateInFunction(callsPerFrame_tobiiHeadPosition))      { m_tobiiHeadPosition = tobii.GetHeadPose().Position; 
                                                                          wasUpdated_tobiiHeadPosition = true; }
        }
	}
     


    private bool UpdateInFunction(int calls) {
        return (calls >= CALLS_PER_FRAME_TO_UPDATE_IN_FUNCTION) || !trySmartUpdating;
    }



    public  Vector2 GazePointViewPort {
        get {
            if(wasUpdated_tobiiViewPort && trySmartUpdating) { 
                return m_tobiiViewPort;
            }
            else {
                return tobii.GetGazePoint().Viewport;
            }
        }
    }


    public Vector2 GazePointScreenPoint {
        get {
            if(wasUpdated_tobiiScreenPoint && trySmartUpdating) {
                return m_tobiiScreenPoint;
            }
            else {
                return tobii.GetGazePoint().Screen;
            }
        }
    }


    public Vector2 GazePointScreenPointUI {
        get {
            if(wasUpdated_tobiiScreenPointUI && trySmartUpdating) {
                return m_tobiiScreenPointUI;
            }
            else {
                return tobii.GetGazePoint().GUI;
            }
        }
    }


    public Quaternion UserHeadRotation {
        get {
            if(wasUpdated_tobiiHeadRotation && trySmartUpdating) { 
                return m_tobiiHeadRotation;
            }
            else {
                return tobii.GetHeadPose().Rotation;
            }
        }
    }


    public Vector3 UserHeadPosition {
        get {
            if(wasUpdated_tobiiHeadPosition && trySmartUpdating) { 
                return m_tobiiHeadPosition;
            }
            else {
                return tobii.GetHeadPose().Position;
            }
        }
    }


    public Camera UserViewPointCamera {
        get {
            return m_tobiiViewPortCamera;
        }
        set {
            tobii.SetCurrentUserViewPointCamera(value);
            m_tobiiViewPortCamera = value;
        }
    }


    public GameObject FocusedObject {
        get {
            return tobii.GetFocusedObject();
        }
    }



    public Ray GazeViewPortToRay() {
        if(wasUpdated_tobiiViewPort && trySmartUpdating) { 
            return m_tobiiViewPortCamera.ViewportPointToRay(m_tobiiViewPort);
        }
        else {
            return m_tobiiViewPortCamera.ViewportPointToRay(tobii.GetGazePoint().Viewport);
        }
    }


    public bool GazeRaycast(out RaycastHit hit, float maxDistance, LayerMask layerMask) {
        return Physics.Raycast(GazeViewPortToRay(), out hit, maxDistance, layerMask);
    }

    public bool GazeRaycast(out RaycastHit hit, float maxDistance = Mathf.Infinity) {
        return Physics.Raycast(GazeViewPortToRay(), out hit, maxDistance);
    }


    private bool NameSpaceFound(string exampleClassPath) {
        string[] s = exampleClassPath.Split('.');
        if (s.Length > 1)
        {
            string nameSpacePath = "";
            for(int i=0; i<s.Length-1; i++) {
                nameSpacePath += s[i];
                nameSpacePath += ".";
            }
            if(nameSpacePath.EndsWith(".")) {
                nameSpacePath = nameSpacePath.Substring(0, nameSpacePath.Length - 1);
            }
            return (System.Type.GetType(exampleClassPath).Namespace == nameSpacePath);
        }
        else
        {
            return false;
        }
    }


}
