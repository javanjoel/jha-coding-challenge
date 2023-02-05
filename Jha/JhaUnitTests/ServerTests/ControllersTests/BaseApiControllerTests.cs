using JhaServer.Controllers;
using JhaUnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaUnitTests.ServerTests.ControllersTests
{
	[TestClass]
	public class BaseApiControllerTests
	{
		[TestMethod]
		public async Task Test_BaseApiController_DoWithTryCatchAsync()
		{
			var mockLogger = new MockInMemoryLogger<DerivedControllerForTesting>();
			var controller = new DerivedControllerForTesting(mockLogger);

			await controller.DoOkAction();

			Assert.AreEqual(1, mockLogger.GetLogs(LogLevel.Trace).Count());
			
			await controller.DoBadAction();

			Assert.AreEqual(2, mockLogger.GetLogs(LogLevel.Trace).Count());
			Assert.AreEqual(1, mockLogger.GetLogs(LogLevel.Error).Count());
		}//end  Test_BaseApiController_DoWithTryCatchAsync




		class DerivedControllerForTesting: BaseApiController
		{
			public DerivedControllerForTesting(ILogger logger)
				: base(logger)
			{
			}


			public async Task<IActionResult> DoOkAction() 
				=> await this.DoWithTryCatchAsync(async () => "ok");


			public async Task<IActionResult> DoBadAction()
				=> await this.DoWithTryCatchAsync(async () => throw new Exception("You did something wrong and you should'na did it!"));
		}

	}
}
