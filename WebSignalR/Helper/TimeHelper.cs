namespace WebSignalR.Helper
{
	public class TimeHelper
	{
		public static string GetTimeSender(DateTime createdDate)
		{
			var currentTime = GetCurrentInVietName();

			var timeSpan = currentTime - createdDate;

			if (timeSpan.TotalDays > 1)
			{
				return createdDate.ToString("dd/MM/yyyy");
			}

			if (timeSpan.TotalHours > 1)
			{
				return ((int)(timeSpan.TotalHours)).ToString() + "h";
			}

			if(timeSpan.TotalMinutes > 1)
			{
				return ((int)(timeSpan.TotalMinutes)).ToString() + "m";
			}

			return ((int)(timeSpan.TotalSeconds)).ToString() + "s";
		}


		public static DateTime GetCurrentInVietName()
		{
			// Specify the time zone ID for Vietnam
			const string timeZoneId = "SE Asia Standard Time"; // This is the ID for the time zone of Vietnam

			// Get the time zone information for Vietnam
			var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

			// Get the current date and time in Vietnam
			var nowInVietnam = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);

			// Display the current date and time in Vietnam
			return nowInVietnam;
		}

	}
}
