cd ..\SocialChefApi

dotnet-ef database update -p SocialChef.Identity\SocialChef.Identity.csproj -c userdbcontext
dotnet-ef database update -p SocialChef.Identity\SocialChef.Identity.csproj -c persistedgrantdbcontext
dotnet-ef database update -p SocialChef.Identity\SocialChef.Identity.csproj -c configurationdbcontext
dotnet-ef database update -p SocialChef.Domain.Relational\SocialChef.Domain.Relational.csproj -c sqldbcontext -s SocialChef.Application\SocialChef.Application.csproj

pause