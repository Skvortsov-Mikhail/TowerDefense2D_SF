using UnityEngine;

namespace TowerDefense
{
    public class TrapPoint : MonoBehaviour
    {
#if UNITY_EDITOR
        private static readonly Color GizmosColor = new Color(1, 0, 1, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmosColor;
            Gizmos.DrawSphere(transform.position, 0.2f);
        }
#endif
    }
}
