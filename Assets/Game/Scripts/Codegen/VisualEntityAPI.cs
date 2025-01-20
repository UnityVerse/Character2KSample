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
    public static class VisualEntityAPI
    {
        ///Keys
        public const int sss = 6; 


        ///Extensions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Getsss(this IEntity obj) => obj.GetValue(sss);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetsss(this IEntity obj, out object value) => obj.TryGetValue(sss, out value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Addsss(this IEntity obj, object value) => obj.AddValue(sss, value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Delsss(this IEntity obj) => obj.DelValue(sss);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Setsss(this IEntity obj, object value) => obj.SetValue(sss, value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Hassss(this IEntity obj) => obj.HasValue(sss);

    }
}
