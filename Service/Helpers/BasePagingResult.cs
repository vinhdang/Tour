using System;

namespace Service.Helpers
{
    public class BasePagingResult : IPagingResult
    {
        private int _totalPages;
        private int _totalRecords;
        private int _pageSize;
        private int _pageIndex;

        public int TotalPages
        {
            get
            {
                if (_totalPages == 0)
                    _totalPages = CalculateTotalPages();

                return _totalPages;
            }
            set { _totalPages = value; }
        }

        public int TotalRecords
        {
            get { return _totalRecords; }
            set
            {
                _totalRecords = value;
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
            }
        }

        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                _pageIndex = value;
            }
        }

        private int CalculateTotalPages()
        {
            if (PageSize == 0 || TotalRecords == 0)
                return 0;
            return Convert.ToInt32(Math.Ceiling(Convert.ToDouble(TotalRecords) / PageSize));
        }
    }

    public interface IPagingResult
    {
        int TotalPages { get; set; }
        int TotalRecords { get; set; }
        int PageSize { get; set; }
        int PageIndex { get; set; }
    }
}
