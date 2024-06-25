using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    private Player _player;

    public bool isMoving = false;
    public bool isTurning = false;
    public float jumpForce = 3f;
    public float turnSpeed = 3f;
    public float turningTime = 0f;

    [SerializeField] private float _moveCooldown = 0, _moveCooltime = 0f;

    public Dictionary<Dir, Vector3Int> DirToVec = new Dictionary<Dir, Vector3Int> {
        { Dir.Forward, new Vector3Int(0, 0, 1) }, { Dir.Backward, new Vector3Int(0, 0, -1) },
        { Dir.Left, new Vector3Int(-1, 0, 0) }, { Dir.Right, new Vector3Int(1, 0, 0) },
        { Dir.Up, new Vector3Int(0, 1, 0) }
    };

    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _player = GetComponent<Player>();

        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();

        _playerInput.OnMove += Move;
        _playerInput.OnStopRotate += StopTurn;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_playerInput.InputKeys[4]) && isTurning == false)
        {
            _rigidbody.useGravity = false;
            transform.DOMoveY(transform.position.y + 1f, 0.75f).SetEase(Ease.OutExpo);
        }
    }

    private void Move(Dir dir)
    {
        if(isMoving == false && dir != Dir.Up && isTurning == false)
        {
            isMoving = true;

            transform.DOMove(transform.position + DirToVec[dir], 0.25f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                isMoving = false;
            });
        }
        else if(dir == Dir.Up && isTurning == false)
        {
            turningTime = 0;
            isTurning = true;
        }

        Turn(turnSpeed);
    }

    private void Turn(float speed)
    {
        if(isTurning)
        {
            turningTime += Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, speed * turningTime * turningTime * 0.75f + 3f));
        }
    }

    private void StopTurn()
    {
        if(isTurning)
        {
            _rigidbody.useGravity = true;
            isTurning = false;
            transform.eulerAngles = Vector3.zero;
        }
    }
}
