using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum Dir
{
    Forward, Left, Right, Backward, Up
}

public class PlayerInput : MonoBehaviour
{
    public Dictionary<KeyCode, Dir> KeyToDir = new Dictionary<KeyCode, Dir>
    {
        { KeyCode.W, Dir.Forward }, { KeyCode.S, Dir.Backward },
        { KeyCode.A, Dir.Left }, { KeyCode.D, Dir.Right },
        { KeyCode.Space, Dir.Up }
    };

    public List<KeyCode> InputKeys = new List<KeyCode> {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
        KeyCode.Space
    };

    public Action<Dir> OnMove;
    public Action OnStopRotate;

    private void Update()
    {
        foreach(KeyCode k in InputKeys)
        {
            if(Input.GetKey(k))
            {
                OnMove?.Invoke(KeyToDir[k]);
            }
        }

        if (Input.GetKeyUp(InputKeys[4])) OnStopRotate?.Invoke();
    }
}
