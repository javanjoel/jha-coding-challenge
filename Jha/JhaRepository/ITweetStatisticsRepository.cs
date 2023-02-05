using JhaModels.Tweets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaRepository
{
    /// <summary>
    /// contract to store and retrieve statistics on incoming twitter tweet stream
    /// </summary>
    public interface ITweetStatisticsRepository
	{
		/// <summary>
		/// consumes the tweet so that whatever values can be extracted and calculated in the <see cref="TweetStatistics"/>
		/// <see cref="GetStatistics"/>
		/// </summary>
		/// <param name="tweet"></param>
		/// <returns></returns>
		Task AddToStatistics(Tweet tweet);

		/// <summary>
		/// Gets the current statistics snapshot
		/// </summary>
		/// <returns></returns>
		Task<TweetStatistics> GetStatistics();
	}
}
