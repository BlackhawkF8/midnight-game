using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour{
    public static LevelManager instance;
    public Bounds bounds;
    public Tilemap tilemap;


    void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    public TileBase GetTile(Vector3Int position){
        return tilemap.GetTile(position);
    }

    private void OnDrawGizmos() {
        bounds = tilemap.localBounds;
        foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
            var localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            var place = tilemap.CellToWorld(localPlace);
            if(tilemap.HasTile(localPlace)){
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(place, Vector3.one);
            }
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
