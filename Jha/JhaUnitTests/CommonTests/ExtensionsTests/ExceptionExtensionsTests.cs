using JhaCommon.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaUnitTests.CommonTests.ExtensionsTests
{
	[TestClass]
	public class ExceptionExtensionsTests
	{
		[TestMethod]
		public void Test_ExceptionExtensions_GetLogMessage()
		{
			var ex = new Exception("My Exception");

			var logMessage = ex.GetLogMessage("test");

			Assert.IsTrue(logMessage.Contains(ex.Message));

			var outerEx = new Exception("Outter log message", ex);

			logMessage = outerEx.GetLogMessage("test");

			Assert.IsTrue(logMessage.Contains(outerEx.Message));
			Assert.IsTrue(logMessage.Contains(ex.Message));
		}//end Test_ExceptionExtensions_GetLogMessage
	}
}
