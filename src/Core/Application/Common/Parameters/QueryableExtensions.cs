using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Application.Common.Parameters
{
    public static class QueryableExtensions
    {
        public static void ApplyOrdering<T>(this ISpecificationBuilder<T> query, string? sortColumn, string? sortOrder) where T : class
        {
            if (!string.IsNullOrEmpty(sortColumn))
            {
                var property = typeof(T).GetProperty(sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    if (string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase))
                    {
                        query.OrderByDescending(x => EF.Property<object>(x, property.Name));
                    }
                    else
                    {
                        query.OrderBy(x => EF.Property<object>(x, property.Name));
                    }
                }
            }
        }

        public static IEnumerable<T> ApplyOrdering<T>(this IEnumerable<T> source, string? sortColumn, string? sortOrder)
        {
            if (string.IsNullOrEmpty(sortColumn)) return source;

            var property = typeof(T).GetProperty(sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null) return source;

            return string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase)
                ? source.OrderByDescending(x => property.GetValue(x, null))
                : source.OrderBy(x => property.GetValue(x, null));
        }

        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> source, string? sortColumn, string? sortOrder)
        {
            if (string.IsNullOrEmpty(sortColumn)) return source;

            var property = typeof(T).GetProperty(sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property == null) return source;

            return string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase)
                ? source.OrderByDescending(x => EF.Property<object>(x, property.Name))
                : source.OrderBy(x => EF.Property<object>(x, property.Name));
        }

    }
}
