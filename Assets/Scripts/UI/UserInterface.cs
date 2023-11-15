using UnityEngine;

public class UserInterface : MonoBehaviour {
    public string id;
    public bool open = false;
    public bool canOverlap = true;
    public GameObject uiObject;

    public void Open(){
        uiObject.SetActive(true);
        open = true;
    }
    public void Close(){
        uiObject.SetActive(false);
        open = false;
    }
    public void Toggle(){
        if(open){
            Close();
        }else{
            Open();
        }
    }
}