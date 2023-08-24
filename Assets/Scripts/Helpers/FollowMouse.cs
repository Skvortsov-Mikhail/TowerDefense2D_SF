using UnityEngine;

namespace TowerDefense
{
    public class FollowMouse : MonoBehaviour
    {
        private void Update()
        {
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(cursorPos.x, cursorPos.y);
        }
    }
}