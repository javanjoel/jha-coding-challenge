using JhaModels.DTO;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaModels.Tweets
{
    /// <summary>
    /// Gets a snapshot of statistics for <see cref="Tweet"/>s that were consumed
    /// </summary>
    public class TweetStatistics
    {
		#region consts

		/// <summary>
		/// the total count of items that should be set in <see cref="TweetStatisticsDTO.TopHashtags"/> and <see cref="TweetStatisticsDTO.TopMentions"/>
		/// </summary>
		public const int TOP_ITEMS_COUNT = 10;

		#endregion consts



		#region fields

		int _totalTweets = 0;

        ConcurrentDictionary<string, int> _topHashtags = new ConcurrentDictionary<string, int>();
        ConcurrentDictionary<string, int> _topMentions = new ConcurrentDictionary<string, int>();

		#endregion fields



		#region Properties

		public int TotalTweets => _totalTweets;

        /// <summary>
        /// the hashtags used across all tweets and their total counts of times used
        /// </summary>
        public ConcurrentDictionary<string, int> Hashtags { get; } = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// The mentions used across all tweets and their total counts of times used
        /// </summary>
        public ConcurrentDictionary<string, int> Mentions { get; } = new ConcurrentDictionary<string, int>();

        #endregion Properties



        #region Methods

        /// <summary>
        /// uses the tweet to increment all statistics for this instance
        /// </summary>
        /// <param name="tweet"></param>
        public void Increment(Tweet tweet)
        {
            //increment the total number of tweets, thread safe interlocked
            Interlocked.Increment(ref _totalTweets);

            //increment all the counts of hashtags
            foreach (var hashTag in tweet.Entities?.Hashtags ?? Enumerable.Empty<TweetEntityHashtag>())
            {
                int newCount = Hashtags.AddOrUpdate(hashTag.Tag, 1, (key, val) => val + 1);

                UpdateTopDictionary(_topHashtags, hashTag.Tag, newCount);
			}

            //increment all the counts of mentions
            foreach (var mention in tweet.Entities?.Mentions ?? Enumerable.Empty<TweetEntityMention>())
            {
                int newCount = Mentions.AddOrUpdate(mention.Username, 1, (key, val) => val + 1);

                UpdateTopDictionary(_topMentions, mention.Username, newCount);
			}
        }//end Increment


        public TweetStatisticsDTO ToDTO()
        {
            return new TweetStatisticsDTO()
            {
                TotalTweets = this.TotalTweets,
                TopHashtags = _topHashtags,
                TopMentions = _topMentions
            };
		}//end ToDTO


        private void UpdateTopDictionary(ConcurrentDictionary<string, int> _dictionaryRef, string newItem, int itemCount)
        {
			if (_dictionaryRef.Count < TOP_ITEMS_COUNT || _dictionaryRef.ContainsKey(newItem))
			{
				//just add this item as the dictionary doesn't even contain the top count
				_dictionaryRef.AddOrUpdate(newItem, itemCount, (k, v) => itemCount);
			}
			else //the dictionary is filled and doesn't contain this key, let's try and add this item
			{
				var itemWithLessCount = _dictionaryRef.FirstOrDefault(kvp => kvp.Value < itemCount);

				if (!string.IsNullOrEmpty(itemWithLessCount.Key))
				{
					//remove the old item
					_dictionaryRef.Remove(itemWithLessCount.Key, out int _);
					//then add the new item 
					_dictionaryRef.TryAdd(newItem, itemCount);
				}
			}
		}

		#endregion Methods
	}
}
