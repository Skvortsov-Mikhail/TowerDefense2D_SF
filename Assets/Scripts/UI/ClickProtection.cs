using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefense
{
    public class ClickProtection : SingletoneBase<ClickProtection>, IPointerClickHandler
    {
        private Image blocker;
        private Action<Vector2> m_OnclickAction;

        private void Start()
        {
            blocker = GetComponent<Image>();

            blocker.enabled = false;
        }

        public void Activate(Action<Vector2> mouseAction)
        {
            blocker.enabled = true;

            m_OnclickAction = mouseAction;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            blocker.enabled = false;

            m_OnclickAction(eventData.pressPosition);

            m_OnclickAction = null;
        }
    }
}