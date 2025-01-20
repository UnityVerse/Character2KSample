/**
* Code generation. Don't modify! 
**/

using UnityEngine;
using Atomic.Entities;
using System.Runtime.CompilerServices;
using Modules.Extensions;
using Atomic.Elements;
using Unity.Mathematics;
using Modules.Gameplay;
using System.Collections.Generic;
using Atomic.AI;

namespace Game.Gameplay
{
    public static class SandboxEntityAPI
    {
        ///Keys
        public const int Character2KData = 1; // object
        public const int PlayerMovementData = 2; // object
        public const int TriggerEventReceiver = 3; // TriggerEventReceiver
        public const int PlayerLookData = 4; // object
        public const int PlayerAnimationData = 5; // object


        ///Extensions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetCharacter2KData(this IEntity obj) => obj.GetValue<object>(Character2KData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetCharacter2KData(this IEntity obj, out object value) => obj.TryGetValue(Character2KData, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddCharacter2KData(this IEntity obj, object value) => obj.AddValue(Character2KData, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasCharacter2KData(this IEntity obj) => obj.HasValue(Character2KData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DelCharacter2KData(this IEntity obj) => obj.DelValue(Character2KData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetCharacter2KData(this IEntity obj, object value) => obj.SetValue(Character2KData, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetPlayerMovementData(this IEntity obj) => obj.GetValue<object>(PlayerMovementData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetPlayerMovementData(this IEntity obj, out object value) => obj.TryGetValue(PlayerMovementData, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddPlayerMovementData(this IEntity obj, object value) => obj.AddValue(PlayerMovementData, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasPlayerMovementData(this IEntity obj) => obj.HasValue(PlayerMovementData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DelPlayerMovementData(this IEntity obj) => obj.DelValue(PlayerMovementData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPlayerMovementData(this IEntity obj, object value) => obj.SetValue(PlayerMovementData, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TriggerEventReceiver GetTriggerEventReceiver(this IEntity obj) => obj.GetValue<TriggerEventReceiver>(TriggerEventReceiver);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetTriggerEventReceiver(this IEntity obj, out TriggerEventReceiver value) => obj.TryGetValue(TriggerEventReceiver, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddTriggerEventReceiver(this IEntity obj, TriggerEventReceiver value) => obj.AddValue(TriggerEventReceiver, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasTriggerEventReceiver(this IEntity obj) => obj.HasValue(TriggerEventReceiver);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DelTriggerEventReceiver(this IEntity obj) => obj.DelValue(TriggerEventReceiver);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetTriggerEventReceiver(this IEntity obj, TriggerEventReceiver value) => obj.SetValue(TriggerEventReceiver, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetPlayerLookData(this IEntity obj) => obj.GetValue<object>(PlayerLookData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetPlayerLookData(this IEntity obj, out object value) => obj.TryGetValue(PlayerLookData, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddPlayerLookData(this IEntity obj, object value) => obj.AddValue(PlayerLookData, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasPlayerLookData(this IEntity obj) => obj.HasValue(PlayerLookData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DelPlayerLookData(this IEntity obj) => obj.DelValue(PlayerLookData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPlayerLookData(this IEntity obj, object value) => obj.SetValue(PlayerLookData, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object GetPlayerAnimationData(this IEntity obj) => obj.GetValue<object>(PlayerAnimationData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetPlayerAnimationData(this IEntity obj, out object value) => obj.TryGetValue(PlayerAnimationData, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddPlayerAnimationData(this IEntity obj, object value) => obj.AddValue(PlayerAnimationData, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasPlayerAnimationData(this IEntity obj) => obj.HasValue(PlayerAnimationData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DelPlayerAnimationData(this IEntity obj) => obj.DelValue(PlayerAnimationData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetPlayerAnimationData(this IEntity obj, object value) => obj.SetValue(PlayerAnimationData, value);
    }
}
