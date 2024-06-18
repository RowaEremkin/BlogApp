using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Rowa.Client.Api
{
    internal static class Request
    {
        #region Const

        private const bool debugBefore = true;
        private const bool debugAfter = true;
        private const string tokenKey = "authorization";
        private const string tokenPrefix = "Bearer ";
        private const string contentTypeJson = "application/json; charset=UTF-8";

        #endregion

        #region Get

        private static UnityWebRequest Get(string url, string token = null)
        {
            if (debugBefore) Debug.Log($"Get url: {url}");
            UnityWebRequest request = UnityWebRequest.Get(url);
            if (!string.IsNullOrEmpty(token)) request.SetRequestHeader(tokenKey, tokenPrefix + token);
            return request;
        }

        /// <summary>
        /// Get request with return json body
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="json">Post data</param>
        /// <param name="token">(optional) Authorization token</param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code, string - json body</param>
        internal static IEnumerator Get(string url, string token = null, Action<HttpStatusCode, string> onComplete = null)
        {
            UnityWebRequest request = Get(url, token);
            yield return request.SendWebRequest();

            if(debugAfter) Debug.Log($"Geted url: {url}\ncode:{request.responseCode}\nbody: {request.downloadHandler.text}");
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text);

            request.Dispose();
        }

        /// <summary>
        /// Get request with return json body and json header
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="json">Post data</param>
        /// <param name="token">(optional) Authorization token</param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code, string - json body</param>
        internal static IEnumerator Get(string url, string token = null, Action<HttpStatusCode, string, Dictionary<string, string>> onComplete = null)
        {
            UnityWebRequest request = Get(url, token);
            yield return request.SendWebRequest();

            if (debugAfter) Debug.Log($"Geted url: {url}\nbody: {request.downloadHandler.text}");
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text, request.GetResponseHeaders());

            request.Dispose();
        }

        #endregion

        #region Post

        private static UnityWebRequest Post(string url, string json, string token = null)
        {
            if (debugBefore) Debug.Log($"Post url: {url}\njson: {json}");
            UnityWebRequest request = UnityWebRequest.Post(url, json, contentTypeJson);
            if (!string.IsNullOrEmpty(token)) request.SetRequestHeader(tokenKey, tokenPrefix + token);
            return request;
        }

        /// <summary>
        /// Post request without json body
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="json">Post data</param>
        /// <param name="token">(optional) Authorization token</param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code</param>
        internal static IEnumerator Post(string url, string json, string token = null, Action<HttpStatusCode> onComplete = null)
        {
            return Post(url, json, token, (HttpStatusCode code, string downloadHandler) =>
            {
                onComplete?.Invoke(code);
            });
        }

        /// <summary>
        /// Post request with return json body
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="json">Post data</param>
        /// <param name="token">(optional) Authorization token</param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code, string - json body</param>
        internal static IEnumerator Post(string url, string json, string token = null, Action<HttpStatusCode, string> onComplete = null)
        {
            UnityWebRequest request = Post(url, json, token);
            yield return request.SendWebRequest();

            if (debugAfter) Debug.Log($"Posted url: {url}\njson: {json}\ncode: {request.responseCode}\nbody: {request.downloadHandler.text}");
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text);

            request.Dispose();
        }

        /// <summary>
        /// Post request with return json body and json header
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="json">Post data</param>
        /// <param name="token">(optional) Authorization token</param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code, string - json body, Dictionary<string, string> - json header</param>
        internal static IEnumerator Post(string url, string json, string token = null, Action<HttpStatusCode, string, Dictionary<string, string>> onComplete = null)
        {
            UnityWebRequest request = Post(url, json, token);
            yield return request.SendWebRequest();

            if (debugAfter) Debug.Log($"Posted url: {url}\njson: {json}\ncode: {request.responseCode}\nbody: {request.downloadHandler.text}\nheaders:\n{GetHeadersString(request.GetResponseHeaders())}");
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text, request.GetResponseHeaders());

            request.Dispose();
        }

        #endregion

        #region Put
        private static UnityWebRequest Put(string url, string json, string token = null)
        {
            if (debugBefore) Debug.Log($"Put url: {url}\njson: {json}");
            UnityWebRequest request = UnityWebRequest.Put(url, json);
            request.SetRequestHeader("Content-type", contentTypeJson);
            if (!string.IsNullOrEmpty(token)) request.SetRequestHeader(tokenKey, tokenPrefix + token);
            return request;
        }

        /// <summary>
        /// Put request without return json body
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="json">Put data</param>
        /// <param name="token">(optional) Authorization token</param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code</
        internal static IEnumerator Put(string url, string json, string token = null, Action<HttpStatusCode> onComplete = null)
        {
            return Put(url, json, token, (HttpStatusCode code, string downloadHandler) =>
            {
                onComplete?.Invoke(code);
            });
        }

        /// <summary>
        /// Put request with return json body
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="json">Put data</param>
        /// <param name="token">(optional) Authorization token</param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code, string - json body</
        internal static IEnumerator Put(string url, string json, string token = null, Action<HttpStatusCode, string> onComplete = null)
        {
            UnityWebRequest request = Put(url, json, token);
            yield return request.SendWebRequest();

            if (debugAfter) Debug.Log($"Put url: {url}\njson: {json}\ncode: {request.responseCode}\nbody: {request.downloadHandler.text}");
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text);

            request.Dispose();
        }

        /// <summary>
        /// Put request with return json body and json header
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="json">Put data</param>
        /// <param name="token">(optional) Authorization token</param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code, string - json body, Dictionary<string, string> - json header</
        internal static IEnumerator Put(string url, string json, string token = null, Action<HttpStatusCode, string, Dictionary<string, string>> onComplete = null)
        {
            UnityWebRequest request = Put(url, json, token);
            yield return request.SendWebRequest();

            if (debugAfter) Debug.Log($"Put url: {url}\njson: {json}\ncode: {request.responseCode}\nbody: {request.downloadHandler.text}\nheaders: {GetHeadersString(request.GetResponseHeaders())}");
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text, request.GetResponseHeaders());

            request.Dispose();
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete request
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="token">(optional) Authorization token</param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code</
        internal static IEnumerator Delete(string url, string token = null, Action<HttpStatusCode> onComplete = null)
        {
            if (debugBefore) Debug.Log($"Delete url: {url}");
            UnityWebRequest request = UnityWebRequest.Delete(url);
            if(!string.IsNullOrEmpty(token)) request.SetRequestHeader(tokenKey, tokenPrefix + token);
            yield return request.SendWebRequest();

            if (debugAfter) Debug.Log($"Delete url: {url}\ncode: {request.responseCode}");
            onComplete?.Invoke((HttpStatusCode)request.responseCode);

            request.Dispose();
        }

        #endregion

        #region Tools
        private static string GetHeadersString(Dictionary<string, string> keyValuePairs)
        {
            string headers = "";
            foreach (KeyValuePair<string, string> pair in keyValuePairs)
            {
                headers += $"[{pair.Key}] - {pair.Value}\n";
            }
            return headers;
        }
        #endregion
    }
}