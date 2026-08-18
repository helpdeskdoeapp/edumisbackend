using Microsoft.EntityFrameworkCore;

namespace edumisbackend.Helpers
{
    public class PaginatedListResponseDTO<T>
    {
        public IReadOnlyCollection<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public PaginatedListResponseDTO(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        //public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        //{
        //    TotalCount = count;
        //    PageNumber = pageNumber;
        //    TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        //    AddRange(items);
        //}

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public static async Task<PaginatedListResponseDTO<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var count = await source.CountAsync(cancellationToken);
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PaginatedListResponseDTO<T>(items, count, pageNumber, pageSize);
        }

        public static PaginatedListResponseDTO<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedListResponseDTO<T>(items, count, pageNumber, pageSize);
        }
    }
}
