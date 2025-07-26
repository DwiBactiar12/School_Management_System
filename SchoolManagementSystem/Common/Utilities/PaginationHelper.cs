using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementSystem.Common.Utilities
{
	public class PaginatedResult<T>
	{
		public List<T> Data { get; set; } = new();
		public int TotalCount { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public int TotalPages { get; set; }
		public bool HasNextPage { get; set; }
		public bool HasPreviousPage { get; set; }

		public PaginatedResult(List<T> data, int totalCount, int page, int pageSize)
		{
			Data = data;
			TotalCount = totalCount;
			Page = page;
			PageSize = pageSize;
			TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
			HasNextPage = page < TotalPages;
			HasPreviousPage = page > 1;
		}
	}

	public class PaginationParameters
	{
		private const int MaxPageSize = 100;
		private int _pageSize = 10;

		public int Page { get; set; } = 1;

		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
		}

		public string SearchTerm { get; set; } = string.Empty;
		public string SortBy { get; set; } = string.Empty;
		public bool SortDescending { get; set; } = false;
	}
}
