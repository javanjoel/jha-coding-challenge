using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaModels.DTO
{
	public class TweetStatisticsDTO
	{
		public int TotalTweets { get; set; }

		public IDictionary<string, int> TopHashtags { get; set; }
		
		public IDictionary<string, int> TopMentions { get; set; }
	}
}
