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
    public static class CoreEntityAPI
    {
        ///Keys
        public const int aaa = 8; 


        ///Extensions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Getaaa(this IEntity obj) => obj.GetValue(aaa);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetaaa(this IEntity obj, out object value) => obj.TryGetValue(aaa, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Addaaa(this IEntity obj, object value) => obj.AddValue(aaa, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Delaaa(this IEntity obj) => obj.DelValue(aaa);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Setaaa(this IEntity obj, object value) => obj.SetValue(aaa, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Hasaaa(this IEntity obj) => obj.HasValue(aaa);

    }
}
