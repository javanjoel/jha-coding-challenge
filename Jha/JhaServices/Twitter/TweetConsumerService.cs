using JhaCommon.Extensions;
using JhaModels.Tweets;
using JhaRepository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JhaServices.Twitter
{
    /// <summary>
    /// this simulates a service that will consume the incoming tweets and store them in a repository
    /// </summary>
    public class TweetConsumerService
	{
		#region fields

		private readonly ITweetStatisticsRepository _repository;
		private readonly ILogger<TweetConsumerService> _logger;
		private readonly string _apiBearerToken;
		private readonly JsonSerializerOptions _deserializeOpts = new JsonSerializerOptions()
		{
			PropertyNameCaseInsensitive = true
		};

		#endregion fields



		#region ctor

		public TweetConsumerService(
			string apiBearerToken,
			ITweetStatisticsRepository repository,
			ILogger<TweetConsumerService> logger)
		{
			this._repository = repository;
			this._logger = logger;
			this._apiBearerToken = apiBearerToken;
		}

		#endregion ctor


		
		public async Task Run()
		{
			_logger.LogTrace($"{nameof(TweetConsumerService)}.{nameof(Run)}");

			var client = new HttpClient();

			WebRequest request = WebRequest.Create("https://api.twitter.com/2/tweets/sample/stream?tweet.fields=entities");

			request.Headers.Add(HttpRequestHeader.Authorization, $"bearer {_apiBearerToken}");

			request.BeginGetResponse(ar =>
			{
				var req = (WebRequest)ar.AsyncState;
				var response = req.EndGetResponse(ar);
				var reader = new StreamReader(response.GetResponseStream());

				//start a long running task to run in the background
				_ = Task.Factory.StartNew(ReadStreamConstantly, reader, TaskCreationOptions.LongRunning);
			}, request);
		}//end Run

		
		private async Task ReadStreamConstantly(object? streamReaderObj)
		{
			var reader = streamReaderObj as StreamReader;
			string? jsonLine = null;
			Tweet currentTweet = null;

			while (!reader.EndOfStream)
			{
				try
				{
					jsonLine = await reader.ReadLineAsync();

					//sometimes the line may be empty... this is not an error, just arbitrarily happens
					if (string.IsNullOrWhiteSpace(jsonLine))
						continue;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.GetLogMessage(nameof(TweetConsumerService), "Error reading from the stream"));

					continue;
				}

				try 
				{
					currentTweet = JsonSerializer.Deserialize<TwitterTweetResponse>(jsonLine, _deserializeOpts).Data;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.GetLogMessage(nameof(TweetConsumerService), "Error parsing json to tweet object"));

					continue;
				}

				try
				{
					await _repository.AddToStatistics(currentTweet);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.GetLogMessage(nameof(TweetConsumerService), "Error adding to twitter repository"));

					continue;
				}
			}

			//if code got here, something happened with the stream.... 
			//...log the issue
			_logger.LogWarning($"{nameof(TweetConsumerService)} lost its connection to twitter stream");
			//... and run again
			Run();
		}//end ReadStreamConstantly
	}
}
