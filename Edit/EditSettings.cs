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
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditSettings class stores and manages syntax coloring information 
	/// for EditControl.
	/// </summary>
	internal class EditSettings
	{
		#region Data Members

		/// <summary>
		/// The associated EditControl object.
		/// </summary>
		private EditControl edit;
		/// <summary>
		/// A value indicating whether to show the indicator margin.
		/// </summary>
		internal bool IndicatorMarginVisible;
		/// <summary>
		/// A value indicating whether to show the line number margin.
		/// </summary>
		internal bool LineNumberMarginVisible;
		/// <summary>
		/// A value indicating whether to show the selection margin.
		/// </summary>
		internal bool SelectionMarginVisible;
		/// <summary>
		/// A value indicating whether to show the user margin.
		/// </summary>
		internal bool UserMarginVisible;
		/// <summary>
		/// A value indicating whether to show the vertical scrollbar.
		/// </summary>
		internal bool VScrollBarVisible;
		/// <summary>
		/// A value indicating whether to show the horizontal scrollbar.
		/// </summary>
		internal bool HScrollBarVisible;
		/// <summary>
		///  A value indicating whether to show the status bar.
		/// </summary>
		internal bool StatusBarVisible;
		/// <summary>
		///  A value indicating whether to show the context menu.
		/// </summary>
		internal bool ContextMenuVisible;
		/// <summary>
		/// A value indicating whether to show white spaces.
		/// </summary>
		internal bool WhiteSpaceVisible;
		/// <summary>
		/// A value indicating whether to show grid lines.
		/// </summary>
		internal bool GridLinesVisible;
		/// <summary>
		/// A value indicating whether to show the right margin line or not.
		/// </summary>
		internal bool RightMarginLineVisible;
		/// <summary>
		/// A value indicating whether the control is readonly.
		/// </summary>
		internal bool ReadOnly;
		/// <summary>
		/// A value indicating whether to hide selection highlighting when
		/// focus is lost.
		/// </summary>
		internal bool HideSelection;
		/// <summary>
		/// A value indicating whether to accept the tab key.
		/// </summary>
		internal bool AcceptsTab;
		/// <summary>
		/// A value indicating whether to accept the return key.
		/// </summary>
		internal bool AcceptsReturn;
		/// <summary>
		/// Boolean value as to whether or not a FileDrop is allowed
		/// </summary>
		internal bool FileDropAllowed;
		/// <summary>
		/// A value indicating whether to modify the case of characters as 
		/// they are typed.
		/// </summary>
		internal CharacterCasing CharacterCasing;
		/// <summary>
		/// The name of the file for syntax coloring settings.
		/// </summary>
		internal string SettingFile;
		/// <summary>
		/// The type of indenting.
		/// </summary>
		internal EditIndentType IndentType;
		/// <summary>
		/// A value indicating whether to insert tabs instead of spaces 
		/// when indenting.
		/// </summary>
		internal bool KeepTabs;
		/// <summary>
		/// A value indicating whether syntax coloring is enabled.
		/// </summary>
		internal bool SyntaxColoringEnabled;
		/// <summary>
		/// A value indicating whether outlining is enabled.
		/// </summary>
		internal bool OutliningEnabled;
		/// <summary>
		/// A value indicating whether automatic outlining is enabled.
		/// </summary>
		internal bool AutomaticOutliningEnabled;
		/// <summary>
		/// The string that comment blocks collapse as.
		/// </summary>
		internal string CollapsedComments;
		/// <summary>
		/// The default string that outlining blocks collapse as.
		/// </summary>
		internal string CollapsedDefault;
		/// <summary>
		/// A value indicating whether the control is multiline.
		/// </summary>
		internal bool Multiline;
		/// <summary>
		/// The sequence of characters for line break.
		/// </summary>
		internal string NewLine;
		/// <summary>
		/// A value indicating whether the ContextChoice event is enabled.
		/// </summary>
		internal bool ContextChoiceEnabled;
		/// <summary>
		/// A value indicating whether the ContextPrompt event is enabled. 
		/// </summary>
		internal bool ContextPromptEnabled;
		/// <summary>
		///  A value indicating whether the ContextTooltip event is enabled.
		/// </summary>
		internal bool ContextTooltipEnabled;
		/// <summary>
		/// A value indicating whether the ContextChanged event is enabled.
		/// </summary>
		internal bool ContentChangedEventEnabled;
		/// <summary>
		/// The color of the border line.
		/// </summary>
		internal Color BorderColor;
		/// <summary>
		/// The name of the font family.
		/// </summary>
		internal string EditFontFamily;
		/// <summary>
		/// The size of the font.
		/// </summary>
		internal string EditFontSize;
		/// <summary>
		/// A value indicating whether the font is italic.
		/// </summary>
		internal string EditFontItalic;
		/// <summary>
		/// A value indicating whether the font is bold.
		/// </summary>
		internal string EditFontBold;
		/// <summary>
		/// The number of spaces for a tab.
		/// </summary>
		internal string TabSize;
		/// <summary>
		/// The number of spaces when indenting.
		/// </summary>
		internal string IndentSize;
		/// <summary>
		/// A value indicating whether auto indenting is enabled.
		/// </summary>
		internal string AutoIndent;
		/// <summary>
		/// A value indicating whether Virtual Space is enabled.
		/// </summary>
		internal string VirtualSpace;
		/// <summary>
		/// A value indicating whether Word Wrap is enabled.
		/// </summary>
		internal string WordWrap;
		/// <summary>
		/// The char that causes a selection window to be popped up.
		/// </summary>
		internal char[] ContextChoiceChar;
		/// <summary>
		/// The char that causes a prompt window to be popped up.
		/// </summary>
		internal char ContextPromptBeginChar;
		/// <summary>
		/// The char that causes a prompt window to be hidden.
		/// </summary>
		internal char ContextPromptEndChar;
		/// <summary>
		/// The char that starts an indenting block.
		/// </summary>
		internal char SmartIndentBeginChar;
		/// <summary>
		/// The char that ends an indenting block.
		/// </summary>
		internal char SmartIndentEndChar;
		/// <summary>
		/// The tag that needs to be repeated on the next line.
		/// </summary>
		internal string SmartRepeatTag;
		/// <summary>
		/// A value indicating whether to start with a blank document.
		/// </summary>
		internal bool StartWithNewFile;
		/// <summary>
		/// The characters that starts a line comment.
		/// </summary>
		internal string LineComment;
		/// <summary>
		/// A value indicating whether any single-line tag is presented.
		/// </summary>
		internal bool HasSingleLineTag;
		/// <summary>
		/// A value indicating whether any multiline tag is presented.
		/// </summary>
		internal bool HasMultiLineTag;
		/// <summary>
		/// The width of the user margin in pixels.
		/// </summary>
		internal int UserMarginWidth;
		/// <summary>
		/// The foreground color of the user margin.
		/// </summary>
		internal Color UserMarginForeColor;
		/// <summary>
		/// The background color of the user margin.
		/// </summary>
		internal Color UserMarginBackColor;
		/// <summary>
		/// A value indicating whether to log ReplaceAll as a composite action.
		/// </summary>
		internal bool LogReplaceAllEnabled;
		/// <summary>
		/// A value indicating the encoding type used for text.
		/// </summary>
		internal Encoding TextEncoding;
		/// <summary>
		/// A value indicating whether to display brace matching.
		/// </summary>
		internal bool BraceMatchingEnabled;
		/// <summary>
		/// The maximum line interval for brace matching.
		/// </summary>
		internal int MaxBraceMatchingLineInterval;
		/// <summary>
		/// A value indicating whether to show the file name in the status 
		/// bar and print/print preview.
		/// </summary>		
		internal bool FileNameVisible;

		/// <summary>
		/// The name of the language.
		/// </summary>
		internal string LanguageName;
		/// <summary>
		/// The description for the file type.
		/// </summary>
		internal string FileDescription;
		/// <summary>
		/// The extension for the file type.
		/// </summary>
		internal string FileExtension;
		/// <summary>
		/// A value indicating whether matching is case sensitive.
		/// </summary>
		internal string MatchCase;
		/// <summary>
		/// The list of color groups.
		/// </summary>
		internal EditColorGroupList ColorGroupList;
		/// <summary>
		/// The list of ColorGroupInfo objects.
		/// </summary>
		internal ArrayList ColorGroupInfoList;
		/// <summary>
		/// The list of TagInfo objects.
		/// </summary>
		internal ArrayList TagInfoList;
		/// <summary>
		/// The list of AdvTagInfo objects.
		/// </summary>
		internal ArrayList AdvTagInfoList;
		/// <summary>
		/// The list of OutliningTagInfo objects.
		/// </summary>
		internal ArrayList OutliningTagInfoList;
		/// <summary>
		/// The list of KeywordInfo objects.
		/// </summary>
		internal ArrayList KeywordInfoList;
		/// <summary>
		/// The list of resource strings.
		/// </summary>
		internal ArrayList ResourceStringList;

		/// <summary>
		/// A value indicating whether case is ignored in matching.
		/// </summary>
		private bool bIgnoreCase = false;

		/// <summary>
		/// A temperary variable to speed up the searching efficiency.
		/// </summary>
		private EditKeywordInfo editKeywordInfoTemp = 
			new EditKeywordInfo("Dummy", "");

		/// <summary>
		/// A value indicating whether any keyword has been added/removed.
		/// </summary>
		internal bool KeywordChanged = false;
		/// <summary>
		/// A value indicating whether any tag has been added/removed.
		/// </summary>
		internal bool TagChanged = false;

		#endregion

		#region Methods

		#region Methods Related to Settings Processing.

		/// <summary>
		/// Constructor.  Creates an EditSettings object associated with the
		/// specified EditControl object.
		/// </summary>
		/// <param name="edit">The associated EditControl object.</param>
		internal EditSettings(EditControl edit)
		{
			this.edit = edit;
			ResetSettings();
			ClearSettings();
		}

		/// <summary>
		/// Gets the color from a string with RGB values formatted as comma
		/// separated numbers, i.e., "#,#,#".
		/// </summary>
		/// <param name="str">The string containing the RGB values of the 
		/// color.</param>
		/// <returns>The color defined by the string.</returns>
		internal static Color GetColor(string str)
		{
			if ((str == null) || (str == string.Empty))
			{
				return Color.Black;
			}
			byte r, g, b;
			int delimiterIndex1 = str.IndexOf(",");
			if (delimiterIndex1 == -1)
			{
				return Color.Black;
			}
			int delimiterIndex2 = str.IndexOf(",", delimiterIndex1 + 1);
			if (delimiterIndex2 == -1)
			{
				return Color.Black;
			}
			try
			{
				r = Byte.Parse(str.Substring(0, delimiterIndex1));
				g = Byte.Parse(str.Substring(delimiterIndex1 + 1, 
					delimiterIndex2 - delimiterIndex1 - 1));
				b = Byte.Parse(str.Substring(delimiterIndex2 + 1));
				return Color.FromArgb(r, g, b);	
			}
			catch
			{
				return Color.Black;
			}
		}

		/// <summary>
		/// Gets the RGB string of a color, formatted as "#,#,#".
		/// </summary>
		/// <param name="clr">The color for which the RGB string is to be 
		/// obtained. </param>
		/// <returns>The string containing the RGB values of the color.
		/// </returns>
		internal static string GetColorRGB(Color clr)
		{
			string strTemp;
			strTemp = clr.R.ToString() + "," + clr.G.ToString() + "," 
				+ clr.B.ToString();
			return strTemp;
		}

		/// <summary>
		/// Tests if the specified string starts with the specified prefix.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <param name="strPrefix">The prefix to be tested.</param>
		/// <returns>true if the string starts with the prefix; otherwise, 
		/// false.</returns>
		internal bool IsPrefix(string str, string strPrefix)
		{
			if ((str == null) || (str == string.Empty))
			{
				return false;
			}
			int delimiterIndex = str.IndexOf("=");
			if (delimiterIndex == -1)
			{
				return false;
			}
			return (str.Substring(0, delimiterIndex).Trim().ToUpper()
				== strPrefix.Trim().ToUpper());
		}

		/// <summary>
		/// Gets the starting (left hand side) substring from the specified 
		/// setting string.
		/// </summary>
		/// <param name="str">A setting string for which the starting 
		/// substring is to be obtained.</param>
		/// <returns>The left hand side substring of the setting string.
		/// </returns>
		internal string GetStartString(string str)
		{
			if ((str == null) || (str == string.Empty))
			{
				return string.Empty;
			}
			int delimiterIndex = str.IndexOf("=");
			if (delimiterIndex == -1)
			{
				return string.Empty;
			}
			return str.Substring(0, delimiterIndex).Trim();
		}

		/// <summary>
		/// Gets the ending (right hand side) substring from the specified 
		/// setting string.
		/// </summary>
		/// <param name="str">A setting string for which the ending 
		/// substring is to be obtained.</param>
		/// <returns>The right hand side substring of the setting string.
		/// </returns>
		internal string GetEndString(string str)
		{
			if ((str == null) || (str == string.Empty))
			{
				return string.Empty;
			}
			int delimiterIndex = str.IndexOf("=");
			if (delimiterIndex == -1)
			{
				return string.Empty;
			}
			return str.Substring(delimiterIndex + 1).Trim();
		}

		/// <summary>
		/// Gets the next non-space and non-comment line.
		/// </summary>
		/// <param name="al">The source string arraylist.</param>
		/// <param name="index">The current index of the line.</param>
		/// <param name="strLine">The next non-space and non-comment line.
		/// </param>
		/// <returns>true if a non-space and non-comment line is found; 
		/// otherwise, false.</returns>
		internal bool ReadNextLine(ArrayList al, ref int index, out string strLine)
		{
			for (int i = index; i < al.Count; i++)
			{
				strLine= ((string)al[i]).Trim();
				if ((strLine.Length != 0) && (strLine[0] != ';'))
				{
					index++;
					return true;
				}
				index++;
			}
			strLine = null;
			return false;
		}

		/// <summary>
		/// Gets the comma-delimited string of sub-outlinings from a string 
		/// array.
		/// </summary>
		/// <param name="strArray">The string array containing names of 
		/// sub-outlinings.</param>
		/// <returns>The comma-delimited string of sub-outlinings.</returns>
		internal static string GetSubOutliningString(string [] strArray)
		{
			string strTemp = string.Empty;
			for (int i = 0; i < strArray.Length; i++)
			{
				if (strTemp != string.Empty)
				{
					strTemp += ",";
				}
				strTemp += strArray[i];
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the names of sub-outlinings as a string array.
		/// </summary>
		/// <param name="str">The string containing comma-delimited names 
		/// of sub-outlinings.</param>
		/// <returns>The string array containing names of sub-outlining.
		/// </returns>
		internal static string [] SplitSubOutliningString(string str)
		{
			if (str == string.Empty)
			{
				return null;
			}
			string [] strArray = str.Split(new Char[] {','});
			int count = 0;
			for (int i = 0; i < strArray.Length; i++)
			{
				if (strArray[i].Trim() != string.Empty)
				{
					count++;
				}
			}
			if (count == 0)
			{
				return null;
			}
			string [] strArrayTemp = new string[count];
			for (int i = 0; i < count; i++)
			{
				if (strArray[i].Trim() != string.Empty)
				{
					strArrayTemp[i] = strArray[i].Trim();
				}
			}
			return strArrayTemp;
		}

		/// <summary>
		/// Resets the setting variables to default values.
		/// </summary>
		internal void ResetSettings()
		{
			IndicatorMarginVisible = true;
			LineNumberMarginVisible = true;
			SelectionMarginVisible = false;
			UserMarginVisible = false;
			VScrollBarVisible = true;
			HScrollBarVisible = true;
			StatusBarVisible = true;
			ContextMenuVisible = true;
			WhiteSpaceVisible = false;
			GridLinesVisible = false;
			RightMarginLineVisible = false;
			ReadOnly = false;
			HideSelection = false;
			AcceptsTab = true;
			AcceptsReturn = true;
			FileDropAllowed = true;
			CharacterCasing = CharacterCasing.Normal;
			SettingFile = string.Empty;
			IndentType = EditIndentType.Block;
			KeepTabs = true;
			SyntaxColoringEnabled = false;
			OutliningEnabled = false;
			AutomaticOutliningEnabled = false;
			CollapsedComments = "/**/";
			CollapsedDefault = "...";
			Multiline = true;
			NewLine = "\r\n";
			ContextChoiceEnabled = true;
			ContextPromptEnabled = true;
			ContextTooltipEnabled = true;
			ContentChangedEventEnabled = true;
			BorderColor = Color.Gray;
			EditFontFamily = "Courier New";
			EditFontSize = "10";
			EditFontItalic = "0";
			EditFontBold = "0";
			TabSize = "4";
			IndentSize = "4";
			AutoIndent = "1";
			VirtualSpace = "0";
			WordWrap = "0";
			ContextChoiceChar = new char[]{'.'};
			ContextPromptBeginChar = '(';
			ContextPromptEndChar = ')';
			SmartIndentBeginChar = '{';
			SmartIndentEndChar = '}';
			SmartRepeatTag = string.Empty;
			StartWithNewFile = false;
			LineComment = string.Empty;
			HasSingleLineTag = false;
			HasMultiLineTag = false;
			UserMarginWidth = 50;
			UserMarginForeColor = Color.Black;
			UserMarginBackColor = Color.White;
			LogReplaceAllEnabled = true;
			TextEncoding = Encoding.UTF8;
			BraceMatchingEnabled = false;
			MaxBraceMatchingLineInterval = 100;
			FileNameVisible = true;
		}

		/// <summary>
		/// Clears the settings loaded from an Ini file.
		/// </summary>
		internal void ClearSettings()
		{
			LanguageName = "Unknown";
			FileDescription = "";
			FileExtension = "";
			MatchCase = "1";
			ColorGroupList = new EditColorGroupList();
			ColorGroupInfoList = new ArrayList();
			TagInfoList = new ArrayList();
			AdvTagInfoList = new ArrayList();
			OutliningTagInfoList = new ArrayList();
			KeywordInfoList = new ArrayList();
			bIgnoreCase = false;
			SetupDefaultColorGroups();
		}

		/// <summary>
		/// Sets up commonly used color groups.
		/// </summary>
		private void SetupDefaultColorGroups()
		{
			ColorGroupList.Add("Text", SystemColors.WindowText, 
				SystemColors.Window, true, true, 
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Selected Text", SystemColors.HighlightText, 
				SystemColors.Highlight, true, true, 
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Inactive Selected Text", 
				SystemColors.InactiveCaptionText, 
				SystemColors.InactiveCaption, true, true, 
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Indicator Margin", Color.Gray, 
				SystemColors.Control, false, false, 
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Line Numbers", Color.DarkCyan, 
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Visible White Space", Color.DarkCyan, 
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Bookmark", Color.Black, 
				Color.Cyan, false, false,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Brace Matching", Color.Red, 
				SystemColors.Window, false, true,
				EditColorGroupType.FrameLine);
//			ColorGroupList.Add("Breakpoint", Color.White, 
//				Color.Maroon, false, false,
//				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Brace Matching", SystemColors.WindowText, 
//				SystemColors.Window, false, false,
//				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Breakpoint (Disabled)", SystemColors.HighlightText, 
//				Color.Maroon, false, false,
//				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Breakpoint (Enabled)", SystemColors.HighlightText, 
//				Color.Maroon, false, false,
//				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Breakpoint (Error)", SystemColors.HighlightText, 
//				Color.Maroon, false, false,
//				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Breakpoint (Warning)", SystemColors.HighlightText, 
//				Color.Maroon, false, false,
//				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Call Return", SystemColors.WindowText, 
//				Color.Green, false, false,
//				EditColorGroupType.RegularText);
			ColorGroupList.Add("Collapsible Text", Color.Gray, 
				SystemColors.Window, false, true,
				EditColorGroupType.FrameLine);
			ColorGroupList.Add("Comment", Color.DarkGreen, 
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Identifier", SystemColors.WindowText, 
//				SystemColors.Window, false, false,
//				EditColorGroupType.RegularText);
			ColorGroupList.Add("Keyword", Color.Blue, 
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Number", SystemColors.WindowText, 
//				SystemColors.Window, true, true,
//				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Operator", SystemColors.WindowText, 
//				SystemColors.Window, true, true,
//				EditColorGroupType.RegularText);
//			ColorGroupList.Add("Preprocessor Keyword", Color.Blue, 
//				SystemColors.Window, false, false,
//				EditColorGroupType.RegularText);
			ColorGroupList.Add("String", SystemColors.WindowText, 
				SystemColors.Window, true, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Wizard Code", Color.Gray, 
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Wave Line White", Color.White,
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Wave Line Black", Color.Black,
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Wave Line Red", Color.Red,
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Wave Line Yellow", Color.Yellow,
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Wave Line Green", Color.Green,
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Wave Line Cyan", Color.Cyan,
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Wave Line Blue", Color.Blue,
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
			ColorGroupList.Add("Wave Line Purple", Color.Purple,
				SystemColors.Window, false, true,
				EditColorGroupType.RegularText);
		}

		/// <summary>
		/// Reads settings from an arraylist of setting strings.
		/// </summary>
		/// <param name="al">The string arraylist from which settings will be 
		/// read./// </param>
		/// <returns>true if settings have been read successfully; otherwise, 
		/// false.</returns>
		internal bool ReadFromStringArrayList(ArrayList al)
		{
			ClearSettings();
			int index = 0;
			string strLine;
			string strLineOld = string.Empty;
			bool bProcessed = false;
			// Read the first line.
			bool bContinue = ReadNextLine(al, ref index, out strLine);
			while (bContinue)
			{                      
				if (strLine.ToUpper() == "[Editor]".ToUpper())
				{
					for (int i = 0; i < 8; i++)
					{
						if (!ReadNextLine(al, ref index, out strLine))
						{
							EditControl.ShowInfoMessage(
								edit.GetResourceString("SettingFormatError"), 
								edit.GetResourceString("ReadSettings"));
							return false;
						}
						if (IsPrefix(strLine, "LanguageName"))
						{
							LanguageName = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "FileDescription"))
						{
							FileDescription = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "FileSuffix") 
							|| IsPrefix(strLine, "FileExtension"))
						{
							FileExtension = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "MatchCase"))
						{
							MatchCase = GetEndString(strLine);
							bIgnoreCase = (MatchCase != "1");
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "AutoIndent"))
						{
							if (GetEndString(strLine) == "1")
							{
								IndentType = EditIndentType.Block;
							}
							else
							{
								IndentType = EditIndentType.None;
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "KeepTabs"))
						{
							string strTemp = GetEndString(strLine);
							KeepTabs = (strTemp == "1");
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "SmartIndentChars"))
						{
							string strTemp = GetEndString(strLine);
							if (strTemp.Length >= 2)
							{
								SmartIndentBeginChar = strTemp[0];
								SmartIndentEndChar = strTemp[1];
								IndentType = EditIndentType.Smart;
							}
							else
							{
								if (IndentType == EditIndentType.Smart)
								{
									IndentType = EditIndentType.Block;
								}
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "SmartRepeatTag"))
						{
							SmartRepeatTag = GetEndString(strLine);
							bProcessed = true;
						}
						else
						{
							// Continue to process in other categories.
							bProcessed = false;
							break;
						}
					}
				}
				else if ((strLine.Length >= 11) && (strLine.Substring(0,11).
					ToUpper() == "[ColorGroup".ToUpper()))
				{
					EditGroupInfo gi = new EditGroupInfo();
					for (int i = 0; i < 6; i++)
					{
						if (!ReadNextLine(al, ref index, out strLine))
						{
							EditControl.ShowInfoMessage(
								edit.GetResourceString("SettingFormatError"), 
								edit.GetResourceString("ReadSettings"));
							return false;
						}
						if (IsPrefix(strLine, "GroupName"))
						{
							gi.GroupName = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "Foreground"))
						{
							gi.Foreground = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "Background"))
						{
							gi.Background = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "ForeColorAutomatic"))
						{
							gi.ForeColorAutomatic = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "BackColorAutomatic"))
						{
							gi.BackColorAutomatic = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "GroupType"))
						{
							gi.GroupType = GetEndString(strLine);
							bProcessed = true;
						}
						else
						{
							// Continue to process in other categories.
							bProcessed = false;
							break;
						}
					}
					ColorGroupInfoList.Add(gi);
				}
				else if ((strLine.Length >= 4) && (strLine.Substring(0,4).
					ToUpper() == "[Tag".ToUpper()))
				{
					EditTagInfo ti = new EditTagInfo();
					int endIndex = strLine.IndexOf("]");
					ti.Name = strLine.Substring(1, endIndex - 1).Trim();
					for (int i = 0; i < 5; i++)
					{
						if (!ReadNextLine(al, ref index, out strLine))
						{
							EditControl.ShowInfoMessage(
								edit.GetResourceString("SettingFormatError"), 
								edit.GetResourceString("ReadSettings"));
							return false;
						}
						if (IsPrefix(strLine, "ColorGroup"))
						{
							ti.ColorGroup = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "BeginTag"))
						{
							if (bIgnoreCase)
							{
								ti.BeginTag = GetEndString(strLine).ToUpper();
							}
							else
							{
								ti.BeginTag = GetEndString(strLine);
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "EndTag"))
						{
							if (bIgnoreCase)
							{
								ti.EndTag = GetEndString(strLine).ToUpper();
							}
							else
							{
								ti.EndTag = GetEndString(strLine);
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "EscapeChar"))
						{
							if (bIgnoreCase)
							{
								ti.EscapeChar = GetEndString(strLine).ToUpper();
							}
							else
							{
								ti.EscapeChar = GetEndString(strLine);
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "MultiLine"))
						{
							ti.MultiLine = GetEndString(strLine);
							bProcessed = true;
						}
						else
						{
							// Continue to process in other categories.
							bProcessed = false;
							break;
						}
					}
					TagInfoList.Add(ti);
					TagChanged = true;
				}
				else if ((strLine.Length >= 7) && (strLine.Substring(0,7).
					ToUpper() == "[AdvTag".ToUpper()))
				{
					EditAdvTagInfo ati = new EditAdvTagInfo();
					int endIndex = strLine.IndexOf("]");
					ati.Name = strLine.Substring(1, endIndex - 1).Trim();
					for (int i = 0; i < 7; i++)
					{
						if (!ReadNextLine(al, ref index, out strLine))
						{
							EditControl.ShowInfoMessage(
								edit.GetResourceString("SettingFormatError"), 
								edit.GetResourceString("ReadSettings"));
							return false;
						}
						if (IsPrefix(strLine, "ColorGroup"))
						{
							ati.ColorGroup = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "ContentColorGroup"))
						{
							ati.ContentColorGroup = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "ToLineEnd"))
						{
							ati.ToLineEnd = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "BeginTag"))
						{
							if (bIgnoreCase)
							{
								ati.BeginTag = GetEndString(strLine).ToUpper();
							}
							else
							{
								ati.BeginTag = GetEndString(strLine);
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "EndTag"))
						{
							if (bIgnoreCase)
							{
								ati.EndTag = GetEndString(strLine).ToUpper();
							}
							else
							{
								ati.EndTag = GetEndString(strLine);
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "EscapeChar"))
						{
							if (bIgnoreCase)
							{
								ati.EscapeChar = GetEndString(strLine).ToUpper();
							}
							else
							{
								ati.EscapeChar = GetEndString(strLine);
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "MultiLine"))
						{
							ati.MultiLine = GetEndString(strLine);
							bProcessed = true;
						}
						else
						{
							// Continue to process in other categories.
							bProcessed = false;
							break;
						}
					}
					AdvTagInfoList.Add(ati);
					TagChanged = true;
				}
				else if ((strLine.Length >= 7) && (strLine.Substring(0,7).
					ToUpper() == "[SubTag".ToUpper()))
				{
					EditTagInfo ti = new EditTagInfo();
					EditAdvTagInfo ati = new EditAdvTagInfo();
					for (int i = 0; i < 6; i++)
					{
						if (!ReadNextLine(al, ref index, out strLine))
						{
							EditControl.ShowInfoMessage(
								edit.GetResourceString("SettingFormatError"), 
								edit.GetResourceString("ReadSettings"));
							return false;
						}
						if (IsPrefix(strLine, "ParentTag"))
						{
							ati = GetAdvTag(GetEndString(strLine));
							if (ti == null)
							{
								EditControl.ShowInfoMessage(
									edit.GetResourceString("SettingFormatError") + "\n" 
									+ "Error Line:  " + strLine, 
									edit.GetResourceString("ReadSettings"));
								return false;
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "ColorGroup"))
						{
							ti.ColorGroup = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "BeginTag"))
						{
							if (bIgnoreCase)
							{
								ti.BeginTag = GetEndString(strLine).ToUpper();
							}
							else
							{
								ti.BeginTag = GetEndString(strLine);
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "EndTag"))
						{
							if (bIgnoreCase)
							{
								ti.EndTag = GetEndString(strLine).ToUpper();
							}
							else
							{
								ti.EndTag = GetEndString(strLine);
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "EscapeChar"))
						{
							if (bIgnoreCase)
							{
								ti.EscapeChar = GetEndString(strLine).ToUpper();
							}
							else
							{
								ti.EscapeChar = GetEndString(strLine);
							}
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "MultiLine"))
						{
							ti.MultiLine = GetEndString(strLine);
							bProcessed = true;
						}
						else
						{
							// Continue to process in other categories.
							bProcessed = false;
							break;
						}
					}
					ati.SubTagInfoList.Add(ti);
					TagChanged = true;
				}
				else if ((strLine.Length >= 12) && (strLine.Substring(0,12).
					ToUpper() == "[SubKeywords".ToUpper()))
				{
					EditAdvTagInfo ati = new EditAdvTagInfo();
					if (!ReadNextLine(al, ref index, out strLine))
					{
						EditControl.ShowInfoMessage(
							edit.GetResourceString("SettingFormatError"), 
							edit.GetResourceString("ReadSettings"));
						return false;
					}
					if (IsPrefix(strLine, "ParentTag"))
					{
						ati = GetAdvTag(GetEndString(strLine));
						if (ati == null)
						{
							EditControl.ShowInfoMessage(
								edit.GetResourceString("SettingFormatError") + "\n" 
								+ "Error Line:  " + strLine, 
								edit.GetResourceString("ReadSettings"));
							return false;
						}
						bProcessed = true;
					}
					else
					{
						EditControl.ShowInfoMessage(
							edit.GetResourceString("SettingFormatError") + "\n"  
							+ "Error Line:  " + strLine, 
							edit.GetResourceString("ReadSettings"));
						return false;
					}
					EditKeywordInfo ki;
					while(ReadNextLine(al, ref index, out strLine))
					{
						if (strLine.StartsWith("["))
						{
							bProcessed = false;
							break;
						}
						string strTemp = (GetStartString(strLine));
						if (bIgnoreCase)
						{
							ki = new EditKeywordInfo(
								GetEndString(strLine).ToUpper(), strTemp);
						}
						else
						{
							ki = new EditKeywordInfo(
								GetEndString(strLine), strTemp);
						}
						ati.SubKeywordInfoList.Add(ki);
						bProcessed = true;
					}
				}
				else if ((strLine.Length >= 10) && (strLine.Substring(0,10).
					ToUpper() == "[Outlining".ToUpper()))
				{
					EditOutliningTagInfo oti = new EditOutliningTagInfo();
					for (int i = 0; i < 8; i++)
					{
						if (!ReadNextLine(al, ref index, out strLine))
						{
							EditControl.ShowInfoMessage(
								edit.GetResourceString("SettingFormatError"), 
								edit.GetResourceString("ReadSettings"));
							return false;
						}
						if (IsPrefix(strLine, "Name"))
						{
							oti.Name = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "Keyword"))
						{
							oti.Keyword = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "BeginTag"))
						{
							oti.BeginTag = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "EndTag"))
						{
							oti.EndTag = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "LineStarting"))
						{
							oti.LineStarting = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "MultiLine"))
						{
							oti.MultiLine = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "CollapseAs"))
						{
							oti.CollapseAs = GetEndString(strLine);
							bProcessed = true;
						}
						else if (IsPrefix(strLine, "SubOutlining"))
						{
							oti.SubOutlining = 
								SplitSubOutliningString(GetEndString(strLine));
							bProcessed = true;
						}
						else
						{
							// Continue to process in other categories.
							bProcessed = false;
							break;
						}
					}
					OutliningTagInfoList.Add(oti);
				}
				else if (strLine.ToUpper() == "[Keywords]".ToUpper())
				{
					EditKeywordInfo ki;
					while(ReadNextLine(al, ref index, out strLine))
					{
						string strTemp = (GetStartString(strLine));
						if (bIgnoreCase)
						{
							ki = new EditKeywordInfo(
								GetEndString(strLine).ToUpper(), strTemp);
						}
						else
						{
							ki = new EditKeywordInfo(
								GetEndString(strLine), strTemp);
						}
						AddKeyword(ki);
					}
					bProcessed = true;
				}
				if (bProcessed)
				{
					bContinue = ReadNextLine(al, ref index, out strLine);
				}
				else
				{
					if (strLineOld == strLine)
					{
						EditControl.ShowErrorMessage(
							edit.GetResourceString("SettingFormatError") + "\n" 
							+ "Error Line:  " + strLine, 
							edit.GetResourceString("ReadSettings"));
						return false;
					}
					else
					{
						strLineOld = strLine;
					}
				}
			}
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				((EditAdvTagInfo)AdvTagInfoList[i]).SubKeywordInfoList.Sort();
			}
			LineComment = GetLineCommentTag();
			HasSingleLineTag = SingleLineTagPresent();
			HasMultiLineTag = MultiLineTagPresent();
			SetupColorGroupList();
			edit.SetupCommonColors();
			return true;
		}

		/// <summary>
		/// Adds a new keyword with the specified color group.
		/// </summary>
		/// <param name="keyword">The keyword to be added.</param>
		/// <param name="colorGroup">The color group for the keyword.</param>
		/// <returns>true if the keyword is new; otherwise, false.
		/// </returns>
		internal bool AddKeyword(string keyword, string colorGroup)
		{
			if (keyword == string.Empty)
			{
				return false;
			}
			EditKeywordInfo ki;
			if (bIgnoreCase)
			{
				ki = new EditKeywordInfo(keyword.ToUpper(), colorGroup);
			}
			else
			{
				ki = new EditKeywordInfo(keyword, colorGroup);
			}
			ki.ColorGroupIndex = GetColorGroupIndex(colorGroup);
			return AddKeyword(ki);
		}

		/// <summary>
		/// Adds a new EditKeywordInfo object.
		/// </summary>
		/// <param name="ki">The EditKeywordInfo object to be added.</param>
		/// <returns>true if the EditKeywordInfo object is new; otherwise, 
		/// false.</returns>
		internal bool AddKeyword(EditKeywordInfo ki)
		{
			int index = KeywordInfoList.BinarySearch(ki);
			if (index < 0)
			{
				index = ~index;
				KeywordInfoList.Insert(index, (EditKeywordInfo) ki);
				KeywordChanged = true;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the specified keyword.
		/// </summary>
		/// <param name="keyword">The keyword to be removed.</param>
		/// <returns>true if the keyword has been found and removed; 
		/// otherwise, false.</returns>
		internal bool RemoveKeyword(string keyword)
		{
			if (keyword == string.Empty)
			{
				return false;
			}
			string keywordTemp;
			if (bIgnoreCase)
			{
				keywordTemp = keyword.ToUpper();
			}
			else
			{
				keywordTemp = keyword;
			}
			for (int i = 0; i < KeywordInfoList.Count; i++)
			{
				if (((EditKeywordInfo)KeywordInfoList[i]).Keyword == keywordTemp)
				{
					KeywordInfoList.RemoveAt(i);
					KeywordChanged = true;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Adds a new tag with the specified attributes.
		/// </summary>
		/// <param name="beginTag">The begin tag for the new tag object.
		/// </param>
		/// <param name="endTag">The end tag for the new tag object.</param>
		/// <param name="escapeChar">The escape char for the new tag object.
		/// </param>
		/// <param name="bMultiLine">The multiline property for the new tag 
		/// object.</param>
		/// <param name="colorGroup">The color group for the new tag object.
		/// </param>
		/// <returns>true if the tag object is new; otherwise, false.
		/// </returns>
		internal bool AddTag(string beginTag, string endTag,
			string escapeChar, bool bMultiLine, string colorGroup)
		{
			if (beginTag == string.Empty)
			{
				return false;
			}
			if (endTag == string.Empty)
			{
				escapeChar = string.Empty;
				bMultiLine = false;
			}
			if (!IsTag(beginTag, endTag))
			{
				EditTagInfo ti = new EditTagInfo();
				ti.BeginTag = beginTag;
				ti.EndTag = endTag;
				ti.EscapeChar = escapeChar;
				ti.MultiLine = bMultiLine ? "1" : "0";
				ti.ColorGroup = colorGroup;
				RegexOptions ro = RegexOptions.Singleline | RegexOptions.Compiled;
				if (IgnoreCase)
				{
					ro |= RegexOptions.IgnoreCase;
				}
				if (bMultiLine)
				{
					string regexStr;
					if (ti.EscapeChar == string.Empty)
					{
						regexStr = GetTagRegexString(ti.EndTag);
					}
					else
					{
						regexStr = "(?<!" + Regex.Escape(ti.EscapeChar) 
							+ ")" + GetTagRegexString(ti.EndTag);
					}
					ti.EndRegex = new Regex(regexStr, ro);
				}
				ti.ColorGroupIndex = GetColorGroupIndex(ti.ColorGroup);
				TagInfoList.Add(ti);
				LineComment = GetLineCommentTag();
				HasSingleLineTag = SingleLineTagPresent();
				HasMultiLineTag = MultiLineTagPresent();
				TagChanged = true;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes a tag with the specified begin tag and end tag.
		/// </summary>
		/// <param name="beginTag">The begin tag for the tag object to 
		/// be removed.</param>
		/// <param name="endTag">The end tag for the tag object to be 
		/// removed.</param>
		/// <returns>true if the tag has been found and removed; otherwise, 
		/// false.</returns>
		internal bool RemoveTag(string beginTag, string endTag)
		{
			if (beginTag == string.Empty)
			{
				return false;
			}
			int index = GetTagIndex(beginTag, endTag);
			if (index > 0)
			{
				TagInfoList.RemoveAt(index);
				LineComment = GetLineCommentTag();
				HasSingleLineTag = SingleLineTagPresent();
				HasMultiLineTag = MultiLineTagPresent();
				TagChanged = true;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the regular expression string for the specified tag.
		/// </summary>
		/// <param name="strTag">The tag string for which the regular 
		/// expression string is to be obtained.</param>
		/// <returns>The regular expression string for the tag.
		/// </returns>
		internal string GetTagRegexString(string strTag)
		{
			if (strTag[0] == '`')
			{
				return strTag.Substring(1);
			}
			else
			{
				return Regex.Escape(strTag);
			}
		}

		/// <summary>
		/// Adds a color group with the specified values of attributes.
		/// </summary>
		/// <param name="groupName">The group name for the new color group 
		/// object.</param>
		/// <param name="foreColor">The forecolor for the new color group 
		/// object.</param>
		/// <param name="backColor">The backcolor for the new color group 
		/// object.</param>
		/// <param name="isAutoForeColor">The AutoForeColor property for the 
		/// new color group object.</param>
		/// <param name="isAutoBackColor">The AutoBackColor property for the 
		/// new color group object.</param>
		/// <param name="groupType">The group type for the new color group
		/// object.</param>
		/// <returns>true if the color group is new; otherwise, false.
		/// </returns>
		internal bool AddColorGroup(string groupName, Color foreColor, 
			Color backColor, bool isAutoForeColor, bool isAutoBackColor, 
			EditColorGroupType groupType)
		{
			EditGroupInfo gi = new EditGroupInfo();
			gi.GroupName = groupName;
			gi.Foreground = GetColorRGB(foreColor);
			gi.Background = GetColorRGB(backColor);
			gi.ForeColorAutomatic = isAutoForeColor ? "1" : "0";
			gi.BackColorAutomatic = isAutoBackColor ? "1" : "0";
			gi.GroupType = ((byte)groupType).ToString();
			if (ColorGroupInfoList.Add(gi) >= 0)
			{
				SetupColorGroupList();
				edit.SetupCommonColors();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the EditColorGroupType corresponding to the specified color 
		/// group type number.
		/// </summary>
		/// <param name="strColorGroupTypeNum">The color group type number of 
		/// the EditColorGroupType.</param>
 		/// <returns>The EditColorGroupType corresponding to the color group 
 		/// number.</returns>
		internal EditColorGroupType GetColorGroupType(string strColorGroupTypeNum)
		{
			EditColorGroupType cgtTemp;
			switch (strColorGroupTypeNum)
			{
				case "0":
					cgtTemp = EditColorGroupType.RegularText;
					break;
				case "1":
					cgtTemp = EditColorGroupType.BoldText;
					break;
				case "2":
					cgtTemp = EditColorGroupType.UnderLine;
					break;
				case "3":
					cgtTemp = EditColorGroupType.FrameLine;
					break;
				default:
					cgtTemp = EditColorGroupType.RegularText;
					break;
			}
			return cgtTemp;
		}

		/// <summary>
		/// Sets up the user color groups.
		/// </summary>
		internal void SetupColorGroupList()
		{
			EditGroupInfo gi;
			for (int i = 0; i < ColorGroupInfoList.Count; i++)
			{
				gi = (EditGroupInfo)ColorGroupInfoList[i];
				SetColorGroup(gi.GroupName, GetColor(gi.Foreground), 
					GetColor(gi.Background), gi.ForeColorAutomatic=="1", 
					gi.BackColorAutomatic=="1", 
					GetColorGroupType(gi.GroupType));
			}
			FillKeywordColorGroupIndex();
			SetupTagInfo();
		}

		/// <summary>
		/// Fills up the color indexes for the keywords.
		/// </summary>
		private void FillKeywordColorGroupIndex()
		{
			for (int i = 0; i < KeywordInfoList.Count; i++)
			{
				((EditKeywordInfo)KeywordInfoList[i]).ColorGroupIndex 
					= GetColorGroupIndex(((EditKeywordInfo)
					KeywordInfoList[i]).ColorGroup);
			}
			EditAdvTagInfo ati;
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				ati = (EditAdvTagInfo)AdvTagInfoList[i];
				for (int j = 0; j < ati.SubKeywordInfoList.Count; j++)
				{
					((EditKeywordInfo)ati.SubKeywordInfoList[j]).ColorGroupIndex 
						= GetColorGroupIndex(((EditKeywordInfo)ati.
						SubKeywordInfoList[j]).ColorGroup);
				}
			}
		}

		/// <summary>
		/// Gets the color group index for the specified color group name.
		/// </summary>
		/// <param name="groupName">The name of the color group.</param>
		/// <returns>The index of the color group.</returns>
		internal short GetColorGroupIndex(string groupName)
		{
			return (short)ColorGroupList.GetColorGroupIndex(groupName);
		}

		/// <summary>
		/// Gets the foreground color of the specified color group name.
		/// </summary>
		/// <param name="groupName">The name of the color group.</param>
		/// <returns>The foreground color of the color group.</returns>
		internal Color GetColorGroupForeColor(string groupName)
		{
			return ColorGroupList.GetForeColor(groupName);
		}

		/// <summary>
		/// Gets the background color of the specified color group name.
		/// </summary>
		/// <param name="groupName">The name of the color group.</param>
		/// <returns>The background color of the color group.</returns>
		internal Color GetColorGroupBackColor(string groupName)
		{
			return ColorGroupList.GetBackColor(groupName);
		}

		/// <summary>
		/// Sets the foreground color of the specified color group name.
		/// </summary>
		/// <param name="groupName">The color group for which the foreground
		/// color is to be set.</param>
		/// <param name="foreColor">The foreground color for the color group.
		/// </param>
		/// <returns>true if the colorgroup exists; otherwise, false.</returns>
		internal bool SetColorGroupForeColor(string groupName, Color foreColor)
		{
			return ColorGroupList.SetForeColor(groupName, foreColor);
		}

		/// <summary>
		/// Sets the background color of the specified color group name.
		/// </summary>
		/// <param name="groupName">The color group for which the background
		/// color is to be set.</param>
		/// <param name="backColor">The background color for the color group.
		/// </param>
		/// <returns>true if the colorgroup exists; otherwise, false.</returns>
		internal bool SetColorGroupBackColor(string groupName, Color backColor)
		{
			return ColorGroupList.SetBackColor(groupName, backColor);
		}

		/// <summary>
		/// Gets the foreground color of the specified color group index.
		/// </summary>
		/// <param name="index">The index of the color group.</param>
		/// <returns>The foreground color of the color group.</returns>
		internal Color GetColorGroupForeColor(int index)
		{
			return ColorGroupList[index].ForeColor;
		}

		/// <summary>
		/// Gets the type of the specified color group index.
		/// </summary>
		/// <param name="index">The index of the color group.</param>
		/// <returns>An int representing the type of the color group.</returns>
		internal EditColorGroupType GetColorGroupType(int index)
		{
			return ColorGroupList[index].GroupType;
		}

		/// <summary>
		/// Gets the background color of the specified color group index.
		/// </summary>
		/// <param name="index">The index of the color group.</param>
		/// <returns>The background color of the color group.</returns>
		internal Color GetColorGroupBackColor(int index)
		{
			return ColorGroupList[index].BackColor;
		}

		/// <summary>
		/// Fills the color indexes for the tags.
		/// </summary>
		internal void SetupTagInfo()
		{
			RegexOptions ro = RegexOptions.Singleline | RegexOptions.Compiled;
			if (IgnoreCase)
			{
				ro |= RegexOptions.IgnoreCase;
			}
			EditTagInfo ti;
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				ti = (EditTagInfo)TagInfoList[i];
				if (ti.MultiLine == "1")
				{
					string regexStr;
					if (ti.EscapeChar == string.Empty)
					{
						regexStr = GetTagRegexString(ti.EndTag);
					}
					else
					{
						regexStr = "(?<!" + Regex.Escape(ti.EscapeChar) 
							+ ")" + GetTagRegexString(ti.EndTag);
					}
					ti.EndRegex = new Regex(regexStr, ro);
				}
				ti.ColorGroupIndex = GetColorGroupIndex(ti.ColorGroup);
			}
			EditAdvTagInfo ati;
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				ati = (EditAdvTagInfo)AdvTagInfoList[i];
				if (ati.MultiLine == "1")
				{
					string regexStr;
					if (ati.EscapeChar == string.Empty)
					{
						regexStr = Regex.Escape(ati.EndTag);
					}
					else
					{
						regexStr = "(?<!" + Regex.Escape(ati.EscapeChar) 
							+ ")" + Regex.Escape(ati.EndTag);
					}
					ati.EndRegex = new Regex(regexStr, ro);
				}
				ati.ColorGroupIndex = GetColorGroupIndex(ati.ColorGroup);
				ati.ContentColorGroupIndex = GetColorGroupIndex(ati.ContentColorGroup);
				ati.SubMultiLineBlockList = 
					new EditMultiLineBlockList(edit.Data);
				for (int j = 0; j < ati.SubTagInfoList.Count; j++)
				{
					((EditTagInfo)ati.SubTagInfoList[j]).ColorGroupIndex = 
						GetColorGroupIndex(((EditTagInfo)ati.
						SubTagInfoList[j]).ColorGroup);
				}
				string strTemp = GetInternalRegExp(ati);
				if (strTemp != string.Empty)
				{
					ati.InternalRegex = new Regex(strTemp, ro);
				}
			}
		}

		/// <summary>
		/// Changes settings for a color group.
		/// </summary>
		/// <param name="groupName">The name of the color group.</param>
		/// <param name="foreColor">The foreground color of the color 
		/// group.</param>
		/// <param name="backColor">The background color of the color 
		/// group.</param>
		/// <param name="isAutoForeColor">A value indicating whether the 
		/// foreground color is the same with that of the normal text.</param>
		/// <param name="isAutoBackColor">A value indicating whether the 
		/// background color is the same with that of the normal text.</param>
		/// <param name="groupType">The type of color group.</param>
		/// <returns>true if the color group already exists; otherwise, 
		/// false.</returns>
		internal bool SetColorGroup(string groupName, Color foreColor, 
			Color backColor, bool isAutoForeColor, bool isAutoBackColor, 
			EditColorGroupType groupType)
		{
			return ColorGroupList.SetGroup(groupName, foreColor, backColor, 
				isAutoForeColor, isAutoBackColor, groupType);
		}

		/// <summary>
		/// Gets the advanced tag with the specified name.
		/// </summary>
		/// <param name="name">The name of the advanced tag.</param>
		/// <returns>The EditAdvTagInfo object with the given name.</returns>
		internal EditAdvTagInfo GetAdvTag(string name)
		{
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				if (((EditAdvTagInfo)AdvTagInfoList[i]).Name == name)
				{
					return (EditAdvTagInfo)AdvTagInfoList[i];
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the regular expression for the subtags of the specified 
		/// EditAdvTagInfo object.
		/// </summary>
		/// <param name="ati">The specified EditAdvTagInfo object.</param>
		/// <returns>The regular expression for the subtags of the given
		/// EditAdvTagInfo object.</returns>
		internal string GetAdvTagSubTagRegExp(EditAdvTagInfo ati)
		{
			string strTemp = string.Empty;
			EditTagInfo ti;
			for (int i = 0; i < ati.SubTagInfoList.Count; i++)
			{
				ti = (EditTagInfo)ati.SubTagInfoList[i];
				if (ti.MultiLine != "1")
				{
					if (ti.EndTag != string.Empty)
					{
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						if (ti.EscapeChar == string.Empty)
						{
							strTemp += Regex.Escape(ti.BeginTag) 
								+ ".*?" + Regex.Escape(ti.EndTag);
						}
						else
						{
							strTemp += Regex.Escape(ti.BeginTag)
								+ ".*?" + "(?<!" 
								+ Regex.Escape(ti.EscapeChar) 
								+ ")" + Regex.Escape(ti.EndTag);
						}
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						strTemp += Regex.Escape(ti.BeginTag) + ".*$";
					}
					else
					{
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						if (EditData.IsWordChar(ti.BeginTag[ti.BeginTag.Length - 1]))
						{
							strTemp += Regex.Escape(ti.BeginTag) + "\\b.*$";
						}
						else
						{
							strTemp += Regex.Escape(ti.BeginTag) + ".*$";
						}
					}
				}
				else
				{
					// Multiline blocks may also appear in one line.
					if (strTemp != string.Empty)
					{
						strTemp += "|";
					}
					if (ti.EscapeChar == string.Empty)
					{
						strTemp += Regex.Escape(ti.BeginTag) 
							+ ".*?" + Regex.Escape(ti.EndTag);
					}
					else
					{
						strTemp += Regex.Escape(ti.BeginTag)
							+ ".*?" + "(?<!" 
							+ Regex.Escape(ti.EscapeChar) 
							+ ")" + Regex.Escape(ti.EndTag);
					}
					if (strTemp != string.Empty)
					{
						strTemp += "|";
					}
					strTemp += Regex.Escape(ti.BeginTag) + ".*$";
				}
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for all the begin tags of the 
		/// specified advanced tag.
		/// </summary>
		/// <param name="ati">The advanced tag.</param>
		/// <returns>The regular expression for all the begin tags of the 
		/// advanced tag.</returns>
		internal string GetAdvTagMultiLineSubBeginTagsRegExp(EditAdvTagInfo ati)
		{
			string strTemp = string.Empty;
			EditTagInfo ti;
			for (int i = 0; i < ati.SubTagInfoList.Count; i++)
			{
				ti = (EditTagInfo)ati.SubTagInfoList[i];
				if (ti.MultiLine == "1")
				{
					if (strTemp != string.Empty)
					{
						strTemp += "|";
					}
					strTemp += Regex.Escape(ti.BeginTag);
				}
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the color group for the specified string if it starts with
		/// a begin subtag of the specified tagIndex.
		/// </summary>
		/// <param name="ati"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		internal short GetSubTagColorIndex(EditAdvTagInfo ati, string str)
		{
			string strTemp = (!bIgnoreCase)? str : str.ToUpper();
			EditTagInfo ti;
			for (int i = 0; i < ati.SubTagInfoList.Count; i++)
			{
				ti = (EditTagInfo)ati.SubTagInfoList[i];
				if (strTemp.StartsWith(ti.BeginTag))
				{
					return ti.ColorGroupIndex;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the index of the subtag if the specified string does not ends 
		/// with the end subtag.
		/// </summary>
		/// <param name="ati"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		internal short GetUnfinishedSubTag(EditAdvTagInfo ati, string str)
		{
			string strTemp = (!bIgnoreCase)? str : str.ToUpper();
			EditTagInfo ti;
			for (int i = 0; i < ati.SubTagInfoList.Count; i++)
			{
				ti = (EditTagInfo)ati.SubTagInfoList[i];
				if (ti.MultiLine != "1")
				{
					return -1;
				}
				if ((strTemp.StartsWith(ti.BeginTag)) && 
					(!strTemp.EndsWith(ti.EndTag)))
				{
					return (short)i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Reads settings from the specified string.
		/// </summary>
		/// <param name="strSettings">The string to read settings from.</param>
		/// <returns>true if settings have been read successfully; otherwise,
		/// false.</returns>
		internal bool ReadFromString(string strSettings)
		{
			if ((strSettings == null) || (strSettings == string.Empty))
			{
				return false;
			}
			return ReadFromStringArrayList(EditControl.
				GetStringArrayList(strSettings));
		}

		/// <summary>
		/// Reads settings from the specified setting file.
		/// </summary>
		/// <param name="fileName">The file to read settings from.</param>
		/// <returns>true if settings have been read successfully; otherwise, 
		/// false.</returns>
		internal bool ReadFromFile(string fileName)
		{
			if ((fileName != null) && (fileName != string.Empty))
			{
				try
				{
					if (!File.Exists(fileName))
					{
						EditControl.ShowInfoMessage("The specified setting file is not found!",
							"Read Setting File");
						return false;
					}
					StreamReader strReader = File.OpenText(fileName);
					string strTemp = strReader.ReadToEnd();
					strReader.Close();
					return ReadFromString(strTemp);
				} 
				catch (IOException e)
				{
					EditControl.ShowInfoMessage(e.Message, "Read Setting File");
					return false;
				}
			}
			else
			{
				EditControl.ShowInfoMessage("No source file is specified!",
					"Read Setting File");
				return false;
			}
		}

		/// <summary>
		/// Writes the settings to a file.
		/// </summary>
		/// <param name="fileName">The file to write to.</param>
		/// <returns>true if the settings have been written successfully; 
		/// otherwise, false.</returns>
		internal bool WriteToFile(string fileName)
		{
			if ((fileName != null) && (fileName != string.Empty))
			{
				try 
				{
					StreamWriter strWriter = File.CreateText(fileName);
					strWriter.WriteLine(";" + " Essential Edit Settings");
					strWriter.WriteLine(string.Empty);
					strWriter.WriteLine("[Editor]");
					strWriter.WriteLine("LanguageName=" + LanguageName);
					strWriter.WriteLine("FileDescription=" + FileDescription);
					strWriter.WriteLine("FileExtension=" + FileExtension);
					strWriter.WriteLine("MatchCase=" + MatchCase);
					strWriter.WriteLine("AutoIndent=" + AutoIndent);
					strWriter.WriteLine("KeepTabs=" + (KeepTabs ? "1" : "0"));
					strWriter.WriteLine("SmartIndentChars=" + 
						SmartIndentBeginChar + SmartIndentEndChar);
					strWriter.WriteLine("SmartRepeatTag=" + SmartRepeatTag);
					strWriter.WriteLine(string.Empty);
					for (int i = 1; i <= ColorGroupInfoList.Count; i++)
					{
						strWriter.WriteLine("[ColorGroup" + i + "]");
						strWriter.WriteLine("GroupName=" 
							+ ((EditGroupInfo)ColorGroupInfoList[i-1]).GroupName);
						strWriter.WriteLine("Foreground=" 
							+ ((EditGroupInfo)ColorGroupInfoList[i-1]).Foreground);
						strWriter.WriteLine("Background=" 
							+ ((EditGroupInfo)ColorGroupInfoList[i-1]).Background);
						strWriter.WriteLine("ForeColorAutomatic=" 
							+ ((EditGroupInfo)ColorGroupInfoList[i-1]).ForeColorAutomatic);
						strWriter.WriteLine("BackColorAutomatic=" 
							+ ((EditGroupInfo)ColorGroupInfoList[i-1]).BackColorAutomatic);
						strWriter.WriteLine("GroupType=" 
							+ ((EditGroupInfo)ColorGroupInfoList[i-1]).GroupType);
						strWriter.WriteLine(string.Empty);
					}
					for (int i = 1; i <= TagInfoList.Count; i++)
					{
						strWriter.WriteLine("[Tag" + i + "]");
						strWriter.WriteLine("ColorGroup=" 
							+ ((EditTagInfo)TagInfoList[i-1]).ColorGroup);
						strWriter.WriteLine("BeginTag=" 
							+ ((EditTagInfo)TagInfoList[i-1]).BeginTag);
						strWriter.WriteLine("EndTag=" 
							+ ((EditTagInfo)TagInfoList[i-1]).EndTag);
						strWriter.WriteLine("MultiLine=" 
							+ ((EditTagInfo)TagInfoList[i-1]).MultiLine);
						strWriter.WriteLine("EscapeChar=" 
							+ ((EditTagInfo)TagInfoList[i-1]).EscapeChar);
						strWriter.WriteLine(string.Empty);
					}
					for (int i = 1; i <= AdvTagInfoList.Count; i++)
					{
						strWriter.WriteLine("[AdvTag" + i + "]");
						strWriter.WriteLine("ColorGroup=" 
							+ ((EditAdvTagInfo)AdvTagInfoList[i-1]).ColorGroup);
						strWriter.WriteLine("ContentColorGroup=" 
							+ ((EditAdvTagInfo)AdvTagInfoList[i-1]).ContentColorGroup);
						strWriter.WriteLine("ToLineEnd=" 
							+ ((EditAdvTagInfo)AdvTagInfoList[i-1]).ToLineEnd);
						strWriter.WriteLine("BeginTag=" 
							+ ((EditAdvTagInfo)AdvTagInfoList[i-1]).BeginTag);
						strWriter.WriteLine("EndTag=" 
							+ ((EditAdvTagInfo)AdvTagInfoList[i-1]).EndTag);
						strWriter.WriteLine("MultiLine=" 
							+ ((EditAdvTagInfo)AdvTagInfoList[i-1]).MultiLine);
						strWriter.WriteLine("EscapeChar=" 
							+ ((EditAdvTagInfo)AdvTagInfoList[i-1]).EscapeChar);
						strWriter.WriteLine(string.Empty);
						for (int j = 1; j <= ((EditAdvTagInfo)AdvTagInfoList[i-1]).
							SubTagInfoList.Count; j++)
						{
							EditTagInfo tempSubTag = (EditTagInfo)
								((EditAdvTagInfo)AdvTagInfoList[i-1]).SubTagInfoList[j-1];
							strWriter.WriteLine("[SubTag" + j + "]");
							strWriter.WriteLine("ParentTag" + "=" + "AdvTag" + i);
							strWriter.WriteLine("ColorGroup=" + tempSubTag.ColorGroup);
							strWriter.WriteLine("BeginTag=" + tempSubTag.BeginTag);
							strWriter.WriteLine("EndTag=" + tempSubTag.EndTag);
							strWriter.WriteLine("MultiLine=" + tempSubTag.MultiLine);
							strWriter.WriteLine("EscapeChar=" + tempSubTag.EscapeChar);
							strWriter.WriteLine(string.Empty);
						}
					}
					for (int i = 1; i <= OutliningTagInfoList.Count; i++)
					{
						strWriter.WriteLine("[Outlining" + i + "]");
						strWriter.WriteLine("Name=" 
							+ ((EditOutliningTagInfo)OutliningTagInfoList[i-1]).Name);
						strWriter.WriteLine("Keyword=" 
							+ ((EditOutliningTagInfo)OutliningTagInfoList[i-1]).Keyword);
						strWriter.WriteLine("BeginTag=" 
							+ ((EditOutliningTagInfo)OutliningTagInfoList[i-1]).BeginTag);
						strWriter.WriteLine("EndTag=" 
							+ ((EditOutliningTagInfo)OutliningTagInfoList[i-1]).EndTag);
						strWriter.WriteLine("LineStarting=" 
							+ ((EditOutliningTagInfo)OutliningTagInfoList[i-1]).LineStarting);
						strWriter.WriteLine("MultiLine=" 
							+ ((EditOutliningTagInfo)OutliningTagInfoList[i-1]).MultiLine);
						strWriter.WriteLine("CollapseAs=" 
							+ ((EditOutliningTagInfo)OutliningTagInfoList[i-1]).CollapseAs);
						if (((EditOutliningTagInfo)OutliningTagInfoList[i-1]).SubOutlining
							== null)
						{
							strWriter.WriteLine("SubOutlining=");
						}
						else
						{
							strWriter.WriteLine("SubOutlining=" 
								+ GetSubOutliningString(((EditOutliningTagInfo)
								OutliningTagInfoList[i-1]).SubOutlining));
						}
						strWriter.WriteLine(string.Empty);
					}
					strWriter.WriteLine("[Keywords]");
					for (int i = 1; i <= KeywordInfoList.Count; i++)
					{
						strWriter.WriteLine(
							((EditKeywordInfo)KeywordInfoList[i-1]).ColorGroup + "="
							+ ((EditKeywordInfo)KeywordInfoList[i-1]).Keyword);
					}
					strWriter.WriteLine(string.Empty);
					strWriter.Close();
					return true;
				} 
				catch (IOException e)
				{
					EditControl.ShowErrorMessage(e.Message, "Write Settings to File");
					return false;
				}
			}
			else
			{
				EditControl.ShowInfoMessage("No destination file is specified!", 
					"Write Settings to File");
				return false;
			}
		}

		/// <summary>
		/// Reads resource strings from the specified string.
		/// </summary>
		/// <param name="str">The string to read resource strings from.
		/// </param>
		/// <returns>true if resource strings have been read successfully; 
		/// otherwise, false.</returns>
		internal bool ReadResourceStrings(string str)
		{
			if ((str == null) || (str == string.Empty))
			{
				return false;
			}
			return ReadResourceStrings(EditControl.GetStringArrayList(str));
		}

		/// <summary>
		/// Reads resource strings from an arraylist of setting strings.
		/// </summary>
		/// <param name="al">The string arraylist from which resource strings 
		/// will be read.</param>
		/// <returns>true if the resource strings have been read successfully; 
		/// otherwise, false.</returns>
		internal bool ReadResourceStrings(ArrayList al)
		{
			int index = 0;
			string strLine;
			ArrayList tempStringList = new ArrayList(); 
			while (ReadNextLine(al, ref index, out strLine))
			{
				string str1 = GetStartString(strLine);
				string str2 = GetEndString(strLine);
				if (str2.Length > 2)
				{
					if ((str2[0] == '\"') && (str2[str2.Length - 1] == '\"'))
					{
						str2 = str2.Substring(1, str2.Length - 2);
					}
				}
				if ((str1.Length > 0) && (str2.Length > 0))
				{
					tempStringList.Add(str1);
					tempStringList.Add(str2);
				}
			}
			if (tempStringList.Count > 0)
			{
				ResourceStringList = tempStringList;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Tests if the specified string is a color group.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <returns>true if the string is a color group; otherwise, false.</returns>
		internal bool IsColorGroup(string str)
		{
			if ((str == null) || (str == string.Empty))
			{
				return false;
			}
			return (GetColorGroupIndex(str) >= 0);
		}

		/// <summary>
		/// Tests if the specified string is a keyword.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <returns>true if the string is a keyword; otherwise, false.</returns>
		internal bool IsKeyword(string str)
		{
			if ((str == null) || (str == string.Empty))
			{
				return false;
			}
			return (GetKeywordColorIndex(str) >= 0);
		}

		/// <summary>
		/// Tests if the specified pair of begin tag and end tag is a tag.
		/// </summary>
		/// <param name="beginTag">The begin tag to be tested.</param>
		/// <param name="endTag">The end tag to be tested.</param>
		/// <returns>true if the specified pair of begin tag and end tag is 
		/// a tag; otherwise, false.</returns>
		internal bool IsTag(string beginTag, string endTag)
		{
			return (GetTagIndex(beginTag, endTag) >= 0);
		}

		/// <summary>
		/// Gets the index for the specified pair of begin tag and end tag.
		/// </summary>
		/// <param name="beginTag"></param>
		/// <param name="endTag"></param>
		/// <returns>The index of the pair of begin tag and end tag.</returns>
		internal int GetTagIndex(string beginTag, string endTag)
		{
			EditTagInfo ti;
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				ti = (EditTagInfo)TagInfoList[i];
				if ((ti.BeginTag == beginTag) && (ti.EndTag == endTag))
				{
					return i;
				}
			}
			return -1;
		}		

		/// <summary>
		/// Gets the index of color group for the specified keyword.
		/// </summary>
		/// <param name="str">The specified keyword.</param>
		/// <returns>The index of color group for the keyword.</returns>
		internal int GetKeywordColorIndex(string str)
		{
			editKeywordInfoTemp.Keyword = bIgnoreCase ? str.ToUpper() : str;
			int index = KeywordInfoList.BinarySearch(editKeywordInfoTemp);
			if (index >= 0)
			{
				return ((EditKeywordInfo)KeywordInfoList[index]).ColorGroupIndex;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// Tests if the specified string starts with a non-letter-digit char.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <returns>true if the string starts with a non-letter-digit char; 
		/// otherwise, false.</returns>
		internal bool HasSpecialStartChar(string str)
		{
			if ((str == null) || (str == string.Empty))
			{
				return false;
			}
			return ((!Char.IsLetterOrDigit(str[0])) && (str[0] != '_'));
		}

		/// <summary>
		/// Tests if the specified string ends with a non-letter-digit char.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <returns>true if the string ends with non-letter-digit char;
		/// otherwise, false.</returns>
		internal bool HasSpecialEndChar(string str)
		{
			if ((str == null) || (str == string.Empty))
			{
				return false;
			}
			return ((!Char.IsLetterOrDigit(str[str.Length - 1])) 
				&& (str[str.Length - 1] != '_'));
		}

		/// <summary>
		/// Tests if the specified string has a non-letter-digit middle char.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <returns>true if the string has a non-letter-digit char;
		/// otherwise, false.</returns>
		internal bool HasSpecialMiddleChar(string str)
		{
			// If the string is empty or its length is less than 3, 
			// there are no middle chars.
			if ((str == null) || (str.Length < 3))
			{
				return false;
			}
			for (int i = 1; i < str.Length - 1; i++)
			{
				if ((!Char.IsLetterOrDigit(str[i])) && (str[i] != '_'))

				{
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Methods Related to Parsing.

		/// <summary>
		/// Returns the characters that start the single-line comment.
		/// </summary>
		/// <returns>The tag for line comment.</returns>
		internal string GetLineCommentTag()
		{
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				// The first line comment tag will be returned.
				if ((((EditTagInfo)TagInfoList[i]).ColorGroup.ToUpper() 
					== "Comment".ToUpper())
					&& (((EditTagInfo)TagInfoList[i]).MultiLine != "1"))
				{
					string strTemp = ((EditTagInfo)TagInfoList[i]).BeginTag;
					// If the tag ends with a letter/number, add an ending space.
					if (Char.IsLetterOrDigit(strTemp[strTemp.Length - 1]))
					{
						strTemp += " ";
					}
					return strTemp;
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// Gets the regular expression for all the keywords.
		/// </summary>
		/// <returns>The regular expression for keywords.</returns>
		internal string GetKeywordsRegExp()
		{
			string strTemp = string.Empty;
			for (int i = 0; i < KeywordInfoList.Count; i++)
			{
				if (strTemp != string.Empty)
				{
					strTemp += "|";
				}
				strTemp += GetOneKeywordRegExp(((EditKeywordInfo)
					KeywordInfoList[i]).Keyword);
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for the specified keyword.
		/// </summary>
		/// <param name="str">The keyword for which the regular expression is 
		/// to be obtained.</param>
		/// <returns>The regular expression of the specified keyword.</returns>
		internal string GetOneKeywordRegExp(string str)
		{
			string strTemp = string.Empty;
			if ((str == null) || (str == string.Empty))
			{
				return strTemp;
			}
			if ((!HasSpecialStartChar(str)) && (!HasSpecialEndChar(str)) 
				&& (!HasSpecialMiddleChar(str)))
			{
				strTemp += "\\b" + Regex.Escape(str) + "\\b";
			}
			else if ((HasSpecialStartChar(str)) && (!HasSpecialEndChar(str)) 
				&& (!HasSpecialMiddleChar(str)))
			{
				strTemp += Regex.Escape(str) + "\\b";
			}
			else if ((!HasSpecialStartChar(str)) && (HasSpecialEndChar(str)) 
				&& (!HasSpecialMiddleChar(str)))
			{
				strTemp += "\\b" + Regex.Escape(str);
			}
			else
			{
				strTemp += Regex.Escape(str);
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for singleline blocks of the current 
		/// language.
		/// </summary>
		/// <returns>The regular expression for singleline blocks.</returns>
		internal string GetSingleLineBlockRegExp()
		{
			string strTemp = string.Empty;
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				if (((EditAdvTagInfo)AdvTagInfoList[i]).MultiLine != "1")
				{
					if (((EditAdvTagInfo)AdvTagInfoList[i]).EndTag != string.Empty)
					{
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						if (((EditAdvTagInfo)AdvTagInfoList[i]).EscapeChar == string.Empty)
						{
							strTemp +=  
								Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).BeginTag) 
								+ ".*?" 
								+ Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).EndTag);
						}
						else
						{
							strTemp += 
								Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).BeginTag) + 
								".*?" + "(?<!" 
								+ Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).EscapeChar) 
								+ ")" +
								Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).EndTag);
						}
						// Consider the case when the end tag is missing.
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						strTemp +=
							Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).BeginTag) 
							+ ".*$";
					}
					else
					{
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						if (Char.IsLetterOrDigit(((EditAdvTagInfo)AdvTagInfoList[i]).
							BeginTag[((EditAdvTagInfo)AdvTagInfoList[i]).BeginTag.Length - 1]))
						{
							strTemp +=
								Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).BeginTag) 
								+ "\\b.*$";
						}
						else
						{
							strTemp += 
								Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).BeginTag) 
								+ ".*$";
						}
					}
				}
				else
				{
					// Multiline tagged blocks may also appear in one line.
					if (strTemp != string.Empty)
					{
						strTemp += "|";
					}
					if (((EditAdvTagInfo)AdvTagInfoList[i]).EscapeChar == string.Empty)
					{
						strTemp +=  
							Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).BeginTag) 
							+ ".*?" 
							+ Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).EndTag);
					}
					else
					{
						strTemp += 
							Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).BeginTag) + 
							".*?" + "(?<!" 
							+ Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).EscapeChar) 
							+ ")" +
							Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).EndTag);
					}
				}
			}
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				if (((EditTagInfo)TagInfoList[i]).MultiLine != "1")
				{
					if (((EditTagInfo)TagInfoList[i]).EndTag != string.Empty)
					{
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						if (((EditTagInfo)TagInfoList[i]).EscapeChar == string.Empty)
						{
							strTemp +=  
								Regex.Escape(((EditTagInfo)TagInfoList[i]).BeginTag) 
								+ ".*?" 
								+ Regex.Escape(((EditTagInfo)TagInfoList[i]).EndTag);
						}
						else
						{
							strTemp +=  
								Regex.Escape(((EditTagInfo)TagInfoList[i]).BeginTag) + 
								".*?" + "(?<!" 
								+ Regex.Escape(((EditTagInfo)TagInfoList[i]).EscapeChar) 
								+ ")" +
								Regex.Escape(((EditTagInfo)TagInfoList[i]).EndTag);
						}
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						strTemp += 
							Regex.Escape(((EditTagInfo)TagInfoList[i]).BeginTag) 
							+ ".*$";
					}
					else
					{
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						if (Char.IsLetterOrDigit(((EditTagInfo)TagInfoList[i]).
							BeginTag[((EditTagInfo)TagInfoList[i]).BeginTag.Length - 1]))
						{
							strTemp += 
								Regex.Escape(((EditTagInfo)TagInfoList[i]).BeginTag) 
								+ "\\b.*$";
						}
						else
						{
							strTemp += 
								Regex.Escape(((EditTagInfo)TagInfoList[i]).BeginTag) 
								+ ".*$";
						}
					}
				}
				else
				{
					// Multiline tagged blocks may also appear in one line.
					if (strTemp != string.Empty)
					{
						strTemp += "|";
					}
					if (((EditTagInfo)TagInfoList[i]).EscapeChar == string.Empty)
					{
						strTemp +=  
							Regex.Escape(((EditTagInfo)TagInfoList[i]).BeginTag) 
							+ ".*?" 
//							+ Regex.Escape(((EditTagInfo)TagInfoList[i]).EndTag);
							+ GetTagRegexString(((EditTagInfo)TagInfoList[i]).EndTag);
					}
					else
					{
						strTemp +=  
							Regex.Escape(((EditTagInfo)TagInfoList[i]).BeginTag) + 
							".*?" + "(?<!" 
							+ Regex.Escape(((EditTagInfo)TagInfoList[i]).EscapeChar) 
							+ ")"
//							+ Regex.Escape(((EditTagInfo)TagInfoList[i]).EndTag);
							+ GetTagRegexString(((EditTagInfo)TagInfoList[i]).EndTag);
					}
				}
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for all the multiline begin tags.
		/// </summary>
		/// <returns>The regular expression for all the multiline begin tags.
		/// </returns>
		internal string GetMultiLineBeginTagsRegExp()
		{
			string strTemp = string.Empty;
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				if (((EditAdvTagInfo)AdvTagInfoList[i]).MultiLine == "1")
				{
					if (strTemp != string.Empty)
					{
						strTemp += "|";
					}
					strTemp +=  
						Regex.Escape(((EditAdvTagInfo)AdvTagInfoList[i]).BeginTag);
				}
			}
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				if (((EditTagInfo)TagInfoList[i]).MultiLine == "1")
				{
					if (strTemp != string.Empty)
					{
						strTemp += "|";
					}
					strTemp +=  
						Regex.Escape(((EditTagInfo)TagInfoList[i]).BeginTag);
				}
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the index of the advanced tag starting the specified string.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		internal short GetAdvTagIndex(string str)
		{
			string strTemp = (!bIgnoreCase)? str : str.ToUpper();
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				if (strTemp.StartsWith(((EditAdvTagInfo)
					AdvTagInfoList[i]).BeginTag))
				{
					return (short)i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Tests if the specified string ends with the specified advanced tag.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="advTagIndex"></param>
		/// <returns></returns>
		internal bool EndsWithAdvTag(string str, short advTagIndex)
		{
			string strTemp = (!bIgnoreCase)? str : str.ToUpper();
			return (strTemp.EndsWith(((EditAdvTagInfo)
				AdvTagInfoList[advTagIndex]).EndTag));
		}

		/// <summary>
		/// Gets the color group for the specified string if it starts 
		/// with a begin tag.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		internal short GetBeginTagColorIndex(string str)
		{
			string strTemp = (!bIgnoreCase)? str : str.ToUpper();
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				if (strTemp.StartsWith(((EditTagInfo)TagInfoList[i]).BeginTag))
				{
					return ((EditTagInfo)TagInfoList[i]).ColorGroupIndex;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the color group for the specified string if it starts with
		/// a multiline begin tag.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		internal short GetMultiLineBeginTagOnlyColorIndex(string str)
		{
			string strTemp = (!bIgnoreCase)? str : str.ToUpper();
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				if ((((EditTagInfo)TagInfoList[i]).MultiLine == "1") 
					&& (strTemp == ((EditTagInfo)TagInfoList[i]).BeginTag))
				{
					return ((EditTagInfo)TagInfoList[i]).ColorGroupIndex;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the regular expression for all the keywords of the 
		/// specified advanced tag.
		/// </summary>
		/// <returns>The regular expression for subkeywords.</returns>
		internal string GetSubKeywordsRegExp(EditAdvTagInfo ati)
		{
			string strTemp = string.Empty;
			for (int i = 0; i < ati.SubKeywordInfoList.Count; i++)
			{
				if (strTemp != string.Empty)
				{
					strTemp += "|";
				}
				strTemp += GetOneKeywordRegExp(((EditKeywordInfo)ati.
					SubKeywordInfoList[i]).Keyword);
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for the internal content of the 
		/// specified EditAdvTagInfo object.
		/// </summary>
		/// <param name="ati">The specified EditAdvTagInfo object.</param>
		/// <returns>The regular expression for the internal content of the 
		/// given EditAdvTagInfo object.</returns>
		internal string GetInternalRegExp(EditAdvTagInfo ati)
		{
			string strTemp = string.Empty;
			string strTemp1 = string.Empty;
			if ((strTemp1 = GetAdvTagSubTagRegExp(ati)) != string.Empty)
			{
				if (strTemp != string.Empty)
				{
					strTemp += "|";
				}
				strTemp += strTemp1;
			}
			if ((strTemp1 = GetSubKeywordsRegExp(ati)) != string.Empty)
			{
				if (strTemp != string.Empty)
				{
					strTemp += "|";
				}
				strTemp += strTemp1;
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the index of color group for the specified keyword for 
		/// the specified advanced tag.
		/// </summary>
		/// <param name="str">The specified keyword.</param>
		/// <returns>The index of color group for the keyword.</returns>
		internal int GetKeywordColorIndex(int tagIndex, string str)
		{
			EditAdvTagInfo ati = (EditAdvTagInfo)AdvTagInfoList[tagIndex];
			editKeywordInfoTemp.Keyword = bIgnoreCase ? str.ToUpper() : str;
			int index = ati.SubKeywordInfoList.BinarySearch(editKeywordInfoTemp);
			if (index >= 0)
			{
				return ((EditKeywordInfo)ati.SubKeywordInfoList[index]).
					ColorGroupIndex;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// Gets the color group for the specified string if it starts 
		/// with an advanced begin tag.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		internal short GetMultiLineAdvBeginTagOnlyColorIndex(string str)
		{
			string strTemp = (!bIgnoreCase)? str : str.ToUpper();
			EditAdvTagInfo ati;
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				ati = (EditAdvTagInfo)AdvTagInfoList[i];
				if (ati.MultiLine == "1")
				{
					if (((ati.ToLineEnd == "1") && strTemp.StartsWith(ati.BeginTag))
						|| (strTemp == ati.BeginTag))
					{
						return ati.ColorGroupIndex;
					}
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the index of the specified begin tag.
		/// </summary>
		/// <param name="str">The string for the begin tag.</param>
		internal short GetTagIndex(string str)
		{
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				if (str.StartsWith(((EditTagInfo)TagInfoList[i]).BeginTag))
				{
					return (short)i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the index of an end tag.
		/// </summary>
		/// <param name="str"></param>
		internal short GetEndTagIndex(string str)
		{
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				if ((((EditTagInfo)TagInfoList[i]).EndTag != string.Empty) 
					&& (str.EndsWith(((EditTagInfo)TagInfoList[i]).EndTag)))
				{
					return (short)i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the regular expression for the whole language.
		/// </summary>
		/// <returns></returns>
		internal string GetLangRegExp()
		{
			string strTemp = string.Empty;
			string strTemp1 = string.Empty;
			if ((strTemp1 = GetSingleLineBlockRegExp()) != string.Empty)
			{
				strTemp += strTemp1;
			}
			if ((strTemp1 = GetKeywordsRegExp()) != string.Empty)
			{
				if (strTemp != string.Empty)
				{
					strTemp += "|";
				}
				strTemp += strTemp1;
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for tags that start a block of 
		/// color text.
		/// </summary>
		/// <returns></returns>
		internal string GetBeginTagCheckingRegexp()
		{
			string strTemp = string.Empty;
			string strTemp1 = string.Empty;
			if ((strTemp1 = GetSingleLineBlockRegExp()) != string.Empty)
			{
				strTemp += strTemp1;
			}
			if ((strTemp1 = GetMultiLineBeginTagsRegExp()) != string.Empty)
			{
				if (strTemp != string.Empty)
				{
					strTemp += "|";
				}
				strTemp += strTemp1;
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for a string mixed with single-line 
		/// blocks.  
		/// </summary>
		/// <returns></returns>
		internal string GetTagCheckingRegExp(string str)
		{
			string strTemp = string.Empty;
			string strTemp1 = string.Empty;
			if ((strTemp1 = GetSingleLineBlockRegExp()) != string.Empty)
			{
				strTemp += strTemp1;
			}
			if (str != string.Empty)
			{
				if (strTemp != string.Empty)
				{
					strTemp += "|";
				}
				strTemp += Regex.Escape(str);
			}
			return strTemp;
		}

		/// <summary>
		/// Gets a value indicating whether there is any multiline tag.
		/// </summary>
		/// <returns>true if there is a multiline tag; otherwise, false.
		/// </returns>
		internal bool MultiLineTagPresent()
		{
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				if (((EditTagInfo)TagInfoList[i]).MultiLine == "1")
				{
					return true;
				}
			}
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				if (((EditAdvTagInfo)AdvTagInfoList[i]).MultiLine == "1")
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets a value indicating whether there is any single-line tag.
		/// </summary>
		/// <returns>true if there is a single-line tag; otherwise, false.
		/// </returns>
		internal bool SingleLineTagPresent()
		{
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				if (((EditTagInfo)TagInfoList[i]).MultiLine != "1")
				{
					return true;
				}
			}
			for (int i = 0; i < AdvTagInfoList.Count; i++)
			{
				if (((EditAdvTagInfo)AdvTagInfoList[i]).MultiLine != "1")
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the font with the specified properties.
		/// </summary>
		/// <param name="fontFamilyName">The family name of the font.</param>
		/// <param name="fontSize">The size of the font.</param>
		/// <param name="fontBold">The bold status of the font.</param>
		/// <returns>The font with the given properties.</returns>
		internal Font GetFont(string fontFamilyName, string fontSize, 
			string fontBold)
		{
			FontFamily fontFamily = new FontFamily(fontFamilyName);
			if (fontBold != "1")
			{
				if(fontFamily.IsStyleAvailable(FontStyle.Regular))
				{
					return new Font(fontFamilyName, 
						Int32.Parse(fontSize), FontStyle.Regular);
				}
				else if(fontFamily.IsStyleAvailable(FontStyle.Bold))
				{
					return new Font(fontFamilyName, 
						Int32.Parse(fontSize), FontStyle.Bold);
				}
				else if(fontFamily.IsStyleAvailable(FontStyle.Italic))
				{
					return new Font(fontFamilyName, 
						Int32.Parse(fontSize), FontStyle.Italic);
				}
				else
				{
					return new Font(FontFamily.GenericMonospace.Name, 
						Int32.Parse(fontSize), FontStyle.Regular);
				}
			}
			else
			{
				if(fontFamily.IsStyleAvailable(FontStyle.Bold))
				{
					return new Font(fontFamilyName, 
						Int32.Parse(fontSize), FontStyle.Bold);
				}
				else if(fontFamily.IsStyleAvailable(FontStyle.Regular))
				{
					return new Font(fontFamilyName, 
						Int32.Parse(fontSize), FontStyle.Regular);
				}
				else if(fontFamily.IsStyleAvailable(FontStyle.Italic))
				{
					return new Font(fontFamilyName, 
						Int32.Parse(fontSize), FontStyle.Italic);
				}
				else
				{
					return new Font(FontFamily.GenericMonospace.Name, 
						Int32.Parse(fontSize), FontStyle.Bold);
				}
			}
		}

		#endregion

		#region Methods Related to Outlining

		/// <summary>
		/// Tests if the tag with the specified index represents a multiline 
		/// comment tag.
		/// </summary>
		/// <param name="tagIndex">The tag index to be tested.</param>
		/// <returns>true if the tag with the specified index represents a 
		/// multiline comment tag; otherwise, false.</returns>
		internal bool IsMultiLineCommentTag(int tagIndex)
		{
			return ((((EditTagInfo)TagInfoList[tagIndex]).MultiLine == "1")
				&& (((EditTagInfo)TagInfoList[tagIndex]).ColorGroup == "Comment"));
		}

		/// <summary>
		/// Gets the regular expression for single-line comments.
		/// </summary>
		/// <returns></returns>
		internal string GetSingleLineCommentRegExp()
		{
			string strTemp = string.Empty;
			for (int i = 0; i < TagInfoList.Count; i++)
			{
				if (((EditTagInfo)TagInfoList[i]).ColorGroup.ToUpper() 
					== "Comment".ToUpper())
				{
					if (((EditTagInfo)TagInfoList[i]).MultiLine != "1")
					{
						if (((EditTagInfo)TagInfoList[i]).EndTag 
							== string.Empty)
						{
							if (strTemp != string.Empty)
							{
								strTemp += "|";
							}
							strTemp += "(?:^[ \t]*"
								+ Regex.Escape(((EditTagInfo)
								TagInfoList[i]).BeginTag) 
								+ ".*$)";
						}
						else
						{
							if (strTemp != string.Empty)
							{
								strTemp += "|";
							}
							strTemp += "(?:^[ \t]*" 
								+ Regex.Escape(((EditTagInfo)
								TagInfoList[i]).BeginTag) 
								+ ".*?" 
								+ Regex.Escape(((EditTagInfo)
								TagInfoList[i]).EndTag) 
								+ "[ \t]*)";
						}
					}
					else
					{
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						strTemp += "(?:^[ \t]*" 
							+ Regex.Escape(((EditTagInfo)
							TagInfoList[i]).BeginTag) 
							+ ".*?" 
							+ Regex.Escape(((EditTagInfo)
							TagInfoList[i]).EndTag) 
							+ "[ \t]*)";
					}
				}
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for a single-line outlining.
		/// </summary>
		/// <returns></returns>
		internal string GetSingleLineOutliningRegExp()
		{
			string strTemp = string.Empty;
			for (int i = 0; i < OutliningTagInfoList.Count; i++)
			{
				EditOutliningTagInfo oti = 
					(EditOutliningTagInfo)OutliningTagInfoList[i];
				if (oti.MultiLine != "1")
				{
					if (oti.EndTag != string.Empty)
					{
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						strTemp += "(?:^[ \t]*" + Regex.Escape(oti.BeginTag) 
							+ ".*?" + Regex.Escape(oti.EndTag) + ")";
					}
					else
					{
						if (strTemp != string.Empty)
						{
							strTemp += "|";
						}
						if (EditData.IsWordChar(oti.BeginTag[oti.BeginTag.Length - 1]))
						{
							strTemp += "(?:^[ \t]*" + Regex.Escape(oti.BeginTag) 
								+ "\\b.*$)";
						}
						else
						{
							strTemp += "(?:^[ \t]*" + Regex.Escape(oti.BeginTag) 
								+ ".*$)";
						}
					}
				}
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the outlining whose begin tag starts the specified string.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		internal EditOutliningTagInfo GetOutliningTag(string str)
		{
			for (int i = 0; i < OutliningTagInfoList.Count; i++)
			{
				EditOutliningTagInfo oti = 
					(EditOutliningTagInfo)OutliningTagInfoList[i];
				if (str.Trim().StartsWith(oti.BeginTag))
				{
					return oti;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets the regular expression for all the begin tags for 
		/// multiline outlining.
		/// </summary>
		/// <returns></returns>
		internal string GetMultiLineOutlingBeginTagsRegExp()
		{
			string strTemp = string.Empty;
			for (int i = 0; i < OutliningTagInfoList.Count; i++)
			{
				EditOutliningTagInfo oti = 
					(EditOutliningTagInfo)OutliningTagInfoList[i];
				if (oti.MultiLine == "1")
				{
					if (strTemp != string.Empty)
					{
						strTemp += "|";
					}
					strTemp += "(?:" + Regex.Escape(oti.BeginTag) + ")";
				}
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for checking the begin tags for 
		/// multiline outlining.
		/// </summary>
		/// <returns></returns>
		internal string GetMultiLineOutlingBeginTagCheckingRegExp()
		{
			string strTemp = string.Empty;
			string strTemp1 = string.Empty;
			if ((strTemp1 = GetSingleLineBlockRegExp()) != string.Empty)
			{
				strTemp += strTemp1;
			}
			if ((strTemp1 = GetMultiLineOutlingBeginTagsRegExp()) 
				!= string.Empty)
			{
				if (strTemp != string.Empty)
				{
					strTemp += "|";
				}
				strTemp += "(?:" + strTemp1 + ")";
			}
			return strTemp;
		}

		/// <summary>
		/// Gets the regular expression for all the end tags of multiline 
		/// outlining blocks.
		/// </summary>
		/// <returns>The regular expression for all the end tags of 
		/// multiline outlining blocks.</returns>
		internal string GetMultiLineOutliningEndTagsRegExp()
		{
			string strTemp = string.Empty;
			for (int i = 0; i < OutliningTagInfoList.Count; i++)
			{
				EditOutliningTagInfo oti = 
					(EditOutliningTagInfo)OutliningTagInfoList[i];
				if (oti.MultiLine == "1")
				{
					if (strTemp != string.Empty)
					{
						strTemp += "|";
					}
					strTemp += Regex.Escape(oti.EndTag);
				}
			}
			return strTemp;
		}

		#endregion

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether character case is ignored.
		/// </summary>
		internal bool IgnoreCase
		{
			get
			{
				bIgnoreCase = (MatchCase != "1");
				return bIgnoreCase;
			}
			set
			{
				bIgnoreCase = value;
				MatchCase = bIgnoreCase ? "0" : "1";
			}
		}

		/// <summary>
		/// Gets or sets the font for the control.
		/// </summary>
		internal Font EditFont
		{
			get
			{
				return GetFont(EditFontFamily, EditFontSize, EditFontBold);
			}
			set
			{
				EditFontFamily = value.FontFamily.Name;
				EditFontSize = value.Size.ToString();
				EditFontBold = value.Bold ? "1" : "0";
			}
		}

	#endregion
	}
}
