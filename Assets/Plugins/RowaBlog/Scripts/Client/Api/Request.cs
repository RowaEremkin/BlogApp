using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Rowa.Blog.Client.Api
{
    internal static class Request
    {
        #region Const

        private const bool debug = true;
        private const string tokenKey = "authorization";
        private const string tokenPrefix = "Bearer ";
        private const string contentTypeJson = "application/json; charset=UTF-8";

        #endregion
        #region Get

        internal static IEnumerator Get(string url, string token = null, Action<HttpStatusCode, string> onComplete = null)
        {
            if(debug) Debug.Log("Get url: " + url);
            UnityWebRequest request = UnityWebRequest.Get(url);
            if(!string.IsNullOrEmpty(token)) request.SetRequestHeader(tokenKey, tokenPrefix + token);
            
            yield return request.SendWebRequest();
            if(debug) Debug.Log("Geted url: " + url + " body: " + request.downloadHandler.text);
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text);

            request.Dispose();
        }

        #endregion

        #region Post

        internal static IEnumerator Post(string url, string json, string token = null, Action<HttpStatusCode> onComplete = null)
        {
            return Post(url, json, token, (HttpStatusCode code, string downloadHandler) =>
            {
                onComplete?.Invoke(code);
            });
        }
        
        internal static IEnumerator Post(string url, string json, string token = null, Action<HttpStatusCode, string> onComplete = null)
        {
            if(debug) Debug.Log("Post url: " + url + " json: " + json);
            UnityWebRequest request = UnityWebRequest.Post(url, json, contentTypeJson);
            if(!string.IsNullOrEmpty(token)) request.SetRequestHeader(tokenKey, tokenPrefix + token);

            yield return request.SendWebRequest();
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text);

            request.Dispose();
        }
        
        /// <summary>
        /// Post request with return json body and json header
        /// </summary>
        /// <param name="url">Path to request</param>
        /// <param name="json"></param>
        /// <param name="token"></param>
        /// <param name="onComplete">(optional) Operation completes callback with http result code, string - json body, string - json header</param>
        internal static IEnumerator Post(string url, string json, string token = null, Action<HttpStatusCode, string, Dictionary<string, string>> onComplete = null)
        {
            if(debug) Debug.Log("Post url: " + url + " json: " + json);
            UnityWebRequest request = UnityWebRequest.Post(url, json, contentTypeJson);
            if(!string.IsNullOrEmpty(token)) request.SetRequestHeader(tokenKey, tokenPrefix + token);

            yield return request.SendWebRequest();
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text, request.GetResponseHeaders());

            request.Dispose();
        }

        #endregion

        #region Put
        internal static IEnumerator Put(string url, string json, string token = null, Action<HttpStatusCode> onComplete = null)
        {
            return Put(url, json, token, (HttpStatusCode code, string downloadHandler) =>
            {
                onComplete?.Invoke(code);
            });
        }
        internal static IEnumerator Put(string url, string json, string token = null, Action<HttpStatusCode, string> onComplete = null)
        {
            if (debug) Debug.Log("Put url: " + url + " json: " + json);
            UnityWebRequest request = UnityWebRequest.Put(url, json);
            
            request.SetRequestHeader("Content-type", contentTypeJson);
            if(!string.IsNullOrEmpty(token)) request.SetRequestHeader(tokenKey, tokenPrefix + token);

            yield return request.SendWebRequest();
            onComplete?.Invoke((HttpStatusCode)request.responseCode, request.downloadHandler.text);

            request.Dispose();
        }

        #endregion

        #region Delete

        internal static IEnumerator Delete(string url, string token = null, Action<HttpStatusCode> onComplete = null)
        {
            if (debug) Debug.Log("Put url: " + url);
            UnityWebRequest request = UnityWebRequest.Delete(url);
            if(!string.IsNullOrEmpty(token)) request.SetRequestHeader(tokenKey, tokenPrefix + token);

            yield return request.SendWebRequest();
            onComplete?.Invoke((HttpStatusCode)request.responseCode);

            request.Dispose();
        }

        #endregion
    }
}