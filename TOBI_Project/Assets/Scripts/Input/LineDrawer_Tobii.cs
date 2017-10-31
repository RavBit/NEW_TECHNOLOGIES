using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LineDrawer_Tobii : MonoBehaviour {

    private bool isDrawing = false;
    private List<Vector2> linePoints = new List<Vector2>();
    private Vector2[] smoothenedLine = new Vector2[1];
    private Vector2 lastPos = Vector2.zero;
    [SerializeField] private float lineThickness = 0.2f;
    [SerializeField] private float minOffset = 0.04f;
    [SerializeField] private float distanceFromCam = 10;
    [SerializeField] private Camera m_camera;
    [SerializeField] private int timesToSmooth = 2;

    [SerializeField] private int maxNumberOfLines = 1;
    private Mesh[] meshes = new Mesh[1];
    private Mesh[] colliderMeshes = new Mesh[1];
    private GameObject[] objects = new GameObject[1];
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float ColliderThickness = 1f;

    [SerializeField] private KeyCode resetKey = KeyCode.DownArrow;

    private void Awake() {
        meshes = new Mesh[maxNumberOfLines];
        colliderMeshes = new Mesh[maxNumberOfLines];
        objects = new GameObject[maxNumberOfLines];
        for(int i=0; i<maxNumberOfLines; i++) {
            meshes[i] = new Mesh();
            objects[i] = new GameObject();

            objects[i].AddComponent<MeshFilter>();
            objects[i].AddComponent<MeshRenderer>();
            objects[i].GetComponent<MeshFilter>().sharedMesh = meshes[i];

            objects[i].AddComponent<MeshCollider>();
            objects[i].GetComponent<MeshCollider>().sharedMesh = colliderMeshes[i];
        }
    }


    private void Update() {

        if(Input.GetKeyDown(resetKey)) {
            foreach(GameObject obj in objects) {
                obj.SetActive(false);
            }
        }

        foreach(GameObject obj in objects) {
            obj.transform.SetPositionAndRotation(m_camera.transform.position, m_camera.transform.rotation);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            StartDrawing();
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0)) {
            StopDrawing();
        }
        else if(isDrawing) {
            AddPoint();
        }
    }


    private void StartDrawing() {
        isDrawing = true;
        linePoints.Clear();
        lastPos = TobiiEasyInput.MousePostion;
        AddPoint();
    }
    private void StopDrawing() {
        isDrawing = false;
        if (linePoints.Count > 1)
        {
            smoothenedLine = CalculateLine(timesToSmooth);
        }
        if (smoothenedLine.Length > 1)
        {
            MakeLineGameobject(smoothenedLine);
        }
    }


    private void MakeLineGameobject(Vector2[] points) {

        
        Mesh[] mshes = new Mesh[maxNumberOfLines];
        for(int i=0; i<meshes.Length-1; i++) {
            mshes[i + 1] = meshes[i];
        }

        Mesh msh = LineAsMesh(points); //LineAsMeshCollider(points); //LineAsMesh(points);
        mshes[0] = msh;

        Mesh[] colmshes = new Mesh[maxNumberOfLines];
        for (int i = 0; i < colliderMeshes.Length - 1; i++)
        {
            colmshes[i + 1] = colliderMeshes[i];
        }

        Mesh colmsh = LineAsMeshCollider(points);
        colmshes[0] = colmsh;


        for (int i=0; i<objects.Length; i++) {
            if(objects[i].GetComponent<MeshFilter>() == null) { objects[i].AddComponent<MeshFilter>(); }
            if (objects[i].GetComponent<MeshRenderer>() == null) { objects[i].AddComponent<MeshRenderer>(); }
            objects[i].GetComponent<MeshFilter>().sharedMesh = mshes[i];
            
            objects[i].SetActive(true);

            if(objects[i].GetComponent<MeshCollider>() == null) { objects[i].AddComponent<MeshCollider>(); }
            objects[i].GetComponent<MeshCollider>().sharedMesh = colmshes[i];

            if (objects[i].GetComponent<MeshRenderer>().sharedMaterial == null)
            {
                objects[i].GetComponent<MeshRenderer>().sharedMaterial = lineMaterial;
            }
        }

        Destroy(meshes[meshes.Length - 1]);
        Destroy(colliderMeshes[colliderMeshes.Length - 1]);
        meshes = mshes;
        colliderMeshes = colmshes;


        /*
        for (int i=0; i<objects.Length-1; i++) {
            if(objects[i+1].GetComponent<MeshRenderer>() == null)   { objects[i+1].AddComponent<MeshRenderer>(); }
            if(objects[i].GetComponent<MeshRenderer>() == null)     { objects[i].AddComponent<MeshRenderer>(); }
            if (objects[i + 1].GetComponent<MeshFilter>() == null) { objects[i + 1].AddComponent<MeshFilter>(); }
            if (objects[i].GetComponent<MeshFilter>() == null) { objects[i].AddComponent<MeshFilter>(); }
            //objects[i + 1].GetComponent<MeshFilter>().sharedMesh.vertices = objects[i].GetComponent<MeshFilter>().sharedMesh.vertices;
            //objects[i + 1].GetComponent<MeshFilter>().sharedMesh.triangles = objects[i].GetComponent<MeshFilter>().sharedMesh.triangles;
        }
        if(objects[0].GetComponent<MeshFilter>() == null) { objects[0].AddComponent<MeshFilter>(); }
        if(objects[0].GetComponent<MeshRenderer>() == null) { objects[0].AddComponent<MeshRenderer>(); }
        */

        //objects[0].GetComponent<MeshFilter>().sharedMesh.vertices = msh.vertices;
        //objects[0].GetComponent<MeshFilter>().sharedMesh.triangles = msh.triangles;        

    }


    private T[] AddToArray<T>(T add, T[] arr) {
        T[] ret = arr;

        for(int i=0; i<ret.Length-1; i++) {
            ret[i + 1] = arr[i];
        }
        ret[0] = add;

        return ret;
    }


    private void AddPoint() {
        Vector2 pos = TobiiEasyInput.MousePostion;
        if (Vector2.Distance(lastPos, pos) >= minOffset) {
            linePoints.Add(pos);
            lastPos = pos;
        }
    }


    private Vector2[] CalculateLine(int smooth) {
        Vector2[] ret = linePoints.ToArray();
        List<Vector2> vec = new List<Vector2>();

        for(int i=0; i<smooth; i++) {
            vec.Add(ret[0]);
            for(int o=0; o<ret.Length-1; o++) {
                //vec.Add(ret[o]);
                vec.Add((ret[o] * 0.75f) + (ret[o + 1] * 0.25f));
                vec.Add((ret[o] * 0.25f) + (ret[o + 1] * 0.75f));
                //vec.Add()
            }
            vec.Add(ret[ret.Length - 1]);

            ret = vec.ToArray();
            vec.Clear();
        }

        return ret;
    }


    private Mesh LineAsMesh(Vector2[] points) {
        if(points.Length < 2) { return null; }
        Mesh ret = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> normals = new List<Vector3>();

        for(int i=0; i<points.Length-1; i++) {

            verts.Add(ToWorld(smoothenedLine[i], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[i + 1], distanceFromCam) - ToWorld(smoothenedLine[i], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[i], distanceFromCam) - m_camera.transform.position).normalized
                        ) * lineThickness / 2));
            verts.Add(ToWorld(smoothenedLine[i], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[i + 1], distanceFromCam) - ToWorld(smoothenedLine[i], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[i], distanceFromCam) - m_camera.transform.position).normalized
                        ) * -lineThickness / 2));

        }
        verts.Add(ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 2], distanceFromCam) - ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) - m_camera.transform.position).normalized
                        ) * lineThickness / 2));
        verts.Add(ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 2], distanceFromCam) - ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) - m_camera.transform.position).normalized
                        ) * -lineThickness / 2));
        
        for(int i=0; i<verts.Count-4; i+=2) {
            tris.Add(i);
            tris.Add(i + 1);
            tris.Add(i + 2);

            tris.Add(i + 2);
            tris.Add(i + 3);
            tris.Add(i + 1);


            tris.Add(i + 2);
            tris.Add(i + 1);
            tris.Add(i);

            tris.Add(i + 1);
            tris.Add(i + 3);
            tris.Add(i + 2);
        }

        for(int i=0; i<verts.Count; i++) {
            normals.Add(-m_camera.transform.forward);
        }


        ret.vertices = verts.ToArray();
        ret.triangles = tris.ToArray();
        ret.normals = normals.ToArray();

        return ret;
    }


    private Mesh LineAsMeshCollider(Vector2[] points)
    {
        if (points.Length < 2) { return null; }
        Mesh ret = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        for (int i = 0; i < points.Length - 1; i++)
        {

            verts.Add((ToWorld(smoothenedLine[i], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[i + 1], distanceFromCam) - ToWorld(smoothenedLine[i], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[i], distanceFromCam) - m_camera.transform.position).normalized
                        ) * lineThickness / 2))-m_camera.transform.forward*ColliderThickness/2);
            verts.Add((ToWorld(smoothenedLine[i], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[i + 1], distanceFromCam) - ToWorld(smoothenedLine[i], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[i], distanceFromCam) - m_camera.transform.position).normalized
                        ) * -lineThickness / 2))-m_camera.transform.forward*ColliderThickness/2);


            verts.Add((ToWorld(smoothenedLine[i], distanceFromCam) + (Vector3.Cross(
                             (ToWorld(smoothenedLine[i + 1], distanceFromCam) - ToWorld(smoothenedLine[i], distanceFromCam)).normalized
                             ,
                             (ToWorld(smoothenedLine[i], distanceFromCam) - m_camera.transform.position).normalized
                         ) * lineThickness / 2)) + m_camera.transform.forward * ColliderThickness / 2);
            verts.Add((ToWorld(smoothenedLine[i], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[i + 1], distanceFromCam) - ToWorld(smoothenedLine[i], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[i], distanceFromCam) - m_camera.transform.position).normalized
                        ) * -lineThickness / 2)) + m_camera.transform.forward * ColliderThickness / 2);

        }
        verts.Add((ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 2], distanceFromCam) - ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) - m_camera.transform.position).normalized
                        ) * lineThickness / 2))-m_camera.transform.forward*ColliderThickness/2);
        verts.Add((ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 2], distanceFromCam) - ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) - m_camera.transform.position).normalized
                        ) * -lineThickness / 2))-m_camera.transform.forward*ColliderThickness/2);



        verts.Add((ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 2], distanceFromCam) - ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) - m_camera.transform.position).normalized
                        ) * lineThickness / 2)) + m_camera.transform.forward * ColliderThickness / 2);
        verts.Add((ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 2], distanceFromCam) - ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[smoothenedLine.Length - 1], distanceFromCam) - m_camera.transform.position).normalized
                        ) * -lineThickness / 2)) + m_camera.transform.forward * ColliderThickness / 2);


        for (int i = 0; i < ((verts.Count) - 8); i += 4)
        {
            
            /*
            tris.Add(i);
            tris.Add(i + 1);
            tris.Add(i + 4);

            tris.Add(i + 1);
            tris.Add(i + 5);
            tris.Add(i + 4);

            tris.Add(i + 4);
            tris.Add(i + 1);
            tris.Add(i);

            tris.Add(i + 4);
            tris.Add(i + 5);
            tris.Add(i + 1);
            */
            

            tris.Add(i);
            tris.Add(i + 2);
            tris.Add(i + 4);

            tris.Add(i + 2);
            tris.Add(i + 6);
            tris.Add(i + 4);


            tris.Add(i + 1);
            tris.Add(i + 3);
            tris.Add(i + 5);

            tris.Add(i + 3);
            tris.Add(i + 7);
            tris.Add(i + 5);




            tris.Add(i + 4);
            tris.Add(i + 2);
            tris.Add(i);

            tris.Add(i + 4);
            tris.Add(i + 6);
            tris.Add(i + 2);


            tris.Add(i + 5);
            tris.Add(i + 3);
            tris.Add(i + 1);

            tris.Add(i + 5);
            tris.Add(i + 7);
            tris.Add(i + 3);
        }

        ret.vertices = verts.ToArray();
        ret.triangles = tris.ToArray();

        return ret;
    }



    private Vector3 ToWorld(Vector2 point, float distance) {
        Vector3 ray = m_camera.transform.InverseTransformDirection(TobiiEasyInput.MouseToRay(point, m_camera).direction);
        float mp = distance / ray.z;
        ray *= mp;
        return ray;
    }


    #if UNITY_EDITOR
    private void OnDrawGizmos() {

        Gizmos.color = Color.green;

        Vector2[] points = linePoints.ToArray();
        if (points.Length > 1) {
            for (int i = 0; i < points.Length - 1; i++) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ToWorld(points[i], distanceFromCam), ToWorld(points[i + 1], distanceFromCam));
            }
        }
        
        if(smoothenedLine.Length > 1) {
            for(int i=0; i<smoothenedLine.Length-1; i++) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(ToWorld(smoothenedLine[i], distanceFromCam), ToWorld(smoothenedLine[i + 1], distanceFromCam));

                if(true) {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(
                        ToWorld(smoothenedLine[i], distanceFromCam)
                    ,
                        ToWorld(smoothenedLine[i], distanceFromCam)+(Vector3.Cross(
                            (ToWorld(smoothenedLine[i + 1], distanceFromCam) - ToWorld(smoothenedLine[i], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[i], distanceFromCam) - m_camera.transform.position).normalized
                        )*lineThickness/2)
                    
                    );
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(
                        ToWorld(smoothenedLine[i], distanceFromCam)
                    ,
                        ToWorld(smoothenedLine[i], distanceFromCam) + (Vector3.Cross(
                            (ToWorld(smoothenedLine[i + 1], distanceFromCam) - ToWorld(smoothenedLine[i], distanceFromCam)).normalized
                            ,
                            (ToWorld(smoothenedLine[i], distanceFromCam) - m_camera.transform.position).normalized
                        ) * -lineThickness / 2)

                    );
                }

            }
        }

    }
    #endif	

}
