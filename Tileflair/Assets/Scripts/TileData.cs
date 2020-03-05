using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class TileData : MonoBehaviour
{
    public string m_name;
    public float m_height;
    public float m_width;
    public string m_finish;
    public float m_pricePerTile;
    public Texture2D m_texture;

    public TileData(string _name, float _height, float _width, string _finish, float _pricePerTile, Texture2D _texture)
    {
        m_name = _name;
        m_height = _height;
        m_width = _width;
        m_finish = _finish;
        m_pricePerTile = _pricePerTile;
        m_texture = _texture;
    }

    public TileData(string[] data)
    {
        if (data.Length == 4)
        {
            m_name = data[0];
            m_height = float.Parse(data[1]);
            m_width = float.Parse(data[2]);
            m_finish = data[3];

            m_texture = new Texture2D(2,2);
            byte[] fileData = File.ReadAllBytes("Assets\\Materials\\Textures\\Tiles\\" + m_name + ".jpg");
            m_texture.LoadImage(fileData);
        }
        else
        {
            Debug.Log("Invalid data used to create tile");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(string[] data)
    {
        if (data.Length == 4)
        {
            m_name = data[0];
            m_height = int.Parse(data[1]);
            m_width = int.Parse(data[2]);
            m_finish = data[3];
        }
        else
        {
            Debug.Log("Invalid data used to create tile");
        }
    }
}
