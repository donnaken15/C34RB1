using System;

namespace DTBTool
{
	// Token: 0x0200000A RID: 10
	public class DTBTreeRoot : DTBTreeInnerNode
	{
		// Token: 0x06000015 RID: 21 RVA: 0x00002DF3 File Offset: 0x00000FF3
		public DTBTreeRoot()
		{
			this.Type = DTBType.InnerNode;
			this.LineNumber = 1;
			this.Version = 1;
		}

		// Token: 0x04000022 RID: 34
		public byte Version;
	}
}
