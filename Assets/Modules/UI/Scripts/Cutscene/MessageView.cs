using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Modules.UI
{
    public sealed class MessageView : MonoBehaviour
    {
        [Serializable]
        public struct Args
        {
            public string sender;
            public Color senderColor;
            
            [Space, TextArea]
            public string message;
            public Color messageColor;
            
            [Space]
            public float duration;
        }
        
        [SerializeField]
        private TMP_Text text;

        private Coroutine coroutine;
        
        public void SetVisible(bool visible)
        {
            this.text.enabled = visible;
        }
        
        public void SetMessage(string message)
        {
            this.text.text = message;
        }

        public void ShowMessage(Args args)
        {
            string message = $"<b><color=#{ColorUtility.ToHtmlStringRGBA(args.senderColor)}>{args.sender}:</color> " +
                             $"<color=#{ColorUtility.ToHtmlStringRGBA(args.messageColor)}>{args.message}</color></b>";
         
            this.text.enabled = true;
            this.text.text = message;

            if (this.coroutine != null)
            {
                this.StopCoroutine(this.coroutine);
            }
            
            this.coroutine = this.StartCoroutine(this.HideMessageRoutine(args.duration));
        }

        private IEnumerator HideMessageRoutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            this.HideMessage();
            this.coroutine = null;
        }
        
        public void ShowMessage(string message)
        {
            this.text.enabled = true;
            this.text.text = message;
        }
        
        public void ShowMessage(string sender, string message, Color color)
        {
            this.text.enabled = true;
            this.text.text = $"<b><color=#{ColorUtility.ToHtmlStringRGBA(color)}>{sender}:</color> {message}</b>";
        }

        public void HideMessage()
        {
            this.text.text = string.Empty;
            this.text.enabled = false;
        }
    }
}