using UnityEngine;
using System.Collections.Generic;

public class UserInterfaceManager : MonoBehaviour {
    public static UserInterfaceManager instance;
    private Dictionary<string, UserInterface> userInterfaces;
    public List<UserInterface> activeUserInterfaces = new List<UserInterface>();
    private DialogueManager dialogueManager;
    private Keytip keytip;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        userInterfaces = new Dictionary<string, UserInterface>();
        foreach(UserInterface ui in FindObjectsOfType<UserInterface>()){
            userInterfaces.Add(ui.id, ui);
        }
    }

    private void Start() {
        dialogueManager = DialogueManager.instance;
        keytip = Keytip.instance;
    }

    

    public void Open(string id){
        UserInterface ui = GetUI(id);
        if(ui == null){
            Debug.LogWarning("No UI with id: " + id + " found");
            return;
        }
        if(!ui.canOverlap){
            CloseAll();
        }
        ui.Open();
        activeUserInterfaces.Add(ui);
    }

    public void Close(string id){
        UserInterface ui = GetUI(id);
        if(ui == null){
            Debug.LogWarning("No UI with id: " + id + " found");
            return;
        }
        ui.Close();
        activeUserInterfaces.Remove(ui);
    }

    public void Toggle(string id){
        UserInterface ui = GetUI(id);
        if(ui == null){
            Debug.LogWarning("No UI with id: " + id + " found");
            return;
        }
        if(ui.open){
            Close(id);
        }else{
            Open(id);
        }
    }

    public void CloseAll(){
        foreach(UserInterface ui in activeUserInterfaces){
            ui.Close();
        }
        activeUserInterfaces.Clear();
    }

    private UserInterface GetUI(string id){
        if(userInterfaces.ContainsKey(id)){
            return userInterfaces[id];
        }
        return null;
    }
}