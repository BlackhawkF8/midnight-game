using UnityEngine;

public class Keytip : MonoBehaviour {
    public static Keytip instance;
    private KeyCode key;
    private string text;
    private UnityEngine.Events.UnityEvent onTrigger;
    private Vector2 worldPosition;
    private bool active = false;
    private UserInterface ui;
    private void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        ui = GetComponent<UserInterface>();
    }
    private void Update(){
        if(active){
            ui.transform.position = Camera.main.WorldToScreenPoint(worldPosition);
            if(Input.GetKeyDown(key)){
                Trigger();
            }
        }
    }

    public void Activate(KeyCode _key, string _text, UnityEngine.Events.UnityEvent _onTrigger, Vector2 _worldPosition){
        active = true;
        key = _key;
        text = _text;
        onTrigger = _onTrigger;
        worldPosition = _worldPosition;
        ui.Open();
        ui.SetText(0, key.ToString());
        ui.SetText(1, text);
    }

    public void Deactivate(){
        active = false;
        ui.WipeUI();
        ui.Close();
    }

    public void Trigger(){
        Deactivate();
    }

}