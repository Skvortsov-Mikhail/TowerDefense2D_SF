using UnityEngine;

namespace TowerDefense
{
    public class TimeLevelCondition : MonoBehaviour, ILevelCondition
    {
        [SerializeField] private float m_TimeLimit;

        private void Start()
        {
            m_TimeLimit += Time.time;
        }

        public bool isCompleted => Time.time > m_TimeLimit;
    }
}