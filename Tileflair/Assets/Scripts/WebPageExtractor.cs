using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebPageExtractor : MonoBehaviour
{
    //public string URL = "https://www.tileflair.co.uk/products/200x100-shades-tsp31-pvt-bevelled-cream";
    //public string m_name;

    [SerializeField] string[] m_tilenames;
    [SerializeField] string[] m_tileURLs;
    [SerializeField] Texture2D[] m_tileTextures;
    [SerializeField]List<TileData> m_tiles = new List<TileData>();
    bool completed = true;

    void Start()
    {
        if (m_tileURLs.Length == m_tilenames.Length && m_tileURLs.Length == m_tileTextures.Length)
        {
            completed = false;
            for (int i = 0; i < m_tileURLs.Length; i++)
            {
                StartCoroutine(GetTile(i));
            }
        }
        else
        {
            Debug.Log("Arrays of names, URLS and Textures are not correct");
        }
    }

    private void Update()
    {
        if (m_tiles.Count == m_tileURLs.Length && !completed)
        {
            GetComponent<WallPainter>().SetTileList(m_tiles);
            completed = true;
            Debug.Log("Tiles added to tile list");
        }
    }

    IEnumerator GetTile(int index)
    {
        int m_width = 0;
        int m_height = 0;
        float m_pricePerTile = 0.0f;
        string m_finish = "";

       UnityWebRequest www = UnityWebRequest.Get(m_tileURLs[index]);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Find the length
            string input = www.downloadHandler.text;
            string search = "<td><strong>Length:</strong> ";
            int p = input.IndexOf(search);
            if (p >= 0)
            {
                // move forward to the value
                int start = p + search.Length;
                // now find the end by searching for the next closing tag starting at the start position, 
                // limiting the forward search to the max value length
                int end = input.IndexOf("</td>", start);
                if (end >= 0)
                {
                    // pull out the substring
                    string v = input.Substring(start, end - start);
                    // finally parse into a float
                    //float value = float.Parse(v);
                    Debug.Log("Width = " + v);
                    v = v.Substring(0, v.Length - 2);
                    m_width = int.Parse(v);
                }
                else
                {
                    Debug.Log("Bad html - closing tag not found");
                }
            }
            else
            {
                Debug.Log("donations span not found");
            }

            //find the width
            search = "<td><strong>Width:</strong> ";
            p = input.IndexOf(search);
            if (p >= 0)
            {
                // move forward to the value
                int start = p + search.Length;
                // now find the end by searching for the next closing tag starting at the start position, 
                // limiting the forward search to the max value length
                int end = input.IndexOf("</td>", start);
                if (end >= 0)
                {
                    // pull out the substring
                    string v = input.Substring(start, end - start);
                    // finally parse into a float
                    //float value = float.Parse(v);
                    Debug.Log("height = " + v);
                    v = v.Substring(0, v.Length - 2);
                    m_height = int.Parse(v);
                }
                else
                {
                    Debug.Log("Bad html - closing tag not found");
                }
            }
            else
            {
                Debug.Log("donations span not found");
            }

            //find price per tile
            search = ">                                                            £";
            p = input.IndexOf(search);
            if (p >= 0)
            {
                // move forward to the value
                int start = p + search.Length;
                // now find the end by searching for the next closing tag starting at the start position, 
                // limiting the forward search to the max value length
                int end = input.IndexOf("</label>", start);
                if (end >= 0)
                {
                    // pull out the substring
                    string v = input.Substring(start, end - start);
                    // finally parse into a float
                    //float value = float.Parse(v);
                    Debug.Log("Price = " + v);
                    v = v.Substring(0, v.Length - 9);
                    m_pricePerTile = float.Parse(v);
                }
                else
                {
                    Debug.Log("Bad html - closing tag not found");
                }
            }
            else
            {
                Debug.Log("donations span not found");
            }

            //find the finish
            search = "<strong>Finish:</strong> ";
            p = input.IndexOf(search);
            if (p >= 0)
            {
                // move forward to the value
                int start = p + search.Length;
                // now find the end by searching for the next closing tag starting at the start position, 
                // limiting the forward search to the max value length
                int end = input.IndexOf("</td>", start);
                if (end >= 0)
                {
                    // pull out the substring
                    string v = input.Substring(start, end - start);
                    // finally parse into a float
                    //float value = float.Parse(v);
                    Debug.Log("Finish = " + v);
                    m_finish = v;
                }
                else
                {
                    Debug.Log("Bad html - closing tag not found");
                }

            }
            else
            {
                Debug.Log("donations span not found");
            }
            TileData newTile = new TileData(m_tilenames[index], m_height, m_width, m_finish, m_pricePerTile, m_tileTextures[index]);
            m_tiles.Add(newTile);
        }
    }
}
