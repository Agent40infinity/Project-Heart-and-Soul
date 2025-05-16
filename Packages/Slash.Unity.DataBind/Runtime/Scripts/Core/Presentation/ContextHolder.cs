// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextHolder.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
	using System;
	using System.ComponentModel;
	using Slash.Unity.DataBind.Core.Data;
	using Slash.Unity.DataBind.Core.Utils;
	using UnityEngine;

	/// <summary>
	///     Holds a data context to specify the context to use on the presentation side.
	/// </summary>
	[AddComponentMenu("Data Bind/Core/[DB] Context Holder")]
	public class ContextHolder : MonoBehaviour
	{
		/// <summary>
		///     Delegate for ContextChanged event.
		/// </summary>
		/// <param name="newContext">New context.</param>
		public delegate void ContextChangedDelegate(object newContext);

		private object context;

		[SerializeField]
		[ContextType]
		[Tooltip("Type of context this holder expects.")]
#pragma warning disable 649
		private string contextType;

#pragma warning restore 649

		/// <summary>
		///     Should a context of the specified type be created at startup?
		/// </summary>
		[SerializeField]
		[Tooltip("Create context on startup?")]
		private bool createContext;

		/// <summary>
		///     Called when the context of this holder changed.
		/// </summary>
		public event ContextChangedDelegate ContextChanged;

		/// <summary>
		///     Data context.
		/// </summary>
		public object Context
		{
			get
			{
				return this.context;
			}
			set
			{
				this.SetContext(value, null);
			}
		}

		/// <summary>
		///     Type of context to create on startup.
		/// </summary>
		public Type ContextType
		{
			get
			{
				try
				{
					return this.contextType != null ? ReflectionUtils.FindType(this.contextType) : null;
				}
				catch (TypeLoadException)
				{
					Debug.LogError("Can't find context type '" + this.contextType + "'.", this);
					return null;
				}
			}
		}

		/// <summary>
		///     Indicates if a context should be created from the specified context type.
		/// </summary>
		public bool CreateContext
		{
			get
			{
				return this.createContext;
			}
			set
			{
				this.createContext = value;
			}
		}

		/// <summary>
		///     Path from parent to the context.
		///     Used to resolve relative paths.
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		///     Sets the context and its path from the parent to the context.
		/// </summary>
		/// <param name="newContext">Context.</param>
		/// <param name="path">Path from the parent context to the specified one.</param>
		public void SetContext(object newContext, string path)
		{
			if (newContext == this.context && path == this.Path)
			{
				return;
			}

			// Custom - pass the game object on to the linked context
			if (newContext is LinkedContext linkedContext)
				linkedContext.SetOwningObject(gameObject);

			var newNotifyPropertyChangedContext = newContext as INotifyPropertyChanged;
			if (newNotifyPropertyChangedContext != null)
			{
				// Wrap INotifyPropertyChanged classes.
				newContext = new NotifyPropertyChangedDataContext(newNotifyPropertyChangedContext);
			}

			this.context = newContext;
			this.Path = path;

			this.NotifyContextOperatorsAboutContextChange();

			this.OnContextChanged();
		}

		/// <summary>
		///     Unity callback.
		/// </summary>
		protected virtual void Awake()
		{
			if (this.Context == null && this.ContextType != null && this.CreateContext)
			{
				var newContext = Activator.CreateInstance(this.ContextType);
				this.SetContext(newContext, null);
			}
		}

		/// <summary>
		///     Notifies the context operators which depend on this context holder about a context change.
		/// </summary>
		protected virtual void NotifyContextOperatorsAboutContextChange()
		{
			// Update child bindings as context changed.
			var contextOperators = this.gameObject.GetComponentsInChildren<IContextOperator>(true);
			foreach (var contextOperator in contextOperators)
			{
				contextOperator.OnContextChanged();
			}
		}

		/// <summary>
		///     Called when the context of this holder changed.
		/// </summary>
		protected virtual void OnContextChanged()
		{
			var handler = this.ContextChanged;
			if (handler != null)
			{
				handler(this.Context);
			}
		}
	}
}