using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipesWebApplication.Repository
{
    public class RecipeRepo
    {
        private RepshoDBE db;

        public RecipeRepo ()
        {
            db = new RepshoDBE();
        }
        ~RecipeRepo()
        {
            db.Dispose();
            db = null;
        }
        public List<Recipe> GetRecipesByID(int recipeCat)
        {
            var db = new RepshoDBE();
            var recipes = db.Recipes.Where(recipe => recipe.RecipeCategoryID == recipeCat).ToList();
            return recipes;
        }
        public List<Recipe> GetAllRecipes()
        {
            var db = new RepshoDBE();
            var recipes = db.Recipes.Select(recipe => recipe).ToList();
            return recipes;
        }
        public IEnumerable<SelectListItem> GetAllRecipeCategories()
        {
            var rc = db.RecipeCategories.Select(s => s).ToList();
            List<SelectListItem> RecipeCategory = new List<SelectListItem>();

            foreach (var item in rc)
            {
                RecipeCategory.Add(
                    new SelectListItem
                    {
                        Value = item.RecipeCategoryID.ToString(),
                        Text = item.RecipeCategory1
                    });
            }

            return RecipeCategory;
        }

        public IEnumerable<SelectListItem> GetAllMeasurmentTypes()
        {
            var mt = db.MeasureTypes.Select(s => s).ToList();
            List<SelectListItem> MeasureTypes = new List<SelectListItem>();

            foreach (var item in mt)
            {
                MeasureTypes.Add(
                    new SelectListItem
                    {
                        Value = item.MeasureTypeID.ToString(),
                        Text = item.MeasureType1
                    });                
            }
            return MeasureTypes;
        }

        public Recipe InsertRecipe(Recipe r)
        {
            db.Recipes.Add(r);
            db.SaveChanges();

            return r;
        }

        public List<Recipe> GetRecipesByCategory(int categoryID)
        {
            var recipes = db.Recipes.Where(r => r.RecipeCategoryID == categoryID).ToList();
            return recipes;
        }
    }   
    
}