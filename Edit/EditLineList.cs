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

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditLineList class stores text and color information for
	/// EditControl with an ArrayList of EditLine objects. 
	/// </summary>
	internal class EditLineList
	{
		#region Data Members

		/// <summary>
		/// The ArrayList for EditLine objects.
		/// </summary>
		private ArrayList editLineList = new ArrayList();

		#endregion

		#region Methods

		/// <summary>
		/// Adds an EditLine object to the internal Arraylist based on the 
		/// specified string.
		/// </summary>
		/// <param name="str">The string for the new EditLine object.</param>
		/// <returns>The index in the internal ArrayList at which the new 
		/// EditLine object has been added.</returns>
		internal int Add(string str)
		{
			return editLineList.Add(new EditLine(str));
		}

		/// <summary>
		/// Adds the specified EditLine object to the internal ArrayList.
		/// </summary>
		/// <param name="editLine">An EditLine object to be added.</param>
		/// <returns>The index in the internal ArrayList at which the 
		/// EditLine object has been added.</returns>
		internal int Add(EditLine editLine)
		{
			return editLineList.Add(editLine);
		}

		/// <summary>
		/// Inserts the specified EditLine to the internal ArrayList at 
		/// the specified index.
		/// </summary>
		/// <param name="index">The index of internal ArrayList at which 
		/// the EditLine object should be inserted.</param>
		/// <param name="editLine">An EditLine object to be inserted.</param>
		internal void Insert(int index, EditLine editLine)
		{
			editLineList.Insert(index, editLine);
		}

		/// <summary>
		/// Remove the EditLine object at the specified index from the 
		/// internal ArrayList.
		/// </summary>
		/// <param name="index">The index of the EditLine object to be removed.
		/// </param>
		internal void RemoveAt(int index)
		{
			editLineList.RemoveAt(index);
		}

		/// <summary>
		/// Clears the internal ArrayList.
		/// </summary>
		internal void Clear()
		{
			editLineList.Clear();
			editLineList.TrimToSize();
		}

		#endregion

		#region Properties

		/// <summary>
		/// The index operator.
		/// </summary>
		internal EditLine this[int i]
		{
			get
			{
				return (EditLine)editLineList[i];
			}
			set
			{
				editLineList[i] = value;
			}
		}

		/// <summary>
		/// The count of EditLine objects.
		/// </summary>
		internal int Count
		{
			get
			{
				return editLineList.Count;
			}
		}

		#endregion
	}
}
