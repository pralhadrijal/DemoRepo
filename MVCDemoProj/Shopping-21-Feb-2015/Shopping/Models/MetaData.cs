using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
    [MetadataType(typeof(PostAdMeta))]
    public partial class PostAd
    {

    }
    public partial class PostAdMeta
    {
        [Required(ErrorMessage = "({0} is required)")]
        [StringLength(35, ErrorMessage = "({0} length mustn't exceed 35 character)")]
        public string Title { get; set; }

        [Required(ErrorMessage = "({0} is required)")]
        [DataType(DataType.Currency, ErrorMessage = "({0} must be in number)")]
        [Range(0, 999999999999999, ErrorMessage = "(Invalid {0} Detected)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "({0} is required)")]
        public decimal Condition { get; set; }

        [Required(ErrorMessage = "({0} is required)")]
        [StringLength(30, ErrorMessage = "({0} length exceed)")]
        public string Warrenty { get; set; }

        [Required(ErrorMessage = "({0} is required)")]
        [StringLength(30, ErrorMessage = "({0} length exceed)")]
        [Display(Name = "Used before")]
        public string UsedFor { get; set; }

        [Required(ErrorMessage = "({0} is required)")]
        [Display(Name = "Price Negotiable")]
        public bool PriceNegotiable { get; set; }

        [Required(ErrorMessage = "({0} is required)")]
        [Display(Name = "Product Description")]
        
        [DataType(DataType.MultilineText)]
        public bool Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime PostedDate { get; set; }

        [DataType(DataType.Url)]
        public string Url { get; set; }

        public bool IsHide { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image1 { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Image2 { get; set; }
        [DataType(DataType.ImageUrl)]
        public string Image3 { get; set; }
    }

    [MetadataType(typeof(UserInfoMeta))]
    public partial class UserInfo
    {
        [Required(ErrorMessage = "* {0} is required")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
    public partial class UserInfoMeta
    {
        [Required(ErrorMessage = "* {0} is required")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "* {0} must be in the range of 5-30 character")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "* {0} is required")]
        [StringLength(500, MinimumLength = 8, ErrorMessage = "* {0} too short")]
        public string Password { get; set; }

        [Required(ErrorMessage = "* {0} is required")]
        [StringLength(50, ErrorMessage = "* {0} character exceed")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "* {0} is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "* {0} is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "* Invalid {0}")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string FirstPhone { get; set; }
        
        [StringLength(10, MinimumLength = 10, ErrorMessage = "* Invalid {0}")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string SecondPhone { get; set; }

        [Required(ErrorMessage = "* {0} is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "* {0} is required")]
        public string Zone { get; set; }

        [Required(ErrorMessage = "* {0} is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "* {0} is required")]
        [Display(Name = "Local Address")]
        public string LocalAddress { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime JoinDate { get; set; }
        
        public string UserRole { get; set; }
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(CategoryMeta))]
    public partial class Category
    {
    }
    public partial class CategoryMeta
    {
        [Required(ErrorMessage = "* {0} is required")]
        [Display(Name = "Category")]
        public string Name { get; set; }
    }

    [MetadataType(typeof(ProductMeta))]
    public partial class Product
    {
    }
    public partial class ProductMeta
    {
        [Required(ErrorMessage = "* {0} is required")]
        [Display(Name = "Product")]
        public string Name { get; set; }
    }

    [MetadataType(typeof(WishListMeta))]
    public partial class WishList
    {
 
    }
    public partial class WishListMeta
    {
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime At { get; set; }        
    }
}