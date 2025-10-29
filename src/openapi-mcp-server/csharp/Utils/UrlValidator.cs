namespace Web4AgentProtocol.OpenAPIMCPServer.Utils
{
	internal static class UrlValidator
	{
		public static bool IsValidHttpUrl(string? url)
		{
			if (string.IsNullOrWhiteSpace(url)) return false;
			if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return false;
			return uri.Scheme is "http" or "https";
		}
	}
}