using API.Models;
using API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace API.Controllers
{
    /// <summary>
    /// API Route for managing recipes
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RecepiesController : ControllerBase
    {
        /// <summary>
        /// Endpoint for getting all recipes
        /// </summary>
        /// <returns>Returns an Array of <see cref="RecepieMinDTO"/></returns>
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

        /// <summary>
        /// Endpoint for getting a specific recipe by its id
        /// </summary>
        /// <param name="id"><see cref="Recipe.Id"/> of a recipe</param>
        /// <returns>Returns a <see cref="RecepieDTO"/></returns>
        [HttpGet("{id}")]
        public ActionResult<RecepieDTO> GetRecipe(int id)
        {
            // Return a Database Entry
            using (var db = new CookingDevContext())
            {
                Recipe? recipe = db.Recipes.Find(id);

                if (recipe == null)
                {
                    return this.NotFound();
                }

                RecepieDTO recipeDTO = new ()
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    Instructions = recipe.Instructions,
                    ingredients = new List<IngredientDTO>(),
                };
                List<RecipeIngredient> recipeIngredients = db.RecipeIngredients.Where(ri => ri.RecipeId == recipe.Id).ToList();

                foreach (var recipeIngredient in recipeIngredients)
                {
                    var ingredient = db.Ingredients.Find(recipeIngredient.IngredientId);

                    if (ingredient == null)
                    {
                        continue;
                    }

                    recipeDTO.ingredients.Add(new IngredientDTO
                    {
                        Id = ingredient.Id,
                        Name = ingredient.Name,
                        Unit = ingredient.Unit,
                        Amount = recipeIngredient.Amount,
                    });
                }

                return this.Ok(recipeDTO);
            }
        }

        /// <summary>
        /// Endpoint to create a new recipe and save it to the database
        /// </summary>
        /// <param name="recipedto"><see cref="RecepieDTO"/> Object</param>
        /// <returns>Returns the created <see cref="RecepieDTO"/></returns>
        [HttpPost]
        public ActionResult<RecepieDTO> AddRecipe(RecepieDTO recipedto)
        {
            var recipeInDB = new Recipe
            {
                Name = recipedto.Name,
                Instructions = recipedto.Instructions,
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

        /// <summary>
        /// Delete a recipe by its id from the database
        /// </summary>
        /// <param name="id"><see cref="Recipe.Id"/> of a recipe</param>
        /// <returns>Statuscode 200 if successfull</returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteRecipe(int id)
        {
            // Delete a Database Entry
            using (var db = new CookingDevContext())
            {
                var recipe = db.Recipes.Find(id);
                if (recipe == null)
                {
                    return this.NotFound();
                }

                db.Recipes.Remove(recipe);
                db.SaveChanges();
                return this.Ok();
            }
        }

        [HttpPut("{id}")]
        public ActionResult<RecepieDTO> UpdateRecipe(int id, RecepieDTO recipedto)
        {
            // Update a Database Entry
            using (var db = new CookingDevContext())
            {
                var recipe = db.Recipes.Find(id);
                if (recipe == null)
                {
                    return this.NotFound();
                }

                recipe.Name = recipedto.Name;
                recipe.Instructions = recipedto.Instructions;
                db.SaveChanges();
            }

            // Delete all RecipeIngredient Entries
            using (var db = new CookingDevContext())
            {
                var recipeIngredients = db.RecipeIngredients.Where(ri => ri.RecipeId == id);
                db.RecipeIngredients.RemoveRange(recipeIngredients);
                db.SaveChanges();
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
                        RecipeId = id,
                        IngredientId = ingredientInDb.Id,
                        Amount = ingredient.Amount,
                    };
                    db.RecipeIngredients.Add(recipeIngredient);
                    db.SaveChanges();
                }
            }

            return recipedto;
        }
    }
}