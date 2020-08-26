using System;

namespace MyLib
{
    public static class MyMath
    {
        public const float Sqrt2 = 1.41421356237f;
        public const float PI = 3.14159265359f;
        public const float Deg2Rad = PI / 180f;

        public static bool SphereSphereIntersection(
            float x1, float y1, float radius1,
            float x2, float y2, float radius2)
        {
            return ((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))
                < ((radius1 + radius2) * (radius1 + radius2));
        }

        public static float PointToPointAngle(float fromX, float fromY, float toX, float toY)
        {
            return (float)Math.Atan2(toY - fromY, toX - fromX);
        }

        public static float DistanceBetweenTwoPoints(float fromX, float fromY, float toX, float toY)
        {
            return (float)Math.Sqrt(Math.Pow(toX - fromX, 2) + Math.Pow(toY - fromY, 2));
        }
    }
}
