using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace _23DH110152_Shopbanacc.Models.ViewModel
{
    public class ProductSearchVM
    {
        //tiêu chí để search theoe tên, mô tả sp
        //hoặc loại sản phẩm
        public string SearchTerm { get; set; }
        //các tiêu chí để search theo giá
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        //thứ tự sắp xếp
        public string SortOrder { get; set; }
        //các thuộc tính hỗ trợ phân trang
        public int PageNumber {  get; set; }//trang hiện tại
        public int PageSize { get; set; } = 10;//số sản phẩm mỗi trang
        //danh sách sản phẩm đã phân trang
        public PagedList.IPagedList<Product> Products { get; set; }
        //public List<Product> Products { get; set; }

    }   
}