using UnityEngine;

public class DeleteAnimFinished : MonoBehaviour
{
    public void DestroyMe()
    {
        GameObject parent = gameObject.transform.parent.gameObject;
        Destroy(parent);
    }
}
