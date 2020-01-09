using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisibilityManager : MonoBehaviour
{
    GameManager m_manager;
    [SerializeField]Material m_transparentMat;
    [SerializeField]Material m_defaultMat;
    [SerializeField]GameObject m_selected;

    // Start is called before the first frame update
    void Start()
    {
        m_manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_manager.GetState())
        {
            case GameManager.State.VIEW:

                if (m_selected)
                {
                    var parent = m_selected.transform.parent;

                    foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
                    {
                        r.material = m_defaultMat;
                    }
                    m_selected = null;
                }

                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Transform selectedObj = hit.transform;
                    if (selectedObj.CompareTag("Wall"))
                    {
                        var parent = selectedObj.transform.parent;
                        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
                        {
                            r.material = m_transparentMat;
                        }
                    }
                    m_selected = selectedObj.gameObject;
                }
                else
                {
                    Debug.Log("I'm looking at nothing");
                    
                }
            break;
        }
    }
}
