using API.Models;
using API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Net.Mime;

namespace API.Controllers
{
    /// <summary>
    /// API Route for managing recipes
    /// </summary>
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class RecepiesController : ControllerBase
    {
        /// <summary>
        /// Gets all recipes and filters by search query string
        /// </summary>
        /// <param name="search">Search query that is filtered agaínst</param>
        /// <returns>Returns an Array of repipes</returns>
        /// <response code="200">Returns an Array of repipes</response>
        [HttpGet]
        public ActionResult<List<RecepieMinDTO>> GetRecipes([FromQuery] string? search)
        {
            // Return all Database Entries
            using (var db = new CookingDevContext())
            {
                var recipes = new List<Recipe>();

                if (search != null)
                {
                    recipes = db.Recipes.Where(r => r.Name.Contains(search)).ToList();
                }
                else
                {
                    recipes = db.Recipes.ToList();
                }

                var recipesDTO = new List<RecepieMinDTO>();

                foreach (var recipe in recipes)
                {
                    recipesDTO.Add(new RecepieMinDTO
                    {
                        Id = recipe.Id,
                        Name = recipe.Name,
                    });
                }

                return this.Ok(recipesDTO);
            }
        }

        /// <summary>
        /// Gets a specific recipe by id
        /// </summary>
        /// <param name="id"><see cref="Recipe.Id"/> of a recipe</param>
        /// <returns>Returns the recipe that belongs to the ID</returns>
        /// <response code="200">Returns the recipe that belongs to the ID</response>
        /// <response code="404">Returned if no recipe is found that belonges to that id</response>
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
                    Ingredients = new List<IngredientDTO>(),
                };
                List<RecipeIngredient> recipeIngredients = db.RecipeIngredients.Where(ri => ri.RecipeId == recipe.Id).ToList();

                foreach (var recipeIngredient in recipeIngredients)
                {
                    var ingredient = db.Ingredients.Find(recipeIngredient.IngredientId);

                    if (ingredient == null)
                    {
                        continue;
                    }

                    recipeDTO.Ingredients.Add(new IngredientDTO
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
        /// Create a new recipe
        /// </summary>
        /// <param name="recipedto">A recipe JSON Objekt</param>
        /// <returns>Returnes the newly created recipe</returns>
        /// <response code="200">Returnes the newly created recipe</response>
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
                foreach (var ingredient in recipedto.Ingredients)
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
        /// Delete a recipe by its id
        /// </summary>
        /// <param name="id"><see cref="Recipe.Id"/> of a recipe</param>
        /// <returns>nothing</returns>
        /// <response code="200">Returnes if successfully deleted</response>
        /// <response code="404">Returned if no recipe is found that belonges to that id</response>
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

        /// <summary>
        /// Modify a recipe by its id
        /// </summary>
        /// <param name="id">The ID of the Recipe that will be edited</param>
        /// <param name="recipedto">A recipe JSON Objekt</param>
        /// <returns>Returnes the edited recipe</returns>
        /// <response code="200">Returnes the edited recipe</response>
        /// <response code="404">Returned if no recipe is found that belonges to that id</response>
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
                foreach (var ingredient in recipedto.Ingredients)
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