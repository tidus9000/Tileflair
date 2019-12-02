using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] GameObject m_point;
    [SerializeField] int m_width;
    [SerializeField] int m_height;
    [SerializeField] float m_distanceBetweenPoints;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        for (int i = 0; i < m_width; i++)
        {
            pos.z = transform.position.z;
            for (int j = 0; j < m_height; j++)
            {
                GameObject.Instantiate(m_point, pos, Quaternion.identity, this.transform);
                pos.z += m_distanceBetweenPoints;
            }
            pos.x += m_distanceBetweenPoints;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Finds the closest point on the grid to a given point in world space
    /// </summary>
    /// <param name="_inputPoint">The given position in world space</param>
    /// <returns></returns>
    public Vector3 FindClosestPoint(Vector3 _inputPoint)
    {
        Vector3 output = Vector3.positiveInfinity;
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (Vector3.Distance(t.position, _inputPoint) < Vector3.Distance(output, _inputPoint))
            {
                output = t.position;
            }
        }
        return output;
    }
}
