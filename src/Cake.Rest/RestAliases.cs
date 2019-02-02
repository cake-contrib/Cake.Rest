using System;
using System.Collections.Generic;
using System.IO;
using Cake.Core;
using Cake.Core.Annotations;
using RestSharp;
using static Cake.Rest.RestUtilities;

namespace Cake.Rest
{
    /// <summary>
    /// REST Request String Body Content-Type
    /// </summary>
    public enum BodyType
    {
        /// <summary>
        /// Indicates that request body contains XML ("text/xml") content.
        /// </summary>
        Xml,

        /// <summary>
        /// Indicates that request body contains JSON ("application/json") content.
        /// </summary>
        Json,

        /// <summary>
        /// Indicates that request body is in Plaintext ("text/plain") format.
        /// </summary>
        PlainText,

        /// <summary>
        /// Indicates that request body is in Simple URL-Encoded Form ("application/x-www-form-urlencoded") format. 
        /// </summary>
        UrlEncodedForm
    }

    /// <summary>
    /// Aliases for simplifying usage of RestSharp in Cake
    /// </summary>
    [CakeAliasCategory("Rest")]
    public static class RestAliases
    {
        
        #region REST - empty body
        /// <summary>
        /// Send a REST request with empty body, and receive its response.
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="headers">HTTP headers, null for no headers</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            IDictionary<string, string> headers = null)
        {
            return Rest(context, method, endpoint, headers, BodyType.PlainText, null);
        }

