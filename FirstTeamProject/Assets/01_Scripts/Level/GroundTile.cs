using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GroundTile : MonoBehaviour
{
    [SerializeField] private Material colorMat;

    private Player _player;
    private Material _currentMat;
    private MeshRenderer _meshCompo;

    public Vector3 position;
    public GameObject tileObject;

    public int distWithPlayer = 0;
    public bool isPlayerTurning = false;
    public float explodeCooldown = 0, explodeCooltime = 12f;
    public bool isExploding = false;

    public GroundTile(Vector3 pos, GameObject tileObj)
    {
        this.position = pos;
        this.tileObject = tileObj;
    }

    private void Awake()
    {
        _meshCompo = transform.GetChild(1).GetComponent<MeshRenderer>();
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

    public void StartExplode()
    {
        if(!isExploding) StartCoroutine("ColorChange");
    }

    public void StopExplode()
    {
        if (isExploding) StopCoroutine("ColorChange");
        _meshCompo.material.DOKill();
        _meshCompo.material.color = Color.white;
        isExploding = false;
    }

    private IEnumerator ColorChange()
    {
        isExploding = true;

        yield return null;
        _meshCompo.material.DOColor(Color.red, 4f).OnComplete(() => { Explode(); });
    }

    public void Explode()
    {
        Debug.LogWarning("############### Explode ###############");
        transform.DOMoveY(transform.position.y - 5f, 0.5f).SetEase(Ease.InQuad).OnComplete(() =>
        {
            
            Destroy(this.gameObject);
        });
    }
}
