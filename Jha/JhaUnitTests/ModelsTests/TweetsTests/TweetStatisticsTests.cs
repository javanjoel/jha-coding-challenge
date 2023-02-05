using JhaModels.Tweets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaUnitTests.ModelsTests.TweetsTests
{
	[TestClass]
	public class TweetStatisticsTests
	{
		string[] _defaultHashtags = new string[] { "jha", "and", "javan", "joel", "is", "awesome", "together" };
		string[] _defaultMentions = new string[] { "john", "jane", "joe", "bob" };

		

		[TestMethod]
		public void Test_TweetStatistics_Increment()
		{
			var ts = new TweetStatistics();

			ts.Increment(TestUtility.CreateTweet());

			Assert.AreEqual(1, ts.TotalTweets);
			
			ts.Increment(TestUtility.CreateTweet(_defaultHashtags, _defaultMentions));

			Assert.AreEqual(2, ts.TotalTweets);
			Assert.AreEqual(_defaultMentions.Length, ts.Mentions.Count);
			Assert.AreEqual(_defaultHashtags.Length, ts.Hashtags.Count);

			ts.Increment(TestUtility.CreateTweet(_defaultHashtags, _defaultMentions));

			Assert.AreEqual(3, ts.TotalTweets);
			//should still be the same since the list of hashtags and mentions are the same
			Assert.AreEqual(_defaultMentions.Length, ts.Mentions.Count);
			Assert.AreEqual(_defaultHashtags.Length, ts.Hashtags.Count);

			//add a new one in this tweet
			ts.Increment(TestUtility.CreateTweet(new string[] { "new" }, new string[] { "new" }));

			Assert.AreEqual(4, ts.TotalTweets);
			//now will be incremented
			Assert.AreEqual(_defaultMentions.Length + 1, ts.Mentions.Count);
			Assert.AreEqual(_defaultHashtags.Length + 1, ts.Hashtags.Count);
		}//end Test_TweetStatistics_Increment


		[TestMethod]
		public void Test_TweetStatistics_ToDTO()
		{
			var ts = new TweetStatistics();

			ts.Increment(TestUtility.CreateTweet());
			ts.Increment(TestUtility.CreateTweet(_defaultHashtags, _defaultMentions));
			ts.Increment(TestUtility.CreateTweet(_defaultHashtags, _defaultMentions));
			ts.Increment(TestUtility.CreateTweet(_defaultHashtags, _defaultMentions));

			var dto = ts.ToDTO();

			Assert.AreEqual(_defaultMentions.Length, dto.TopMentions.Count);
			Assert.AreEqual(_defaultHashtags.Length, dto.TopHashtags.Count);
			//should be a count of 3 in each
			Assert.AreEqual(3, dto.TopMentions.First().Value);
			Assert.AreEqual(3, dto.TopHashtags.First().Value);

			//add new items to the list
			var newHashtags = new string[TweetStatistics.TOP_ITEMS_COUNT];
			var newMentions = new string[TweetStatistics.TOP_ITEMS_COUNT];

			for (int i = 0; i < TweetStatistics.TOP_ITEMS_COUNT; i++)
			{
				newHashtags[i] = $"hastag{i}";
				newMentions[i] = $"mention{i}";
			}

			ts.Increment(TestUtility.CreateTweet(newHashtags, newMentions));
			
			dto = ts.ToDTO();

			//even though we add more than the max, still should only be the max of items in the list
			Assert.AreEqual(TweetStatistics.TOP_ITEMS_COUNT, dto.TopMentions.Count);
			Assert.AreEqual(TweetStatistics.TOP_ITEMS_COUNT, dto.TopHashtags.Count);

			//all the original items should be in the list
			Assert.IsTrue(_defaultMentions.All(m => dto.TopMentions.ContainsKey(m)));
			Assert.IsTrue(_defaultHashtags.All(m => dto.TopHashtags.ContainsKey(m)));

			//add the new items to the list multiple times so that they override all older items
			ts.Increment(TestUtility.CreateTweet(newHashtags, newMentions));
			ts.Increment(TestUtility.CreateTweet(newHashtags, newMentions));
			ts.Increment(TestUtility.CreateTweet(newHashtags, newMentions));

			//now all the top items should only be the NEW hashtags and mentions
			dto = ts.ToDTO();

			Assert.IsTrue(newMentions.All(m => dto.TopMentions.ContainsKey(m)));
			Assert.IsTrue(newHashtags.All(m => dto.TopHashtags.ContainsKey(m)));
		}//end Test_TweetStatistics_ToDTO
	}
}
