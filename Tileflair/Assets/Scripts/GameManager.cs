using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    Paintmanager m_paintManager;
    WallVisibilityManager m_wallVisibility;

    public enum State
    {
        BUILD,
        PAINT,
        PLACE,
        VIEW
    }

    State m_gameState = State.BUILD;
    State m_previousState;
    Text m_priceText;

    public float m_price;

    // Start is called before the first frame update
    void Start()
    {
        m_priceText = GameObject.Find("PriceText").GetComponent<Text>();
        m_paintManager = GameObject.Find("PaintManager").GetComponent<Paintmanager>();
        m_wallVisibility = GameObject.Find("WallVisibilityManager").GetComponent<WallVisibilityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        m_priceText.text = "Price: £" + m_price.ToString("F2");
    }

    public void SwitchStates(State _state)
    {
        if (m_gameState == State.PAINT)
        {
            m_paintManager.End();
        }
        if (m_gameState == State.VIEW)
        {
            m_wallVisibility.End();
        }
        m_previousState = m_gameState;
        m_gameState = _state;
    }

    public State GetState()
    {
        return m_gameState;
    }

    public State GetPreviousState()
    {
        return m_previousState;
    }
}
