using UnityEngine;
[CreateAssetMenu(fileName = "New Dialogue", menuName = "Custom/Dialogue")]
public class Dialogue : ScriptableObject{
    public string dialogueId;
    [TextArea(3, 10)] public string[] sentences;
    public Character character;
}