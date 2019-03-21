using System;
using System.Linq.Expressions;
using VoidCore.Model.Queries;
using VoidCore.Test.AspNet.Data.TestModels.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Queries
{
    public class UserSpecification : QuerySpecificationAbstract<User>
    {
        public UserSpecification(params Expression<Func<User, bool>>[] criteria) : base(criteria)
        {
            ApplyOrderBy(u => u.Name);
        }
    }
}
