using System.Collections.Generic;
using UnityEngine;

public class InfiniteTilemap : MonoBehaviour
{
    public GameObject tilePrefab;
    public int tileSize = 10;
    public int viewDistance = 5;

    private Dictionary<Vector2Int, GroundTile> tiles = new Dictionary<Vector2Int, GroundTile>();
    private Transform playerTransform;
    private Vector2Int playerTilePosition;

    void Start()
    {
        playerTransform = Camera.main.transform;
        UpdateTiles();
    }

    void Update()
    {
        Vector2Int newPlayerTilePosition = new Vector2Int(
            Mathf.FloorToInt(playerTransform.position.x / tileSize),
            Mathf.FloorToInt(playerTransform.position.z / tileSize)
        );

        if (newPlayerTilePosition != playerTilePosition)
        {
            playerTilePosition = newPlayerTilePosition;
            UpdateTiles();
        }
    }

    void UpdateTiles()
    {
        List<Vector2Int> tilesToRemove = new List<Vector2Int>();

        foreach (var tile in tiles)
        {
            if (Vector2Int.Distance(tile.Key, playerTilePosition) > viewDistance)
            {
                tilesToRemove.Add(tile.Key);
            }
        }

        foreach (var tilePosition in tilesToRemove)
        {
            Destroy(tiles[tilePosition].tileObject);
            tiles.Remove(tilePosition);
        }

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector2Int tilePosition = new Vector2Int(playerTilePosition.x + x, playerTilePosition.y + z);

                if (!tiles.ContainsKey(tilePosition))
                {
                    Vector3 worldPosition = new Vector3(tilePosition.x * tileSize, 0, tilePosition.y * tileSize);
                    GameObject tileObject = Instantiate(tilePrefab, worldPosition, Quaternion.identity);
                    tiles.Add(tilePosition, new GroundTile(worldPosition, tileObject));
                }
            }
        }
    }
}
