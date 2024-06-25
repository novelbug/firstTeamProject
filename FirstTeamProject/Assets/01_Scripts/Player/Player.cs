using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Slider turningGaugeSlider;

    public GroundTile standingGround = null;

    private PlayerMove _playerMove;
    private PlayerInput _playerInput;
    // private CharacterController _characterController;

    private RaycastHit hit;
    private float _maxDist = 5f;

    /// <summary>
    /// 돌면서 올라가는 게이지
    /// </summary>
    public float turningGauge = 0f;
    /// <summary>
    /// 게이지가 다 차거나 이동/떨어지는 상태면 false, 아니면 true
    /// </summary>
    public bool canTurn = true;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerInput = GetComponent<PlayerInput>();
        //  _characterController = GetComponent<CharacterController>();

        _playerInput.OnStopRotate += HandleOnStopTurning;
        _playerMove.OnMove += HandleOnMove;
    }

    private void Update()
    {
        // canTurn = (_characterController.isGrounded && !_playerMove.isMoving) ? true : false;

        if (_playerMove.isTurning)
        {
            turningGauge += ((turningGauge == 0) ? 0.1f : turningGauge) * Time.deltaTime * 1.5f;
            turningGaugeSlider.value = Mathf.Clamp(turningGauge, 0, 10f);

            if(turningGaugeSlider.value >= 9.99f)
            {
                _playerInput.OnStopRotate.Invoke();
            }
        }
        else turningGauge = 0f;

        if(Physics.Raycast(transform.position, Vector3.down, out hit, _maxDist))
        {
            if(hit.collider.gameObject.TryGetComponent(out GroundTile ground))
            {
                standingGround = ground;
            }
        }
    }

    private void HandleOnStopTurning()
    {
        turningGaugeSlider.value = turningGauge;
    }

    private void HandleOnMove(Vector3Int dir)
    {

    }
}
