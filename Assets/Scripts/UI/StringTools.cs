using UnityEngine;
using System.Text.RegularExpressions;

public static class StringTools
{
    public static string SplitCamelCase(string input)
    {
        return Regex.Replace(input, "(\\B[A-Z])", " $1");
    }
}
