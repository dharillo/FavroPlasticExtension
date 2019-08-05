﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using FavroPlasticExtension.Favro.API;

namespace FavroPlasticExtensionTests.Helpers
{
    /// <summary>
    /// Helper class to create response objects for the test suites
    /// </summary>
    public class FakeResponseFactory
    {
        private readonly Assembly runningAssembly;
        private readonly string baseNamespace;
        private readonly Dictionary<string, string> cache;

        public FakeResponseFactory()
        {
            runningAssembly = Assembly.GetExecutingAssembly();
            baseNamespace = "FavroPlasticExtensionTests";
            cache = new Dictionary<string, string>();
        }

        /// <summary>
        /// Creates a response which content field is the resource file indicated text
        /// </summary>
        /// <param name="resourcePath">Path inside this project to the resource. Example "Responses.users_page1.json"</param>
        /// <returns>Response object with empty headers, no error and content established to the resource text</returns>
        public Response GetResponseFromFile(string resourcePath)
        {
            string resourceText = GetResourceContent(resourcePath);
            return new Response
            {
                Content = resourceText
            };
        }

        private string GetResourceContent(string resourcePath)
        {
            return cache.ContainsKey(resourcePath) ? cache[resourcePath] : LoadResourceText(resourcePath);
        }

        private string LoadResourceText(string resourcePath)
        {
            string result = null;
            using (Stream stream = runningAssembly.GetManifestResourceStream($"{baseNamespace}.{resourcePath}"))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            cache[resourcePath] = result;
            return result;
        }

        /// <summary>
        /// Creates a response instance with the error given
        /// </summary>
        /// <param name="error">Error to store in the response instance created</param>
        /// <returns>Response created with empty string as content and error equal to the given exception</returns>
        public Response GetErrorResponse(Exception error)
        {
            return new Response
            {
                Content = string.Empty,
                Error = error
            };
        }
    }
}