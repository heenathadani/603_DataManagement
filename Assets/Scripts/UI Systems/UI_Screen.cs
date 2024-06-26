using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Device;

namespace Economy.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Animator))]
    public class UI_Screen : MonoBehaviour
    {
        #region Variables

        [Header("Main Properties")]
        public Selectable m_StartSelectable;

        [Header("Screen Events")]
        public UnityEvent onScreenStart = new UnityEvent();
        public UnityEvent onScreenClose = new UnityEvent();

        private Animator animator;
        #endregion

        #region Main Methods
        public void Init()
        {
            animator = GetComponent<Animator>();
            if (m_StartSelectable)
            {
                EventSystem.current.SetSelectedGameObject(m_StartSelectable.gameObject);
            }
        }
        #endregion
        #region Helper Methods
        public virtual void StartScreen()
        {
            if (onScreenStart != null)
            {
                onScreenStart.Invoke();
            }
            HandleAnimator(true);
        }
        public virtual void CloseScreen()
        {
            if (onScreenClose != null)
            {
                onScreenClose.Invoke();
            }
            HandleAnimator(false);
        }
        void HandleAnimator(bool cond)
        {
            if (animator)
            {
                animator.SetBool("show", cond);
            }
        }
        #endregion

    }
}
