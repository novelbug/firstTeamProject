using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private Slider turningGaugeSlider;
    [SerializeField] private TextMeshPro warningText;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private LayerMask _whatIsGround;

    public GroundTile standingGround = null;

    private PlayerMove _playerMove;
    private PlayerInput _playerInput;

    TileData tileData;

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

        _playerInput.OnStopRotate += HandleOnStopTurning;
        _playerMove.OnMove += HandleOnMove;
        _playerMove.OnPlayerStopped += HandleOnPlayerStopped;

        tileData = new TileData();
    }

    private void Update()
    {
        // canTurn = (_characterController.isGrounded && !_playerMove.isMoving) ? true : false;

        if (_playerMove.isTurning)
        {
            turningGauge += ((turningGauge == 0) ? 0.1f : turningGauge) * Time.deltaTime * 1.5f;
        }
        else
        {
            turningGauge -= ((turningGauge == 0) ? 0.1f : turningGauge) * Time.deltaTime * 0.1f;
        }
        turningGauge = Mathf.Clamp01(turningGauge);
        turningGaugeSlider.value = turningGauge;

        if (turningGaugeSlider.value >= 0.99f)
        {
            _playerInput.OnStopRotate.Invoke();
        }



        if (Physics.Raycast(transform.position, Vector3.down * 3f, out hit))
        {
            if (hit.transform.parent.gameObject.TryGetComponent(out GroundTile ground))
            {
                standingGround = ground;
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * 3f);
    }

    private void HandleOnStopTurning()
    {
        turningGaugeSlider.value = turningGauge;
    }

    private void HandleOnMove(Vector3Int dir)
    {
        StopCoroutine("StoppedWarning");
        warningText.alpha = 0;

        //Vector3Int posInt = Vector3Int.FloorToInt(transform.position) - Vector3Int.down;
        //TileBase standingTile = groundTilemap.GetTile(posInt);
        //standingTile.GetTileData(posInt, groundTilemap, ref tileData);
        //standingGround = tileData.gameObject.GetComponent<GroundTile>();
    }

    private void HandleOnPlayerStopped()
    {
        warningText.alpha = 1;
        warningText.text = "3";

        StartCoroutine("StoppedWarning");
    }

    private IEnumerator StoppedWarning()
    {
        yield return new WaitForSeconds(1f);
        warningText.text = "2";
        yield return new WaitForSeconds(1f);
        warningText.text = "1";
        yield return new WaitForSeconds(1f);
        warningText.text = "0";
        DropBlockOnPlayer();
    }

    private void DropBlockOnPlayer()
    {
        Debug.LogWarning("Dropped!!!");

    }
}
