namespace Slash.Unity.DataBind.Core.Data
{
    using System;
    using System.ComponentModel;

    /// <summary>
    ///     Value observer that works with <see cref="INotifyPropertyChanged" /> as a parent object.
    /// </summary>
    public class NotifyPropertyChangedNodeValueObserver : INodeValueObserver
    {
        private readonly INotifyPropertyChanged parentObject;

        private readonly string propertyName;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="parentObject">Parent object.</param>
        /// <param name="propertyName">Property name to observe.</param>
        public NotifyPropertyChangedNodeValueObserver(INotifyPropertyChanged parentObject, string propertyName)
        {
            this.propertyName = propertyName;
            this.parentObject = parentObject;
        }

        /// <inheritdoc />
        public event Action ValueChanged
        {
            add
            {
                if (this.valueChanged == null)
                {
                    if (this.parentObject != null)
                    {
                        this.parentObject.PropertyChanged += this.OnPropertyChanged;
                    }
                }

                this.valueChanged += value;
            }
            remove
            {
                this.valueChanged -= value;

                if (this.valueChanged == null)
                {
                    if (this.parentObject != null)
                    {
                        this.parentObject.PropertyChanged -= this.OnPropertyChanged;
                    }
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        private event Action valueChanged;

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.propertyName)
            {
                this.OnValueChanged();
            }
        }

        private void OnValueChanged()
        {
            var handler = this.valueChanged;
            if (handler != null)
            {
                handler();
            }
        }
    }
}