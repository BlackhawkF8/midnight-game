using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour{
    public static LevelManager instance;
    public Bounds bounds;
    public Tilemap tilemap;
    public Vector2 tilemapOffset = Vector2.one * .5f;

    public List<CollidableAsset> collidableAssets = new List<CollidableAsset>();

    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }

        bounds = tilemap.localBounds;
        collidableAssets.AddRange(FindObjectsOfType<CollidableAsset>());
    }

    public TileBase GetTile(Vector3Int position){
        return tilemap.GetTile(position);
    }

    private void OnDrawGizmos() {
        bounds = tilemap.localBounds;
        foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            var place = tilemap.CellToWorld(localPlace) + (Vector3)tilemapOffset;
            if(tilemap.HasTile(localPlace)){
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(place, Vector3.one);
            }
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
