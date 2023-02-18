using System;

namespace Platformer.GameCore.Helpers
{
	public static class TimeFormatter
	{
		public static string GetFormattedTime(float seconds) =>
			TimeSpan.FromSeconds(seconds).ToString("mm\\:ss");
	}
}