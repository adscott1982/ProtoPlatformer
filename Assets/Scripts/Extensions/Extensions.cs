using System;
using UnityEngine;

namespace Extensions
{
    public static class Extensions
    {
        public static void ClampXMaxSpeed(this Rigidbody2D rb, float maxSpeed)
        {
            // If not currently at max speed, return
            if (!(Math.Abs(rb.velocity.x) > maxSpeed))
            {
                return;
            }

            var velocity = rb.velocity;
            velocity.x = velocity.x > 0 ? maxSpeed : -maxSpeed;
            rb.velocity = velocity;
        }

        public static void ClampYMaxSpeed(this Rigidbody2D rb, float maxSpeed)
        {
            // If not currently at max speed, return
            if (!(Math.Abs(rb.velocity.y) > maxSpeed))
            {
                return;
            }

            var velocity = rb.velocity;
            velocity.y = velocity.y > 0 ? maxSpeed : -maxSpeed;
            rb.velocity = velocity;
        }

        public static void DecelerateX(this Rigidbody2D rb, float deceleration)
        {
            var velocity = rb.velocity;
            deceleration = deceleration * Time.fixedDeltaTime;

            if (Mathf.Abs(velocity.x) - deceleration < 0)
            {
                velocity.x = 0f;
            }
            else
            {
                velocity.x = velocity.x > 0 ? velocity.x - deceleration : velocity.x + deceleration;
            }

            rb.velocity = velocity;
        }

        public static void FlipXScale(this Transform transform)
        {
            var currentScale = transform.localScale;
            currentScale.x = -currentScale.x;
            transform.localScale = currentScale;
        }

        public static bool IsApproxZero(this float value)
        {
            return Math.Abs(value) < AndyTools.FloatEqualityTolerance;
        }
    }
}
