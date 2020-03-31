using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_DeleteWhenDone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        //get the midpoint of all the walls
        //find the angle from this point to all the walls?
        //if within a certain range, rotate 180

        Gizmos.DrawLine(transform.position, transform.forward);
    }

    /// <summary>
    /// Rotates the wall so that it is always facing inwards
    /// </summary>
    [ContextMenu("Rotate correctly")]
    public void RotateCorrect()
    {
        int i = 0;
        Vector3 position = Vector3.zero;
        GameObject inner = new GameObject();
        GameObject outer = new GameObject();
        bool innerFound = false;
        bool outerFound = false;

        //find the inner and outer objects in the prefab
        foreach (Transform t in transform)
        {
            if (t.name == "Inner")
            {
                inner = t.gameObject;
                innerFound = true;
            }
            if (t.name == "Outer")
            {
                outer = t.gameObject;
                outerFound = true;
            }
        }

        //get the average position of all walls
        Transform[] transforms = transform.parent.GetComponentsInChildren<Transform>();
        foreach (Transform t in transforms)
        {
            position += t.position;
            i++;
        }
        position /= i;

        //if we have confirmed that we are a proper wall prefab, 
        //find whether the outer or inner object is closer to the average position
        if (innerFound && outerFound)
        {
            float innerDist = Vector3.Distance(inner.transform.position, position);
            float outerDist = Vector3.Distance(outer.transform.position, position);

            //turn the wall by 180 if we are facing the wrong direction
            if (innerDist > outerDist)
            {
                Vector3 newrot = transform.rotation.eulerAngles;
                newrot.y += 180;
                transform.rotation = Quaternion.Euler(newrot);
            }
        }
        else
        {
            Debug.Log("Inner and outer objects not found for wall");
        }
    }
}
