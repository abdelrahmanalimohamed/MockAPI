namespace MockAPI.Application.Exceptions
{
	public sealed class ValidationExceptions : Exception
	{
		public IDictionary<string, string[]> Errors { get; }

		public ValidationExceptions(IDictionary<string, string[]> errors)
			: base("Validation failed")
		{
			Errors = errors;
		}
	}
}