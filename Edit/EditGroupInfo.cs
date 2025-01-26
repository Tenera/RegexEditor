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
	/// The EditGroupInfo class stores the information for a color group.
	/// </summary>
	internal class EditGroupInfo
	{
		/// <summary>
		/// The name for the coloring group.
		/// </summary>
		internal string GroupName = string.Empty;
		/// <summary>
		/// The foreground color in "R,G,B" format.
		/// </summary>
		internal string Foreground = string.Empty;
		/// <summary>
		/// The background color in "R,G,B" format.
		/// </summary>
		internal string Background = string.Empty;
		/// <summary>
		/// A value indicating whether the foreground color 
		/// uses the normal text color.
		/// </summary>
		internal string ForeColorAutomatic = string.Empty;
		/// <summary>
		/// A value indicating whether the foreground color
		/// uses the normal text color.
		/// </summary>
		internal string BackColorAutomatic = string.Empty;
		/// <summary>
		/// The name for the coloring group.
		/// </summary>
		internal string GroupType = string.Empty;
	}
}
