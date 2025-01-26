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
using System.Text.RegularExpressions;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditTagInfo class represents a generic tag.
	/// </summary>
	internal class EditTagInfo
	{
		/// <summary>
		/// The name for the tag.
		/// </summary>
		internal string Name = string.Empty;
		/// <summary>
		/// The beginning characters for the tag.
		/// </summary>
		internal string BeginTag = string.Empty;
		/// <summary>
		/// The ending characters for the tag.
		/// </summary>
		internal string EndTag = string.Empty;
		/// <summary>
		/// The escape char for the EndTag, i.e., the EndTag after this
		/// char will be treated as normal characters in matching.
		/// </summary>
		internal string EscapeChar = string.Empty;
		/// <summary>
		/// A value indicating whether tagged blocks could be multiline.
		/// </summary>
		internal string MultiLine = string.Empty;
		/// <summary>
		/// The color group for the tag.
		/// </summary>
		internal string ColorGroup = string.Empty;
		/// <summary>
		/// The color group index for the tag.
		/// </summary>
		internal short ColorGroupIndex = -1;
		/// <summary>
		/// The regular expression object for EndTag matching.
		/// </summary>
		internal Regex EndRegex = null;
	}

	/// <summary>
	/// The EditAdvTagInfo class represents an advanced tag that could 
	/// have internal coloring schemes.
	/// </summary>
	internal class EditAdvTagInfo : EditTagInfo
	{
		/// <summary>
		/// The color group for tagged content.
		/// </summary>
		internal string ContentColorGroup = string.Empty;
		/// <summary>
		/// Whether the tag color is valid to the line end, i.e., tagged 
		/// content at the same line with the tag will have the tag color.
		/// </summary>
		internal string ToLineEnd = string.Empty;
		/// <summary>
		/// The color group index for tagged content.
		/// </summary>
		internal short ContentColorGroupIndex = -1;
		/// <summary>
		/// The index of the SubTag being processed.
		/// </summary>
		internal short CurrentSubTagIndex = -1;
		/// <summary>
		/// The list of SubKeywords.
		/// </summary>
		internal ArrayList SubKeywordInfoList = new ArrayList();
		/// <summary>
		/// The list of SubTags.
		/// </summary>
		internal ArrayList SubTagInfoList = new ArrayList();
		/// <summary>
		/// The buffer for subtagged multiline blocks.
		/// </summary>
		internal EditMultiLineBlockList SubMultiLineBlockList;
		/// <summary>
		/// The regular expression object for internal matching.
		/// </summary>
		internal Regex InternalRegex = null;
	}

	/// <summary>
	/// The EditOutliningTagInfo class represents an outlining tag.
	/// </summary>
	internal class EditOutliningTagInfo
	{
		/// <summary>
		/// The name of the outlining.
		/// </summary>
		internal string Name;
		/// <summary>
		/// The keyword for the outlining.
		/// </summary>
		internal string Keyword;
		/// <summary>
		/// The beginning characters for the outlining.
		/// </summary>
		internal string BeginTag;
		/// <summary>
		/// The ending characters for the outlining.
		/// </summary>
		internal string EndTag;
		/// <summary>
		/// A value indicating whether the keyword should start a line.
		/// </summary>
		internal string LineStarting;
		/// <summary>
		/// A value indicating whether the outlining block could be multiline.
		/// </summary>
		internal string MultiLine;
		/// <summary>
		/// The string representing collapsed outlining blocks.
		/// </summary>
		internal string CollapseAs;
		/// <summary>
		/// A list of valid SubOutlining names.
		/// </summary>
		internal string [] SubOutlining;

		/// <summary>
		/// Default constructor. Creates a new EditOutliningTagInfo object 
		/// with default values for data members. 
		/// </summary>
		internal EditOutliningTagInfo()
		{
			this.Name = string.Empty;
			this.Keyword = string.Empty;
			this.BeginTag = string.Empty;
			this.EndTag = string.Empty;
			this.LineStarting = string.Empty;
			this.MultiLine = string.Empty;
			this.CollapseAs = string.Empty;
			this.SubOutlining = null;
		}

		/// <summary>
		/// Overloaded constructor. Creates a new EditOutliningTagInfo 
		/// object with specified values for data members.
		/// </summary>
		/// <param name="name">Initial value for Name.</param>
		/// <param name="keyword">Initial value for Keyword.</param>
		/// <param name="beginTag">Initial value for BeginTag.</param>
		/// <param name="endTag">Initial value for EndTag.</param>
		/// <param name="lineStarting">Initial value for LineStarting.</param>
		/// <param name="multiLine">Initial value for MultiLine.</param>
		/// <param name="collapseAs">Initial value for CollapseAs.</param>
		/// <param name="subOutlining">Initial value for SubOutlining.</param>
		internal EditOutliningTagInfo(string name, string keyword, 
			string beginTag, string endTag, string lineStarting, 
			string multiLine, string collapseAs, string [] subOutlining)
		{
			this.Name = name;
			this.Keyword = keyword;
			this.BeginTag = beginTag;
			this.EndTag = endTag;
			this.LineStarting = lineStarting;
			this.MultiLine = multiLine;
			this.CollapseAs = collapseAs;
			this.SubOutlining = subOutlining;
		}
	}
}
