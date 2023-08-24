using UnityEngine;

namespace TowerDefense
{
    public class WizardInMagicTower : MonoBehaviour
    {
        private Tower parentTower;
        private SpriteRenderer sr;
        private Animator anim;

        public void SetParentTower(Tower tower)
        {
            parentTower = tower;
        }

        private void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if (parentTower == null) return;

            if (parentTower.Target == null)
            {
                anim.enabled = false;
                anim.recorderStartTime = 0;
            }

            else
            {
                anim.enabled = true;

                if (parentTower.Target.transform.position.x > transform.position.x)
                {
                    sr.flipX = true;
                }
                else
                {
                    sr.flipX = false;
                }
            }
        }
    }
}
