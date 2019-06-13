using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using RecipesWebApplication.Models;
using RecipesWebApplication.Repository;

namespace RecipesWebApplication.Controllers
{
    public class RecipeController : BaseController
    {
        private RecipeRepo rr;
        public RecipeController()
        {
            rr = new RecipeRepo();
        }
        ~RecipeController()
        {
            rr.Dispose();
        }
        // GET: Recipe
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CreateRecipe(int? recipeID)
        {
            CreateRecipeVM recipeVM = new CreateRecipeVM();
            if (recipeID != null && recipeID!= 0)
            { 
                
                var r = rr.GetRecipeByID((int)recipeID);
                recipeVM.RecipeName = r.RecipeName;
                recipeVM.PrepTime = r.PrepTime;
                recipeVM.CookTime = r.CookTime;
                recipeVM.TotalTime = r.TotalTime;
                recipeVM.RecipeCategoryID = r.RecipeCategoryID;
                recipeVM.RecipeIngredients = r.RecipeIngredients.ToList();
                recipeVM.RecipeSteps = r.RecipeSteps.ToList();
            }

            IngredientRepo ingredientRepo = new IngredientRepo();
            var ingredientCategories = ingredientRepo.GetIngredientCategories();
            var listRecipeCategories = rr.GetAllRecipeCategoriesSelect();
            var listMeasueTypes = rr.GetAllMeasurmentTypes();
            recipeVM.RecipeCategories = listRecipeCategories;
            recipeVM.MeasurmentTypes = listMeasueTypes;
            recipeVM.IngredientCategories = ingredientCategories;
            return View(recipeVM);
        }
        [HttpPost]
        public ActionResult CreateRecipe(CreateRecipeVM model)
        {
            var validImgTypes = new string[]
            {
                "image/gif",
                "image/jpeg",
                "image/pjpeg",
                "image/png"
            };

            if(model.RecipeImage == null || model.RecipeImage.ContentLength == 0)
            {
                TempData["ErrorMSG"] = "Missing Image";
            }
            else if(!validImgTypes.Contains(model.RecipeImage.ContentType))
            {
                TempData["ErrorMSG"] = "Image must be either a GIF, JPEG, PJPEG, or PNG";
            }
            if(ModelState.IsValid)
            {

                var uploadDir = "~/Images/RecipeImages";
                var imagePath = System.IO.Path.Combine(Server.MapPath(uploadDir), model.RecipeName + model.RecipeImage.FileName);
                var imageUrl = System.IO.Path.Combine(uploadDir, model.RecipeName + model.RecipeImage.FileName);
                model.RecipeImage.SaveAs(imagePath);

                
                Recipe r = new Recipe();
                r.RecipeName = model.RecipeName;
                r.PrepTime = model.PrepTime;
                r.CookTime = model.CookTime;
                r.TotalTime = model.TotalTime;
                r.RecipeImage = imageUrl;
                r.IsHidden = 1;
                r.RecipeIngredients = model.RecipeIngredients;
                r.RecipeSteps = model.RecipeSteps;
                r.RecipeCategoryID = model.RecipeCategoryID;
                if (model.RecipeID != 0)
                {
                    r.RecipeID = model.RecipeID;
                    r = rr.UpdateRecipe(r);
                }
                else
                {
                    r = rr.InsertRecipe(r);
                }

                return View("ViewRecipe", r);
            }
            return RedirectToAction("CreateRecipe");
        }
       
        public ActionResult GetIngredientsByCategory(int category)
        {
            IngredientsVM i = new IngredientsVM();
            IngredientRepo ir = new IngredientRepo();

            var ingredients = ir.GetIngredientsByCategory(category);
            i.Ingrdients = ingredients;

            return PartialView("IngredientList", i);

        }

        public ActionResult RecipeCategories()
        {
            RecipesVM recipes = new RecipesVM();
            recipes.RecipeCategories = rr.GetAllRecipeCategoriesList();
            return View(recipes);
        }

        public ActionResult GetRecipeByCategory(int catID)
        {
            RecipesVM recipes = new RecipesVM();            
            recipes.Recipes = rr.GetRecipesByCategory(catID);
            return PartialView("RecipeEditDelete", recipes);
        }

        public ActionResult DeleteRecipes(int recipeID)
        {
            if(recipeID != 0)
            {
                rr.MakeRecipeHidden(recipeID);
            }

            return Content(recipeID.ToString());
        }


    }
}