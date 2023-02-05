using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JhaUnitTests.Mocks
{
	public class MockInMemoryLogger<t> : ILogger<t>
	{
		ConcurrentDictionary<LogLevel, List<string>> _dictionary = new ConcurrentDictionary<LogLevel, List<string>>();


		
		public IDisposable? BeginScope<TState>(TState state) where TState : notnull
			=> new DoNothingDisposable();


		public bool IsEnabled(LogLevel logLevel) 
			=> true;
		

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			var logsForLevel = _dictionary.AddOrUpdate(logLevel, new List<string>(), (ll, list) => list);

			if (state != null)
				logsForLevel.Add(state.ToString());
		}


		public IEnumerable<string> GetLogs(LogLevel level)
		{
			if (_dictionary.TryGetValue(level, out List<string> logs))
				return logs;

			return Enumerable.Empty<string>();
		} 



		class DoNothingDisposable : IDisposable
		{
			public void Dispose()
			{
				//do nothing
			}
		}
	}
}
