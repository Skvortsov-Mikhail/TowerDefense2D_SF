using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense
{
    public class BuyControl : MonoBehaviour
    {
        [SerializeField] private TowerBuyControl m_TowerBuyControlPrefab;

        // private List<TowerBuyControl> m_ActiveControls;

        private RectTransform m_RectTransform;

        private void Awake()
        {
            gameObject.SetActive(true);

            m_RectTransform = GetComponent<RectTransform>();

            // m_ActiveControls = new List<TowerBuyControl>();

            BuildSite.OnClickEvent += MoveToBuildSite;

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            BuildSite.OnClickEvent -= MoveToBuildSite;
        }

        private void MoveToBuildSite(Transform buildSite)
        {
            if (buildSite != null)
            {
                var position = Camera.main.WorldToScreenPoint(buildSite.position);

                m_RectTransform.anchoredPosition = position;

                gameObject.SetActive(true);
            }

            else
            {
                gameObject.SetActive(false);
            }

            foreach (var tbc in GetComponentsInChildren<TowerBuyControl>())
            {
                tbc.SetBuildSite(buildSite);
            }
        }
    }
}
