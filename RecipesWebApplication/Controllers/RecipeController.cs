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
        public ActionResult CreateRecipe()
        {
            CreateRecipeVM cr = new CreateRecipeVM();
            IngredientRepo ingredientRepo = new IngredientRepo();
            var ingredientCategories = ingredientRepo.GetIngredientCategories();
            var listRecipeCategories = rr.GetAllRecipeCategories();
            var listMeasueTypes = rr.GetAllMeasurmentTypes();
            cr.RecipeCategories = listRecipeCategories;
            cr.MeasurmentTypes = listMeasueTypes;
            cr.IngredientCategories = ingredientCategories;
            return View(cr);
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
                var imageUrl = System.IO.Path.Combine(uploadDir, imagePath);
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
                r = rr.InsertRecipe(r);

                return View("ViewRecipe", r);
            }
            return RedirectToAction("CreateRecipe");
        }
        public ActionResult ViewRecipe(Recipe r)
        {
            return View(r);
        }

        public ActionResult GetIngredientsByCategory(int category)
        {
            IngredientsVM i = new IngredientsVM();
            IngredientRepo ir = new IngredientRepo();

            var ingredients = ir.GetIngredientsByCategory(category);
            i.Ingrdients = ingredients;

            return PartialView("IngredientList", i);

        }


    }
}