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
    public static class AIEntityAPI
    {
        ///Keys
        public const int ddd = 7; 


        ///Extensions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Getddd(this IEntity obj) => obj.GetValue(ddd);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetddd(this IEntity obj, out object value) => obj.TryGetValue(ddd, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Addddd(this IEntity obj, object value) => obj.AddValue(ddd, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Delddd(this IEntity obj) => obj.DelValue(ddd);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Setddd(this IEntity obj, object value) => obj.SetValue(ddd, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Hasddd(this IEntity obj) => obj.HasValue(ddd);

    }
}
