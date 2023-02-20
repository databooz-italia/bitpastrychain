cd "../BitPastry.Backend.DTO/Entities"
del *.*
cd "../../BitPastry.Backend.Data"
del BitPastryDB.cs
dotnet ef dbcontext scaffold "server=localhost; Database=BitPastry; uid=root" Pomelo.EntityFrameworkCore.MySql --output-dir "../BitPastry.Backend.DTO/Entities" --context "BitPastryDB" --context-dir "./" --no-onconfiguring --no-build  --force
