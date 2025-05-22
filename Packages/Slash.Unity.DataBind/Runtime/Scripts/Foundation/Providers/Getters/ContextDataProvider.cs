namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;
    using UnityEngine;

    /// <summary>
    ///     Base class to get a data value from a bound context.
    /// </summary>
    public class ContextDataProvider : DataProvider
    {
        /// <summary>
        ///     Data binding for context to get data from.
        /// </summary>
        [Tooltip("Data binding for context to get data from")]
        public DataBinding Context;

        /// <summary>
        ///     Type of context the binding provides.
        ///     Used to show the path drop down.
        /// </summary>
        [ContextType]
        [Tooltip("Type of context the binding provides.")]
        public string ContextType;

        private Context currentContext;

        /// <summary>
        ///     Current data value.
        /// </summary>
        private object data;

        /// <summary>
        ///     Path to get data from context from.
        /// </summary>
        [Tooltip("Path to get data from context from")]
        [ContextPath(Filter = ~ContextMemberFilter.Methods | ~ContextMemberFilter.Contexts)]
        public string Path;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return this.data;
            }
        }

        private Context CurrentContext
        {
            get
            {
                return this.currentContext;
            }
            set
            {
                if (value == this.currentContext)
                {
                    return;
                }

                if (this.currentContext != null)
                {
                    this.currentContext.RemoveListener(this.Path, this.OnContextValueChanged);
                }

                this.currentContext = value;

                if (this.currentContext != null)
                {
                    this.currentContext.RegisterListener(this.Path, this.OnContextValueChanged);
                }

                this.OnContextValueChanged(this.currentContext != null
                    ? this.currentContext.GetValue(this.Path)
                    : null);
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();
            this.RemoveBinding(this.Context);
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.AddBinding(this.Context);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.CurrentContext = this.Context.GetValue<Context>();
        }

        private void OnContextValueChanged(object newData)
        {
            this.data = newData;
            this.OnValueChanged();
        }
    }
}