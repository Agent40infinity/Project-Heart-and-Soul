namespace Slash.Unity.DataBind.Core.Data
{
    using System.ComponentModel;
    using Slash.Unity.DataBind.Core.Utils;

    /// <summary>
    ///   Context to wrap a <see cref="INotifyPropertyChanged"/> data class.
    /// </summary>
    public class NotifyPropertyChangedDataContext : Context
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public NotifyPropertyChangedDataContext(INotifyPropertyChanged dataContext)
            : base(dataContext)
        {
        }

        /// <summary>
        ///     Data object this context wraps.
        /// </summary>
        public INotifyPropertyChanged DataObject
        {
            get
            {
                return (INotifyPropertyChanged) this.GetValue(DataBindSettings.SelfReferencePath);
            }
        }
    }
}