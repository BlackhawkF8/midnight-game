using UnityEngine;


[CreateAssetMenu(fileName = "NPC", menuName = "Custom/NPC", order = 0)]
public class Character : ScriptableObject {
    public string id;
    public string displayName;
    public Sprite portrait;
}