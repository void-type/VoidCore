using System;
using System.Linq.Expressions;
using VoidCore.Model.Queries;
using VoidCore.Test.AspNet.Data.TestModels.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Queries
{
    public class CategorySpecification : QuerySpecificationAbstract<Category>
    {
        public CategorySpecification(params Expression<Func<Category, bool>>[] criteria) : base(criteria) { }
    }
}
