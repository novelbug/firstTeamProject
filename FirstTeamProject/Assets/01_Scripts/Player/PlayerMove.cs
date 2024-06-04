using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    public bool isMoving = false;
    public float jumpForce = 3f;

    public Dictionary<Dir, Vector3Int> DirToVec = new Dictionary<Dir, Vector3Int> {
        { Dir.Forward, new Vector3Int(0, 0, 1) }, { Dir.Backward, new Vector3Int(0, 0, -1) },
        { Dir.Left, new Vector3Int(-1, 0, 0) }, { Dir.Right, new Vector3Int(1, 0, 0) },
        { Dir.Up, new Vector3Int(0, 1, 0) }
    };

    private PlayerInput _playerInput;
    private Rigidbody _rbCompo;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rbCompo = GetComponent<Rigidbody>();

        _playerInput.OnMove += Move;
    }

    private void Move(Dir dir)
    {
        if(isMoving == false && dir != Dir.Up)
        {
            isMoving = true;

            transform.DOMove(transform.position + DirToVec[dir], 0.25f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                isMoving = false;
            });
        }
        else if(dir == Dir.Up)
        {
            _rbCompo.AddForce(Vector3.up * jumpForce);
        }
    }
}
