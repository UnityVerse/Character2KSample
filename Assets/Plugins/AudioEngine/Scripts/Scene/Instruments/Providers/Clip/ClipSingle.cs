using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AudioEngine
{
    [Serializable, InlineProperty] 
    internal class ClipSingle : IClipProvider
    {
        [SerializeField]
        private AudioClip clip;

        public AudioClip Value => this.clip;

        public float MaxLength => this.clip != null ? clip.length : 0;
    }
}