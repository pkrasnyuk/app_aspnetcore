using System.Collections.Generic;

namespace WebAppCore.DAL.PagingHelpers
{
    public class PagedResults<TEntity>
    {
        /// <summary>
        /// The page number this page represents.
        /// </summary>
        public long PageNumber { get; set; }

        /// <summary>
        /// The size of this page.
        /// </summary>
        public long PageSize { get; set; }

        /// <summary>
        /// The total number of pages available.
        /// </summary>
        public long TotalNumberOfPages { get; set; }

        /// <summary>
        /// The total number of records available.
        /// </summary>
        public long TotalNumberOfEntities { get; set; }

        /// <summary>
        /// The records this page represents.
        /// </summary>
        public ICollection<TEntity> Entities { get; set; }

        public PagedResults()
        {
            Entities = new List<TEntity>();
        }
    }
}
