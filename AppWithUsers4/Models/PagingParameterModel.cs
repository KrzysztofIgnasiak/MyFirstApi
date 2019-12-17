using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWithUsers4.Models
{
    public class PagingParameterModel
    {
        const int MaxPageSize = 20;

        public int PageNumber { get; set; } = 1;

        public int _PageSize { get; set; } = 1;

        public int PageSize
        {

            get { return _PageSize; }
            set
            {
                _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }
    }
}