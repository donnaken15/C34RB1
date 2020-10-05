using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DTBTool
{
	// Token: 0x02000019 RID: 25
	public class DTAParser
	{
		// Token: 0x06000054 RID: 84 RVA: 0x000052DC File Offset: 0x000034DC
		public DTAParser(TextReader _Input)
		{
			this.Input = _Input;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000052F8 File Offset: 0x000034F8
		public DTAParserToken GetToken()
		{
			DTAParserToken dtaparserToken = new DTAParserToken();
			for (;;)
			{
				if (this.IsEndOfLine())
				{
					if (!this.GetNextLine())
					{
						break;
					}
				}
				else if (char.IsWhiteSpace(this.Line[this.LinePos]))
				{
					this.LinePos++;
				}
				else
				{
					char c = this.Line[this.LinePos];
					if (c <= ';')
					{
						switch (c)
						{
						case '"':
						case '\'':
							goto IL_1B7;
						case '#':
						case '%':
						case '&':
							break;
						case '$':
							goto IL_22D;
						case '(':
						case ')':
							goto IL_D4;
						default:
							if (c == ';')
							{
								if (!this.GetNextLine())
								{
									goto Block_9;
								}
								continue;
							}
							break;
						}
					}
					else
					{
						switch (c)
						{
						case '[':
						case ']':
							goto IL_D4;
						case '\\':
							break;
						default:
							switch (c)
							{
							case '{':
							case '}':
								goto IL_D4;
							}
							break;
						}
					}
					dtaparserToken.Line = this.LineCount;
					dtaparserToken.Position = this.LinePos;
					dtaparserToken.Text = this.GetSimpleKeywordToken();
					if (!(dtaparserToken.Text == ""))
					{
						goto Block_15;
					}
				}
			}
			dtaparserToken.Status = DTAParserStatus.EOF;
			return dtaparserToken;
			Block_9:
			dtaparserToken.Status = DTAParserStatus.EOF;
			return dtaparserToken;
			IL_D4:
			char c2 = this.Line[this.LinePos];
			switch (c2)
			{
			case '(':
				dtaparserToken.Type = DTBType.InnerNode;
				break;
			case ')':
				dtaparserToken.Status = DTAParserStatus.InnerNodeEnd;
				dtaparserToken.Type = DTBType.InnerNode;
				break;
			default:
				switch (c2)
				{
				case '[':
					dtaparserToken.Type = DTBType.PropertyInnerNode;
					break;
				case '\\':
					break;
				case ']':
					dtaparserToken.Status = DTAParserStatus.PropertyInnerNodeEnd;
					dtaparserToken.Type = DTBType.PropertyInnerNode;
					break;
				default:
					switch (c2)
					{
					case '{':
						dtaparserToken.Type = DTBType.ScriptInnerNode;
						break;
					case '}':
						dtaparserToken.Status = DTAParserStatus.ScriptInnerNodeEnd;
						dtaparserToken.Type = DTBType.ScriptInnerNode;
						break;
					}
					break;
				}
				break;
			}
			dtaparserToken.Line = this.LineCount;
			dtaparserToken.Position = this.LinePos;
			dtaparserToken.Text = this.Line[this.LinePos++].ToString();
			return dtaparserToken;
			IL_1B7:
			char c3 = this.Line[this.LinePos];
			if (c3 != '"')
			{
				if (c3 == '\'')
				{
					dtaparserToken.Type = DTBType.Keyword;
				}
			}
			else
			{
				dtaparserToken.Type = DTBType.String;
			}
			dtaparserToken.Line = this.LineCount;
			dtaparserToken.Position = this.LinePos;
			dtaparserToken.Text = this.GetStringToken(this.Line[this.LinePos++]);
			return dtaparserToken;
			IL_22D:
			dtaparserToken.Type = DTBType.Variable;
			dtaparserToken.Line = this.LineCount;
			dtaparserToken.Position = this.LinePos;
			this.LinePos++;
			dtaparserToken.Text = this.GetSimpleKeywordToken();
			return dtaparserToken;
			Block_15:
			dtaparserToken.Type = this.GetSimpleKeywordTokenType(dtaparserToken.Text);
			return dtaparserToken;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000055BC File Offset: 0x000037BC
		private string GetSimpleKeywordToken()
		{
			string text = "";
			for (;;)
			{
				if (text.EndsWith("/*"))
				{
					text = text.Remove(text.Length - 2);
					int num;
					for (;;)
					{
						num = this.Line.IndexOf("*/", this.LinePos);
						if (num != -1)
						{
							break;
						}
						if (!this.GetNextLine())
						{
							goto Block_3;
						}
					}
					this.LinePos = num + 2;
				}
				if (this.IsEndOfLine() || char.IsWhiteSpace(this.Line[this.LinePos]) || this.Line[this.LinePos] == '(' || this.Line[this.LinePos] == ')' || this.Line[this.LinePos] == '{' || this.Line[this.LinePos] == '}' || this.Line[this.LinePos] == '[' || this.Line[this.LinePos] == ']')
				{
					return text;
				}
				if (char.IsControl(this.Line[this.LinePos]))
				{
					DTAParserWarning dtaparserWarning = new DTAParserWarning();
					dtaparserWarning.Line = this.LineCount;
					dtaparserWarning.Position = this.LinePos;
					dtaparserWarning.Text = "The character is a control character.";
					this.WarningList.Add(dtaparserWarning);
				}
				if (this.Line[this.LinePos] == '\'' || this.Line[this.LinePos] == '"')
				{
					DTAParserWarning dtaparserWarning2 = new DTAParserWarning();
					dtaparserWarning2.Line = this.LineCount;
					dtaparserWarning2.Position = this.LinePos;
					dtaparserWarning2.Text = "The character is likely to be a complex keyword or string terminator. Did you forget a initializer character?";
					this.WarningList.Add(dtaparserWarning2);
				}
				text += this.Line[this.LinePos++];
			}
			Block_3:
			throw new DTBException("The last multi-line comment is never closed.");
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000057B8 File Offset: 0x000039B8
		private DTBType GetSimpleKeywordTokenType(string Text)
		{
			switch (Text)
			{
			case "#ifdef":
				return DTBType.IfDef;
			case "#else":
				return DTBType.Else;
			case "#endif":
				return DTBType.EndIf;
			case "#define":
				return DTBType.Define;
			case "#include":
				return DTBType.Include;
			case "#merge":
				return DTBType.Merge;
			case "#ifndef":
				return DTBType.IfNDef;
			case "kDataUnhandled":
				return DTBType.kDataUnhandled;
			}
			int num2;
			if (Text.StartsWith("0x") || Text.StartsWith("0X"))
			{
				string s = Text.Substring(2);
				if (int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2))
				{
					return DTBType.Integer;
				}
			}
			if (int.TryParse(Text, out num2))
			{
				return DTBType.Integer;
			}
			float num3;
			if (float.TryParse(Text, NumberStyles.Float, CultureInfo.InvariantCulture, out num3))
			{
				return DTBType.Float;
			}
			return DTBType.Keyword;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000058F0 File Offset: 0x00003AF0
		private string GetStringToken(char EndChar)
		{
			string text = "";
			for (;;)
			{
				if (this.IsEndOfLine())
				{
					DTAParserWarning dtaparserWarning = new DTAParserWarning();
					dtaparserWarning.Line = this.LineCount;
					dtaparserWarning.Position = this.LinePos;
					dtaparserWarning.Text = "ERROR: A newline character has been found in the middle of a string. Check if you've closed the last string, or use the \\n escape sequence to insert a new line. (Note: This does not abort DTA parsing because some official files have this problem).";
					this.WarningList.Add(dtaparserWarning);
					if (!this.GetNextLine())
					{
						break;
					}
					text += "\r\n";
				}
				else
				{
					if (this.Line[this.LinePos] == EndChar)
					{
						goto Block_3;
					}
					if (this.Line[this.LinePos] == '\\')
					{
						this.LinePos++;
						if (this.IsEndOfLine())
						{
							goto Block_5;
						}
						char c = this.Line[this.LinePos];
						if (c != 'n')
						{
							if (c != 'q')
							{
								DTAParserWarning dtaparserWarning2 = new DTAParserWarning();
								dtaparserWarning2.Line = this.LineCount;
								dtaparserWarning2.Position = this.LinePos;
								dtaparserWarning2.Text = "ERROR: Unknown escape sequence, will copy the characters literally. (Note: This does not about DTA parsing because some official files have this problem).";
								this.WarningList.Add(dtaparserWarning2);
								text += '\\';
								text += this.Line[this.LinePos];
							}
							else
							{
								text += '"';
							}
						}
						else
						{
							text += '\n';
						}
						this.LinePos++;
					}
					else
					{
						if (char.IsControl(this.Line[this.LinePos]))
						{
							DTAParserWarning dtaparserWarning3 = new DTAParserWarning();
							dtaparserWarning3.Line = this.LineCount;
							dtaparserWarning3.Position = this.LinePos;
							dtaparserWarning3.Text = "The character is a control character.";
							this.WarningList.Add(dtaparserWarning3);
						}
						text += this.Line[this.LinePos++];
					}
				}
			}
			throw new DTBException("End of file found while reading a string or a keyword.");
			Block_3:
			this.LinePos++;
			return text;
			Block_5:
			throw new DTBException("End of line found while expecting a escape sequence.");
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00005AF2 File Offset: 0x00003CF2
		private bool IsEndOfLine()
		{
			return this.LineCount == 0 || this.Line.Length == this.LinePos;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00005B12 File Offset: 0x00003D12
		private bool GetNextLine()
		{
			this.Line = this.Input.ReadLine();
			if (this.Line == null)
			{
				return false;
			}
			this.LinePos = 0;
			this.LineCount++;
			return true;
		}

		// Token: 0x0400005C RID: 92
		private TextReader Input;

		// Token: 0x0400005D RID: 93
		private string Line;

		// Token: 0x0400005E RID: 94
		private int LinePos;

		// Token: 0x0400005F RID: 95
		private int LineCount;

		// Token: 0x04000060 RID: 96
		public List<DTAParserWarning> WarningList = new List<DTAParserWarning>();
	}
}
