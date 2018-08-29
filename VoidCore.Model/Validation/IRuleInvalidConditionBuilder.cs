namespace VoidCore.Model.Validation
{
    /// <summary>
    /// An interface to build a rule invalid condition.
    /// </summary>
    public interface IRuleBuilder
    {
        /// <summary>
        /// A condition that is ture when the model is invalid. Multiple "when" statements can be chained together to make a more complex rule.
        /// </summary>
        /// <param name="invalidCondition"></param>
        /// <returns></returns>
        IRuleBuilder ValidWhen(bool invalidCondition);

        /// <summary>
        /// A condition that claims when the rule should be ignored.
        /// </summary>
        /// <param name="suppressionCondition"></param>
        /// <returns></returns>
        IRuleBuilder ExceptWhen(bool suppressionCondition);
    }
}
