namespace Slash.Unity.DataBind.Core.Data
{
    using Slash.Unity.DataBind.Core.Utils;

    /// <summary>
    ///     A data context node which doesn't have any children.
    /// </summary>
    public class LeafDataNode : DataNode
    {
        /// <inheritdoc />
        public LeafDataNode(NodeTypeInfo typeInfo, IDataNode parentNode, string name) : base(typeInfo,
            parentNode, name)
        {
        }

        /// <inheritdoc />
        public LeafDataNode(NodeTypeInfo typeInfo, string name) : base(typeInfo, name)
        {
        }

        /// <inheritdoc />
        public override IDataNode FindDescendant(string path)
        {
            // No children.
            return null;
        }
    }
}