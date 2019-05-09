using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RecipesWebApplication.Repository
{

    public class IngredientRepo
    {
        private RepshoDBE db;

        public IngredientRepo ()
        {
            db = new RepshoDBE();
        }
        ~IngredientRepo()
        {
            db.Dispose();
            db = null;
        }
        public IEnumerable<SelectListItem> GetAllIngredientCategories()
        {            
            var ic = db.IngredientCategories.Select(s => s).ToList();
            List<SelectListItem> IngredientCategory = new List<SelectListItem>();

            foreach (var item in ic)
            {
                IngredientCategory.Add(
                    new SelectListItem
                    {
                        Value = item.IngredientCategoryID.ToString(),
                        Text = item.IngredientCategory1
                    });  
            }
            return IngredientCategory;
        }
        public Ingredient InsertIngredient(Ingredient i)
        {
            db.Ingredients.Add(i);
            db.SaveChanges();
            return i;
        }
        public Ingredient GetIngredientByID(int ingredientID)
        {
            var ingredient = db.Ingredients.FirstOrDefault(i => i.IngredientID == ingredientID);
            return ingredient;
        }

        public List<IngredientCategory> GetIngredientCategories()
        {
            var ingredientCategories = db.IngredientCategories.Select(ic => ic).ToList();
            return ingredientCategories;
        }

        public List<Ingredient> GetIngredientsByCategory(int categoryID)
        {
            var ingredients = db.Ingredients.Where(i => i.IngredientCategoryID == categoryID).ToList();
            return ingredients;
        }
        public List<Ingredient> GetIngredients()
        {
            var ingredients = db.Ingredients.Select(i => i).ToList();
            return ingredients;
        }
    }
}
