using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ParsecGaming;
using ParsecUnity;

public class InputManager
{
    private bool w = false;
    public bool GetWDown(int id)
    {
        if (ParsecInput.GetKey(id, KeyCode.W))
        {
            if (!w)
            {
                w = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
