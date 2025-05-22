// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameObjectSingleSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    ///     Base class for a setter for a game object.
    /// </summary>
    /// <typeparam name="T">Type of data to set.</typeparam>
    public abstract class GameObjectSingleSetter<T> : SingleSetter<T>, ISerializationCallbackReceiver
    {
        /// <summary>
        ///     Target object.
        /// </summary>
        [DataTypeHintExplicit(typeof(GameObject))]
        public DataBinding TargetBinding;

        /// <summary>
        ///     For backward compatibility.
        /// </summary>
        [SerializeField]
        [FormerlySerializedAs("Target")]
        [HideInInspector]
        private GameObject constantTarget;

        /// <summary>
        ///     Current taret to set value to.
        /// </summary>
        public GameObject Target
        {
            get { return this.TargetBinding.GetValue<GameObject>(); }
        }

        /// <summary>
        ///     <para>
        ///         Implement this method to receive a callback after Unity deserializes your object.
        ///     </para>
        /// </summary>
        public void OnAfterDeserialize()
        {
            if (!this.IsTargetBindingSet())
            {
#if UNITY_EDITOR
                if (this.constantTarget == null)
                {
                    return;
                }
#endif
                this.TargetBinding = new DataBinding
                {
                    Type = DataBindingType.Reference,
                    Reference = this.constantTarget
                };
                this.constantTarget = null;
            }
        }

        /// <summary>
        ///     <para>
        ///         Implement this method to receive a callback before Unity serializes your object.
        ///     </para>
        /// </summary>
        public void OnBeforeSerialize()
        {
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();
            this.RemoveBinding(this.TargetBinding);
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.AddBinding(this.TargetBinding);
        }

        /// <inheritdoc />
        protected override void OnObjectValueChanged()
        {
            if (this.Target != null)
            {
                base.OnObjectValueChanged();
            }
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected void Reset()
        {
            if (!this.IsTargetBindingSet())
            {
                this.TargetBinding = new DataBinding {Type = DataBindingType.Reference, Reference = this.gameObject};
            }
        }

        private bool IsTargetBindingSet()
        {
            return
                !(this.TargetBinding == null
                  || this.TargetBinding.Type == DataBindingType.Context &&
                  string.IsNullOrEmpty(this.TargetBinding.Path));
        }
    }
}