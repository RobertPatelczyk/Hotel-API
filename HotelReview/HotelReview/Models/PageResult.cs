using System;
using System.Collections.Generic;

namespace HotelReview.Models
{
    public class PageResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PageResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalPages = totalCount;
            ItemFrom = pageSize * (pageNumber - 1) + 1;
            ItemsTo = ItemFrom + pageSize - 1;
            TotalItemsCount = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
