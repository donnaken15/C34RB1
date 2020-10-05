using System;

namespace DTBTool
{
	// Token: 0x02000005 RID: 5
	public class DTBTreeItem
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002CF8 File Offset: 0x00000EF8
		public static Type GetClassType(DTBType _Type)
		{
			switch (_Type)
			{
			case DTBType.Integer:
			case DTBType.kDataUnhandled:
			case DTBType.Else:
			case DTBType.EndIf:
				return typeof(DTBTreeInteger);
			case DTBType.Float:
				return typeof(DTBTreeFloat);
			case DTBType.Variable:
			case DTBType.Keyword:
			case DTBType.IfDef:
			case DTBType.String:
				break;
			case (DTBType)3:
			case (DTBType)4:
			case (DTBType)10:
			case (DTBType)11:
			case (DTBType)12:
			case (DTBType)13:
			case (DTBType)14:
			case (DTBType)15:
				goto IL_9F;
			case DTBType.InnerNode:
			case DTBType.ScriptInnerNode:
			case DTBType.PropertyInnerNode:
				return typeof(DTBTreeInnerNode);
			default:
				switch (_Type)
				{
				case DTBType.Define:
				case DTBType.Include:
				case DTBType.Merge:
				case DTBType.IfNDef:
					break;
				default:
					goto IL_9F;
				}
				break;
			}
			return typeof(DTBTreeString);
			IL_9F:
			throw new DTBException("Internal error (DTBTreeItem: Unknown type).");
		}

		// Token: 0x0400001B RID: 27
		public DTBType Type;
	}
}
