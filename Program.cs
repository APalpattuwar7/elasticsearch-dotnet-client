using System;
using Elastic.Transport;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var settings = new ElasticsearchClientSettings(new Uri("https://localhost:9200"))
    .CertificateFingerprint("<FINGERPRINT>")
    .Authentication(new BasicAuthentication("elastic", "51Vdb+1*xq71T6x3Glip"));

var client = new ElasticsearchClient(settings);

var tweet = new Tweet 
{
    Id = 1,
    User = "stevejgordon",
    PostDate = new DateTime(2009, 11, 15),
    Message = "Trying out the client, so far so good?"
};

var response = await client.IndexAsync(tweet, request => request.Index("my-tweet-index")); 

// if (response.IsValid) 
// {
//      Console.WriteLine($"Index document with ID {response.Id} succeeded."); 
// }

var tweetResponse = await client.GetAsync<Tweet>(1, idx => idx.Index("my-tweet-index")); 
var tweetResult = tweetResponse.Source; 

tweet.Message = "This is a new message"; 

var updateResponse = await client.UpdateAsync<Tweet, object>("my-tweet-index", 1, u => u
        .Doc(tweet)); 

// if (response.IsValid)
// {
//     Console.WriteLine("Update document succeeded.");
// }

app.Run();
