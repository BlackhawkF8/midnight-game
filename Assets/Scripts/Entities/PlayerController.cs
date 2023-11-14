using System;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private KeyCode[] movementKeys = new KeyCode[4]; // Up, Right, Down, Left
    [SerializeField ]private bool[] collidingOnEdge = new bool[4]; // Top, Right, Bottom, Left
    private Vector2 moveDelta = Vector2.zero;
    private Vector2 velocity = Vector2.zero;
    [SerializeField] private float velocityDivisor = 2f;
    [SerializeField] Vector2 minMaxVelocity = new Vector2(-5, 5);
    private BoxCollider2D boxCollider;
    private static LevelManager levelManager;
    private TileBase[] surroundingTiles = new TileBase[9];
    private Vector3Int[] surroundingTilePositions = new Vector3Int[9];
    [SerializeField] private float collisionThreshold = .1f;
    [SerializeField] private float gravity = 1f;
    private void Awake() {
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
            // Each of these horrible ifs checks if the player is colliding with the tile and if so, moves the player out of the tile
            if(collisionMovement.y == 0 && simulatedPlayerBounds.max.y >= tileBounds.min.y && simulatedPlayerBounds.min.y < tileBounds.min.y && simulatedPlayerBounds.max.x > tileBounds.min.x +collisionThreshold && simulatedPlayerBounds.min.x < tileBounds.max.x - collisionThreshold){
                collidingOnEdge[0] = true;
                float distance = tileBounds.min.y - simulatedPlayerBounds.max.y;
                collisionMovement.y += distance;
            }
            if(collisionMovement.x == 0 && simulatedPlayerBounds.min.x <= tileBounds.max.x && simulatedPlayerBounds.max.x > tileBounds.max.x && simulatedPlayerBounds.max.y > tileBounds.min.y + collisionThreshold && simulatedPlayerBounds.min.y < tileBounds.max.y - collisionThreshold){
                collidingOnEdge[1] = true;
                float distance = tileBounds.max.x - simulatedPlayerBounds.min.x;
                collisionMovement.x += distance;
            }
            if(collisionMovement.y == 0 && simulatedPlayerBounds.min.y <= tileBounds.max.y && simulatedPlayerBounds.max.y > tileBounds.max.y && simulatedPlayerBounds.max.x > tileBounds.min.x + collisionThreshold && simulatedPlayerBounds.min.x < tileBounds.max.x - collisionThreshold){
                collidingOnEdge[2] = true;
                float distance = tileBounds.max.y - simulatedPlayerBounds.min.y;
                collisionMovement.y += distance;
            }
            if(collisionMovement.x == 0 && simulatedPlayerBounds.max.x >= tileBounds.min.x && simulatedPlayerBounds.min.x < tileBounds.min.x && simulatedPlayerBounds.max.y > tileBounds.min.y + collisionThreshold && simulatedPlayerBounds.min.y < tileBounds.max.y - collisionThreshold){
                collidingOnEdge[3] = true;
                float distance = tileBounds.min.x - simulatedPlayerBounds.max.x;
                collisionMovement.x += distance;
            }
        }
        transform.position = roundVectorToPlaces((Vector2)transform.position + moveDelta + collisionMovement, 2);
    }
    private Vector2 roundVectorToPlaces(Vector2 vector, int places){
        return new Vector2(Mathf.Round(vector.x * Mathf.Pow(10, places)) / Mathf.Pow(10, places), Mathf.Round(vector.y * Mathf.Pow(10, places)) / Mathf.Pow(10, places));
    }
}
