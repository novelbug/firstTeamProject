using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private GameObject _playerModeling;

    private Player _player;

    public bool isMoving = false;
    public bool isTurning = false;
    public bool canMove = true;
    public float jumpForce = 3f;
    public float turnSpeed = 3f;
    public float turningTime = 0f;
    public int playerMaxZPos = 0;

    [SerializeField] private float _moveCooltime = 2f;

    public Dictionary<Dir, Vector3Int> DirToVec = new Dictionary<Dir, Vector3Int> {
        { Dir.Forward, new Vector3Int(0, 0, 1) }, { Dir.Backward, new Vector3Int(0, 0, -1) },
        { Dir.Left, new Vector3Int(-1, 0, 0) }, { Dir.Right, new Vector3Int(1, 0, 0) },
        { Dir.Up, new Vector3Int(0, 1, 0) }
    };

    public Action<Vector3Int> OnMove;
    public Action OnPlayerStopped;

    private PlayerInput _playerInput;
    private Rigidbody _rigidbody;

    private bool _isPlayerDoInput = false;

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
        canMove = !GameManager.Instance.isGameStopped && !GameManager.Instance.isGameOver;

        if (Input.GetKeyDown(_playerInput.InputKeys[4]) && isTurning == false)
        {
            _rigidbody.useGravity = false;
            transform.DOMoveY(transform.position.y + 1f, 0.75f).SetEase(Ease.OutExpo);
        }

        if (playerMaxZPos < (int)transform.position.z)
            playerMaxZPos = (int)transform.position.z;
        GameManager.Instance.gameScore = playerMaxZPos;

        CheckPlayerStopped();
    }

    private void Move(Dir dir)
    {
        if(canMove)
        {
            if (isTurning == false)
            {
                if (isMoving == false && dir != Dir.Up)
                {
                    StartCoroutine("MoveCoroutine", dir);
                }
                else if (dir == Dir.Up)
                {
                    turningTime = 0;
                    isTurning = true;
                }
            }
            else
            {
                Turn(turnSpeed);
            }
        }
    }

    private void CheckPlayerStopped()
    {
        _isPlayerDoInput = false;
        foreach (KeyCode k in _playerInput.InputKeys)
        {
            if (Input.GetKey(k)) _isPlayerDoInput = true;
        }
        _isPlayerDoInput = (isTurning || isMoving) ? true : false;

        if (_isPlayerDoInput == false)
            OnPlayerStopped?.Invoke();
    }

    private void Turn(float speed)
    {
        if(isTurning)
        {
            turningTime += Time.deltaTime;
            _playerModeling.transform.Rotate(new Vector3(0, 0, speed * turningTime * turningTime * 0.75f + 3f));
        }
    }

    private void StopTurn()
    {
        if(isTurning)
        {
            _rigidbody.useGravity = true;
            isTurning = false;
            _playerModeling.transform.eulerAngles = Vector3.zero;
        }
    }

    private IEnumerator MoveCoroutine(Dir dir)
    {
        yield return null;

        if(_player.standingGround != null) _player.standingGround.StopExplode();
        int dirOnRange = (Mathf.Abs(transform.position.x) >= 4.5f) ? ((transform.position.x > 0) ? 1 : -1) : 0;
        if ((dirOnRange == 0 || (dirOnRange > 0 && dir != Dir.Right) || (dirOnRange < 0 && dir != Dir.Left)) && !isMoving)
        {
            isMoving = true;
            OnMove?.Invoke(DirToVec[dir]);

            if (dir == Dir.Forward) GameManager.Instance.gameScore++;
            else if (dir == Dir.Backward) GameManager.Instance.gameScore--;

            transform.DOComplete();
            transform.DOMove(transform.position + DirToVec[dir], 0.25f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                isMoving = false;
            });
        }
    }
}
