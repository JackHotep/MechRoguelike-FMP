using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Weapons/New Trail", order = 2)]
public class TrailConfig : ScriptableObject
{
    public Material material;
    public AnimationCurve widthAnim;
    public float duration = 0.2f;
    public float minVert = 0.1f;
    public Gradient colour;

    public float flyDist = 100f;
    public float tracerSpeed = 200f;
}
