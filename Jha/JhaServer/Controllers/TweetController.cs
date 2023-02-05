using JhaModels.Tweets;
using JhaRepository;
using JhaModels.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JhaServer.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class TweetController : BaseApiController
	{
		private readonly ITweetStatisticsRepository _tweetStatisticsRepository;

		
		
		public TweetController(
			ITweetStatisticsRepository tweetStatisticsRepository,
			ILogger<TweetController> logger)
			: base(logger)
		{
			this._tweetStatisticsRepository = tweetStatisticsRepository;
		}



		/// <summary>
		/// returns the <see cref="TweetStatisticsDTO"/> for the consumed tweets
		/// </summary>
		/// <returns></returns>
		[HttpGet("statistics")]
		public async Task<IActionResult> GetStatistics()
		{
			return await this.DoWithTryCatchAsync(async () =>
			{
				var statistics = await _tweetStatisticsRepository.GetStatistics();

				return statistics.ToDTO();
			});
		}//end GetStatistics
	}
}
