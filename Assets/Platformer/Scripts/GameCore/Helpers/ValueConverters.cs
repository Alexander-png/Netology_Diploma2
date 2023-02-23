using System;

namespace Platformer.GameCore.Helpers
{
	public static class TimeFormatter
	{
		public static string GetFormattedTime(float seconds)
        {
			try
            {
				return TimeSpan.FromSeconds(seconds).ToString("mm\\:ss");
			}
			catch (OverflowException)
            {
				return "--:--";
            }
        }
	}
}