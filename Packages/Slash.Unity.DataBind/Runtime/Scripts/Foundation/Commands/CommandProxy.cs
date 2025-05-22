namespace Slash.Unity.DataBind.Foundation.Commands
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Proxy for a context command.
    ///   E.g. to switch between different commands depending on another provider.
    /// </summary>
    public class CommandProxy : DataBindingOperator
    {
        /// <summary>
        ///   Command this proxy is for.
        /// </summary>
        public DataBinding Command;

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();
            this.RemoveBinding(this.Command);
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.AddBinding(this.Command);
        }

        /// <summary>
        ///   Invokes the bound command.
        /// </summary>
        public void InvokeCommand()
        {
            this.InvokeCommand(new object[] { });
        }

        /// <summary>
        ///   Invokes the bound command with the specified argument.
        /// </summary>
        /// <param name="value">Command argument.</param>
        public void InvokeCommand(int value)
        {
            this.InvokeCommand(new object[] { value });
        }

        /// <summary>
        ///   Invokes the bound command with the specified argument.
        /// </summary>
        /// <param name="value">Command argument.</param>
        public void InvokeCommand(float value)
        {
            this.InvokeCommand(new object[] { value });
        }

        /// <summary>
        ///   Invokes the bound command with the specified argument.
        /// </summary>
        /// <param name="value">Command argument.</param>
        public void InvokeCommand(string value)
        {
            this.InvokeCommand(new object[] { value });
        }

        /// <summary>
        ///   Invokes the bound command with the specified argument.
        /// </summary>
        /// <param name="value">Command argument.</param>
        public void InvokeCommand(bool value)
        {
            this.InvokeCommand(new object[] { value });
        }

        /// <summary>
        ///   Invokes the bound command with the specified arguments.
        /// </summary>
        /// <param name="args">Arguments to invoke the bound command with.</param>
        public void InvokeCommand(params object[] args)
        {
            var command = this.Command.GetValue<Command>();
            if (command == null)
            {
                Debug.LogWarning("No command bound", this);
                return;
            }

            command.InvokeCommand(args);
        }
    }
}