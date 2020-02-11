using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButtonPanel : MonoBehaviour
{
    List<TileData> m_tiles;
    [SerializeField] GameObject m_buttonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        m_tiles = GameObject.Find("PaintManager").GetComponent<WallPainter>().GetTileList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Spawn Buttons")]
    public void SpawnButtons()
    {
        if (m_tiles == null)
        {
            m_tiles = GameObject.Find("PaintManager").GetComponent<WallPainter>().GetTileList();
        }

        int xPos = -300;
        int xDifference = 200;
        for (int i = 0; i < m_tiles.Count; i++)
        {
            Vector3 newpos = new Vector3(xPos, -10, 0);
            GameObject newButton = GameObject.Instantiate(m_buttonPrefab, transform);
            newButton.transform.localPosition = newpos;
            m_buttonPrefab.GetComponent<TileButton>().setup(m_tiles[i], i);
            xPos += xDifference;
        }
    }
}
