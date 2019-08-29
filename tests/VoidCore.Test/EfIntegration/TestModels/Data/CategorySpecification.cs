using System;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Data
{
    public class CategorySpecification : QuerySpecificationAbstract<Category>
    {
        public CategorySpecification(params Expression<Func<Category, bool>>[] criteria) : base(criteria) { }
    }
}
