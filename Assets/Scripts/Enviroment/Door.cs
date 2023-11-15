using UnityEngine;

[RequireComponent(typeof(TriggerArea))]
public class Door : MonoBehaviour {
    [SerializeField] private Vector2 destination;
    [SerializeField] private bool startsClosed = true;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private Item key;
    [SerializeField] private Sprite openSprite;
    private TriggerArea triggerArea;

    private SpriteRenderer spriteRenderer;
    
    private void Awake() {
        triggerArea = GetComponent<TriggerArea>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void GoThroughDoor(){
        if(!isLocked){
            PlayerController.instance.transform.position = destination;
        }else{
            print("Door is locked");
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        #if !UNITY_EDITOR
        Gizmos.DrawSphere(triggerArea.bounds.center, 0.1f);
        #endif
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(destination, 0.1f);
    }
}