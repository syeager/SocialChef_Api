﻿using System;
using System.Threading.Tasks;
using LittleByte.Asp.Database;
using LittleByte.Asp.Exceptions;
using SocialChef.Business.Document;
using SocialChef.Business.Relational;

namespace SocialChef.Domain.Recipes
{
    public interface IRecipeCreator
    {
        Task<Recipe> CreateAsync(Recipe recipe);
    }

    public class RecipeCreator : IRecipeCreator
    {
        private readonly CosmosContext documentContext;
        private readonly SqlDbContext sqlContext;

        public RecipeCreator(CosmosContext documentContext, SqlDbContext sqlContext)
        {
            this.documentContext = documentContext;
            this.sqlContext = sqlContext;
        }

        public async Task<Recipe> CreateAsync(Recipe recipe)
        {
            var cheDao = await sqlContext.Chefs.FindAsync(recipe.ChefID.Value);
            if(cheDao == null)
            {
                throw new NotFoundException(typeof(Chef), recipe.ChefID.Value);
            }

            RecipeDao recipeDao = recipe;
            recipeDao.ID = Guid.Empty;

            await documentContext.AddAndSaveAsync(recipeDao);

            var chefRecipe = new ChefRecipe(recipeDao.ChefID, recipeDao.ID);
            await sqlContext.AddAndSaveAsync(chefRecipe);

            return recipeDao;
        }
    }
}