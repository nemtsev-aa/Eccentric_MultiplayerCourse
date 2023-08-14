using UnityEngine;

public class SetSkin : MonoBehaviour {
    [SerializeField] private MeshRenderer[] _meshRenderers;

    public void SetMaterial(Material material) {
        foreach (MeshRenderer iRenderer in _meshRenderers) {
            iRenderer.material = material;
        }
    }
}
