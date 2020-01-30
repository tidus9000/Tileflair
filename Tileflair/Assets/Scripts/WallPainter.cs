using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPainter : MonoBehaviour
{
    [SerializeField] Texture2D m_activeTile;
    [SerializeField] GameObject m_tileObj;
    Vector3 m_posDown;
    Vector3 m_posUp;
    GameObject m_testTile;
    GameManager m_gameManager;
    Paintmanager m_paintmanager;
    bool m_mousedown = false;

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        m_paintmanager = GameObject.Find("PaintManager").GetComponent<Paintmanager>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_gameManager.GetState())
        {
            case GameManager.State.BUILD:
                break;
            case GameManager.State.PAINT:
                if (Input.GetMouseButtonDown(0))
                {
                    //Spawn an initial tile object
                    m_posDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));
                    m_testTile = GameObject.Instantiate(m_tileObj, m_posDown, Quaternion.identity);
                    //Change the texture to our active tile
                    m_testTile.GetComponentInChildren<Renderer>().material.mainTexture = m_activeTile;
                    //Scale it down to the correct measurements
                    //here we will read from a json file that
                    m_testTile.GetComponentInChildren<Transform>().localScale = (new Vector3(0.02f, 0.01f, 1.0f));
                    m_mousedown = true;
                }
                if (m_mousedown)
                {
                    //Essentially want to make a grid of points based on the measurements of the tile
                    //m_posDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    m_testTile.transform.position = m_posDown;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Destroy(m_testTile.gameObject);
                    m_posUp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    GameObject tiles = new GameObject();
                    Instantiate(tiles, Vector3.zero, Quaternion.identity);
                    tiles.name = "Tiles";
                    tiles.transform.parent = m_paintmanager.GetActiveWall().transform;
                    //for some reason the increment has to be multiplied by 10. I don't know why but hey it works
                    for (float x = m_posDown.x; x < m_posUp.x; x += 0.2f )
                    {
                        for (float y = m_posDown.y; y < m_posUp.y; y += 0.1f)
                        {
                            m_testTile = GameObject.Instantiate(m_tileObj, new Vector3(x, y, 0), Quaternion.identity);
                            //Change the texture to our active tile
                            m_testTile.GetComponentInChildren<Renderer>().material.mainTexture = m_activeTile;
                            //Scale it down to the correct measurements
                            m_testTile.GetComponentInChildren<Transform>().localScale = (new Vector3(0.02f, 0.01f, 1.0f));
                            m_testTile.transform.parent = tiles.transform;
                        }
                    }
                    Vector3 newpos = tiles.transform.localPosition;
                    newpos.x = -0.551f;
                    tiles.transform.localPosition = newpos;
                    m_mousedown = false;
                }
                break;
            case GameManager.State.PLACE:
                break;
            case GameManager.State.VIEW:
                break;
        }
    }
}
