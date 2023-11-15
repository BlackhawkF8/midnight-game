using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour {
    public Image portrait;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    private Dialogue activeDialogue;
    public KeyCode advanceKey = KeyCode.Space;
    private int activeSentence = 0;

    public void SetDialogue(Dialogue dialogue){
        activeSentence = 0;
        activeDialogue = dialogue;
        portrait.sprite = dialogue.character.portrait;
        nameText.text = dialogue.character.displayName;
        dialogueText.text = dialogue.sentences[0];
    }

    public void CloseDialogue(){
        activeDialogue = null;
        portrait.sprite = null;
        nameText.text = "";
        dialogueText.text = "";
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
        dialogueText.text += "\n" + activeDialogue.sentences[activeSentence];
    }

    private void Update(){
        if(Input.GetKeyDown(advanceKey)){
            AdvanceDialogue();
        }
    }
}