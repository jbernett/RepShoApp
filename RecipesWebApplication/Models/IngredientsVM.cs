using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecipesWebApplication.Repository;

namespace RecipesWebApplication.Models
{
    public class IngredientsVM
    {
        public List<Ingredient> Ingrdients { get; set; }
        public List<IngredientCategory> IngredientCategories { get; set; }
    }
}