using UnityEngine;

namespace AudioEngine
{
    public interface IUISoundPlayer
    {
        void SetCatalog(UISoundCatalog catalog);

        bool PlayOneShot(string soundKey);
        void PlayOneShot(AudioClip sound);
    }
}