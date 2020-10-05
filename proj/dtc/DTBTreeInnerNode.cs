using System;
using System.Collections.Generic;

namespace DTBTool
{
	// Token: 0x02000009 RID: 9
	public class DTBTreeInnerNode : DTBTreeItem
	{
		// Token: 0x04000020 RID: 32
		public int LineNumber = 1;

		// Token: 0x04000021 RID: 33
		public List<DTBTreeItem> SubNodes = new List<DTBTreeItem>();
	}
}
