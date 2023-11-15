using UnityEngine;

[RequireComponent(typeof(UserInterface))]
public class DialogueManager : MonoBehaviour {
    public static DialogueManager instance;
    private UserInterface ui;
    private Dialogue activeDialogue;
    public KeyCode advanceKey = KeyCode.Space;
    private int activeSentence = 0;
    private string[] parameters;

    private void Awake(){
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
        ui = GetComponent<UserInterface>();
    }
    public void SetDialogue(Dialogue dialogue, string[] _params){
        activeDialogue = dialogue;
        ui.Open();
        ui.SetImage(0, dialogue.character.portrait);
        ui.SetText(0, dialogue.character.displayName);
        activeSentence = -1;
        parameters = _params;
        AdvanceDialogue();
    }

    public void CloseDialogue(){
        activeDialogue = null;
        ui.WipeUI();
        ui.Close();
    }

    public void AdvanceDialogue(){
        if(!activeDialogue){
            return;
        }
        activeSentence++;
        if(activeSentence >= activeDialogue.sentences.Length){
            CloseDialogue();
            return;
        }
        ui.AppendText(1, "\n" + ReplaceParameters(activeDialogue.sentences[activeSentence]));
    }

    private string ReplaceParameters(string sentence){
        string newSentence = sentence;
        for(int i = 0; i < parameters.Length; i++){
            newSentence = newSentence.Replace("{" + i + "}", parameters[i]);
        }
        return newSentence;
    }

    private void Update(){
        if(Input.GetKeyDown(advanceKey)){
            AdvanceDialogue();
        }
    }
}