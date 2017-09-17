using System;
using System.Collections.Generic;

public static class Extenstions
{
    public static void Do<T>(this IList<T> enumerable, Action<T> todo)
    {
        for (int i = 0; i < enumerable.Count; i++)
        {
            todo(enumerable[i]);
        }
    }
}