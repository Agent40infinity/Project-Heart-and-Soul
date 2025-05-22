namespace Slash.Unity.DataBind.Core.Data
{
    using System;

    /// <summary>
    ///   Trigger to inform a context about a one shot event.
    /// </summary>
    public class DataTrigger
    {
        #region Events

        /// <summary>
        ///   Called when trigger was invoked.
        /// </summary>
        public event Action Invoked;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Invokes the trigger.
        /// </summary>
        public void Invoke()
        {
            this.OnInvoked();
        }

        #endregion

        #region Methods

        private void OnInvoked()
        {
            var handler = this.Invoked;
            if (handler != null)
            {
                handler();
            }
        }

        #endregion
    }
}