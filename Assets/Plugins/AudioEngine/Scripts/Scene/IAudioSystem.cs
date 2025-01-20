using System;
using UnityEngine;

namespace AudioEngine
{
    public interface IAudioSystem
    {
        bool LoadBank(AudioBank audioBank);
        bool UnloadBank(AudioBank audioBank);

        void Pause();
        void Resume();

        bool PlayEvent(string eventId, float threshold = 0, AudioArgs args = null);
        bool PlayEvent(string eventId, Vector3 position, float threshold = 0, AudioArgs args = null);
        bool PlayEvent(string eventId, Vector3 position, Quaternion rotation, float threshold = 0, AudioArgs args = null);
        bool PlayEvent(string eventId, Transform pivot, float threshold = 0, AudioArgs args = null);

        bool TryCreateEvent(string eventId, out AudioEventHandle result);
        bool TryCreateEvent(string eventId, Vector3 position, Quaternion rotation, out AudioEventHandle result);
        AudioEventHandle CreateEvent(string eventId);
        AudioEventHandle CreateEvent(string eventId, Vector3 position, Quaternion rotation);

        bool DisposeEvent(string eventId);
        bool DisposeEvent(string eventId, float fadeoutTime); //?
        bool DisposeEvent(string eventId, AnimationCurve curve, float fadeoutTime); //?
        
        void DisposeEvents(string eventId);
        void DisposeEvents(string eventId, float fadeoutTime);
        bool DisposeEvents(string eventId, AnimationCurve curve, float fadeoutTime);

        bool FindEvent(string eventId, out AudioEventHandle result);
        int FindEventsNonAlloc(string eventId, AudioEventHandle[] results);

        bool TryGetBool(string paramId, out bool result);
        bool TryGetInt(string paramId, out int result);
        bool TryGetFloat(string paramId, out float result);

        bool GetBool(string paramId);
        int GetInt(string paramId);
        float GetFloat(string paramId);

        void SetBool(string paramId, bool value);
        void SetInt(string paramId, int value);
        void SetFloat(string paramId, float value);

        void SetTrigger(string triggerId, Action trigger); //?
        void ResetTrigger(string triggerId);
        
        void ListenCallback(string callbackId, Action callback);
        void UnlistenCallback(string callbackId, Action callback);
    }
}