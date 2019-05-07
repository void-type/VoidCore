using System;
using System.Linq.Expressions;
using VoidCore.Model.Data;

namespace VoidCore.Test.AspNet.Data.TestModels.Data
{
    public class UserSpecification : QuerySpecificationAbstract<User>
    {
        public UserSpecification(params Expression<Func<User, bool>>[] criteria) : base(criteria)
        {
            ApplyOrderBy(u => u.Name);
        }
    }
}
