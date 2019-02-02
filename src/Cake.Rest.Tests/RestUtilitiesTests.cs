using System;
using System.IO;
using FluentAssertions;
using RestSharp;
using Xunit;

namespace Cake.Rest.Tests
{
    public sealed class RestUtilitiesTests
    {
        public const string ValidEndpoint = "http://myhost.com:9060/api";
        public const string ValidHost = "http://myhost.com:9060";
        public const string ValidMethod = "GET";
        public const string ValidFilename = "filename.zip";
        public const string ValidContentType = "application/octet-stream";
        
        [Fact]
        public void GetHost_ShouldThrowIfNullGiven()
        {
            // Given
            Uri endpoint = null;
            
            // When
            var exception = Record.Exception(() => RestUtilities.GetHost(endpoint));

            // Then
            exception.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void GetHost_ParseTestShouldSucceed()
        {
            // Given
            var endpoint = new Uri(ValidEndpoint);

            // When
            var host = RestUtilities.GetHost(endpoint);

            // Then
            host.Should().BeOneOf(ValidHost, ValidHost + "/");
        }

        [Fact]
        public void GetBodyContentType_OutOfRangeValue()
        {
            // Given
            BodyType value = (BodyType)int.MaxValue;
            
            // When
            var exception = Record.Exception(() => RestUtilities.GetBodyContentType(value));
            
            // Then
            exception.Should().BeOfType<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GetRequest_ShouldThrowWhenUnsupportedMethodGiven()
        {
            // Given
            string method = "SAYHELLO";
            
            // When
            var exception = Record.Exception(() => RestUtilities.GetRequest(method, new Uri(ValidEndpoint), null));

            // Then
            exception.Should().BeOfType<ArgumentOutOfRangeException>().Subject.ParamName.Should().Be("method");
        }
        
        
        [Fact]
        public void GetRequest_ShouldSucceed()
        {
            // Given
            // All valid constants
            var endpoint = new Uri(ValidEndpoint);
            // RestSharp (at least for now) doesn't parse query string of resource.
            var resource = endpoint.PathAndQuery;
            
            // When
            var result = RestUtilities.GetRequest(ValidMethod, endpoint, null);

            // Then
            result.Should().BeAssignableTo<IRestRequest>().
                Subject.Resource.Should().Be(resource);
        }

        
        [Fact]
        public void GetRequest_ShouldThrowWhenMethodIsNull()
        {
            // Given
            string method = null;
            
            // When
            var exception = Record.Exception(() => RestUtilities.GetRequest(method, new Uri(ValidEndpoint), null));

            // Then
            exception.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("method");
        }
        
        [Fact]
        public void GetRequest_ShouldThrowWhenEndpointNull()
        {
            // Given
            Uri endpoint = null;
            
            // When
            var exception = Record.Exception(() => RestUtilities.GetRequest(ValidMethod, endpoint, null));

            // Then
            exception.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("endpoint");
        }

        [Fact]
        public void GetFileParam_ShouldThrowIfBodyIsNull()
        {
            // Given
            Stream bodyStream = null;
            
            // When
            var exception = Record.Exception(() => RestUtilities.GetFileParam(ValidFilename, bodyStream, ValidContentType));
            
            // Then
            exception.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("bodyStream");
        }
        
        [Fact]
        public void GetFileParam_ShouldThrowIfContentTypeNull()
        {
            using (var stream = new MemoryStream())
            {
                // Given
                string contentType = null;
                
                // When
                var exception = Record.Exception(() => RestUtilities.GetFileParam(ValidFilename, stream, contentType));

                // Then
                exception.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("contentType");
            }
        }

        [Fact]
        public void GetClientInstance_ShouldThrowIfHostIsNull()
        {
         
            // Given
            string host = null;
        
            // When
            var exception = Record.Exception(() => RestUtilities.GetClientInstance(host));

            // Then
            exception.Should().BeOfType<ArgumentNullException>().Subject.ParamName.Should().Be("host");
        }
        
        
        [Fact]
        public void GetClientInstance_ShouldThrowIfHostIsInvalid()
        {
            // Given
            string host = "??????? Where Are My Invalid Values ???????";
        
            // When
            var exception = Record.Exception(() => RestUtilities.GetClientInstance(host));

            // Then
            exception.Should().BeOfType<ArgumentException>().Subject.ParamName.Should().Be("host");
        }

        [Fact]
        public void GetClientInstance_ShouldSucceed()
        {
            // Given
            // The valid host
            
            // When
            var exception = Record.Exception(() => RestUtilities.GetClientInstance(ValidHost));
            
            // Then
            exception.Should().Be(null);
        }
    }
}