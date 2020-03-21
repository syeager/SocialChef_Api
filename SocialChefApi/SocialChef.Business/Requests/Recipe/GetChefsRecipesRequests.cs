using System;
using JetBrains.Annotations;
using LittleByte.Asp.Business;

namespace SocialChef.Business.Requests
{
    public class GetChefsRecipesRequests : PageRequest
    {
        public Guid ChefID { get; set; }

        [UsedImplicitly]
        public GetChefsRecipesRequests()
        {
        }

        public GetChefsRecipesRequests(Guid chefID)
        {
            ChefID = chefID;
        }
    }
}