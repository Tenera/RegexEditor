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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The class for storing a keyword and its color group.
	/// </summary>
	internal class EditKeywordInfo : IComparable
	{
		#region Data Members

		/// <summary>
		/// The string of the keyword.
		/// </summary>
		internal string Keyword = string.Empty;
		/// <summary>
		/// The name of the color group for the keyword.
		/// </summary>
		internal string ColorGroup = string.Empty;
		/// <summary>
		/// The index of the color group for the keyword.
		/// </summary>
		internal short ColorGroupIndex = -1;

		#endregion

		#region Methods

		/// <summary>
		/// Overloaded constructor. Creates a new EditLine object with the data members
		/// set to be the specified values.
		/// </summary>
		/// <param name="keyword">Initial keyword.</param>
		/// <param name="colorGroup">Initial colorGroup.</param>
		internal EditKeywordInfo(string keyword, string colorGroup)
		{
			this.Keyword = keyword;
			this.ColorGroup = colorGroup;
		}

		/// <summary>
		/// Overloaded. Compares the current EditKeywordInfo object with a specified 
		/// EditKeywordInfo object.
		/// </summary>
		/// <param name="o">An EditKeywordInfo object to compare.</param>
		/// <returns>A signed number indicating the relative values (based on the 
		/// index of the starting char) of the current EditKeywordInfo object and specified 
		/// EditKeywordInfo object:  
		/// -1 - the current EditKeywordInfo object is smaller than the specified EditKeywordInfo object;
		/// 0 - the current EditKeywordInfo object is equal to the specified EditKeywordInfo object;
		/// 1 - the current EditKeywordInfo object is larger than the specified EditKeywordInfo object.</returns>
		int IComparable.CompareTo(object o)
		{
			EditKeywordInfo lcTemp = (EditKeywordInfo)o;
			return this.Keyword.CompareTo(lcTemp.Keyword);
		}

		#endregion
	}
}
