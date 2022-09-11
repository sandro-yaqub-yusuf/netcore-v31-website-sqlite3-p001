using System;
using System.Collections.Generic;
using System.Linq;

namespace KITAB.Products.Infra.Paginations
{
    public class PaginationList<T> : List<T>
    {
        public PaginationData PaginationData { get; private set; }

        public PaginationList(ref List<T> p_items, ref int p_count, ref int p_pageNumber, ref int p_pageSize)
        {
            PaginationData = new PaginationData
            {
                TotalCount = p_count,
                PageSize = p_pageSize,
                CurrentPage = p_pageNumber,
                TotalPages = (int)Math.Ceiling(p_count / (double)p_pageSize),
                HasPrevious = (p_pageNumber > 1)
            };

            PaginationData.HasNext = (p_pageNumber < PaginationData.TotalPages);
    
            AddRange(p_items);
        }
    
        public static PaginationList<T> PaginateData(ref List<T> p_source, ref int p_pageNumber, ref int p_pageSize)
        {
            int count = p_source.Count();

            List<T> items = p_source.Skip((p_pageNumber - 1) * p_pageSize).Take(p_pageSize).ToList();
    
            return new PaginationList<T>(ref items, ref count, ref p_pageNumber, ref p_pageSize);
        }
    }
}
