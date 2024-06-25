using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    [SerializeField] private Material colorMat;
    [SerializeField] private float _changingTime;

    private Player _player;
    private Material _currentMat;
    private MeshRenderer _meshCompo;

    public Vector3 position;
    public GameObject tileObject;

    public int distWithPlayer = 0;
    public bool isPlayerTurning = false;
    public float explodeCooldown = 0, explodeCooltime = 12f;

    public GroundTile(Vector3 pos, GameObject tileObj)
    {
        this.position = pos;
        this.tileObject = tileObj;
    }

    private void Awake()
    {
        _meshCompo = GetComponent<MeshRenderer>();
        _currentMat = _meshCompo.material;
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if(isPlayerTurning)
        {
            explodeCooldown += Time.deltaTime;
            Debug.Log($"MING : {explodeCooldown / explodeCooltime}");
            _meshCompo.material = colorMat;
        }
        else
        {
            _meshCompo.material = _currentMat;
        }

        distWithPlayer = (int)gameObject.transform.position.z - (int)_player.gameObject.transform.position.z;
        if (distWithPlayer < -10) gameObject.transform.position += new Vector3Int(0, 0, 60);
    }

    public void Explode()
    {
        _changingTime = 0;
        StartCoroutine(ColorChange());
    }

    private IEnumerator ColorChange()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        _changingTime += 0;
    }
}
