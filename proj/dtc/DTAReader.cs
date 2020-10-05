using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DTBTool
{
	// Token: 0x0200000B RID: 11
	public class DTAReader
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002E14 File Offset: 0x00001014
		public static List<DTAParserWarning> ReadRootNode(DTBTreeRoot Root, TextReader Input)
		{
			DTAParser dtaparser = new DTAParser(Input);
			DTAReader.ReadNodeList(Root, dtaparser, DTAParserStatus.EOF);
			return dtaparser.WarningList;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002E38 File Offset: 0x00001038
		private static void ReadNodeList(DTBTreeInnerNode InnerNode, DTAParser Parser, DTAParserStatus BreakAt)
		{
			DTBTreeItem dtbtreeItem;
			do
			{
				dtbtreeItem = DTAReader.ReadNode(Parser, BreakAt);
				if (dtbtreeItem != null)
				{
					InnerNode.SubNodes.Add(dtbtreeItem);
				}
			}
			while (dtbtreeItem != null);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002E60 File Offset: 0x00001060
		private static DTBTreeItem ReadNode(DTAParser Parser, DTAParserStatus BreakAt)
		{
			DTAParserToken token = Parser.GetToken();
			if (token.Status == BreakAt)
			{
				return null;
			}
			if (token.Status != DTAParserStatus.OK)
			{
				throw new DTBException(string.Concat(new string[]
				{
					"Unexpected end character (at line ",
					token.Line.ToString(),
					", position ",
					token.Position.ToString(),
					")."
				}));
			}
			DTBType type = token.Type;
			switch (type)
			{
			case DTBType.Integer:
			{
				DTBTreeInteger dtbtreeInteger = new DTBTreeInteger();
				dtbtreeInteger.Type = token.Type;
				if (token.Text.StartsWith("0x") || token.Text.StartsWith("0X"))
				{
					dtbtreeInteger.Integer = Convert.ToInt32(token.Text, 16);
				}
				else
				{
					dtbtreeInteger.Integer = Convert.ToInt32(token.Text, 10);
				}
				return dtbtreeInteger;
			}
			case DTBType.Float:
				return new DTBTreeFloat
				{
					Type = token.Type,
					Float = Convert.ToSingle(token.Text, CultureInfo.InvariantCulture)
				};
			case DTBType.Variable:
			case DTBType.Keyword:
			case DTBType.String:
				return new DTBTreeString
				{
					Type = token.Type,
					String = token.Text
				};
			case (DTBType)3:
			case (DTBType)4:
			case (DTBType)10:
			case (DTBType)11:
			case (DTBType)12:
			case (DTBType)13:
			case (DTBType)14:
			case (DTBType)15:
				goto IL_2F7;
			case DTBType.kDataUnhandled:
			case DTBType.Else:
			case DTBType.EndIf:
				return new DTBTreeInteger
				{
					Type = token.Type,
					Integer = 0
				};
			case DTBType.IfDef:
				break;
			case DTBType.InnerNode:
			case DTBType.ScriptInnerNode:
			case DTBType.PropertyInnerNode:
			{
				DTBTreeInnerNode dtbtreeInnerNode = new DTBTreeInnerNode();
				DTAParserStatus breakAt;
				switch (token.Type)
				{
				case DTBType.InnerNode:
					breakAt = DTAParserStatus.InnerNodeEnd;
					goto IL_2D0;
				case DTBType.ScriptInnerNode:
					breakAt = DTAParserStatus.ScriptInnerNodeEnd;
					goto IL_2D0;
				case DTBType.PropertyInnerNode:
					breakAt = DTAParserStatus.PropertyInnerNodeEnd;
					goto IL_2D0;
				}
				throw new DTBException("Internal error (DTA Reader: Unknown type).");
				IL_2D0:
				dtbtreeInnerNode.Type = token.Type;
				dtbtreeInnerNode.LineNumber = token.Line;
				DTAReader.ReadNodeList(dtbtreeInnerNode, Parser, breakAt);
				return dtbtreeInnerNode;
			}
			default:
				switch (type)
				{
				case DTBType.Define:
				case DTBType.Include:
				case DTBType.Merge:
				case DTBType.IfNDef:
					break;
				default:
					goto IL_2F7;
				}
				break;
			}
			DTBTreeString dtbtreeString = new DTBTreeString();
			DTAParserToken token2 = Parser.GetToken();
			if (token2.Status != DTAParserStatus.OK)
			{
				throw new DTBException(string.Concat(new string[]
				{
					"Invalid format found in a macro declaration (at line ",
					token.Line.ToString(),
					", position ",
					token.Position.ToString(),
					")."
				}));
			}
			if (token2.Type != DTBType.Keyword)
			{
				throw new DTBException(string.Concat(new string[]
				{
					"Macro name not followed by a keyword (at line ",
					token.Line.ToString(),
					", position ",
					token.Position.ToString(),
					")."
				}));
			}
			dtbtreeString.Type = token.Type;
			dtbtreeString.String = token2.Text;
			return dtbtreeString;
			IL_2F7:
			throw new DTBException("Unknown type \"" + token.Type.ToString() + "\"");
		}
	}
}
