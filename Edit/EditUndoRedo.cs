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
	/// The EditUndoRedo class manages Redo/Undo actions for EditControl.
	/// </summary>
	internal class EditUndoRedo
	{
		#region Data Members

		/// <summary>
		/// The stack for undoable actions.
		/// </summary>
		private EditActionStack editUndoStack = new EditActionStack();
		/// <summary>
		/// The stack for redoable actions.
		/// </summary>
		private EditActionStack editRedoStack = new EditActionStack();

		#endregion

		#region Methods

		/// <summary>
		/// Adds the specified action to the Undo stack.
		/// </summary>
		/// <param name="act">The action to be added to the Undo stack.</param>
		internal void AddUndoAction(EditAction act)
		{
			editUndoStack.PushAction(act);
		}

		/// <summary>
		/// Adds the specifed action to the Redo stack.
		/// </summary>
		/// <param name="act">The action to be added to the Redo stack.</param>
		internal void AddRedoAction(EditAction act)
		{
			editRedoStack.PushAction(act);
		}

		/// <summary>
		/// Undoes the action at the top of the Undo stack.
		/// </summary>
		/// <returns>true if the action is undone successfully; 
		/// otherwise, false.</returns>
		internal bool Undo()
		{
			if(!CanUndo)
			{
				return true;
			}
			EditAction act = editUndoStack.PopAction();
			if(act.Undo())
			{
				editRedoStack.PushAction(act);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Redoes the action at the top of the Redo stack.
		/// </summary>
		/// <returns>true if the action is redone successfully; otherwise, false.
		/// </returns>
		internal bool Redo()
		{
			if(!CanRedo)
			{
				return true;
			}
			EditAction act = editRedoStack.PopAction();
			if(act.Redo())
			{
				editUndoStack.PushAction(act);
				return true;
			}	
			return false;
		}

		/// <summary>
		/// Undoes the specified number of actions.
		/// </summary>
		/// <param name="num">The number of actions to be undone.</param>
		/// <returns>true if all the specified actions are undone successfully; 
		/// otherwise, false.</returns>
		internal bool Undo(int num)
		{
			for(int i = 0; i < num; i++)
			{
				if(!Undo())
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Redoes the specified number of actions.
		/// </summary>
		/// <param name="num">The number of actions to be redone.</param>
		/// <returns>true if all the specified actions are redone successfully; 
		/// otherwise, false.</returns>
		internal bool Redo(int num)
		{
			for(int i = 0; i < num; i++)
			{
				if(!Redo())
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Pops, or removes, the EditAction object at the top of the Undo stack.
		/// </summary>
		internal void PopUndo()
		{
			editUndoStack.PopAction();
		}

		/// <summary>
		/// Pops, or removes, the EditAction object at the top of the Redo stack.
		/// </summary>
		internal void PopRedo()
		{
			editRedoStack.PopAction();
		}

		/// <summary>
		/// Clears all the actions in the Undo stack.
		/// </summary>
		internal void ClearUndo()
		{
			editUndoStack.Clear();
		}

		/// <summary>
		/// Clears all the actions in the Redo stack.
		/// </summary>
		internal void ClearRedo()
		{
			editRedoStack.Clear();
		}

		/// <summary>
		/// Gets the action at the top of the Undo stack without removing it.
		/// </summary>
		/// <returns>The EditAction object at the top of the Undo stack.
		/// </returns>
		internal EditAction PeekUndoAction()
		{
			return editUndoStack.PeekAction();
		}

		/// <summary>
		/// Gets the action at the top of the Redo stack without removing it.
		/// </summary>
		/// <returns>the EditAction object at the top of the Redo stack.
		/// </returns>
		internal EditAction PeekRedoAction()
		{
			return editRedoStack.PeekAction();
		}

		#endregion

		#region internal Properties

		/// <summary>
		/// Gets a value indicating whether there is any redoable action.
		/// </summary>
		internal bool CanRedo
		{
			get
			{
				return editRedoStack.Count > 0;
			}
		}

		/// <summary>
		/// Gets a value indicating whether there is any undoable action.
		/// </summary>
		internal bool CanUndo
		{
			get
			{
				return editUndoStack.Count > 0;
			}
		}

		/// <summary>
		/// Gets the number of actions in the Undo stack.
		/// </summary>
		internal int UndoableActionCount
		{
			get
			{
				return editUndoStack.Count;
			}
		}

		/// <summary>
		/// Gets the number of actions in the Redo stack.
		/// </summary>
		internal int RedoableActionCount
		{
			get
			{
				return editRedoStack.Count;
			}
		}

		#endregion
	}
}
