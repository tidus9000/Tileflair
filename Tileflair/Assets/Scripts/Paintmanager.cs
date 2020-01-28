using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintmanager : MonoBehaviour
{
    [SerializeField] Transform m_activeWallLocation;
    [SerializeField] GameObject m_buttons;
    [SerializeField] float rotateangle = 0;
    Vector3 m_originalWallLocation;
    Quaternion m_originalWallRotation;
    int m_activeWall;
    List<Transform> walls = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialise()
    {
        GameObject room = GameObject.Find("Room");
        if (room)
        {
            m_buttons.SetActive(true);
            //Store the wall location
            foreach (Transform child in room.transform)
            {
                walls.Add(child);
            }
            if (walls.Count > 0)
            {
                walls.RemoveAt((walls.Count) - 1);
                m_activeWall = 0;
                m_originalWallLocation = walls[m_activeWall].position;
                m_originalWallRotation = GetWallChildRotation();
                //move the wall to the location of the camera
                walls[m_activeWall].position = m_activeWallLocation.position;
                RotateActiveWallToMinus90();
                //rotate the camera around the wall
                //-90
                //get the walls rotation

                /*Vector3 midpoint = GetMidpointOfActiveWall();
                foreach(Transform child in walls[m_activeWall])
                {
                    child.RotateAround(midpoint, Vector3.up, child.rotation.y);
                }*/

            }
        }
        else
        {
            Debug.Log("No room loaded");
        }
    }

    public void ChangeWall(int _index)
    {
        if (_index >= 0 && _index < walls.Count)
        {
            //move the old active wall back to the room
            walls[m_activeWall].position = m_originalWallLocation;
            //walls[m_activeWall].rotation = m_originalWallRotation;
            RotateActiveWallToAngle(m_originalWallRotation.eulerAngles.y);


            //store the new walls original location
            m_activeWall = _index;
            m_originalWallLocation = walls[m_activeWall].position;
            m_originalWallRotation = GetWallChildRotation();
            //move the new active wall to the camera
            walls[m_activeWall].position = m_activeWallLocation.position;
            RotateActiveWallToMinus90();
            /*Vector3 midpoint = GetMidpointOfActiveWall();
            foreach (Transform child in walls[m_activeWall])
            {
                child.RotateAround(midpoint, Vector3.up, child.rotation.y);
            }*/

            /*Camera.main.transform.RotateAround(GetMidpointOfActiveWall(), Vector3.up,
                walls[m_activeWall].GetComponentInChildren<Transform>().rotation.y + 90);*/
        }
        else
        {
            Debug.Log("Given wall index is out of range");
        }
    }

    /// <summary>
    /// Goes to the next wall on the list
    /// </summary>
    public void NextWall()
    {
        if (m_activeWall >= walls.Count - 1)
        {
            ChangeWall(0);
        }else
        {
            ChangeWall(m_activeWall + 1);
        }
    }

    /// <summary>
    /// Goes to the previous wall on the list
    /// </summary>
    public void PreviousWall()
    {
        if (m_activeWall <= 0)
        {
            ChangeWall(walls.Count - 1);
        }
        else
        {
            ChangeWall(m_activeWall - 1);
        }
    }

    public void End()
    {
        m_buttons.SetActive(false);
        walls[m_activeWall].position = m_originalWallLocation;
        //walls[m_activeWall].rotation = m_originalWallRotation;
        RotateActiveWallToAngle(m_originalWallRotation.eulerAngles.y);
        walls.Clear();
    }

    /// <summary>
    /// returns the midpoint of the active wall
    /// </summary>
    /// <returns></returns>
    Vector3 GetMidpointOfActiveWall()
    {
        Vector3 midPoint = Vector3.zero;
        int count = 0;

        foreach(Transform child in walls[m_activeWall])
        {
            midPoint += child.position;
            count++;
        }

        midPoint /= count;
        return midPoint;
    }

    [ContextMenu("Rotate")]
    public void RotateActiveWallToMinus90()
    {
        //I want the angle to be -90
        Vector3 midpoint = GetMidpointOfActiveWall();
        foreach (Transform child in walls[m_activeWall])
        {
            float angle = child.localRotation.y + 90;
            Quaternion rotation = child.rotation;
            angle = rotation.eulerAngles.y + 90;
            child.RotateAround(midpoint, Vector3.up, -angle);
        }
    }

    void RotateActiveWallToAngle(float _angle)
    {
        Vector3 midpoint = GetMidpointOfActiveWall();
        foreach (Transform child in walls[m_activeWall])
        {
            if (child.CompareTag("Wall"))
            {
                Quaternion rotation = child.rotation;
                float angle = rotation.eulerAngles.y + (_angle * -1);
                child.RotateAround(midpoint, Vector3.up, -angle);
            }
        }
    }

    /// <summary>
    /// Returns the selected Wall
    /// </summary>
    /// <returns></returns>
    public GameObject GetActiveWall()
    {
        return walls[m_activeWall].gameObject;
    }

    /// <summary>
    /// Returns the rotation of a wall block
    /// </summary>
    /// <returns></returns>
    Quaternion GetWallChildRotation()
    {
        return walls[m_activeWall].GetChild(0).transform.rotation;
    }
}
