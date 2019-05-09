using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipesWebApplication.Repository;

namespace RecipesWebApplication.Models
{
    public class CreateIngredientVM
    {
        public string IngredientName { get; set; }
        public int IngredientCategoryID { get; set; }
        public IEnumerable<SelectListItem> IngredientCategories { get; set; }
        public string IngredientCategory { get; set; }
    }
}