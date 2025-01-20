using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        public static bool CanSetHeight(this IEntity entity, float newHeight, bool preserveFootPosition)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // vis2k fix:
            // IMPORTANT: adjust height BEFORE ever calculating the center.
            //            previously it was adjusted AFTER calculating the center.
            //            so the center would NOT EXACTLY be the center anymore
            //            if the height was adjusted.
            //            => causing all future center calculations to be wrong.
            //            => causing center.y to increase every time
            //            => causing the character to float in the air over time
            //            see also: https://github.com/vis2k/uMMORPG/issues/36
            newHeight = entity.ValidateHeight(newHeight);

            // calculate the new capsule center & height
            Vector3 newCenter = preserveFootPosition ? 
                Helpers.CalculateCenterWithSameFootPosition(data.center, data.height, newHeight, data.skinWidth)
                : data.center;
            
            if (Mathf.Approximately(data.height, newHeight))
            {
                // Height remains the same
                return true;
            }

            return entity.CanSetHeightAndCenter(newHeight, newCenter);
        }
    }
}