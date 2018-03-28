using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DCommon.Collections.Pagination;

namespace DCommon.Mvc.Bootstrap.Pager
{
    /// <summary>
    /// Renders a pager component from an IPagination datasource.
    /// </summary>
    public class Pager : IHtmlString
    {
        private readonly IPagination _pagination;
        private readonly ViewContext _viewContext;

        private string _paginationFormat = "显示 {0} - {1} 总共 <b>{2}</b> 条记录 ";
        private string _paginationSingleFormat = "显示 {0} - {1} ";
        private string _paginationFirst = "首页";
        private string _paginationPrev = "上一页";
        private string _paginationNext = "下一页";
        private string _paginationLast = "末页";
        private string _pageQueryName = "页";
        private int _maxPagerLinks = 5;

        /// <summary>
        /// Creates a new instance of the Pager class.
        /// </summary>
        /// <param name="pagination">The IPagination datasource</param>
        /// <param name="context">The view context</param>
        public Pager(IPagination pagination, ViewContext context)
        {
            _pagination = pagination;
            _viewContext = context;
        }

        protected ViewContext ViewContext
        {
            get { return _viewContext; }
        }


        /// <summary>
        /// Specifies the query string parameter to use when generating pager links. The default is 'page'
        /// </summary>
        public Pager QueryParam(string queryStringParam)
        {
            _pageQueryName = queryStringParam;
            return this;
        }
        /// <summary>
        /// Specifies the format to use when rendering a pagination containing a single page. 
        /// The default is 'Showing {0} of {1}' (eg 'Showing 1 of 3')
        /// </summary>
        public Pager SingleFormat(string format)
        {
            _paginationSingleFormat = format;
            return this;
        }

        /// <summary>
        /// Specifies the format to use when rendering a pagination containing multiple pages. 
        /// The default is 'Showing {0} - {1} of {2}' (eg 'Showing 1 to 3 of 6')
        /// </summary>
        public Pager Format(string format)
        {
            _paginationFormat = format;
            return this;
        }

        /// <summary>
        /// Text for the 'first' link.
        /// </summary>
        public Pager First(string first)
        {
            _paginationFirst = first;
            return this;
        }

        /// <summary>
        /// Text for the 'prev' link
        /// </summary>
        public Pager Previous(string previous)
        {
            _paginationPrev = previous;
            return this;
        }

        /// <summary>
        /// Text for the 'next' link
        /// </summary>
        public Pager Next(string next)
        {
            _paginationNext = next;
            return this;
        }

        /// <summary>
        /// Text for the 'last' link
        /// </summary>
        public Pager Last(string last)
        {
            _paginationLast = last;
            return this;
        }

        ///// <summary>
        ///// Uses a lambda expression to generate the URL for the page links.
        ///// </summary>
        ///// <param name="urlBuilder">Lambda expression for generating the URL used in the page links</param>
        //public Pager Link(Func<int, string> urlBuilder)
        //{
        //    _urlBuilder = urlBuilder;
        //    return this;
        //}

        public Pager MaxPagerLinks(int maxPagerLinks)
        {
            _maxPagerLinks = maxPagerLinks < 1 ? 5 : maxPagerLinks;
            return this;
        }

        // For backwards compatibility with WebFormViewEngine
        public override string ToString()
        {
            return ToHtmlString();
        }

        public string ToHtmlString()
        {
            if (_pagination.TotalItems == 0)
            {
                return null;
            }

            var builder = new StringBuilder();

            builder.Append("<div class='datatable-footer'>");
            RenderLeftSideOfPager(builder);

            if (_pagination.TotalPages > 1)
            {
                RenderRightSideOfPager(builder);
            }

            builder.Append(@"</div>");

            return builder.ToString();
        }

        protected virtual void RenderLeftSideOfPager(StringBuilder builder)
        {
            builder.Append("<div class='dataTables_info'>");

            //Special case handling where the page only contains 1 item (ie it's a details-view rather than a grid)
            if (_pagination.PageSize == 1)
            {
                RenderNumberOfItemsWhenThereIsOnlyOneItemPerPage(builder);
            }
            else
            {
                RenderNumberOfItemsWhenThereAreMultipleItemsPerPage(builder);
            }
            builder.Append("</div>");
        }

