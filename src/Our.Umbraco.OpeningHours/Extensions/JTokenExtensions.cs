using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Our.Umbraco.OpeningHours.Extensions
{

    public static class JObjectExtensions
    {
        /// <summary>
        /// Gets an object from a token matching the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="obj">The parent object.</param>
        /// <param name="path">A <see cref="string"/> that contains a JPath expression.</param>
        /// <param name="func">The delegate (callback method) used for parsing the object.</param>
        /// <returns>An instance of <typeparamref name="T"/>, or the default value of <typeparamref name="T"/> if not
        /// found.</returns>
        public static T GetObject<T>(this JObject obj, string path, Func<JObject, T> func)
        {
            return obj == null ? default(T) : func(obj.SelectToken(path) as JObject);
        }

        /// <summary>
        /// Gets the string value of the token matching the specified <paramref name="path"/>, or <c>null</c> if
        /// <paramref name="path"/> doesn't match a token.
        /// </summary>
        /// <param name="obj">The parent object.</param>
        /// <param name="path">A <see cref="string"/> that contains a JPath expression.</param>
        /// <returns>An instance of <see cref="string"/>, or <c>null</c>.</returns>
        public static string GetString(this JObject obj, string path)
        {
            if (obj == null) return null;
            JToken token = GetSimpleTypeTokenFromPath(obj, path);
            return token?.Value<string>();
        }

        /// <summary>
        /// Gets the value of the token matching the specified <paramref name="path"/>, or <c>null</c> if
        /// <paramref name="path"/> doesn't match a token.
        /// </summary>
        /// <param name="obj">The parent object.</param>
        /// <param name="path">A <see cref="string"/> that contains a JPath expression.</param>
        /// <param name="callback">The callback used for converting the string value.</param>
        /// <returns>An instance of <typeparamref name="T"/>, or <c>null</c>.</returns>
        public static T GetString<T>(this JObject obj, string path, Func<string, T> callback)
        {
            if (obj == null) return default(T);
            JToken token = GetSimpleTypeTokenFromPath(obj, path);
            return token == null ? default(T) : callback(token.Value<string>());
        }

        /// <summary>
        /// Gets the <see cref="bool"/> value of the token matching the specified <paramref name="path"/>, or
        /// <c>0</c> if <paramref name="path"/> doesn't match a token.
        /// </summary>
        /// <param name="obj">The parent object.</param>
        /// <param name="path">A <see cref="string"/> that contains a JPath expression.</param>
        /// <returns>An instance of <see cref="bool"/>.</returns>
        public static bool GetBoolean(this JObject obj, string path)
        {
            return GetBoolean(obj, path, x => x);
        }

        /// <summary>
        /// Gets the <see cref="bool"/> value of the token matching the specified <paramref name="path"/> and parses
        /// it into an instance of <typeparamref name="T"/>, or the default value of <typeparamref name="T"/> if
        /// <paramref name="path"/> doesn't match a token.
        /// </summary>
        /// <param name="obj">The parent object.</param>
        /// <param name="path">A <see cref="string"/> that contains a JPath expression.</param>
        /// <param name="callback">A callback function used for parsing or converting the token value.</param>
        /// <returns>An instance of <see cref="bool"/>, or <c>false</c> if <paramref name="path"/>
        /// doesn't match a token.</returns>
        public static T GetBoolean<T>(this JObject obj, string path, Func<bool, T> callback)
        {

            // Get the token from the path
            JToken token = GetSimpleTypeTokenFromPath(obj, path);

            // Check whether the token is null
            if (token == null || token.Type == JTokenType.Null) return default(T);

            // Convert the value to a boolean
            bool value = token.ToString().Equals("true",StringComparison.InvariantCultureIgnoreCase);

            // Invoke the callback and return the value
            return callback(value);

        }

        /// <summary>
        /// Gets the <see cref="JToken"/> at the specified <paramref name="path"/>. If the type of the token is either
        /// <see cref="JTokenType.Object"/> or <see cref="JTokenType.Array"/>, the method will return
        /// <c>null</c> instead.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/>.</param>
        /// <param name="path">A <see cref="string"/> that contains a JPath expression.</param>
        /// <returns>An instance of <see cref="JToken"/>, or <c>null</c>.</returns>
        private static JToken GetSimpleTypeTokenFromPath(JObject obj, string path)
        {
            JToken token = obj?.SelectToken(path);
            return token == null || token.Type == JTokenType.Object || token.Type == JTokenType.Array ? null : token;
        }

        /// <summary>
        /// Gets the items of the <see cref="JArray"/> from the token matching the specfied <paramref name="path"/>.
        /// </summary>
        /// <param name="obj">The instance of <see cref="JObject"/>.</param>
        /// <param name="path">A <see cref="string"/> that contains a JPath expression.</param>
        /// <param name="callback">A callback function used for parsing or converting the token value.</param>
        /// <returns>An array of <typeparamref name="T"/>. If the a matching token isn't found, an empty array will
        /// still be returned.</returns>
        public static T[] GetArrayItems<T>(this JObject obj, string path, Func<JObject, T> callback)
        {

            if (!(obj?.SelectToken(path) is JArray token)) return new T[0];

            return (
                from JObject child in token
                select callback(child)
            ).ToArray();

        }


    }

}