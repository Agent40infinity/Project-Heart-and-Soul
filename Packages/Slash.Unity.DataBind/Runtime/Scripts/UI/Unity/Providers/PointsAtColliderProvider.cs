// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointsAtColliderProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Providers
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Provides a boolean data value which indicates if a camera is currently pointing at a collider.
    /// </summary>
    public class PointsAtColliderProvider : DataProvider
    {
        /// <summary>
        ///   Camera to check.
        /// </summary>
        public DataBinding CameraBinding;

        /// <summary>
        ///   Collider to check.
        /// </summary>
        public Collider Collider;

        /// <summary>
        ///   Maximum distance to raycast for hit.
        /// </summary>
        public float MaxDistance = 5000.0f;

        private bool wasHitting;

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.wasHitting;
            }
        }

        private Camera Camera
        {
            get
            {
                return this.CameraBinding.GetValue<Camera>();
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.CameraBinding);
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.CameraBinding);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Update()
        {
            var isHitting = this.CheckIfOnCanvas();
            if (isHitting != this.wasHitting)
            {
                this.wasHitting = isHitting;
                this.OnValueChanged();
            }
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }

        private bool CheckIfOnCanvas()
        {
            if (this.Camera == null || this.Collider == null)
            {
                return false;
            }

            var position = this.Camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

            var ray = this.Camera.ScreenPointToRay(position);
            RaycastHit hitInfo;
            return this.Collider.Raycast(ray, out hitInfo, this.MaxDistance);
        }
    }
}