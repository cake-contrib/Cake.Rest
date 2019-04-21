using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using RestSharp;
using RestSharp.Serialization;

namespace Cake.Rest
{
    /// <summary>
    /// Contains utilities for interaction with RestSharp
    /// </summary>
    public static class RestUtilities
    {
        private static readonly Dictionary<string, RestClient> Clients = new Dictionary<string, RestClient>();
        private static readonly IRestSerializer JsonSerializer = new JsonNetSerializer();

        /// <summary>
        /// Gets an instance of <seealso cref="IRestClient"/> from <see cref="Clients"/>, or instantiates a new one
        /// if not found in the dictionary. Then, configures it for buffered/non-buffered read and write, forces the use
        /// of Newtonsoft.Json as serializer, and returns the object.
        /// </summary>
        /// <param name="host">Left part of a URL or in other words, HTTP Server Authority in "http://myserver.com:8080" format.</param>
        /// <param name="bufferedRead">Whether the underlying <see cref="WebRequest"/> is allowed to buffer while reading the response or not.</param>
        /// <param name="bufferedWrite">Whether the underlying <see cref="WebRequest"/> is allowed to buffer while writing the request or not.</param>
        /// <exception cref="ArgumentException"><paramref name="host"/> is null, empty or made of whitespaces or is an invalid URI.</exception>
        /// <returns>An instance of <see cref="IRestClient"/> ready for making REST requests.</returns>
        public static IRestClient GetClientInstance(
            string host,
            bool bufferedRead = true,
            bool bufferedWrite = true)
        {   
            if(string.IsNullOrWhiteSpace(host))
                throw new ArgumentNullException(nameof(host));
            
            RestClient client;
            if (Clients.ContainsKey(host))
                client = Clients[host];
            else
            {
                if(!Uri.TryCreate(host, UriKind.Absolute, out _))
                    throw new ArgumentException("Invalid host.", nameof(host));
                
                client = new RestClient(host);
                Clients.Add(host, client);
            }

            // should disable buffering when dealing with blobs, as they can be very big
            // and we would run out of memory when WebRequest has its built-in buffering enabled.
            client.ConfigureWebRequest(x =>
            {
                x.AllowReadStreamBuffering = bufferedRead;
                x.AllowWriteStreamBuffering = bufferedWrite;
            });

            // force usage of json.net
            return client.UseSerializer(() => JsonSerializer);
        }

        /// <summary>
        /// Returns the left part (Authority) of URI given to it.
        /// </summary>
        /// <param name="endpoint">Endpoint URI</param>
        /// <exception cref="ArgumentNullException"><paramref name="endpoint"/> can't be null.</exception>
        /// <returns>Server Authority of <paramref name="endpoint"/></returns>
        public static string GetHost(Uri endpoint)
        {
            if(endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));
            
            // return endpoint.GetLeftPart(UriPartial.Authority);
            return endpoint.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped);
        }

        /// <summary>
        /// Returns MIME Type string for given <see cref="BodyType"/> (used for requests with a body of type "<see cref="string"/>").
        /// </summary>
        /// <param name="bodyType">Content Type</param>
        /// <returns>corresponding MIME type</returns>
        /// <exception cref="ArgumentOutOfRangeException">If value of <paramref name="bodyType"/> is not in values defined by <see cref="BodyType"/>.</exception>
        public static string GetBodyContentType(BodyType bodyType)
        {
            switch (bodyType)
            {
                case BodyType.Xml:
                    return "text/xml";
                case BodyType.Json:
                    return "application/json";
                case BodyType.PlainText:
                    return "text/plain";
                case BodyType.UrlEncodedForm:
                    return "application/x-www-form-urlencoded";
                default:
                    throw new ArgumentOutOfRangeException(nameof(bodyType), bodyType, null);
            }
        }

        /// <summary>
        /// Creates and configures an instance of <see cref="RestRequest"/>.
        /// </summary>
        /// <param name="method">HTTP Method</param>
        /// <param name="endpoint">API Endpoint URI</param>
        /// <param name="headers">HTTP Headers</param>
        /// <returns>Instance of <see cref="RestRequest"/> configured with given parameters.</returns>
        /// <exception cref="ArgumentNullException">Either <paramref name="method"/> or <paramref name="endpoint"/> are null, or <paramref name="method"/> is empty or made up of whitespaces.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Value given as <paramref name="method"/> is not defined in <see cref="Method"/>, thus not supported by RestSharp.</exception>
        public static IRestRequest GetRequest(string method, Uri endpoint, IDictionary<string, string> headers)
        {
            if (string.IsNullOrWhiteSpace(method))
                throw new ArgumentNullException(nameof(method));

            if (!Enum.TryParse(method, true, out Method methodParsed))
                throw new ArgumentOutOfRangeException(nameof(method), method,
                    "Method not supported by RestSharp");

            if (endpoint == null)
                throw new ArgumentNullException(nameof(endpoint));

            var request = new RestRequest(endpoint.PathAndQuery, methodParsed);

            if (headers != null)
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }

            return request;
        }

        /// <summary>
        /// Creates <see cref="FileParameter"/> instance for use with AddFile.
        /// </summary>
        /// <param name="fileName">Name of the file included in body, only for Content-Disposition header.</param>
        /// <param name="bodyStream">Stream of request body contents</param>
        /// <param name="contentType">MIME type of file included in body, use "application/octet-stream" if unsure.</param>
        /// <exception cref="ArgumentNullException"><paramref name="bodyStream"/> is null, or <paramref name="contentType"/> is null, empty or whitespace.</exception>
        /// <returns>Instance of <see cref="FileParameter"/> containing given parameters.</returns>
        public static FileParameter GetFileParam(
            string fileName, 
            Stream bodyStream, 
            string contentType)
        {
            if(bodyStream == null)
                throw new ArgumentNullException(nameof(bodyStream));
            
            if(string.IsNullOrWhiteSpace(contentType))
                throw new ArgumentNullException(nameof(contentType));
            
            return new FileParameter
            {
                Name = contentType,
                ContentType = contentType,
                ContentLength = bodyStream.Length,
                Writer = bodyStream.CopyTo,
                FileName = fileName
            };
        }
    }
}