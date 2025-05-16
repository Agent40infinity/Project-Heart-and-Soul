// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Context.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Data
{
	using System;
	using Slash.Unity.DataBind.Core.Utils;

	/// <summary>
	///     Base class for a data context which contains properties to bind to.
	/// </summary>
	public abstract class Context : IDataContext
	{
		private readonly DataTree dataTree;

		/// <summary>
		///     Constructor.
		/// </summary>
		protected Context()
		{
			var typeInfo = new ObjectNodeTypeInfo(this);
			var rootNode = new BranchDataNode(typeInfo, "Context");
			this.dataTree = new DataTree(rootNode);
		}

		/// <summary>
		///     Constructor.
		/// </summary>
		/// <param name="rootObject">Root of data.</param>
		protected Context(object rootObject)
		{
			var typeInfo = new ObjectNodeTypeInfo(rootObject);
			var rootNode = new BranchDataNode(typeInfo, "Context");
			this.dataTree = new DataTree(rootNode);
		}

		/// <inheritdoc />
		public object GetValue(string path)
		{
			return this.dataTree.GetValue(path);
		}

		/// <inheritdoc />
		public object RegisterListener(string path, Action<object> onValueChanged)
		{
			return this.dataTree.RegisterListener(path, onValueChanged);
		}

		/// <inheritdoc />
		public void RemoveListener(string path, Action<object> onValueChanged)
		{
			this.dataTree.RemoveListener(path, onValueChanged);
		}

		/// <inheritdoc />
		public void SetValue(string path, object value)
		{
			this.dataTree.SetValue(path, value);
		}
	}

	/// <summary>
	/// Context that is aware of the game object that is attached to (@HACK MVVM pattern breaking hack...)
	/// </summary>
	public abstract class LinkedContext : Context
	{
		protected UnityEngine.GameObject owningGameObject = null;


		public UnityEngine.GameObject GetOwningObject() => owningGameObject;
		
		public void SetOwningObject(UnityEngine.GameObject newOwner)
		{
			owningGameObject = newOwner;
		}

	}
}