        /// <summary>
        /// Sends a REST request with empty body, and receives its response. 
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="headers">HTTP headers, null for no headers</param>
        /// <param name="responseWriter">Lambda function for reading response and writing to desired place.</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            IDictionary<string, string> headers,
            Action<Stream> responseWriter)
        {
            return Rest(context, method, endpoint, headers, BodyType.PlainText, null, responseWriter);
        }

        /// <summary>
        /// Sends a REST request with empty body, and receives its response. 
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="responseWriter">Lambda function for reading response and writing to desired place.</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            Action<Stream> responseWriter)
        {
            return Rest(context, method, endpoint, null, BodyType.PlainText, null, responseWriter);
        }

        /// <summary>
        /// Sends a REST request with empty body, and receives its response. 
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="headers">HTTP headers, null for no headers</param>
        /// <param name="advancedResponseWriter">Lambda function for reading response and writing to desired place.</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            IDictionary<string, string> headers,
            Action<Stream, IHttpResponse> advancedResponseWriter)
        {
            return Rest(context, method, endpoint, headers, BodyType.PlainText, null, advancedResponseWriter);
        }
        
        /// <summary>
        /// Sends a REST request with empty body, and receives its response. 
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="advancedResponseWriter">Lambda function for reading response and writing to desired place.</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            Action<Stream, IHttpResponse> advancedResponseWriter)
        {
            return Rest(context, method, endpoint, null, BodyType.PlainText, null, advancedResponseWriter);
        }
        #endregion
        
        #region REST - with body - string response
        /// <summary>
        /// Sends a REST request and receives its response. 
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="headers">HTTP headers, null for no headers</param>
        /// <param name="bodyType">Content type of request body</param>
        /// <param name="body">Contents of request body</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            IDictionary<string, string> headers,
            BodyType bodyType,
            string body)
        {
            var request = GetRequest(method, endpoint, headers);

            if (body != null)
                request.AddParameter(GetBodyContentType(bodyType), body, ParameterType.RequestBody);

            return GetClientInstance(GetHost(endpoint)).Execute(request);
        }


        /// <summary>
        /// Sends a REST request and receives its response.  
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="headers">HTTP headers, null for no headers</param>
        /// <param name="contentType">MIME type of file included in body, use "application/octet-stream" if unsure.</param>
        /// <param name="fileName">Name of the file included in body, only for Content-Disposition header.</param>
        /// <param name="bodyStream">Stream of request body contents</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.IO")]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            IDictionary<string, string> headers,
            string contentType,
            string fileName,
            Stream bodyStream)
        {
            var request = GetRequest(method, endpoint, headers);

            if (bodyStream != null)
                request.Files.Add(GetFileParam(fileName, bodyStream, contentType));

            return GetClientInstance(GetHost(endpoint), true, false).Execute(request);
        }
        #endregion
        
        #region REST - with body - simple response handling
        /// <summary>
        /// Sends a REST request and receives its response.  
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="headers">HTTP headers, null for no headers</param>
        /// <param name="bodyType">Content type of request body</param>
        /// <param name="body">Contents of request body</param>
        /// <param name="responseWriter">Lambda function for reading response and writing to desired place.</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.IO")]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            IDictionary<string, string> headers,
            BodyType bodyType,
            string body,
            Action<Stream> responseWriter)
        {
            var request = GetRequest(method, endpoint, headers);

            if (body != null)
                request.AddParameter(GetBodyContentType(bodyType), body, ParameterType.RequestBody);

            request.ResponseWriter = responseWriter;

            return GetClientInstance(GetHost(endpoint), false).Execute(request);
        }

        /// <summary>
        /// Sends a REST request and receives its response.  
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="headers">HTTP headers, null for no headers</param>
        /// <param name="contentType">MIME type of file included in body, use "application/octet-stream" if unsure.</param>
        /// <param name="fileName">Name of the file included in body, only for Content-Disposition header.</param>
        /// <param name="bodyStream">Stream of request body contents</param>
        /// <param name="responseWriter">Lambda function for reading response and writing to desired place.</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.IO")]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            IDictionary<string, string> headers,
            string contentType,
            string fileName,
            Stream bodyStream,
            Action<Stream> responseWriter)
        {
            var request = GetRequest(method, endpoint, headers);

            if (bodyStream != null)
                request.Files.Add(GetFileParam(fileName, bodyStream, contentType));

            request.ResponseWriter = responseWriter;

            return GetClientInstance(GetHost(endpoint), false, false).Execute(request);
        }
        #endregion

        #region REST - with body - advanced response handling

        /// <summary>
        /// Sends a REST request and receives its response.  
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="headers">HTTP headers, null for no headers</param>
        /// <param name="bodyType">Content type of request body</param>
        /// <param name="body">Contents of request body</param>
        /// <param name="advancedResponseWriter">Lambda function for reading response and writing to desired place.</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.IO")]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            IDictionary<string, string> headers,
            BodyType bodyType,
            string body,
            Action<Stream, IHttpResponse> advancedResponseWriter)
        {
            var request = GetRequest(method, endpoint, headers);

            if (body != null)
                request.AddParameter(GetBodyContentType(bodyType), body, ParameterType.RequestBody);

            request.AdvancedResponseWriter = advancedResponseWriter;

            return GetClientInstance(GetHost(endpoint), false).Execute(request);
        }

        /// <summary>
        /// Sends a REST request and receives its response.  
        /// </summary>
        /// <param name="context">Cake build context</param>
        /// <param name="method">HTTP method</param>
        /// <param name="endpoint">Endpoint local address, starting with /</param>
        /// <param name="headers">HTTP headers, null for no headers</param>
        /// <param name="contentType">MIME type of file included in body, use "application/octet-stream" if unsure.</param>
        /// <param name="fileName">Name of the file included in body, only for Content-Disposition header.</param>
        /// <param name="bodyStream">Stream of request body contents</param>
        /// <param name="advancedResponseWriter">Lambda function for reading response and writing to desired place.</param>
        /// <returns>REST response</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("System.IO")]
        [CakeNamespaceImport("System.Collections.Generic")]
        [CakeNamespaceImport("RestSharp")]
        public static IRestResponse Rest(
            this ICakeContext context,
            string method,
            Uri endpoint,
            IDictionary<string, string> headers,
            string contentType,
            string fileName,
            Stream bodyStream,
            Action<Stream, IHttpResponse> advancedResponseWriter)
        {
            var request = GetRequest(method, endpoint, headers);

            if (bodyStream != null)
                request.Files.Add(GetFileParam(fileName, bodyStream, contentType));

            request.AdvancedResponseWriter = advancedResponseWriter;

            return GetClientInstance(GetHost(endpoint), false, false).Execute(request);
        }
        #endregion

    }
}