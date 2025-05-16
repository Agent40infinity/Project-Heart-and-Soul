// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
	using UnityEngine;

	/// <summary>
	///     Setter which activates/deactivates a game object depending on the boolean data value.
	/// </summary>
	[AddComponentMenu("Data Bind/Foundation/Setters/[DB] Active Setter")]
	public class ActiveSetter : GameObjectSingleSetter<bool>
	{
		/// <summary>
		///     Indicates that setter doesn't get disabled when component is disabled.
		///     This way the active setter will still check the data value even if the game object
		///     the setter is on is disabled.
		/// </summary>
		[Tooltip(
			"Indicates that setter doesn't get disabled when component is disabled. This way the active setter will still check the data value even if the game object the setter is on is disabled.")]
		public bool DontDisableOnInactive;

		[Tooltip("Invert the value")]
		public bool Invert;

		/// <inheritdoc />
		public override void Disable()
		{
			if (!this.DontDisableOnInactive)
			{
				base.Disable();
			}
		}

		/// <inheritdoc />
		protected override void OnValueChanged(bool newValue)
		{
			this.Target.SetActive(Invert? !newValue : newValue);
		}
	}
}