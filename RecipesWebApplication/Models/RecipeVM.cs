using RecipesWebApplication.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RecipesWebApplication.Models
{
    public class RecipeVM
    {
        public string RecipeName { get; set; }
        public string RecipeCategory { get; set; }
        public TimeSpan PrepTime { get; set; }
        public TimeSpan CookTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public HttpPostedFileBase RecipeImage { get; set; }
        public List<RecipeStep> RecipeSteps { get; set; }
        public List<RecipeIngredient> RecipeIngredients { get; set; }
        public List<Ingredient> Ingredients { get; set; }
    }
}