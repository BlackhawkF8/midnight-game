using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerArea : MonoBehaviour {
    private BoxCollider2D boxCollider;
    private static PlayerController player;
    public bool isTriggered = false;
    public bool isOneShot = false;
    public KeyCode key = KeyCode.None;
    public string keyText = "";
    public Vector2 keyTipOffset = Vector2.up;
    [Tooltip("should a icon pop up to show which key to press?")]public bool showKeyTip = true;
    public UnityEngine.Events.UnityEvent onEnter;
    public UnityEngine.Events.UnityEvent onStay;
    public UnityEngine.Events.UnityEvent onLeave;
    public UnityEngine.Events.UnityEvent onKey;

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Start(){
        player = PlayerController.instance;
    }



    private void Update(){
        if(player != null){
            if(boxCollider.bounds.Contains(player.transform.position)){
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
        if(showKeyTip){
            Keytip.instance.Activate(key, keyText, onKey, (Vector2)boxCollider.bounds.center + keyTipOffset );
        }
    }
    private void onPlayerLeave(){
        onLeave.Invoke();
        if(showKeyTip){
            Keytip.instance.Deactivate();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GetComponent<BoxCollider2D>().bounds.center, GetComponent<BoxCollider2D>().bounds.size);
    }
}