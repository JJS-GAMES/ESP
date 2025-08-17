using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Footstep Material Surface")]
public class FootstepMaterialSurface : ScriptableObject
{
    public PhysicsMaterial Material;

    public AudioClip[] Clips;
}
