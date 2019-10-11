using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using SS.GiftShop.Core;

namespace SS.GiftShop.Infrastructure
{
    public sealed class S3FileSystem : IFileSystem
    {
        private readonly IAmazonS3 _client;
        private readonly string _bucket;
        private readonly S3CannedACL _acl;

        public S3FileSystem(IAmazonS3 client, string bucket, S3CannedACL acl)
        {
            _client = client;
            _bucket = bucket;
            _acl = acl;
        }

        public Task Copy(string source, string destination, IDictionary<string, string> metadata = null)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            var response = await _client.DeleteObjectAsync(_bucket, path);
            return IsSuccessful(response.HttpStatusCode);
        }

        public async Task<bool> Exists(string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            var metadata = await _client.GetObjectMetadataAsync(_bucket, path);
            return IsSuccessful(metadata.HttpStatusCode);
        }

        public async Task<bool> Read(Stream outputStream, string path)
        {
            if (outputStream is null)
            {
                throw new ArgumentNullException(nameof(outputStream));
            }

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            var response = await _client.GetObjectAsync(_bucket, path);

            if (IsSuccessful(response.HttpStatusCode))
            {
                using (response.ResponseStream)
                {
                    await response.ResponseStream.CopyToAsync(outputStream);
                    outputStream.Position = 0;
                }

                return true;
            }

            return false;
        }

        public async Task Save(Stream contentStream, string path, IDictionary<string, string> metadata = null)
        {
            if (contentStream is null)
            {
                throw new ArgumentNullException(nameof(contentStream));
            }

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }

            var request = new PutObjectRequest
            {
                BucketName = _bucket,
                Key = path,
                InputStream = contentStream,
                CannedACL = _acl
            };

            if (metadata != null)
            {
                foreach (var kvp in metadata)
                {
                    request.Metadata.Add(kvp.Key, kvp.Value);
                }
            }

            var response = await _client.PutObjectAsync(request);
            if (!IsSuccessful(response.HttpStatusCode))
            {
                // TODO: Throw error
            }
        }

        private static bool IsSuccessful(HttpStatusCode status)
        {
            // TODO: Validate this
            return HttpStatusCode.OK <= status && status < HttpStatusCode.Ambiguous;
        }
    }
}
