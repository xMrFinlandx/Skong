﻿using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Extensions
    {
        public static Vector2 To4DVector(this Vector2 direction)
        {
            var x = direction.x;
            var y = direction.y;

            if (Mathf.Abs(y) > 0)
            {
                y = Mathf.Round(y);
                x = 0;
            }
            else
            {
                x = Mathf.Round(x);
                y = 0;
            }

            return new Vector2(x, y);
        }

        public static T GetRandom<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static void PlayWithRandomStart(this Animator animator, AnimationClip clip)
        {
            animator.Play(clip.name, 0, Random.Range(0, clip.length));
        }
    }
}