using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipesWebApplication.Repository;

namespace RecipesWebApplication.Models
{
    public class RecipesVM
    {
        public List<Recipe> Recipes { get; set; }
        public List<RecipeCategory> RecipeCategories { get; set; }
    }
}