using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemies;
    [SerializeField] private GameObject[] WarningZones;
    [SerializeField] private Transform[] PrefabTransforms;

    public static ObstacleCreator Instance;

    public Player _player;

    private Vector3Int _playerDir;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _player = FindObjectOfType<Player>();
        _player.playerMove.OnMove += OnMoveHandle;
    }

    private void OnMoveHandle(Vector3Int dir)
    {
        _playerDir = new Vector3Int(0, 0, dir.z);

        bool isTrue30 = Random.Range(1, 101) <= 30;
        if(isTrue30) StartCoroutine("SpawnSparrow");

        bool isTrue20 = Random.Range(1, 101) <= 20;
        if (isTrue20) StartCoroutine("SpawnMice");

        bool isTrue10 = Random.Range(1, 101) <= 10;
        if (isTrue10) StartCoroutine("SpawnAC");
    }

    private IEnumerator SpawnSparrow()
    {
        Debug.LogWarning("Spawning Sparrows!!!");

        Vector3 warningZoneSpawnPos = new Vector3(0, -0.25f, _player.transform.position.z) + _playerDir;
        var zone = Instantiate(WarningZones[0], warningZoneSpawnPos, Quaternion.Euler(90, 0, 0), PrefabTransforms[0]);

        yield return new WaitForSeconds(1f);

        Destroy(zone);
        bool isLeft = Random.Range(1, 3) < 2;
        Vector3 enemySpawnPos = new Vector3(20 * (isLeft ? -1 : 1), 0, warningZoneSpawnPos.z);
        Instantiate(Enemies[0], enemySpawnPos, Quaternion.Euler(0, (isLeft ? 90 : -90), 0), PrefabTransforms[1]);
    }

    private IEnumerator SpawnAC()
    {
        Debug.LogWarning("Spawning AC!!!");

        Vector3 warningZoneSpawnPos = new Vector3(_player.transform.position.x, -0.25f, _player.transform.position.z);
        var zone = Instantiate(WarningZones[2], warningZoneSpawnPos, Quaternion.Euler(90, 0, 0), PrefabTransforms[0]);

        yield return new WaitForSeconds(1.5f);

        Vector3 enemySpawnPos = new Vector3(zone.transform.position.x, 10f, zone.transform.position.z);
        Destroy(zone);
        Instantiate(Enemies[2], enemySpawnPos, Quaternion.identity, PrefabTransforms[1]);
    }

    private IEnumerator SpawnMice()
    {
        Debug.LogWarning("Spawning Mice!!!");

        Vector3 warningZoneSpawnPos = new Vector3(_player.transform.position.x, -0.25f, _player.transform.position.z);
        List<GameObject> zones = new List<GameObject>();
        zones.Add(Instantiate(WarningZones[1], warningZoneSpawnPos, Quaternion.Euler(90, 0, 0), PrefabTransforms[0]));

        int percent = Random.Range(1, 4);
        if (percent != 1)
        {
            zones.Add(Instantiate(WarningZones[1], warningZoneSpawnPos + new Vector3(1, 0, 0), Quaternion.Euler(90, 0, 0), PrefabTransforms[0]));
            zones.Add(Instantiate(WarningZones[1], warningZoneSpawnPos - new Vector3(1, 0, 0), Quaternion.Euler(90, 0, 0), PrefabTransforms[0]));
        }
        if (percent == 3)
        {
            zones.Add(Instantiate(WarningZones[1], warningZoneSpawnPos + new Vector3(2, 0, 0), Quaternion.Euler(90, 0, 0), PrefabTransforms[0]));
            zones.Add(Instantiate(WarningZones[1], warningZoneSpawnPos - new Vector3(2, 0, 0), Quaternion.Euler(90, 0, 0), PrefabTransforms[0]));
        }

        yield return new WaitForSeconds(1.5f);

        foreach(GameObject zone in zones)
        {
            Vector3 enemySpawnPos = new Vector3(zone.transform.position.x, -0.45f, _player.transform.position.z + 50);
            Destroy(zone.gameObject);
            Instantiate(Enemies[1], enemySpawnPos, Quaternion.Euler(0, 180, 0), PrefabTransforms[1]);
        }
    }
}
