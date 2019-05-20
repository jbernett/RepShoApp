using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RecipesWebApplication.Models;
using RecipesWebApplication.Repository;

namespace RecipesWebApplication.Controllers
{
    public class IngredientController : BaseController
    {
        // GET: Ingredient
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpGet]
        public ActionResult CreateIngredient()
        {
            CreateIngredientVM ci = new CreateIngredientVM();
            IngredientRepo ingredientRepo = new IngredientRepo();
            var listOfCategories = ingredientRepo.GetAllIngredientCategories();
            ci.IngredientCategories = listOfCategories;
            return View(ci);
        }

        [HttpPost]
        public ActionResult CreateIngredient(CreateIngredientVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {
                IngredientRepo ri = new IngredientRepo();
                Ingredient i = new Ingredient();
                i.IngredientName = model.IngredientName;
                i.IngredientCategoryID = model.IngredientCategoryID;
                i = ri.InsertIngredient(i);                    
                ModelState.Clear();
                return RedirectToAction("ViewIngredient", new { i.IngredientID });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ActionResult ViewIngredient(int IngredientID)
        {
            try
            {
                CreateIngredientVM vm = new CreateIngredientVM();
                IngredientRepo ri = new IngredientRepo();
                var ingredient = ri.GetIngredientByID(IngredientID);
                vm.IngredientName = ingredient.IngredientName;
                vm.IngredientCategory = ingredient.IngredientCategory.IngredientCategory1;
                return View(vm);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult ViewIngredients()
        {
            IngredientsVM ic = new IngredientsVM();
            IngredientRepo ri = new IngredientRepo();
            ic.IngredientCategories = ri.GetIngredientCategories();
            ic.Ingrdients = ri.GetIngredients();
            return View(ic);
        }
        [HttpPost]
        public ActionResult ViewIngredients(int category)
        {
            IngredientsVM ic = new IngredientsVM();
            IngredientRepo ri = new IngredientRepo();
            ic.IngredientCategories = ri.GetIngredientCategories();
            ic.Ingrdients = ri.GetIngredientsByCategory(category);
            return View(ic);
        }
    }  
}