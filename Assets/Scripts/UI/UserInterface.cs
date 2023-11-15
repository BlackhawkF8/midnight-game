using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterface : MonoBehaviour {
    public string id;
    public bool open = false;
    public bool canOverlap = true;
    public GameObject uiObject;
    public Image[] images;
    public TextMeshProUGUI[] texts;

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
    public void SetImage(int index, Sprite sprite){
        images[index].sprite = sprite;
    }
    public void SetText(int index, string text){
        texts[index].text = text;
    }
    public void AppendText(int index, string text){
        texts[index].text += text;
    }
    public void WipeUI(){
        foreach(Image image in images){
            image.sprite = null;
        }
        foreach(TextMeshProUGUI text in texts){
            text.text = "";
        }
    }
}