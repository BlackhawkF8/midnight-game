using System;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour{
    public static PlayerController instance;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private KeyCode[] movementKeys = new KeyCode[4]; // Up, Right, Down, Left
    [SerializeField ]private bool[] collidingOnEdge = new bool[4]; // Top, Right, Bottom, Left
    private Vector2 moveDelta = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    [SerializeField] private float velocityDivisor = 2f;
    [SerializeField] Vector2 minMaxVelocity = new Vector2(-5, 5);
    [HideInInspector]public BoxCollider2D boxCollider;
    private static LevelManager levelManager;
    private TileBase[] surroundingTiles = new TileBase[9];
    private Vector3Int[] surroundingTilePositions = new Vector3Int[9];
    [SerializeField] private float collisionThreshold = .1f;
    [SerializeField] private float gravity = 1f;
    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        levelManager = LevelManager.instance;
        boxCollider = GetComponent<BoxCollider2D>();
        surroundingTilePositions = new Vector3Int[]{
            Vector3Int.up + Vector3Int.left,
            Vector3Int.up,
            Vector3Int.up + Vector3Int.right,
            Vector3Int.left,
            Vector3Int.zero,
            Vector3Int.right,
            Vector3Int.down + Vector3Int.left,
            Vector3Int.down,
            Vector3Int.down + Vector3Int.right
        };
    }

    void FixedUpdate(){
        movementHandle();
    }

    /// <summary>
    /// Updates the players position based on the input and the collision with the tilemap
    /// </summary>
    private void movementHandle(){
        moveDelta = Vector2.zero;
        velocity.y  += (Input.GetKey(movementKeys[0]) ? walkSpeed*Time.fixedDeltaTime : 0) + (Input.GetKey(movementKeys[2]) ? -walkSpeed*Time.fixedDeltaTime : 0);
        velocity.x  += (Input.GetKey(movementKeys[1]) ? walkSpeed*Time.fixedDeltaTime : 0) + (Input.GetKey(movementKeys[3]) ? -walkSpeed*Time.fixedDeltaTime : 0);
        velocity.y = Mathf.Clamp(velocity.y, minMaxVelocity.x, minMaxVelocity.y);
        velocity.x = Mathf.Clamp(velocity.x, minMaxVelocity.x, minMaxVelocity.y);
        velocity.y /= velocityDivisor;
        velocity.x /= velocityDivisor;
        if(Mathf.Abs(velocity.x) < .01f){
            velocity.x = 0;
        }
        if(Mathf.Abs(velocity.y) < .01f){
            velocity.y = 0;
        }
        moveDelta.y += velocity.y;
        moveDelta.x += velocity.x;
        Vector3Int playerTilePosition = levelManager.tilemap.WorldToCell((Vector2)transform.position+(Vector2.right*.5f));
        for(int i = 0; i < surroundingTilePositions.Length; i++){
            surroundingTiles[i] = levelManager.GetTile(playerTilePosition + surroundingTilePositions[i]);
        }
        
        for(int i=0;i<4;i++){
            collidingOnEdge[i] = false;
        }
        Bounds simulatedPlayerBounds = boxCollider.bounds;
        simulatedPlayerBounds.center += (Vector3)moveDelta;
        Vector2 collisionMovement = Vector2.zero;
        for(int i = 0; i < surroundingTiles.Length; i++){
            if(!surroundingTiles[i]){
                continue;
            }
            Bounds tileBounds = levelManager.tilemap.GetBoundsLocal(playerTilePosition + surroundingTilePositions[i]);
            tileBounds.center += (Vector3)levelManager.tilemapOffset;
            Vector2 offset = getMovementOffset(simulatedPlayerBounds, tileBounds);
            if(collisionMovement.x == 0){
                collisionMovement.x = offset.x;
            }
            if(collisionMovement.y == 0){
                collisionMovement.y = offset.y;
            }
        }
        foreach(CollidableAsset ca in levelManager.collidableAssets){
            Vector2 offset = getMovementOffset(simulatedPlayerBounds, ca.bounds);
            if(collisionMovement.x == 0){
                collisionMovement.x = offset.x;
            }
            if(collisionMovement.y == 0){
                collisionMovement.y = offset.y;
            }
        }

        transform.position = roundVectorToPlaces((Vector2)transform.position + moveDelta + collisionMovement, 2);
    }

    private Vector2 getMovementOffset(Bounds a, Bounds b){
        Vector3 offset = Vector3.zero;
        if(!a.Intersects(b))
            return offset;
        
        float minX = a.min.x - b.min.x;
        float maxX = b.max.x - a.max.x;
        float minY = a.min.y - b.min.y;
        float maxY = b.max.y - a.max.y;

        bool isAbove = maxY < 0;
        bool isBelow = minY < 0;
        bool isLeft = minX < 0;
        bool isRight = maxX < 0;

        float overlapAbove = b.max.y - a.min.y;
        float overlapBelow = -(a.max.y - b.min.y);
        float overlapLeft = -(a.max.x - b.min.x);
        float overlapRight = b.max.x - a.min.x;

        switch(isAbove, isBelow, isLeft, isRight){
            case (false, false, false, false): // inside
                Debug.LogError("Object stuck inside!");
                break;
            case (true, false, false, false): // above
                offset.y = overlapAbove;
                break;
            case (false, true, false, false): // below
                offset.y = overlapBelow;
                break;
            case (false, false, true, false): // left
                offset.x = overlapLeft;
                break;
            case (false, false, false, true): // right
                offset.x = overlapRight;
                break;
            case (true, false, true, false): // above left
                if(minX < maxY){
                    offset.x = overlapLeft;
                }else{
                    offset.y = overlapAbove;
                }
                break;
            case (true, false, false, true): // above right
                if(maxX < maxY){
                    offset.x = overlapRight;
                }else{
                    offset.y = overlapAbove;
                }
                break;
            case (false, true, true, false): // below left
                if(minX < minY){
                    offset.x = overlapLeft;
                }else{
                    offset.y = overlapBelow;
                }
                break;
            case (false, true, false, true): // below right
                if(maxX < minY){
                    offset.x = overlapRight;
                }else{
                    offset.y = overlapBelow;
                }
                break;
        }
        return offset;
    }

    private Vector2 roundVectorToPlaces(Vector2 vector, int places){
        return new Vector2(Mathf.Round(vector.x * Mathf.Pow(10, places)) / Mathf.Pow(10, places), Mathf.Round(vector.y * Mathf.Pow(10, places)) / Mathf.Pow(10, places));
    }
}
