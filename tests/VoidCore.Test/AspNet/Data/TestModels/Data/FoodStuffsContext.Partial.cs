using Microsoft.EntityFrameworkCore;

namespace VoidCore.Test.AspNet.Data.TestModels.Data
{
    public partial class FoodStuffsContext
    {
        public FoodStuffsContext(DbContextOptions<FoodStuffsContext> options) : base(options) { }
    }
}
