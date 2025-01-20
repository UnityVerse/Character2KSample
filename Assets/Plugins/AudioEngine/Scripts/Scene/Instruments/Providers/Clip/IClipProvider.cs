using UnityEngine;

namespace AudioEngine
{
    internal interface IClipProvider
    {
        AudioClip Value { get; }
        
        float MaxLength { get; }
    }
}