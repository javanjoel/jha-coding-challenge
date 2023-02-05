using System.Runtime.CompilerServices;
using System.Text;

namespace JhaCommon.Extensions
{
	public static class ExceptionExtensions
	{
		/// <summary>
		/// gets a full exception message with all inner exceptions for logging purposes
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="typeName">the type name of the type that had this exception</param>
		/// <param name="caller">the calling method name that had the exception</param>
		/// <returns></returns>
		public static string GetLogMessage(this Exception ex, string typeName, string? additionalMessage = "", [CallerMemberName] string caller = "")
		{
			var sb = new StringBuilder($"{typeName}.{caller}, EXCEPTION - {ex.Message}");
			var innerEx = ex.InnerException;

			if (!string.IsNullOrEmpty(additionalMessage))
				sb.Append($" - {additionalMessage}");

			while (innerEx != null)
			{
				sb.Append($" - [INNER EXCEPTION] = {innerEx.Message}");

				innerEx = innerEx.InnerException;
			}

			return sb.ToString();
		}//end GetLogMessage
	}
}
