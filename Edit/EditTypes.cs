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

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// Specifies the indent type for EditControl.
	/// </summary>
	public enum EditIndentType : byte
	{
		/// <summary>
		/// No indent.
		/// </summary>
		None = 0,
		/// <summary>
		/// Block indent.
		/// </summary>
		Block = 1,
		/// <summary>
		/// Smart indent.
		/// </summary>
		Smart = 2
	}

	/// <summary>
	/// Specifies the outlining symbol type for EditControl.
	/// </summary>
	public enum EditOutliningSymbolType : byte
	{
		/// <summary>
		/// Nothing.
		/// </summary>
		None = 0,
		/// <summary>
		/// A vertical line.
		/// </summary>
		InsideBlock = 1,
		/// <summary>
		/// A minus sign in a rectangle with surrounding white space margin.
		/// </summary>
		BlockStart = 2,
		/// <summary>
		/// A terminating tick marker.
		/// </summary>
		BlockEnd = 3,
		/// <summary>
		/// A plus sign in a rectangle with surrounding white space margin.
		/// </summary>
		BlockCollapsed = 4,
		/// <summary>
		/// A minus sign in a rectangle.
		/// </summary>
		SubBlockStart = 5,
		/// <summary>
		/// A tick marker.
		/// </summary>
		SubBlockEnd = 6,
		/// <summary>
		/// A plus sign in a rectangle.
		/// </summary>
		SubBlockCollapsed = 7
	}

	/// <summary>
	/// Specifies the action type for EditControl.
	/// </summary>
	public enum EditActionType : byte
	{
		/// <summary>
		/// No action.
		/// </summary>
		None = 0,
		/// <summary>
		/// User action.
		/// </summary>
		UserAction = 1,
		/// <summary>
		/// Insert text.
		/// </summary>
		Insert = 2,
		/// <summary>
		/// Delete text.
		/// </summary>
		Delete = 3,
		/// <summary>
		/// Move caret.
		/// </summary>
		MoveCaret = 4,
		/// <summary>
		/// Select text.
		/// </summary>
		SelectText = 5,
		/// <summary>
		/// Cut selected text.
		/// </summary>
		Cut = 6,
		/// <summary>
		/// Paste text from the clipboard.
		/// </summary>
		Paste = 7,
		/// <summary>
		/// Type text.
		/// </summary>
		Type = 8,
		/// <summary>
		/// Press tab key.
		/// </summary>
		Tab = 9,
		/// <summary>
		/// Press enter key.
		/// </summary>
		Enter = 10,
		/// <summary>
		/// Press backspace key.
		/// </summary>
		Backspace = 11,
		/// <summary>
		/// Indent the selected text.
		/// </summary>
		Indent = 12,
		/// <summary>
		/// Unindent the selected text.
		/// </summary>
		Unindent = 13,
		/// <summary>
		/// Comment the selected text.
		/// </summary>
		CommentLines = 14,
		/// <summary>
		/// Uncomment the selected text.
		/// </summary>
		UncommentLines = 15,
		/// <summary>
		/// Drag and Drop.
		/// </summary>
		DragDrop = 16,
		/// <summary>
		/// Tabify/Untabify the selected text.
		/// </summary>
		ConvertTabsSpaces = 17,
		/// <summary>
		/// Automatically indent.
		/// </summary>
		AutoIndent = 18,
		/// <summary>
		/// Replace text.
		/// </summary>
		Replace = 19,
		/// <summary>
		/// Replace all.
		/// </summary>
		ReplaceAll = 20,
		/// <summary>
		/// Change case.
		/// </summary>
		ChangeCase = 21,
		/// <summary>
		/// Delete horizontal white space.
		/// </summary>
		DeleteHorizontalWhiteSpace = 22,
	}

	/// <summary>
	/// Specifies the color group type for EditControl.
	/// </summary>
	public enum EditColorGroupType : byte
	{
		/// <summary>
		/// Normal text.
		/// </summary>
		RegularText = 0,
		/// <summary>
		/// Bold text.
		/// </summary>
		BoldText = 1,
		/// <summary>
		/// Underline for text.
		/// </summary>
		UnderLine = 2,
		/// <summary>
		/// Frame-line for text.
		/// </summary>
		FrameLine = 3
	}
}
