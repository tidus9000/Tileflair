using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLoader : MonoBehaviour
{
    List<TileData> m_tiles = new List<TileData>();
    // Start is called before the first frame update
    void Start()
    {
        TextAsset m_tileData = Resources.Load<TextAsset>("TileList");

        string[] data = m_tileData.text.Split(new char[] { '\n' });

        for (int i = 1; i < data.Length - 1; i++)
        {
            string[] row = data[i].Split(new char[] { ','});
            TileData td = new TileData(row);
            m_tiles.Add(td);
        }

        GetComponent<WallPainter>().SetTileList(m_tiles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
