namespace Slash.Unity.DataBind.Foundation.Providers.VR
{
    using System;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;
    using UnityEngine.Serialization;
#if UNITY_2017_2_OR_NEWER
    using InputTracking = UnityEngine.XR.InputTracking;
    using XRNode = UnityEngine.XR.XRNode;
#else
    using InputTracking = UnityEngine.VR.InputTracking;
    using XRNode = UnityEngine.VR.VRNode;

#endif

    /// <summary>
    ///     Provides the world position of a specific vr node.
    ///     See https://forum.unity3d.com/threads/world-positions-of-left-right-eye.350076/
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/VR/[DB] Input Tracking Position Provider")]
    public class InputTrackingPositionProvider : DataProvider
    {
        /// <summary>
        ///     Node to get local position for.
        /// </summary>
        [FormerlySerializedAs("VRNode")]
        public XRNode XRNode;

        private Vector3 position;

        /// <summary>
        ///     Indicates if dummy values should be used.
        ///     Useful for testing in editor.
        /// </summary>
        private bool useDummyValues;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return this.GetPosition();
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();

#if UNITY_EDITOR
            this.useDummyValues = true;
#else
            this.useDummyValues = false;
#endif
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected void Update()
        {
            var newPosition = this.GetPosition();
            if (this.position != newPosition)
            {
                this.position = newPosition;
                this.OnValueChanged();
            }
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }

        private Vector3 GetPosition()
        {
            if (this.useDummyValues)
            {
                switch (this.XRNode)
                {
                    case XRNode.LeftEye:
                        return new Vector3(-0.0315f, 0);
                    case XRNode.RightEye:
                        return new Vector3(0.0315f, 0);
                    case XRNode.CenterEye:
                        return new Vector3(0, 0);
                    case XRNode.Head:
                        return new Vector3(0, 0);
#if UNITY_5_5_OR_NEWER
                    case XRNode.LeftHand:
                        return new Vector3(0, 0);
                    case XRNode.RightHand:
                        return new Vector3(0, 0);
#endif
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Quaternion.Inverse(InputTracking.GetLocalRotation(this.XRNode)) *
                   InputTracking.GetLocalPosition(this.XRNode);
        }
    }
}