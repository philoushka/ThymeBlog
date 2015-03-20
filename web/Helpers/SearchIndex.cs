using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thyme.Web.Models;

namespace Thyme.Web.Helpers
{
    public class BlogPostSearchIndex
    {

        SearchServiceClient serviceClient;

        public BlogPostSearchIndex(string searchServiceName, string apiKey)
        {
            serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
            Task.Run(() => CreateBlogPostsIndex()).Wait();
        }

        /// <summary>
        /// Delete the index at Azure
        /// </summary>
        public async Task NukeBlogPostsIndex()
        {
            if (await serviceClient.Indexes.ExistsAsync(Config.AzureSearchIndexName))
            {
                await serviceClient.Indexes.DeleteAsync(Config.AzureSearchIndexName);
            }
        }

        /// <summary>
        /// Set up the index at the Azure endpoint.
        /// </summary>
        public async Task CreateBlogPostsIndex()
        {
            if (await serviceClient.Indexes.ExistsAsync(Config.AzureSearchIndexName) == false)
            {
                var blogPostIndex = new Index
                {
                    Name = Config.AzureSearchIndexName,
                    Fields = IndexFields().ToList()
                };

                await serviceClient.Indexes.CreateAsync(blogPostIndex);
            }
        }

        /// <summary>
        /// The part of the item to be indexed. This includes the blogpost's id and the body
        /// </summary>
        /// <returns>Fields for each property in the index to create</returns>
        private IEnumerable<Field> IndexFields()
        {
            yield return new Field("Id", DataType.String) { IsKey = true, IsRetrievable = true };
            yield return new Field("BlogPostBody", DataType.String) { IsKey = false, IsSearchable = true, IsFilterable = true, IsSortable = true, IsRetrievable = true };
        }

        private SearchIndexClient BlogPostsClient()
        {
            return serviceClient.Indexes.GetClient(Config.AzureSearchIndexName);
        }

        /// <summary>
        /// Add blog posts to the index.
        /// </summary>
        /// <param name="posts">A collection of posts</param>
        /// <returns></returns>
        public async Task AddToIndex(params IndexBlogPost[] posts)
        {
            using (SearchIndexClient indexClient = BlogPostsClient())
            {
                try
                {
                    await indexClient.Documents.IndexAsync(IndexBatch.Create(posts.Select(doc => IndexAction.Create(doc))));
                }
                catch (Exception e)
                {
                    string indexErrMsg = e.Message;
                }
            }
        }

        /// <summary>
        /// Search the index 
        /// </summary>
        /// <param name="searchText">A string token to find within documents in the index. For no search token, send empty string.</param>
        /// <param name="oDataFilter">Optional. An OData $filter expression to apply to the search
        ///   query. (For syntax, see https://msdn.microsoft.com/en-us/library/azure/dn798921.aspx)
        /// </param>
        /// <returns>A collection of IndexBlogPost with their related scores and keyword highlights.</returns>
        public async Task<DocumentSearchResponse<IndexBlogPost>> SearchIndex(string searchText, string oDataFilter = null)
        {
            if (searchText.IsNullorEmpty())
            {
                searchText = "*";
            }
            var sp = new SearchParameters { Filter = oDataFilter };

            using (SearchIndexClient indexClient = BlogPostsClient())
            {
                return await indexClient.Documents.SearchAsync<IndexBlogPost>(searchText, sp);
            }
        }

        public async Task RemoveFromIndex(params string[] postIds)
        {
            using (SearchIndexClient indexClient = BlogPostsClient())
            {
                var docsToDelete = postIds.Select(p => new IndexAction(IndexActionType.Delete, new Document { { "Id", p } }));
                await indexClient.Documents.IndexAsync(IndexBatch.Create(docsToDelete));
            }
        }
    }
    public class IndexBlogPost
    {
        public string Id { get; set; }
        public string BlogPostBody { get; set; }
    }
}
