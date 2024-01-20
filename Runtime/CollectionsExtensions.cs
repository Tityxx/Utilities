using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Tityx.Utilities
{
    public static class CollectionsExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            int index = UnityEngine.Random.Range(0, list.Count);
            return list[index];
        }

        public static List<T> GetRandomElements<T>(this List<T> list, int count)
        {
            var tmp = new List<T>(list);
            tmp.Shuffle();
            var result = new List<T>();
            for (int i = 0; i < count && tmp.Count > 0; i++)
            {
                result.Add(tmp[0]);
                tmp.RemoveAt(0);
            }
            return result;
        }

        public static void SetActive(this List<GameObject> list, bool active)
        {
            foreach (var obj in list)
            {
                obj.SetActive(active);
            }
        }

        public static T[,] GetTwoDimensionArrayFromList<T>(this IList<T> list, int rows, int columns)
        {
            T[,] array = new T[rows, columns];

            for (int y = 0, index = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++, index++)
                {
                    array[y, x] = list[index];
                }
            }
            return array;
        }

        public static List<T> GetListFromTwoDimensionArray<T>(this T[,] array)
        {
             var list = new List<T>();
            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    int index = y * array.GetLength(0) + x;
                    list[index] = array[y, x];
                }
            }
            return list;
        }

        public static T MinBy<T, TResult>(this List<T> list, Func<T, TResult> func) where TResult : IComparable
        {
            var value = list[0];
            var min = func(value);

            for (int i = 1; i < list.Count; i++)
            {
                var tmpMin = func(list[i]);
                if (tmpMin.CompareTo(min) < 0)
                {
                    value = list[i];
                    min = tmpMin;
                }
            }
            return value;
        }

        public static T MaxBy<T, TResult>(this List<T> list, Func<T, TResult> func) where TResult : IComparable
        {
            var value = list[0];
            var max = func(value);

            for (int i = 1; i < list.Count; i++)
            {
                var tmpMax = func(list[i]);
                if (tmpMax.CompareTo(max) > 0)
                {
                    value = list[i];
                    max = tmpMax;
                }
            }
            return value;
        }
    }
}