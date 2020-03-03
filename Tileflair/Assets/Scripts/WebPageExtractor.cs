using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebPageExtractor : MonoBehaviour
{
    public string URL = "https://www.tileflair.co.uk/products/200x100-shades-tsp31-pvt-bevelled-cream";

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {

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
                    Debug.Log("Length = " + v);
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
                    Debug.Log("Width = " + v);
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
        }
    }
}
