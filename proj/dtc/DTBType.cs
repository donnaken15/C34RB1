using System;

namespace DTBTool
{
	// Token: 0x02000004 RID: 4
	public enum DTBType
	{
		// Token: 0x0400000B RID: 11
		Integer,
		// Token: 0x0400000C RID: 12
		Float,
		// Token: 0x0400000D RID: 13
		Variable,
		// Token: 0x0400000E RID: 14
		Keyword = 5,
		// Token: 0x0400000F RID: 15
		kDataUnhandled,
		// Token: 0x04000010 RID: 16
		IfDef,
		// Token: 0x04000011 RID: 17
		Else,
		// Token: 0x04000012 RID: 18
		EndIf,
		// Token: 0x04000013 RID: 19
		InnerNode = 16,
		// Token: 0x04000014 RID: 20
		ScriptInnerNode,
		// Token: 0x04000015 RID: 21
		String,
		// Token: 0x04000016 RID: 22
		PropertyInnerNode,
		// Token: 0x04000017 RID: 23
		Define = 32,
		// Token: 0x04000018 RID: 24
		Include,
		// Token: 0x04000019 RID: 25
		Merge,
		// Token: 0x0400001A RID: 26
		IfNDef
	}
}
