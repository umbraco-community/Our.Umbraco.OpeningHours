using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Our.Umbraco.OpeningHours.Extensions
{
    public static class JArrayExtensions
    {
    
        /// <summary>
        /// Gets an array of <typeparamref name="T"/> from the token matching the specified <paramref name="path"/>,
        /// using the specified delegate <paramref name="callback"/> for parsing each item in the array.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/>.</param>
        /// <param name="path">A <see cref="string"/> that contains a JPath expression.</param>
        /// <param name="callback">A callback function used for parsing or converting the token value.</param>
        public static T[] GetArray<T>(this JObject obj, string path, Func<JObject, T> callback)
        {

            if (!(obj?.SelectToken(path) is JArray token)) return null;

            return (
                from child in token
                where child is JObject
                select callback((JObject)child)
            ).ToArray();

        }
    }
}