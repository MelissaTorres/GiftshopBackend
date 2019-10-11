using System;
using System.Linq;
using SS.GiftShop.Core;

namespace SS.GiftShop.Infrastructure
{
    public sealed class DomainUrlService : IUrlService
    {
        private const char PathSeparator = '/';

        public Uri DomainUri { get; }

        public Uri ClientDomainUri { get; }

        public string BasePath { get; }

        public DomainUrlService(Uri domainUri, string basePath, Uri clientDomainUri)
        {
            DomainUri = domainUri ?? throw new ArgumentNullException(nameof(domainUri));
            if (!domainUri.IsAbsoluteUri)
            {
                throw new ArgumentException("Domain URL is not an absolute URL", nameof(domainUri));
            }
            ClientDomainUri = clientDomainUri ?? throw new ArgumentNullException(nameof(clientDomainUri));
            if (!clientDomainUri.IsAbsoluteUri)
            {
                throw new ArgumentException("Client domain URL is not an absolute URL", nameof(domainUri));
            }

            if (!string.IsNullOrEmpty(basePath))
            {
                BasePath = GetBasePath(basePath);
            }
        }

        public string GetUri(params string[] paths)
        {
            if (paths is null)
            {
                throw new ArgumentNullException(nameof(paths));
            }

            if (paths.Length == 0)
            {
                return DomainUri.ToString();
            }

            var path = paths[0];

            if (BasePath != null)
            {
                path = JoinPaths(BasePath, path);
            }

            foreach (var additional in paths.Skip(1))
            {
                path = JoinPaths(path, additional);
            }

            return new Uri(DomainUri, path).ToString();
        }

        public string GetSurveyUri(Guid surveyId, Guid employeeId)
        {
            return new Uri(ClientDomainUri, $"/survey/{surveyId}/fill/{employeeId}").ToString();
        }

        private static string JoinPaths(string path1, string path2)
        {
            if (path2.StartsWith(PathSeparator))
            {
                // Already a root path
                // TODO: Show throw?
                return path2;
            }

            if (!path1.EndsWith(PathSeparator))
            {
                path1 += PathSeparator;
            }

            return path1 + path2;
        }

        /// <summary>
        /// Ensures the base path starts and ends with a path separator.
        /// </summary>
        /// <param name="basePath">The provided base path.</param>
        /// <returns>The base path in the form &quot;/path/&quot;.</returns>
        private static string GetBasePath(string basePath)
        {
            if (!basePath.StartsWith(PathSeparator))
            {
                basePath = PathSeparator + basePath;
            }

            if (!basePath.EndsWith(PathSeparator))
            {
                basePath += PathSeparator;
            }

            return basePath;
        }
    }
}
