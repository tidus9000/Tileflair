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
    GameObject m_wall;
    Vector3 m_posDown;
    Vector3 m_posUp;
    GridManager m_grid;
    bool m_mousedown = false;

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
                if (m_mousedown)
                {
                    //rotate the initial object to where the mouse is pointing
                    m_startObj.transform.LookAt(m_grid.FindClosestPoint(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10))));
                }
                if (Input.GetMouseButtonDown(0))
                {
                    //Spawn an initial wall object on the grid where the user has clicked
                    m_posDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    m_posDown = m_grid.FindClosestPoint(m_posDown);
                    m_wall = new GameObject("Wall");
                    m_wall.transform.parent = m_room.transform;
                    m_startObj = GameObject.Instantiate(m_point, m_posDown, Quaternion.identity, m_wall.transform);
                    m_mousedown = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    //Calculate how many wall objects need to be made and Spawn them up to the point where the user has let go
                    m_posUp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    m_posUp = m_grid.FindClosestPoint(m_posUp);


                    float distance = Vector3.Distance(m_posDown, m_posUp);
                    int noObjects = (int)(distance / m_point.transform.localScale.x);
                    Vector3 divisionsVec = m_posUp - m_posDown;
                    divisionsVec /= noObjects;

                    for (int i = 0; i < noObjects; i++)
                    {
                        m_posDown += divisionsVec;
                        GameObject.Instantiate(m_point, m_posDown, m_startObj.transform.rotation, m_wall.transform);
                    }
                    m_mousedown = false;
                }
                break;
            case GameManager.State.PAINT:
                if (m_manager.GetPreviousState() == GameManager.State.BUILD)
                {
                    Destroy(m_wall);
                }
                break;
            case GameManager.State.PLACE:
                if (m_manager.GetPreviousState() == GameManager.State.BUILD)
                {
                    Destroy(m_wall);
                }
                break;
            case GameManager.State.VIEW:
                if (m_manager.GetPreviousState() == GameManager.State.BUILD)
                {
                    Destroy(m_wall);
                    target = m_room.transform;
                }
                //What i want
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
}
