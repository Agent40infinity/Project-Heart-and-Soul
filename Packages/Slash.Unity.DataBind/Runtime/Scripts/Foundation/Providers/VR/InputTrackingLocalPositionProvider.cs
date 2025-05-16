namespace Slash.Unity.DataBind.Foundation.Providers.VR
{
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
    ///   Provides the local position of a specific vr node.
    ///   See https://docs.unity3d.com/ScriptReference/VR.InputTracking.GetLocalPosition.html
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/VR/[DB] Input Tracking Local Position Provider")]
    public class InputTrackingLocalPositionProvider : DataProvider
    {
        /// <summary>
        ///   Node to get local position for.
        /// </summary>
        [FormerlySerializedAs("VRNode")]
        public XRNode XRNode;

        private Vector3 position;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return this.GetPosition();
            }
        }

        /// <summary>
        ///   Unity callback.
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
            return InputTracking.GetLocalPosition(this.XRNode);
        }
    }
}