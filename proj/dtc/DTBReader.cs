using System;
using System.IO;
using System.Text;

namespace DTBTool
{
	// Token: 0x02000012 RID: 18
	public class DTBReader
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00003EEA File Offset: 0x000020EA
		public static void ReadRootNode(DTBTreeRoot Root, BinaryReader Input)
		{
			Root.Version = Input.ReadByte();
			if (Root.Version != 1)
			{
				throw new DTBException("The version of the DTB file is not 1");
			}
			DTBReader.ReadNodeList(Root, Input);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003F14 File Offset: 0x00002114
		private static void ReadNodeList(DTBTreeInnerNode InnerNode, BinaryReader Input)
		{
			int num = (int)Input.ReadInt16();
			InnerNode.LineNumber = Input.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				DTBTreeItem item = DTBReader.ReadNode(Input);
				InnerNode.SubNodes.Add(item);
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003F54 File Offset: 0x00002154
		private static DTBTreeItem ReadNode(BinaryReader Input)
		{
			DTBType dtbtype = (DTBType)Input.ReadInt32();
			Type classType = DTBTreeItem.GetClassType(dtbtype);
			if (classType == typeof(DTBTreeInteger))
			{
				return new DTBTreeInteger
				{
					Type = dtbtype,
					Integer = Input.ReadInt32()
				};
			}
			if (classType == typeof(DTBTreeFloat))
			{
				return new DTBTreeFloat
				{
					Type = dtbtype,
					Float = Input.ReadSingle()
				};
			}
			if (classType == typeof(DTBTreeString))
			{
				DTBTreeString dtbtreeString = new DTBTreeString();
				int num = Input.ReadInt32();
				byte[] array = new byte[num];
				Input.Read(array, 0, num);
				dtbtreeString.Type = dtbtype;
				dtbtreeString.String = Encoding.GetEncoding(1252).GetString(array);
				return dtbtreeString;
			}
			if (classType == typeof(DTBTreeInnerNode))
			{
				DTBTreeInnerNode dtbtreeInnerNode = new DTBTreeInnerNode();
				dtbtreeInnerNode.Type = dtbtype;
				DTBReader.ReadNodeList(dtbtreeInnerNode, Input);
				return dtbtreeInnerNode;
			}
			throw new DTBException("Unknown type \"" + dtbtype.ToString() + "\"");
		}
	}
}
