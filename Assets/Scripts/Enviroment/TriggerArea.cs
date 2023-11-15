using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerArea : MonoBehaviour {
    public Bounds bounds;
    private static PlayerController player;
    public bool isTriggered = false;
    public bool isOneShot = false;
    public KeyCode key = KeyCode.None;
    [Tooltip("should a icon pop up to show which key to press?")]public bool showKeyTip = true;
    public UnityEngine.Events.UnityEvent onEnter;
    public UnityEngine.Events.UnityEvent onStay;
    public UnityEngine.Events.UnityEvent onLeave;
    public UnityEngine.Events.UnityEvent onKey;

    private void Awake() {
        bounds = GetComponent<BoxCollider2D>().bounds;
    }
    private void Start(){
        player = PlayerController.instance;
    }



    private void Update(){
        if(player != null){
            if(bounds.Contains(player.boxCollider.bounds.center)){
                onPlayerStay();
            }else{
                if(isTriggered){
                    isTriggered = false;
                    onPlayerLeave();
                }
            }
        }
    }

    private void onPlayerStay(){
        if(isOneShot && isTriggered) 
            return;
        if(!isTriggered){
            isTriggered = true;
            onPlayerEnter();
        }
        onStay.Invoke();
        if(key != KeyCode.None && Input.GetKeyDown(key)){
            onKey.Invoke();
        }
    }
    private void onPlayerEnter(){
        onEnter.Invoke();
    }
    private void onPlayerLeave(){
        onLeave.Invoke();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}