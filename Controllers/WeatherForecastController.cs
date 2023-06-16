using API.Models;
using API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecepiesController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<RecepieMinDTO> GetAllRecipes()
        {
            // Return all Database Entrys
            using (var db = new CookingDevContext())
            {
                var recipes = db.Recipes;
                var recipesMinDTO = new List<RecepieMinDTO>();
                foreach (var recipe in recipes)
                {
                    recipesMinDTO.Add(new RecepieMinDTO
                    {
                        Id = recipe.Id,
                        Name = recipe.Name,
                        
                    });
                }

                return recipesMinDTO;
            }
        }


        [HttpPost]
        public RecepieDTO AddRecipe(RecepieDTO recipedto)
        {
            var recipeInDB = new Recipe
            {
                Name = recipedto.Name,
                Instructions = recipedto.Instructions
            };

            // Create a Recipe Entry and get the ID
            using (var db = new CookingDevContext())
            {
                db.Recipes.Add(recipeInDB);
                db.SaveChanges();
                recipedto.Id = recipeInDB.Id;
            }

           // Create all Ingredient Entries if they don't exist and create the RecipeIngredient Entries
            using (var db = new CookingDevContext())
            {
                foreach (var ingredient in recipedto.ingredients)
                {
                    var ingredientInDb = db.Ingredients.FirstOrDefault(i => (i.Name == ingredient.Name) && (i.Unit == ingredient.Unit));
                    if (ingredientInDb == null)
                    {
                        ingredientInDb = new Ingredient
                        {
                            Name = ingredient.Name,
                            Unit = ingredient.Unit,
                        };
                        db.Ingredients.Add(ingredientInDb);
                        db.SaveChanges();
                    }

                    var recipeIngredient = new RecipeIngredient
                    {
                        RecipeId = recipeInDB.Id,
                        IngredientId = ingredientInDb.Id,
                        Amount = ingredient.Amount,
                    };
                    db.RecipeIngredients.Add(recipeIngredient);
                    db.SaveChanges();
                }
            }

            return recipedto;
        }

        [HttpPut]
        public void UpdateRecipe(Recipe recipe)
        {
            // Update a Database Entry
            using (var db = new CookingDevContext())
            {
                db.Recipes.Update(recipe);
                db.SaveChanges();
            }
        }

        [HttpDelete("{id}")]
        public void DeleteRecipe(int id)
        {
            // Delete a Database Entry
            using (var db = new CookingDevContext())
            {
                var recipe = db.Recipes.Find(id);
                db.Recipes.Remove(recipe);
                db.SaveChanges();
            }
        }
    }
}