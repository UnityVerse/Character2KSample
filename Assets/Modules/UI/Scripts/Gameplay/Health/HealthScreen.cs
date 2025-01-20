using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.UI
{
    public sealed class HealthScreen : MonoBehaviour
    {
        private const float LOW_INJURED = 0.75f;
        private const float MEDIUM_INJURED = 0.5f;
        private const float HIGH_INJURED = 0.25f;
        private const float CRITICAL_INJURED = 0.10f;
        
        [SerializeField]
        private Image staticIndication;

        [SerializeField]
        private Image dynamicIndication;

        [SerializeField]
        private Image takeDamageView;

        private Sequence takeDamageTween;
        private Tweener criticalTween;

        private void Awake()
        {
            this.takeDamageView.color = new Color(1, 1, 1, 0);
            this.dynamicIndication.color = new Color(1, 1, 1, 0);
        }

        public void ChangePercent(float percent)
        {
            switch (percent)
            {
                case > LOW_INJURED:
                    this.DisableInjure();
                    break;
                case <= LOW_INJURED and > MEDIUM_INJURED:
                    this.SetLowInjure();
                    break;
                case <= MEDIUM_INJURED and > HIGH_INJURED:
                    this.SetMediumInjure();
                    break;
                case <= HIGH_INJURED and > CRITICAL_INJURED:
                    this.SetHighInjure();
                    break;
                case <= CRITICAL_INJURED:
                    this.SetCriticalInjure();
                    break;
            }
        }

        public void TakeDamage(int damage)
        {
            if (this.takeDamageTween != null)
            {
                this.takeDamageTween.Kill();
            }

            this.takeDamageTween = DOTween
                .Sequence()
                .Append(this.takeDamageView.DOFade(Mathf.Clamp(damage / 40.0f, 0.2f, 1.0f), 0.05f))
                .Append(this.takeDamageView.DOFade(0, 0.2f));
        }
        
        private void StartCriticalTween()
        {
            if (this.criticalTween != null)
            {
                return;
            }
            
            this.dynamicIndication.color = new Color(1, 1, 1, 0);
            this.criticalTween = this.dynamicIndication
                .DOFade(0.85f, 0.75f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        
        private void StopCriticalTween()
        {
            if (this.criticalTween == null)
            {
                return;
            }

            this.criticalTween.Kill();
            this.criticalTween = null;
            this.dynamicIndication.color = new Color(1, 1, 1, 0);
        }

      
        
        private void SetCriticalInjure()
        {
            this.staticIndication.color = new Color(1, 1, 1, 0.8f);
            this.StartCriticalTween();
        }
       
        private void SetHighInjure()
        {
            this.staticIndication.color = new Color(1, 1, 1, 0.6f);
            this.StopCriticalTween();
        }

        private void SetMediumInjure()
        {
            this.staticIndication.color = new Color(1, 1, 1, 0.4f);
            this.StopCriticalTween();
        }

        private void SetLowInjure()
        {
            this.staticIndication.color = new Color(1, 1, 1, 0.2f);
            this.StopCriticalTween();
        }

        private void DisableInjure()
        {
            this.staticIndication.color = new Color(1, 1, 1, 0);
            this.StopCriticalTween();
        }
    }
}