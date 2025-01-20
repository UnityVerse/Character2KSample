using System.Collections.Generic;
using UnityEngine;

namespace Modules.Extensions
{
    public static class ParticleExtensions
    {
        public static void PlayAll(this IEnumerable<ParticleSystem> particles, bool withChildren = true)
        {
            if (particles == null)
            {
                return;
            }

            foreach (var particle in particles)
            {
                if (particle != null)
                {
                    particle.Play(withChildren);
                }
            }
        }
    }
}