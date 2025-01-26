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
	/// The EditActionStack class provides stack storage and management for 
	/// EditAction objects.
	/// </summary>
	internal class EditActionStack
	{

		#region Data Members

		/// <summary>
		/// The internal stack for EditAction objects.
		/// </summary>
		private Stack editActionStack = new Stack();

		#endregion

		#region Methods

		/// <summary>
		/// Pushes, or places, the specified EditAction object onto 
		/// the action stack.
		/// </summary>
		/// <param name="act">The EditAction object to be pushed.</param>
		internal void PushAction(EditAction act)
		{
			editActionStack.Push(act);
		}

		/// <summary>
		/// Pops, or removes, the EditAction object at the top of the
		/// action stack.
		/// </summary>
		/// <returns>the EditAction object at the top of the stack.</returns>
		internal EditAction PopAction()
		{
			return (EditAction)editActionStack.Pop();
		}

		/// <summary>
		/// Returns the action at the top of the Stack without removing it.
		/// </summary>
		/// <returns>the EditAction object at the top of the stack.</returns>
		internal EditAction PeekAction()
		{
			return (EditAction)editActionStack.Peek();
		}

		/// <summary>
		/// Removes all the EditAction objects from the stack.
		/// </summary>
		internal void Clear()
		{
			editActionStack.Clear();
		}

		#endregion

		#region internal Properties

		/// <summary>
		/// The count of the EditAction objects in the action stack.
		/// </summary>
		internal int Count
		{
			get
			{
				return editActionStack.Count;
			}
		}

		#endregion
	}
}
