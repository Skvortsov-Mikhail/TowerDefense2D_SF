using UnityEngine;

namespace TowerDefense
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float m_Lifetime;

        private float m_Timer;

        void Update()
        {
            if(m_Timer < m_Lifetime)
            {
                m_Timer += Time.deltaTime;
            }

            else
            {
                Destroy(gameObject);
            }
        }
    }
}