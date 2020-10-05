using System;
using System.IO;
using System.Text;

namespace DTBTool
{
	// Token: 0x02000011 RID: 17
	public class DTBWriter
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00003D90 File Offset: 0x00001F90
		public static void WriteRootNode(DTBTreeRoot Root, BinaryWriter Output)
		{
			Output.Write(Root.Version);
			DTBWriter.WriteNodeList(Root, Output);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00003DA8 File Offset: 0x00001FA8
		private static void WriteNodeList(DTBTreeInnerNode InnerNode, BinaryWriter Output)
		{
			if (InnerNode.SubNodes.Count >= 65536)
			{
				throw new DTBException("An inner node has more than 0x10000 subnodes");
			}
			Output.Write((ushort)InnerNode.SubNodes.Count);
			Output.Write(InnerNode.LineNumber);
			foreach (DTBTreeItem node in InnerNode.SubNodes)
			{
				DTBWriter.WriteNode(node, Output);
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003E38 File Offset: 0x00002038
		private static void WriteNode(DTBTreeItem Node, BinaryWriter Output)
		{
			Output.Write((int)Node.Type);
			if (Node is DTBTreeInteger)
			{
				DTBTreeInteger dtbtreeInteger = (DTBTreeInteger)Node;
				Output.Write(dtbtreeInteger.Integer);
				return;
			}
			if (Node is DTBTreeFloat)
			{
				DTBTreeFloat dtbtreeFloat = (DTBTreeFloat)Node;
				Output.Write(dtbtreeFloat.Float);
				return;
			}
			if (Node is DTBTreeString)
			{
				DTBTreeString dtbtreeString = (DTBTreeString)Node;
				byte[] bytes = Encoding.GetEncoding(1252).GetBytes(dtbtreeString.String);
				Output.Write(bytes.Length);
				Output.Write(bytes);
				return;
			}
			if (Node is DTBTreeInnerNode)
			{
				DTBTreeInnerNode innerNode = (DTBTreeInnerNode)Node;
				DTBWriter.WriteNodeList(innerNode, Output);
				return;
			}
			throw new DTBException("Internal Error (DTB Writer: Unknown type).");
		}
	}
}
