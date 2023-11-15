using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CollidableAsset : MonoBehaviour
{
    public Bounds bounds;
    private void Awake(){
        bounds = GetComponent<BoxCollider2D>().bounds;
    }
}
