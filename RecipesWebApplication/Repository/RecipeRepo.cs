using RecipesWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipesWebApplication.Repository
{
    public class RecipeRepo: IDisposable
    {
        private RepshoDBE db;

        public RecipeRepo ()
        {
            db = new RepshoDBE();
        }
        ~RecipeRepo()
        {
            Dispose();
        }

        public List<Recipe> GetAllRecipes()
        {
            var recipes = db.Recipes.Where(r => r.IsHidden != 0).ToList();
            return recipes;
        }
        public IEnumerable<SelectListItem> GetAllRecipeCategoriesSelect()
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
        public List<RecipeCategory> GetAllRecipeCategoriesList()
        {
            return db.RecipeCategories.ToList();
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
            DetatchData(r);

            var newR = db.Recipes.Where(s => s.RecipeID == r.RecipeID).FirstOrDefault();

            return newR;
        }

        private void DetatchData(Recipe r)
        {
            foreach (var i in r.RecipeIngredients.ToList())
            {
                db.Entry<RecipeIngredient>(i).State = System.Data.Entity.EntityState.Detached;
            }
            foreach (var i in r.RecipeSteps.ToList())
            {
                db.Entry<RecipeStep>(i).State = System.Data.Entity.EntityState.Detached;
            }
            db.Entry<Recipe>(r).State = System.Data.Entity.EntityState.Detached;
        }

        public Recipe UpdateRecipe (Recipe r)
        {
            var oldR = db.Recipes.Where(s => s.RecipeID == r.RecipeID).FirstOrDefault();
            foreach (var ri in oldR.RecipeIngredients.ToList())
            {
                db.RecipeIngredients.Remove(ri);
            }
            foreach (var rs in oldR.RecipeSteps.ToList())
            {
                db.RecipeSteps.Remove(rs);
            }
            oldR.PrepTime = r.PrepTime;
            oldR.CookTime = r.CookTime;
            oldR.CookTime = r.CookTime;
            oldR.RecipeImage = r.RecipeImage;
            oldR.RecipeCategoryID = r.RecipeCategoryID;
            oldR.RecipeName = r.RecipeName;
            foreach(var ri in r.RecipeIngredients)
            {
                oldR.RecipeIngredients.Add(ri);
            }
            foreach(var rs in r.RecipeSteps)
            {
                oldR.RecipeSteps.Add(rs);
            }
            
            db.SaveChanges();
            DetatchData(r);
            var newR = db.Recipes.Where(s => s.RecipeID == r.RecipeID).FirstOrDefault();
            return newR;

        }

        public List<Recipe> GetRecipesByCategory(int categoryID)
        {
            if (categoryID == 0) return db.Recipes.Where(r => r.IsHidden != 0).ToList();        
            
            return db.Recipes.Where(r => r.RecipeCategoryID == categoryID && r.IsHidden != 0).ToList();
        }

        public void MakeRecipeHidden(int recipeID)
        {
            var recipe = db.Recipes.Where(s => s.RecipeID == recipeID).FirstOrDefault();
            recipe.IsHidden = 0;
            db.SaveChanges();
        }

        public Recipe GetRecipeByID(int recipeID)
        {
            return db.Recipes.Where(r => r.RecipeID == recipeID && r.IsHidden != 0).FirstOrDefault();
        }

        public void Dispose()
        {
            if(db != null)
                db.Dispose();

            db = null;
        }
    }   
    
}