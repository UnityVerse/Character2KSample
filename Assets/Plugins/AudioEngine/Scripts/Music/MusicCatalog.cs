using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioEngine
{
    [CreateAssetMenu(
        fileName = "MusicCatalog",
        menuName = "AudioEngine/Music/New MusicCatalog"
    )]
    public sealed class MusicCatalog : ScriptableObject, IReadOnlyList<AudioClip>
    {
        [SerializeField]
        private List<AudioClip> clips;

        public int Count => this.clips.Count;

        public AudioClip this[int index]
        {
            get { return this.clips[index]; }
        }

        public IEnumerator<AudioClip> GetEnumerator()
        {
            return this.clips.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}