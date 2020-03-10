using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    Camera m_mainCam;
    [SerializeField] Camera m_orthCam;
    [SerializeField] Camera m_persCam;
    [SerializeField] Camera m_paintCam;
    [SerializeField] Camera m_placeCam;
    GameObject m_gridmanager;
    GameManager m_gameManager;
    Paintmanager m_paintmanager;
    GameObject m_tp;

    // Start is called before the first frame update
    void Start()
    {
        m_mainCam = GetComponent<Camera>();
        m_gridmanager = GameObject.Find("GridManager");
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_paintmanager = GameObject.Find("PaintManager").GetComponent<Paintmanager>();
        m_tp = GameObject.Find("TilesPanel");
        m_tp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToPlaceMode()
    {
        m_mainCam.orthographic = false;
        m_mainCam.transform.position = m_persCam.transform.position;
        m_mainCam.transform.rotation = m_persCam.transform.rotation;
        m_gridmanager.SetActive(false);
        m_tp.SetActive(false);
        m_gameManager.SwitchStates(GameManager.State.PLACE);
    }

    public void SwitchToPerspective()
    {
        m_mainCam.orthographic = false;
        m_mainCam.transform.position = m_persCam.transform.position;
        m_mainCam.transform.rotation = m_persCam.transform.rotation;
        m_gridmanager.SetActive(false);
        m_tp.SetActive(false);
        m_gameManager.SwitchStates(GameManager.State.VIEW);
    }

    public void SwitchToOrthographic()
    {
        m_mainCam.orthographic = true;
        m_mainCam.transform.position = m_orthCam.transform.position;
        m_mainCam.transform.rotation = m_orthCam.transform.rotation;
        m_gridmanager.SetActive(true);
        m_tp.SetActive(false);
        m_gameManager.SwitchStates(GameManager.State.BUILD);
    }

    public void SwitchToPaintMode()
    {
        m_mainCam.orthographic = true;
        //Switch to wall Cam position
        m_mainCam.transform.position = m_paintCam.transform.position;
        m_mainCam.transform.rotation = m_paintCam.transform.rotation;
        m_paintmanager.Initialise();
        m_gridmanager.SetActive(false);
        m_tp.SetActive(true);
        m_tp.GetComponent<TileButtonPanel>().SpawnButtons();
        m_gameManager.SwitchStates(GameManager.State.PAINT);
    }
}
