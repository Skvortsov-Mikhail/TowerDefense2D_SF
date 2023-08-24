using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense
{
    public class TDPatrolController : AIController
    {
        private Path m_path;

        private int m_PathIndex;

        [SerializeField] private UnityEvent OnEndPath;

        public void SetPath(Path newPath)
        {
            m_path = newPath;
            m_PathIndex = 0;

            SetPatrolBehaviour(m_path[m_PathIndex]);
        }

        protected override void GetNewPoint()
        {
            m_PathIndex++;

            if(m_path.PathLenght > m_PathIndex)
            {
                SetPatrolBehaviour(m_path[m_PathIndex]);
            }

            else
            {
                OnEndPath.Invoke();

                Destroy(gameObject); // Здесь скорее всего будем уменьшать здоровье игрока
            }
        }
    }
}