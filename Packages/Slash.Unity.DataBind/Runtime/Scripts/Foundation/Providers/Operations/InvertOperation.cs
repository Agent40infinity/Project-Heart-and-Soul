namespace Slash.Unity.DataBind.Foundation.Providers.Operations
{
    using Slash.Unity.DataBind.Core.Presentation;

    /// <summary>
    ///   Base class for inversion operations.
    /// </summary>
    public abstract class InvertOperation<TData> : DataProvider
    {
        /// <summary>
        ///   Binding to data to invert.
        /// </summary>
        [DataTypeHintGenericType]
        public DataBinding Data;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var value = this.Data.GetValue<TData>();
                return this.Invert(value);
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();
            this.RemoveBinding(this.Data);
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.AddBinding(this.Data);
        }

        /// <summary>
        ///   Inverts the specified value.
        /// </summary>
        /// <param name="value">Value to invert.</param>
        /// <returns>Inverted value.</returns>
        protected abstract TData Invert(TData value);

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}