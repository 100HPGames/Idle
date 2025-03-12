// ––‒–‒––––––‒–‒‒‒––‒––––––‒––––‒––‒‒–
// Коммерческая лицензия подписчика
// (c) 2023-2025 Leopotam <leopotam@yandex.ru>
// ––‒–‒––––––‒–‒‒‒––‒––––––‒––––‒––‒‒–

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Leopotam.GoogleDocs.Unity
{ 
    public static class GoogleDocsLoader
    {
        public static async Task<(string, string)> LoadCsvPage(string fullPageUrl)
        {
            var url = fullPageUrl
                .Replace("?", "")
                .Replace("#", "&")
                .Replace("/edit", "/export?format=csv&")
                .Replace("&&", "&");
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out _))
            {
                return (default, $"Ошибочный адрес ресурса: \"{fullPageUrl}\"");
            }

            using (var client = UnityWebRequest.Get(url))
            {
                try
                {
                    await client.SendWebRequest();
                    if (client.error != null)
                    {
                        return (null, client.error);
                    }

                    var data = client.downloadHandler.text;
                    return (data, default);
                }
                catch (Exception ex)
                {
                    return (default, ex.Message);
                }
            }
        }
        
        public static async Task<(Dictionary<string, string>, string)> LoadFullPage(string fullPageUrl, string dividerTag)
        {
            var url = fullPageUrl
                .Replace("?", "")
                .Replace("#", "&")
                .Replace("/edit", "/export?format=csv&")
                .Replace("&&", "&");
            using (var client = UnityWebRequest.Get(url))
            {
                try
                {
                    await client.SendWebRequest();
                    if (client.error != null)
                        return (null, client.error);

                    var data = client.downloadHandler.text;
                    List<string> innerTags = GetInnerTags(data, dividerTag);
                    Dictionary<string, string> dict = FillPageSections(data, innerTags, dividerTag);
                    return (dict, default);
                }
                catch (Exception ex)
                {
                    return (default, ex.Message);
                }
            }
        }
        
        private static List<string> GetInnerTags(string data, string dividerTag)
        {
            List<string> tags = new List<string>();
            string tag = "";
            int currentTagIndex = 0;
            do
            {
                int start = data.IndexOf(dividerTag, currentTagIndex, StringComparison.Ordinal) + dividerTag.Length + 1;
                int length = data.IndexOf(",", start, StringComparison.Ordinal) - start;
                tag = data.Substring(start, length);
                currentTagIndex = start + length;
                if (tag != "")
                    tags.Add(tag);
            } while (tag != "");

            return tags;
        }

        private static Dictionary<string, string> FillPageSections(string data, List<string> innerTags, string divider)
        {
            Dictionary<string, string> dict = new();
            while (data.Contains(",,"))
                data = data.Replace(",,", ",");

            data = data.Replace("\n,", "\n");
            data = data.Replace(",\r", "\r");

            while (data.Contains("\r\n\r\n"))
                data = data.Replace("\r\n\r\n", "\r\n");

            foreach (var tag in innerTags)
            {
                int tagStart = data.IndexOf(divider + "," + tag, StringComparison.Ordinal);
                int start = data.IndexOf("\n", tagStart, StringComparison.Ordinal) + 1;
                int length = data.IndexOf("-->", start, StringComparison.Ordinal) - start - 2;
                string entry = data.Substring(start, length);
                // SaveObjectToResources(entry, tag);
                dict[tag] = entry;
            }

            return dict;
        }
        
        public static void SaveObjectToResources<T>(T instance, string name = default)
        {
#if UNITY_EDITOR
            string json = JsonConvert.SerializeObject(instance, Formatting.Indented);
            string path = "";
            string fileName = name ?? typeof(T).Name;
            path = $"Assets/Resources/GameData/{fileName}.txt";
            File.WriteAllText(path, json);
            Debug.Log("PATH = " + path);
#endif
        }
    }

    static class AsyncExtensions
    {
        public static TaskAwaiter GetAwaiter(this UnityEngine.AsyncOperation op)
        {
            var tcs = new TaskCompletionSource<object>();
            op.completed += _ => tcs.SetResult(null);
            return ((Task)tcs.Task).GetAwaiter();
        }
    }
}
#if ENABLE_IL2CPP
// Unity IL2CPP performance optimization attribute.
namespace Unity.IL2CPP.CompilerServices {
    enum Option {
        NullChecks = 1,
        ArrayBoundsChecks = 2
    }

    [AttributeUsage (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property, Inherited
 = false, AllowMultiple = true)]
    class Il2CppSetOptionAttribute : Attribute {
        public Option Option { get; private set; }
        public object Value { get; private set; }

        public Il2CppSetOptionAttribute (Option option, object value) { Option = option; Value = value; }
    }
}
#endif