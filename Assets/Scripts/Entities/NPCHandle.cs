using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the movement and interactions of NPCs
/// </summary>
public class NPCHandle : MonoBehaviour {
    [Header("NPC Settings")]
    public Character character;
    private PlayerController player;

    [Header("Pathing Settings")]
    public bool inFollowMode = false;
    [SerializeField] private List<Vector3> path;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private float nodeReachedRange = 0.3f;
    [SerializeField] private float maxDistBetweenNodes = 2f;
    [SerializeField] private float nodeCombinationRange = 2f;
    [SerializeField]private int trackPlayerEveryTicks = 10;
    private int currentTick = 0;

    private void Start(){
        player = PlayerController.instance;
        path = new List<Vector3>();
    }

    private void FixedUpdate(){
        if(inFollowMode){
            FollowPlayer();
        }
    }

    /// <summary>
    /// Follows the player and updates the path
    /// </summary>
    private void FollowPlayer(){
        if(currentTick == trackPlayerEveryTicks){
            currentTick = 0;
            // track the player
            if(path.Count > 0){
                if(Vector2.Distance(path[path.Count - 1], player.transform.position) > maxDistBetweenNodes){
                    path.Add(player.transform.position);
                }else{
                    for(int i = 0; i < path.Count; i++){
                        if(Vector2.Distance(path[i], player.transform.position) < nodeCombinationRange && path[i].z == 0){
                            path.RemoveRange(i, path.Count - i);
                            break;
                        }
                    }
                }
            }
            path.Add(player.transform.position);
        }
        currentTick++;
        // follow the path
        if(path.Count > 0){
            if(path[0].z != 0){ // if the path is a door
                transform.position = new Vector2(path[0].x, path[0].y);
                path.RemoveAt(0);
            }else if(Vector2.Distance(transform.position, path[0]) > nodeReachedRange && Vector2.Distance(transform.position, player.transform.position) > stoppingDistance){
                transform.position = Vector2.MoveTowards(transform.position, path[0], (speed + (path.Count / 30f)) * Time.fixedDeltaTime );
            }else{
                path.RemoveAt(0);
            }
        }
    }

    public void GoThroughDoor(Door door){
        if(!inFollowMode)
            return;
        path.Add((Vector2)door.transform.position);
        path.Add(new Vector3(door.destination.x, door.destination.y, 99f));
        print(path[path.Count - 1]);
    }

    public void SayNextDialogue(){
        print("Saying next dialogue");
    }

    private void OnDrawGizmos(){
        if(path != null){
            for(int i = 0; i < path.Count; i++){
                Gizmos.color = Color.black;
                if(path[i].z != 0)
                    Gizmos.color = Color.red;
                Gizmos.DrawSphere(path[i], 0.1f);
                if(i > 0 && path[i].z == 0){
                    Gizmos.DrawLine(path[i], path[i - 1]);
                }
            }
        }
    }

}