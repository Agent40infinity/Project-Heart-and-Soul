namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Set the fill amount of an Image depending on the data value,
    ///   smoothly changing it over time.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Image Fill Amount Smooth Setter (Unity)")]
    public class ImageFillAmountSmoothSetter : ImageFillAmountSetter
    {
        #region Fields

        /// <summary>
        ///   Amount to add/remove from image fill amount per second.
        /// </summary>
        public float ChangePerSecond = 1.0f;

        /// <summary>
        ///   When checked, instead reducing fill amounts, fills till 100% and then fills again to smaller value. 
        ///   Useful for level-ups in experience bars, for example.
        /// </summary>
        [Tooltip(
            "When checked, instead reducing fill amounts, fills till 100% and then fills again to smaller value. "
            + "Useful for level-ups in experience bars, for example.")]
        public bool NeverReduceFillAmount;

        private float targetValue;

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override void UpdateTargetValue(Image target, float value)
        {
            if (value < 0)
            {
                Debug.LogErrorFormat("Invalid image fill amount {0} for game object {1}.", value, this.gameObject);
                return;
            }

            this.targetValue = value;
        }

        private void Update()
        {
            var target = this.Target;
            if (target == null)
            {
                return;
            }

            var difference = this.targetValue - target.fillAmount;
            var maxDifference = Time.deltaTime * this.ChangePerSecond;
            var appliedChange = Mathf.Clamp(Mathf.Abs(difference), 0, maxDifference);
            
            if (this.NeverReduceFillAmount && difference < 0)
            {
                // Fill till 100%, then fill again to smaller value in next frame.
                target.fillAmount += maxDifference;

                if (target.fillAmount >= 1.0f)
                {
                    target.fillAmount = 0.0f;
                }
            }
            else
            {
                target.fillAmount += Mathf.Sign(difference) * appliedChange;
            }
        }

        #endregion
    }
}