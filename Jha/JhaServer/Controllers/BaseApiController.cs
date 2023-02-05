using JhaCommon.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text;
using SysComDataAnnotations = System.ComponentModel.DataAnnotations;

namespace JhaServer.Controllers
{
	/// <summary>
	/// used as the base class for all controllers
	/// </summary>
	public abstract class BaseApiController : ControllerBase
	{
		#region ctor

		public BaseApiController(ILogger logger)
		{
			Logger = logger;
		}

		#endregion ctor



		#region properties

		protected ILogger Logger { get; }

		#endregion properties



		#region Methods

		protected void LogTrace([CallerMemberName] string caller = "")
		{
			var methodName = $"{this.GetType().Name}.{caller}";

			Logger.LogTrace(methodName);
		}


		protected void LogError(Exception ex, [CallerMemberName] string caller = "")
		{
			LogError(null, ex, caller);
		}


		/// <summary>
		/// logs the error with the additional message
		/// </summary>
		/// <param name="additionalMessage"></param>
		/// <param name="ex"></param>
		/// <param name="caller"></param>
		protected void LogError(string? additionalMessage, Exception ex, [CallerMemberName] string caller = "")
		{
			var fullErrorMessage = ex.GetLogMessage(this.GetType().Name, additionalMessage, caller);

			Logger.LogError(fullErrorMessage);
		}



		/// <summary>
		/// used as a shortcut to try and execute the action, get the result, log any traces and errors, and handle any exceptions that may occur
		/// </summary>
		/// <param name="options">the options on how this action shold behave</param>
		/// <param name="action"></param>
		/// <param name="caller"></param>
		/// <returns></returns>
		protected async Task<IActionResult> DoWithTryCatchAsync(Func<Task<object>> action, [CallerMemberName] string caller = "")
		{
			LogTrace(caller);

			try
			{
				var result = await action();

				//if the result itself is an IActionResult...
				if (result is IActionResult actionResult)
					//... we can just return it directly
					return actionResult;

				return Ok(result);
			}
			catch (Exception ex)
			{
				LogError(ex, caller);

				return CreateErrorResponse(ex);
			}
		}//end DoWithTryCatchAsync


		/// <summary>
		/// throws a validation exception which, if caught and  handled by this controller, will return a Bad request (400) with the validation errors
		/// </summary>
		/// <param name="message"></param>
		/// <exception cref="ValidationException"></exception>
		protected void ThrowValidationException(string? message = null)
		{
			throw new SysComDataAnnotations.ValidationException(message);
		}


		protected IActionResult CreateErrorResponse(Exception ex)
		{
			var sb = new StringBuilder();
			var exType = ex.GetType().Name;
			//grab a reference to it so we can loop through inner exceptions
			var exRef = ex;

			while (exRef != null)
			{
				sb.Append(exRef.Message);

				exRef = exRef.InnerException;

				if (exRef != null)
					sb.Append("\nINNER EXCEPTION:\n");
			}

			return ex switch
			{
				//validaton error:
				SysComDataAnnotations.ValidationException => ValidationProblem(sb.ToString(), statusCode: StatusCodes.Status400BadRequest, type: exType),
				//default error:
				_ => Problem(sb.ToString(), statusCode: StatusCodes.Status500InternalServerError, type: exType)
			};
		}//end CreateErrorResponse
		
		#endregion Methods
	}
}
