using System;
using System.Collections.Generic;
using System.IO;

namespace DTBTool
{
	// Token: 0x02000014 RID: 20
	public class DTBDebugger
	{
		// Token: 0x0600004A RID: 74 RVA: 0x0000508D File Offset: 0x0000328D
		public static void Print(DTBTreeItem Node)
		{
			DTBDebugger.Print(Node, Console.Out, 0);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000509B File Offset: 0x0000329B
		public static void Print(DTBTreeItem Node, TextWriter Output)
		{
			DTBDebugger.Print(Node, Output, 0);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000050A8 File Offset: 0x000032A8
		public static void Print(DTBTreeItem Node, TextWriter Output, int TabCount)
		{
			for (int i = 0; i < TabCount; i++)
			{
				Output.Write("\t");
			}
			if (Node is DTBTreeInteger)
			{
				DTBTreeInteger dtbtreeInteger = (DTBTreeInteger)Node;
				Output.WriteLine("DTBTreeInteger = " + dtbtreeInteger.Integer.ToString());
				return;
			}
			if (Node is DTBTreeFloat)
			{
				DTBTreeFloat dtbtreeFloat = (DTBTreeFloat)Node;
				Output.WriteLine("DTBTreeFloat = " + dtbtreeFloat.Float.ToString());
				return;
			}
			if (Node is DTBTreeString)
			{
				DTBTreeString dtbtreeString = (DTBTreeString)Node;
				Output.WriteLine("DTBTreeString = " + dtbtreeString.String.ToString());
				return;
			}
			if (Node is DTBTreeInnerNode)
			{
				DTBTreeInnerNode dtbtreeInnerNode = (DTBTreeInnerNode)Node;
				Output.WriteLine("DTBTreeInnerNode > Line " + dtbtreeInnerNode.LineNumber.ToString());
				using (List<DTBTreeItem>.Enumerator enumerator = dtbtreeInnerNode.SubNodes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DTBTreeItem node = enumerator.Current;
						DTBDebugger.Print(node, Output, TabCount + 1);
					}
					return;
				}
			}
			throw new DTBException("Internal Error (DTB Debugger: Unknown type).");
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000051D4 File Offset: 0x000033D4
		public static void ShowTokens(TextReader Input, TextWriter Output)
		{
			DTAParser dtaparser = new DTAParser(Input);
			for (;;)
			{
				DTAParserToken token = dtaparser.GetToken();
				if (token.Status == DTAParserStatus.EOF)
				{
					break;
				}
				Output.WriteLine("Token Status: " + token.Status.ToString());
				Output.WriteLine("Token Type: " + token.Type.ToString());
				Output.WriteLine("Token Line: " + token.Line.ToString());
				Output.WriteLine("Token Position: " + token.Position.ToString());
				Output.WriteLine("Token Text: \"" + token.Text + "\"");
				Output.WriteLine();
			}
		}
	}
}
