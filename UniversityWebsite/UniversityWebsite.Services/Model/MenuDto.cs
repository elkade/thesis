using System.Collections.Generic;

namespace UniversityWebsite.Services.Model
{
    public class MenuDto
    {
        public List<MenuItemDto> Items = new List<MenuItemDto>();
        public string CountryCode { get; set; }
        public int GroupId { get; set; }
    }

    public class MenuItemDto
    {
        public string UrlName { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        public int PageId { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public class MenuData
    {
        public int? MenuId { get; set; }
        public int? GroupId { get; set; }
        public string CountryCode { get; set; }
        public List<MenuItemData> Items { get; set; }
    }

    public class MenuItemData
    {
        public int PageId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
    }
}
