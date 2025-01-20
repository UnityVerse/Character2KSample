using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Add the movement vector to the moveVectors list.
        //      moveVector: Move vector to add.
        //      canSlide: Can the movement slide along obstacles?
        public static void AddMoveVector(this IEntity entity, Vector3 moveVector, bool canSlide = true)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            data.m_MoveVectors.Add(new MoveVector(moveVector, canSlide));
        }
    }
}