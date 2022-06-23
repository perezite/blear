using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class StringHelper
{
    public static string FromLastOccurence(string haystack, string needle)
    {
        return haystack.Substring(haystack.LastIndexOf(needle) + needle.Length);
    }

    public static string UntilLastOccurence(string haystack, string needle)
    {
        return haystack.Substring(0, haystack.LastIndexOf(needle));
    }
}