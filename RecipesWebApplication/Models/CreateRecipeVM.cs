using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipesWebApplication.Repository;

namespace RecipesWebApplication.Models
{
    public class CreateRecipeVM
    {
        public int RecipeID { get; set; }
        [Required(ErrorMessage = "*")]
        public string RecipeName { get; set; }
        [Required(ErrorMessage = "*")]
        public int RecipeCategoryID { get; set; }
        [Required(ErrorMessage = "*")]
        public TimeSpan PrepTime { get; set; }
        [Required(ErrorMessage = "*")]
        public TimeSpan CookTime { get; set; }
        [Required(ErrorMessage = "*")]
        public TimeSpan TotalTime { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase RecipeImage { get; set; }
        [Required(ErrorMessage = "*")]
        public List<RecipeStep> RecipeSteps { get; set; }
        [Required(ErrorMessage = "*")]
        public List<RecipeIngredient> RecipeIngredients { get; set; }


        public string Category { get; set; }
        public IEnumerable<SelectListItem> RecipeCategories { get; set; }
        public List<IngredientCategory> IngredientCategories { get; set; }
        public decimal Amount { get; set; }
        public IEnumerable<SelectListItem> MeasurmentTypes { get; set; }
        public string Instruction { get; set; }
        public string Error { get; set; }
    }
}