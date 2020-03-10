using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TileButton : MonoBehaviour
{
    [SerializeField]int m_index = 0;
    [SerializeField] string m_name;
    WallPainter m_wallPainter;

    // Start is called before the first frame update
    void Start()
    {
        m_wallPainter = GameObject.Find("PaintManager").GetComponent<WallPainter>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setup(TileData _tile, int _index)
    {
        m_index = _index;
        //Image im = GetComponent<Image>();
        //Sprite newSprite = Sprite.Create(_tile.m_texture, Rect.zero, Vector2.zero);
        //im.sprite = newSprite;
        GetComponentInChildren<Text>().text = _tile.m_name;
        m_name = _tile.m_name;
    }

    public void setActive()
    {
        m_wallPainter.setActiveTile(m_index);
    }
}
