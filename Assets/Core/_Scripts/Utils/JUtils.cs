﻿using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class JUtils
{
    #region Audio Utilities

    // Minimum dB value to avoid -∞ when normalized is 0
    private const float MinDb = -80.0f;

    /// <summary>
    /// Converts a dB value to a normalized 0-1 range.
    /// </summary>
    /// <param name="db">The dB value to convert.</param>
    /// <returns>A float value in the range 0-1 representing the normalized volume.</returns>
    public static float DbToNormalized(float db)
    {
        // Clamp dB to avoid values lower than the minimum (silence)
        db = Mathf.Clamp(db, MinDb, 0);
        return Mathf.Pow(10, db / 20.0f);
    }

    /// <summary>
    /// Converts a normalized 0-1 value to a dB value.
    /// </summary>
    /// <param name="normalized">The normalized value to convert, in the range 0-1.</param>
    /// <returns>A float representing the value in dB.</returns>
    public static float NormalizedToDb(float normalized)
    {
        // Clamp normalized value to avoid -∞ at 0
        normalized = Mathf.Clamp(normalized, Mathf.Epsilon, 1.0f);
        return 20.0f * Mathf.Log10(normalized);
    }

    #endregion

    private const string COLORED_TEXT_FORMAT = "<color={0}>{1}</color>";

    public static void Print<T>(this T[] array)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 0; i < array.Length; i++)
        {
            sb.Append(array[i].ToString());
            sb.Append(i < array.Length - 1 ? ", " : "");
        }
        sb.Append("]");
        Debug.Log(sb.ToString());
    }

    public static void Print<T>(this List<T> list)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("[");
        for (int i = 0; i < list.Count; i++)
        {
            sb.Append(list[i].ToString());
            sb.Append(i < list.Count - 1 ? ", " : "");
        }
        sb.Append("]");
        Debug.Log(sb.ToString());
    }

    public static int StringToSeedFNV1a(string input)
    {
        if (string.IsNullOrEmpty(input))
            return 0;

        const uint fnvPrime = 0x01000193;
        uint hash = 0x811C9DC5;

        foreach (char c in input)
        {
            hash ^= c;
            hash *= fnvPrime;
        }

        // Ensure the seed is positive and fits in an integer
        return (int)(hash & 0x7FFFFFFF);
    }

    public static string GenerateTextSlider(float normalizedValue, int count = 5)
    {
        normalizedValue = Mathf.Clamp01(normalizedValue);
        int filledCount = Mathf.RoundToInt(normalizedValue * count);

        StringBuilder sb = new StringBuilder();
        sb.Append('[');
        for (int i = 0; i < count; i++)
        {
            if (i < filledCount)
            {
                sb.Append('■');
            }
            else
            {
                sb.Append('□');
            }
        }
        sb.Append(']');
        return sb.ToString();
    }

    public static string GenerateTextSlider(int value, int min, int max, int count = 5)
    {
        count = Mathf.Max(count, 0);

        StringBuilder sb = new StringBuilder();

        // Calculate the fraction of the range covered by the current value
        float fraction = Mathf.InverseLerp(min, max, value);
        int filledCount = Mathf.RoundToInt(fraction * count);

        sb.Append('[');
        for (int i = 0; i < count; i++)
        {
            if (i < filledCount)
            {
                sb.Append('■');
            }
            else
            {
                sb.Append('□');
            }
        }
        sb.Append(']');

        return sb.ToString();
    }

    public static string FormatColor(string message, Color color)
    {
        return string.Format(COLORED_TEXT_FORMAT, "#" + ColorUtility.ToHtmlStringRGBA(color), message);
    }
}