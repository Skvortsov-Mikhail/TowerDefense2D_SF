using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private CircleArea m_StartArea;
        public CircleArea StartArea => m_StartArea;

        [SerializeField] private AIPointPatrol[] points;
        public int PathLenght { get => points.Length; }
        public AIPointPatrol this[int i] { get => points[i]; }


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            foreach (var point in points)
            {
                Gizmos.DrawSphere(point.transform.position, point.Radius);
            }
        }
#endif
    }
}
