using JhaModels.Tweets;
using JhaRepository;
using JhaServer.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaUnitTests.ServerTests.ControllersTests
{
	[TestClass]
	public class TweetControllerTests
	{
		[TestMethod]
		public async Task Test_TweetController_GetStatistics()
		{
			var mockRepo = new Mock<ITweetStatisticsRepository>();
			var mockLogger = new Mock<ILogger<TweetController>>();

			mockRepo.Setup(r => r.GetStatistics()).ReturnsAsync(new TweetStatistics());

			var controller = new TweetController(mockRepo.Object, mockLogger.Object);

			var result = await controller.GetStatistics();

			Assert.IsInstanceOfType(result, typeof(OkObjectResult));
		}//end Test_TweetController_GetStatistics
	}
}
