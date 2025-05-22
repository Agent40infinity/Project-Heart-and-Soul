namespace Slash.Unity.DataBind.Foundation.Setters
{
    /// <summary>
    ///   Base class for a setter which uses a data property to determine if an item is shown
    ///   beneath the game object of the target component.
    /// </summary>
    public abstract class ItemSetter : SingleSetter
    {
        #region Fields

        private bool hasItem;

        #endregion

        #region Methods

        /// <summary>
        ///   Changes the context of an existing item.
        /// </summary>
        /// <param name="itemContext">New item context.</param>
        protected abstract void ChangeContext(object itemContext);

        /// <summary>
        ///   Clears created item.
        /// </summary>
        protected abstract void ClearItem();

        /// <summary>
        ///   Creates an item for the specified item context.
        /// </summary>
        /// <param name="itemContext">Item context for the item to create.</param>
        protected abstract void CreateItem(object itemContext);

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        protected override void OnObjectValueChanged()
        {
            var newValue = this.Data.Value;
            if (newValue == null)
            {
                // Clear item.
                this.ClearItem();
                this.hasItem = false;
            }
            else
            {
                if (!this.hasItem)
                {
                    // Create new item.
                    this.CreateItem(newValue);
                    this.hasItem = true;
                }
                else
                {
                    this.ChangeContext(newValue);
                }
            }
        }

        #endregion
    }
}