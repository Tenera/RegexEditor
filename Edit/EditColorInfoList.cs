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
	/// The EditColorInfoList class represents coloring information 
	/// for a line with an internal arraylist of EditColorInfo objects 
	/// and provides methods to manage these EditColorInfo objects. 
	/// </summary>
	internal class EditColorInfoList
	{
		#region Data Members

		/// <summary>
		/// The arraylist for EditColorInfo objects.
		/// </summary>
		private ArrayList editColorInfoList;

		#endregion

		#region Methods

		/// <summary>
		/// Default constructor. Creates a new EditColorInfoList with 
		/// an empty internal arraylist for EditColorInfo objects.
		/// </summary>
		internal EditColorInfoList()
		{
			editColorInfoList = new ArrayList();
		}

		/// <summary>
		/// Overloaded constructor. Creates a new EditColorInfoList 
		/// object and copies all the values of data members from the 
		/// specified EditColorInfoList object.
		/// </summary>
		/// <param name="cil">The EditColorInfoList object from which 
		/// the values of data members will be copied.</param>
		internal EditColorInfoList(EditColorInfoList cil)
		{
			editColorInfoList = new ArrayList();
			if (cil != null)
			{
				for (int i = 0; i < cil.Count; i++)
				{
					Add(new EditColorInfo(cil[i]));
				}			
			}
		}

		/// <summary>
		/// Adds an EditColorInfo object to the internal arraylist.
		/// </summary>
		/// <param name="ci">The EditColorInfo object to be added.</param>
		/// <returns>The index in the internal arraylist at which the 
		/// EditColorInfo object has been added.</returns>
		internal int Add(EditColorInfo ci)
		{
			int index = editColorInfoList.BinarySearch(ci);
			if (index < 0)
			{
				editColorInfoList.Insert(~index, (EditColorInfo) ci);
			}
			return index;
		}

		/// <summary>
		/// Adds a new EditColorInfo object to the internal arraylist 
		/// based on the specified starting char, ending char, and 
		/// color group indexes.
		/// </summary>
		/// <param name="startChar">The starting char index for the 
		/// new EditColorInfo object.</param>
		/// <param name="endChar">The ending char index for the 
		/// new EditColorInfo object.</param>
		/// <param name="colorGroupIndex">The color group index for the 
		/// new EditColorInfo object.</param>
		/// <returns>The index in the internal arraylist at which the 
		/// new EditColorInfo object has been added.</returns>
		internal int Add(int startChar, int endChar, int colorGroupIndex)
		{
			return Add(new EditColorInfo(startChar, endChar, colorGroupIndex));
		}

		/// <summary>
		/// Removes the specified EditColorInfo object from the internal 
		/// arraylist.
		/// </summary>
		/// <param name="ci">The EditColorInfo object to be removed.</param>
		internal void Remove(EditColorInfo ci)
		{
			editColorInfoList.Remove((EditColorInfo) ci);
		}

		/// <summary>
		/// Removes the EditColorInfo object at the specified index from 
		/// the internal arraylist.
		/// </summary>
		/// <param name="index">The index of the EditColorInfo object 
		/// to be removed.</param>
		internal void RemoveAt(int index)
		{
			editColorInfoList.RemoveAt(index);
		}

		/// <summary>
		/// Clears the whole internal arraylist.
		/// </summary>
		internal void Clear()
		{
			editColorInfoList.Clear();
		}

		/// <summary>
		/// Gets the stored data in the format of an array of short integers.
		/// </summary>
		/// <returns>An array of short integers containing coloring information
		/// for a line.</returns>
		internal short [] GetDataArray()
		{
			short [] fTemps = new short[3*editColorInfoList.Count];
			int count = 0;
			for (int i = 0; i < editColorInfoList.Count; i++)
			{
				fTemps[count++] = (short)((EditColorInfo) editColorInfoList[i]).StartChar;
				fTemps[count++] = (short)((EditColorInfo) editColorInfoList[i]).EndChar;
				fTemps[count++] = (short)((EditColorInfo) editColorInfoList[i]).ColorGroupIndex;
			}
			return fTemps;
		}

		/// <summary>
		/// The index operator.
		/// </summary>
		internal EditColorInfo this[int i]
		{
			get
			{
				return (EditColorInfo) editColorInfoList[i];
			}
			set
			{
				editColorInfoList[i] = value;
			}
		}

		/// <summary>
		/// The count of EditColorInfo objects in the internal arraylist.
		/// </summary>
		internal int Count
		{
			get
			{
				return editColorInfoList.Count;
			}
		}

		#endregion
	}
}
