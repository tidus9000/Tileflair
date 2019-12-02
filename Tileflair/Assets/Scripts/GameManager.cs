using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        BUILD,
        PAINT,
        PLACE,
        VIEW
    }

    State m_gameState = State.BUILD;
    State m_previousState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SwitchStates(State _state)
    {
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
