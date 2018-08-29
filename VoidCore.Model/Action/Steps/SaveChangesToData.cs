using VoidCore.Model.Action.Responder;
using VoidCore.Model.Data;

namespace VoidCore.Model.Action.Steps
{
    /// <summary>
    /// Save all changes to persistence.
    /// </summary>
    public class SaveChangesToData : IActionStep
    {
        /// <summary>
        /// Construct a new action.
        /// </summary>
        /// <param name="data">The persistable instance to invoke SaveChanges on</param>
        public SaveChangesToData(IPersistable data)
        {
            _data = data;
        }

        /// <inheritdoc/>
        public void Perform(IActionResponder respond)
        {
            _data.SaveChanges();
        }

        private readonly IPersistable _data;
    }
}
