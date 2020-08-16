cd ..\SocialChefApi

dotnet-ef database update -s SocialChef.Application\SocialChef.Application.csproj -c SqlDbContext

pause