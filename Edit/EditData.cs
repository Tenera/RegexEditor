//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
//  Copyright Syncfusion Inc. 2001 - 2003. All rights reserved. Use of this code is subject to the terms of our 
//  license. A copy of the current license can be obtained at any time by e-mailing licensing@syncfusion.com. 
//  Re-distribution in any form is strictly prohibited. Any infringement will be prosecuted under applicable laws. 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 
/*******************************************************************************
*                    Essential Edit - A syntax coloring edit                   *
*                                Author: B. Wu                                 *
********************************************************************************/

using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditData class represents the data model used by EditControl.
	/// </summary>
	internal class EditData
	{
		#region Data Members

		/// <summary>
		/// The associated EditControl object.
		/// </summary>
		private EditControl edit;
		/// <summary>
		/// The associated EditSettings object. 
		/// </summary>
		private EditSettings editSettings;

		/// <summary>
		/// The buffer for text and coloring information.
		/// </summary>
		internal EditLineList LineList;
		/// <summary>
		/// The list of multiline blocks of tagged text.
		/// </summary>
		internal EditMultiLineBlockList MultiLineBlockList;

		/// <summary>
		/// The RegexOptions object used in Regex matching.
		/// </summary>
		internal RegexOptions RegExpOptions = RegexOptions.Singleline;
		/// <summary>
		/// The Regex object for the language.
		/// </summary>
		internal Regex LangRegex = null;
		/// <summary>
		/// The Regex object for the tags that start either a single-line 
		/// or a multiline block of tagged text.
		/// </summary>
		internal Regex BeginTagCheckingRegex = null;
		/// <summary>
		/// The Regex object for the tags that start a multiline block of
		/// tagged text.
		/// </summary>
		internal Regex MultiLineBeginTagsRegex = null;

		/// <summary>
		/// The Regex objects for outlining parsing.
		/// </summary>
		internal Regex SingleLineCommentRegex = null;
		internal Regex SingleLineOutliningRegex = null;
		internal Regex MultiLineOutliningBeginTagCheckingRegex = null;

		/// <summary>
		/// The temporary Match object.
		/// </summary>
		private Match editMatch;
		/// <summary>
		/// The temporary MatchCollection object.
		/// </summary>
		private MatchCollection editMatches;
		/// <summary>
		/// The temporary EditColorInfoList object.
		/// </summary>
		private EditColorInfoList editColorInfoListTemp;

		/// <summary>
		/// The root of outlining objects.
		/// </summary>
		internal EditOutlining OutliningRoot;
		/// <summary>
		/// The current outlining.
		/// </summary>
		private EditOutlining editCurrentOutlining;
		/// <summary>
		/// The last line that outlining information has been updated to.
		/// </summary>
		private int editOutliningLineUpdated;

		/// <summary>
		/// The thread for background updating of outlining information.
		/// </summary>
		private Thread editOutliningThread;
		/// <summary>
		/// The ThreadStart function for the background outlining thread.
		/// </summary>
		private ThreadStart editOutliningThreadStart;
		/// <summary>
		/// A variable indicating whether to continue background outlining 
		/// processing.
		/// </summary>
		private bool bOutliningThreadContinue;

		internal static readonly EditOutliningTagInfo CommentOutliningTag = 
			new EditOutliningTagInfo("comment", string.Empty, string.Empty, 
			string.Empty, string.Empty, string.Empty, string.Empty, null);

		internal static readonly EditOutliningTagInfo ManualOutliningTag = 
			new EditOutliningTagInfo("manual", string.Empty, string.Empty, 
			string.Empty, string.Empty, string.Empty, string.Empty, null);

		#endregion

		#region Methods

		/// <summary>
		/// Creates a new EditData object associated with the specified 
		/// EditControl object.
		/// </summary>
		/// <param name="edit">The associated EditControl object.</param>
		internal EditData(EditControl edit)
		{
			this.edit = edit;
			this.editSettings = edit.Settings;
			OutliningRoot = new EditOutlining(this,  
				new EditLocation(0, 0), new EditLocation(-1, -1), null);
			LineList = new EditLineList();
			MultiLineBlockList = new EditMultiLineBlockList(this);
			editCurrentOutlining = OutliningRoot;
			editColorInfoListTemp = new EditColorInfoList();
			editOutliningLineUpdated = 0;
			editOutliningThreadStart = new ThreadStart(UpdateOutliningInfo);
			editOutliningThread = new Thread(editOutliningThreadStart);
			editOutliningThread.Priority = ThreadPriority.Lowest;
		}

		/// <summary>
		/// Clears all the stored data.
		/// </summary>
		internal void Clear()
		{
			OutliningRoot.Clear();
			MultiLineBlockList.Clear();
			LineList.Clear();
			editCurrentOutlining = OutliningRoot;
			editOutliningLineUpdated = 0;
		}

		/// <summary>
		/// Tests if the specified line number is valid.
		/// </summary>
		/// <param name="ln">The line number to be tested.</param>
		/// <returns>true if the line number is valid; otherwise, false.
		/// </returns>
		internal bool IsValidLine(int ln)
		{
			return ((ln >= 1) && (ln <= LineList.Count));
		}

		/// <summary>
		/// Tests if the specified location is valid.
		/// </summary>
		/// <param name="ln">The line of the location to be tested.</param>
		/// <param name="ch">The char of the location to be tested.</param>
		/// <returns>true if the location is valid; otherwise, false.
		/// </returns>
		internal bool IsValidLineChar(int ln, int ch)
		{
			if (!IsValidLine(ln))
			{
				return false;
			}
			if (ch < 1)
			{
				return false;
			}
			if (ch > GetLineLengthPlusOne(ln))
			{
				return edit.VirtualSpace;
			}
			return true;
		}

		/// <summary>
		/// Gets the number of occupied lines for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range.</param>
		/// <param name="lnEnd">The ending line of the line range.</param>
		/// <returns>The number of visible lines in the line range.</returns>
		internal int GetOccupiedLineCount(int lnStart, int lnEnd)
		{
			int count = 0;
			int lnStartTemp = Math.Max(1, lnStart);
			int lnEndTemp = Math.Min(LineList.Count, lnEnd);
			if (!edit.OutliningEnabled)
			{
				return (lnEndTemp - lnStartTemp + 1);
			}
			for (int i = lnStartTemp; i <= lnEndTemp; i++)
			{
				if (!LineList[i-1].Hidden)
				{
					count += LineList[i-1].LinesOccupied;	
				}
			}
			return count;
		}

		/// <summary>
		/// Gets the stored line index for the specified visible line index.
		/// </summary>
		/// <param name="ln">The visible line index for which the stored line
		/// index is to be obtained.</param>
		/// <returns>The stored line index of the visible line index.
		/// </returns>
		internal int GetStoredLineIndex(int ln)
		{
			if (!edit.OutliningEnabled)
			{
				return ln;
			}
			int lnLast = LineList.Count;
			int lnTemp = 0;
			for (int i = 1; i <= lnLast; i++)
			{
				lnTemp += LineList[i-1].Hidden ? 
					0 : (int)LineList[i-1].LinesOccupied;
				if (lnTemp == ln)
				{
					return i;
				}
			}
			return lnLast;
		}

		/// <summary>
		/// Gets the visible line index for the specified stored line index.
		/// </summary>
		/// <param name="ln">The stored line index for which the visible line
		/// index is to be obtained.</param>
		/// <returns>The visible line index for the stored line index.
		/// </returns>
		internal int GetOccupiedLineIndex(int ln)
		{
			if (!edit.OutliningEnabled)
			{
				return ln;
			}
			if (IsValidLine(ln))
			{
				return GetOccupiedLineCount(1, ln);
			}
			return 1;
		}

		/// <summary>
		/// Gets the line visible at the specified offset from the specified 
		/// stored line.
		/// </summary>
		/// <param name="ln">The stored line from which the visible line is 
		/// to be obtained.</param>
		/// <param name="offset">The offset in visible lines.</param>
		/// <returns>The visible line at the specified offset from the 
		/// specified stored line.</returns>
		internal int GetVisibleLineByOffset(int ln, int offset)
		{
			int lnTemp = ln;
			if (offset > 0)
			{
				for (int i = 0; i < offset; i++)
				{
					lnTemp = GetNextVisibleLine(lnTemp);
				}
			}
			else
			{
				for (int i = 0; i > offset; i--)
				{
					lnTemp = GetPreviousVisibleLine(lnTemp);
				}
			}
			return lnTemp;
		}

		/// <summary>
		/// Gets the line at which the specified line is displayed.
		/// </summary>
		/// <param name="ln">The line for which the located line is to be 
		/// obtained.</param>
		/// <returns>The line at which the specified line is displayed.
		/// </returns>
		internal int GetLocatedLine(int ln)
		{
			if (!edit.OutliningEnabled)
			{
				return ln;
			}
			else
			{
				if (LineList[ln-1].Hidden)
				{
					return GetPreviousVisibleLine(ln);
				}
				else
				{
					return ln;
				}
			}
		}

		/// <summary>
		/// Gets the previous visible line before the specified line.
		/// </summary>
		/// <param name="ln">The line for which the previous visible line
		/// is to be obtained.</param>
		/// <returns>The previous visible line before the specified line.
		/// </returns>
		internal int GetPreviousVisibleLine(int ln)
		{
			if (ln <= 1)
			{
				return 1;
			}
			if (!edit.OutliningEnabled)
			{
				return (ln - 1);
			}
			else
			{
				for (int i = ln - 1; i >= 1; i--)
				{
					if (!LineList[i-1].Hidden)
					{
						return i;
					}
				}
				return ln;
			}
		}

		/// <summary>
		/// Gets the next visible line after the specified line.
		/// </summary>
		/// <param name="ln">The line for which the next visible line is 
		/// to be obtained.</param>
		/// <returns>The next visible line after the specified line.
		/// </returns>
		internal int GetNextVisibleLine(int ln)
		{
			if (ln >= LineList.Count)
			{
				return LineList.Count;
			}
			if (!edit.OutliningEnabled)
			{
				return (ln + 1);
			}
			else
			{
				int lnLast = LineList.Count;
				for (int i = ln + 1; i <= lnLast; i++)
				{
					if (!LineList[i-1].Hidden)
					{
						return i;
					}
				}
				return ln;
			}
		}

		/// <summary>
		/// Gets the string object for the specified line.
		/// </summary>
		/// <param name="ln">The line at which the string object is located.
		/// </param>
		/// <returns>The string object of the line.</returns>
		internal string GetStringObject(int ln)
		{
			if (IsValidLine(ln))
			{
				return LineList[ln-1].LineString;
			}
			else
			{
				return string.Empty;
			}
		}

		/// <overload>
		/// Gets the string at the specified location.
		/// </overload>
		/// <summary>
		/// Gets the string at the specified line (without the trailing 
		/// newline characters).
		/// </summary>
		/// <param name="ln">The line at which the string is located.</param>
		/// <returns>The string at the line.</returns>
		internal string GetString(int ln)
		{
			if (!IsValidLine(ln))
			{
				return string.Empty;
			}
			return string.Copy(LineList[ln-1].LineString);
		}

		/// <summary>
		/// Gets the whole string (with trailing line break characters) at 
		/// the specified line.
		/// </summary>
		/// <param name="ln">The line for which the whole string is to be
		/// obtained.</param>
		/// <returns>The whole string at the line.</returns>
		internal string GetWholeString(int ln)
		{
			if (!IsValidLine(ln))
			{
				return string.Empty;
			}
			if (ln == LineList.Count)
			{
				return string.Copy(LineList[ln-1].LineString);
			}
			else 
			{
				return (LineList[ln-1].LineString + editSettings.NewLine);
			}
		}

		/// <summary>
		/// Gets the text in the specified location range.
		/// </summary>
		/// <param name="lnStart">The line of the starting location of the 
		/// location range for which the text is to be obtained.</param>
		/// <param name="chStart">The char of the starting location of the 
		/// location range for which the text is to be obtained.</param>
		/// <param name="lnEnd">The line of the ending location of the 
		/// location range for which the text is to be obtained.</param>
		/// <param name="chEnd">The char of the ending location of the 
		/// location range for which the text is to be obtained.</param>
		/// <returns>The text in the location range.</returns>
		internal string GetString(int lnStart, int chStart, int lnEnd, int chEnd)
		{
			EditLocationRange lcrNorm = 
				(new EditLocationRange(lnStart, chStart, lnEnd, chEnd)).Normalize();
			if (lcrNorm.Start == lcrNorm.End)
			{
				return string.Empty;
			}
			if ((!IsValidLineChar(lcrNorm.Start.L, lcrNorm.Start.C)) 
				|| (!IsValidLineChar(lcrNorm.End.L, lcrNorm.End.C)))
			{
				return string.Empty;
			}
			else if (lcrNorm.Start.L == lcrNorm.End.L)
			{
				int lnLength = GetLineLength(lcrNorm.Start.L);
				if (lnLength == 0)
				{
					return string.Empty;
				}
				return GetStringObject(lcrNorm.Start.L).Substring(lcrNorm.
					Start.C - 1, Math.Min(lnLength + 1, lcrNorm.End.C) 
					- lcrNorm.Start.C);
			}
			else
			{
				StringBuilder sbTemp = new StringBuilder(string.Empty);
				sbTemp.Append(GetWholeString(lcrNorm.Start.L).
					Substring(lcrNorm.Start.C - 1));
				for (int i = lcrNorm.Start.L + 1; i < lcrNorm.End.L; i++)
				{
					sbTemp.Append(GetWholeString(i));
				}
				int lnLength = GetLineLength(lcrNorm.End.L);
				if (lnLength > 0)
				{
					sbTemp.Append(GetStringObject(lcrNorm.End.L).Substring(0, 
						Math.Min(lnLength + 1, lcrNorm.End.C) - 1));
				}
				return sbTemp.ToString();
			}
		}

		/// <summary>
		/// Gets the length of the string at the specified line.
		/// </summary>
		/// <param name="ln">The line for which the string length is to be 
		/// obtained.</param>
		/// <returns>The string length of the line.</returns>
		internal int GetLineLength(int ln)
		{
			return LineList[ln-1].LineString.Length;
		}

		/// <summary>
		/// Gets the length of the whole string at the specified line.
		/// </summary>
		/// <param name="ln">The line for which the whole string length is 
		/// to be obtained.</param>
		/// <returns>The whole string length of the line.</returns>
		internal int GetWholeLineLength(int ln)
		{
			return GetWholeString(ln).Length;
		}

		/// <summary>
		/// Gets the ending char index for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the ending char index 
		/// is to be obtained.</param>
		/// <returns>The ending char index.</returns>
		internal int GetLineLengthPlusOne(int ln)
		{
			return (LineList[ln-1].LineString.Length + 1);
		}

		/// <summary>
		/// Gets the word at the specified location.
		/// </summary>
		/// <param name="ln">The line of the location at which the word 
		/// is located.</param>
		/// <param name="ch">The char of the location at which the word 
		/// is located.</param>
		/// <returns>The word at the location.</returns>
		internal string GetWord(int ln, int ch)
		{
			EditLocationRange lcr = GetWordLocationRange(ln, ch);
			if (lcr != EditLocationRange.Empty)
			{
				return GetString(lcr.Start.L, lcr.Start.C, lcr.End.L, lcr.End.C);
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Gets the current word that can be a valid identifier.
		/// </summary>
		/// <param name="ln">The line of the location at which the word 
		/// is located.</param>
		/// <param name="ch">The char of the location at which the word 
		/// is located.</param>
		/// <returns>The word at the location.</returns>
		internal string GetIdentifierWord(int ln, int ch)
		{
			return GetWord(ln, ch);
		}

		/// <summary>
		/// Gets the location range for the word at the specified location.
		/// </summary>
		/// <param name="ln">The line of the location at which the word is 
		/// located.</param>
		/// <param name="ch">The char of the location at which the word is 
		/// located.</param>
		/// <returns>The location range for the word.</returns>
		internal EditLocationRange GetWordLocationRange(int ln, int ch)
		{
			if (!IsValidLineChar(ln, ch))
			{
				return EditLocationRange.Empty;
			}
			int lnLength = GetLineLength(ln);
			if ((lnLength == 0) || (ch > lnLength + 1))
			{
				return new EditLocationRange(ln, ch, ln, ch);
			}
			int startCol = 1;
			int endCol = 1;
			int lnLengthPlusOne = lnLength + 1;
			string strLine = LineList[ln-1].LineString;
			char chTemp;
			if (ch <= 1)
			{
				startCol = 1;
				endCol = startCol + 1;
				chTemp = strLine[startCol-1];
				if (IsSpaceOrTab(chTemp))
				{
					for (int i = endCol; i < lnLengthPlusOne; i++)
					{
						if (!IsSpaceOrTab(strLine[i-1]))
						{
							endCol = i;
							break;
						}
						if (i == lnLength)
						{
							endCol = lnLengthPlusOne;
						}
					}
				}
				else if (IsWordChar(chTemp))
				{
					for (int i = endCol; i < lnLengthPlusOne; i++)
					{
						if (!IsWordChar(strLine[i-1]))
						{
							endCol = i;
							break;
						}
						if (i == lnLength)
						{
							endCol = lnLengthPlusOne;
						}
					}
				}
			}
			else if (ch >= lnLengthPlusOne)
			{
				endCol = lnLengthPlusOne;
				startCol = endCol - 1;
				chTemp = strLine[startCol-1];
				if (IsSpaceOrTab(chTemp))
				{
					for (int i = startCol - 1; i >= 1; i--)
					{
						if (!IsSpaceOrTab(strLine[i-1]))
						{
							startCol = i + 1;
							break;
						}
						if (i == 1)
						{
							startCol = 1;
						}
					}
				}
				else if (IsWordChar(chTemp))
				{
					for (int i = startCol - 1; i >= 1; i--)
					{
						if (!IsWordChar(strLine[i-1]))
						{
							startCol = i + 1;
							break;
						}
						if (i == 1)
						{
							startCol = 1;
						}
					}
				}
			}
			else
			{
				startCol = ch;
				endCol = startCol + 1;
				chTemp = strLine[startCol-1];
				if (IsSpaceOrTab(chTemp))
				{
					for (int i = startCol - 1; i >= 1; i--)
					{
						if (!IsSpaceOrTab(strLine[i-1]))
						{
							startCol = i + 1;
							break;
						}
						if (i == 1)
						{
							startCol = 1;
						}
					}
					for (int i = endCol; i < lnLengthPlusOne; i++)
					{
						if (!IsSpaceOrTab(strLine[i-1]))
						{
							endCol = i;
							break;
						}
						if (i == lnLength)
						{
							endCol = lnLengthPlusOne;
						}
					}
				}
				else if (IsWordChar(chTemp))
				{
					for (int i = startCol - 1; i >= 1; i--)
					{
						if (!IsWordChar(strLine[i-1]))
						{
							startCol = i + 1;
							break;
						}
						if (i == 1)
						{
							startCol = 1;
						}
					}
					for (int i = endCol; i < lnLengthPlusOne; i++)
					{
						if (!IsWordChar(strLine[i-1]))
						{
							endCol = i;
							break;
						}
						if (i == lnLength)
						{
							endCol = lnLengthPlusOne;
						}
					}
				}
			}
			return new EditLocationRange(ln, startCol, ln, endCol);
		}

		/// <summary>
		/// Tests if the specified char is a valid word character .
		/// </summary>
		/// <param name="ch">The char to be tested.</param>
		/// <returns>true if the char is a word character; otherwise, false.
		/// </returns>
		internal static bool IsWordChar(char ch)
		{
			return (Char.IsLetterOrDigit(ch) || (ch == '_'));
		}

		/// <summary>
		/// Tests if the specified char is a space or a tab.
		/// </summary>
		/// <param name="ch">The char to be tested.</param>
		/// <returns>true if the char is a space or a tab.</returns>
		internal static bool IsSpaceOrTab(char ch)
		{
			return ((ch == ' ') || (ch == '\t'));
		}

		/// <summary>
		/// Tests if the specified string consists of spaces and tabs only.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <returns>true if the string consists of spaces and tabs only; 
		/// otherwise, false.</returns>
		internal static bool IsSpacesOrTabsOnly(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if ((str[i] != ' ') && (str[i] != '\t'))
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Tests if the specified line consists of spaces and tabs only.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <returns>true if the line consists of spaces and tabs only; 
		/// otherwise, false.</returns>
		internal bool IsSpacesOrTabsOnlyLine(int ln)
		{
			if (IsValidLine(ln))
			{
				string strLine = LineList[ln-1].LineString;
				for (int i = 0; i < strLine.Length; i++)
				{
					if ((strLine[i] != ' ') && (strLine[i] != '\t'))
					{
						return false;
					}
				}
			}
			return true;
		}

		/// <summary>
		/// Tests if the specified string consists of spaces only before any tab.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <param name="firstTab">The char index of the first tab character.
		/// </param>
		/// <returns>true if the the specified string consists of spaces only 
		/// before any tab; otherwise, false.</returns>
		private bool CheckSpacesBeforeTab(string str, ref int firstTab)
		{
			firstTab = -1;
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == ' ')
				{
					continue;
				}
				else if (str[i] == '\t')
				{
					firstTab = i;
					return true;
				}
				else
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Inserts a string as a new line before the specified line.
		/// </summary>
		/// <param name="ln">The line before which the string will be inserted.
		/// </param>
		/// <param name="str">The string to be inserted.</param>
		/// <returns>true if the string is inserted; otherwise, false.</returns>
		internal bool InsertLine(int ln, string str)
		{
			EditLine editLn = new EditLine(str);
			if (IsValidLine(ln))
			{
				LineList.Insert(ln - 1, editLn);
				return true;
			}
			else
			{
				if (ln == LineList.Count + 1)
				{
					LineList.Add(editLn);
					return true;
				}
				else
				{
					return false;
				}
			}
		}

		/// <summary>
		/// Inserts an array list of strings at the specified location.
		/// </summary>
		/// <param name="lc">The location for insertion.</param>
		/// <param name="strList">The string arraylist to be inserted.</param>
		/// <returns>The location range of the inserted string arraylist.
		/// </returns>
		internal EditLocationRange Insert(EditLocation lc, ArrayList strList)
		{
			bOutliningThreadContinue = false;
			if ((strList == null) || (strList.Count == 0))
			{
				return new EditLocationRange(lc, lc);
			}
			if (!IsValidLineChar(lc.L, lc.C))
			{
				if ((lc.L == LineList.Count + 1) && (lc.C == 1))
				{
					LineList.Add(string.Empty);
				}
				else
				{
					return new EditLocationRange(lc, lc);
				}
			}
			string strLine = GetString(lc.L);
			int oldLn = lc.L;
			int oldCh = lc.C;
			int newLn = lc.L;
			int newCh = lc.C;
			if (strList.Count == 1)
			{
				LineList[oldLn-1].LineString = LineList[oldLn-1].
					LineString.Insert(oldCh - 1, (string)strList[0]);
				newCh = oldCh + ((string)strList[0]).Length;
				UpdateWaveLineDataFromInsertion(lc.L, oldCh, newCh);
			}
			else
			{
				short [] sTemp1 = null;
				short [] sTemp2 = null;
				bool bStartInMiddle1 = false;; 
				short startCgi1 = -1;
				bool bEndInMiddle1 = false;
				short endCgi1 = -1;
				bool bStartInMiddle2 = false; 
				short startCgi2 = -1;
				bool bEndInMiddle2 = false;
				short endCgi2 = -1;
				if (GetLineLength(oldLn) != 0)
				{
					sTemp1 = GetPartialWaveLineData(oldLn, (short)1, 
						(short)(oldCh - 1), true, 
						ref bStartInMiddle1, ref startCgi1, 
						ref bEndInMiddle1, ref endCgi1);
					sTemp2 = GetPartialWaveLineData(oldLn, (short)oldCh, 
						(short)GetLineLength(oldLn), false, 
						ref bStartInMiddle2, ref startCgi2, 
						ref bEndInMiddle2, ref endCgi2);
					LineList[oldLn-1].LineString = LineList[oldLn-1].
						LineString.Remove(oldCh - 1, 
						strLine.Length - oldCh + 1);
				}
				LineList[oldLn-1].LineString = LineList[oldLn-1].
					LineString.Insert(oldCh - 1, (string)strList[0]);
				if (bEndInMiddle1)
				{
					sTemp1[sTemp1.Length - 2] = 
						(short)LineList[oldLn-1].LineString.Length;
				}
				LineList[oldLn-1].IndicatorData = sTemp1;
				for (int i = 1; i < strList.Count; i++)
				{
					newLn++;
					InsertLine(newLn, (string)strList[i]);
					short len = (short)((string)strList[i]).Length;
					if (bEndInMiddle1 && (len > 0))
					{
						short [] sTemp3 = new short[3];
						sTemp3[0] = 1;
						sTemp3[1] = len;
						sTemp3[2] = endCgi1;
						LineList[newLn-1].IndicatorData = sTemp3;
					}
				}
				newCh = GetLineLengthPlusOne(newLn);
				if (strLine.Length != 0)
				{
					LineList[newLn-1].LineString = LineList[newLn-1].
						LineString.Insert(newCh - 1, strLine.Substring(oldCh - 1));
					AdjustWaveLineData(sTemp2, (short)oldCh, (short)newCh);
					if (bEndInMiddle1)
					{
						sTemp2[0] = 1;
					}
					LineList[newLn-1].IndicatorData = sTemp2;
				}
			}
			return new EditLocationRange(oldLn, oldCh, newLn, newCh);
		}

		/// <summary>
		/// Deletes the text in the specified range.
		/// </summary>
		/// <param name="lcStart">The starting location for deletion.</param>
		/// <param name="lcEnd">The ending location for deletion.</param>
		/// <returns>true if some characters are deleted; otherwise, false.
		/// </returns>
		internal bool Delete(EditLocation lcStart, EditLocation lcEnd)
		{
			bOutliningThreadContinue = false;
			if (lcStart == lcEnd)
			{
				return false;
			}
			if ((!IsValidLineChar(lcStart.L, lcStart.C)) 
				|| (!IsValidLineChar(lcEnd.L, lcEnd.C)))
			{
				return false;
			}
			EditLocation lcStartTemp = (lcStart < lcEnd) ? lcStart : lcEnd;
			EditLocation lcEndTemp = (lcStart > lcEnd) ? lcStart : lcEnd;
			if (lcStartTemp.L == lcEndTemp.L)
			{
				int lnLengthPlusOne = GetLineLengthPlusOne(lcStartTemp.L);
				if (lcStartTemp.C >= lnLengthPlusOne)
				{
					return false;
				}
				int endC = Math.Min(lcEndTemp.C, lnLengthPlusOne);
				UpdateWaveLineFromDeletion(lcStartTemp.L, lcStartTemp.C, endC);
				LineList[lcStartTemp.L-1].LineString = LineList[lcStartTemp.L-1].
					LineString.Remove(lcStartTemp.C - 1, endC - lcStartTemp.C);
			}
			else if ((lcStartTemp.L == lcEndTemp.L - 1) 
				&& (lcStartTemp.C == 1) && 
				((lcEndTemp.C == 1) || (GetLineLength(lcEndTemp.L) == 0)))
			{
				LineList.RemoveAt(lcStartTemp.L - 1);
			}
			else
			{
				bool bStartInMiddle1 = false;; 
				short startCgi1 = -1;
				bool bEndInMiddle1 = false;
				short endCgi1 = -1;
				short [] sTemp1 = GetPartialWaveLineData(lcStartTemp.L, 
					(short)1, (short)(lcStartTemp.C - 1), true,
					ref bStartInMiddle1, ref startCgi1,
					ref bEndInMiddle1, ref endCgi1);
				short [] sTemp2 = GetPartialWaveLineData(lcEndTemp.L, 
					(short)lcEndTemp.C, (short)GetLineLength(lcEndTemp.L), false,
					ref bStartInMiddle1, ref startCgi1,
					ref bEndInMiddle1, ref endCgi1);
				if (GetLineLength(lcStartTemp.L) != 0)
				{	
					int lnLengthPlusOne = GetLineLengthPlusOne(lcStartTemp.L);
					if (lcStartTemp.C < lnLengthPlusOne)
					{
						LineList[lcStartTemp.L-1].LineString = 
							LineList[lcStartTemp.L-1].LineString.
							Remove(lcStartTemp.C - 1, 
							lnLengthPlusOne - lcStartTemp.C);
					}
				}
				if (lcEnd.C < GetLineLengthPlusOne(lcEnd.L))
				{
					LineList[lcStartTemp.L-1].LineString = 
						LineList[lcStartTemp.L-1].LineString.
						Insert(lcStartTemp.C - 1, LineList[lcEnd.L-1].
						LineString.Substring(lcEnd.C - 1));
				}
				for (int i = lcEndTemp.L; i > lcStartTemp.L; i--)
				{
					LineList.RemoveAt(i - 1);
				}
				AdjustWaveLineData(sTemp2, (short)lcEndTemp.C, 
					(short)lcStartTemp.C);
				short [] sTemp3 = MergeWaveLineData(sTemp1, sTemp2);
				LineList[lcStartTemp.L-1].IndicatorData = sTemp3;
			}
			return true;
		}

		/// <summary>
		/// Updates all the outlining objects from an insertion.
		/// </summary>
		/// <param name="lcr">The location range of the insertion.</param>
		/// <returns>true if the viewport needs redrawing; otherwise, 
		/// false.</returns>
		internal bool UpdateOutliningFromInsertion(EditLocationRange lcr)
		{
			return OutliningRoot.UpdateFromInsertion(lcr);
		}

		/// <summary>
		/// Updates all the outlining objects from a deletion.
		/// </summary>
		/// <param name="lcr">The location range of the deletion.</param>
		/// <returns>true if the viewport needs redrawing; otherwise, 
		/// false.</returns>
		internal bool UpdateOutliningFromDeletion(EditLocationRange lcr)
		{
			return OutliningRoot.UpdateFromDeletion(lcr);
		}

		/// <summary>
		/// Converts spaces/tabs for the text in the specified range.
		/// </summary>
		/// <param name="lcr">The location range for which the conversion 
		/// is to be conducted.</param>
		/// <param name="bToSpaces">A value indicating whether the conversion 
		/// is from tabs to spaces.</param>
		/// <returns>The string resulted from the conversion.</returns>
		internal string ConvertTabsSpaces(EditLocationRange lcr, bool bToSpaces)
		{
			EditLocationRange lcrNorm = lcr.Normalize();
			string strLine;
			StringBuilder strBdr = new StringBuilder(string.Empty);
			if (bToSpaces)
			{
				int tabOffset;
				int currentCol;
				int currentChar;
				if (lcrNorm.Start.L == lcrNorm.End.L)
				{
					strLine = LineList[lcrNorm.Start.L-1].LineString;
					currentCol = 1;
					currentChar = 1;
					if (lcrNorm.Start.C > 1)
					{
						for (int i = 0; i < lcrNorm.Start.C - 1; i++)
						{
							if (strLine[i] == '\t')
							{
								tabOffset = edit.TabSize 
									- (currentCol - 1)%edit.TabSize;
								currentCol += tabOffset;
							}
							else
							{
								currentCol++;
							}
						}
						currentChar = lcrNorm.Start.C;
					}
					for (int j = lcrNorm.Start.C - 1; j < lcrNorm.End.C - 1; j++)
					{
						if (strLine[j] == '\t')
						{
							tabOffset = edit.TabSize 
								- (currentCol - 1)%edit.TabSize;
							strBdr.Append(new string(' ', tabOffset));
							currentCol += tabOffset;
							currentChar += tabOffset;
						}
						else
						{
							strBdr.Append(strLine[j]);
							currentCol++;
							currentChar++;
						}
					}
				}
				else
				{
					strLine = LineList[lcrNorm.Start.L-1].LineString;
					currentCol = 1;
					if (lcrNorm.Start.C > 1)
					{
						for (int i = 0; i < lcrNorm.Start.C - 1; i++)
						{
							if (strLine[i] == '\t')
							{
								tabOffset = edit.TabSize 
									- (currentCol - 1)%edit.TabSize;
								currentCol += tabOffset;
							}
							else
							{
								currentCol++;
							}
						}
					}
					for (int j = lcrNorm.Start.C - 1; 
						j < GetLineLength(lcrNorm.Start.L); j++)
					{
						if (strLine[j] == '\t')
						{
							tabOffset = edit.TabSize 
								- (currentCol - 1)%edit.TabSize;
							strBdr.Append(new string(' ', tabOffset));
							currentCol += tabOffset;
						}
						else
						{
							strBdr.Append(strLine[j]);
							currentCol++;
						}
					}
					strBdr.Append(editSettings.NewLine);
					for (int i = lcrNorm.Start.L + 1; i < lcrNorm.End.L; i++)
					{
						strLine = LineList[i-1].LineString;
						currentCol = 1;
						for (int j = 0; j < GetLineLength(i); j++)
						{
							if (strLine[j] == '\t')
							{
								tabOffset = edit.TabSize 
									- (currentCol - 1)%edit.TabSize;
								currentCol += tabOffset;
								strBdr.Append(new string(' ', tabOffset));
							}
							else
							{
								strBdr.Append(strLine[j]);
								currentCol++;
							}
						}
						strBdr.Append(editSettings.NewLine);
					}
					strLine = LineList[lcrNorm.End.L-1].LineString;
					currentCol = 1;
					currentChar = 1;
					for (int j = 0; j < lcrNorm.End.C - 1; j++)
					{
						if (strLine[j] == '\t')
						{
							tabOffset = edit.TabSize 
								- (currentCol - 1)%edit.TabSize;
							strBdr.Append(new string(' ', tabOffset));
							currentCol += tabOffset;
							currentChar += tabOffset;
						}
						else
						{
							strBdr.Append(strLine[j]);
							currentCol++;
							currentChar++;
						}
					}
				}
			}
			else
			{
				int tabOffset;
				int currentCol;
				int currentChar;
				if (lcrNorm.Start.L == lcrNorm.End.L)
				{
					strLine = LineList[lcrNorm.Start.L-1].LineString;
					currentCol = 1;
					currentChar = 1;
					if (lcrNorm.Start.C > 1)
					{
						for (int i = 0; i < lcrNorm.Start.C - 1; i++)
						{
							if (strLine[i] == '\t')
							{
								tabOffset = edit.TabSize 
									- (currentCol - 1)%edit.TabSize;
								currentCol += tabOffset;
							}
							else
							{
								currentCol++;
							}
						}
						currentChar = lcrNorm.Start.C;
					}
					for (int j = lcrNorm.Start.C - 1; j < lcrNorm.End.C - 1; j++)
					{
						ConvertSpaceToTab(lcrNorm.End.C, ref j, 
							ref currentCol, ref currentChar, strLine, ref strBdr);
					}
				}
				else
				{
					strLine = LineList[lcrNorm.Start.L-1].LineString;
					currentCol = 1;
					currentChar = 1;
					if (lcrNorm.Start.C > 1)
					{
						for (int i = 0; i < lcrNorm.Start.C - 1; i++)
						{
							if (strLine[i] == '\t')
							{
								tabOffset = edit.TabSize 
									- (currentCol - 1)%edit.TabSize;
								currentCol += tabOffset;
							}
							else
							{
								currentCol++;
							}
						}
					}
					for (int j = lcrNorm.Start.C - 1; 
						j < GetLineLength(lcrNorm.Start.L); j++)
					{
						ConvertSpaceToTab(GetLineLength(lcrNorm.Start.L) + 1, ref j, 
							ref currentCol, ref currentChar, strLine, ref strBdr);
					}
					strBdr.Append(editSettings.NewLine);
					for (int i = lcrNorm.Start.L + 1; i < lcrNorm.End.L; i++)
					{
						strLine = LineList[i-1].LineString;
						currentCol = 1;
						currentChar = 1;
						for (int j = 0; j < GetLineLength(i); j++)
						{
							ConvertSpaceToTab(GetLineLength(i) + 1, ref j, 
								ref currentCol, ref currentChar, strLine, ref strBdr);
						}
						strBdr.Append(editSettings.NewLine);
					}
					strLine = LineList[lcrNorm.End.L-1].LineString;
					currentCol = 1;
					currentChar = 1;
					for (int j = 0; j < lcrNorm.End.C - 1; j++)
					{
						ConvertSpaceToTab(lcrNorm.End.C, ref j, 
							ref currentCol, ref currentChar, strLine, ref strBdr);
					}
				}
			}
			return strBdr.ToString();
		}

		/// <summary>
		/// Converts spaces to tabs for the text in the specified range.
		/// </summary>
		/// <param name="chEndar"></param>
		/// <param name="index"></param>
		/// <param name="currentCol"></param>
		/// <param name="currentChar"></param>
		/// <param name="strLine"></param>
		/// <param name="strBdr"></param>
		private void ConvertSpaceToTab(int chEndar, ref int index, ref int currentCol, 
			ref int currentChar, string strLine, ref StringBuilder strBdr)
		{
			int tabOffset;
			int firstTab;
			tabOffset = edit.TabSize - (currentCol - 1)%edit.TabSize;
			if (index + tabOffset >= chEndar)
			{
				strBdr.Append(strLine[index]);
				currentChar++;
				if (strLine[index] == '\t')
				{
					currentCol += tabOffset;
				}
				else
				{
					currentCol++;
				}
				return;
			}
			if (strLine[index] == ' ')
			{
				firstTab = -1;
				if (CheckSpacesBeforeTab(strLine.Substring(index, tabOffset), ref firstTab))
				{
					strBdr.Append("\t");
					currentCol += tabOffset;
					currentChar++;
					if (firstTab == -1)
					{
						index += tabOffset - 1;
					}
					else
					{
						index += firstTab;
					}
				}
				else
				{
					strBdr.Append(strLine[index]);
					currentCol++;
					currentChar++;
				}
			}
			else if (strLine[index] == '\t')
			{
				strBdr.Append(strLine[index]);
				currentCol += tabOffset;
				currentChar++;
			}
			else
			{
				strBdr.Append(strLine[index]);
				currentCol++;
				currentChar++;
			}
		}

		/// <summary>
		/// Deletes white space for the text in the specified range.
		/// </summary>
		/// <param name="lcr">The location range for which white space is to 
		/// be deleted.</param>
		/// <returns>The deleted string of white space.</returns>
		internal string DeleteHorizontalWhiteSpace(EditLocationRange lcr)
		{
			EditLocationRange lcrNorm = lcr.Normalize();
			StringBuilder strBdr = new StringBuilder(string.Empty);
			string strLine;
			string strTemp;
			bool bOneSpaceOk;
			if (lcrNorm.Start.L == lcrNorm.End.L)
			{
				strLine = LineList[lcrNorm.Start.L-1].LineString;
				if (strLine != string.Empty)
				{
					strTemp = strLine.Substring(lcrNorm.Start.C - 1, 
						lcrNorm.End.C - lcrNorm.Start.C);
					strTemp = strTemp.Trim();
					bOneSpaceOk = false;
					for (int i = 0; i < strTemp.Length; i++)
					{
						if ((strTemp[i] == ' ') || (strTemp[i] == '\t'))
						{
							if (bOneSpaceOk)
							{
								strBdr.Append(strTemp[i]);
								bOneSpaceOk = false;
							}
						}
						else
						{
							strBdr.Append(strTemp[i]);
							bOneSpaceOk = true;
						}
					}
				}
			}
			else
			{
				strLine = LineList[lcrNorm.Start.L-1].LineString;
				if (strLine != string.Empty)
				{
					strTemp = strLine.Substring(lcrNorm.Start.C - 1);
					strTemp = strTemp.Trim();
					bOneSpaceOk = false;
					for (int i = 0; i < strTemp.Length; i++)
					{
						if ((strTemp[i] == ' ') || (strTemp[i] == '\t'))
						{
							if (bOneSpaceOk)
							{
								strBdr.Append(strTemp[i]);
								bOneSpaceOk = false;
							}
						}
						else
						{
							strBdr.Append(strTemp[i]);
							bOneSpaceOk = true;
						}
					}
					strBdr.Append(editSettings.NewLine);
				}
				for (int i = lcrNorm.Start.L + 1; i < lcrNorm.End.L; i++)
				{
					strLine = LineList[i-1].LineString;
					if (strLine != string.Empty)
					{
						strTemp = strLine;
						strTemp = strTemp.Trim();
						bOneSpaceOk = false;
						for (int j = 0; j < strTemp.Length; j++)
						{
							if ((strTemp[j] == ' ') || (strTemp[j] == '\t'))
							{
								if (bOneSpaceOk)
								{
									strBdr.Append(strTemp[j]);
									bOneSpaceOk = false;
								}
							}
							else
							{
								strBdr.Append(strTemp[j]);
								bOneSpaceOk = true;
							}
						}
					}
					strBdr.Append(editSettings.NewLine);
				}
				strLine = LineList[lcrNorm.End.L-1].LineString;	
				if (strLine != string.Empty)
				{
					strTemp = strLine.Substring(0, lcrNorm.End.C - 1);
					strTemp = strTemp.Trim();
					bOneSpaceOk = false;
					for (int i = 0; i < strTemp.Length; i++)
					{
						if ((strTemp[i] == ' ') || (strTemp[i] == '\t'))
						{
							if (bOneSpaceOk)
							{
								strBdr.Append(strTemp[i]);
								bOneSpaceOk = false;
							}
						}
						else
						{
							strBdr.Append(strTemp[i]);
							bOneSpaceOk = true;
						}
					}
				}
			}
			return strBdr.ToString();
		}

		/// <summary>
		/// Reads the contents of the specified file into the text buffer.
		/// </summary>
		/// <param name="fileName">The file to be read.</param>
		/// <returns>return true if successful; otherwise, false</returns>
		internal bool LoadFile(string fileName)
		{
			if ((fileName != null) && (fileName != string.Empty))
			{
				try
				{
					if (!File.Exists(fileName))
					{
						EditControl.ShowInfoMessage(
							edit.GetResourceString("FileNotFound"), 
							edit.GetResourceString("LoadFile"));
						return false;
					}
					StreamReader strReader = new StreamReader(fileName, 
						edit.TextEncoding);
					StringBuilder strBdr = new StringBuilder(string.Empty);
					int chTemp1;
					int chTemp2;
					chTemp1 = strReader.Read();
					while (chTemp1 != -1)
					{
						if (((char)chTemp1) == '\r')
						{
							chTemp2 = strReader.Read();
							if (chTemp2 == -1)
							{
								LineList.Add(strBdr.ToString());
								chTemp1 = chTemp2;
							}
							else if (((char)chTemp2) == '\n')
							{
								LineList.Add(strBdr.ToString());
								strBdr.Remove(0, strBdr.Length);
								chTemp1 = strReader.Read();
							}
							else
							{
								LineList.Add(strBdr.ToString());
								strBdr.Remove(0, strBdr.Length);
								strBdr.Append((char)chTemp2);
								chTemp1 = strReader.Read();
							}
						}
						else if (((char)chTemp1) == '\n')
						{
							LineList.Add(strBdr.ToString());
							strBdr.Remove(0, strBdr.Length);
							chTemp1 = strReader.Read();
						}
						else
						{
							strBdr.Append((char)chTemp1);
							chTemp1 = strReader.Read();
						}
					}
					LineList.Add(strBdr.ToString());
					strReader.DiscardBufferedData();
					strReader.Close();
					if (edit.AutomaticOutliningEnabled)
					{
						StopOutliningThread();
						StartOutliningThread();
					}
					return true;
				} 
				catch (IOException e)
				{
					EditControl.ShowErrorMessage(e.Message, 
						edit.GetResourceString("LoadFile"));
					return false;
				}
			}
			else
			{
				EditControl.ShowInfoMessage(
					edit.GetResourceString("NoSourceFile"), 
					edit.GetResourceString("LoadFile"));
				return false;
			}
/*
			if ((fileName != null) && (fileName != string.Empty))
			{
				try
				{
					if (!File.Exists(fileName))
					{
						EditControl.ShowInfoMessage(edit.StrFileNotFound, edit.StrLoadFile);
						return false;
					}
					StreamReader strReader = new StreamReader(fileName, 
						edit.TextEncoding);
					string strTemp = strReader.ReadToEnd();
					edit.Insert(strTemp);
					edit.ClearUndo();
					edit.CurrentLine = 1;
					edit.CurrentChar = 1;
					if (edit.AutomaticOutliningEnabled)
					{
						StopOutliningThread();
						StartOutliningThread();
					}
					strReader.Close();
					return true;
				} 
				catch (IOException e)
				{
					EditControl.ShowErrorMessage(e.Message, edit.StrLoadFile);
					return false;
				}
			}
			else
			{
				EditControl.ShowInfoMessage(edit.StrNoSourceFile, edit.StrLoadFile);
				return false;
			}
*/
		}

		/// <summary>
		/// Inserts the contents of the specified file at the current caret 
		/// location.
		/// </summary>
		/// <param name="fileName">The file to be inserted.</param>
		internal bool InsertFile(string fileName)
		{
			if ((fileName != null) && (fileName != string.Empty))
			{
				try
				{
					if (!File.Exists(fileName))
					{
						EditControl.ShowInfoMessage(
							edit.GetResourceString("FileNotFound"), 
							edit.GetResourceString("InsertFile"));
						return false;
					}
					StreamReader strReader = new StreamReader(fileName, 
						edit.TextEncoding);
					string strTemp = strReader.ReadToEnd();
					edit.Insert(strTemp);
					strReader.Close();
					return true;
				} 
				catch (IOException e)
				{
					EditControl.ShowErrorMessage(e.Message, 
						edit.GetResourceString("InsertFile"));
					return false;
				}
			}
			else
			{
				EditControl.ShowInfoMessage(
					edit.GetResourceString("NoSourceFile"), 
					edit.GetResourceString("InsertFile"));
				return false;
			}
		}

		/// <summary>
		/// Saves the contents of the text buffer to the specified file.
		/// </summary>
		/// <param name="fileName">The name and location of the file to save.
		/// </param>
		/// <returns>return true if successful; otherwise, false</returns>
		internal bool SaveFile(string fileName)
		{
			if ((fileName != null) && (fileName != string.Empty))
			{
				try 
				{
					if (IsReadOnly(fileName))
					{
						return false;
					}
					StreamWriter strWriter =  new StreamWriter(fileName, 
						false, edit.TextEncoding);
					int lnLast = LineList.Count;
					for (int i = 1; i < lnLast; i++)
					{
						strWriter.Write(LineList[i-1].LineString 
							+ editSettings.NewLine);
					}
					if (LineList[lnLast-1].LineString.Length > 0)
					{
						strWriter.Write(LineList[lnLast-1].LineString);
					}
					strWriter.Close();
					return true;
				} 
				catch (IOException e)
				{
					EditControl.ShowErrorMessage(e.Message, 
						edit.GetResourceString("SaveFile"));
					return false;
				}
			}
			else
			{
				EditControl.ShowErrorMessage(
					edit.GetResourceString("NoDestinationFile"), 
					edit.GetResourceString("SaveFile"));
				return false;
			}
		}

		/// <summary>
		/// Saves the text and color information to an HTML file.
		/// </summary>
		/// <param name="fileName">The file to save to.</param>
		/// <returns>return true if successful; otherwise, false</returns>
		internal bool SaveAsHTML(string fileName)
		{
			if ((fileName != null) && (fileName != string.Empty))
			{
				try 
				{
					if (IsReadOnly(fileName))
					{
						return false;
					}
					string [] HtmlStart = {	"<HTML>",
											  "<HEAD>",
											  "<TITLE>Essential Edit Generated HTML</TITLE>",
											  "<META http-equiv=\"Content-Type\" content=\"text/html; charset=windows-1252\">",
											  "</HEAD>"};
					string [] HtmlEnd = {	"</PRE>",
											"</FONT>",
											"</BODY>",
											"</HTML>"};
					StreamWriter strWriter = new StreamWriter(fileName, 
						false, edit.TextEncoding);
					for (int i = 0; i < HtmlStart.Length; i++)
					{
						strWriter.WriteLine(HtmlStart[i]);
					}
					strWriter.WriteLine(GetHTMLBackColor(edit.TextBackColor));
					strWriter.Write(GetHTMLFont(edit.Font.Name, 
						(int)edit.Font.Size));
					StringBuilder strBdr = new StringBuilder();
					int lnLast = LineList.Count;
					for (int i = 1; i < lnLast; i++)
					{
						GetHTMLLine(ref strBdr, i);
						strWriter.WriteLine(strBdr.ToString());
					}
					GetHTMLLine(ref strBdr, lnLast);
					strWriter.Write(strBdr.ToString());
					for (int i = 0; i < HtmlEnd.Length; i++)
					{
						strWriter.WriteLine(HtmlEnd[i]);
					}
					strWriter.Close();
					return true;
				} 
				catch (IOException e)
				{
					EditControl.ShowErrorMessage(e.Message, 
						edit.GetResourceString("SaveAs"));
					return false;
				}
			}
			else
			{
				EditControl.ShowInfoMessage(
					edit.GetResourceString("NoDestinationFile"), 
					edit.GetResourceString("SaveAs"));
				return false;
			}		   
		}

		/// <summary>
		/// Gets the string for the setting of the HTML background color.
		/// </summary>
		/// <param name="clr">the desired background color</param>
		/// <returns>the string containing the setting of the background 
		/// color.</returns>
		protected string GetHTMLBackColor(Color clr)
		{
			return "<BODY BGCOLOR=\"" + GetHTMLColor(clr) + "\">";
		}

		/// <summary>
		/// Gets the string for the setting of the HTML foreground color.
		/// </summary>
		/// <param name="clr">the desired foreground color</param>
		/// <returns>the string containing the setting of the foreground 
		/// color.</returns>
		protected string GetHTMLColor(Color clr)
		{
			return String.Format("#{0:X2}{1:X2}{2:X2}", clr.R, clr.G, clr.B);
		}

		/// <summary>
		/// Gets the string for the setting of the HTML Font.
		/// </summary>
		/// <param name="fontName">the name of the font</param>
		/// <param name="size">the size of the font</param>
		/// <returns>the string containing the setting of the font.</returns>
		protected string GetHTMLFont(string fontName, int size)
		{
			return "<FONT FACE=\"" + fontName /*+ "\"" 
				* + " SIZE=\"" + size.ToString()*/ + "\">" + "<PRE>";
		}

		/// <summary>
		/// Gets the HTML format string for the specified string in 
		/// the specified color.
		/// </summary>
		/// <param name="str">the specified string</param>
		/// <param name="clr">the specified color</param>
		/// <returns>the converted HTML format string</returns>
		protected string GetHTMLColoredString(string str, Color clr)
		{
			string strTemp = str.Replace("<", "&lt");
			strTemp = strTemp.Replace(">", "&gt");
			return "<FONT COLOR=\"" + GetHTMLColor(clr) + "\">" 
				+ strTemp + "</FONT>";
		}

		/// <summary>
		/// Gets the specified line in HTML format.
		/// </summary>
		/// <param name="ln">The desired line.</param>
		/// <returns>The line string in HTML format.</returns>
		protected void GetHTMLLine(ref StringBuilder strBdr, int ln)
		{
			strBdr.Remove(0, strBdr.Length);
			EditLine editLn = GetLineWithTabInSpaces(ln);
			short [] ctd = editLn.ColorTextData;
			string strLine = editLn.LineString;
			if ((edit.SyntaxColoringEnabled) && (ctd != null))
			{
				if (ctd.Length == 1)
				{
					strBdr.Append(GetHTMLColoredString(strLine, 
						editSettings.GetColorGroupForeColor(ctd[0])));
				}
				else
				{
					string strTemp1;
					int currentChar = 1;
					for (int i = 0; i < ctd.Length; i += 3)
					{
						if (ctd[i] > currentChar)
						{
							strTemp1 = strLine.Substring(currentChar - 1, 
								ctd[i] - currentChar);
							strBdr.Append(GetHTMLColoredString(strTemp1, 
								edit.TextForeColor));
							currentChar = ctd[i];
						}
						strTemp1 = strLine.Substring(ctd[i] - 1, 
							ctd[i+1] - ctd[i] + 1);
						strBdr.Append(GetHTMLColoredString(strTemp1, 
							editSettings.GetColorGroupForeColor(ctd[i+2])));
						currentChar = ctd[i+1] + 1;
					}
					if (currentChar <= strLine.Length)
					{
						strTemp1 = strLine.Substring(currentChar - 1);
						strBdr.Append(GetHTMLColoredString(strTemp1, 
							edit.TextForeColor));
					}
				}
			}
			else
			{
				strBdr.Append(GetHTMLColoredString(strLine, edit.TextForeColor));
			}
		}

		/// <summary>
		/// Tests if the specified file is readonly.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		internal bool IsReadOnly(string fileName)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			if (fileInfo.Exists)
			{
				if ((fileInfo.Attributes & FileAttributes.ReadOnly) 
					== FileAttributes.ReadOnly)
				{
					EditControl.ShowInfoMessage(
						edit.GetResourceString("FileReadonly"), 
						edit.GetResourceString("SaveFile"));
					return true;
				}
				return false;
			}
			return false;
		}

		/// <summary>
		/// Saves the text and color information to a RTF file.
		/// </summary>
		/// <param name="fileName">The file to save to.</param>
		/// <returns>true if successful; otherwise, false</returns>
		internal bool SaveAsRTF(string fileName)
		{
			if (IsReadOnly(fileName))
			{
				return false;
			}
			RichTextBox rtb = new RichTextBox();
			rtb.Font = edit.Font;
			if ((fileName != null) && (fileName != string.Empty))
			{
				int lnLast = LineList.Count;
				for (int i = 1; i <= lnLast; i++)
				{
					InsertRTFLine(ref rtb, i);
				}
				rtb.SaveFile(fileName);
				return true;
			}
			else
			{
				EditControl.ShowErrorMessage(
					edit.GetResourceString("NoDestinationFile"), 
					edit.GetResourceString("SaveAs"));
				return false;
			}
		}

		/// <summary>
		/// Inserts the content of the specified line into the richtext box.
		/// </summary>
		/// <param name="rtb"></param>
		/// <param name="ln"></param>
		internal void InsertRTFLine(ref RichTextBox rtb, int ln)
		{
			UpdateLineInfo(ln);
			short [] ctd = LineList[ln-1].ColorTextData;
			string strLine = LineList[ln-1].LineString;
			if ((edit.SyntaxColoringEnabled) && (ctd != null))
			{
				if (ctd.Length == 1)
				{
					rtb.SelectionColor = editSettings.GetColorGroupForeColor(ctd[0]);
					rtb.AppendText(strLine);
					if (ln < LineList.Count)
					{
						rtb.AppendText(editSettings.NewLine);
					}
				}
				else
				{
					string strTemp1;
					int currentChar = 1;
					for (int i = 0; i < ctd.Length; i += 3)
					{
						if (ctd[i] > currentChar)
						{
							strTemp1 = strLine.Substring(currentChar - 1, 
								ctd[i] - currentChar);
							rtb.SelectionColor = edit.TextForeColor;
							rtb.AppendText(strTemp1);
							currentChar = ctd[i];
						}
						strTemp1 = strLine.Substring(ctd[i] - 1, 
							ctd[i+1] - ctd[i] + 1);
						rtb.SelectionColor = 
							editSettings.GetColorGroupForeColor(ctd[i+2]);
						rtb.AppendText(strTemp1);
						currentChar = ctd[i+1] + 1;
					}
					if (currentChar <= strLine.Length)
					{
						strTemp1 = strLine.Substring(currentChar - 1);
						rtb.SelectionColor = edit.TextForeColor;
						rtb.AppendText(strTemp1);
					}
					if (ln < LineList.Count)
					{
						rtb.AppendText(editSettings.NewLine);
					}
				}
			}
			else
			{
				rtb.SelectionColor = edit.TextForeColor;
				rtb.AppendText(strLine);
				if (ln < LineList.Count)
				{
					rtb.AppendText(editSettings.NewLine);
				}
			}
		}

		/// <summary>
		/// Gets the EditLine object of the specified line, with all tabs 
		/// converted into spaces.
		/// </summary>
		/// <param name="ln">The line for which the EditLine object is to be 
		/// obtained.</param>
		/// <returns>The EditLine object of the line, with all tabs converted 
		/// into spaces.</returns>
		internal EditLine GetLineWithTabInSpaces(int ln)
		{
			UpdateLineInfo(ln);
			EditLine editLnTemp = new EditLine(LineList[ln-1]);
			StringBuilder sbTemp = new StringBuilder(string.Empty);
			int currentCol = 1;
			int tabOffset;
			bool bChangeColorInfo;
			if (edit.SyntaxColoringEnabled)
			{
				if ((editLnTemp.ColorTextData == null) || 
					(editLnTemp.ColorTextData.Length == 1))
				{
					bChangeColorInfo = false;
				}
				else
				{
					bChangeColorInfo = true;
				}
			}
			else
			{
				bChangeColorInfo = false;
			}
			for (int i = 0; i < LineList[ln-1].LineString.Length; i++)
			{
				if (editLnTemp.LineString[i] != '\t')
				{
					sbTemp.Append(editLnTemp.LineString[i]);
					currentCol++;
				}
				else
				{
					tabOffset = edit.TabSize - (currentCol - 1)%edit.TabSize;
					sbTemp.Append(new string(' ', tabOffset));
					if (bChangeColorInfo)
					{
						for (int k = 0; k < editLnTemp.ColorTextData.Length; k += 3)
						{
							if (editLnTemp.ColorTextData[k] >= currentCol)
							{
								editLnTemp.ColorTextData[k] += 
									(short)(tabOffset - 1);
								editLnTemp.ColorTextData[k+1] += 
									(short)(tabOffset - 1);
							}
						}
					}
					currentCol += tabOffset;
				}
			}
			editLnTemp.LineString = sbTemp.ToString();
			return editLnTemp;
		}

		/// <summary>
		/// Gets the EditLine object with selection coloring information.
		/// </summary>
		/// <param name="editLn"></param>
		/// <param name="chStart"></param>
		/// <param name="chEnd"></param>
		/// <param name="cgi"></param>
		/// <returns></returns>
		internal EditLine GetEditLineWithHighlight(EditLine editLn, 
			short chStart, short chEnd, short cgi)
		{
			short [] ctdOld = editLn.ColorTextData;
			EditLine editLnTemp = new EditLine(editLn);
			int lnLength = editLn.LineString.Length;
			int lnLengthPlusOne = lnLength + 1;
			short [] sTemp;
			int sDataLength = 0;
			if (chEnd < chStart)
			{
				return editLnTemp;
			}
			if (editLn.ColorTextData == null)
			{
				sTemp = new short[9];
			}
			else
			{
				sTemp = new short [editLn.ColorTextData.Length + 9];
			}
			if (editLn.ColorTextData == null)
			{
				sTemp[sDataLength++] = chStart;
				sTemp[sDataLength++] = chEnd;
				sTemp[sDataLength++] = cgi;
			}
			else if (editLn.ColorTextData.Length == 1)
			{
				if (chStart > 1)
				{
					sTemp[sDataLength++] = 1;
					sTemp[sDataLength++] = (short)(chStart - 1);
					sTemp[sDataLength++] = editLn.ColorTextData[0];
				}
				sTemp[sDataLength++] = chStart;
				sTemp[sDataLength++] = chEnd;
				sTemp[sDataLength++] = cgi;
				if (chEnd < lnLength)
				{
					sTemp[sDataLength++] = (short)(chEnd + 1);
					sTemp[sDataLength++] = (short)lnLength;
					sTemp[sDataLength++] = editLn.ColorTextData[0];
				}
			}
			else
			{
				bool bAdded = false;
				for (int i = 0; i < ctdOld.Length; i += 3)
				{
					if (ctdOld[i+1] < chStart)
					{
						sTemp[sDataLength++] = ctdOld[i];
						sTemp[sDataLength++] = ctdOld[i+1];
						sTemp[sDataLength++] = ctdOld[i+2];
					}
					else if (ctdOld[i] > chEnd)
					{
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
						sTemp[sDataLength++] = ctdOld[i];
						sTemp[sDataLength++] = ctdOld[i+1];
						sTemp[sDataLength++] = ctdOld[i+2];
					}
					else if ((ctdOld[i] >= chStart) && (ctdOld[i+1] <= chEnd))
					{
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
					}
					else if ((ctdOld[i] >= chStart) && (ctdOld[i+1] > chEnd))
					{
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
						sTemp[sDataLength++] = (short)(chEnd + 1);
						sTemp[sDataLength++] = ctdOld[i+1];
						sTemp[sDataLength++] = ctdOld[i+2];
					}
					else if ((ctdOld[i] < chStart) && (ctdOld[i+1] <= chEnd))
					{
						sTemp[sDataLength++] = ctdOld[i];
						sTemp[sDataLength++] = (short)(chStart - 1);
						sTemp[sDataLength++] = ctdOld[i+2];
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
					}
					else if ((ctdOld[i] < chStart) && (ctdOld[i+1] > chEnd))
					{
						sTemp[sDataLength++] = ctdOld[i];
						sTemp[sDataLength++] = (short)(chStart - 1);
						sTemp[sDataLength++] = ctdOld[i+2];
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
						sTemp[sDataLength++] = (short)(chEnd + 1);
						sTemp[sDataLength++] = ctdOld[i+1];
						sTemp[sDataLength++] = ctdOld[i+2];
					}
				}
				if (!bAdded)
				{
					sTemp[sDataLength++] = chStart;
					sTemp[sDataLength++] = chEnd;
					sTemp[sDataLength++] = cgi;
					bAdded = true;
				}
			}
			editLnTemp.ColorTextData = new short [sDataLength];
			for (int i = 0; i < sDataLength; i++)
			{
				editLnTemp.ColorTextData[i] = sTemp[i];
			}
			if (chEnd == lnLengthPlusOne)
			{
				editLnTemp.LineString = editLn.LineString + "\n";
			}
			return editLnTemp;
		}

		/// <summary>
		/// Gets the partial EditLine object from the specified EditLine 
		/// object.
		/// </summary>
		/// <param name="editLn">The EditLine object from which the partial 
		/// EditLine object is to be obtained.</param>
		/// <param name="chStart">The starting char of the partial EditLine 
		/// object.</param>
		/// <param name="chEnd">The ending char of the partial EditLine 
		/// object.</param>
		/// <returns>The partial EditLine object from the specified EditLine 
		/// object.</returns>
		internal EditLine GetPartialEditLine(EditLine editLn, int chStart, int chEnd)
		{
			EditLine editLnOld = editLn;
			short [] ctdOld = editLnOld.ColorTextData;
			EditLine editLnTemp = new EditLine(editLn);
			editLnTemp.LineString = editLnOld.LineString.Substring(chStart - 1, 
				chEnd - chStart + 1);
			if (editLnOld.ColorTextData == null)
			{
				editLnTemp.ColorTextData = null;
			}
			else if (editLnOld.ColorTextData.Length == 1)
			{
				editLnTemp.ColorTextData = new short[1];
				editLnTemp.ColorTextData[0] = editLnOld.ColorTextData[0];
			}
			else
			{
				short [] sTemp = new short[editLnOld.ColorTextData.Length];
				int sDataLength = 0;
				short offset = (short)(chStart - 1);
				for (int i = 0; i < ctdOld.Length; i += 3)
				{
					if (ctdOld[i + 1] < chStart)
					{
						continue;
					}
					if (ctdOld[i] > chEnd)
					{
						break;
					}
					else if ((ctdOld[i] >= chStart) && (ctdOld[i+1] <= chEnd))
					{
						sTemp[sDataLength++] = (short)(ctdOld[i] - offset);
						sTemp[sDataLength++] = (short)(ctdOld[i+1] - offset);
						sTemp[sDataLength++] = ctdOld[i+2];
					}
					else if ((ctdOld[i] >= chStart) && (ctdOld[i+1] > chEnd))
					{
						sTemp[sDataLength++] = (short)(ctdOld[i] - offset);
						sTemp[sDataLength++] = (short)(chEnd - offset);
						sTemp[sDataLength++] = ctdOld[i+2];
						break;
					}
					else if ((ctdOld[i] < chStart) && (ctdOld[i+1] <= chEnd))
					{
						sTemp[sDataLength++] = (short)(chStart - offset);
						sTemp[sDataLength++] = (short)(ctdOld[i+1] - offset);
						sTemp[sDataLength++] = ctdOld[i+2];
					}
					else if ((ctdOld[i] < chStart) && (ctdOld[i+1] > chEnd))
					{
						sTemp[sDataLength++] = (short)(chStart - offset);
						sTemp[sDataLength++] = (short)(chEnd - offset);
						sTemp[sDataLength++] = ctdOld[i+2];
						break;
					}
				}
				editLnTemp.ColorTextData = new short [sDataLength];
				for (int i = 0; i < sDataLength; i++)
				{
					editLnTemp.ColorTextData[i] = sTemp[i];
				}
			}
			return editLnTemp;
		}

		/// <summary>
		/// Finds the first occurance of the specified string.
		/// </summary>
		/// <param name="lc"></param>
		/// <param name="str"></param>
		/// <param name="bMatchCase"></param>
		/// <param name="bMatchWholeWord"></param>
		/// <param name="bSearchHiddenText"></param>
		/// <param name="bSearchUp"></param>
		/// <param name="bUseRegex"></param>
		/// <param name="bUseWildcards"></param>
		/// <returns></returns>
		internal EditLocationRange Find(EditLocation lc, string str, 
			bool bMatchCase, bool bMatchWholeWord, 
			bool bSearchHiddenText, bool bSearchUp, 
			bool bUseRegex, bool bUseWildcards, bool bWholeRange)
		{
			if ((str == string.Empty) || (LineList.Count == 0))
			{
				return EditLocationRange.Empty;
			}
			else
			{
				RegexOptions rgOpt = RegexOptions.Singleline;
				if (!bMatchCase)
				{
					rgOpt |= RegexOptions.IgnoreCase;
				}
				if (!bUseRegex)
				{
					if (bUseWildcards)
					{
						str = GetRegExpFromWildcards(str);
					}
					else
					{
						str = Regex.Escape(str);
					}
				}
				if (bMatchWholeWord)
				{
					str = "\\b" + str + "\\b";
				}
				Match match;
				int lnLength = LineList[lc.L-1].LineString.Length;
				if (!bSearchUp)
				{
					if (lc.C <= lnLength)
					{
						match = Regex.Match(LineList[lc.L-1].LineString.
							Substring(lc.C - 1), str, rgOpt);
						if (match != Match.Empty)
						{
							return new EditLocationRange(lc.L, lc.C + match.Index,
								lc.L, lc.C + match.Index + match.Length);
						}
					}
					int lnLast = LineList.Count;
					for (int i = lc.L + 1; i <= lnLast; i++)
					{
						match = Regex.Match(LineList[i-1].LineString, 
							str, rgOpt);
						if (match != Match.Empty)
						{
							return new EditLocationRange(i, 1 + match.Index,
								i, 1 + match.Index + match.Length);
						}
					}
					if (bWholeRange)
					{
						for (int i = 1; i <= lc.L; i++)
						{
							match = Regex.Match(LineList[i-1].LineString, 
								str, rgOpt);
							if (match != Match.Empty)
							{
								return new EditLocationRange(i, 1 + match.Index,
									i, 1 + match.Index + match.Length);
							}
						}
					}
					return EditLocationRange.Empty;
				}
				else
				{
					rgOpt = rgOpt | RegexOptions.RightToLeft;
					if (lc.C > 1)
					{
						match = Regex.Match(LineList[lc.L-1].LineString.
							Substring(0, lc.C - 1), str, rgOpt);
						if (match != Match.Empty)
						{
							return new EditLocationRange(lc.L, 1 + match.Index,
								lc.L, 1 + match.Index + match.Length);
						}
					}
					for (int i = lc.L - 1; i > 0; i--)
					{
						match = Regex.Match(LineList[i-1].LineString, 
							str, rgOpt);
						if (match != Match.Empty)
						{
							return new EditLocationRange(i, 1 + match.Index,
								i, 1 + match.Index + match.Length);
						}
					}
					if (bWholeRange)
					{
						for (int i = LineList.Count; i >= lc.L; i--)
						{
							match = Regex.Match(LineList[i-1].LineString, 
								str, rgOpt);
							if (match != Match.Empty)
							{
								return new EditLocationRange(i, 1 + match.Index,
									i, 1 + match.Index + match.Length);
							}
						}
					}
					return EditLocationRange.Empty;
				}
			}
		}

		/// <summary>
		/// Finds the first occurance of the matching string.
		/// </summary>
		/// <param name="lc"></param>
		/// <param name="strBegin"></param>
		/// <param name="strEnd"></param>
		/// <param name="bMatchBegin"></param>
		/// <returns></returns>
		internal EditLocationRange FindMatch(EditLocation lc, string strBegin, 
			string strEnd, bool bMatchBegin)
		{
			if ((strBegin == string.Empty) || (strEnd == string.Empty) 
				|| (LineList.Count == 0))
			{
				return EditLocationRange.Empty;
			}
			else
			{
				string strTemp;
				int matchLevel = 0;
				RegexOptions rgOpt = RegexOptions.Singleline;
				strTemp	= Regex.Escape(strBegin) + "|" + Regex.Escape(strEnd);
				MatchCollection matches;
				int lnLength = LineList[lc.L-1].LineString.Length;
				if (bMatchBegin)
				{
					if (lc.C <= lnLength)
					{
						matches = Regex.Matches(LineList[lc.L-1].LineString.
							Substring(lc.C - 1), strTemp, rgOpt);
						for (int j = 0; j < matches.Count; j++)
						{
							if (matches[j].Value == strEnd)
							{
								if (matchLevel == 0)
								{
									return new EditLocationRange(lc.L, 
										lc.C + matches[j].Index, lc.L, 
										lc.C + matches[j].Index + matches[j].Length);
								}
								else
								{
									matchLevel--;
								}
							}
							else
							{
								matchLevel++;
							}
						}
					}
					int highLimit = Math.Min(LineList.Count, 
						lc.L + edit.MaxBraceMatchingLineInterval);
					for (int i = lc.L + 1; i <= highLimit; i++)
					{
						matches = Regex.Matches(LineList[i-1].LineString, 
							strTemp, rgOpt);
						for (int j = 0; j < matches.Count; j++)
						{
							if (matches[j].Value == strEnd)
							{
								if (matchLevel == 0)
								{
									return new EditLocationRange(i, 
										1 + matches[j].Index, i, 
										1 + matches[j].Index + matches[j].Length);
								}
								else
								{
									matchLevel--;
								}
							}
							else
							{
								matchLevel++;
							}
						}
					}
					return EditLocationRange.Empty;
				}
				else
				{
					rgOpt = rgOpt | RegexOptions.RightToLeft;
					if (lc.C > 1)
					{
						matches = Regex.Matches(LineList[lc.L-1].LineString.
							Substring(0, lc.C - 1), strTemp, rgOpt);
						for (int j = 0; j < matches.Count; j++)
						{
							if (matches[j].Value == strBegin)
							{
								if (matchLevel == 0)
								{
									return new EditLocationRange(lc.L, 
										1 + matches[j].Index, lc.L, 
										1 + matches[j].Index + matches[j].Length);
								}
								else
								{
									matchLevel--;
								}
							}
							else
							{
								matchLevel++;
							}
						}
					}
					int lowLimit = Math.Max(1, lc.L - edit.MaxBraceMatchingLineInterval);
					for (int i = lc.L - 1; i >= lowLimit; i--)
					{
						matches = Regex.Matches(LineList[i-1].LineString, 
							strTemp, rgOpt);
						for (int j = 0; j < matches.Count; j++)
						{
							if (matches[j].Value == strBegin)
							{
								if (matchLevel == 0)
								{
									return new EditLocationRange(i, 
										1 + matches[j].Index, i, 
										1 + matches[j].Index + matches[j].Length);
								}
								else
								{
									matchLevel--;
								}
							}
							else
							{
								matchLevel++;
							}
						}
					}
					return EditLocationRange.Empty;
				}
			}
		}

		/// <summary>
		/// Starts the background thread for outlining updating.
		/// </summary>
		internal void StartOutliningThread()
		{
			editOutliningThread = new Thread(editOutliningThreadStart);
			editOutliningThread.Priority = ThreadPriority.Lowest;
			bOutliningThreadContinue = true;
			editOutliningThread.Start();
		}

		/// <summary>
		/// Stops the background thread for outlining updating.
		/// </summary>
		internal void StopOutliningThread()
		{
			if (editOutliningThread != null)
			{
				if(editOutliningThread.IsAlive)
				{
					editOutliningThread.Abort();
				}
			}
		}

		/// <summary>
		/// Updates information for outlining objects.
		/// </summary>
		private void UpdateOutliningInfo()
		{
			OutliningRoot.RemoveAllDescendants();
			editCurrentOutlining = OutliningRoot;
			int lnLast = LineList.Count;
			for (int i = 1; i <= lnLast; i++)
			{
				if (!bOutliningThreadContinue)
				{
					return;
				}
				UpdateOutlining(i);
			}
			edit.RedrawSelectionMarginLater();
		}

		/// <summary>
		/// Update color information to the specified line.
		/// </summary>
		/// <param name="ln">The line to which color information will be 
		/// updated.</param>
		internal void UpdateLineInfo(int ln)
		{
			edit.UpdateMaxColumn(ln);
			if (edit.LineInfoUpdateActive)
			{
				edit.BeginUpdate();
				LineInfoUpdateEventArgs liue = new LineInfoUpdateEventArgs(ln);
				edit.RaiseLineInfoUpdate(liue);
				edit.EndUpdate();
				if (liue.Handled)
				{
					LineList[ln-1].HasUpdatedColor = true;
					return;
				}
			}
			if (!edit.SyntaxColoringEnabled)
			{
				LineList[ln-1].ColorTextData = null;
				LineList[ln-1].HasUpdatedColor = true;
				return;
			}
			int lnLength = GetLineLength(ln);
			UpdateMultiLineBlocks(ln, lnLength);
			EditLine editLn = LineList[ln-1];
			if (editLn.HasUpdatedColor)
			{
				return;
			}
			if (lnLength == 0)
			{
				editLn.ColorTextData = null;
				editLn.HasUpdatedColor = true;
				return;
			}
			if (!edit.HasMultiLineTag)
			{
				editColorInfoListTemp.Clear();
				GetColorInfo(ref editColorInfoListTemp, 
					editLn.LineString, 0, ln);
				editLn.ColorTextData = editColorInfoListTemp.GetDataArray();
				editLn.HasUpdatedColor = true;
				return;
			}
			short chStart1;
			short chEnd1;
			short tagIndex1;
			short colorIndex1;
			bool bAdvTag1;
			bool blnStart1;
			bool blnEnd1;
			short chStart2;
			short chEnd2;
			short tagIndex2;
			short colorIndex2;
			bool bAdvTag2;
			bool blnStart2;
			bool blnEnd2;
			string str = editLn.LineString;
			if (MultiLineBlockList.GetMultiLineColorInfo(ln, 1, 
				out chStart1, out chEnd1, out tagIndex1, out colorIndex1, 
				out bAdvTag1, out blnStart1, out blnEnd1))
			{	
				if (chEnd1 == lnLength)
				{
					if (!bAdvTag1)
					{
						editLn.ColorTextData = new short [] { colorIndex1 };
						editLn.HasUpdatedColor = true;
						return;
					}
					else
					{
						editColorInfoListTemp.Clear();
						EditAdvTagInfo ati = 
							(EditAdvTagInfo)editSettings.AdvTagInfoList[tagIndex1];
						if (blnStart1)
						{
							editColorInfoListTemp.Add(1, ati.BeginTag.Length,
								ati.ColorGroupIndex);
							if (lnLength > ati.BeginTag.Length)
							{
								if (ati.ToLineEnd == "1")
								{
									editColorInfoListTemp.Add(1 + ati.BeginTag.Length, 
										lnLength, ati.ColorGroupIndex);
								}
								else
								{
									GetInternalBlockColorInfo(ln, tagIndex1, 
										ref editColorInfoListTemp, 
										str.Substring(ati.BeginTag.Length), 
										ati.BeginTag.Length);
								}
							}
						}
						else if (blnEnd1)
						{
							editMatch = ati.EndRegex.Match(str);
							int tagStartIndex = editMatch.Index;
							int tagEndIndex = editMatch.Index + editMatch.Length;
							if (tagStartIndex == 0)
							{
								editColorInfoListTemp.Add(1, 
									tagEndIndex, ati.ColorGroupIndex);
								if (lnLength > ati.EndTag.Length)
								{
									editColorInfoListTemp.Add(1 + tagEndIndex, 
										lnLength, ati.ColorGroupIndex);
								}
							}
							else
							{
								GetInternalBlockColorInfo(ln, tagIndex1, 
									ref editColorInfoListTemp, 
									str.Substring(0, tagStartIndex), 0);
								editColorInfoListTemp.Add(1 + tagStartIndex, 
									tagEndIndex, ati.ColorGroupIndex);
								if (lnLength > tagEndIndex)
								{
									editColorInfoListTemp.Add(1 + tagEndIndex, 
										lnLength, ati.ColorGroupIndex);
								}
							}
						}
						else
						{
							GetInternalBlockColorInfo(ln, tagIndex1, 
								ref editColorInfoListTemp, str, 0);
						}
						editLn.ColorTextData = editColorInfoListTemp.GetDataArray();
						editLn.HasUpdatedColor = true;
						return;
					}
				}
				else
				{
					editColorInfoListTemp.Clear();
					if (!bAdvTag1)
					{
						editColorInfoListTemp.Add(1, chEnd1, colorIndex1);
					}
					else
					{
						EditAdvTagInfo ati = 
							(EditAdvTagInfo)editSettings.AdvTagInfoList[tagIndex1];
						editMatch = ati.EndRegex.Match(str);
						int tagStartIndex = editMatch.Index;
						int tagEndIndex = editMatch.Index + editMatch.Length;
						if (tagStartIndex == 0)
						{
							editColorInfoListTemp.Add(1, 
								ati.EndTag.Length, ati.ColorGroupIndex);
						}
						else
						{
							GetInternalBlockColorInfo(ln, tagIndex1, 
								ref editColorInfoListTemp, 
								str.Substring(0, tagStartIndex), 0);
							editColorInfoListTemp.Add(1 + tagStartIndex, 
								tagEndIndex, ati.ColorGroupIndex);
						}
					}
					if (MultiLineBlockList.GetMultiLineColorInfo(ln, 
						lnLength, out chStart2, out chEnd2, out tagIndex2, 
						out colorIndex2, out bAdvTag2, out blnStart2, 
						out blnEnd2))
					{
						if (chStart2 > chEnd1 + 1)
						{
							GetColorInfo(ref editColorInfoListTemp, 
								str.Substring(chEnd1, chStart2 - 1 - chEnd1), 
								chEnd1, ln);
						}
						if (!bAdvTag2)
						{
							editColorInfoListTemp.Add(chStart2, lnLength, colorIndex2);
						}
						else
						{
							EditAdvTagInfo ati = 
								(EditAdvTagInfo)editSettings.AdvTagInfoList[tagIndex2];
							editColorInfoListTemp.Add(chStart2, chStart2 
								+ ati.BeginTag.Length - 1, ati.ColorGroupIndex);
							if (lnLength > chStart2 + ati.BeginTag.Length - 1)
							{
								if (ati.ToLineEnd == "1")
								{
									editColorInfoListTemp.Add(chStart2 + ati.BeginTag.Length, 
										lnLength, ati.ContentColorGroupIndex);
								}
								else
								{
									GetInternalBlockColorInfo(ln, tagIndex2, 
										ref editColorInfoListTemp, 
										str.Substring(chStart2 + ati.BeginTag.Length - 1), 
										chStart2 + ati.BeginTag.Length - 1);
								}
							}
						}
					}
					else
					{
						GetColorInfo(ref editColorInfoListTemp, 
							str.Substring(chEnd1), chEnd1, ln);
					}
					editLn.ColorTextData = editColorInfoListTemp.GetDataArray();
					editLn.HasUpdatedColor = true;
					return;
				}
			}
			else if (MultiLineBlockList.GetMultiLineColorInfo(ln, lnLength, 
				out chStart2, out chEnd2, out tagIndex2, out colorIndex2, out bAdvTag2, 
				out blnStart2, out blnEnd2))
			{
				editColorInfoListTemp.Clear();
				GetColorInfo(ref editColorInfoListTemp, 
					str.Substring(0, chStart2 - 1), 0, ln);
				if (!bAdvTag2)
				{
					editColorInfoListTemp.Add(chStart2, lnLength, colorIndex2);
				}
				else
				{
					EditAdvTagInfo ati = 
						(EditAdvTagInfo)editSettings.AdvTagInfoList[tagIndex2];
					editColorInfoListTemp.Add(chStart2, chStart2 + ati.BeginTag.Length - 1,
						ati.ColorGroupIndex);
					if (lnLength > chStart2 + ati.BeginTag.Length - 1)
					{
						if (ati.ToLineEnd == "1")
						{
							editColorInfoListTemp.Add(chStart2 + ati.BeginTag.Length, 
								lnLength, ati.ColorGroupIndex);
						}
						else
						{
							GetInternalBlockColorInfo(ln, tagIndex2, ref editColorInfoListTemp, 
								str.Substring(chStart2 + ati.BeginTag.Length - 1), 
								chStart2 + ati.BeginTag.Length - 1);
						}
					}
				}
				editLn.ColorTextData = editColorInfoListTemp.GetDataArray();
				editLn.HasUpdatedColor = true;
				return;
			}
			else
			{
				editColorInfoListTemp.Clear();
				GetColorInfo(ref editColorInfoListTemp, str, 0, ln);
				editLn.ColorTextData = editColorInfoListTemp.GetDataArray();
				editLn.HasUpdatedColor = true;
				return;
			}
		}

		/// <summary>
		/// Tests if the specified line is highlighted.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <returns>true if the specified line is highlighted; otherwise, 
		/// false.</returns>
		internal bool IsHighlighted(int ln)
		{
			if (IsValidLine(ln))
			{
				return LineList[ln-1].Highlighted;
			}
			return false;
		}

		/// <summary>
		/// Highlights the specified line.
		/// </summary>
		/// <param name="ln">The line to be highlighted.</param>
		internal void AddHighlight(int ln)
		{
			if (IsValidLine(ln))
			{
				LineList[ln-1].Highlighted = true;
			}
		}

		/// <summary>
		/// Highlights the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line to be highlighted.</param>
		/// <param name="lnEnd">The ending line to be highlighted.</param>
		internal void AddHighlight(int lnStart, int lnEnd)
		{
			for (int i = lnStart; i <= lnEnd; i++)
			{
				AddHighlight(i);
			}
		}

		/// <summary>
		/// Removes highlight for the specified line.
		/// </summary>
		/// <param name="ln">The line for which highlight is to be removed.
		/// </param>
		internal void RemoveHighlight(int ln)
		{
			if (IsValidLine(ln))
			{
				LineList[ln-1].Highlighted = false;
			}
		}

		/// <summary>
		/// Removes highlight for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for which 
		/// highlight is to be removed.</param>
		/// <param name="lnEnd">The ending line of the line range for which  
		/// highlight is to be removed.</param>
		internal void RemoveHighlight(int lnStart, int lnEnd)
		{
			for (int i = lnStart; i <= lnEnd; i++)
			{
				RemoveHighlight(i);
			}
		}

		/// <summary>
		/// Removes all highlight for lines.
		/// </summary>
		internal void RemoveAllHighlight()
		{
			int lnLast = LineList.Count;
			for (int i = 1; i <= lnLast; i++)
			{
				LineList[i-1].Highlighted = false;
			}
		}

		/// <summary>
		/// Tests if the specified line has a custom foreground color.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <returns>true if the specified line has a custom foreground color; 
		/// otherwise, false.</returns>
		internal bool IsCustomForeColor(int ln)
		{
			if (IsValidLine(ln))
			{
				return LineList[ln-1].IsCustomForeColor;
			}
			return false;
		}

		/// <summary>
		/// Sets the foreground color of the specified line to be a custom color.
		/// </summary>
		/// <param name="ln">The line for which the foreground color is to be 
		/// set.</param>
		/// <param name="clr">The custom foreground color for the line.</param>
		internal void SetCustomForeColor(int ln, Color clr)
		{
			if (IsValidLine(ln))
			{
				LineList[ln-1].CustomForeColor = clr;
			}
		}

		/// <summary>
		/// Sets the foreground color of the specified line range to be a 
		/// custom color.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for which 
		/// the custom foreground color is to be set.</param>
		/// <param name="lnEnd">The ending line of the line range for which 
		/// the custom foreground color is to be set.</param>
		/// <param name="clr">The custom foreground color for the lines.</param>
		internal void SetCustomForeColor(int lnStart, int lnEnd, Color clr)
		{
			for (int i = lnStart; i <= lnEnd; i++)
			{
				SetCustomForeColor(i, clr);
			}
		}

		/// <summary>
		/// Removes the custom foreground color for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the custom foreground color 
		/// is to be removed.</param>
		internal void RemoveCustomForeColor(int ln)
		{
			if (IsValidLine(ln))
			{
				LineList[ln-1].RemoveCustomForeColor();
			}
		}

		/// <summary>
		/// Removes custom foreground colors for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for which 
		/// custom foreground colors is to be removed.</param>
		/// <param name="lnEnd">The ending line of the line range for which 
		/// custom foreground colors is to be removed.</param>
		internal void RemoveCustomForeColor(int lnStart, int lnEnd)
		{
			for (int i = lnStart; i <= lnEnd; i++)
			{
				RemoveCustomForeColor(i);
			}
		}

		/// <summary>
		/// Removes all the custom foreground colors for lines.
		/// </summary>
		internal void RemoveAllCustomForeColor()
		{
			int lnLast = LineList.Count;
			for (int i = 1; i <= lnLast; i++)
			{
				LineList[i-1].RemoveCustomForeColor();
			}
		}

		/// <summary>
		/// Tests if the specified line has a custom background color.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <returns>true if the specified line has a custom background color; 
		/// otherwise, false.</returns>
		internal bool IsCustomBackColor(int ln)
		{
			if (IsValidLine(ln))
			{
				return LineList[ln-1].IsCustomBackColor;
			}
			return false;
		}

		/// <summary>
		/// Sets the background color of the specified line to be a custom color.
		/// </summary>
		/// <param name="ln">The line for which the background color is to be 
		/// set.</param>
		/// <param name="clr">The custom background color for the line.</param>
		internal void SetCustomBackColor(int ln, Color clr)
		{
			if (IsValidLine(ln))
			{
				LineList[ln-1].CustomBackColor = clr;
			}
		}

		/// <summary>
		/// Sets the background color of the specified line range to be a 
		/// custom color.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for which 
		/// the custom background color is to be set.</param>
		/// <param name="lnEnd">The ending line of the line range for which 
		/// the custom background color is to be set.</param>
		/// <param name="clr">The custom background color for the lines.</param>
		internal void SetCustomBackColor(int lnStart, int lnEnd, Color clr)
		{
			for (int i = lnStart; i <= lnEnd; i++)
			{
				SetCustomBackColor(i, clr);
			}
		}

		/// <summary>
		/// Removes the custom background color for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the custom background color 
		/// is to be removed.</param>
		internal void RemoveCustomBackColor(int ln)
		{
			if (IsValidLine(ln))
			{
				LineList[ln-1].RemoveCustomBackColor();
			}
		}

		/// <summary>
		/// Removes custom background colors for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for which 
		/// custom background colors is to be removed.</param>
		/// <param name="lnEnd">The ending line of the line range for which 
		/// custom background colors is to be removed.</param>
		internal void RemoveCustomBackColor(int lnStart, int lnEnd)
		{
			for (int i = lnStart; i <= lnEnd; i++)
			{
				RemoveCustomBackColor(i);
			}
		}

		/// <summary>
		/// Removes all the custom background colors for lines.
		/// </summary>
		internal void RemoveAllCustomBackColor()
		{
			int lnLast = LineList.Count;
			for (int i = 1; i <= lnLast; i++)
			{
				LineList[i-1].RemoveCustomBackColor();
			}
		}

		/// <summary>
		/// Adds an indicator at the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="indicatorName"></param>
		/// <returns></returns>
		internal bool AddIndicator(int ln, string indicatorName)
		{
			if (!IsValidLine(ln))
			{
				return false;
			}
			int idcIndex;
			idcIndex = edit.GetIndicatorIndex(indicatorName);
			if (idcIndex == -1)
			{
				return false;
			}
			if (LineList[ln-1].IndicatorData == null)
			{
				LineList[ln-1].IndicatorData = 
					new short[] { (short)(- idcIndex) };
				return true;
			}
			else
			{
				int oldLength = LineList[ln-1].IndicatorData.Length;
				int insertIndex = oldLength;
				for (int i = GetIndicatorDataIndex(ln); i < oldLength; i++)
				{
					if (LineList[ln-1].IndicatorData[i] == - idcIndex)
					{
						return true;
					}
					else if (LineList[ln-1].IndicatorData[i] > - idcIndex)
					{
						insertIndex = i;
						break;
					}
				}
				short [] id = new short[oldLength + 1];
				if (insertIndex == 0)
				{
					id[0] = (short) - idcIndex;
					for (int i = 0; i < oldLength; i++)
					{
						id[i+1] = LineList[ln-1].IndicatorData[i];
					}
				}
				else if (insertIndex == oldLength)
				{
					for (int i = 0; i < oldLength; i++)
					{
						id[i] = LineList[ln-1].IndicatorData[i];
					}
					id[oldLength] = (short)-idcIndex;
				}
				else
				{
					for (int i = 0; i < insertIndex; i++)
					{
						id[i] = LineList[ln-1].IndicatorData[i];
					}
					id[insertIndex] = (short) - idcIndex;
					for (int i = insertIndex; i < oldLength; i++)
					{
						id[i + 1] = LineList[ln-1].IndicatorData[i];
					}
				}
				LineList[ln-1].IndicatorData = id;
				return true;
			}
		}

		/// <summary>
		/// Removes an indicator at the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="indicatorName"></param>
		/// <returns></returns>
		internal bool RemoveIndicator(int ln, string indicatorName)
		{
			if (!IsValidLine(ln))
			{
				return false;
			}
			int idcIndex;
			idcIndex = edit.GetIndicatorIndex(indicatorName);
			if (idcIndex == -1)
			{
				return false;
			}
			if (LineList[ln-1].IndicatorData == null)
			{
				return true;
			}
			else
			{
				int oldLength = LineList[ln-1].IndicatorData.Length;
				int removeIndex = -1;
				for (int i = GetIndicatorDataIndex(ln); i < oldLength; i++)
				{
					if (LineList[ln-1].IndicatorData[i] == - idcIndex)
					{
						removeIndex = i;
						break;
					}
				}
				if (removeIndex == -1)
				{
					return true;
				}
				if (oldLength == 1)
				{
					LineList[ln-1].IndicatorData = null;
					return true;
				}
				short [] id = new short[oldLength-1];
				if (removeIndex == 0)
				{
					for (int i = 1; i < oldLength; i++)
					{
						id[i-1] = LineList[ln-1].IndicatorData[i];
					}
				}
				else if (removeIndex == oldLength)
				{
					for (int i = 0; i < oldLength - 1; i++)
					{
						id[i] = LineList[ln-1].IndicatorData[i];
					}
				}
				else
				{
					for (int i = 0; i < removeIndex; i++)
					{
						id[i] = LineList[ln-1].IndicatorData[i];
					}
					for (int i = removeIndex + 1; i < oldLength; i++)
					{
						id[i-1] = LineList[ln-1].IndicatorData[i];
					}
				}
				LineList[ln-1].IndicatorData = id;
				return true;
			}
		}

		/// <summary>
		/// Tests if the specified indicator appears at the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="indicatorName"></param>
		/// <returns></returns>
		internal bool HasIndicator(int ln, string indicatorName)
		{
			if (!IsValidLine(ln))
			{
				return false;
			}
			if ((LineList[ln-1].IndicatorData == null) 
				|| (LineList[ln-1].IndicatorData.Length == 0))
			{
				return false;
			}
			for (int i = GetIndicatorDataIndex(ln); 
				i < LineList[ln-1].IndicatorData.Length; i++)
			{
				if (LineList[ln-1].IndicatorData[i] > 0)
				{
					return false;
				}
				else if (edit.GetIndicatorName(-LineList[ln-1].IndicatorData[i])
					== indicatorName)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Goes to the previous specified indicator before the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <returns></returns>
		internal int GetPreviousIndicator(int ln, string indicatorName)
		{
			if (!IsValidLine(ln))
			{
				return -1;
			}
			if (ln > 1)
			{
				for (int i = ln - 1; i > 0; i--)
				{
					if (HasIndicator(i, indicatorName))
					{
						return i;
					}
				}
			}
			for (int i = LineList.Count; i > ln; i--)
			{
				if (HasIndicator(i, indicatorName))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Goes to the next specified indicator before the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <returns></returns>
		internal int GetNextIndicator(int ln, string indicatorName)
		{
			if (!IsValidLine(ln))
			{
				return -1;
			}
			int lnLast = LineList.Count;
			if (ln < lnLast)
			{
				for (int i = ln + 1; i <= lnLast; i++)
				{
					if (HasIndicator(i, indicatorName))
					{
						return i;
					}
				}
			}
			for (int i = 1; i < ln; i++)
			{
				if (HasIndicator(i, indicatorName))
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Clears the specified indicator in all the lines.
		/// </summary>
		internal void ClearIndicator(string indicatorName)
		{
			int lnLast = LineList.Count;
			for (int i = 1; i <= lnLast; i++)
			{
				RemoveIndicator(i, indicatorName);
			}
		}

		/// <summary>
		/// Adds a wave line at the specified location range.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="chStart"></param>
		/// <param name="chEnd"></param>
		/// <param name="wlName"></param>
		internal void AddWaveLine(int ln, int chStart, int chEnd, string wlName)
		{
			if (chStart >= chEnd)
			{
				return;
			}
			if (!IsValidLine(ln))
			{
				return;
			}
			short cgi = (short)editSettings.GetColorGroupIndex(wlName);
			if (cgi < 0)
			{
				return;
			}
			EditLine editLn = LineList[ln-1];
			short chStartTemp = Math.Max((short)1, (short)chStart);
			short chEndTemp = Math.Min((short)editLn.LineString.Length, 
				(short)(chEnd-1));
			if ((editLn.IndicatorData == null) 
				|| (editLn.IndicatorData.Length == 0))
			{
				short [] sTemp = new short[3];
				sTemp[0] = chStartTemp;
				sTemp[1] = chEndTemp;
				sTemp[2] = cgi;
				editLn.IndicatorData = sTemp;
			}
			else
			{
				short [] sTemp = InsertWaveLineData(editLn.IndicatorData, 
					chStartTemp, chEndTemp, cgi);
				editLn.IndicatorData = sTemp;
			}
		}

		/// <summary>
		/// Updates the wave line data due to an insertion. 
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="chStart"></param>
		/// <param name="chEnd"></param>
		internal void UpdateWaveLineDataFromInsertion(int ln, int chStart, int chEnd)
		{
			if (chStart >= chEnd)
			{
				return;
			}
			EditLine editLn = LineList[ln-1];
			if ((editLn.IndicatorData == null) 
				|| (editLn.IndicatorData.Length == 0))
			{
				return;
			}
			else
			{
				int indexStart = GetWaveLineDataIndex(editLn.IndicatorData);
				if (indexStart == -1)
				{
					return;
				}
				else
				{
					short chStartTemp = Math.Max((short)1, (short)chStart);
					short chEndTemp = Math.Min((short)(editLn.LineString.Length + 1), 
						(short)chEnd);
					int chLen = chEndTemp - chStartTemp;
					for (int i = indexStart; i < editLn.IndicatorData.Length; i += 3)
					{
						if (editLn.IndicatorData[i] >= chStartTemp)
						{
							editLn.IndicatorData[i] += (short)chLen;
						}
						if (editLn.IndicatorData[i+1] >= chStartTemp)
						{
							editLn.IndicatorData[i+1] += (short)chLen;
						}
					}
				}
			}
		}

		/// <summary>
		/// Updates the wave line data due to a deletion. 
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="chStart"></param>
		/// <param name="chEnd"></param>
		internal void UpdateWaveLineFromDeletion(int ln, int chStart, int chEnd)
		{
			if (chStart >= chEnd)
			{
				return;
			}
			EditLine editLn = LineList[ln-1];
			if ((editLn.IndicatorData == null) 
				|| (editLn.IndicatorData.Length == 0))
			{
				return;
			}
			else
			{
				int indexStart = GetWaveLineDataIndex(editLn.IndicatorData);
				if (indexStart == -1)
				{
					return;
				}
				else
				{
					short chStartTemp = Math.Max((short)1, (short)chStart);
					short chEndTemp = Math.Min((short)(editLn.LineString.Length + 1), 
						(short)chEnd);
					for (int i = indexStart; i < editLn.IndicatorData.Length; i += 3)
					{
						if (editLn.IndicatorData[i] > chStartTemp)
						{
							if (editLn.IndicatorData[i] >= chEndTemp)
							{
								editLn.IndicatorData[i] -= (short)(chEndTemp - chStartTemp);
							}
							else
							{
								editLn.IndicatorData[i] = chStartTemp;
							}
						}
						if (editLn.IndicatorData[i+1] >= chStartTemp)
						{
							if (editLn.IndicatorData[i+1] >= chEndTemp)
							{
								editLn.IndicatorData[i+1] -= (short)(chEndTemp - chStartTemp);
							}
							else
							{
								editLn.IndicatorData[i+1] = (short)(chStartTemp - 1);
							}
						}
					}
					short [] sTemp = CleanWaveLineData(editLn.IndicatorData);
					editLn.IndicatorData = sTemp;
				}
			}
		}

		/// <summary>
		/// Adds wave lines for the specified location range.
		/// </summary>
		/// <param name="lcr">The location range of wave lines.</param>
		/// <param name="wlName">The color group name of wave lines.</param>
		internal void AddWaveLine(EditLocationRange lcr, string wlName)
		{
			if (lcr.Start.L == lcr.End.L)
			{
				AddWaveLine(lcr.Start.L, lcr.Start.C, lcr.End.C, wlName);
			}
			else
			{
				int lnLengthPlusOne = GetLineLengthPlusOne(lcr.Start.L);
				AddWaveLine(lcr.Start.L, lcr.Start.C, lnLengthPlusOne, wlName);
				for (int i = lcr.Start.L + 1; i < lcr.End.L; i++)
				{
					lnLengthPlusOne = GetLineLengthPlusOne(i);
					AddWaveLine(i, 1, lnLengthPlusOne, wlName);
				}
				AddWaveLine(lcr.End.L, 1, lcr.End.C, wlName);
			}
		}

		/// <summary>
		/// Inserts the data of a wave line into the wave line data array.
		/// </summary>
		internal short [] InsertWaveLineData(short [] origData,  
			short chStart, short chEnd, short cgi)
		{
			int indexStart = GetWaveLineDataIndex(origData);
			if (indexStart == -1)
			{
				short [] sTemp = new short[origData.Length + 3];
				origData.CopyTo(sTemp, 0);
				sTemp[origData.Length] = chStart;
				sTemp[origData.Length+1] = chEnd;
				sTemp[origData.Length+2] = cgi;
				return sTemp;
			}
			else
			{
				short [] sTemp = new short[origData.Length + 9];
				int sDataLength = indexStart;
				for (int i = 0; i < indexStart; i++)
				{
					sTemp[i] = origData[i];
				}
				bool bAdded = false;
				for (int i = indexStart; i < origData.Length; i += 3)
				{
					if (origData[i+1] < chStart)
					{
						sTemp[sDataLength++] = origData[i];
						sTemp[sDataLength++] = origData[i+1];
						sTemp[sDataLength++] = origData[i+2];
					}
					else if (origData[i] > chEnd)
					{
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
						sTemp[sDataLength++] = origData[i];
						sTemp[sDataLength++] = origData[i+1];
						sTemp[sDataLength++] = origData[i+2];
					}
					else if ((origData[i] >= chStart) && (origData[i+1] <= chEnd))
					{
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
					}
					else if ((origData[i] >= chStart) && (origData[i+1] > chEnd))
					{
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
						sTemp[sDataLength++] = (short)(chEnd + 1);
						sTemp[sDataLength++] = origData[i+1];
						sTemp[sDataLength++] = origData[i+2];
					}
					else if ((origData[i] < chStart) && (origData[i+1] <= chEnd))
					{
						sTemp[sDataLength++] = origData[i];
						sTemp[sDataLength++] = (short)(chStart - 1);
						sTemp[sDataLength++] = origData[i+2];
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
					}
					else if ((origData[i] < chStart) && (origData[i+1] > chEnd))
					{
						sTemp[sDataLength++] = origData[i];
						sTemp[sDataLength++] = (short)(chStart - 1);
						sTemp[sDataLength++] = origData[i+2];
						if (!bAdded)
						{
							sTemp[sDataLength++] = chStart;
							sTemp[sDataLength++] = chEnd;
							sTemp[sDataLength++] = cgi;
							bAdded = true;
						}
						sTemp[sDataLength++] = (short)(chEnd + 1);
						sTemp[sDataLength++] = origData[i+1];
						sTemp[sDataLength++] = origData[i+2];
					}
				}
				if (!bAdded)
				{
					sTemp[sDataLength++] = chStart;
					sTemp[sDataLength++] = chEnd;
					sTemp[sDataLength++] = cgi;
					bAdded = true;
				}
				short [] sTempResult = new short [sDataLength];
				for (int i = 0; i < sDataLength; i++)
				{
					sTempResult[i] = sTemp[i];
				}
				return PackWaveLineData(sTempResult);
			}
		}

		/// <summary>
		/// Removes all the wave lines at the specified line.
		/// </summary>
		/// <param name="ln"></param>
		internal void RemoveWaveLines(int ln)
		{
			RemoveWaveLines(ln, 1, GetLineLengthPlusOne(ln));
		}

		/// <summary>
		/// Removes wave lines for the specified range of a line.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="chStart"></param>
		/// <param name="chEnd"></param>
		internal void RemoveWaveLines(int ln, int chStart, int chEnd)
		{
			if (chStart >= chEnd)
			{
				return;
			}
			if (!IsValidLine(ln))
			{
				return;
			}
			EditLine editLn = LineList[ln-1];
			short chStartTemp = Math.Max((short)1, (short)chStart);
			short chEndTemp = Math.Min((short)editLn.LineString.Length, 
				(short)(chEnd-1));
			if ((editLn.IndicatorData == null) 
				|| (editLn.IndicatorData.Length == 0))			
			{
				return;
			}
			else
			{
				short [] sTemp = RemoveWaveLineData(editLn.IndicatorData, 
					chStartTemp, chEndTemp);
				editLn.IndicatorData = sTemp;
			}
		}

		/// <summary>
		/// Removes wave lines for the specified location range.
		/// </summary>
		/// <param name="lcr">The location range of wave lines.</param>
		internal void RemoveWaveLines(EditLocationRange lcr)
		{
			if (lcr.Start.L == lcr.End.L)
			{
				RemoveWaveLines(lcr.Start.L, lcr.Start.C, lcr.End.C);
			}
			else
			{
				int lnLengthPlusOne = GetLineLengthPlusOne(lcr.Start.L);
				RemoveWaveLines(lcr.Start.L, lcr.Start.C, lnLengthPlusOne);
				for (int i = lcr.Start.L + 1; i < lcr.End.L; i++)
				{
					lnLengthPlusOne = GetLineLengthPlusOne(i);
					RemoveWaveLines(i, 1, lnLengthPlusOne);
				}
				RemoveWaveLines(lcr.End.L, 1, lcr.End.C);
			}
		}

		/// <summary>
		/// Removes wave lines in the specified range from the wave line data array.
		/// </summary>
		/// <param name="origData">The original wave line data.</param>
		/// <param name="chStart">The starting char index.</param>
		/// <param name="chEnd">The ending char index.</param>
		internal short [] RemoveWaveLineData(short [] origData, short chStart, short chEnd)
		{
			int indexStart = GetWaveLineDataIndex(origData);
			if (indexStart == -1)
			{
				return origData;
			}
			short [] sTemp = new short[origData.Length + 9];
			for (int i = 0; i < indexStart; i++)
			{
				sTemp[i] = origData[i];
			}
			int sDataLength = indexStart;
			for (int i = indexStart; i < origData.Length; i += 3)
			{
				if (origData[i+1] < chStart)
				{
					sTemp[sDataLength++] = origData[i];
					sTemp[sDataLength++] = origData[i+1];
					sTemp[sDataLength++] = origData[i+2];
				}
				else if (origData[i] > chEnd)
				{
					sTemp[sDataLength++] = origData[i];
					sTemp[sDataLength++] = origData[i+1];
					sTemp[sDataLength++] = origData[i+2];
				}
				else if ((origData[i] >= chStart) && (origData[i+1] <= chEnd))
				{
					continue;
				}
				else if ((origData[i] >= chStart) && (origData[i+1] > chEnd))
				{
					sTemp[sDataLength++] = (short)(chEnd + 1);
					sTemp[sDataLength++] = origData[i+1];
					sTemp[sDataLength++] = origData[i+2];
				}
				else if ((origData[i] < chStart) && (origData[i+1] <= chEnd))
				{
					sTemp[sDataLength++] = origData[i];
					sTemp[sDataLength++] = (short)(chStart - 1);
					sTemp[sDataLength++] = origData[i+2];
				}
				else if ((origData[i] < chStart) && (origData[i+1] > chEnd))
				{
					sTemp[sDataLength++] = origData[i];
					sTemp[sDataLength++] = (short)(chStart - 1);
					sTemp[sDataLength++] = origData[i+2];
					sTemp[sDataLength++] = (short)(chEnd + 1);
					sTemp[sDataLength++] = origData[i+1];
					sTemp[sDataLength++] = origData[i+2];
				}
			}
			short [] sTempResult = new short [sDataLength];
			for (int i = 0; i < sDataLength; i++)
			{
				sTempResult[i] = sTemp[i];
			}
			int sIndexStart = -1;
			for (int i = 0; i < sTempResult.Length; i++)
			{
				if (sTempResult[i] > 0)
				{
					sIndexStart = i;
					break;
				}
			}
			if (sIndexStart == -1)
			{
				return sTempResult;
			}
			else
			{
				return PackWaveLineData(sTempResult);
			}
		}

		/// <summary>
		/// Adjusts the wave line data due to the starting location change.
		/// </summary>
		/// <param name="origData">The original wave line data.</param>
		/// <param name="chStartOld">The old starting char index.</param>
		/// <param name="chStartNew">The new starting char index.</param>
		internal void AdjustWaveLineData(short [] origData, 
			short chStartOld, short chStartNew)
		{
			int indexStart = GetWaveLineDataIndex(origData);
			if (indexStart == -1)
			{
				return;
			}
			short chLen = (short)(chStartNew - chStartOld);
			for (int i = indexStart; i < origData.Length; i += 3)
			{
				origData[i] += chLen;
				origData[i+1] += chLen;
			}
		}

		/// <summary>
		/// Gets a portion of the wave line data.
		/// </summary>
		/// <param name="ln">The line of the wave line data.</param>
		/// <param name="chStart">The starting char index.</param>
		/// <param name="chEnd">The ending char index.</param>
		/// <param name="bKeepPrefix">A value indicating whether to keep the 
		/// data before the wave line data.</param>
		/// <param name="bStartInMiddle">A value indicating whether the new 
		/// data start in the middle of the old data.</param>
		/// <param name="startCgi">The color group index of the starting char.
		/// </param>
		/// <param name="bEndInMiddle">A value indicating whether the new 
		/// data end in the middle of the old data.</param>
		/// <param name="endCgi">The color group index of the ending char.
		/// </param>
		/// <returns>The specified portion of the wave line data.</returns>
		internal short [] GetPartialWaveLineData(int ln, short chStart, 
			short chEnd, bool bKeepPrefix, ref bool bStartInMiddle, 
			ref short startCgi, ref bool bEndInMiddle, ref short endCgi)
		{
			bStartInMiddle = false;
			startCgi = -1;
			bEndInMiddle = false;
			endCgi = -1;
			short [] origData = LineList[ln-1].IndicatorData;
			int indexStart = GetWaveLineDataIndex(origData);
			if (indexStart == -1)
			{
				if (bKeepPrefix)
				{
					return origData;
				}
				else
				{
					return null;
				}
			}
			short [] sTemp = new short[origData.Length];
			int sDataLength;
			if (bKeepPrefix)
			{
				for (int i = 0; i < indexStart; i++)
				{
					sTemp[i] = origData[i];
				}
				sDataLength = indexStart;
			}
			else
			{
				sDataLength = 0;
			}
			for (int i = indexStart; i < origData.Length; i += 3)
			{
				if (origData[i+1] < chStart)
				{
					continue;
				}
				else if (origData[i] > chEnd)
				{
					continue;
				}
				else if ((origData[i] >= chStart) && (origData[i+1] <= chEnd))
				{
					sTemp[sDataLength++] = origData[i];
					sTemp[sDataLength++] = origData[i+1];
					sTemp[sDataLength++] = origData[i+2];
				}
				else if ((origData[i] >= chStart) && (origData[i+1] > chEnd))
				{
					sTemp[sDataLength++] = origData[i];
					sTemp[sDataLength++] = chEnd;
					sTemp[sDataLength++] = origData[i+2];
					bEndInMiddle = true;
					endCgi = origData[i+2];
				}
				else if ((origData[i] < chStart) && (origData[i+1] <= chEnd))
				{
					sTemp[sDataLength++] = chStart;
					sTemp[sDataLength++] = origData[i+1];
					sTemp[sDataLength++] = origData[i+2];
					bStartInMiddle = true;
					startCgi = origData[i+2];
				}
				else if ((origData[i] < chStart) && (origData[i+1] > chEnd))
				{
					sTemp[sDataLength++] = chStart;
					sTemp[sDataLength++] = chEnd;
					sTemp[sDataLength++] = origData[i+2];
					bStartInMiddle = true;
					startCgi = origData[i+2];
					bEndInMiddle = true;
					endCgi = origData[i+2];
				}
			}
			short [] sTempResult = new short [sDataLength];
			for (int i = 0; i < sDataLength; i++)
			{
				sTempResult[i] = sTemp[i];
			}
			return sTempResult;
		}

		/// <summary>
		/// Gets the index of the wave line data for the give data array.
		/// </summary>
		/// <param name="data">The data array for which the wave line data 
		/// index is to be found.</param>
		/// <returns>The index of the wave line data.</returns>
		internal int GetWaveLineDataIndex(short [] data)
		{
			int indexStart = -1;
			if (data != null)
			{
				for (int i = 0; i < data.Length; i++)
				{
					if (data[i] > 0)
					{
						indexStart = i;
						break;
					}
				}
			}
			return indexStart;
		}

		/// <summary>
		/// Gets the index of the indicator data at the specified line.
		/// </summary>
		/// <param name="ln">The line for which the index of the indicator data 
		/// is to be found.</param>
		/// <returns>The index of the indicator data.</returns>
		internal int GetIndicatorDataIndex(int ln)
		{
			int indexStart = 0;
			if (LineList[ln-1].IsCustomForeColor 
				|| LineList[ln-1].IsCustomBackColor)
			{
				indexStart = 8;
			}
			return indexStart;
		}

		/// <summary>
		/// Merges two sets of wave line data.
		/// </summary>
		/// <param name="origData1"></param>
		/// <param name="origData2"></param>
		/// <returns></returns>
		internal short [] MergeWaveLineData(short [] origData1, short [] origData2)
		{
			int indexStart = GetWaveLineDataIndex(origData2);
			if (indexStart == -1)
			{
				return origData1;
			}
			short [] sTemp;
			if (origData1 == null)
			{
				sTemp = new short[origData2.Length - indexStart];
				int mergeIndex = 0;
				for (int i = indexStart; i < origData2.Length; i += 3)
				{
					sTemp[mergeIndex++] = origData2[i];
					sTemp[mergeIndex++] = origData2[i+1];
					sTemp[mergeIndex++] = origData2[i+2];
				}
			}
			else
			{
				sTemp = new short[origData1.Length + origData2.Length - indexStart];
				origData1.CopyTo(sTemp, 0);
				int mergeIndex = origData1.Length;
				for (int i = indexStart; i < origData2.Length; i += 3)
				{
					sTemp[mergeIndex++] = origData2[i];
					sTemp[mergeIndex++] = origData2[i+1];
					sTemp[mergeIndex++] = origData2[i+2];
				}
			}
			return sTemp;
		}

		/// <summary>
		/// Removes the data for wave lines with zero/negative length.
		/// </summary>
		/// <param name="origData"></param>
		/// <returns></returns>
		internal short [] CleanWaveLineData(short [] origData)
		{
			int indexStart = GetWaveLineDataIndex(origData);
			if (indexStart == -1)
			{
				return origData;
			}
			short [] sTemp = new short[origData.Length];
			for (int i = 0; i < indexStart; i++)
			{
				sTemp[i] = origData[i];
			}
			int sDataLength = indexStart;
			for (int i = indexStart; i < origData.Length; i += 3)
			{
				if (origData[i] <= origData[i+1])
				{
					sTemp[sDataLength++] = origData[i];
					sTemp[sDataLength++] = origData[i+1];
					sTemp[sDataLength++] = origData[i+2];
				}
			}
			if (sDataLength == 0)
			{
				return null;
			}
			short [] sResult = new short[sDataLength];
			for (int i = 0; i < sDataLength; i++)
			{
				sResult[i] = sTemp[i];
			}
			return sResult;
		}

		/// <summary>
		/// Packs the adjacent wave lines that have the same color in the 
		/// given wave line data array.
		/// </summary>
		/// <param name="origData">The wave line data array to be packed.</param>
		/// <returns>The packed wave line data array.</returns>
		internal short [] PackWaveLineData(short [] origData)
		{
			int indexStart = GetWaveLineDataIndex(origData);
			if (indexStart == -1)
			{
				return origData;
			}
			short [] sTemp = new short[origData.Length];
			for (int i = 0; i < indexStart; i++)
			{
				sTemp[i] = origData[i];
			}
			sTemp[indexStart] = origData[indexStart];
			sTemp[indexStart+1] = origData[indexStart+1];
			sTemp[indexStart+2] = origData[indexStart+2];
			int sDataLength = indexStart+3;
			for (int i = indexStart+3; i < origData.Length; i += 3)
			{
				if ((origData[i+2] == sTemp[sDataLength-1])
					&& (origData[i] == sTemp[sDataLength-2] + 1))
				{
					sTemp[sDataLength - 2] = origData[i+1];
				}
				else
				{
					sTemp[sDataLength++] = origData[i];
					sTemp[sDataLength++] = origData[i+1];
					sTemp[sDataLength++] = origData[i+2];
				}
			}
			short [] sResult = new short[sDataLength];
			for (int i = 0; i < sDataLength; i++)
			{
				sResult[i] = sTemp[i];
			}
			return sResult;
		}

		/// <summary>
		/// Colors the specified string.
		/// </summary>
		/// <param name="cil"></param>
		/// <param name="str"></param>
		/// <param name="offsetIndex"></param>
		/// <param name="ln"></param>
		internal void GetColorInfo(ref EditColorInfoList cil, string str, 
			int offsetIndex, int ln)
		{
			if (LangRegex != null)
			{
				short tempIndex;
				editMatches = LangRegex.Matches(str);			
				for (int i = 0; i < editMatches.Count; i++)
				{
					if ((tempIndex = editSettings.
						GetAdvTagIndex(editMatches[i].Value)) != -1)
					{
						GetAdvTagBlockColorInfo(ln, tempIndex, ref cil, 
							editMatches[i].Value, offsetIndex + editMatches[i].Index);
					}
					else if ((tempIndex = editSettings.
						GetBeginTagColorIndex(editMatches[i].Value)) != -1)
					{
						cil.Add(1 + offsetIndex + editMatches[i].Index, 
							offsetIndex + editMatches[i].Index + editMatches[i].Length, 
							tempIndex);
					}
					else if ((tempIndex = (short)editSettings.
						GetKeywordColorIndex(editMatches[i].Value)) >= 0)
					{
						cil.Add(1 + offsetIndex + editMatches[i].Index, 
							offsetIndex + editMatches[i].Index + editMatches[i].Length, 
							tempIndex);
					}
					else
					{
						EditControl.ShowErrorMessage(editMatches[i].Value, 
							edit.GetResourceString("InvalidRegExpMatching"));
					}
				}
			}
		}

		/// <summary>
		/// Gets the color information for the string with the specified advanced tag.
		/// </summary>
		/// <param name="tagIndex"></param>
		/// <param name="cil"></param>
		/// <param name="str"></param>
		/// <param name="offsetIndex"></param>
		/// <param name="ln"></param>
		internal void GetAdvTagBlockColorInfo(int ln, short tagIndex, 
			ref EditColorInfoList cil, string str, int offsetIndex)
		{
			EditAdvTagInfo ati = 
				(EditAdvTagInfo)editSettings.AdvTagInfoList[tagIndex];
			if (ati.EndTag == string.Empty)
			{
				cil.Add(1 + offsetIndex, offsetIndex + ati.BeginTag.Length,
					ati.ColorGroupIndex);
				if (str.Length > ati.BeginTag.Length)
				{
					GetInternalBlockColorInfo(ln, tagIndex, ref cil, 
						str.Substring(ati.BeginTag.Length), 
						offsetIndex + ati.BeginTag.Length);
				}
			}
			else
			{
				if (editSettings.EndsWithAdvTag(str, tagIndex))
				{
					cil.Add(1 + offsetIndex, offsetIndex + ati.BeginTag.Length,
						ati.ColorGroupIndex);
					if (str.Length > ati.BeginTag.Length)
					{
						GetInternalBlockColorInfo(ln, tagIndex, ref cil, 
							str.Substring(ati.BeginTag.Length, 
							str.Length - ati.BeginTag.Length - ati.EndTag.Length), 
							offsetIndex + ati.BeginTag.Length);
					}
					cil.Add(1 + offsetIndex + str.Length - ati.EndTag.Length, 
						offsetIndex + str.Length,
						ati.ColorGroupIndex);
				}
				else
				{
					cil.Add(1 + offsetIndex, offsetIndex + ati.BeginTag.Length,
						ati.ColorGroupIndex);
					if (str.Length > ati.BeginTag.Length)
					{
						GetInternalBlockColorInfo(ln, tagIndex, ref cil, 
							str.Substring(ati.BeginTag.Length), 
							offsetIndex + ati.BeginTag.Length);
					}
				}
			}
		}

		/// <summary>
		/// Gets the color information for the string with the specified advanced tag.
		/// </summary>
		/// <param name="tagIndex"></param>
		/// <param name="cil"></param>
		/// <param name="str"></param>
		/// <param name="offsetIndex"></param>
		/// <param name="ln"></param>
		/// <param name="blnStart"></param>
		/// <param name="blnEnd"></param>
		internal void GetMultiLineAdvTagBlockColorInfo(short tagIndex, 
			ref EditColorInfoList cil, string str, int offsetIndex, int ln,
			bool blnStart, bool blnEnd)
		{
			EditAdvTagInfo ati = 
				(EditAdvTagInfo)editSettings.AdvTagInfoList[tagIndex];
			if (blnStart)
			{
				if (ati.ToLineEnd == "1")
				{
					cil.Add(1 + offsetIndex, offsetIndex + str.Length,
						ati.ColorGroupIndex);
				}
				else
				{
					cil.Add(1 + offsetIndex, offsetIndex + ati.BeginTag.Length,
						ati.ColorGroupIndex);
					if (str.Length > ati.BeginTag.Length)
					{
						GetInternalBlockColorInfo(ln, tagIndex, ref cil, 
							str.Substring(ati.BeginTag.Length), 
							offsetIndex + ati.BeginTag.Length);
					}
				}
			}
			else if (blnEnd)
			{
				if (ati.ToLineEnd == "1")
				{
					cil.Add(1 + offsetIndex, offsetIndex + str.Length,
						ati.ColorGroupIndex);
				}
			}
			if (editSettings.EndsWithAdvTag(str, tagIndex))
			{
				cil.Add(1 + offsetIndex, offsetIndex + ati.BeginTag.Length,
					ati.ColorGroupIndex);
				if (str.Length > ati.BeginTag.Length)
				{
					GetInternalBlockColorInfo(ln, tagIndex, ref cil, 
						str.Substring(ati.BeginTag.Length, 
						str.Length - ati.BeginTag.Length - ati.EndTag.Length), 
						offsetIndex + ati.BeginTag.Length);
				}
				cil.Add(1 + offsetIndex + str.Length - ati.EndTag.Length, 
					offsetIndex + str.Length,
					ati.ColorGroupIndex);
			}
			else
			{
				cil.Add(1 + offsetIndex, offsetIndex + ati.BeginTag.Length,
					ati.ColorGroupIndex);
				if (str.Length > ati.BeginTag.Length)
				{
					GetInternalBlockColorInfo(ln, tagIndex, ref cil, 
						str.Substring(ati.BeginTag.Length), 
						offsetIndex + ati.BeginTag.Length);
				}
			}
		}

		/// <summary>
		/// Gets the color information for the internal block of the 
		/// specified advanced tag.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="tagIndex"></param>
		/// <param name="cil"></param>
		/// <param name="str"></param>
		/// <param name="offsetIndex"></param>
		internal void GetInternalBlockColorInfo(int ln, short tagIndex, 
			ref EditColorInfoList cil, string str, int offsetIndex)
		{
			EditAdvTagInfo ati = (EditAdvTagInfo)edit.
				Settings.AdvTagInfoList[tagIndex];
			ati.SubMultiLineBlockList.ClearFrom(ln, offsetIndex + 1);
			int currentIndex = 0;
			if (ati.InternalRegex == null)
			{
				cil.Add(1 + offsetIndex + currentIndex, 
					offsetIndex + str.Length, ati.ContentColorGroupIndex);
				ati.SubMultiLineBlockList.LocationUpdated.L = ln;
				ati.SubMultiLineBlockList.LocationUpdated.C = offsetIndex + str.Length;
				return;
			}
			if (ati.SubMultiLineBlockList.MultiLineTagIndex == -1)
			{
				MatchCollection matches = ati.InternalRegex.Matches(str);
				int tempIndex;
				for (int i = 0; i < matches.Count; i++)
				{
					if (matches[i].Index > currentIndex)
					{
						cil.Add(1 + offsetIndex + currentIndex, 
							offsetIndex + matches[i].Index,
							ati.ContentColorGroupIndex);
					}
					if ((tempIndex = editSettings.
						GetKeywordColorIndex(tagIndex, matches[i].Value)) >= 0)
					{
						cil.Add(1 + offsetIndex + matches[i].Index, 
							offsetIndex + matches[i].Index + matches[i].Length, 
							tempIndex);
					}
					else
					{
						cil.Add(1 + offsetIndex + matches[i].Index, 
							offsetIndex + matches[i].Index + matches[i].Length,
							editSettings.GetSubTagColorIndex(ati, matches[i].Value));
					}
					currentIndex = matches[i].Index + matches[i].Length;
					if (i == matches.Count - 1)
					{
						short iTemp = 
							editSettings.GetUnfinishedSubTag(ati, matches[i].Value);
						if (iTemp != -1)
						{
							ati.SubMultiLineBlockList.Add(new EditLocation(ln, 
								offsetIndex + 1), 
								EditLocation.Infinite,
								ati.ColorGroupIndex, 
								iTemp, false);
							ati.SubMultiLineBlockList.MultiLineTagIndex = iTemp;
						}
					}
				}
				if (currentIndex < str.Length)
				{
					cil.Add(1 + offsetIndex + currentIndex, 
						offsetIndex + str.Length,
						ati.ContentColorGroupIndex);
				}
				ati.SubMultiLineBlockList.LocationUpdated.L = ln;
				ati.SubMultiLineBlockList.LocationUpdated.C = offsetIndex + str.Length;
			}
			else
			{
				int chEnd;
				if (HasMultiLineEndSubTag(tagIndex, ati.SubMultiLineBlockList.
					MultiLineTagIndex, str, out chEnd))
				{
					cil.Add(1 + offsetIndex, offsetIndex + chEnd,
						((EditTagInfo)ati.SubTagInfoList[ati.SubMultiLineBlockList.
						MultiLineTagIndex]).ColorGroupIndex);
					ati.SubMultiLineBlockList[ati.SubMultiLineBlockList.Count-1].End 
						= new EditLocation(ln, offsetIndex + chEnd);
					ati.SubMultiLineBlockList.MultiLineTagIndex = -1;
					if (chEnd < str.Length)
					{
						GetInternalBlockColorInfo(ln, tagIndex, ref cil, 
							str.Substring(chEnd), offsetIndex + chEnd);
					}
				}
				else
				{
					cil.Add(1 + offsetIndex, offsetIndex + str.Length,
						((EditTagInfo)ati.SubTagInfoList[ati.SubMultiLineBlockList.
						MultiLineTagIndex]).ColorGroupIndex);
				}
				ati.SubMultiLineBlockList.LocationUpdated.L = ln;
				ati.SubMultiLineBlockList.LocationUpdated.C = offsetIndex + str.Length;
			}
		}

		/// <summary>
		/// Checks whether a multiline tag appears in the specified 
		/// character range at the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="chStart"></param>
		/// <param name="chEnd"></param>
		internal void CheckMultiLineBlock(int ln, int chStart, int chEnd)
		{
			if ((chStart > chEnd) || chStart > GetLineLength(ln))
			{
				return;
			}
			short mlti;
			int chTemp;
			bool isAdvTag;
			string str = LineList[ln-1].LineString.Substring(chStart - 1, 
				chEnd - chStart + 1);
			if (MultiLineBlockList.MultiLineTagIndex == -1)
			{
				mlti = GetMultiLineBeginTagIndex(str, out chTemp, out isAdvTag);
				if (mlti != -1)
				{
					MultiLineBlockList.MultiLineTagIndex = mlti;
					short cgi = isAdvTag ? 
						((EditAdvTagInfo)(editSettings.AdvTagInfoList[mlti])).ColorGroupIndex :
						((EditTagInfo)(editSettings.TagInfoList[mlti])).ColorGroupIndex;
					MultiLineBlockList.Add(new EditLocation(ln, 
						chStart - 1 + chTemp), EditLocation.Infinite,
						cgi, (short) mlti, isAdvTag);
				}
			}
			else
			{
				if (HasMultiLineEndTag(MultiLineBlockList.MultiLineTagIndex,
					MultiLineBlockList[MultiLineBlockList.Count-1].IsAdvTag,
					str, out chTemp))
				{
					MultiLineBlockList[MultiLineBlockList.Count-1].End 
						= new EditLocation(ln, chStart - 1 + chTemp);
					MultiLineBlockList.MultiLineTagIndex = -1;
					if (chStart - 1 + chTemp < chEnd)
					{
						int chStartNew = chStart - 1 + chTemp + 1;
						str = LineList[ln-1].LineString.Substring(
							chStartNew - 1, chEnd - chStartNew + 1);
						mlti = GetMultiLineBeginTagIndex(str, out chTemp, out isAdvTag);
						if (mlti != -1)
						{
							MultiLineBlockList.MultiLineTagIndex = mlti;
							short cgi = isAdvTag ? 
								((EditAdvTagInfo)(editSettings.AdvTagInfoList[mlti])).ColorGroupIndex :
								((EditTagInfo)(editSettings.TagInfoList[mlti])).ColorGroupIndex;
							MultiLineBlockList.Add(new EditLocation(ln, 
								chStartNew - 1 + chTemp), EditLocation.Infinite,
								cgi, (short) mlti, isAdvTag);
						}
					}
				}
			}
			MultiLineBlockList.LocationUpdated.L = ln;
			MultiLineBlockList.LocationUpdated.C = chEnd;
		}

		/// <summary>
		/// Marks the updated flag to make color information of the
		/// specified line range invalid.
		/// </summary>
		/// <param name="lcStart"></param>
		/// <param name="lcEnd"></param>
		internal void InvalidateRangeInfo(EditLocation lcStart, EditLocation lcEnd)
		{
			if (LineList.Count <= 0)
			{
				return;
			}
			int lnStart = lcStart.L;
			int lnEnd = lcEnd.L;
			if (!edit.HasMultiLineTag)
			{
				ClearColorInfo(lnStart, lnEnd);
				return;
			}
			if (lnStart != lnEnd)
			{
				ClearColorInfoFrom(lnStart);
				return;
			}
			if (MultiLineBlockList.GetMultiLineBlockIndex(lnStart) != -1)
			{
				ClearColorInfoFrom(lnStart);
				return;
			}
			if (edit.SyntaxColoringEnabled && edit.HasMultiLineTag)
			{
				editMatch = MultiLineBeginTagsRegex.Match(LineList[lnStart-1].LineString);
				if (editMatch != Match.Empty)
				{
					ClearColorInfoFrom(lnStart);
					return;
				}
			}
			EditAdvTagInfo ati;
			for (int i = 0; i < editSettings.AdvTagInfoList.Count; i++)
			{
				ati = (EditAdvTagInfo)editSettings.AdvTagInfoList[i];
				if (ati.SubMultiLineBlockList.GetMultiLineBlockIndex(lnStart) != -1)
				{
					ClearColorInfoFrom(lnStart);
					return;
				}
				editMatch = Regex.Match(LineList[lnStart-1].LineString, 
					editSettings.GetAdvTagMultiLineSubBeginTagsRegExp(ati), 
					RegExpOptions);
				if (editMatch != Match.Empty)
				{
					ClearColorInfoFrom(lnStart);
					return;
				}
			}
			ClearColorInfo(lnStart, lnEnd);
		}

		/// <summary>
		/// Clears color information in the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for 
		/// which the color information is to be cleared.</param>
		/// <param name="lnEnd">The ending line of the line range for 
		/// which the color information is to be cleared.</param>
		internal void ClearColorInfo(int lnStart, int lnEnd)
		{
			int lnLast = Math.Min(lnEnd, LineList.Count);
			for (int i = lnStart; i <= lnLast; i++)
			{
				LineList[i-1].ColorTextData = null;
				LineList[i-1].HasUpdatedColor = false;
			}
			if (edit.AutomaticOutliningEnabled)
			{
				StopOutliningThread();
				StartOutliningThread();
			}
		}

		/// <summary>
		/// Clears color information from the specified line.
		/// </summary>
		/// <param name="ln">The line from which color information is to 
		/// be cleared.</param>
		internal void ClearColorInfoFrom(int ln)
		{
			ClearMultiLineBlockFrom(ln, 0);
			LineList[ln-1].HasUpdatedColor = false;
			if (edit.AutomaticOutliningEnabled)
			{
				StopOutliningThread();
				StartOutliningThread();
			}
		}

		/// <summary>
		/// Updates outlining information up to the specified line.
		/// </summary>
		/// <param name="ln">The line to which the color information is
		/// to be updated.</param>
		internal void UpdateOutlining(int ln)
		{
			if (!edit.AutomaticOutliningEnabled)
			{
				return;
			}
			if (editOutliningLineUpdated >= ln)
			{
				return;
			}
			for (int i = editOutliningLineUpdated + 1; i <= ln; i++)
			{
				CheckOutlining(i);
			}
		}

		/// <summary>
		/// Tests if the specified line is in an outlining object.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		internal void CheckOutlining(int ln)
		{
			string str = LineList[ln-1].LineString;
			if ((str == string.Empty) || (IsSpacesOrTabsOnly(str)))
			{
				ExtendPreviousOutlining(ln);
				return;
			}
			bool bWithComment = false;
			string strTemp = GetEffectiveString(ln, ref bWithComment);
			if (strTemp == string.Empty)
			{
				if (bWithComment)
				{
					AddOutliningLine(ln, CommentOutliningTag);
				}
				return;
			}
			if (IsSpacesOrTabsOnly(strTemp))
			{
				if (!bWithComment)
				{
					ExtendPreviousOutlining(ln);
				}
				else
				{
					AddOutliningLine(ln, CommentOutliningTag);
				}
				return;
			}
			if (editCurrentOutlining != OutliningRoot) 
			{
				if (HasOutliningEndTag(editCurrentOutlining, strTemp))
				{
					if (editCurrentOutlining.InternalLevel == 0)
					{
						TerminateCurrentOutlining(ln);
						return;
					}
					else
					{
						editCurrentOutlining.InternalLevel--;
					}
				}
				else
				{
					if (SingleLineCommentRegex != null)
					{
						editMatch = SingleLineCommentRegex.Match(strTemp);
						if (editMatch != Match.Empty)
						{
							AddOutliningLine(ln, CommentOutliningTag);
							return;
						}
					}
					if (SingleLineOutliningRegex != null)
					{
						editMatch = SingleLineOutliningRegex.Match(strTemp);
						if (editMatch != Match.Empty)
						{
							AddOutliningLine(ln, editSettings.GetOutliningTag(editMatch.Value));
							return;
						}
					}
					if (MultiLineOutliningBeginTagCheckingRegex != null)
					{
						editMatch = MultiLineOutliningBeginTagCheckingRegex.Match(strTemp);
						EditOutliningTagInfo oti;
						while (editMatch != Match.Empty)
						{
							oti = editSettings.GetOutliningTag(editMatch.Value);
							if (oti != null)
							{
								AddOutliningLine(ln, oti);
								return;
							}
							editMatch = editMatch.NextMatch();
						}
					}
				}
//				LineList[ln-1].Outlining = editCurrentOutlining;
				editOutliningLineUpdated = ln;
				return;
			}
			if (SingleLineCommentRegex != null)
			{
				editMatch = SingleLineCommentRegex.Match(strTemp);
				if (editMatch != Match.Empty)
				{
					AddOutliningLine(ln, CommentOutliningTag);
					return;
				}
			}
			if (SingleLineOutliningRegex != null)
			{
				editMatch = SingleLineOutliningRegex.Match(strTemp);
				if (editMatch != Match.Empty)
				{
					AddOutliningLine(ln, editSettings.GetOutliningTag(editMatch.Value));
					return;
				}
			}
			if (MultiLineOutliningBeginTagCheckingRegex != null)
			{
				editMatch = MultiLineOutliningBeginTagCheckingRegex.Match(strTemp);
				EditOutliningTagInfo oti;
				while (editMatch != Match.Empty)
				{
					oti = editSettings.GetOutliningTag(editMatch.Value);
					if (oti != null)
					{
						AddOutliningLine(ln, oti);
						return;
					}
					editMatch = editMatch.NextMatch();
				}
			}
//			LineList[ln-1].Outlining = editCurrentOutlining;
			editOutliningLineUpdated = ln;
		}

		/// <summary>
		/// Gets the effective string at the specified line (the string 
		/// outside multiline blocks.
		/// </summary>
		/// <param name="ln">The line for which the effective string is to
		/// be obtained.</param>
		/// <param name="bWithComment">A value indicating whether the 
		/// effective string is with comment.</param>
		/// <returns></returns>
		internal string GetEffectiveString(int ln, ref bool bWithComment)
		{
			string strTemp = string.Empty;
			if (!edit.HasMultiLineTag)
			{
				bWithComment = false;
				strTemp = LineList[ln-1].LineString;
				return strTemp;
			}
			int lnLength = GetLineLength(ln);
			short chStart1;
			short chEnd1;
			short tagIndex1;
			short chStart2;
			short chEnd2;
			short tagIndex2;
			if (MultiLineBlockList.GetMultiLineBlockInfo(ln, 1, 
				out chStart1, out chEnd1, out tagIndex1))
			{	
				if (chEnd1 == lnLength)
				{
					bWithComment = editSettings.IsMultiLineCommentTag(tagIndex1);
					return strTemp;
				}
				else
				{
					if (MultiLineBlockList.GetMultiLineBlockInfo(ln, 
						lnLength, out chStart2, out chEnd2, out tagIndex2))
					{
						bWithComment = (editSettings.IsMultiLineCommentTag(tagIndex1))
							&& (editSettings.IsMultiLineCommentTag(tagIndex2));
						strTemp = LineList[ln-1].LineString.
							Substring(chEnd1, chStart2 - 1 - chEnd1);
						return strTemp;
					}
					else
					{
						bWithComment = editSettings.IsMultiLineCommentTag(tagIndex1);
						strTemp = LineList[ln-1].LineString.Substring(chEnd1);
						return strTemp;
					}
				}
			}
			else if (MultiLineBlockList.GetMultiLineBlockInfo(ln, lnLength, 
				out chStart1, out chEnd1, out tagIndex1))
			{
				bWithComment = editSettings.IsMultiLineCommentTag(tagIndex1);
				strTemp = LineList[ln-1].LineString.Substring(0, chStart1 - 1);
				return strTemp;
			}
			else
			{
				bWithComment = false;
				strTemp = LineList[ln-1].LineString;
				return strTemp;
			}
		}

		/// <summary>
		/// Extends the previous outlining object before the specified line.
		/// </summary>
		/// <param name="ln"></param>
		internal void ExtendPreviousOutlining(int ln)
		{
			if (ln <= 1)
			{
				return;
			}
			EditOutlining otln = GetLeafOutlining(ln - 1); // LineList[ln-2].Outlining;
			if (otln.IsRoot)
			{
				return;
			}
			if (otln.OutliningTag == CommentOutliningTag)
			{
				otln.End.L = ln;
				otln.End.C = GetLineLengthPlusOne(ln);
			}
			editOutliningLineUpdated = ln;
		}

		/// <summary>
		/// Terminates the current outlining object at the specified line.
		/// </summary>
		/// <param name="ln"></param>
		internal void TerminateCurrentOutlining(int ln)
		{
			if (editCurrentOutlining == OutliningRoot)
			{
				return;
			}
			if (editCurrentOutlining.EndLine == -1)
			{
				editCurrentOutlining.End.L = ln;
				editCurrentOutlining.End.C = GetLineLengthPlusOne(ln);
			}
			editOutliningLineUpdated = ln;
			editCurrentOutlining = editCurrentOutlining.ParentOutlining;
		}

		/// <summary>
		/// Adds an outlining line with the specified tag information object.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="oti"></param>
		internal void AddOutliningLine(int ln, EditOutliningTagInfo oti)
		{
			EditOutlining otln = editCurrentOutlining;
			if (!otln.IsRoot)
			{
//				if (!otln.IsSubOutlining(oti))
				{
					editOutliningLineUpdated = ln;
					return;
				}
			}
			if (oti == CommentOutliningTag)
			{
				if (otln.ChildCount > 0)
				{
					if ((otln[otln.ChildCount-1].EndLine == ln - 1) && 
						(otln[otln.ChildCount-1].OutliningTag == oti))
					{
						otln[otln.ChildCount-1].End.L = ln;
						otln[otln.ChildCount-1].End.C = GetLineLengthPlusOne(ln);
					}
					else
					{
						EditOutlining otlnNew = otln.AddChild(new EditLocation(ln, 1), 
							new EditLocation(ln, GetLineLengthPlusOne(ln)), oti);
					}
				}
				else
				{
					EditOutlining otlnNew = otln.AddChild(new EditLocation(ln, 1), 
						new EditLocation(ln, GetLineLengthPlusOne(ln)), oti);
				}
			}
			else
			{
				if (oti.MultiLine != "1")
				{
					if (otln.ChildCount > 0)
					{
						if ((otln[otln.ChildCount-1].EndLine == ln - 1) && 
							(otln[otln.ChildCount-1].OutliningTag == oti))
						{
							otln[otln.ChildCount-1].End.L = ln;
							otln[otln.ChildCount-1].End.C = GetLineLengthPlusOne(ln);
						}
						else
						{
							EditOutlining otlnNew = otln.AddChild(new EditLocation(ln, 1), 
								new EditLocation(ln, GetLineLengthPlusOne(ln)), oti);
						}
					}
					else
					{
						EditOutlining otlnNew = otln.AddChild(new EditLocation(ln, 1), 
							new EditLocation(ln, GetLineLengthPlusOne(ln)), oti);
					}
				}
				else
				{
					editCurrentOutlining = otln.AddChild(new EditLocation(ln, 1), 
						new EditLocation(-1, -1), oti);
				}
			}
			editOutliningLineUpdated = ln;
		}

		/// <summary>
		/// Hides the specified range as an outlining object.
		/// </summary>
		internal bool HideAsOutlining(EditLocationRange lcr)
		{
			EditLocationRange lcrNorm = lcr.Normalize();
			EditOutlining otln1 = GetLeafOutlining(lcrNorm.Start.L);
			EditOutlining otln2 = GetLeafOutlining(lcrNorm.End.L);
			if (otln1 != otln2)
			{
				EditOutlining otln3 = otln1.GetSharedAncestor(otln2);
				otln3.RemoveDescendants(lcrNorm.Start.L);
				otln3.RemoveDescendants(lcrNorm.End.L);
				EditOutlining otlnNew;
				otlnNew = otln3.AddChild(lcrNorm.Start, lcrNorm.End, ManualOutliningTag);
				otlnNew.Collapsed = true;
				return (otlnNew != null);
			}
			else
			{
				if ((otln1.IsBoundaryLine(lcrNorm.Start.L)) || 
					(otln1.IsBoundaryLine(lcrNorm.End.L)))
				{
					EditOutlining otln3 = otln1.ParentOutlining;
					otln3.Remove(otln1, true);
					otln1 = otln3;
				}
				EditOutlining otlnNew;
				otlnNew = otln1.AddChild(lcrNorm.Start, lcrNorm.End, ManualOutliningTag);
				otlnNew.Collapsed = true;
				return (otlnNew != null);
			}
		}

		/// <summary>
		/// Removes the outlining at the specified line. 
		/// </summary>
		/// <param name="ln">The line at which the outlining will be removed.
		/// </param>
		internal void StopHidingCurrent(int ln)
		{
			EditOutlining otln = GetLeafOutlining(ln);
			if (!otln.IsRoot)
			{
				EditOutlining otlnParent = otln.ParentOutlining;
				otlnParent.Remove(otln, true);
			}
		}

		/// <summary>
		/// Updates multiline block information to the specified location.
		/// </summary>
		/// <param name="ln">The line of the location to which the multiline 
		/// information will be updated to.</param>
		/// <param name="ch">The char of the location to which the multiline 
		/// information will be updated to.</param>
		internal void UpdateMultiLineBlocks(int ln, int ch)
		{
			if (!(edit.SyntaxColoringEnabled && edit.HasMultiLineTag))
			{
				return;
			}
			if (MultiLineBlockList.LocationUpdated.GreaterThanOrEqualTo(ln, ch))
			{
				return;
			}
			EditLocation lcStart;
			if (MultiLineBlockList.LocationUpdated == EditLocation.None)
			{
				lcStart = new EditLocation(edit.FirstLineChar);
			}
			else
			{
				if (MultiLineBlockList.LocationUpdated.C >= 
					GetLineLength(MultiLineBlockList.LocationUpdated.L))
				{
					lcStart = new EditLocation(MultiLineBlockList.
						LocationUpdated.L + 1, 1);
				}
				else
				{
					lcStart = new EditLocation(MultiLineBlockList.
						LocationUpdated.L, MultiLineBlockList.
						LocationUpdated.C + 1);
				}
			}
			if ((lcStart > edit.LastLineChar) || (lcStart.GreaterThan(ln, ch)))
			{
				return;
			}
			if (lcStart.L == ln)
			{
				if (GetLineLength(lcStart.L) == 0)
				{
					MultiLineBlockList.LocationUpdated.L = lcStart.L;
					MultiLineBlockList.LocationUpdated.C = 1;
					LineList[lcStart.L-1].ColorTextData = null;
					LineList[lcStart.L-1].HasUpdatedColor = true;
				}
				else
				{
					CheckMultiLineBlock(lcStart.L, lcStart.C, ch);
					LineList[lcStart.L-1].HasUpdatedColor = false;
				}
			}
			else
			{
				if (GetLineLength(lcStart.L) == 0)
				{
					MultiLineBlockList.LocationUpdated.L = lcStart.L;
					MultiLineBlockList.LocationUpdated.C = 1;
					LineList[lcStart.L-1].ColorTextData = null;
					LineList[lcStart.L-1].HasUpdatedColor = true;
				}
				else
				{
					CheckMultiLineBlock(lcStart.L, lcStart.C, 
						GetLineLength(lcStart.L));
					LineList[lcStart.L-1].HasUpdatedColor = false;
				}
				for (int i = lcStart.L + 1; i < ln; i++)
				{
					if (GetLineLength(i) == 0)
					{
						MultiLineBlockList.LocationUpdated.L = i;
						MultiLineBlockList.LocationUpdated.C = 1;
						LineList[i-1].ColorTextData = null;
						LineList[i-1].HasUpdatedColor = true;
					}
					else
					{
						CheckMultiLineBlock(i, 1, GetLineLength(i));
						LineList[i-1].HasUpdatedColor = false;
					}
				}
				if (GetLineLength(ln) == 0)
				{
					MultiLineBlockList.LocationUpdated.L = ln;
					MultiLineBlockList.LocationUpdated.C = 1;
					LineList[ln-1].ColorTextData = null;
					LineList[ln-1].HasUpdatedColor = true;
				}
				else
				{
					CheckMultiLineBlock(ln, 1, ch);
					LineList[ln-1].HasUpdatedColor = false;
				}
			}
		}

		/// <summary>
		/// Tests if there is a multiline begin tag in the specified string.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="chStart"></param>
		/// <returns></returns>
		internal short GetMultiLineBeginTagIndex(string str, out int chStart, 
			out bool isAdvTag)
		{
			editMatch = MultiLineBeginTagsRegex.Match(str);
			if (editMatch != Match.Empty)
			{
				editMatch = BeginTagCheckingRegex.Match(str);
				while (editMatch != Match.Empty)
				{
					if (editSettings.GetMultiLineAdvBeginTagOnlyColorIndex(
						editMatch.Value) != -1)
					{
						isAdvTag = true;
						chStart = 1 + editMatch.Index;
						return editSettings.GetAdvTagIndex(editMatch.Value);
					}
					else if (editSettings.GetMultiLineBeginTagOnlyColorIndex(
						editMatch.Value) != -1)
					{
						isAdvTag = false;
						chStart = 1 + editMatch.Index;
						return editSettings.GetTagIndex(editMatch.Value);
					}
					editMatch = editMatch.NextMatch();
				}			
			}
			isAdvTag = false;
			chStart = -1;
			return -1;
		}

		/// <summary>
		/// Checks if there is an end tag in the specified string for the 
		/// specified multiline tag index.
		/// </summary>
		/// <param name="mlti"></param>
		/// <param name="bAdvTag"></param>
		/// <param name="str"></param>
		/// <param name="chEnd"></param>
		/// <returns></returns>
		internal bool HasMultiLineEndTag(int mlti, bool bAdvTag, string str, 
			out int chEnd)
		{
			if (!bAdvTag)
			{
				editMatch = ((EditTagInfo)(editSettings.TagInfoList[mlti])).
					EndRegex.Match(str);
			}
			else
			{
				EditAdvTagInfo ati = (EditAdvTagInfo)
					editSettings.AdvTagInfoList[mlti];
				if (ati.ToLineEnd == "1")
				{
					string matchStr;
					matchStr = "(?:" + Regex.Escape(ati.EndTag) + ".*$)";
					editMatch = Regex.Match(str, matchStr, RegExpOptions);
				}
				else
				{
					editMatch = ati.EndRegex.Match(str);
				}
			}
			if (editMatch != Match.Empty)
			{
				chEnd = editMatch.Index + editMatch.Length;
				return true;
			}
			chEnd = -1;
			return false;
		}

		/// <summary>
		/// Checks if there is an end tag in the specified string for the 
		/// specified subtag index of the specified advanced tag.
		/// </summary>
		/// <param name="tagIndex"></param>
		/// <param name="subTagIndex"></param>
		/// <param name="str"></param>
		/// <param name="chEnd"></param>
		/// <returns></returns>
		internal bool HasMultiLineEndSubTag(short tagIndex, short subTagIndex, 
			string str, out int chEnd)
		{
			EditAdvTagInfo ati = (EditAdvTagInfo)editSettings.AdvTagInfoList[tagIndex];
			string matchStr = Regex.Escape(((EditTagInfo)ati.
				SubTagInfoList[subTagIndex]).EndTag);
			editMatch = Regex.Match(str, matchStr, RegExpOptions);
			if (editMatch != Match.Empty)
			{
				chEnd = editMatch.Index + editMatch.Length;
				return true;
			}
			chEnd = -1;
			return false;
		}

		/// <summary>
		/// Checks if there is an outlining end tag in the specified string 
		/// for the specified outlining.
		/// </summary>
		/// <param name="otln"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		internal bool HasOutliningEndTag(EditOutlining otln, string str)
		{
			EditOutliningTagInfo oti = otln.OutliningTag;
			string matchStr = editSettings.GetTagCheckingRegExp(oti.EndTag);
			editMatch = Regex.Match(str, matchStr, RegExpOptions);
			while (editMatch != Match.Empty)
			{
				if (editMatch.Value == oti.EndTag)
				{
					return true;
				}
				editMatch = editMatch.NextMatch();
			}
			return false;
		}

		/// <summary>
		/// Clears all the multiline blocks after the specified location.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		internal void ClearMultiLineBlockFrom(int ln, int ch)
		{
			MultiLineBlockList.ClearFrom(ln, ch);
			EditAdvTagInfo ati;
			for (int i = 0; i < editSettings.AdvTagInfoList.Count; i++)
			{
				ati = (EditAdvTagInfo)editSettings.AdvTagInfoList[i];
				ati.SubMultiLineBlockList.ClearFrom(ln, ch);
			}
		}

		/// <summary>
		/// Gets the location before the sepcified location.
		/// </summary>
		/// <param name="ln">The line of the location for which the previous 
		/// location is to be obtained.</param>
		/// <param name="ch">The char of the location for which the previous 
		/// location is to be obtained.</param>
		/// <returns>The previous location before the specified location.
		/// </returns>
		internal EditLocation GetPreviousLineChar(int ln, int ch)
		{
			if (!edit.OutliningEnabled)
			{
				return GetPreviousLineCharNoOutlining(ln, ch);
			}
			EditOutlining otln = GetLeafOutlining(ln);
			if (!otln.Collapsed)
			{
				return GetPreviousLineCharNoOutlining(ln, ch);
			}
			else
			{
				if (otln.Start.GreaterThanOrEqualTo(ln, ch))
				{
					return GetPreviousLineCharNoOutlining(ln, ch);
				}
				else if (otln.End.LessThan(ln, ch))
				{
					return GetPreviousLineCharNoOutlining(ln, ch);
				}
				else
				{
					return new EditLocation(otln.Start);
				}
			}
		}

		/// <summary>
		/// Gets the location before the sepcified location without considering 
		/// outlining.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		/// <returns></returns>
		internal EditLocation GetPreviousLineCharNoOutlining(int ln, int ch)
		{
			if (edit.FirstLineChar.GreaterThanOrEqualTo(ln, ch))
			{
				return new EditLocation(edit.FirstLineChar);
			}
			else if (edit.LastLineChar.LessThan(ln, ch))
			{
				return new EditLocation(edit.LastLineChar);
			}
			else if (ch > 1)
			{
				if (ch <= GetLineLengthPlusOne(ln))
				{
					return new EditLocation(ln, ch - 1);
				}
				else
				{
					return new EditLocation(ln, GetLineLengthPlusOne(ln));
				}
			}
			else
			{
				return new EditLocation(ln - 1, GetLineLengthPlusOne(ln - 1));
			}
		}

		/// <summary>
		/// Gets the location after the sepcified location.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		/// <returns></returns>
		internal EditLocation GetNextLineChar(int ln, int ch)
		{
			if (!edit.OutliningEnabled)
			{
				return GetNextLineCharNoOutlining(ln, ch);
			}
			EditOutlining otln = GetLeafOutlining(ln);
			if (otln.IsRoot)
			{
				return GetNextLineCharNoOutlining(ln, ch);
			}
			else if (!otln.Collapsed)
			{
				return GetNextLineCharNoOutlining(ln, ch);
			}
			else
			{
				if (otln.Start.GreaterThan(ln, ch))
				{
					return GetNextLineCharNoOutlining(ln, ch);
				}
				else if (otln.End.LessThanOrEqualTo(ln, ch))
				{
					return GetNextLineCharNoOutlining(ln, ch);
				}
				else
				{
					return new EditLocation(otln.End);
				}
			}
		}

		/// <summary>
		/// Gets the location after the sepcified location without considering 
		/// outlining.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		/// <returns></returns>
		internal EditLocation GetNextLineCharNoOutlining(int ln, int ch)
		{
			if (edit.FirstLineChar.GreaterThan(ln, ch))
			{
				return new EditLocation(edit.FirstLineChar);
			}
			else if (edit.LastLineChar.LessThanOrEqualTo(ln, ch))
			{
				return edit.LastLineChar;
			}
			else if (ch < GetLineLengthPlusOne(ln))
			{
				return new EditLocation(ln, ch + 1);
			}
			else
			{
				return new EditLocation(ln + 1, 1);
			}
		}

		/// <summary>
		/// Gets the char index of the first non-white-space character at the 
		/// specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <returns></returns>
		internal int GetFirstNonSpaceChar(int ln)
		{
			return GetNextNonSpaceChar(ln, 1);
		}

		/// <summary>
		/// Gets the next non-space position from the specified location.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		/// <returns></returns>
		internal int GetNextNonSpaceChar(int ln, int ch)
		{
			if (GetLineLength(ln) == 0)
			{
				return 1;
			}
			string strLine = LineList[ln-1].LineString;
			for (int i = ch - 1; i < strLine.Length; i++)
			{
				if (!IsSpaceOrTab(strLine[i]))
				{
					return i+1;
				}
			}
			return strLine.Length + 1;
		}

		/// <summary>
		/// Gets the last non-white-space char index at the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <returns></returns>
		internal int GetLastNonSpaceChar(int ln)
		{
			return GetPreviousNonSpaceChar(ln, GetLineLengthPlusOne(ln));
		}

		/// <summary>
		/// Gets the previous non-space position from the specified location.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		/// <returns></returns>
		internal int GetPreviousNonSpaceChar(int ln, int ch)
		{
			if ((GetLineLength(ln) == 0) || (ch == 1))
			{
				return 1;
			}
			string strLine = LineList[ln-1].LineString;
			for (int i = ch - 2; i >= 0; i--)
			{
				if (!IsSpaceOrTab(strLine[i]))
				{
					return i+1;
				}
			}
			return 1;
		}

		/// <summary>
		/// Updates a location due to an insertion.
		/// </summary>
		/// <param name="lc">The location to be updated.</param>
		/// <param name="lcr">The location range of the insertion.</param>
		/// <param name="bInclusive">A value indicating whether the location 
		/// should be updated inclusively.</param>
		internal void UpdateFromInsertion(ref EditLocation lc, 
			EditLocationRange lcr, bool bInclusive)
		{
			if (bInclusive)
			{
				if (lcr.Start <= lc) 
				{
					if (lcr.Start.L == lc.L)
					{
						lc.C += lcr.End.C - lcr.Start.C;
					}
					lc.L += lcr.End.L - lcr.Start.L;
				}
			}
			else
			{
				if (lcr.Start < lc) 
				{
					if (lcr.Start.L == lc.L)
					{
						lc.C += lcr.End.C - lcr.Start.C;
					}
					lc.L += lcr.End.L - lcr.Start.L;
				}
			}
		}

		/// <summary>
		/// Updates a location due to a deletion.
		/// </summary>
		/// <param name="lc">The location to be updated.</param>
		/// <param name="lcr">The location range of the deletion.</param>
		internal void UpdateFromDeletion(ref EditLocation lc, EditLocationRange lcr)
		{
			if (lcr.Contains(lc))
			{
				lc.L = lcr.Start.L;
				lc.C = lcr.Start.C;
			}
			else if (lcr.End < lc)
			{
				if (lcr.End.L == lc.L)
				{
					lc.C += - lcr.End.C + lcr.Start.C;
				}
				lc.L -= lcr.End.L - lcr.Start.L;
			}
		}

		/// <summary>
		/// Tests if the specified line is hidden.
		/// </summary>
		/// <param name="ln"></param>
		internal bool IsHidden(int ln)
		{
			EditOutlining otln = GetLeafOutlining(ln);
			if (otln.IsRoot)
			{
				return false;
			}
			else if (otln.Collapsed)
			{
				if (ln == otln.StartLine)
				{
					return otln.HasCollapsedAncestor;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return otln.HasCollapsedAncestor;
			}
		}

		/// <summary>
		/// Gets the white-space position range from the specified line and char.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		/// <param name="ch1"></param>
		/// <param name="ch2"></param>
		internal void GetWhiteSpaceRange(int ln, int ch, out int ch1, out int ch2)
		{
			if ((GetLineLength(ln) == 0))
			{
				ch1 = 1;
				ch2 = 1;
				return;
			}
			string strLine = LineList[ln-1].LineString;
			int lnLengthPlusOne = GetLineLengthPlusOne(ln);
			if (ch == 1)
			{
				ch1 = 1;
				ch2 = 1;
				for (int i = 0; i < lnLengthPlusOne - 1; i++)
				{
					if (IsSpaceOrTab(strLine[i]))
					{
						ch2 = i + 2;
					}
					else
					{
						break;
					}
				}
			}
			else if (ch >= lnLengthPlusOne)
			{
				ch1 = lnLengthPlusOne;
				ch2 = lnLengthPlusOne;
				for (int i = lnLengthPlusOne - 2; i >= 0; i--)
				{
					if (IsSpaceOrTab(strLine[i]))
					{
						ch1 = i + 1;
					}
					else
					{
						break;
					}
				}
			}
			else
			{
				ch1 = ch;
				ch2 = ch;
				for (int i = ch - 1; i < lnLengthPlusOne - 1; i++)
				{
					if (IsSpaceOrTab(strLine[i]))
					{
						ch2 = i + 2;
					}
					else
					{
						break;
					}
				}
				for (int i = ch - 2; i >= 0; i--)
				{
					if (IsSpaceOrTab(strLine[i]))
					{
						ch1 = i + 1;
					}
					else
					{
						break;
					}
				}
			}
		}

		/// <summary>
		/// Gets the outlining object containing the specified location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		/// <returns>The outlining object containing the location.
		/// </returns>
		internal EditOutlining GetLeafOutlining(int ln, int ch)
		{
			return OutliningRoot.GetLeafOutlining(ln, ch);
		}

		/// <summary>
		/// Gets the outlining object containing the specified location.
		/// </summary>
		/// <param name="lc">The location at which the outlining object is to 
		/// be obtained.</param>
		/// <returns>The outlining object containing the location.</returns>
		internal EditOutlining GetLeafOutlining(EditLocation lc)
		{
			return OutliningRoot.GetLeafOutlining(lc);
		}

		/// <summary>
		/// Gets the outlining object containing the specified line.
		/// </summary>
		/// <param name="ln">The line at which the outlining object is to 
		/// be obtained.</param>
		/// <returns>The outlining object containing the line.</returns>
		internal EditOutlining GetLeafOutlining(int ln)
		{
			return OutliningRoot.GetLeafOutlining(ln);
		}

		/// <summary>
		/// Gets the length of the prefixed string length that includes 
		/// trailing white spaces.
		/// </summary>
		/// <param name="strPrefix"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		internal int GetPrefixLength(string strPrefix, string str)
		{
			Match match;
			string strPattern = "[ \t]*" + Regex.Escape(strPrefix) + "[ \t]*";
			match = Regex.Match(str, strPattern, RegExpOptions);
			if (match != Match.Empty)
			{
				return match.Length;
			}
			return 0;
		}

		/// <summary>
		/// Gets the collapsed outlining object at the sepcified location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		/// <returns>The collapsed outlining object at the location.</returns>
		internal EditOutlining GetCollapsedOutlining(int ln, int ch)
		{
			EditOutlining otln = GetLeafOutlining(new EditLocation(ln, ch));
			if (otln.IsRoot)
			{
				return null;
			}
			if (!otln.Collapsed)
			{
				return null;
			}
			return otln;
		}

		/// <summary>
		/// Removes all the outlining objects.
		/// </summary>
		internal void RemoveAllOutlining()
		{
			OutliningRoot.RemoveAllDescendants();
		}

		/// <summary>
		/// Gets the tooltip for the specified outlining object.
		/// </summary>
		/// <param name="otln">The outlining object for which the tooltip is 
		/// to be obtained.</param>
		/// <returns>The tooltip text for the outlining.</returns>
		internal string GetOutliningTooltips(EditOutlining otln)
		{
			int maxLine = 23;
			if (otln.EndLine - otln.StartLine > maxLine)
			{
				int endLine = otln.StartLine + maxLine;
				return GetString(otln.Start.L, otln.Start.C, endLine, 
					GetLineLengthPlusOne(endLine)).Replace("\t", 
					new string(' ', edit.TabSize)) + "...";
			}
			else
			{
				return GetString(otln.Start.L, otln.Start.C, otln.End.L, otln.End.C).
					Replace("\t", new string(' ', edit.TabSize));
			}
		}

		/// <summary>
		/// Gets the regular expression for the specified wildcards expression.
		/// </summary>
		/// <param name="str">The wildcards expression for which the regular 
		/// expression is to be obtained.</param>
		/// <returns>The regular expression for the wildcards expression.
		/// </returns>
		internal string GetRegExpFromWildcards(string str)
		{
			string strTemp = string.Empty;
			string strWildcardsRegExp = "(?<!" + Regex.Escape("\\") + ")" + 
				Regex.Escape("[") + ".*?" + 
				"(?<!" + Regex.Escape("\\") + ")" + Regex.Escape("]");
			MatchCollection matches = Regex.Matches(str, strWildcardsRegExp, 
				RegexOptions.Singleline);
			if (matches.Count == 0)
			{
				strTemp = str.Replace("?", ".");
				strTemp = strTemp.Replace("#", "[0-9]");
				strTemp = strTemp.Replace("*", ".*");
				return strTemp;
			}
			else
			{
				string strTemp1 = string.Empty;
				StringBuilder strTemp2;
				int indexStart = 0;
				for (int i = 0; i < matches.Count; i++)
				{
					strTemp1 = str.Substring(indexStart, matches[i].Index);
					strTemp1 = str.Replace("?", ".");
					strTemp1 = strTemp.Replace("#", "[0-9]");
					strTemp1 = strTemp.Replace("*", ".*");
					strTemp2 = new StringBuilder(matches[i].Value);
					if (strTemp2[1] == '!')
					{
						strTemp2[1] = '^';
					}
					strTemp = strTemp + strTemp1 + strTemp2.ToString();
					indexStart = matches[i].Index + matches[i].Length;
				}
				if (indexStart < str.Length)
				{
					strTemp1 = str.Substring(indexStart);
					strTemp1 = str.Replace("?", ".");
					strTemp1 = strTemp.Replace("#", "[0-9]");
					strTemp1 = strTemp.Replace("*", ".*");
					strTemp = strTemp + strTemp1;
				}
				return strTemp;
			}
		}

		/// <summary>
		/// Gets the user data object for the specified line.
		/// </summary>
		/// <param name="ln">The line at which the user data is to be 
		/// obtained.</param>
		/// <returns>The user data object at the line.</returns>
		internal object GetUserData(int ln)
		{
			return LineList[ln-1].UserData;
		}

		/// <summary>
		/// Sets the user data object for the specified line.
		/// </summary>
		/// <param name="ln">The line at which the user data is to be set.
		/// </param>
		/// <param name="data">The user data object at the line.</param>
		internal void SetUserData(int ln, object data)
		{
			LineList[ln-1].UserData = data;
		}

		/// <summary>
		/// Sets the displayed lines for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the displayed lines are to 
		/// be set.</param>
		/// <param name="lns">The displayed lines for the line.</param>
		internal void SetLinesOccupied(int ln, short lns)
		{
			if (LineList[ln-1].LinesOccupied != lns)
			{
				LineList[ln-1].LinesOccupied = lns;
			}
		}

		/// <summary>
		/// Gets the displayed lines for the specified line.
		/// </summary>
		/// <return>The displayed lines for the specified line.</return>
		internal int GetLinesOccupied(int ln)
		{
			return LineList[ln-1].LinesOccupied;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the associated EditControl object.
		/// </summary>
		internal EditControl Edit
		{
			get
			{
				return edit;
			}
		}

		/// <summary>
		/// Gets the first visible line of the document.
		/// </summary>
		internal int FirstVisibleLine
		{
			get
			{
				if (!edit.OutliningEnabled)
				{
					return 1;
				}
				int lnLast = LineList.Count;
				for (int i = 1; i <= lnLast; i++)
				{
					if (!LineList[i-1].Hidden)
					{
						return i;
					}
				}
				return 1;
			}
		}

		/// <summary>
		/// Gets the last visible line of the document.
		/// </summary>
		internal int LastVisibleLine
		{
			get
			{
				if (!edit.OutliningEnabled)
				{
					return LineList.Count;
				}
				for (int i = LineList.Count; i > 0; i--)
				{
					if (!LineList[i-1].Hidden)
					{
						return i;
					}
				}
				return 1;
			}
		}

		/// <summary>
		/// Gets the total number of visible lines.
		/// </summary>
		internal int VisibleLineCount
		{
			get
			{
				if (!edit.OutliningEnabled)
				{
					return LineList.Count;
				}
				else
				{
					return LineList.Count - OutliningRoot.GetCollapsedLines();
				}
			}
		}

		#endregion
	}
}
