using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SchoolManagementSystem.Common.Helpers
{
	public static class ValidationHelper
	{
		public static Dictionary<string, string[]> ExtractErrors(ModelStateDictionary modelState)
		{
			return modelState
				.Where(x => x.Value.Errors.Count > 0)
				.ToDictionary(
					kvp => kvp.Key,
					kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
				);
		}
	}
}
