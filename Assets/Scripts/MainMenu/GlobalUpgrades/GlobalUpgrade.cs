using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class GlobalUpgrade : MonoBehaviour
    {
        [SerializeField] private UpgradeAsset m_Asset;

        [SerializeField] private Image m_UpgradeIcon;
        [SerializeField] private Text m_LvlText;
        [SerializeField] private Text m_CostText;
        [SerializeField] private Button m_BuyButton;
        [SerializeField] private GameObject m_PricePanel;

        private int m_UpgradeCost;

        public void Initialize()
        {
            m_UpgradeIcon.sprite = m_Asset.sprite;

            int level = Upgrades.GetUpgradeLevel(m_Asset);

            if(level >= m_Asset.costByLevel.Length)
            {
                m_LvlText.text = level.ToString();

                m_BuyButton.interactable = false;

                var buttonText = m_BuyButton.GetComponentInChildren<Text>();
                buttonText.color = Color.red;
                buttonText.text = "Максимум";

                m_PricePanel.SetActive(false);
            }
            else
            {
                m_LvlText.text = level.ToString();

                m_UpgradeCost = m_Asset.costByLevel[level];

                m_CostText.text = m_UpgradeCost.ToString();
            }
        }

        public void Buy()
        {
            Upgrades.BuyUpgrade(m_Asset);
            Initialize();
        }

        public void CheckCost(int money)
        {
            m_BuyButton.interactable = money >= m_UpgradeCost;
        }
    }
}