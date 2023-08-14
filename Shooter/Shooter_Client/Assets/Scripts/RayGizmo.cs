using UnityEngine;

public class RayGizmo : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
}
