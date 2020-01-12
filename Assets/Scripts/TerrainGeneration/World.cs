using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public bool autoUpdate = true;
    [Range(2, 256)] //256**2 is about the maximum number of vertices a mesh can have
    public int resolution = 10;
    [SerializeField, HideInInspector] //Serialize so it saves in the editor but hide it from showing up in the inspector window
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;
    MeshCollider[] meshColliders;

    public int seed;

    public ShapeSettings shapeSettings;

    ShapeGenerator shapeGenerator = new ShapeGenerator();

    [HideInInspector]
    public bool shapeSettingsFoldout;

    private void Start()
    {
        GeneratePlanet();
    }

    public void GeneratePlanet()
    {
        Initialise();
        GenerateMesh();
        //GenerateColours(); //No automated colour generation (yet)
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialise();
            GenerateMesh();
        }
    }

    void Initialise()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[1]; //Changed six to 1 because we are only interested in the Vector3.up case
        }

        terrainFaces = new TerrainFace[1]; //Changed six to 1 because we are only interested in the Vector3.up case

        for (int i = 0; i < 1; i++) //Iterate over 6 faces of cube
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("mesh");
                meshObject.transform.parent = transform; //Add the new meshobject to current transform

                meshObject.AddComponent<MeshRenderer>();
                meshObject.AddComponent<MeshCollider>();
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
                meshObject.GetComponent<MeshCollider>().sharedMesh = meshFilters[i].sharedMesh;
                meshObject.tag = "ground";
                meshObject.AddComponent<Rigidbody>();
                meshObject.GetComponent<Rigidbody>().isKinematic = true;
                meshObject.GetComponent<Rigidbody>().useGravity = false;
                Debug.Log(meshObject.GetComponent<Rigidbody>());
                meshObject.transform.localScale = new Vector3(100, 100, 100);
            }
            if (meshFilters[i].sharedMesh == null)
            {
                meshFilters[i].sharedMesh = new Mesh();
            }
            //meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution);
            bool renderFace = true; //faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
            // Debug.Log(meshFilters[i].gameObject);
            //meshColliders[i] = meshFilters[i].gameObject.AddComponent<MeshCollider>() as MeshCollider;
            // meshColliders[i].sharedMesh = meshFilters[i].mesh;
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 1; i++) //Changed 6 to 1
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
            GameObject meshObject = meshFilters[i].gameObject;
            if (meshObject.GetComponent<MeshCollider>() == null)
            {
                meshObject.AddComponent<MeshCollider>();
            }
            meshObject.GetComponent<MeshCollider>().sharedMesh = meshFilters[i].sharedMesh;
            meshObject.tag = "ground";
            if (meshObject.GetComponent<Rigidbody>() == null)
            {
                meshObject.AddComponent<Rigidbody>();
            }
            meshObject.GetComponent<Rigidbody>().isKinematic = true;
            meshObject.GetComponent<Rigidbody>().useGravity = false;
            Debug.Log(meshObject.GetComponent<Rigidbody>());
        }

        //colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }
}