        protected virtual void RenderRightSideOfPager(StringBuilder builder)
        {
            builder.Append("<div class='dataTables_paginate paging_full_numbers'>");

            if (_pagination.TotalItems > 1)
            {
                var firstIndividualPageIndex = _pagination.PageNumber - _maxPagerLinks + 1;
                var lastIndividualPageIndex = _pagination.PageNumber + _maxPagerLinks;
                firstIndividualPageIndex = firstIndividualPageIndex < 0 ? 0 : firstIndividualPageIndex;
                lastIndividualPageIndex = lastIndividualPageIndex > _pagination.TotalPages
                                              ? _pagination.TotalPages
                                              : lastIndividualPageIndex;

                //首页，上一页
                builder.Append(CreatePageLink(0, _paginationFirst, "first", !_pagination.HasPreviousPage));
                builder.Append(CreatePageLink(_pagination.PageNumber - 1, _paginationPrev, "previous", !_pagination.HasPreviousPage));

                builder.Append("<span>");
                if (firstIndividualPageIndex > 0)
                {
                    builder.Append("<a href=‘javascript:void(0)’>...</a>");
                }
                for (int i = firstIndividualPageIndex; i < lastIndividualPageIndex; i++)
                {
                    if (_pagination.PageNumber == i)
                    {
                        builder.Append(CreateActivePageLink((i + 1).ToString()));
                    }
                    else
                    {
                        builder.Append(CreatePageLink(i, (i + 1).ToString()));
                    }
                }
                if (lastIndividualPageIndex < _pagination.TotalPages - 1)
                {
                    builder.Append("<a href=‘javascript:void(0)’>...</a>");
                }
                builder.Append("</span>");

                //末页，下一页
                builder.Append(CreatePageLink(_pagination.PageNumber + 1, _paginationNext, "next", !_pagination.HasNextPage));
                builder.Append(CreatePageLink(_pagination.TotalPages - 1, _paginationLast, "last", !_pagination.HasNextPage));
            }
            builder.Append("</div>");
        }

        protected virtual void RenderNumberOfItemsWhenThereIsOnlyOneItemPerPage(StringBuilder builder)
        {
            builder.AppendFormat(_paginationSingleFormat, _pagination.FirstItem + 1, _pagination.TotalItems);
        }

        protected virtual void RenderNumberOfItemsWhenThereAreMultipleItemsPerPage(StringBuilder builder)
        {
            builder.AppendFormat(_paginationFormat, _pagination.FirstItem + 1, _pagination.LastItem + 1, _pagination.TotalItems);
        }

        private string CreatePageLink(int pageNumber, string text, string cssClass = null, bool disabled = false)
        {
            var builder = new TagBuilder("a");
            builder.SetInnerText(text);
            builder.Attributes["href"] = "javascript:void(0)";
            builder.Attributes["class"] = "paginate_button";
            if(cssClass != null)
                builder.AddCssClass(cssClass);
            if (disabled)
            {
                builder.AddCssClass("paginate_button_disabled");
            }
            else
            {
                builder.Attributes["data-pageNumber"] = pageNumber.ToString();
                builder.Attributes["data-ajaxPager"] = "true";  
            }

            return builder.ToString(TagRenderMode.Normal);
        }

        private static string CreateActivePageLink(string text)
        {
            var builder = new TagBuilder("a");
            builder.SetInnerText(text);
            builder.Attributes["href"] = "javascript:void(0)";
            builder.Attributes["class"] = "paginate_active";

            return builder.ToString(TagRenderMode.Normal);
        }

        //private string CreateDefaultUrl(int pageNumber)
        //{
        //    var routeValues = new RouteValueDictionary();

        //    foreach (var key in _viewContext.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
        //    {
        //        routeValues[key] = _viewContext.RequestContext.HttpContext.Request.QueryString[key];
        //    }

        //    routeValues[_pageQueryName] = pageNumber;

        //    var url = UrlHelper.GenerateUrl(null, null, null, routeValues, RouteTable.Routes, _viewContext.RequestContext, true);
        //    return url;
        //}
    }
}