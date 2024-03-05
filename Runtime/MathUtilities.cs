using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Tityx.Utilities
{
    public static class MathUtilities
    {
        public static Vector3 XZ(this Vector2 value)
        {
            return new Vector3(value.x, 0f, value.y);
        }

        public static Vector3 XZ(this Vector3 value)
        {
            return new Vector3(value.x, 0f, value.z);
        }

        public static Vector3 YZ(this Vector3 value)
        {
            return new Vector3(0f, value.y, value.z);
        }

        public static Vector3 XY(this Vector3 value)
        {
            return new Vector3(value.x, value.y, 0f);
        }

        public static Vector2 GetRandomPosition(Vector2 size)
        {
            return new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
        }

        public static Vector2 GetRandomPositionInsideCircle(Vector2 center, float minRadius, float maxRadius)
        {
            Vector2 circle = Random.insideUnitCircle;
            Vector2 pos = center + circle * Random.Range(minRadius, maxRadius);
            return pos;
        }

        public static Vector2 GetRandomPositionOnCircle(Vector2 center, float minRadius, float maxRadius)
        {
            Vector2 circle = Random.insideUnitCircle;
            Vector2 pos = center + circle.normalized * Random.Range(minRadius, maxRadius);
            return pos;
        }

        public static Vector3 GetVectorRotationBetweenTwoPointsIn2D(Vector2 from, Vector2 to)
        {
            Vector2 direction = to - from;
            direction.Normalize();

            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return new Vector3(0f, 0f, rot_z - 90);
        }

        public static Vector3 GetVectorRotationBetweenTwoPointsIn3D(Vector3 from, Vector3 to, bool onlyXZ)
        {
            Vector3 direction = to - from;
            if (onlyXZ)
                direction.y = 0;

            direction.Normalize();

            float rot_y = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            return new Vector3(0f, rot_y, 0f);
        }

        public static Quaternion GetQuaternionRotationBetweenTwoPointsIn2D(Vector2 from, Vector2 to)
        {
            return Quaternion.Euler(GetVectorRotationBetweenTwoPointsIn2D(from, to));
        }

        public static Quaternion GetQuaternionRotationBetweenTwoPointsIn3D(Vector3 from, Vector3 to, bool onlyXZ)
        {
            return Quaternion.Euler(GetVectorRotationBetweenTwoPointsIn3D(from, to, onlyXZ));
        }

        public static float GetDistanceXZ(Transform t1, Transform t2)
        {
            return GetDistanceXZ(t1.position, t2.position);
        }

        public static float GetDistanceXZ(Vector3 pos1, Vector3 pos2)
        {
            return Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(pos2.x, pos2.z));
        }

        public static bool IsDistanceLess(Vector2 pos1, Vector2 pos2, float distance, bool includeEquals = false)
        {
            float sqrDistance = Vector2.SqrMagnitude(pos1 - pos2);
            float powDistance = distance * distance;

            if (includeEquals) return sqrDistance <= powDistance;
            else return sqrDistance < powDistance;
        }

        public static bool IsDistanceLess(Vector3 pos1, Vector3 pos2, float distance, bool includeEquals = false)
        {
            float sqrDistance = Vector3.SqrMagnitude(pos1 - pos2);
            float powDistance = distance * distance;

            if (includeEquals) return sqrDistance <= powDistance;
            else return sqrDistance < powDistance;
        }

        public static bool IsDistanceLessXZ(Vector3 pos1, Vector3 pos2, float distance, bool includeEquals = false)
        {
            return IsDistanceLess(new Vector3(pos1.x, 0f, pos1.z), new Vector3(pos2.x, 0f, pos2.z), distance, includeEquals);
        }

        public static bool IsPointInQuad(this Vector2 pos, Vector2 tr, Vector2 br, bool includeEquals = false)
        {
            if (includeEquals)
                return
                    pos.x >= br.x && pos.x <= tr.x &&
                    pos.y >= br.y && pos.y <= tr.y;
            else
                return
                    pos.x > br.x && pos.x < tr.x &&
                    pos.y > br.y && pos.y < tr.y;
        }

        public static bool IsPointInCircle(Vector2 pos, Vector2 center, float minRadius, float maxRadius)
        {
            float sqrDistance = Vector2.SqrMagnitude(center - pos);
            return sqrDistance <= maxRadius * maxRadius && sqrDistance >= minRadius * minRadius;
        }

        public static bool IsPointInTriangle(Vector3 a, Vector3 b, Vector3 c, Vector3 pos)
        {
            // Compute vectors        
            Vector3 v0 = c - a;
            Vector3 v1 = b - a;
            Vector3 v2 = pos - a;

            // Compute dot products
            float dot00 = Vector3.Dot(v0, v0);
            float dot01 = Vector3.Dot(v0, v1);
            float dot02 = Vector3.Dot(v0, v2);
            float dot11 = Vector3.Dot(v1, v1);
            float dot12 = Vector3.Dot(v1, v2);

            // Compute barycentric coordinates
            float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            return (u >= 0) && (v >= 0) && (u + v < 1);
        }

        public static Vector3 GetPointOnBezierCurve(Vector3 start, Vector3 end, float middleOffset, float t, Vector3 crossDirection)
        {
            Vector3 direction = (end - start).normalized;
            Vector3 cross = Vector3.Cross(direction, crossDirection);
            Vector3 middle = Vector3.Lerp(start, end, 0.5f) + cross * middleOffset;
            return GetPointOnBezierCurve(start, middle, middle, end, t);
        }

        public static Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            float u = 1f - t;
            float t2 = t * t;
            float u2 = u * u;
            float u3 = u2 * u;
            float t3 = t2 * t;

            Vector3 result =
                (u3) * p0 +
                (3f * u2 * t) * p1 +
                (3f * u * t2) * p2 +
                (t3) * p3;

            return result;
        }

        public static Vector3 GetPointOnLine(Vector3 start, Vector3 end, Vector3 point)
        {
            Vector3 lineDirection = (end - start).normalized;
            Vector3 startToTouchPos = point - start;
            float dot = Vector3.Dot(startToTouchPos, lineDirection);
            return start + lineDirection * dot;
        }

        public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
        {
            bool isXFirstGreater = min.x > max.x;
            bool isYFirstGreater = min.y > max.y;
            bool isZFirstGreater = min.z > max.z;

            return new Vector3(
                Mathf.Clamp(value.x, !isXFirstGreater ? min.x : max.x, isXFirstGreater ? min.x : max.x),
                Mathf.Clamp(value.y, !isYFirstGreater ? min.y : max.y, isYFirstGreater ? min.y : max.y),
                Mathf.Clamp(value.z, !isZFirstGreater ? min.z : max.z, isZFirstGreater ? min.z : max.z)
                );
        }

        public static bool IsSuccessEquation(string equation, List<string> prms)
        {
            List<string> equationList = ParseEquation(equation);
            string answer = equationList[^1];
            //Remove '=' and answer. For example: 3+4=7 => 3+4
            equationList.RemoveAt(equationList.Count - 1);
            equationList.RemoveAt(equationList.Count - 1);

            string eq = string.Format(string.Join("", equationList), prms.ToArray());
#if UNITY_2022_3_OR_NEWER
            if (!ExpressionEvaluator.Evaluate(equation, out float result))
                return false;
#else
            if (!float.TryParse(new System.Data.DataTable().Compute(equation, "").ToString(), out float result))
                return false;
#endif

            return result.ToString() == answer;
        }

        public static List<string> ParseEquation(string equation)
        {
            equation = equation.Replace(',', '.');
            List<string> res = new List<string>();
            foreach (var match in Regex.Matches(equation, @"([*+/\-)(=])|([0-9]+)|(\{[0-9]+})"))
            {
                res.Add(match.ToString());
            }
            return res;
        }
    }
}