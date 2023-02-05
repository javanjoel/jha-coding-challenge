using JhaModels.Tweets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaRepository
{
    /// <summary>
    /// the concrete implementation of the ITweetStatisticsRepository interface to hold the statistics in memory.
    /// NOTE: Not a valid prouction-ready way of consuming and storing this data
    /// </summary>
    public class InMemoryTweetStatisticsRepository : ITweetStatisticsRepository
	{
		/// <summary>
		/// singleton instance
		/// </summary>
		public static readonly TweetStatistics Instance = new TweetStatistics();

		
		
		public async Task AddToStatistics(Tweet tweet)
		{
			Instance.Increment(tweet);
		}//end AddToStatistics


		public async Task<TweetStatistics> GetStatistics() => Instance;
	}
}
