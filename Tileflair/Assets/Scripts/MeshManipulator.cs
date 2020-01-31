﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManipulator : MonoBehaviour
{
    Mesh mesh;
    [SerializeField]Vector3[] Vertices;
    [SerializeField]Vector3[] WorldVertices;
    [SerializeField]Vector3[] m_floorWorldVerts;
    bool m_add;
    MouseInputBuild m_buildManager;
 

// Start is called before the first frame update
void Start()
    {
        m_buildManager = GameObject.Find("MouseManager").GetComponent<MouseInputBuild>();
        mesh = GetComponent<MeshFilter>().mesh;
        Vertices = mesh.vertices;
        WorldVertices = new Vector3[Vertices.Length];
    }

    // Update is called once per frame
    void Update()
    {
        //Vertices[0].y += 1 * Time.deltaTime;
        for (int i = 0; i < WorldVertices.Length; i++)
        {
            WorldVertices[i] = transform.TransformPoint(Vertices[i]);
        }

        if (m_add)
        {
            WorldVertices[0].y += 1;
            m_add = false;
        }

        for (int i = 0; i < Vertices.Length; ++i)
        {
            Vertices[i] = transform.InverseTransformPoint(WorldVertices[i]);
        }

        mesh.vertices = Vertices;
        //mesh.RecalculateBounds();
    }

    [ContextMenu("Generate Floor")]
    public void Add1()
    {
        transform.position = Vector3.zero;
        m_floorWorldVerts = m_buildManager.GetRoomFloorVerts();

        for (int i = 0; i < m_floorWorldVerts.Length; i++)
        {
            Vertices[i] = transform.InverseTransformPoint(m_floorWorldVerts[i]);
        }

        int[] newTriangles = new int[(m_floorWorldVerts.Length * 3) - 102];
        int[] triangles = mesh.triangles;
        for (int i = 0; i < newTriangles.Length; i++)
        {
            newTriangles[i] = triangles[i];
        }

        mesh.triangles = newTriangles;
    }

    /// <summary>
    /// Finds the centre point of multiple points
    /// </summary>
    /// <returns></returns>
    Vector3 FindCenter(Vector3[] Points)
    {
        Vector3 center = Vector3.zero;
        foreach (Vector3 v3 in Points)
        {
            center += v3;
        }
        return center / Points.Length;
    }

  
}
