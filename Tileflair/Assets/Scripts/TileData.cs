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
    public Texture2D m_texture;

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
        TextureImporter x = new TextureImporter();
        x.textureType = TextureImporterType.NormalMap;
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
