using System;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace VoidCore.Test.EfIntegration.TestModels.Data
{
    public class CategoriesSpecification : QuerySpecificationAbstract<Category>
    {
        public CategoriesSpecification(params Expression<Func<Category, bool>>[] criteria) : base(criteria) { }
    }
}
