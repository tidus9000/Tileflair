﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPainter : MonoBehaviour
{
    [SerializeField] GameObject m_tileObj;
    Vector3 m_posDown;
    Vector3 m_posUp;
    GameObject m_testTile;
    GameManager m_gameManager;
    Paintmanager m_paintmanager;
    bool m_mousedown = false;

    List<TileData> m_tiles;
    TileData m_activeTile;
    int m_activeTileIndex = 0;
    [SerializeField] GameObject m_tileButton;

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
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 100f))
                    {
                        //Spawn an initial tile object
                        m_posDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));
                        m_testTile = GameObject.Instantiate(m_tileObj, m_posDown, Quaternion.identity);
                        //Change the texture to our active tile
                        m_testTile.GetComponentInChildren<Renderer>().material.mainTexture = m_activeTile.m_texture;
                        //Scale it down to the correct measurements
                        //here we will read from a json file that
                        m_testTile.GetComponentInChildren<Transform>().localScale =
                            (new Vector3(m_activeTile.m_width / 10000, m_activeTile.m_height / 10000, 1.0f));
                        m_mousedown = true;
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("Right Click");
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 100f))
                    {
                        //Spawn an initial tile object
                        m_posDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4));
                        m_testTile = GameObject.Instantiate(m_tileObj, m_posDown, Quaternion.identity);
                        
                        //Filter the texture of the active tile
                        Texture2D filteredTex = NormalMapTools.FilterMedian(m_activeTile.m_texture, 5);
                        //Create the normal map
                        Texture normalTex = NormalMapTools.CreateNormalmap(filteredTex, 6);

                        Renderer r = m_testTile.GetComponentInChildren<Renderer>();
                        //Change the texture to our active tile
                        r.material.mainTexture = m_activeTile.m_texture;
                        //Set the normal map
                        r.material.SetTexture("_BumpMap", normalTex);
                        //create and set the specular map
                        Texture2D specularTex = NormalMapTools.CreateSpecular(filteredTex, 1.2f, 0.5f); // using filtered texture
                        r.material.SetTexture("_SpecMap", specularTex);

                        //Scale it down to the correct measurements
                        //here we will read from a json file that
                        m_testTile.GetComponentInChildren<Transform>().localScale =
                            (new Vector3(m_activeTile.m_width / 10000, m_activeTile.m_height / 10000, 1.0f));
                        m_mousedown = true;
                    }
                }
                if (m_mousedown)
                {
                    //Essentially want to make a grid of points based on the measurements of the tile
                    //m_posDown = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    m_testTile.transform.position = m_posDown;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (m_testTile && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 100f))
                    {
                        Destroy(m_testTile.gameObject);
                        m_posUp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                        GameObject tiles = new GameObject();
                        Instantiate(tiles, Vector3.zero, Quaternion.identity);
                        tiles.name = "Tiles";
                        tiles.transform.parent = m_paintmanager.GetActiveWall().transform;
                        //for some reason the increment has to be multiplied by 10. I don't know why but hey it works
                        for (float x = m_posDown.x; x < m_posUp.x; x += m_activeTile.m_width / 1000)
                        {
                            for (float y = m_posDown.y; y < m_posUp.y; y += m_activeTile.m_height / 1000)
                            {
                                m_testTile = GameObject.Instantiate(m_tileObj, new Vector3(x, y, 0), Quaternion.identity);
                                //Change the texture to our active tile
                                m_testTile.GetComponentInChildren<Renderer>().material.mainTexture = m_activeTile.m_texture;
                                //Scale it down to the correct measurements
                                m_testTile.GetComponentInChildren<Transform>().localScale =
                                    (new Vector3(m_activeTile.m_width / 10000, m_activeTile.m_height / 10000, 1.0f));
                                m_testTile.transform.parent = tiles.transform;
                            }
                        }
                        Vector3 newpos = tiles.transform.localPosition;
                        newpos.x = -0.551f;
                        tiles.transform.localPosition = newpos;
                        m_mousedown = false;
                    }
                }

                if (Input.GetMouseButtonUp(1))
                {
                    Debug.Log("Right Click Up");
                    if (m_testTile && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 100f))
                    {
                        GameObject temptile = m_testTile.gameObject;
                        Destroy(m_testTile.gameObject);
                        m_posUp = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                        GameObject tiles = new GameObject();
                        Instantiate(tiles, Vector3.zero, Quaternion.identity);
                        tiles.name = "Tiles";
                        tiles.transform.parent = m_paintmanager.GetActiveWall().transform;
                        //for some reason the increment has to be multiplied by 10. I don't know why but hey it works
                        for (float x = m_posDown.x; x < m_posUp.x; x += m_activeTile.m_width / 1000)
                        {
                            for (float y = m_posDown.y; y < m_posUp.y; y += m_activeTile.m_height / 1000)
                            {
                                m_testTile = GameObject.Instantiate(temptile, new Vector3(x, y, 0), Quaternion.identity);
                                //shouldn't have to change the material since its the same object we generated before
                                //Scale it down to the correct measurements
                                m_testTile.GetComponentInChildren<Transform>().localScale =
                                    (new Vector3(m_activeTile.m_width / 10000, m_activeTile.m_height / 10000, 1.0f));
                                m_testTile.transform.parent = tiles.transform;
                            }
                        }
                        Vector3 newpos = tiles.transform.localPosition;
                        newpos.x = -0.551f;
                        tiles.transform.localPosition = newpos;
                        m_mousedown = false;
                    }
                }
                break;
            case GameManager.State.PLACE:
                break;
            case GameManager.State.VIEW:
                break;
        }
    }

    public void SetTileList(List<TileData> _tiles)
    {
        m_tiles = _tiles;
        if (m_tiles.Count > 0)
        {
            m_activeTile = m_tiles[0];
        }
    }

    public List<TileData> GetTileList()
    {
        return m_tiles;
    }

    public void setActiveTile(int _index)
    {
        m_activeTileIndex = _index;
        m_activeTile = m_tiles[m_activeTileIndex];
    }
}
