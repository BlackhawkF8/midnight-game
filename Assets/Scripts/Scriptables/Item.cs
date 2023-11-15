using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Custom/Item", order = 0)]
public class Item : ScriptableObject {
    public string itemId;
    public string displayName;
    public string description;
    public Sprite sprite;
}