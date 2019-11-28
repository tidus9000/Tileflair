using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputBuild : MonoBehaviour
{
    [SerializeField] GameObject m_point;
    GameObject m_startObj;
    Vector3 m_posDown;
    Vector3 m_posUp;
    bool m_mousedown = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_mousedown)
        {
            m_startObj.transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)));
        }
        if (Input.GetMouseButtonDown(0))
        {
            m_posDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            m_startObj = GameObject.Instantiate(m_point, m_posDown, Quaternion.identity);
            m_mousedown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_posUp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

            float distance = Vector3.Distance(m_posDown, m_posUp);
            int noObjects =  (int)(distance / m_point.transform.localScale.x);
            Vector3 divisionsVec = m_posUp - m_posDown;
            divisionsVec /= noObjects;

            for (int i = 0; i < noObjects; i++)
            {
                m_posDown += divisionsVec;
                GameObject.Instantiate(m_point, m_posDown, m_startObj.transform.rotation);
            }
            m_mousedown = false;
        }
    }
}
