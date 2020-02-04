using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputBuild : MonoBehaviour
{

    //Build State Variables
    [SerializeField] GameObject m_point;
    GameManager m_manager;
    GameObject m_startObj;
    GameObject m_room;
    Vector3 m_posDown;
    Vector3 m_posUp;
    GridManager m_grid;
    bool m_mousedown = false;

    //Floor
    GameObject m_floor;
    Vector3 defaultFloorPos;

    //Place State Variables
    [SerializeField] GameObject m_selectedRoomObject;
    GameObject m_activeObject;
    bool m_rightMouseheld;
    public RaycastHit m_hit = new RaycastHit();

    //View State Variables
    public Transform target;
    public float distance = 2.0f;
    public float xSpeed = 20.0f;
    public float ySpeed = 20.0f;
    public float yMinLimit = -90f;
    public float yMaxLimit = 90f;
    public float distanceMin = 10f;
    public float distanceMax = 10f;
    public float smoothTime = 2f;
    float rotationYAxis = 0.0f;
    float rotationXAxis = 0.0f;
    float velocityX = 0.0f;
    float velocityY = 0.0f;



    // Start is called before the first frame update
    void Start()
    {
        //Set up for build State
        m_grid = GameObject.Find("GridManager").GetComponent<GridManager>();
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_room = new GameObject("Room");

        //Setup for the floor in view mode
        m_floor = GameObject.Find("Floor");
        defaultFloorPos = m_floor.transform.position;

        //Setup for View State
        Vector3 angles = transform.eulerAngles;
        rotationYAxis = angles.y;
        rotationXAxis = angles.x;
        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_manager.GetState())
        {
            case GameManager.State.BUILD:
                if (m_manager.GetPreviousState() == GameManager.State.VIEW 
                    || m_manager.GetPreviousState() == GameManager.State.PLACE)
                {
                    m_floor.transform.position = defaultFloorPos;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    //Spawn an initial wall object on the grid where the user has clicked
                    m_posDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    m_posDown = m_grid.FindClosestPoint(m_posDown);
                    m_startObj = GameObject.Instantiate(m_point, m_posDown, Quaternion.identity, m_room.transform);
                    m_mousedown = true;
                }
                if (m_mousedown)
                {
                    //rotate the initial object to where the mouse is pointing
                    if (m_startObj)
                    {
                        Vector3 closestPoint = m_grid.FindClosestPoint(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
                        m_startObj.transform.LookAt(closestPoint);
                        if (closestPoint != m_posDown)
                        {
                            float scale = Vector3.Distance(closestPoint, m_posDown);
                            Vector3 newscale = m_startObj.transform.localScale;
                            newscale.z = scale;
                            m_startObj.transform.localScale = newscale;
                            m_startObj.transform.position = (closestPoint + m_posDown) / 2;
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    ///*****
                    ///This was an old method I used for making the walls. It was bad but I'm keeping it here for the purposes of documentation
                    ///*****
                    ////Calculate how many wall objects need to be made and Spawn them up to the point where the user has let go
                    //m_posUp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    //m_posUp = m_grid.FindClosestPoint(m_posUp);


                    //float distance = Vector3.Distance(m_posDown, m_posUp);
                    //int noObjects = (int)(distance / m_point.transform.localScale.x);
                    //Vector3 divisionsVec = m_posUp - m_posDown;
                    //divisionsVec /= noObjects;

                    //for (int i = 0; i < noObjects; i++)
                    //{
                    //    m_posDown += divisionsVec;
                    //    GameObject.Instantiate(m_point, m_posDown, m_startObj.transform.rotation, m_wall.transform);
                    //}

                    m_mousedown = false;
                }
                break;
            case GameManager.State.PAINT:
                if (m_manager.GetPreviousState() == GameManager.State.BUILD)
                {
                    Destroy(m_startObj);
                }
                if (m_manager.GetPreviousState() == GameManager.State.VIEW
                    || m_manager.GetPreviousState() == GameManager.State.PLACE)
                {
                    m_floor.transform.position = defaultFloorPos;
                }
                break;
            case GameManager.State.PLACE:
                if (m_manager.GetPreviousState() == GameManager.State.BUILD)
                {
                    Destroy(m_startObj);
                }
                if (m_manager.GetPreviousState() != GameManager.State.VIEW)
                {
                    target = m_room.transform;
                    Vector3 newpos = Vector3.zero;
                    newpos.y = -1.4f;
                    m_floor.transform.position = newpos;
                }
                if (target)
                {

                    if (Input.GetMouseButton(0))
                    {
                        velocityX += (xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f) * Time.deltaTime;
                        velocityY += (ySpeed * Input.GetAxis("Mouse Y") * 0.02f) * Time.deltaTime;
                    }
                    if (Input.GetMouseButtonDown(1))
                    {
                        Ray mouseRay = GenerateMouseRay();
                        //Vector3 newposition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                        //newposition.y = m_floor.transform.position.y;
                        //m_activeObject = GameObject.Instantiate(m_selectedRoomObject, newposition, Quaternion.identity);
                        if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out m_hit))
                        {
                            GameObject hitObj = m_hit.transform.gameObject;
                            Plane objPlane = new Plane(Vector3.up, hitObj.transform.position);
                            //Calculate offset
                            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                            float rayDistance;
                            objPlane.Raycast(mRay, out rayDistance);
                            m_activeObject = GameObject.Instantiate(m_selectedRoomObject,
                                (mRay.GetPoint(rayDistance) - hitObj.transform.position), Quaternion.identity);
                            m_rightMouseheld = true;
                        }
                        else
                        {
                            Debug.Log("Not hitting anything");
                        }
                    }
                    if (Input.GetMouseButtonUp(1))
                    {
                        m_rightMouseheld = false;
                    }
                    if (m_rightMouseheld)
                    {
                        GameObject hitObj = m_hit.transform.gameObject;
                        Plane objPlane = new Plane(Vector3.up, hitObj.transform.position);
                        //Calculate offset
                        Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                        float rayDistance;
                        objPlane.Raycast(mRay, out rayDistance);
                        m_activeObject.transform.position = mRay.GetPoint(rayDistance) - hitObj.transform.position;
                    }
                    rotationYAxis += velocityX;
                    rotationXAxis -= velocityY;
                    rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
                    Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                    Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
                    Quaternion rotation = toRotation;

                    distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
                    RaycastHit hit;
                    if (Physics.Linecast(target.position, transform.position, out hit))
                    {
                        distance -= hit.distance;
                    }
                    Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                    Vector3 position = rotation * negDistance + target.position;

                    Camera.main.transform.rotation = rotation;
                    Camera.main.transform.position = position;
                    velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
                    velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
                }
                break;
            case GameManager.State.VIEW:
                if (m_manager.GetPreviousState() == GameManager.State.BUILD)
                {
                    Destroy(m_startObj);
                }
                if (m_manager.GetPreviousState() != GameManager.State.VIEW)
                {
                    target = m_room.transform;
                    Vector3 newpos = Vector3.zero;
                    newpos.y = -1.4f;
                    m_floor.transform.position = newpos;
                }
                //What i want:
                //When a user clicks, holds and drags, they should be able to rotate the room model.
                if (target)
                {

                    if (Input.GetMouseButton(0))
                    {
                        velocityX += (xSpeed * Input.GetAxis("Mouse X") * distance * 0.02f) * Time.deltaTime;
                        velocityY += (ySpeed * Input.GetAxis("Mouse Y") * 0.02f) * Time.deltaTime;
                    }
                    rotationYAxis += velocityX;
                    rotationXAxis -= velocityY;
                    rotationXAxis = ClampAngle(rotationXAxis, yMinLimit, yMaxLimit);
                    Quaternion fromRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
                    Quaternion toRotation = Quaternion.Euler(rotationXAxis, rotationYAxis, 0);
                    Quaternion rotation = toRotation;

                    distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
                    RaycastHit hit;
                    if (Physics.Linecast(target.position, transform.position, out hit))
                    {
                        distance -= hit.distance;
                    }
                    Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                    Vector3 position = rotation * negDistance + target.position;

                    Camera.main.transform.rotation = rotation;
                    Camera.main.transform.position = position;
                    velocityX = Mathf.Lerp(velocityX, 0, Time.deltaTime * smoothTime);
                    velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
                }
                break;

        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// Gets the world position of all the lowest vertices of the walls to generate the floor;
    /// </summary>
    /// <returns></returns>
    public Vector3[] GetRoomFloorVerts()
    {
        List<Vector3> Verts = new List<Vector3>();

        //Go through each wall and find the lowest vertices on the y axis
        foreach(Transform child in m_room.transform)
        {

            Mesh m_mesh = child.GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = m_mesh.vertices;

            //find the lowest y value
            float lowestY = float.MaxValue;
            foreach(Vector3 v in vertices)
            {
                if (v.y < lowestY)
                {
                    lowestY = v.y;
                }
            }
            //Then loop again to find all the values that match that and add them to the vert list
            foreach (Vector3 v in vertices)
            {
                if (v.y == lowestY)
                {
                    Verts.Add(child.TransformPoint(v));
                }
            }
        }

        //check for duplicate points
        for (int i = 0; i < Verts.Count; i++)
        {
            for (int j = 0; j < Verts.Count; j++)
            {
                if (i != j)
                {
                    if (Verts[i] == Verts[j])
                    {
                        Verts.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
        return Verts.ToArray();
    }

    Ray GenerateMouseRay()
    {
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
                                            Input.mousePosition.y,
                                            Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
                                            Input.mousePosition.y,
                                            Camera.main.nearClipPlane);
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        Ray mr = new Ray(mousePosN, mousePosF - mousePosN);
        return mr;
    }
}
