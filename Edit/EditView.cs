//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
//  Copyright Syncfusion Inc. 2001 - 2003. All rights reserved. Use of this code is subject to the terms of our 
//  license. A copy of the current license can be obtained at any time by e-mailing licensing@syncfusion.com. 
//  Re-distribution in any form is strictly prohibited. Any infringement will be prosecuted under applicable laws. 
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
 
/*******************************************************************************
*                    Essential Edit - A syntax coloring edit                   *
*                                Author: B. Wu                                 *
********************************************************************************/

using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditView class presents the contents of EditControl.
	/// </summary>
	internal class EditView : System.Windows.Forms.UserControl
	{
		#region Data Members

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// The associated EditControl object.
		/// </summary>
		private EditControl edit;
		/// <summary>
		/// The EditData object that contains text and color information.
		/// </summary>
		private EditData editData;
		/// <summary>
		/// The EditSettings object that contains settings.
		/// </summary>
		private EditSettings editSettings;

		/// <summary>
		/// The horizontal scrollBar.
		/// </summary>
		private EditHScrollBar editHScrollBar = new EditHScrollBar();
		/// <summary>
		/// The vertical scrollBar.
		/// </summary>
		private EditVScrollBar editVScrollBar = new EditVScrollBar();
		/// <summary>
		/// The bottom-right corner area.
		/// </summary>
		private EditArea editCornerArea = new EditArea();
		/// <summary>
		/// The button for the minimized splitter.
		/// </summary>
		private EditButton editSplitterButton = new EditButton();
		/// <summary>
		/// A value indicating whether the splitter button is visible.
		/// </summary>
		private bool bSplitterButtonVisible = true;

		/// <summary>
		/// The left margin of the text area.
		/// </summary>
		private int editLeftMarginWidth = 2;
		/// <summary>
		/// The rectangle for the whole content area.
		/// </summary>
		private Rectangle editContentRect;
		/// <summary>
		/// The rectangle for the indicator margin.
		/// </summary>
		private Rectangle editIndicatorRect;
		/// <summary>
		/// The rectangle for the line number margin.
		/// </summary>
		private Rectangle editLineNumberRect;
		/// <summary>
		/// The rectangle for the selection margin.
		/// </summary>
		private Rectangle editSelectionRect;
		/// <summary>
		/// The rectangle for the text area.
		/// </summary>
		private Rectangle editTextRect;
		/// <summary>
		/// The rectangle for the user margin.
		/// </summary>
		private Rectangle editUserRect;

		/// <summary>
		/// The context menu for the horizontal scrollBar.
		/// </summary>
		private ContextMenu editHScrollBarContextMenu = new ContextMenu();
		/// <summary>
		/// The context menu for the vertical scrollBar.
		/// </summary>
		private ContextMenu editVScrollBarContextMenu = new ContextMenu();

		/// <summary>
		/// The point where the left-mouse-down occured.
		/// </summary>
		private Point editMouseLeftDownPoint = new Point(-1, -1);
		/// <summary>
		/// The point where the right-mouse-up occured.
		/// </summary>
		private Point editMouseRightUpPoint = new Point(-1, -1);
		/// <summary>
		/// The current point of the mouse.
		/// </summary>
		private Point editMouseCurrentPoint = new Point(-1, -1);
		/// <summary>
		/// The item under the mouse pointer.
		/// </summary>
		private string editCurrentItem = string.Empty;
		/// <summary>
		/// A value indicating whether a message is relayed from internal 
		/// controls.
		/// </summary>
		private bool bMessageRelay = false;

		/// <summary>
		/// The line/column location of the viewport.
		/// </summary>
		private EditLocation editViewportLineColumn = new EditLocation();
		/// <summary>
		/// The subline of the current line from which the viewport starts.
		/// </summary>
		private int editViewportFirstLineSubLine = 0;
		/// <summary>
		/// A value indicating whether the value of the horizontal scrollbar 
		/// is changed by code.
		/// </summary>
		private bool bHScrollBarValueInternalChange = false;
		/// <summary>
		/// A value indicating whether the value of the vertical scrollbar 
		/// is changed by code.
		/// </summary>
		private bool bVScrollBarValueInternalChange = false;
		/// <summary>
		/// The location of the viewport in pixels.
		/// </summary>
		private Point editViewportLocation = new Point();
		/// <summary>
		/// A variable determining whether to process any painting request.
		/// </summary>
		private int editPaintFreezeLevel = 0;

		/// <summary>
		/// The x-coordinate of the right margin line.
		/// </summary>
		private int editRightMarginLineX;

		/// <summary>
		/// The context tooltip.
		/// </summary>
		private ToolTip editContextTooltip;
		/// <summary>
		/// The dialog for popup context choices.
		/// </summary>
		private ContextChoice editContextChoice;
		/// <summary>
		/// The dialog for popup context prompts.
		/// </summary>
		private ContextPrompt editContextPrompt;
		/// <summary>
		/// The location where a dialog is popped up.
		/// </summary>
		private EditLocation editPopupLocation = new EditLocation();

		/// <summary>
		/// A helper variable for caret displaying.
		/// </summary>
		private bool bHasCaret = false;

		/// <summary>
		/// A variable indicating the initial panning status.
		/// </summary>
		private bool bPanningInit = false;
		/// <summary>
		/// A variable indicating the panning status.
		/// </summary>
		private bool bPanning = false;
		/// <summary>
		/// The anchor location of panning.
		/// </summary>
		private Point editPanAnchorLocation = new Point();
		/// <summary>
		/// The panning speed.
		/// </summary>
		private int editPanSpeed = 0;
		/// <summary>
		/// The offset of the viewport during panning.
		/// </summary>
		private Point editPanOffset = new Point();
		/// <summary>
		/// The timer for panning.
		/// </summary>
		private System.Timers.Timer editPanTimer = new System.Timers.Timer();

		/// <summary>
		/// A value indicating whether the mouse-down starts a drag-and-drop.
		/// </summary>
		private bool bDragValid = false;

		/// <summary>
		/// The timer for drag-and-drop.
		/// </summary>
		private System.Timers.Timer editDragAndDropTimer 
			= new System.Timers.Timer();

		/// <summary>
		/// The timer for double-click checking.
		/// </summary>
		private System.Timers.Timer editDoubleClickTimer 
			= new System.Timers.Timer();

		/// <summary>
		/// The timer for selecting outside the viewport.
		/// </summary>
		private System.Timers.Timer editSelectingTimer 
			= new System.Timers.Timer();

		#endregion

		#region Imported Methods

		/// <summary>
		/// Creates a new shape for the system caret and assigns ownership of 
		/// the caret to the specified window. The caret shape can be a line, 
		/// a block, or a bitmap.
		/// </summary>
		[DllImport("user32")]
		internal static extern bool CreateCaret(int hwnd, int hBitmap, 
			int nWidth, int nHeight);
		/// <summary>
		/// Moves the caret to the specified coordinates. If the window that 
		/// owns the caret was created with the CS_OWNDC class style, then 
		/// the specified coordinates are subject to the mapping mode of the 
		/// device context associated with that window.
		/// </summary>
		[DllImport("user32")]
		internal static extern bool SetCaretPos(int X, int Y);
		/// <summary>
		/// Makes the caret visible on the screen at the caret's current 
		/// position. When the caret becomes visible, it begins flashing 
		/// automatically.
		/// </summary>
		[DllImport("user32")]
		internal static extern bool ShowCaret(int hWnd);
		/// <summary>
		/// Removes the caret from the screen. Hiding a caret does not destroy
		/// its current shape or invalidate the insertion point.
		/// </summary>
		[DllImport("user32")]
		internal static extern bool HideCaret(int hWnd);
		/// <summary>
		/// Destroys the caret's current shape, frees the caret from the window,
		/// and removes the caret from the screen.
		/// </summary>
		[DllImport("user32")]
		internal static extern bool DestroyCaret();

		#endregion

		#region Methods Related to Construction/Destruction

		/// <summary>
		/// Creates an EditView object associated with the specified EditControl
		/// object.
		/// </summary>
		internal EditView(EditControl edit, bool bShowSplitterButton)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			// TODO: Add any initialization after the InitForm call
			this.edit = edit;
			this.editData = edit.Data;
			this.editSettings = edit.Settings;
			this.TabStop = false;
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			bSplitterButtonVisible = bShowSplitterButton;
			editContentRect = new Rectangle(ClientRectangle.X, 
				ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			editIndicatorRect = new Rectangle(ClientRectangle.X, 
				ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			editLineNumberRect = new Rectangle(ClientRectangle.X, 
				ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			editSelectionRect = new Rectangle(ClientRectangle.X, 
				ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			editTextRect = new Rectangle(ClientRectangle.X, 
				ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			editUserRect = new Rectangle(ClientRectangle.X, 
				ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
			editViewportLineColumn.L = 1;
			editViewportLineColumn.C = 1;
			editHScrollBar.Minimum = 1;
			editHScrollBar.Maximum = 1;
			editHScrollBar.SmallChange = 1;
			editHScrollBar.LargeChange = 1;
			SetHScrollBarValue(1);
			editVScrollBar.Minimum = 1;
			editVScrollBar.Maximum = 1;
			editVScrollBar.SmallChange = 1;
			editVScrollBar.LargeChange = 1;
			SetVScrollBarValue(1);
			editHScrollBar.Cursor = Cursors.Arrow;
			editVScrollBar.Cursor = Cursors.Arrow;
			editCornerArea.Cursor = Cursors.Arrow;
			editSplitterButton.Cursor = Cursors.Arrow;
			editSplitterButton.BackColor = SystemColors.Control;
			editSplitterButton.Height = edit.SplitterHeight + 4;
			Controls.Add(editHScrollBar);
			Controls.Add(editCornerArea);
			Controls.Add(editVScrollBar);
			Controls.Add(editSplitterButton);
			SetupContextMenu();
			editContextTooltip = new ToolTip();
			editContextTooltip.Active = false;
			editContextTooltip.AutomaticDelay = 0;
			editContextTooltip.AutoPopDelay = 30000;
			editContextTooltip.ShowAlways = true;
			editContextTooltip.SetToolTip(this, string.Empty);
			editContextChoice = new ContextChoice(this);
			editContextPrompt = new ContextPrompt(this);
			this.GotFocus += new EventHandler(This_GotFocus);
			this.LostFocus += new EventHandler(This_LostFocus);
			this.SizeChanged += new EventHandler(This_SizeChanged);
			this.MouseDown += new MouseEventHandler(This_MouseDown);
			this.MouseMove += new MouseEventHandler(This_MouseMove);
			this.MouseUp += new MouseEventHandler(This_MouseUp);
			this.MouseWheel += new MouseEventHandler(This_MouseWheel);
			this.KeyDown += new KeyEventHandler(This_KeyDown);
			this.KeyUp += new KeyEventHandler(This_KeyUp);
			this.KeyPress += new KeyPressEventHandler(This_KeyPress);
			this.DragEnter += new DragEventHandler(This_DragEnter);
			this.DragOver += new DragEventHandler(This_DragOver);
			this.DragDrop += new DragEventHandler(This_DragDrop);
			editHScrollBar.GotFocus += new EventHandler(This_GotFocus);
			editVScrollBar.GotFocus += new EventHandler(This_GotFocus);
			editCornerArea.GotFocus += new EventHandler(This_GotFocus);
			editHScrollBar.ValueChanged += 
				new EventHandler(HScrollBar_ValueChanged);
			editVScrollBar.ValueChanged += 
				new EventHandler(VScrollBar_ValueChanged);
			editHScrollBar.MouseUp += new MouseEventHandler(HScrollBar_MouseUp);
			editVScrollBar.MouseUp += new MouseEventHandler(VScrollBar_MouseUp);
			editSplitterButton.MouseDown += 
				new MouseEventHandler(SplitterButton_MouseDown);
			editSplitterButton.MouseEnter += 
				new EventHandler(SplitterButton_MouseEnter);
			editPanTimer.Elapsed += new ElapsedEventHandler(PanTimer_Elapsed);
			editSelectingTimer.Interval = 100;
			editSelectingTimer.Elapsed += 
				new ElapsedEventHandler(SelectingTimer_Elapsed);
			editDragAndDropTimer.Interval = 300;
			editDragAndDropTimer.Elapsed += 
				new ElapsedEventHandler(DragAndDropTimer_Elapsed);
			editDoubleClickTimer.Interval = SystemInformation.DoubleClickTime;
			editDoubleClickTimer.Elapsed += 
				new ElapsedEventHandler(DoubleClickTimer_Elapsed);
			SetupMessageRelay();
			MeasureCharWidth();
		}

		/// <summary>
		/// Sets up the relay for common messages from internal controls to 
		/// EditControl.
		/// </summary>
		protected void SetupMessageRelay()
		{
			editHScrollBar.MouseDown += new MouseEventHandler(Relay_MouseDown);
			editVScrollBar.MouseDown += new MouseEventHandler(Relay_MouseDown);
			editCornerArea.MouseDown += new MouseEventHandler(Relay_MouseDown);
			editSplitterButton.MouseDown += new MouseEventHandler(Relay_MouseDown);
			editHScrollBar.MouseUp += new MouseEventHandler(Relay_MouseUp);
			editVScrollBar.MouseUp += new MouseEventHandler(Relay_MouseUp);
			editCornerArea.MouseUp += new MouseEventHandler(Relay_MouseUp);
			editSplitterButton.MouseUp += new MouseEventHandler(Relay_MouseUp);
			editHScrollBar.MouseMove += new MouseEventHandler(Relay_MouseMove);
			editVScrollBar.MouseMove += new MouseEventHandler(Relay_MouseMove);
			editCornerArea.MouseMove += new MouseEventHandler(Relay_MouseMove);
			editSplitterButton.MouseMove += new MouseEventHandler(Relay_MouseMove);
			editHScrollBar.MouseEnter += new EventHandler(Relay_MouseEnter);
			editVScrollBar.MouseEnter += new EventHandler(Relay_MouseEnter);
			editCornerArea.MouseEnter += new EventHandler(Relay_MouseEnter);
			editSplitterButton.MouseEnter += new EventHandler(Relay_MouseEnter);
			editHScrollBar.MouseHover += new EventHandler(Relay_MouseHover);
			editVScrollBar.MouseHover += new EventHandler(Relay_MouseHover);
			editCornerArea.MouseHover += new EventHandler(Relay_MouseHover);
			editSplitterButton.MouseHover += new EventHandler(Relay_MouseHover);
			editHScrollBar.MouseLeave += new EventHandler(Relay_MouseLeave);
			editVScrollBar.MouseLeave += new EventHandler(Relay_MouseLeave);
			editCornerArea.MouseLeave += new EventHandler(Relay_MouseLeave);
			editSplitterButton.MouseLeave += new EventHandler(Relay_MouseLeave);
			editHScrollBar.MouseWheel += new MouseEventHandler(Relay_MouseWheel);
			editVScrollBar.MouseWheel += new MouseEventHandler(Relay_MouseWheel);
			editCornerArea.MouseWheel += new MouseEventHandler(Relay_MouseWheel);
			editSplitterButton.MouseWheel += new MouseEventHandler(Relay_MouseWheel);
		}

		/// <summary>
		/// Gets the widths for common chars.
		/// </summary>
		internal void MeasureCharWidth()
		{
			if (!edit.CharWidthUpdated)
			{
				this.Font = edit.Font;
				Graphics g = this.CreateGraphics();
				edit.StringFormatNoTab = new StringFormat(StringFormat.GenericTypographic);
				edit.StringFormatNoTab.FormatFlags = 
					StringFormatFlags.MeasureTrailingSpaces;
				PointF ptf = new PointF(0.0f, 0.0f);
				for (int i = 0; i < 256; i++)
				{
					edit.CharWidth[i] = g.MeasureString(((char)i).ToString(), 
						Font, ptf, edit.StringFormatNoTab).Width;
				}
				float tabWidth = g.MeasureString(new string(' ', edit.TabSize), 
					Font, ptf, edit.StringFormatNoTab).Width;
				for (int i = 0; i < edit.TabStops.Length; i++)
				{
					edit.TabStops[i] = tabWidth;
				}
				edit.StringFormatTab = new StringFormat(StringFormat.GenericTypographic);
				edit.StringFormatTab.FormatFlags = 
					StringFormatFlags.MeasureTrailingSpaces;
				edit.StringFormatTab.SetTabStops(0.0f, edit.TabStops);
				edit.CharWidthUpdated = true;
			}
		}

		/// <summary> 
		/// Cleans up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// EditView
			// 
			this.Name = "EditView";

		}
		#endregion

		/// <summary>
		/// Relays the MouseDown event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void Relay_MouseDown(object sender, MouseEventArgs e)
		{
			bMessageRelay = true;
			MouseEventArgs eNew = new MouseEventArgs(e.Button, e.Clicks, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.Delta);
			OnMouseDown(eNew);
			bMessageRelay = false;
		}

		/// <summary>
		/// Relays the MouseUp event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void Relay_MouseUp(object sender, MouseEventArgs e)
		{
			bMessageRelay = true;
			MouseEventArgs eNew = new MouseEventArgs(e.Button, e.Clicks, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.Delta);
			OnMouseUp(eNew);
			bMessageRelay = false;
		}

		/// <summary>
		/// Relays the MouseMove event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void Relay_MouseMove(object sender, MouseEventArgs e)
		{
			bMessageRelay = true;
			MouseEventArgs eNew = new MouseEventArgs(e.Button, e.Clicks, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.Delta);
			OnMouseMove(eNew);
			bMessageRelay = false;
		}

		/// <summary>
		/// Relays the MouseEnter event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Relay_MouseEnter(object sender, EventArgs e)
		{
			bMessageRelay = true;
			OnMouseEnter(e);
			bMessageRelay = false;
		}

		/// <summary>
		/// Relays the MouseHover event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Relay_MouseHover(object sender, EventArgs e)
		{
			bMessageRelay = true;
			OnMouseHover(e);
			bMessageRelay = false;
		}

		/// <summary>
		/// Relays the MouseLeave event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Relay_MouseLeave(object sender, EventArgs e)
		{
			bMessageRelay = true;
			OnMouseLeave(e);
			bMessageRelay = false;
		}

		/// <summary>
		/// Relays the MouseWheel event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		private void Relay_MouseWheel(object sender, MouseEventArgs e)
		{
			bMessageRelay = true;
			MouseEventArgs eNew = new MouseEventArgs(e.Button, e.Clicks, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.Delta);
			OnMouseWheel(eNew);
			bMessageRelay = false;
		}

		/// <summary>
		/// Sets up the context menu for the scrollbars.
		/// </summary>
		private void SetupContextMenu()
		{
			// The context menu items for the horizontal scrollBar.
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("Scroll Here", 
				new EventHandler(HScrollBarMenuItem_Click), Shortcut.None));
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("-"));
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("Left Edge", 
				new EventHandler(HScrollBarMenuItem_Click), Shortcut.None));
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("Right Edge", 
				new EventHandler(HScrollBarMenuItem_Click), Shortcut.None));
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("-"));
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("Page Left", 
				new EventHandler(HScrollBarMenuItem_Click), Shortcut.None));
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("Page Right", 
				new EventHandler(HScrollBarMenuItem_Click), Shortcut.None));
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("-"));
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("Scroll Left", 
				new EventHandler(HScrollBarMenuItem_Click), Shortcut.None));
			editHScrollBarContextMenu.MenuItems.Add(new MenuItem("Scroll Right", 
				new EventHandler(HScrollBarMenuItem_Click), Shortcut.None));
			editHScrollBar.ContextMenu = editHScrollBarContextMenu;
			// The context menu items for the vertical scrollBar.
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("Scroll Here", 
				new EventHandler(VScrollBarMenuItem_Click), Shortcut.None));
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("-"));
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("Top", 
				new EventHandler(VScrollBarMenuItem_Click), Shortcut.None));
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("Bottom", 
				new EventHandler(VScrollBarMenuItem_Click), Shortcut.None));
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("-"));
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("Page Up", 
				new EventHandler(VScrollBarMenuItem_Click), Shortcut.None));
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("Page Down", 
				new EventHandler(VScrollBarMenuItem_Click), Shortcut.None));
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("-"));
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("Scroll Up", 
				new EventHandler(VScrollBarMenuItem_Click), Shortcut.None));
			editVScrollBarContextMenu.MenuItems.Add(new MenuItem("Scroll Down", 
				new EventHandler(VScrollBarMenuItem_Click), Shortcut.None));
			editVScrollBar.ContextMenu = editVScrollBarContextMenu;
			// Empty context menus for the corner area and splitter button.
			editCornerArea.ContextMenu = edit.EmptyContextMenu;
			editSplitterButton.ContextMenu = edit.EmptyContextMenu;
		}

		#endregion

		#region Handlers for Events

		/// <summary>
		/// Handles the DragEnter event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_DragEnter(object sender, DragEventArgs e) 
		{
			if (edit.Updateable && (e.Data.GetDataPresent(DataFormats.Text)
				|| e.Data.GetDataPresent(DataFormats.UnicodeText)))
			{
				if (edit.DragInit)
				{
					e.Effect = ((e.KeyState & 8) == 8) ? DragDropEffects.Copy : 
						DragDropEffects.Move;
				}
				else
				{
					e.Effect = DragDropEffects.Copy;
				}
				Focus();
			}
			else if (e.Data.GetDataPresent(DataFormats.FileDrop) && edit.AllowFileDrop)
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		/// <summary>
		/// Handles the DragOver event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_DragOver(object sender, DragEventArgs e) 
		{
			if (edit.Updateable && (e.Data.GetDataPresent(DataFormats.Text)
				|| e.Data.GetDataPresent(DataFormats.UnicodeText)))
			{
				int lTemp;
				int cTemp;
				Point pt = PointToClient(new Point(e.X, e.Y));
				GetLineChar(pt.X, pt.Y, out lTemp, out cTemp);
				edit.CurrentLine = lTemp;
				edit.CurrentChar = cTemp;
				UpdateCaretPos();
				if (edit.DragInit)
				{
					e.Effect = ((e.KeyState & 8) == 8) ? DragDropEffects.Copy : 
						DragDropEffects.Move;
				}
				else
				{
					e.Effect = DragDropEffects.Copy;
				}
			}
			else if (e.Data.GetDataPresent(DataFormats.FileDrop) && edit.AllowFileDrop)
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		/// <summary>
		/// Handles the DragDrop event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_DragDrop(object sender, DragEventArgs e) 
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop) && edit.AllowFileDrop)
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (edit.SaveModified())
				{
					edit.LoadFile(files[0]);
				}
				return;
			}
			if (edit.Updateable)
			{
				int lTemp;
				int cTemp;
				Point pt = PointToClient(new Point(e.X, e.Y));
				GetLineChar(pt.X, pt.Y, out lTemp, out cTemp);
				if (!bDragValid && (Math.Abs(pt.X - editMouseLeftDownPoint.X) < 
					SystemInformation.DoubleClickSize.Width) 
					&& (Math.Abs(pt.Y - editMouseLeftDownPoint.Y) < 
					SystemInformation.DoubleClickSize.Height))
				{
					edit.DragInit = false;
					if (edit.HasSelection)
					{
						edit.UnSelect();
						return;
					}
				}
				BeginUpdate();
				if (edit.DragInit)
				{
					edit.DragInit = false;
					if (!edit.Selection.Contains(lTemp, cTemp))
					{
						if ((e.KeyState & 8) == 8)
						{
							string strTemp = edit.SelectedText;
							edit.UnSelect();
							EditLocationRange lcr = edit.Insert(strTemp);
							edit.Select(lcr);
						}
						else
						{
							edit.MoveSelection(lTemp, cTemp);
						}
					}
					this.Cursor = Cursors.IBeam;
				}
				else if ((e.Data.GetDataPresent(DataFormats.Text))
					|| (e.Data.GetDataPresent(DataFormats.UnicodeText)))
				{
					edit.Insert(e.Data.GetData(DataFormats.UnicodeText).ToString());
				}
				EndUpdate();
				Redraw();
			}
		}

		/// <summary>
		/// Creates and displays the caret when focus is on the current view.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_GotFocus(object sender, EventArgs e)
		{
			if (!edit.HasContent)
			{
				return;
			}
			edit.ActiveView = this;
			edit.OldActiveView = this;
			CreateEditCaret();
			ShowCaret();
			
			Redraw();

		}

		/// <summary>
		/// Destroys the caret when focus is lost.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_LostFocus(object sender, EventArgs e)
		{
			if ((!editContextChoice.ContainsFocus) 
				&& (!editContextPrompt.ContainsFocus))
			{
				DestroyEditCaret();
			}
			if (bPanningInit)
			{
				bPanningInit = false;
			}
			if (bPanning)
			{
				bPanning = false;
				editPanTimer.Stop();
				PanOffsetY = 0;
			}
			Redraw();
		}

		/// <summary>
		/// Handles the SizeChanged event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_SizeChanged(object sender, EventArgs e)
		{
			UpdateControls();
		}

		/// <summary>
		/// Handles the MouseDown event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void This_MouseDown(object sender, MouseEventArgs e)
		{
			if ((bMessageRelay) || (!edit.HasContent))
			{
				return;
			}
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			edit.OriginalX = -1;
			if (e.Button == MouseButtons.Middle)
			{
				bPanningInit = !bPanning;
				bPanning = false;
				if (bPanningInit)
				{
					edit.PanImage.MakeTransparent(edit.TextBackColor);
					Cursor = Cursors.NoMoveVert;
					this.Capture = true;
					editPanAnchorLocation.X = e.X;
					editPanAnchorLocation.Y = e.Y;
				}
				else
				{
					SetCursor(e.X, e.Y);
				}
				PanOffsetY = 0;
				Redraw();
			} 
			else
			{
				if (bPanningInit || bPanning)
				{
					QuitPanning(e);
					return;
				}
				int lTemp;
				int cTemp;
				GetLineChar(e.X, e.Y, out lTemp, out cTemp);
				if (e.Button == MouseButtons.Right)
				{
					if (edit.HasSelection)
					{
						if (!edit.Selection.Contains(lTemp, cTemp))
						{
							edit.UnSelect();
						}
					}
					else
					{
						edit.CurrentLine = lTemp;
						edit.CurrentChar = cTemp;
						ScrollToViewCaret();
					}
				}
				else if (e.Button == MouseButtons.Left)
				{
					int oldLine = edit.CurrentLine;
					int oldChar = edit.CurrentChar;
					edit.CurrentLine = lTemp;
					edit.CurrentChar = cTemp;
					ScrollToViewCaret();
					if (editDoubleClickTimer.Enabled)
					{
						editDoubleClickTimer.Stop();
						if (ExpandOutlining(edit.CurrentLine, edit.CurrentChar))
						{
							return;
						}
						if (editTextRect.Contains(e.X, e.Y))
						{
							if ((Math.Abs(editMouseLeftDownPoint.X - e.X) 
								< SystemInformation.DoubleClickSize.Width)
								&& (Math.Abs(editMouseLeftDownPoint.Y - e.Y) 
								< SystemInformation.DoubleClickSize.Height))
							{
								if (edit.DoubleClickSelectActive)
								{
									DoubleClickSelectEventArgs dcseArgs = 
										new DoubleClickSelectEventArgs(edit.CurrentLineChar);
									edit.RaiseDoubleClickSelect(dcseArgs);
									if (dcseArgs.LocationRange != null)
									{
										edit.Select(dcseArgs.LocationRange);
									}
								}
								else
								{
									EditLocationRange lcr = edit.
										GetWordLocationRange(edit.CurrentLineChar);
									if (lcr != EditLocationRange.Empty)
									{
										edit.Select(lcr);
									}
								}
								return;
							}
						}
					}
					editMouseLeftDownPoint.X = e.X;
					editMouseLeftDownPoint.Y = e.Y;
					editDoubleClickTimer.Start();
					if (editIndicatorRect.Contains(editMouseLeftDownPoint))
					{
						return;
					}
					else if (editLineNumberRect.Contains(editMouseLeftDownPoint))
					{
						edit.StartSelecting(edit.CurrentLine, 1, true);
						edit.SelectLine(edit.CurrentLine);
					}
					else if (editSelectionRect.Contains(editMouseLeftDownPoint))
					{
						EditOutlining otln = editData.GetLeafOutlining(edit.CurrentLine);
						if (!otln.IsRoot)
						{
							if (edit.CurrentLine == otln.StartLine)
							{
								edit.ToggleOutliningExpansion(edit.CurrentLine);
								return;
							}
						}
						edit.StartSelecting(edit.CurrentLine, 1, true);
						edit.SelectLine(edit.CurrentLine);
					}
					else 
					{
						if(Control.ModifierKeys == Keys.Shift)
						{
							if(!edit.HasSelection)
							{
								edit.Select(oldLine,oldChar,edit.CurrentLine,edit.CurrentChar,true);
							}
							edit.ExtendSelection(edit.CurrentLineChar);
						}
						else
						{
							if (edit.HasSelection)
							{
								if (edit.Selection.Contains(edit.CurrentLineChar)
									&& this.AllowDrop) 
								{
									edit.DragInit = true;
									bDragValid = false;
									editDragAndDropTimer.Start();

									DoDragDrop(edit.SelectedText, 
										DragDropEffects.Copy | DragDropEffects.Move);
								}
								else
								{
									edit.UnSelect();
									edit.StartSelecting(edit.CurrentLineChar, true);
								}
							}
							else
							{
								edit.StartSelecting(edit.CurrentLineChar, true);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Handles the MouseMove event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void This_MouseMove(object sender, MouseEventArgs e)
		{
			if (bMessageRelay)
			{
				return;
			}
			if (!edit.HasContent)
			{
				this.Cursor = Cursors.Arrow;
				return;
			}
			if (bPanningInit)
			{
				bPanningInit = false;
				bPanning = true;
				this.Capture = true;
				editPanTimer.Start();
			}
			if (bPanning)
			{
				if ((e.Y - editPanAnchorLocation.Y) > edit.PanImage.Width/2)
				{
					this.Cursor = Cursors.PanSouth;
					editPanTimer.Interval = Math.Max(1, 250 /
						(e.Y - editPanAnchorLocation.Y - edit.PanImage.Width/2));
					if (editPanTimer.Interval > 50)
					{
						editPanSpeed = 1;
					}
					else if (editPanTimer.Interval > 20)
					{
						editPanSpeed = 2;
					}
					else if (editPanTimer.Interval > 10)
					{
						editPanSpeed = 3;
					}
					else
					{
						editPanSpeed = 5;
					}
				}
				else if ((e.Y - editPanAnchorLocation.Y) < -edit.PanImage.Width/2)
				{
					this.Cursor = Cursors.PanNorth;
					editPanTimer.Interval = Math.Max(1, - 250 /
						(e.Y - editPanAnchorLocation.Y + edit.PanImage.Width/2));
					if (editPanTimer.Interval > 50)
					{
						editPanSpeed = -1;
					}
					else if (editPanTimer.Interval > 20)
					{
						editPanSpeed = -2;
					}
					else if (editPanTimer.Interval > 10)
					{
						editPanSpeed = -3;
					}
					else
					{
						editPanSpeed = -5;
					}
				}
				else
				{
					this.Cursor = Cursors.NoMoveVert;
					editPanSpeed = 0;
				}
			}
			else
			{
				if (e.Button == MouseButtons.Left)
				{
					if (editTextRect.Contains(e.X, e.Y))
					{
						this.Cursor = Cursors.IBeam;
					}
					int lTemp;
					int cTemp;
					GetLineChar(e.X, e.Y, out lTemp, out cTemp);
					if (edit.Selection.IsSelecting)
					{
						edit.CurrentLine = lTemp;
						edit.CurrentChar = cTemp;
						if (edit.Selection.End != edit.CurrentLineChar)
						{
							edit.ExtendSelection(edit.CurrentLineChar);
						}
						editMouseCurrentPoint.X = e.X;
						editMouseCurrentPoint.Y = e.Y;
						ScrollToViewCaret();
						if (editTextRect.Contains(e.X, e.Y))
						{
							editSelectingTimer.Stop();
						}
						else
						{
							editSelectingTimer.Start();
						}
					}
				}
				else if (e.Button == MouseButtons.None)
				{
					if (edit.ISearch)
					{
						this.Cursor = edit.ISearchCursor;
					}
					else if (editIndicatorRect.Contains(e.X, e.Y))
					{
						if (editContextTooltip.Active)
						{
							editContextTooltip.Active = false;
						}
						this.Cursor = Cursors.Arrow;
					}
					else if (editLineNumberRect.Contains(e.X, e.Y)) 
					{
						if (editContextTooltip.Active)
						{
							editContextTooltip.Active = false;
						}
						this.Cursor = edit.NECursor;
					}
					else if (editSelectionRect.Contains(e.X, e.Y))
					{
						if (editContextTooltip.Active)
						{
							editContextTooltip.Active = false;
						}
						int ln = GetLineFromY(e.Y);
						EditOutlining otln = editData.GetLeafOutlining(ln);
						if ((!otln.IsRoot) && (ln == otln.StartLine))
						{
							this.Cursor = Cursors.Arrow;
						}
						else
						{
							this.Cursor = edit.NECursor;
						}
					}
					else if (editTextRect.Contains(e.X, e.Y))
					{
						int lTemp;
						int cTemp;
						GetLineChar(e.X, e.Y, out lTemp, out cTemp);
						EditOutlining otln = 
							editData.GetCollapsedOutlining(lTemp, cTemp);
						if (otln != null)
						{
							this.Cursor = Cursors.IBeam;
							if (!editContextTooltip.Active)
							{
								editContextTooltip.Active = true;
								editContextTooltip.SetToolTip(this, 
									editData.GetOutliningTooltips(otln));
							}
							return;
						}
						if (edit.Selection.Contains(lTemp, cTemp))
						{
							if (editContextTooltip.Active)
							{
								editContextTooltip.Active = false;
							}
							this.Cursor = Cursors.Arrow;
						}
						else
						{
							this.Cursor = Cursors.IBeam;
							if (edit.ContextTooltipPopupActive)
							{
								string item = 
									editData.GetIdentifierWord(lTemp, cTemp);
								if (item != string.Empty)
								{
									ContextTooltipPopupEventArgs ctpeArgs = 
										new ContextTooltipPopupEventArgs(item, 
										new EditLocation(lTemp, cTemp));
									edit.RaiseContextTooltipPopup(ctpeArgs);
									if ((ctpeArgs.ItemTooltip != null) && 
										(ctpeArgs.ItemTooltip != string.Empty))
									{
										if (item != editCurrentItem)
										{
											if (!editContextTooltip.Active)
											{
												editContextTooltip.Active = true;
											}
											editContextTooltip.SetToolTip(this, 
												ctpeArgs.ItemTooltip);
											editCurrentItem = item;
										}
										return;
									}
								}
								editCurrentItem = string.Empty;
							}
							if (editContextTooltip.Active)
							{
								editContextTooltip.Active = false;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Handles the MouseUp event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void This_MouseUp(object sender, MouseEventArgs e)
		{
			if ((bMessageRelay) || (!edit.HasContent))
			{
				return;
			}
			editSelectingTimer.Stop();
			if (e.Button == MouseButtons.Middle)
			{
				if (bPanning)
				{
					this.Capture = false;
					bPanningInit = false;
					bPanning = false;
					PanOffsetY = 0;
					Redraw();
					return;
				}
				if (bPanningInit)
				{
					bPanningInit = false;
					bPanning = true;
					this.Capture = true;
					editPanTimer.Start();
					return;
				}
			}
			if (bPanningInit || bPanning)
			{
				QuitPanning(e);
				return;
			}
			if (editIndicatorRect.Contains(editMouseLeftDownPoint)
				&& (editMouseLeftDownPoint == new Point(e.X, e.Y)))
			{
				//				edit.ToggleBreakpoint(GetLineFromY(editMouseLeftDownPoint.Y));
				//				edit.RedrawLine(GetLineFromY(editMouseLeftDownPoint.Y));
			}
			else
			{
				if (e.Button == MouseButtons.Left)
				{
					if (edit.Selection.IsSelecting)
					{
						edit.Selection.StopSelecting();
						if (!edit.Selection.HasSelection)
						{
							edit.Selection.UnSelect();
							if (editTextRect.Contains(editMouseLeftDownPoint))
							{
								EditOutlining otln = editData.
									GetLeafOutlining(edit.CurrentLine, 
									edit.CurrentChar);
								if ((otln != editData.OutliningRoot) 
									&& (otln.Collapsed))
								{
									edit.Select(otln);
								}
							}
						}
					}
					UpdateCaretPos();
				}
			}
		}

		/// <summary>
		/// Handles the MouseWheel event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void This_MouseWheel(object sender, MouseEventArgs e)
		{
			if ((bMessageRelay) || (!edit.HasContent))
			{
				return;
			}
			if (bPanningInit || bPanning)
			{
				QuitPanning(e);
			}
			else
			{
				Scroll((e.Delta > 0) ? -SystemInformation.MouseWheelScrollLines :
					SystemInformation.MouseWheelScrollLines);
			}
		}

		/// <summary>
		/// Determines whether the specified key is a regular input key or 
		/// a special key that requires preprocessing.
		/// </summary>
		/// <param name="keyData">One of the Keys values.</param>
		/// <returns>true if the specified key is a regular input key; otherwise, 
		/// false.</returns>
		protected override bool IsInputKey(Keys keyData)
		{
			switch(keyData & Keys.KeyCode)
			{
				case Keys.Enter:
					return edit.AcceptsReturn;
				case Keys.Tab:
					return false;
				case Keys.Up:
				case Keys.Down:
				case Keys.Left:
				case Keys.Right:
				case Keys.Home:
				case Keys.End:
				case Keys.Insert:
				case Keys.Delete:
					return true;
				default:
					break;
			}
			return base.IsInputKey(keyData);
		}

		/// <summary>
		/// Determines if a character is an input character that the control 
		/// recognizes.
		/// </summary>
		/// <param name="charCode">The character to test.</param>
		/// <returns>true if the character should be sent directly to the 
		/// control and not preprocessed; otherwise, false.</returns>
		protected override bool IsInputChar(char charCode)
		{
			if ((charCode == '\r') && (!edit.AcceptsReturn))
			{
				return false;
			}
			else if ((charCode == '\t') && (!edit.AcceptsTab))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Processes a dialog key.
		/// </summary>
		/// <param name="keyData">One of the Keys values that represents the key 
		/// to process.</param>
		/// <returns>true if the key was processed by the control; otherwise, 
		/// false.</returns>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (editContextTooltip.Active)
			{
				editContextTooltip.Active = false;
			}
			if (keyData == Keys.Tab) 
			{
				if (!edit.AcceptsTab)
				{
					return base.ProcessDialogKey(keyData);
				}
				else
				{
					edit.TabKey();
					return true;
				}
			}
			else if (keyData == (Keys.Tab | Keys.Shift)) 
			{ 
				if (!edit.AcceptsTab)
				{
					return base.ProcessDialogKey(keyData);
				}
				else
				{
					edit.ShiftTabKey();
					return true;
				}
			}
			else if (keyData == Keys.Up)
			{
				UpArrowKey();
				return true;
			}
			else if (keyData == Keys.Down)
			{
				DownArrowKey();
				return true;
			}
			else if	(keyData == Keys.Left)
			{
				LeftArrowKey();
				return true;
			}
			else if (keyData == Keys.Right)
			{
				RightArrowKey();
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Shift | Keys.Up))
			{
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Shift | Keys.Down))
			{
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Shift | Keys.Left))
			{
				WordLeftExtend();
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Shift | Keys.Right))
			{
				WordRightExtend();
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Up))
			{
				ScrollUpOneLine();
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Down))
			{
				ScrollDownOneLine();
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Left))
			{
				WordLeft();
				return true;
			}
			else if (keyData == (Keys.Control | Keys.Right))
			{
				WordRight();
				return true;
			}			
			else if (keyData == (Keys.Shift | Keys.Up))
			{
				LineUpExtend();
				return true;
			}
			else if (keyData == (Keys.Shift | Keys.Down))
			{
				LineDownExtend();
				return true;
			}
			else if (keyData == (Keys.Shift | Keys.Left))
			{
				CharLeftExtend();
				return true;
			}
			else if (keyData == (Keys.Shift | Keys.Right))
			{
				CharRightExtend();
				return true;
			}
			else
			{
				return base.ProcessDialogKey(keyData);
			}
		}

		/// <summary>
		/// Handles the KeyDown event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyEventArgs that contains the event data.</param>
		private void This_KeyDown(object sender, KeyEventArgs e)
		{
			edit.RaiseKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			if (editContextTooltip.Active)
			{
				editContextTooltip.Active = false;
			}
			if (!edit.HasContent)
			{
				return;
			}
			if (e.Alt)
			{
				return;
			}
			e.Handled = true;
			switch (e.KeyCode)
			{
				case Keys.Insert:
					if (e.Control)
					{
						edit.Copy();
					}
					else if (e.Shift)
					{
						edit.Paste();
					}
					else
					{
						edit.InsertMode = !edit.InsertMode;
					}
					break;
				case Keys.Home:
					if ((e.Shift) && (e.Control))
					{
						DocumentStartExtend();
					}
					else if (e.Shift)
					{
						LineStartExtend();
					}
					else if (e.Control)
					{
						DocumentStart();
					} 
					else
					{
						HomeKey();
					}
					break;
				case Keys.End:
					if ((e.Shift) && (e.Control))
					{
						DocumentEndExtend();
					}
					else if (e.Shift)
					{
						LineEndExtend();
					}
					else if (e.Control)
					{
						DocumentEnd();
					} 
					else
					{
						EndKey();
					}
					break;
				case Keys.PageUp:
					if ((e.Shift) && (e.Control))
					{
						PageTopExtend();
					}
					else if (e.Shift)
					{
						PageUpExtend();
					}
					else if (e.Control)
					{
						PageTop();
					}
					else
					{
						PageUp();
					}
					break;
				case Keys.Right:
				case Keys.Left:
				case Keys.Up:
				case Keys.Down:
					ProcessDialogKey(e.KeyData);
					break;
				case Keys.PageDown:
					if ((e.Shift) && (e.Control))
					{
						PageBottomExtend();
					}
					else if (e.Shift)
					{
						PageDownExtend();
					}
					else if (e.Control)
					{
						PageBottom();
					}
					else
					{
						PageDown();
					}
					break;
				case Keys.Delete:
					edit.DeleteKey();
					break;
				case Keys.F3:
					if (e.Control)
					{
						string strTemp;
						if (edit.HasSelection)
						{
							strTemp = edit.SelectedText;
							if (strTemp.IndexOf("\n") != -1)
							{
								strTemp = edit.GetWord(edit.Selection.Start);
							}
						}
						else
						{
							strTemp = edit.GetCurrentWord();
						}
						edit.FRDlg.AddToSearchedList(strTemp);
						edit.FRDlg.TextSearched = strTemp;
						FindNext(strTemp, false);
					}
					else
					{
						if (edit.FRDlg.TextSearched != string.Empty)
						{
							FindNext(edit.FRDlg.TextSearched, false);
						}
						else
						{
							FindAndReplace(false);
						}
					}
					break;
				default:
					e.Handled = false;
					break;
			}
		}

		/// <summary>
		/// Handles the KeyUp event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyEventArgs that contains the event data.</param>
		private void This_KeyUp(object sender, KeyEventArgs e)
		{
			edit.RaiseKeyUp(e);
		}

		/// <summary>
		/// Handles the KeyPress event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyPressEventArgs that contains the event data.
		/// </param>
		private void This_KeyPress(object sender, KeyPressEventArgs e)
		{
			edit.RaiseKeyPress(e);
			if (e.Handled)
			{
				return;
			}
			if (editContextTooltip.Active)
			{
				editContextTooltip.Active = false;
			}
			if (!edit.Updateable)
			{
				return;
			}
			edit.OriginalX = -1;
			switch(e.KeyChar)
			{
				case '\b':
					edit.BackSpaceKey();
					break;
				case '\r':
					if (!edit.AcceptsReturn)
					{
						return;
					}
					edit.EnterKey();
					break;
				case (char)27:
					edit.EscapeKey();
					break;
				default:
					if (e.KeyChar >= 32)
					{
						if (edit.ISearch)
						{
							edit.ISearchString += e.KeyChar.ToString();
							ISearchNext(edit.ISearchString);
						}
						else
						{
							edit.HandleKeyInput(e.KeyChar);
							KeyProcessEx(e.KeyChar);
						}
					}
					break;
			}
			e.Handled = true;
		}

		internal bool IsContextChoiceChar(char ch)
		{
			IEnumerator choices = edit.ContextChoiceChar.GetEnumerator();
			
			while(choices.MoveNext())
			{
				if((char)choices.Current == ch)
					return true;

				//choices.MoveNext();
			}
			return false;
		}
		/// <summary>
		/// Provides some additional processing of the specified character.
		/// </summary>
		/// <param name="ch">The character to be processed.</param>
		private void KeyProcessEx(char ch)
		{
			if (edit.Updateable)
			{
				if ((edit.ContextChoicePopupActive) && 
					IsContextChoiceChar(ch))
				{
					if ((!editContextChoice.Visible) && 
						(edit.CurrentChar > 1))
					{
						string item = editData.
							GetIdentifierWord(edit.CurrentLine, 
							edit.CurrentChar - 2);
						//						if (item != string.Empty)
						//						{
						ContextChoicePopupEventArgs ccpeArgs = 
							new ContextChoicePopupEventArgs(ch.ToString(), 
							item, edit.CurrentLineChar);
						edit.RaiseContextChoicePopup(ccpeArgs);
						if (ccpeArgs.Choices != null)
						{
							int xTemp;
							int yTemp;
							GetPoint(edit.CurrentLineChar, out xTemp, out yTemp);
							editContextChoice.Location = PointToScreen(
								new Point(xTemp - 10, yTemp + edit.LineHeight));
							editContextChoice.ItemFont = ccpeArgs.ItemFont;
							editContextChoice.ItemsPerPage = ccpeArgs.ItemsPerPage;
							editContextChoice.ItemImageList = ccpeArgs.ItemImageList;
							editContextChoice.ItemList = ccpeArgs.Choices;
							editContextChoice.ItemImageIndexList = ccpeArgs.ImageIndexes;
							editContextChoice.Show();
							editPopupLocation.L = edit.CurrentLine; 
							editPopupLocation.C = edit.CurrentChar; 
						}
						//						}
					}
					else
					{
						editContextChoice.Hide();
					}
				}
				if ((edit.ContextPromptPopupActive) &&
					(ch == edit.ContextPromptBeginChar))
				{ 
					if ((!editContextPrompt.Visible) &&
						(edit.CurrentChar > 2))
					{
						string item = editData.
							GetIdentifierWord(edit.CurrentLine, 
							edit.CurrentChar - 2);
						if (item != string.Empty)
						{
							ContextPromptPopupEventArgs cppeArgs = 
								new ContextPromptPopupEventArgs(item, 
								edit.CurrentLineChar);
							edit.RaiseContextPromptPopup(cppeArgs);
							if (cppeArgs.Prompts != null)
							{
								int xTemp;
								int yTemp;
								GetPoint(edit.CurrentLineChar, 
									out xTemp, out yTemp);
								editContextPrompt.Location = PointToScreen(
									new Point(xTemp - 10, yTemp + edit.LineHeight));
								editContextPrompt.SetPrompts(cppeArgs.Prompts);
								editContextPrompt.Show();
								editPopupLocation.L = edit.CurrentLine; 
								editPopupLocation.C = edit.CurrentChar; 
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Handles the KeyPress event of the popup ContextChoice dialog.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyPressEventArgs that contains the event data.
		/// </param>
		internal void KeyPressWithContextChoice(object sender, KeyPressEventArgs e)
		{
			string currentWord = edit.GetCurrentWord();
			bool bIsWordStartAContextChar = (currentWord.Length>0 &&
				!IsContextChoiceChar(currentWord[0]));

			if (e.KeyChar == (char)27)
			{
				editContextChoice.Hide();
			}
			else if (IsContextChoiceChar(e.KeyChar))
			{
				if (CurrentChoiceHasText())
				{
					
					DeleteCurrentWordIf(bIsWordStartAContextChar);
					edit.Insert(editContextChoice.ListBoxChoices.Text);
				}
				edit.Insert(e.KeyChar.ToString());
				editContextChoice.Hide();
			}
			else if (e.KeyChar == '\r')
			{
				if (CurrentChoiceHasText() )
				{
					DeleteCurrentWordIf(bIsWordStartAContextChar);
					edit.Insert(editContextChoice.ListBoxChoices.Text);
					editContextChoice.Hide();
				}
				else //choice text was empty
				{
					edit.EnterKey();
					editContextChoice.Hide();
				}
			}
			else if (e.KeyChar == '\b')
			{
				edit.BackSpaceKey();
				if (edit.CurrentLineChar < editPopupLocation)
				{
					editContextChoice.Hide();
				}
				else
				{
					int index = editContextChoice.ListBoxChoices.
						FindString(edit.GetCurrentWord());
					editContextChoice.ListBoxChoices.SelectedIndex = index;
				}
			}
			else if (e.KeyChar == ' ')
			{
				edit.HandleKeyInput(e.KeyChar);
				editContextChoice.Hide();
			}
			else if (e.KeyChar == '\t')
			{
				if (CurrentChoiceHasText())
				{
					DeleteCurrentWordIf(bIsWordStartAContextChar);
					edit.Insert(editContextChoice.ListBoxChoices.Text);
					editContextChoice.Hide();
				}
				else
				{
					DeleteCurrentWordIf(bIsWordStartAContextChar);
					edit.Insert(editContextChoice.ListBoxChoices.Items[0].ToString());
					editContextChoice.Hide();
				}
			}
			else
			{
				edit.HandleKeyInput(e.KeyChar);
				int index = editContextChoice.ListBoxChoices.
					FindString(edit.GetCurrentWord());
				editContextChoice.ListBoxChoices.SelectedIndex = index;
			}
		}

		private void DeleteCurrentWordIf(bool ShouldDelete)
		{
			if(ShouldDelete)
			{
				edit.DeleteCurrentWord();
			}
		}

		private bool CurrentChoiceHasText()
		{
			return editContextChoice.ListBoxChoices.Text != null && 
				editContextChoice.ListBoxChoices.Text!=string.Empty &&
				editContextChoice.ListBoxChoices.SelectedIndex >-1;
		}

		/// <summary>
		/// Handles the KeyPress event of the ContextPrompt dialog.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyPressEventArgs that contains the event data.
		/// </param>
		internal void KeyPressWithContextPrompt(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '\b')
			{
				edit.BackSpaceKey();
				if (edit.CurrentLineChar < editPopupLocation)
				{
					editContextPrompt.Hide();
				}
			}
			else if (e.KeyChar == '\r')
			{
				edit.EnterKey();
				editContextPrompt.Hide();
			}
			else if (e.KeyChar == (char)27)
			{
				editContextPrompt.Hide();
			}
			else
			{
				edit.HandleKeyInput(e.KeyChar);
				if (e.KeyChar == edit.ContextPromptEndChar)
				{
					editContextPrompt.Hide();
				}
			}
		}

		/// <summary>
		/// Handles the MouseDown event of the splitter button.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void SplitterButton_MouseDown(object sender, MouseEventArgs e)
		{
			if ((edit.HasContent) && (e.Button == MouseButtons.Left))
			{
				SplitterButtonVisible = false;
				edit.DisplaySplitter();
			}
		}

		/// <summary>
		/// Handles the MouseOver event of the splitter button.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void SplitterButton_MouseEnter(object sender, EventArgs e)
		{
			if (edit.HasContent)
			{
				if (editSplitterButton.Cursor != Cursors.HSplit)
				{
					editSplitterButton.Cursor = Cursors.HSplit;
				}
			}
			else
			{
				if (editSplitterButton.Cursor != Cursors.Arrow)
				{
					editSplitterButton.Cursor = Cursors.Arrow;
				}
			}
		}

		/// <summary>
		/// Handles the ValueChanged event of the horizontal scrollBar.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void HScrollBar_ValueChanged(Object sender, EventArgs e)
		{
			if (!bHScrollBarValueInternalChange)
			{
				if (editViewportLineColumn.C == editHScrollBar.Value)
				{
					return;
				}
				editViewportLineColumn.C = Math.Max(1, editHScrollBar.Value);
				UpdateViewportX();
				Redraw();
			}
		}

		/// <summary>
		/// Handles the ValueChanged event of the vertical scrollBar.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void VScrollBar_ValueChanged(Object sender, EventArgs e)
		{
			if (!bVScrollBarValueInternalChange)
			{
				int lnTemp = editData.GetStoredLineIndex(editVScrollBar.Value);
				if (editViewportLineColumn.L != lnTemp)
				{
					editViewportLineColumn.L = Math.Max(1, lnTemp);
					Redraw();
				}
			}
		}

		/// <summary>
		/// Handles the MouseUp event of the horizontal scrollbar.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void HScrollBar_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				editMouseRightUpPoint.X = e.X;
				editMouseRightUpPoint.Y = e.Y;
			}
		}

		/// <summary>
		/// Handles the MouseUp event of the vertical scrollbar.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.
		/// </param>
		private void VScrollBar_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				editMouseRightUpPoint.X = e.X;
				editMouseRightUpPoint.Y = e.Y;
			}
		}

		/// <summary>
		/// Handles the Click event of the horizontal scrollBar context menu 
		/// items.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void HScrollBarMenuItem_Click(Object sender, EventArgs e)
		{
			string itemText = ((MenuItem)sender).Text.Replace("&", string.Empty);
			switch (itemText)
			{
				case "Scroll Here":
				{
					editHScrollBar.ScrollHere(editMouseRightUpPoint.X 
						- editHScrollBar.Location.X, 
						editMouseRightUpPoint.Y - editHScrollBar.Location.Y);
					editMouseRightUpPoint.X = -1;
					editMouseRightUpPoint.Y = -1;
					break;
				}
				case "Left Edge":
				{
					ScrollToLeftEdge();
					break;
				}
				case "Right Edge":
				{
					ScrollToRightEdge();
					break;
				}
				case "Page Left":
				{
					ScrollLeftOnePage();
					break;
				}
				case "Page Right":
				{
					ScrollRightOnePage();
					break;
				}
				case "Scroll Left":
				{
					ScrollLeftOneColumn();
					break;
				}
				case "Scroll Right":
				{
					ScrollRightOneColumn();
					break;
				}
				default:
				{
					break;
				}
			}
		}

		/// <summary>
		/// Handles the Click event of the vertical scrollBar context menu items.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void VScrollBarMenuItem_Click(Object sender, EventArgs e)
		{
			string itemText = ((MenuItem)sender).Text.Replace("&", string.Empty);
			switch (itemText)
			{
				case "Scroll Here":
				{
					editVScrollBar.ScrollHere(editMouseRightUpPoint.X 
						- editVScrollBar.Location.X, 
						editMouseRightUpPoint.Y - editVScrollBar.Location.Y);
					editMouseRightUpPoint.X = -1;
					editMouseRightUpPoint.Y = -1;
					break;
				}
				case "Top":
				{
					ScrollToFirstLine();
					break;
				}
				case "Bottom":
				{
					ScrollToLastLine();
					break;
				}
				case "Page Up":
				{
					ScrollUpOnePage();
					break;
				}
				case "Page Down":
				{
					ScrollDownOnePage();
					break;
				}
				case "Scroll Up":
				{
					ScrollUpOneLine();
					break;
				}
				case "Scroll Down":
				{
					ScrollDownOneLine();
					break;
				}
				default:
				{
					break;
				}
			}
		}

		/// <summary>
		/// Handles the Elapsed event of the panning timer.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An ElapsedEventArgs that contains the event data.
		/// </param>
		private void PanTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (!bPanning)
			{
				editPanTimer.Stop();
				PanOffsetY = 0;
			}
			else
			{
				if (((ViewportFirstLine >= MaxViewportFirstLine) 
					&& (editPanSpeed > 0))
					||((ViewportFirstLine <= 1) && (editPanSpeed < 0)))
				{
					PanOffsetY = 0;
				}
				else
				{
					PanOffsetY += editPanSpeed;
					if (PanOffsetY >= edit.LineHeight)
					{
						if (ViewportFirstLine < MaxViewportFirstLine)
						{
							PanOffsetY -= edit.LineHeight;
							ViewportFirstLine = editData.
								GetVisibleLineByOffset(ViewportFirstLine, 1);
							return;
						}
					}
					else if (PanOffsetY <= - edit.LineHeight)
					{
						if (ViewportFirstLine > 1)
						{
							PanOffsetY += edit.LineHeight;
							ViewportFirstLine = editData.
								GetVisibleLineByOffset(ViewportFirstLine, -1);
							return;
						}
					}
				}
			}
			Redraw();
		}

		/// <summary>
		/// Handles the Elapsed event of the dragging timer.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An ElapsedEventArgs that contains the event data.
		/// </param>
		private void DragAndDropTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			editDragAndDropTimer.Stop();
			if (edit.DragInit)
			{
				bDragValid = true;
				Cursor = edit.DragMoveCursor;
			}
		}

		/// <summary>
		/// Handles the Elapsed event of the double-click checking timer.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An ElapsedEventArgs that contains the event data.
		/// </param>
		private void DoubleClickTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			editDoubleClickTimer.Stop();
		}

		/// <summary>
		/// Handles the Elapsed event of the selecting timer.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An ElapsedEventArgs that contains the event data.
		/// </param>
		private void SelectingTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			int lTemp;
			int cTemp;
			GetLineChar(editMouseCurrentPoint.X, editMouseCurrentPoint.Y, 
				out lTemp, out cTemp);
			edit.CurrentLine = lTemp;
			edit.CurrentChar = cTemp;
			if (edit.Selection.End != edit.CurrentLineChar)
			{
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			ScrollToViewCaret();
		}

		#endregion

		#region Methods Related to Caret Handling

		/// <summary>
		/// Updates the caret position and shows the caret.
		/// </summary>
		internal void ShowCaret()
		{
			if (this == edit.ActiveView)
			{
				UpdateCaretPos();
			}
			ShowCaret(this.Handle.ToInt32());
		}

		/// <summary>
		/// Hides the caret.
		/// </summary>
		internal void HideCaret()
		{
			HideCaret(this.Handle.ToInt32());
		}

		/// <summary>
		/// Updates the caret position.
		/// </summary>
		internal void UpdateCaretPos()
		{
			if (editPaintFreezeLevel > 0)
			{
				return;
			}
			if (bHasCaret)
			{
				int xTemp;
				int yTemp;
				GetPoint(edit.CurrentLineChar, out xTemp, out yTemp);
				xTemp = ((xTemp >= editTextRect.X) && 
					(xTemp < editTextRect.Right)) ? xTemp : -1;
				SetCaretPos(xTemp, yTemp);
			}
		}

		/// <summary>
		/// Updates the caret size.
		/// </summary>
		internal void UpdateCaretSize()
		{
			DestroyEditCaret();
			CreateEditCaret();
			ShowCaret();
		}

		/// <summary>
		/// Creates the caret for the view.
		/// </summary>
		internal void CreateEditCaret()
		{
			if (edit.InsertMode)
			{
				CreateCaret(this.Handle.ToInt32(), 0, edit.CaretWidth, Font.Height);
			}
			else 
			{
				if (edit.HasSelection)
				{
					CreateCaret(this.Handle.ToInt32(), 0, edit.CaretWidth, Font.Height);
				}
				else
				{
					int width = 0;
					string strLine = editData.GetStringObject(edit.CurrentLine);
					char ch;
					ch = (edit.CurrentChar >= strLine.Length + 1) ? ' ' : 
						strLine[edit.CurrentChar-1];
					if (ch < 256)
					{
						if (ch != '\t')
						{
							width = (int)edit.CharWidth[ch];
						}
						else
						{
							if (edit.CurrentChar == 1)
							{
								width = (int)edit.CharWidth[' '] * edit.TabSize;
							}
							else
							{
								Graphics g = this.CreateGraphics();
								float fStart = GetStringWidth(g, 
									strLine.Substring(0, edit.CurrentChar - 1));
								float fEnd = GetStringWidth(g, 
									strLine.Substring(0, edit.CurrentChar)); 
								width = (int)(fEnd - fStart);
							}
						}
					}
					else
					{
						Graphics g = this.CreateGraphics();
						width = (int) g.MeasureString(ch.ToString(), 
							Font, editViewportLocation, 
							edit.StringFormatNoTab).Width;
					}
					CreateCaret(this.Handle.ToInt32(), 0, width, Font.Height);
				}
			}
			bHasCaret = true;
		}

		/// <summary>
		/// Destroys the caret for the view.
		/// </summary>
		internal void DestroyEditCaret()
		{
			DestroyCaret();
			bHasCaret = false;
		}

		#endregion

		#region Methods Related to Popup Dialogs

		
		/// <summary>
		/// Invokes the Find and Replace dialog.
		/// </summary>
		internal void FindAndReplace(bool bReplace)
		{
			string strTemp;
			if (edit.HasSelection)
			{
				strTemp = edit.SelectedText;
				if (strTemp.IndexOf("\n") != -1)
				{
					strTemp = edit.GetWord(edit.Selection.Start);
				}
			}
			else
			{
				strTemp = edit.GetCurrentWord();
			}
			edit.FRDlg.SetState(this, strTemp, bReplace);
			edit.FRDlg.Owner = edit.ParentForm;
			edit.FRDlg.Show();
		}

		/// <summary>
		/// Invokes the GoTo dialog.
		/// </summary>
		internal void GoTo()
		{
			GoToDlg dlg = new GoToDlg();
			dlg.SetDialogItemText(edit);
			dlg.Label = edit.GetResourceString("DialogItemLineNumber") 
				+ " (1 - " + edit.LineCount + "):";
			dlg.LineNumber = edit.CurrentLine;
			dlg.ShowDialog(this);
			if (dlg.DialogResult == DialogResult.OK)
			{
				if (dlg.LineNumber != -1)
				{
					GoToLine(dlg.LineNumber);
				}
			}
		}

		#endregion

		#region Methods Related to Scrollbar Handling

		/// <summary>
		/// Updates the horizontal scrollBar.
		/// </summary>
		internal void UpdateHScrollBar()
		{
			UpdateHScrollBarRange();
			UpdateHScrollBarValue();
		}

		/// <summary>
		/// Updates the range of horizontal scrollBar.
		/// </summary>
		internal void UpdateHScrollBarRange()
		{
			if (edit.HasContent)
			{
				int vcc = ViewportColumnCount;
				editHScrollBar.Minimum = 1;
				editHScrollBar.Maximum = Math.Max(edit.MaxColumn, vcc);
				edit.MaxColumn = editHScrollBar.Maximum;
				if (vcc > 0)
				{
					editHScrollBar.LargeChange = vcc;
				}
			}
			else
			{
				editHScrollBar.Minimum = 1;
				editHScrollBar.Maximum = 1;
				editHScrollBar.LargeChange = 1;
			}
		}

		/// <summary>
		/// Updates the value of the horizontal scrollBar.
		/// </summary>
		internal void UpdateHScrollBarValue()
		{
			if (edit.HasContent)
			{
				if (editViewportLineColumn.C < editHScrollBar.Minimum)
				{
					SetHScrollBarValue(editHScrollBar.Minimum);
				}
				else if (editViewportLineColumn.C > editHScrollBar.Maximum)
				{
					SetHScrollBarValue(editHScrollBar.Maximum);
				}
				else
				{
					SetHScrollBarValue(editViewportLineColumn.C);
				}
			}
		}

		/// <summary>
		/// Updates the vertical scrollBar.
		/// </summary>
		internal void UpdateVScrollBar()
		{
			UpdateVScrollBarRange();
			UpdateVScrollBarValue();
		}

		/// <summary>
		/// Updates the range of the vertical scrollBar.
		/// </summary>
		internal void UpdateVScrollBarRange()
		{
			if (edit.HasContent)
			{
				int vlc = ViewportLineCount;
				editVScrollBar.Minimum = 1;
				editVScrollBar.Maximum = Math.Max(edit.VisibleLineCount, vlc);
				if (vlc > 0)
				{
					editVScrollBar.LargeChange = vlc;
				}
			}
			else
			{
				editVScrollBar.Minimum = 1;
				editVScrollBar.Maximum = 1;
				editVScrollBar.LargeChange = 1;
			}
		}

		/// <summary>
		/// Updates the value of the vertical scrollBar.
		/// </summary>
		internal void UpdateVScrollBarValue()
		{
			if (edit.HasContent)
			{
				int lnTemp = editData.GetOccupiedLineIndex(editViewportLineColumn.L);
				if (lnTemp < editVScrollBar.Minimum)
				{
					SetVScrollBarValue(editVScrollBar.Minimum);
				}
				else if (lnTemp > editVScrollBar.Maximum)
				{
					SetVScrollBarValue(editVScrollBar.Maximum);
				}
				else
				{
					SetVScrollBarValue(lnTemp);
				}
			}
		}

		/// <summary>
		/// Sets the value of the horizontal scrollBar.
		/// </summary>
		private void SetHScrollBarValue(int newValue)
		{
			bHScrollBarValueInternalChange = true;
			editHScrollBar.Value = newValue;
			bHScrollBarValueInternalChange = false;
		}

		/// <summary>
		/// Sets the value of the vertical scrollBar.
		/// </summary>
		private void SetVScrollBarValue(int newValue)
		{
			bVScrollBarValueInternalChange = true;
			editVScrollBar.Value = newValue;
			bVScrollBarValueInternalChange = false;
		}

		#endregion

		#region Methods Related to Editing

		/// <summary>
		/// Quits the panning state.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		private void QuitPanning(MouseEventArgs e)
		{
			this.Capture = false;
			bPanningInit = false;
			bPanning = false;
			PanOffsetY = 0;
			SetCursor(e.X, e.Y);
			Redraw();
		}

		/// <summary>
		/// Sets the cursor of the view based on the mouse location.
		/// </summary>
		/// <param name="X">The x-coordinate of the mouse pointer.</param>
		/// <param name="Y">The y-coordinate of the mouse pointer.</param>
		private void SetCursor(int X, int Y)
		{
			if (editTextRect.Contains(X, Y))
			{
				this.Cursor = Cursors.IBeam;
			}
			else if (editLineNumberRect.Contains(X, Y))
			{
				this.Cursor = edit.NECursor;
			}
			else if (editSelectionRect.Contains(X, Y))
			{
				int lTemp;
				int cTemp;
				Point pt = PointToClient(new Point(X, Y));
				GetLineChar(pt.X, pt.Y, out lTemp, out cTemp);
				EditOutlining otln = editData.GetLeafOutlining(lTemp);
				if ((!otln.IsRoot) && (lTemp == otln.StartLine))
				{
					this.Cursor = Cursors.Arrow;
				}
				else
				{
					this.Cursor = edit.NECursor;
				}
			}
			else
			{
				this.Cursor = Cursors.Arrow;
			}
		}

		/// <summary>
		/// Starts the incremental search.
		/// </summary>
		internal void IncrementalSearch()
		{
			edit.ISearch = true;
			edit.ISearchString = string.Empty;
			edit.UnSelect();
			this.Cursor = edit.ISearchCursor;
		}

		/// <summary>
		/// Aborts the incremental search.
		/// </summary>
		internal void AbortIncrementalSearch()
		{
			edit.ISearch = false;
			if (edit.HasSelection)
			{
				string strTemp = edit.SelectedText;
				edit.FRDlg.AddToSearchedList(strTemp);
				edit.FRDlg.TextSearched = strTemp;
			}
			this.Cursor = Cursors.IBeam;
		}

		/// <summary>
		/// Moves the caret to the specified line.
		/// </summary>
		internal void GoToLine(int ln)
		{
			if (ln < 1)
			{
				edit.CurrentLine = 1;
			}
			else if (ln > edit.LineCount)
			{
				edit.CurrentLine = edit.LineCount;
			}
			else
			{
				edit.CurrentLine = ln;
			}
			ScrollToViewLine(edit.CurrentLine);
		}

		/// <summary>
		/// Scrolls the viewport to show the specified line.
		/// </summary>
		/// <param name="ln">The line to be shown.</param>
		internal void ScrollToViewLine(int ln)
		{
			edit.CurrentChar = 1;
			if (ln < ViewportFirstLine)
			{
				ViewportFirstLine = Math.Max(1, ln - ViewportLineCount/2);
			}
			else if (ln > ViewportLastLine)
			{
				ViewportFirstLine = Math.Min(MaxViewportFirstLine, 
					ln - ViewportLineCount/2);
			}
			ScrollToViewCaret();
		}

		/// <summary>
		/// Makes the specified line display in the center of the viewport.
		/// </summary>
		/// <param name="ln">The line around which the viewport should be 
		/// centered.</param>
		internal void CenterLine(int ln)
		{
			int lnTemp = ln - ViewportLineCount/2;
			if (lnTemp <= 1)
			{
				ViewportFirstLine = 1;
			}
			else if (lnTemp >= MaxViewportFirstLine)
			{
				ViewportFirstLine = MaxViewportFirstLine;
			}
			else
			{
				ViewportFirstLine = lnTemp;
			}
		}

		/// <summary>
		/// Sets the visibility of the horizontal scrollbar.
		/// </summary>
		/// <param name="bVisible">A value indicating the visibility of the 
		/// horizontal scrollbar.</param>
		internal void SetHScrollBarVisible(bool bVisible)
		{
			editHScrollBar.Visible = bVisible;
			editCornerArea.Visible = edit.HScrollBarVisible && edit.VScrollBarVisible;
		}

		/// <summary>
		/// Sets the visibility of the vertical scrollbar.
		/// </summary>
		/// <param name="bVisible">A value indicating the visibility of the 
		/// vertical scrollbar.</param>
		internal void SetVScrollBarVisible(bool bVisible)
		{
			editVScrollBar.Visible = bVisible;
			editCornerArea.Visible = edit.HScrollBarVisible && edit.VScrollBarVisible;
		}

		/// <summary>
		/// Updates all the controls and contents in the view.
		/// </summary>
		internal void UpdateControls()
		{
			int oldHeight = editContentRect.Height;
			int oldBottom = editContentRect.Bottom;
			int vScrollBarWidth = edit.VScrollBarVisible ? 
				editVScrollBar.Width : 0;
			int hScrollBarHeight = edit.HScrollBarVisible ? 
				editHScrollBar.Height : 0;
			int splitterHeight = bSplitterButtonVisible ? 
				editSplitterButton.Height : 0;
			editContentRect.Width = ClientRectangle.Width - 2 * edit.BorderWidth
				- vScrollBarWidth;
			editContentRect.Height = ClientRectangle.Height - 2 * edit.BorderWidth 
				- hScrollBarHeight;
			editVScrollBar.Height = editContentRect.Height - splitterHeight;
			editVScrollBar.Left = ClientRectangle.Width - editVScrollBar.Width 
				- edit.BorderWidth;
			editVScrollBar.Top = ClientRectangle.Top + edit.BorderWidth 
				+ splitterHeight;
			editHScrollBar.Width = editContentRect.Width;
			editHScrollBar.Left = edit.BorderWidth;
			editHScrollBar.Top = ClientRectangle.Height - editHScrollBar.Height 
				- edit.BorderWidth;
			editCornerArea.Left = editHScrollBar.Right;
			editCornerArea.Top = editHScrollBar.Top;
			if (!edit.InSplitting)
			{
				editVScrollBar.Visible = edit.VScrollBarVisible;
				editHScrollBar.Visible = edit.HScrollBarVisible;
				editCornerArea.Width = editVScrollBar.Width;
				editCornerArea.Height = editHScrollBar.Height;
				editCornerArea.Visible = edit.HScrollBarVisible && edit.VScrollBarVisible;
				editSplitterButton.Width = editVScrollBar.Width;
				editSplitterButton.Visible = bSplitterButtonVisible;
				editIndicatorRect.Width = (edit.IndicatorMarginVisible)? 
					(GetColumnWidth(2) + 2) : 0;
				editLineNumberRect.Width = (edit.LineNumberMarginVisible)? 
					((GetColumnWidth(Math.Max(5, GetDigits(edit.LineCount)))) + 2) : 0;
				editSelectionRect.Width = (edit.SelectionMarginVisible)? 
					(GetColumnWidth(1) + 6) : 0;
				editUserRect.Width = (edit.UserMarginVisible)? 
					(edit.UserMarginWidth) : 0;
				editTextRect.Width = editContentRect.Width 
					- editSelectionRect.Width - editIndicatorRect.Width 
					- editLineNumberRect.Width - editUserRect.Width;
				editContentRect.X = ClientRectangle.Left + edit.BorderWidth;
				editContentRect.Y = ClientRectangle.Top + edit.BorderWidth;
				editIndicatorRect.X = editContentRect.X;
				editIndicatorRect.Y = editContentRect.Y;
				editLineNumberRect.X = editIndicatorRect.Right;
				editLineNumberRect.Y = editContentRect.Y;
				editSelectionRect.X = editLineNumberRect.Right;
				editSelectionRect.Y = editContentRect.Y;
				editTextRect.X = editSelectionRect.Right;
				editTextRect.Y = editContentRect.Y;
				editUserRect.X = editTextRect.Right;
				editUserRect.Y = editContentRect.Y;
				editSplitterButton.Left = editVScrollBar.Left;
				editSplitterButton.Top = editContentRect.Y;
			}
			editIndicatorRect.Height = editContentRect.Height;
			editLineNumberRect.Height = editContentRect.Height;
			editSelectionRect.Height = editContentRect.Height;
			editTextRect.Height = editContentRect.Height;
			editUserRect.Height = editContentRect.Height;
			editHScrollBar.Refresh();
			editVScrollBar.Refresh();
			editCornerArea.Refresh();
			editSplitterButton.Refresh();
			if (RemoveViewportExtraLines())
			{
				UpdateViewportY();
				UpdateVScrollBar();
				if (!edit.InSplitting)
				{
					UpdateViewportX();
					UpdateHScrollBar();
				}
				Redraw();
				return;
			}
			if (edit.InSplitting)
			{
				UpdateViewportY();
				UpdateVScrollBar();
				if (oldHeight < editContentRect.Height)
				{
					Redraw(new Rectangle(ClientRectangle.X, oldBottom,
						ClientRectangle.Width, 
						ClientRectangle.Bottom - oldBottom + 1));
				}
				else
				{
					Redraw(new Rectangle(ClientRectangle.X, 
						ClientRectangle.Bottom - edit.BorderWidth, 
						ClientRectangle.Width, edit.BorderWidth));
				}
			}
			else
			{
				UpdateViewportX();
				UpdateViewportY();
				UpdateHScrollBar();
				UpdateVScrollBar();
				Redraw();
			}
		}

		/// <summary>
		/// Finds the specified string starting from the current caret location.
		/// </summary>
		/// <param name="str">The string to be found.</param>
		/// <param name="bSearchUp">A value indicating whether to search in the 
		/// upward direction.</param>
		/// <returns>true if the string was found; otherwise, false.</returns>
		internal bool FindNext(string str, bool bSearchUp)
		{
			EditLocationRange lcrTemp;
			if (!bSearchUp)
			{
				if (edit.HasSelection)
				{
					edit.CurrentLineChar = edit.Selection.GetEnd();
				}
				lcrTemp = edit.Find(edit.CurrentLineChar, str, bSearchUp);
			}
			else
			{
				if (edit.HasSelection)
				{
					edit.CurrentLineChar = edit.Selection.GetStart();
				}
				lcrTemp = edit.Find(edit.CurrentLineChar, str, bSearchUp);
			}
			if (lcrTemp != EditLocationRange.Empty)
			{
				edit.UnSelect();
				edit.Select(lcrTemp);
				ScrollToViewCaret();
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Finds the specified string for incremental search.
		/// </summary>
		/// <param name="str">The string to be find.</param>
		internal bool ISearchNext(string str)
		{
			EditLocationRange lcrTemp;
			if (edit.HasSelection)
			{
				edit.CurrentLineChar = edit.Selection.GetStart();
			}
			lcrTemp = edit.Find(edit.CurrentLineChar, str, false);
			if (lcrTemp != EditLocationRange.Empty)
			{
				edit.UnSelect();
				edit.Select(lcrTemp);
				ScrollToViewCaret();
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Finds the first occurance of the specified string.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="bMatchCase"></param>
		/// <param name="bMatchWholeWord"></param>
		/// <param name="bSearchHiddenText"></param>
		/// <param name="bSearchUp"></param>
		/// <param name="bUseRegex"></param>
		/// <param name="bUseWildcards"></param>
		/// <returns></returns>
		internal EditLocationRange Find(string str, bool bMatchCase, 
			bool bMatchWholeWord, bool bSearchHiddenText, bool bSearchUp, 
			bool bUseRegex, bool bUseWildcards, bool bWholeRange)
		{
			if (!bSearchUp)
			{
				if (edit.HasSelection)
				{
					edit.CurrentLineChar = edit.Selection.GetEnd();
				}
			}
			else
			{
				if (edit.HasSelection)
				{
					edit.CurrentLineChar = edit.Selection.GetStart();
				}
			}
			EditLocationRange lcr = editData.Find(edit.CurrentLineChar, 
				str, bMatchCase, bMatchWholeWord, bSearchHiddenText, 
				bSearchUp, bUseRegex, bUseWildcards, bWholeRange);
			if (lcr != EditLocationRange.Empty)
			{
				edit.UnSelect();
				edit.Select(lcr);
				ScrollToViewCaret();
			}
			return lcr;
		}

		/// <summary>
		/// Goes to the viewport that contains the caret.
		/// </summary>
		/// <returns></returns>
		internal bool ScrollToViewCaret()
		{
			if (GoToCaretViewport())
			{
				Redraw();
				return true;
			}
			UpdateCaretPos();
			return false;
		}

		/// <summary>
		/// Goes to the viewport that contains the caret without redrawing.
		/// </summary>
		/// <returns>true if the viewport has been adjusted; otherwise, 
		/// false.</returns>
		internal bool GoToCaretViewport()
		{
			BeginUpdate();
			bool bViewportLineAdjusted = AdjustViewportLine();
			bool bViewportCharAdjusted = AdjustViewportChar();
			EndUpdate();
			return (bViewportLineAdjusted || bViewportCharAdjusted);
		}

		/// <summary>
		/// Resets the viewport.
		/// </summary>
		internal void ResetView()
		{
			editViewportLineColumn.L = 1;
			editViewportLineColumn.C = 1;
			UpdateViewportX();
			UpdateViewportY();
			UpdateHScrollBar();
			UpdateVScrollBar();
			Redraw();
		}

		/// <summary>
		/// Draws the invalidated viewport.
		/// </summary>
		/// <param name="pe">A PaintEventArgs that contains the event data.
		/// </param>
		protected override void OnPaint(PaintEventArgs pe)
		{
			int top = pe.ClipRectangle.Top;
			int bottom = pe.ClipRectangle.Bottom;
			int height = bottom - top;
			if (!edit.HasContent)
			{
				pe.Graphics.FillRectangle(new SolidBrush(edit.TextBackColor), 
					editTextRect.Left, top, editTextRect.Width, height);
				if (edit.SelectionMarginVisible)
				{
					pe.Graphics.FillRectangle(new SolidBrush(edit.SelectionMarginBackColor), 
						editSelectionRect.Left, top, editSelectionRect.Width, height);
					DrawDummyOutlining(pe.Graphics);
				}
				if (edit.IndicatorMarginVisible)
				{
					ClearIndicatorMargin(pe.Graphics, top, height);
				}
				if (edit.LineNumberMarginVisible)
				{
					ClearLineNumberMargin(pe.Graphics, top, height); 
				}
				if (edit.UserMarginVisible)
				{
					ClearUserMargin(pe.Graphics, top, height); 
				}
				if (edit.InDesignMode)
				{
					pe.Graphics.DrawString(edit.Name, Font,
						new SolidBrush(edit.TextForeColor),
						editContentRect.Left + 1, editContentRect.Top, 
						new StringFormat(StringFormat.GenericTypographic));
				}
				if (edit.RightMarginLineVisible)
				{
					Pen pen = new Pen(edit.IndicatorMarginForeColor, 1);
					pen.DashStyle = DashStyle.Dash;
					pe.Graphics.DrawLine(pen, 
						editRightMarginLineX, top,
						editRightMarginLineX, bottom);
				}
				DrawBorder(pe.Graphics);
				return;
			}
			if (editPaintFreezeLevel > 0)
			{
				return;
			}
			// Make sure there are no interferences from caret displaying.
			if (bHasCaret)
			{
				HideCaret(this.Handle.ToInt32());
			}
			pe.Graphics.FillRectangle(new SolidBrush(edit.TextBackColor), 
				editTextRect.Left, top, editTextRect.Width, height);
			int lnStart = GetLineFromY(top);
			int lnEnd = GetLineFromY(bottom);
			for (int i = lnStart; i <= lnEnd; i++)
			{
				if (!editData.LineList[i-1].Hidden)
				{
					DrawTextLine(pe.Graphics, i);
				}
			}
			if (edit.IndicatorMarginVisible)
			{
				ClearIndicatorMargin(pe.Graphics, top, height);
				for (int i = lnStart; i <= lnEnd; i++)
				{
					if (!editData.LineList[i-1].Hidden)
					{
						DrawIndicatorMargin(pe.Graphics, i);
					}
				}
			}
			if (edit.LineNumberMarginVisible)
			{
				ClearLineNumberMargin(pe.Graphics, top, height);
				for (int i = lnStart; i <= lnEnd; i++)
				{
					if (!editData.LineList[i-1].Hidden)
					{
						DrawLineNumberMargin(pe.Graphics, i);
					}
				}
			}
			if (edit.SelectionMarginVisible)
			{
				pe.Graphics.FillRectangle(new SolidBrush(edit.SelectionMarginBackColor), 
					editSelectionRect.Left, top, editSelectionRect.Width, height);
				for (int i = lnStart; i <= lnEnd; i++)
				{
					if (!editData.LineList[i-1].Hidden)
					{
						DrawSelectionMargin(pe.Graphics, i);
					}
				}
			}
			if (edit.UserMarginVisible)
			{
				ClearUserMargin(pe.Graphics, top, height);
				if (edit.UserMarginPaintActive)
				{
					for (int i = lnStart; i <= lnEnd; i++)
					{
						if (!editData.LineList[i-1].Hidden)
						{
							UserMarginPaintEventArgs umpArgs = new 
								UserMarginPaintEventArgs(pe.Graphics,
								new Rectangle(editUserRect.Left, GetLineY(i), 
								editUserRect.Width, Font.Height), i);
							edit.RaiseUserMarginPaint(umpArgs);
						}
					}
				}
			}
			if (edit.GridLinesVisible)
			{
				for (int i = lnStart; i <= lnEnd; i++)
				{
					if (!editData.LineList[i-1].Hidden)
					{
						DrawGridLine(pe.Graphics, i);
					}
				}
			}
			if (edit.RightMarginLineVisible)
			{
				Pen pen = new Pen(edit.IndicatorMarginForeColor, 1);
				pen.DashStyle = DashStyle.Dash;
				pe.Graphics.DrawLine(pen, 
					editRightMarginLineX, top,
					editRightMarginLineX, bottom);
			}
			DrawBorder(pe.Graphics);
			if (bPanning)
			{
				pe.Graphics.DrawImage(edit.PanImage, 
					new Point(editPanAnchorLocation.X - edit.PanImage.Width/2,
					editPanAnchorLocation.Y - edit.PanImage.Height/2 + 1));
			}
			// Show the caret again.
			if (bHasCaret)
			{
				ShowCaret();
			}
		}

		/// <summary>
		/// Gets the EditLine object for the specified displayed line index.
		/// </summary>
		/// <param name="index">The index of displayed line.
		/// </param>
		internal EditLine GetVisibleEditLine(int index)
		{
			if ((!edit.OutliningEnabled) && (!edit.WordWrap))
			{
				return editData.LineList[index-1];
			}
			else
			{
				return editData.LineList[index-1];
			}
		}

		/// <summary>
		/// Updates the X coordinate of the viewport.
		/// </summary>
		internal void UpdateViewportX()
		{
			editViewportLocation.X = editTextRect.Left + editLeftMarginWidth
				- GetColumnWidth(editViewportLineColumn.C - 1);
			editRightMarginLineX = GetColumnWidth(edit.RightMarginLineColumn) 
				+ editViewportLocation.X;
		}

		/// <summary>
		/// Updates the Y coordinate of the viewport.
		/// </summary>
		internal void UpdateViewportY()
		{
			editViewportLocation.Y = editContentRect.Top - editPanOffset.Y;
		}

		/// <summary>
		/// Increases the no-paint level in the content area.
		/// </summary>
		internal void BeginUpdate()
		{
			editPaintFreezeLevel++;
		}

		/// <summary>
		/// Decreases the no-paint level in the content area.
		/// </summary>
		internal void EndUpdate()
		{
			if (editPaintFreezeLevel > 0)
			{
				editPaintFreezeLevel--;
			}
		}

		/// <summary>
		/// Calls Invalidate when painting is allowed.
		/// </summary>
		internal void Redraw()
		{
			if (editPaintFreezeLevel == 0)
			{
				Invalidate();
			}
		}

		/// <summary>
		/// Calls Invalidate for the specified rectangle when painting 
		/// is allowed.
		/// </summary>
		/// <param name="rect">The rectangle to be redrawn.</param>
		internal void Redraw(Rectangle rect)
		{
			if (editPaintFreezeLevel == 0)
			{
				Invalidate(rect);
			}
		}

		/// <summary>
		/// Calls Invalidate for the specified rectangle when painting 
		/// is allowed.
		/// </summary>
		/// <param name="rect">The rectangle to be redrawn.</param>
		/// <param name="bForced">A value indicating whether redrawing is  
		/// forced.</param>
		internal void Redraw(Rectangle rect, bool bForced)
		{
			if (bForced)
			{
				Invalidate(rect);
			}
			else
			{
				if (editPaintFreezeLevel == 0)
				{
					Invalidate(rect);
				}
			}
		}

		/// <summary>
		/// Redraws the selection margin when painting is allowed.
		/// </summary>
		internal void RedrawSelectionMargin()
		{
			if (editPaintFreezeLevel == 0)
			{
				Invalidate(editSelectionRect);
			}
		}

		/// <summary>
		/// Clears the indicator margin.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		internal void ClearIndicatorMargin(Graphics g, int top, int height)
		{
			g.FillRectangle(new SolidBrush(edit.IndicatorMarginBackColor), 
				editIndicatorRect.Left, top,
				editIndicatorRect.Width, height);
			// Draw the Indicator Margin boundary line.
			g.DrawLine(new Pen(edit.IndicatorMarginForeColor, 1), 
				editIndicatorRect.Right - 1, editIndicatorRect.Top,
				editIndicatorRect.Right - 1, editIndicatorRect.Bottom);
		}

		/// <summary>
		/// Clears the line number margin.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		internal void ClearLineNumberMargin(Graphics g, int top, int height)
		{
			g.FillRectangle(new SolidBrush(edit.LineNumberBackColor), 
				editLineNumberRect.Left, top,
				editLineNumberRect.Width, height);
			// Draw the line number Margin boundary line.
			Pen pen = new Pen(edit.LineNumberForeColor, 1);
			pen.DashStyle = DashStyle.Dot;
			g.DrawLine(pen, editLineNumberRect.Right - 1, 
				editLineNumberRect.Top,
				editLineNumberRect.Right - 1, editLineNumberRect.Bottom);
		}

		/// <summary>
		/// Clears the user margin.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		internal virtual void ClearUserMargin(Graphics g, int top, int height)
		{
			g.FillRectangle(new SolidBrush(edit.UserMarginBackColor), 
				editUserRect.Left, top,
				editUserRect.Width, height);
			// Draw the Indicator Margin boundary line.
			g.DrawLine(new Pen(edit.UserMarginForeColor, 1), 
				editUserRect.Left - 1, editUserRect.Top,
				editUserRect.Left - 1, editUserRect.Bottom);
		}

		/// <summary>
		/// Draws the indicators at the specified line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln">The line to be drawn.</param>
		private void DrawIndicatorMargin(Graphics g, int ln)
		{
			if (editData.LineList[ln-1].IndicatorData == null)
			{
				return;
			}
			for (int i = editData.GetIndicatorDataIndex(ln); 
				i < editData.LineList[ln-1].IndicatorData.Length; i++)
			{
				int ii = editData.LineList[ln-1].IndicatorData[i];
				if (ii <= 0)
				{
					edit.IndicatorList[-ii].Draw(g, new Rectangle(
						editIndicatorRect.Left, GetLineY(ln),
						editIndicatorRect.Width, Font.Height), 
						editSettings.GetColorGroupForeColor(
						edit.IndicatorList[-ii].GetName()), 
						editSettings.GetColorGroupBackColor(
						edit.IndicatorList[-ii].GetName()));
				}
				else
				{
					break;
				}
			}
		}

		/// <summary>
		/// Draws the line number margin of the specified line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln">The line to be drawn.</param>
		private void DrawLineNumberMargin(Graphics g, int ln)
		{
			string strTemp = ln.ToString();
			float strWidth = GetStringWidth(g, strTemp);
			if (strWidth > editLineNumberRect.Width)
			{
				UpdateControls();
				return;
			}
			g.DrawString(strTemp, Font, 
				new SolidBrush(edit.LineNumberForeColor),
				editLineNumberRect.Right - 2 - strWidth,
				GetLineY(ln), new StringFormat(StringFormat.GenericTypographic));
		}

		/// <summary>
		/// Draws the specified line in the selection margin.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln">The line to be drawn.</param>
		private void DrawSelectionMargin(Graphics g, int ln)
		{
			if (!edit.OutliningEnabled)
			{
				return;
			}
			EditOutliningSymbolType ost = GetOutliningSymbol(ln);
			int squareWidth = editSelectionRect.Width - 6;
			int midX = editSelectionRect.Left 
				+ editSelectionRect.Width/2 - 
				(editSelectionRect.Width/2 - squareWidth/2)%2;
			int lineY = GetLineY(ln);
			Pen pen = new Pen(edit.SelectionMarginForeColor, 1);
			g.DrawLine(pen, midX, lineY, midX, lineY + edit.LineHeight - 1);
			int YCoord = lineY + (edit.LineHeight - squareWidth) / 2;
			Rectangle boxRectangle = new Rectangle(midX - squareWidth/2, YCoord, 
				squareWidth, squareWidth);
			if (ost == EditOutliningSymbolType.InsideBlock) 
			{
				return;
			}
			if ((ost == EditOutliningSymbolType.BlockStart) 
				|| (ost == EditOutliningSymbolType.BlockEnd)
				|| (ost == EditOutliningSymbolType.BlockCollapsed)
				|| (ost == EditOutliningSymbolType.None))
			{
				// Clear the background
				Rectangle backRectangle = new Rectangle(editSelectionRect.Left, 
					lineY, editSelectionRect.Width, edit.LineHeight);
				g.FillRectangle(new SolidBrush(edit.SelectionMarginBackColor), 
					backRectangle);
			}
			if ((ost == EditOutliningSymbolType.SubBlockEnd) 
				||(ost == EditOutliningSymbolType.BlockEnd))
			{
				if (ost == EditOutliningSymbolType.BlockEnd) 
				{
					g.DrawLine(pen, midX, lineY, midX, YCoord + squareWidth/2);
				}
				g.DrawLine(pen, midX, YCoord + squareWidth/2,
					midX + editSelectionRect.Width/2 - 2, 
					YCoord + squareWidth/2);
			} 
			else if ((ost == EditOutliningSymbolType.BlockStart)
				|| (ost == EditOutliningSymbolType.SubBlockStart))
			{	
				g.FillRectangle(new SolidBrush(edit.CollapsedTextBackColor), 
					boxRectangle);
				g.DrawRectangle(pen, boxRectangle);
				g.DrawLine(pen, midX - squareWidth/2 + 2, 
					YCoord + (squareWidth)/2,
					midX - squareWidth/2 + 2 + squareWidth - 4, 
					YCoord + (squareWidth)/2);
			} 
			else if ((ost == EditOutliningSymbolType.BlockCollapsed)
				|| (ost == EditOutliningSymbolType.SubBlockCollapsed))
			{
				g.FillRectangle(new SolidBrush(edit.CollapsedTextBackColor), 
					boxRectangle);
				g.DrawRectangle(pen, boxRectangle);
				g.DrawLine(pen, midX - squareWidth/2 + 2, 
					YCoord + (squareWidth)/2,
					midX - squareWidth/2 + 2 + squareWidth - 4, 
					YCoord + (squareWidth)/2);
				g.DrawLine(pen, midX, YCoord + 2, midX, YCoord + squareWidth - 2);
			}
		}

		/// <summary>
		/// Gets the outlining symbol of the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <returns></returns>
		private EditOutliningSymbolType GetOutliningSymbol(int ln)
		{
			EditOutlining otln = editData.GetLeafOutlining(ln);
			if (otln.IsRoot)
			{
				return EditOutliningSymbolType.None;
			}
			else
			{
				if (otln.Collapsed)
				{
					if (ln == otln.StartLine)
					{
						if (otln.ParentOutlining == editData.OutliningRoot)
						{
							return EditOutliningSymbolType.BlockCollapsed;
						}
						else
						{
							return EditOutliningSymbolType.SubBlockCollapsed;
						}
					}
					else
					{
						return EditOutliningSymbolType.InsideBlock;
					}
				}
				else
				{
					if (ln == otln.StartLine)
					{
						if (otln.ParentOutlining == editData.OutliningRoot)
						{
							return EditOutliningSymbolType.BlockStart;
						}
						else
						{
							return EditOutliningSymbolType.SubBlockStart;
						}
					}
					else if (ln == otln.EndLine)
					{
						if (otln.ParentOutlining == editData.OutliningRoot)
						{
							return EditOutliningSymbolType.BlockEnd;
						}
						else
						{
							return EditOutliningSymbolType.SubBlockEnd;
						}
					}
					else
					{
						return EditOutliningSymbolType.InsideBlock;
					}
				}
			}
		}

		/// <summary>
		/// Gets the widths for the given string.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		internal float GetStringWidth(Graphics g, string str)
		{
			return g.MeasureString(str, Font, 
				editViewportLocation, edit.StringFormatTab).Width;
		}

		/// <summary>
		/// Gets the wordwrap positions for the specified string to fit the 
		/// specified width.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		internal short[] GetWordWrapChars(string str, float width)
		{
			if (str == string.Empty)
			{
				return null;
			}
			ArrayList alTemp = new ArrayList();
			string strTemp = str;
			int chTemp = GetFirstWordWrapChar(str, width);
			while (chTemp != -1)
			{
				alTemp.Add(chTemp);
				strTemp = strTemp.Substring(chTemp);
				chTemp = GetFirstWordWrapChar(strTemp, width);
			}
			if (alTemp.Count == 0)
			{
				return null;
			}
			else
			{
				short [] sTemp = new short[alTemp.Count];
				for (int i = 0; i < alTemp.Count; i++)
				{
					sTemp[i] = (short)alTemp[i];
				}
				return sTemp;
			}
		}

		/// <summary>
		/// Gets the first char where wordwrap happens.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="width"></param>
		/// <returns></returns>
		internal int GetFirstWordWrapChar(string str, float width)
		{
			float widthTemp = 0;
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] < 256)
				{
					widthTemp += edit.CharWidth[str[i]];
				}
				else
				{
					Graphics g = this.CreateGraphics();
					widthTemp += g.MeasureString(str[i].ToString(), 
						Font, editViewportLocation, 
						edit.StringFormatNoTab).Width;
				}
				if (widthTemp > width)
				{
					if ((!Char.IsLetterOrDigit(str[i])) && (str[i] != '_'))
					{
						return i+1;
					}
					else
					{
						for (int j = i-1; j > 0; j--)
						{
							if ((!Char.IsLetterOrDigit(str[j])) && (str[j] != '_'))
							{
								return j+1;
							}
						}
						return i+1;
					}
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the total number of columns for the specified width.
		/// </summary>
		/// <param name="width">The width in pixels.</param>
		/// <returns></returns>
		internal int GetColumnCountFromWidth(float width)
		{
			if (width <= 0)
			{
				return 1;
			}
			return (int)(width / edit.CharWidth['x'] + 1.01);
		}

		/// <summary>
		/// Gets the total number of space char for the specified width.
		/// </summary>
		/// <param name="width">The width in pixels.</param>
		/// <returns></returns>
		internal int GetSpaceCountFromWidth(float width)
		{
			if (width <= 0)
			{
				return 1;
			}
			return (int)(width / edit.CharWidth[' '] + 1.01);
		}

		/// <summary>
		/// Gets the total number of columns for the specified string.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		internal int GetColumnCount(string str)
		{
			Graphics g = this.CreateGraphics();
			return GetColumnCountFromWidth(GetStringWidth(g, str));
		}

		/// <summary>
		/// Gets the total number of columns for the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <returns>The column count for the line.</returns>
		internal int GetColumnCount(int ln)
		{
			return GetColumnCount(editData.LineList[ln - 1].LineString);
		}

		/// <summary>
		/// Tests if the specified location is valid.
		/// </summary>
		/// <param name="ln">The line of the location to be tested.</param>
		/// <param name="col">The column of the location to be tested.</param>
		/// <returns>true if the specified location is valid; otherwise, 
		/// false.</returns>
		internal bool IsValidLineColumn(int ln, int col)
		{
			if (!editData.IsValidLine(ln))
			{
				return false;
			}
			if (col < 1)
			{
				return false;
			}
			if (col > GetColumnCount(ln))
			{
				return edit.VirtualSpace;
			}
			return true;
		}

		/// <summary>
		/// Draws the border of the content area.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		protected virtual void DrawBorder(Graphics g)
		{
			if (edit.BorderStyle == BorderStyle.FixedSingle)
			{
				g.DrawRectangle(new Pen(edit.BorderColor, edit.BorderWidth),
					ClientRectangle.X, ClientRectangle.Y, 
					ClientRectangle.Width - edit.BorderWidth, 
					ClientRectangle.Height - edit.BorderWidth);
			}
			else if (edit.BorderStyle == BorderStyle.Fixed3D)
			{
				ControlPaint.DrawBorder3D(g, ClientRectangle, Border3DStyle.Sunken);
			}
		}

		/// <summary>
		/// Draws the grid line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		protected virtual void DrawGridLine(Graphics g, int ln)
		{
			int yPos = GetLineY(ln) + Font.Height;
			g.DrawLine(new Pen(edit.BorderColor, 1), 
				editContentRect.Left, yPos, 
				editContentRect.Right, yPos);
		}

		/// <summary>
		/// Gets the point from the specified location.
		/// </summary>
		/// <param name="lc"></param>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		internal void GetPoint(EditLocation lc, out int X, out int Y)
		{
			if (!edit.OutliningEnabled)
			{
				GetPointNoOutlining(lc, out X, out Y);
			}
			else
			{
				EditOutlining otln = editData.GetLeafOutlining(lc.L);
				if (!otln.Collapsed)
				{
					GetPointNoOutlining(lc, out X, out Y);
				}
				else
				{
					Y = GetLineY(otln.Start.L);
					int chStart = -1;
					int chEnd = -1;
					string strLine = editData.GetStringObject(otln.Start.L);
					string strTemp = 
						otln.GetCollapsedString(ref chStart, ref chEnd);
					Graphics g = this.CreateGraphics();
					if (lc <= otln.Start)
					{
						X = (int)(GetStringWidth(g, strLine.Substring(0, lc.C - 1)) 
							+ editViewportLocation.X);
					}
					else if (lc > otln.End)
					{
						string strLine2 = editData.GetStringObject(otln.EndLine).
							Substring(otln.End.C - 1, lc.C - otln.End.C);
						X = (int)(GetStringWidth(g, strLine.Substring(0, chStart - 1)
							+ strTemp + strLine2) + editViewportLocation.X);
					}
					else
					{
						X = (int)(GetStringWidth(g, strLine.Substring(0, chStart - 1) 
							+ strTemp) + editViewportLocation.X);
					}
				}
			}
		}

		/// <summary>
		/// Gets the point from the specified location without considering 
		/// outlining.
		/// </summary>
		/// <param name="lc"></param>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		internal void GetPointNoOutlining(EditLocation lc, out int X, out int Y)
		{
			Y = GetLineY(lc.L);
			if (lc.C <= 1) 
			{
				X = editViewportLocation.X;
			} 
			else 
			{
				int lnLengthPlusOne = editData.GetLineLengthPlusOne(lc.L);
				string strTemp;
				if (lc.C <= lnLengthPlusOne)
				{
					strTemp = editData.GetStringObject(lc.L).Substring(0, lc.C - 1);
				}
				else
				{
					strTemp = editData.GetStringObject(lc.L)
						+ new string(' ', lc.C - lnLengthPlusOne);
				}
				Graphics g = this.CreateGraphics();
				X = (int)(GetStringWidth(g, strTemp) + editViewportLocation.X);
			}
		}

		/// <summary>
		/// Gets the point from the specified location.
		/// </summary>
		/// <param name="lc"></param>
		/// <returns></returns>
		internal Point GetPoint(EditLocation lc)
		{
			int xTemp;
			int yTemp;
			GetPoint(edit.CurrentLineChar, out xTemp, out yTemp);
			return new Point(xTemp, yTemp);
		}

		/// <summary>
		/// Gets the location from the specified point.
		/// </summary>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		/// <param name="L"></param>
		/// <param name="C"></param>
		internal void GetLineChar(int X, int Y, out int L, out int C)
		{
			L = GetLineFromY(Y);
			string strLine = editData.GetStringObject(L);
			if (!edit.OutliningEnabled)
			{
				GetChar(strLine, X, out C);
			}
			else
			{
				EditOutlining otln = editData.GetLeafOutlining(L);
				if (!otln.Collapsed)
				{
					GetChar(strLine, X, out C);
				}
				else
				{
					int chStart = -1;
					int chEnd = -1;
					string strTemp;
					strTemp = otln.GetCollapsedString(ref chStart, ref chEnd);
					strLine = strLine.Substring(0, chStart - 1) + strTemp + 
						editData.GetStringObject(otln.EndLine).Substring(otln.End.C - 1);
					GetChar(strLine, X, out C);
					if (C > chStart) 
					{
						L = otln.End.L;
						if (C < (chStart + strTemp.Length))
						{
							C = otln.End.C;
						}
						else
						{
							C = otln.End.C + C - (chStart + strTemp.Length);
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets the char index at the specified X location for the specified 
		/// string.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="X"></param>
		/// <param name="C"></param>
		internal void GetChar(string str, int X, out int C)
		{
			C = 1;
			int XTemp = X - editViewportLocation.X;
			string strTemp = str;
			if (edit.VirtualSpace)
			{
				strTemp += new string(' ', GetSpaceCountFromWidth(XTemp));
			}
			else
			{
				if (strTemp == string.Empty)
				{
					return;
				}
			}
			Graphics g = this.CreateGraphics();
			float strWidthBefore;
			float strWidthAfter;
			strWidthAfter = GetStringWidth(g, strTemp);
			if (XTemp >= strWidthAfter)
			{
				C = strTemp.Length + 1;
				return;
			}											
			int iMin = 1;
			int iMax = strTemp.Length;
			int iMid;
			while (iMin <= iMax)
			{
				iMid = (iMin + iMax)/2;
				strWidthBefore = GetStringWidth(g, strTemp.Substring(0,iMid-1));
				strWidthAfter = GetStringWidth(g, strTemp.Substring(0,iMid));
				if ((XTemp <= strWidthAfter) && (XTemp >= strWidthBefore))
				{
					if (XTemp < strWidthBefore + (strWidthAfter 
						- strWidthBefore)/2.0f)
					{
						C = iMid;
					}
					else
					{
						C = iMid + 1;
					}
					return;
				}
				else if (XTemp < strWidthBefore)
				{
					iMax = iMid - 1;
				}
				else
				{
					iMin = iMid + 1;
				}
			}
		}

		/// <summary>
		/// Gets the location from the specified pixel location.
		/// </summary>
		/// <param name="X">The X-coordinate of the location.</param>
		/// <param name="Y">The Y-coordinate of the location.</param>
		/// <returns>The line/char of the specified pixel location.</returns>
		internal EditLocation GetLineChar(int X, int Y)
		{
			int lTemp;
			int cTemp;
			GetLineChar(X, Y, out lTemp, out cTemp);
			return new EditLocation(lTemp, cTemp);
		}

		/// <summary>
		/// Gets the width of the specified columns with the reference letter.
		/// </summary>
		/// <param name="col"></param>
		/// <returns></returns>
		private int GetColumnWidth(int col)
		{
			if (col < 1)
			{
				return 0;
			}
			return (int)(col * edit.CharWidth[(int)'x']);
		}

		/// <summary>
		/// Gets the column start location.
		/// </summary>
		/// <param name="g"></param>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		/// <returns></returns>
		private float GetCharStartX(Graphics g, int ln, int ch)
		{
			if (ch <= 1) return 0;
			string strLine = editData.GetStringObject(ln);
			if (strLine == string.Empty)
			{
				return 0;
			}
			else
			{
				int chTemp;
				chTemp = Math.Min(ch, edit.GetLineLengthPlusOne(ln));
				return GetStringWidth(g, editData.GetStringObject(ln).
					Substring(0, chTemp-1));
			}
		}

		/// <summary>
		/// Draws a dummy outlining region.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		internal void DrawDummyOutlining(Graphics g)
		{
			int squareWidth = editSelectionRect.Width - 6;
			int midX = editSelectionRect.Left 
				+ editSelectionRect.Width/2 - 
				(editSelectionRect.Width/2 - squareWidth/2)%2;
			int lineY = editSelectionRect.Top;
			Pen pen = new Pen(edit.SelectionMarginForeColor, 1);
			int YCoord = lineY + (edit.LineHeight - squareWidth) / 2;
			int lineTopY = lineY + edit.LineHeight;
			g.DrawEllipse(pen, midX - squareWidth/2, YCoord, squareWidth, squareWidth);
			lineY = editSelectionRect.Bottom - edit.LineHeight;
			YCoord = lineY + (edit.LineHeight - squareWidth) / 2;
			g.DrawEllipse(pen, midX - squareWidth/2, YCoord, squareWidth, squareWidth);
			g.DrawLine(pen, midX, lineTopY, midX, lineY);
		}

		/// <summary>
		/// Gets the number of digits for the specified positive integer.
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		private int GetDigits(int num)
		{
			if (num == 0)
			{
				return 1;
			}
			int nDigit = 0;
			for (int i = num; i > 0; i /= 10)
			{
				nDigit++;
			}
			return nDigit;
		}

		/// <summary>
		/// Scrolls up the viewport by the specified number of lines.
		/// </summary>
		/// <param name="ln">The number (negative value for scrolling 
		/// up) of lines to be scrolled. </param>
		internal void Scroll(int ln)
		{
			if (ln < 0)
			{
				ViewportFirstLine = editData.GetVisibleLineByOffset(ViewportFirstLine, ln);
			}
			else if (ln > 0)
			{
				ViewportFirstLine = Math.Min(MaxViewportFirstLine, 
					editData.GetVisibleLineByOffset(ViewportFirstLine, ln));
			}
		}

		/// <summary>
		/// Scrolls the viewport to the first line.
		/// </summary>
		internal void ScrollToFirstLine()
		{
			ViewportFirstLine = editData.FirstVisibleLine;
		}

		/// <summary>
		/// Scrolls the viewport to the last line.
		/// </summary>
		internal void ScrollToLastLine()
		{
			ViewportFirstLine = MaxViewportFirstLine;
		}

		/// <summary>
		/// Scrolls the viewport up by one line.
		/// </summary>
		internal void ScrollUpOneLine()
		{
			SaveCurrentX();
			Scroll(-1);
			if (edit.CurrentLine > ViewportLastLine)
			{
				edit.CurrentLine = ViewportLastLine;
				edit.CurrentChar = GetCharFromX(edit.OriginalX);
				ScrollToViewCaret();
			}
		}

		/// <summary>
		/// Scrolls the viewport down by one line.
		/// </summary>
		internal void ScrollDownOneLine()
		{
			SaveCurrentX();
			Scroll(1);
			if (edit.CurrentLine < ViewportFirstLine)
			{
				edit.CurrentLine = ViewportFirstLine;
				edit.CurrentChar = GetCharFromX(edit.OriginalX);
				ScrollToViewCaret();
			}
		}

		/// <summary>
		/// Scrolls the viewport up by one page.
		/// </summary>
		internal void ScrollUpOnePage()
		{
			Scroll(-ViewportLineCount);
		}

		/// <summary>
		/// Scrolls the viewport down by one page.
		/// </summary>
		internal void ScrollDownOnePage()
		{
			Scroll(ViewportLineCount);
		}

		/// <summary>
		/// Scrolls the viewport left by the specified column number.
		/// </summary>
		/// <param name="col">The number of columns to scroll.</param>
		internal void ScrollLeft(int col)
		{
			if (ViewportFirstColumn == 1)
			{
				return;
			}
			ViewportFirstColumn = Math.Max(1, ViewportFirstColumn - col);
		}

		/// <summary>
		/// Scrolls the viewport right by the specified column number.
		/// </summary>
		/// <param name="col">The number of columns to scroll.</param>
		internal void ScrollRight(int col)
		{
			if (ViewportFirstColumn >= editHScrollBar.Maximum)
			{
				return;
			}
			ViewportFirstColumn = Math.Min(editHScrollBar.Maximum, 
				ViewportFirstColumn + col);
		}

		/// <summary>
		/// Scrolls the viewport left by one column.
		/// </summary>
		internal void ScrollLeftOneColumn()
		{
			ScrollLeft(1);
		}

		/// <summary>
		/// Scrolls the viewport right by one column.
		/// </summary>
		internal void ScrollRightOneColumn()
		{
			ScrollRight(1);
		}

		/// <summary>
		/// Scrolls the viewport left by one page.
		/// </summary>
		internal void ScrollLeftOnePage()
		{
			ScrollLeft(ViewportColumnCount);
		}

		/// <summary>
		/// Scrolls the viewport right by one page.
		/// </summary>
		internal void ScrollRightOnePage()
		{
			ScrollRight(ViewportColumnCount);
		}

		/// <summary>
		/// Scrolls the viewport to the left edge.
		/// </summary>
		internal void ScrollToLeftEdge()
		{
			ViewportFirstColumn = 1;
		}

		/// <summary>
		/// Scrolls the viewport to the right edge.
		/// </summary>
		internal void ScrollToRightEdge()
		{
			ViewportFirstColumn = editHScrollBar.Maximum;
		}

		/// <summary>
		/// Handles the input of the up arrow key.
		/// </summary>
		internal void UpArrowKey()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				edit.CurrentLineChar = edit.Selection.GetStart();
				edit.UnSelect();
			}
			LineUp();
		}

		/// <summary>
		/// Handles the input of the down arrow key.
		/// </summary>
		internal void DownArrowKey()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				edit.CurrentLineChar = edit.Selection.GetEnd();
				edit.UnSelect();
			}
			LineDown();
		}

		/// <summary>
		/// Handles the input of the left arrow key.
		/// </summary>
		internal void LeftArrowKey()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				edit.CurrentLineChar = edit.Selection.GetStart();
				edit.UnSelect();
			}
			else
			{
				CharLeft();
			}
		}

		/// <summary>
		/// Reponses to the right arrow key.
		/// </summary>
		internal void RightArrowKey()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				edit.CurrentLineChar = edit.Selection.GetEnd();
				edit.UnSelect();
			}
			else
			{
				CharRight();
			}
		}

		/// <summary>
		/// Reponses to the Home key.
		/// </summary>
		internal void HomeKey()
		{
			if (edit.HasSelection)
			{
				edit.CurrentLineChar = edit.Selection.GetStart();
				edit.UnSelect();
			}
			LineStart();
		}

		/// <summary>
		/// Reponses to the End key.
		/// </summary>
		internal void EndKey()
		{
			if (edit.HasSelection)
			{
				edit.CurrentLineChar = edit.Selection.GetEnd();
				edit.UnSelect();
			}
			LineEnd();
		}

		/// <summary>
		/// Redraws the specified line.
		/// </summary>
		/// <param name="ln">The line to be redrawn.</param>
		internal void RedrawLine(int ln)
		{
			if ((ln >= ViewportFirstLine) && (ln <= ViewportLastLine + 1))
			{
				Redraw(GetRectangle(ln, ln));
			}
		}

		/// <summary>
		/// Redraws the specified line.
		/// </summary>
		/// <param name="ln">The line to be redrawn.</param>
		/// <param name="bForced">A value indicating whether redrawing is  
		/// forced.</param>
		internal void RedrawLine(int ln, bool bForced)
		{
			if ((ln >= ViewportFirstLine) && (ln <= ViewportLastLine + 1))
			{
				Redraw(GetRectangle(ln, ln), bForced);
			}
		}

		/// <summary>
		/// Redraws from the specified line.
		/// </summary>
		/// <param name="ln">The line from which the redrawing is started.</param>
		internal void RedrawFromLine(int ln)
		{
			int lnStart = Math.Max(ln, ViewportFirstLine);
			int bottomLn = Math.Min(edit.LineCount, ViewportLastLine + 1);
			if (bottomLn == edit.LineCount)
			{
				Redraw(GetRectangle(lnStart, -1));
			}
			else
			{
				Redraw(GetRectangle(lnStart, bottomLn));	
			}
		}

		/// <summary>
		/// Redraws the specified line range. 
		/// </summary>
		/// <param name="lnStart">The starting line of the line range to 
		/// be redrawn.</param>
		/// <param name="lnEnd">The ending line of the line range to be 
		/// redrawn.</param>
		internal void RedrawLines(int lnStart, int lnEnd)
		{
			int lnStartTemp = (lnStart <= lnEnd) ? lnStart : lnEnd;
			int lnEndTemp = (lnStart <= lnEnd) ? lnEnd : lnStart;
			lnStartTemp = Math.Max(lnStartTemp, ViewportFirstLine);
			lnEndTemp = Math.Min(lnEndTemp, ViewportLastLine + 1);
			if (lnStartTemp <= lnEndTemp)
			{
				Redraw(GetRectangle(lnStartTemp, lnEndTemp));
			}
		}

		/// <summary>
		/// Draws the specified text line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln">The line to be drawn.</param>
		protected virtual void DrawTextLine(Graphics g, int ln)
		{
			editData.UpdateLineInfo(ln);
			if (editData.LineList[ln-1].Hidden)
			{
				return;
			}
			int col = 1;
			float xPos = editViewportLocation.X;
			float yPos = GetLineY(ln);
			if (editData.LineList[ln-1].Highlighted)
			{
				DrawHighlightedLine(g, ln, col, xPos, yPos);
			}
			else if ((edit.HasSelection) && (!edit.HideSelection))
			{
				if ((edit.Selection.GetStart().L <= ln)
					&& (edit.Selection.GetEnd().L >= ln))
				{
					DrawWithSelection(g, ln, col, xPos, yPos);
				}
				else
				{
					EditOutlining otln = editData.GetLeafOutlining(ln);
					if (otln.Collapsed)
					{
						if ((edit.Selection.GetStart().L <= otln.EndLine)
							&& (edit.Selection.GetEnd().L >= otln.EndLine))
						{
							DrawWithSelection(g, ln, col, xPos, yPos);
						}
						else
						{
							DrawWithoutSelection(g, ln, col, xPos, yPos);
						}
					}
					else
					{
						DrawWithoutSelection(g, ln, col, xPos, yPos);
					}
				}
			}
			else
			{
				DrawWithoutSelection(g, ln, col, xPos, yPos);
			}
			DrawWaveLine(g, ln);
			if(edit.BraceMatchingEnabled)
			{
				DrawBraceMatching(g, ln);
			}
			if (edit.WhiteSpaceVisible && (ln == edit.LineCount))
			{
				xPos += GetStringWidth(g, editData.GetStringObject(ln));
				DrawVisibleEOF(g, new Rectangle((int)xPos, (int)yPos, 
					(int)(edit.CharWidth[' ']), edit.LineHeight));
			}
			if ((!editData.LineList[ln-1].Highlighted) && 
				(!editData.LineList[ln-1].IsCustomBackColor))
			{
				g.FillRectangle(new SolidBrush(edit.TextBackColor), 
					(int)editTextRect.X, (int)yPos, 
					editLeftMarginWidth, edit.LineHeight);
			}
		}

		/// <summary>
		/// Draw a text line with no highlighted portions.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln">The line to be drawn.</param>
		protected virtual void DrawWithoutSelection(Graphics g, int ln, int col, 
			float xPos, float yPos)
		{
			Color CustomForeColor = Color.Empty;
			if(editData.LineList[ln-1].IsCustomForeColor)
			{
				CustomForeColor = editData.LineList[ln-1].CustomForeColor;
			}
			if (editData.LineList[ln-1].IsCustomBackColor)
			{
				g.FillRectangle(new 
					SolidBrush(editData.LineList[ln-1].CustomBackColor), 
					new Rectangle(editTextRect.Left, (int)yPos, 
					editTextRect.Width, edit.LineHeight));
			}
			EditOutlining otln = editData.GetLeafOutlining(ln);
			if (otln.Collapsed)
			{
				editData.UpdateLineInfo(otln.EndLine);
				int chStart = -1;
				int chEnd = -1;
				string strTemp;
				strTemp = otln.GetCollapsedString(ref chStart, ref chEnd);
				if (chStart > 1)
				{
					EditLine editLn1 = editData.GetPartialEditLine(
						editData.LineList[ln-1], 1, chStart - 1);
					DrawColorTextLine(g, ref col, ref xPos, yPos, editLn1,CustomForeColor);
				}
				DrawCollapsedOutlining(g, ref col, ref xPos, yPos, strTemp, 
					edit.CollapsedTextBackColor);
				int lnLengthPlusOne2 = editData.GetLineLengthPlusOne(otln.EndLine);
				if (otln.End.C < lnLengthPlusOne2)
				{
					EditLine editLn2 = editData.GetPartialEditLine(
						editData.LineList[otln.EndLine-1], 
						otln.End.C, lnLengthPlusOne2 - 1);
					DrawColorTextLine(g,  ref col, ref xPos, yPos, editLn2,CustomForeColor);
				}
				return;
			}
			DrawColorTextLine(g, ref col, ref xPos, yPos, 
				editData.LineList[ln-1],CustomForeColor);
		}

		/// <summary>
		/// Draws a highlighted text line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln">The line to be drawn with highlight.</param>
		protected virtual void DrawHighlightedLine(Graphics g, int ln, int col, 
			float xPos, float yPos)
		{
			short cgi = (Focused || edit.FRDlg.Visible) ? 
				edit.SelectedTextColorGroupIndex : 
				edit.InactiveSelectedTextColorGroupIndex;
			g.FillRectangle(new SolidBrush(
				editSettings.GetColorGroupBackColor(cgi)), 
				new Rectangle((int)xPos, (int)yPos, 
				editTextRect.Width, edit.LineHeight));
			EditLine editLn = editData.GetEditLineWithHighlight(editData.
				LineList[ln-1], 1, (short)editData.GetLineLength(ln), cgi);
			DrawColorTextLine(g, ref col, ref xPos, yPos, editLn,Color.Empty);
		}

		/// <summary>
		/// Draw a text line with a highlighted portion.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln">The line to be drawn.</param>
		protected virtual void DrawWithSelection(Graphics g, int ln, int col, 
			float xPos, float yPos)
		{
			Color CustomForeColor = Color.Empty;
			if(editData.LineList[ln-1].IsCustomForeColor)
			{
				CustomForeColor = editData.LineList[ln-1].CustomForeColor;
			}
			if (editData.LineList[ln-1].IsCustomBackColor)
			{
				g.FillRectangle(new 
					SolidBrush(editData.LineList[ln-1].CustomBackColor), 
					new Rectangle(editTextRect.Left, (int)yPos, 
					editTextRect.Width, edit.LineHeight));
			}
			EditLocationRange selNorm = edit.Selection.Normalize();
			short cgi = (Focused || edit.FRDlg.Visible) ? 
				edit.SelectedTextColorGroupIndex : 
				edit.InactiveSelectedTextColorGroupIndex;
			int selStartCh;
			int selEndCh;
			int lnLengthPlusOne = editData.GetLineLengthPlusOne(ln);
			selStartCh = (selNorm.Start.LessThan(ln, 1))? 1 : selNorm.Start.C;
			selEndCh = (selNorm.End.GreaterThan(ln, lnLengthPlusOne)) ? 
			lnLengthPlusOne : (selNorm.End.C - 1);
			EditOutlining otln = editData.GetLeafOutlining(ln);
			if (otln.Collapsed)
			{
				editData.UpdateLineInfo(otln.EndLine);
				int chStart = -1;
				int chEnd = -1;
				string strTemp;
				strTemp = otln.GetCollapsedString(ref chStart, ref chEnd);
				if (chStart > 1)
				{
					EditLine editLn0 = editData.GetPartialEditLine(
						editData.LineList[ln-1], 1, chStart - 1);
					if (selStartCh >= chStart)
					{
						DrawColorTextLine(g, ref col, ref xPos, 
							yPos, editLn0,CustomForeColor);
					}
					else
					{
						short selEnd = (selEndCh < chStart) ? 
							(short)selEndCh : (short)(chStart - 1);
						EditLine editLn1 = editData.
							GetEditLineWithHighlight(editLn0, 
							(short)selStartCh, selEnd, cgi);
						DrawColorTextLine(g, ref col, ref xPos, 
							yPos, editLn1,CustomForeColor);
					}
				}
				if (selNorm.Contains(otln))
				{
					DrawCollapsedOutlining(g, ref col, ref xPos, yPos, strTemp, 
						editSettings.GetColorGroupBackColor(cgi));
				}
				else
				{
					DrawCollapsedOutlining(g, ref col, ref xPos, yPos, strTemp, 
						edit.CollapsedTextBackColor);
				}
				int lnLengthPlusOne2 = editData.GetLineLengthPlusOne(otln.EndLine);
				selStartCh = (selNorm.Start.LessThan(otln.EndLine, 1))? 
					1 : selNorm.Start.C;
				selEndCh = (selNorm.End.GreaterThan(otln.EndLine, lnLengthPlusOne2)) ? 
					lnLengthPlusOne2 - 1 : (selNorm.End.C - 1);
				if (otln.End.C < lnLengthPlusOne2)
				{
					if (selEndCh < otln.End.C)
					{
						EditLine editLn0 = editData.GetPartialEditLine(
							editData.LineList[otln.EndLine-1], 
							otln.End.C, lnLengthPlusOne2 - 1);
						DrawColorTextLine(g, ref col, ref xPos, 
							yPos, editLn0,CustomForeColor);
					}
					else
					{
						EditLine editLn0 = editData.
							GetEditLineWithHighlight(editData.
							LineList[otln.End.L-1], 
							(short)selStartCh, (short)selEndCh, cgi);
						EditLine editLn1 = editData.GetPartialEditLine(
							editLn0, otln.End.C, lnLengthPlusOne2 - 1);
						DrawColorTextLine(g, ref col, ref xPos, 
							yPos, editLn1,CustomForeColor);
					}
				}
				return;
			}
			EditLine editLn = editData.GetEditLineWithHighlight(editData.
				LineList[ln-1], (short)selStartCh, (short)selEndCh, cgi);
			DrawColorTextLine(g, ref col, ref xPos, yPos, editLn,CustomForeColor);
		}

		/// <summary>
		/// Draw the specified EditLine object at the specified line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="col">The starting column.</param>
		/// <param name="xPos">The starting X location.</param>
		/// <param name="yPos">The Y location.</param>
		/// <param name="editLn">The EditLine object to be drawn.</param>
		protected virtual void DrawColorTextLine(Graphics g, ref int col, 
			ref float xPos, float yPos, EditLine editLn,Color CustomForeColor)
		{
			Color ForeColor = edit.TextForeColor;
			if(CustomForeColor != Color.Empty)
			{
				ForeColor = CustomForeColor;
			}
			string strLine = editLn.LineString;
			short [] ctd = editLn.ColorTextData;
			if (strLine == string.Empty)
			{
				return;
			}
			if (ctd == null)
			{
				DrawColorString(g, ref col, ref xPos, yPos, strLine, 
					ForeColor, edit.TextBackColor, !editLn.IsCustomBackColor,0);
			}
			else
			{
				if (ctd.Length == 1)
				{
					DrawColorString(g, ref col, ref xPos, yPos, strLine, 
						editSettings.GetColorGroupForeColor(ctd[0]), 
						editSettings.GetColorGroupBackColor(ctd[0]),
						!editLn.IsCustomBackColor,
						editSettings.GetColorGroupType(ctd[0]));
				}
				else
				{
					string strTemp1;
					int charCurrent = 1;
					for (int i = 0; i < ctd.Length; i += 3)
					{
						if (ctd[i] > charCurrent)
						{
							strTemp1 = strLine.Substring(charCurrent - 1, 
								ctd[i] - charCurrent);
							DrawColorString(g, ref col, ref xPos, yPos, strTemp1, 
								ForeColor, edit.TextBackColor, 
								!editLn.IsCustomBackColor,0);
							if (xPos > editContentRect.Right)
							{
								break;
							}
							charCurrent = ctd[i];
						}
						strTemp1 = strLine.Substring(ctd[i] - 1, 
							ctd[i+1] - ctd[i] + 1);
						DrawColorString(g, ref col, ref xPos, yPos, strTemp1, 
							editSettings.GetColorGroupForeColor(ctd[i+2]), 
							editSettings.GetColorGroupBackColor(ctd[i+2]),
							!editLn.IsCustomBackColor,
							editSettings.GetColorGroupType(ctd[i+2]));
						if (xPos > editContentRect.Right)
						{
							break;
						}
						charCurrent = ctd[i+1] + 1;
					}
					if (charCurrent < strLine.Length + 1)
					{
						if (xPos < editContentRect.Right)
						{
							strTemp1 = strLine.Substring(charCurrent - 1);
							DrawColorString(g, ref col, ref xPos, yPos, strTemp1,  
								ForeColor, edit.TextBackColor, !editLn.IsCustomBackColor,0);
						}
					}
				}
			}
		}

		/// <summary>
		/// Get the next tab stop position.
		/// </summary>
		/// <param name="xPos"></param>
		/// <returns></returns>
		internal float GetNextTabX(float xPos)
		{
			int n = (int)((xPos - editViewportLocation.X)/edit.TabStops[0] + 1.01);
			return n * edit.TabStops[0] + editViewportLocation.X;
		}

		/// <summary>
		/// Draws the specified string at the specified location with the 
		/// specified foreground and background colors.
		/// </summary>
		/// <param name="g">The graphics to draw on.</param>
		/// <param name="col">The starting column.</param>
		/// <param name="xPos">The starting X position.</param>
		/// <param name="yPos">The starting Y position.</param>
		/// <param name="str">The string to be drawn.</param>
		/// <param name="foreColor">The foreground color for the string.</param>
		/// <param name="backColor">The background color for the string.</param>
		/// <param name="bFillBackColor">A value indicating whether to draw the 
		/// string with the specified background color.</param>
		protected void DrawColorString(Graphics g, ref int col, ref float xPos, float yPos, 
			string str, Color foreColor, Color backColor, bool bFillBackColor,EditColorGroupType groupType)
		{
			System.Drawing.Font font = this.Font;
			switch(groupType)
			{
				case EditColorGroupType.BoldText:
					font = new Font(font.FontFamily,font.Size,FontStyle.Bold);
					break;
				case EditColorGroupType.UnderLine:
					font = new Font(font.FontFamily,font.Size,FontStyle.Underline);
					break;
			}

			SolidBrush foreBrush = new SolidBrush(foreColor);
			float [] xPosArray = new float[str.Length + 1];
			xPosArray[0] = xPos;
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '\t')
				{
					xPosArray[i+1] = GetNextTabX(xPosArray[i]);
					col += edit.TabSize - (col - 1)%edit.TabSize;
				}
				else
				{
					if (str[i] < 256)
					{
						xPosArray[i+1] = xPosArray[i] + edit.CharWidth[str[i]];
					}
					else
					{
						xPosArray[i+1] = xPosArray[i] + 
							g.MeasureString(str[i].ToString(), 
							font, editViewportLocation, 
							edit.StringFormatNoTab).Width;
					}
					col++;
				}
			}
			xPos = xPosArray[str.Length];
			if ((xPosArray[str.Length] < editTextRect.Left) ||
				(xPosArray[0] > editTextRect.Right))
			{
				return;
			}
			if (bFillBackColor && (backColor != edit.TextBackColor))
			{
				g.FillRectangle(new SolidBrush(backColor), 
					new RectangleF(xPosArray[0], yPos, 
					xPosArray[str.Length] - xPosArray[0], 
					edit.LineHeight));
			}
			if (!edit.WhiteSpaceVisible)
			{
				for (int i = 0; i < str.Length; i++)
				{
					if ((str[i] != ' ') && (str[i] != '\t'))
					{
						g.DrawString(str[i].ToString(), font, 
							foreBrush, xPosArray[i], yPos, 
							edit.StringFormatNoTab);
					}
				}
			}
			else
			{
				for (int i = 0; i < str.Length; i++)
				{
					if (str[i] == ' ')
					{
						DrawVisibleSpace(g, new Rectangle((int)xPosArray[i], 
							(int)yPos, (int)(xPosArray[i+1] - xPosArray[i]), 
							edit.LineHeight));
					}
					else if (str[i] == '\t')
					{
						DrawVisibleTab(g, new Rectangle((int)xPosArray[i], 
							(int)yPos, (int)(xPosArray[i+1] - xPosArray[i]), 
							edit.LineHeight));
					}
					else
					{
						g.DrawString(str[i].ToString(), font, 
							foreBrush, xPosArray[i], yPos, 
							edit.StringFormatNoTab);
					}
				}
			}
		}

		/// <summary>
		/// Gets the visible rectangle of the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range.</param>
		/// <param name="lnEnd">The ending line of the line range.</param>
		/// <returns>The rectangle for the line range.</returns>
		internal Rectangle GetRectangle(int lnStart, int lnEnd)
		{
			if (lnEnd == -1)
			{
				return new Rectangle(editContentRect.Left, 
					GetLineY(lnStart) - 1, editContentRect.Width, 
					editContentRect.Bottom);
			}
			else
			{
				return new Rectangle(editContentRect.Left, 
					GetLineY(lnStart) - 1, editContentRect.Width, 
					Math.Min(editContentRect.Bottom, 
					(lnEnd - lnStart + 1) * edit.LineHeight + 1));
			}
		}

		/// <summary>
		/// Adjusts the viewport line.
		/// </summary>
		/// <returns></returns>
		internal bool AdjustViewportLine()
		{
			int oldValue = ViewportFirstLine;
			int lnTemp = editData.GetLocatedLine(edit.CurrentLine);
			int maxLn = MaxViewportFirstLine;
			if (lnTemp < ViewportFirstLine)
			{
				ViewportFirstLine = Math.Min(lnTemp, maxLn);
			}
			else if (lnTemp > ViewportLastLine)
			{
				ViewportFirstLine = editData.GetVisibleLineByOffset(edit.
					CurrentLine, - ViewportLineCount + 1);
			}
			else if (lnTemp > maxLn)
			{
				ViewportFirstLine = Math.Min(ViewportFirstLine, maxLn);
			}
			return (ViewportFirstLine != oldValue);
		}

		/// <summary>
		/// Adjusts the viewport char.
		/// </summary>
		internal bool AdjustViewportChar()
		{
			int xTemp;
			int yTemp;
			GetPoint(edit.CurrentLineChar, out xTemp, out yTemp);
			float strWidth = 0;
			string strLine = editData.GetStringObject(edit.CurrentLine);
			if (strLine != string.Empty)
			{
				Graphics g = this.CreateGraphics();
				strWidth = GetStringWidth(g, strLine.Substring(0, 
					Math.Min(edit.CurrentChar - 1, strLine.Length)));
			}
			int oldValue = ViewportFirstColumn;
			if (xTemp <= editTextRect.Left + 2)
			{
				ViewportFirstColumn = Math.Max(1, GetColumnCountFromWidth(strWidth) - 2);
				return (ViewportFirstColumn != oldValue);
			}
			else if (xTemp >= editTextRect.Right - (int)edit.CharWidth['x'])
			{
				ViewportFirstColumn = Math.Max(1, GetColumnCountFromWidth(strWidth)
					- ViewportColumnCount + 10);
				return (ViewportFirstColumn != oldValue);
			}
			return false;
		}

		/// <summary>
		/// Adjusts the viewport if empty lines are presented after the last line.
		/// </summary>
		internal bool RemoveViewportExtraLines()
		{
			int maxLn = MaxViewportFirstLine;
			if (editViewportLineColumn.L > maxLn)
			{
				editViewportLineColumn.L = maxLn;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the Y location of the starting point of a line.
		/// </summary>
		/// <param name="ln"></param>
		/// <returns></returns>
		internal int GetLineY(int ln)
		{
			int lnTemp;
			if (ln < 1)
			{
				lnTemp = 1;
			}
			else if (ln > edit.LineCount)
			{
				lnTemp = edit.LineCount;
			}
			else
			{
				lnTemp = ln;
			}
			if (!edit.OutliningEnabled)
			{
				return (int)((lnTemp - editViewportLineColumn.L) 
					* edit.LineHeight + editViewportLocation.Y);
			}
			int tempY = editViewportLocation.Y;
			if (lnTemp > editViewportLineColumn.L)
			{
				tempY += editData.GetOccupiedLineCount(editViewportLineColumn.L 
					+ 1, lnTemp) * edit.LineHeight;
			}
			else if (lnTemp < editViewportLineColumn.L)
			{
				tempY -= editData.GetOccupiedLineCount(lnTemp,
					editViewportLineColumn.L - 1) * edit.LineHeight;
			}
			return tempY;
		}

		/// <summary>
		/// Gets the line number from the specified Y coordinate.
		/// </summary>
		/// <param name="ptY">The Y coordinate for which the line number
		/// is desired.</param>
		/// <returns>The line number for the specified Y coordinate.
		/// </returns>
		internal int GetLineFromY(int ptY)
		{
			int visibleLines;
			int pixelOffset = ptY - editViewportLocation.Y;
			if (pixelOffset < 0)
			{
				visibleLines = pixelOffset/edit.LineHeight - 1;
			}
			else
			{
				visibleLines = pixelOffset/edit.LineHeight;
			}
			if (!edit.OutliningEnabled)
			{
				int ln = visibleLines + editViewportLineColumn.L;
				if (ln < 1)
				{
					return 1;
				}
				else if (ln > edit.LineCount)
				{
					return edit.LineCount;
				}
				else
				{
					return ln;
				}
			}
			else
			{
				int lnTemp = editViewportLineColumn.L;
				if (visibleLines > 0)
				{
					for (int i = 0; i < visibleLines; i++)
					{
						lnTemp = editData.GetNextVisibleLine(lnTemp);
					}
				}
				else
				{
					for (int i = 0; i > visibleLines; i--)
					{
						lnTemp = editData.GetPreviousVisibleLine(lnTemp);
					}
				}
				return lnTemp;
			}
		}

		/// <summary>
		/// Moves the caret right by one char.
		/// </summary>
		protected void CharRightCaretPosition()
		{
			edit.OriginalX = -1;
			if (edit.VirtualSpace)
			{
				edit.CurrentChar += 1;
			}
			else
			{
				edit.CurrentLineChar = edit.GetNextLineChar(edit.CurrentLineChar);
			}
		}

		/// <summary>
		/// Moves the caret right by one char.
		/// </summary>
		internal void CharRight()
		{
			CharRightCaretPosition();
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret right by one char and select the passed range.
		/// </summary>
		internal void CharRightExtend()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				CharRightCaretPosition();
				edit.ExtendSelection(edit.CurrentLineChar);
				ScrollToViewCaret();
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				CharRightCaretPosition();
				edit.Select(lcOld, edit.CurrentLineChar);
				ScrollToViewCaret();
			}
		}

		/// <summary>
		/// Moves the caret left by one char.
		/// </summary>
		protected void CharLeftCaretPosition()
		{
			edit.OriginalX = -1;
			if (edit.VirtualSpace)
			{
				if (edit.CurrentChar > 1)
				{
					edit.CurrentChar -= 1;
				}
			}
			else
			{
				edit.CurrentLineChar = edit.GetPreviousLineChar(edit.CurrentLineChar);
			}
		}

		/// <summary>
		/// Moves the caret left by one char.
		/// </summary>
		internal void CharLeft()
		{
			CharLeftCaretPosition();
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret left by one char and selects the passed range.
		/// </summary>
		internal void CharLeftExtend()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				CharLeftCaretPosition();
				edit.ExtendSelection(edit.CurrentLineChar);
				ScrollToViewCaret();
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				CharLeftCaretPosition();
				edit.Select(lcOld, edit.CurrentLineChar);
				ScrollToViewCaret();
			}
		}

		/// <summary>
		/// Moves the caret left by one word.
		/// </summary>
		internal void WordLeft()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				edit.UnSelect();
			}
			WordLeftCaretPosition();
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret left by one word.
		/// </summary>
		internal void WordLeftCaretPosition()
		{
			edit.OriginalX = -1;
			if (edit.CurrentChar == 1)
			{
				if (edit.CurrentLine > 1)
				{
					edit.CurrentLineChar = edit.GetWordLocationRange(
						edit.CurrentLine - 1, 
						edit.GetLineLengthPlusOne(edit.CurrentLine - 1)).Start;
				}
			}
			else
			{
				EditLocationRange lcr = edit.GetCurrentWordLocationRange();
				if (lcr.Start.C == edit.CurrentChar)
				{
					edit.CurrentLineChar = edit.GetWordLocationRange(
						lcr.Start.L, lcr.Start.C - 1).Start;
				}
				else
				{
					edit.CurrentLineChar = lcr.Start;
				}
			}
		}

		/// <summary>
		/// Moves the caret left by one word and selects the passed range.
		/// </summary>
		internal void WordLeftExtend()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				WordLeftCaretPosition();
				edit.ExtendSelection(edit.CurrentLineChar);
				ScrollToViewCaret();
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				WordLeftCaretPosition();
				edit.Select(lcOld, edit.CurrentLineChar);
				ScrollToViewCaret();
			}
		}

		/// <summary>
		/// Moves the caret right by one word.
		/// </summary>
		internal void WordRight()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				edit.UnSelect();
			}
			WordRightCaretPosition();
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret right by one word.
		/// </summary>
		internal void WordRightCaretPosition()
		{
			if (edit.CurrentChar == edit.GetLineLengthPlusOne(edit.CurrentLine))
			{
				if (edit.CurrentLine < edit.LineCount)
				{
					edit.CurrentLineChar = edit.GetWordLocationRange(
						edit.CurrentLine + 1, 1).Start;
				}
			}
			else
			{
				EditLocationRange lcr = edit.GetCurrentWordLocationRange();
				if (lcr.End.C == edit.CurrentChar)
				{
					edit.CurrentLineChar = edit.GetWordLocationRange(
						lcr.Start.L, lcr.End.C + 1).End;
				}
				else
				{
					edit.CurrentLineChar = lcr.End;
				}
			}
		}

		/// <summary>
		/// Moves the caret right by one word and selects the passed range.
		/// </summary>
		internal void WordRightExtend()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				WordRightCaretPosition();
				edit.ExtendSelection(edit.CurrentLineChar);
				ScrollToViewCaret();
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				WordRightCaretPosition();
				edit.Select(lcOld, edit.CurrentLineChar);
				ScrollToViewCaret();
			}
		}

		/// <summary>
		/// Moves the caret up by one line.
		/// </summary>
		internal void LineUp()
		{
			SaveCurrentX();
			if (edit.CurrentLine > 1) 
			{
				edit.CurrentLine = editData.GetVisibleLineByOffset(edit.CurrentLine, -1);
				edit.CurrentChar = GetCharFromX(edit.OriginalX);
			}
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret up by one line and selects the passed range.
		/// </summary>
		internal void LineUpExtend()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				LineUp();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				LineUp();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
		}

		/// <summary>
		/// Moves the caret down by one line.
		/// </summary>
		internal void LineDown()
		{
			SaveCurrentX();
			if (edit.CurrentLine < edit.LineCount) 
			{
				edit.CurrentLine = editData.GetVisibleLineByOffset(edit.CurrentLine, 1);
				edit.CurrentChar = GetCharFromX(edit.OriginalX);
			}
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret down by one line and selects the passed range.
		/// </summary>
		internal void LineDownExtend()
		{
			if (edit.ISearch)
			{
				AbortIncrementalSearch();
			}
			if (edit.HasSelection)
			{
				LineDown();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				LineDown();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
		}

		/// <summary>
		/// Scrolls the contents up by one page.
		/// </summary>
		internal void PageUp()
		{
			SaveCurrentX();
			edit.CurrentLine = editData.GetVisibleLineByOffset(edit.CurrentLine, 
				- ViewportLineCount);
			ViewportFirstLine = editData.GetVisibleLineByOffset(ViewportFirstLine, 
				- ViewportLineCount);
			edit.CurrentChar = GetCharFromX(edit.OriginalX);
			Redraw();
		}

		/// <summary>
		/// Scrolls the contents up by one page and selects the passed range.
		/// </summary>
		internal void PageUpExtend()
		{
			if (edit.HasSelection)
			{
				PageUp();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				PageUp();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
			Redraw();
		}

		/// <summary>
		/// Scrolls the contents down by one page.
		/// </summary>
		internal void PageDown()
		{
			SaveCurrentX();
			edit.CurrentLine = editData.GetVisibleLineByOffset(edit.CurrentLine, 
				ViewportLineCount);
			ViewportFirstLine = Math.Min(MaxViewportFirstLine, 
				editData.GetVisibleLineByOffset(ViewportFirstLine, 
				ViewportLineCount));
			edit.CurrentChar = GetCharFromX(edit.OriginalX);
			Redraw();
		}

		/// <summary>
		/// Scrolls the contents down by one page and selects the passed range.
		/// </summary>
		internal void PageDownExtend()
		{
			if (edit.HasSelection)
			{
				PageDown();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				PageDown();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
			Redraw();
		}

		/// <summary>
		/// Moves the caret to the start of the line.
		/// </summary>
		internal void LineStart()
		{
			edit.OriginalX = -1;
			int firstNonSpace = edit.GetFirstNonSpaceChar(edit.CurrentLine);
			if (edit.CurrentChar > firstNonSpace)
			{
				edit.CurrentChar = firstNonSpace;
			}
			else if (edit.CurrentChar == 1)
			{
				edit.CurrentChar = firstNonSpace;
			}
			else
			{
				edit.CurrentChar = 1;
			}
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret to the start of the line and selects the passed range.
		/// </summary>
		internal void LineStartExtend()
		{
			if (edit.HasSelection)
			{
				LineStart();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				LineStart();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
		}

		/// <summary>
		/// Moves the caret to the end of the line.
		/// </summary>
		internal void LineEnd()
		{
			edit.OriginalX = -1;
			edit.CurrentChar = edit.GetLineLengthPlusOne(edit.CurrentLine);
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret to the end of the line and selects the 
		/// passed range.
		/// </summary>
		internal void LineEndExtend()
		{
			if (edit.HasSelection)
			{
				LineEnd();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				LineEnd();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
		}

		/// <summary>
		/// Moves the caret to the top of the current page.
		/// </summary>
		internal void PageTop()
		{
			SaveCurrentX();
			if (edit.HasSelection)
			{
				edit.UnSelect();
			}
			edit.CurrentLine = ViewportFirstLine;
			edit.CurrentChar = GetCharFromX(edit.OriginalX);
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret to the top of the current page and selects 
		/// the passed range.
		/// </summary>
		internal void PageTopExtend()
		{
			SaveCurrentX();
			if (edit.HasSelection)
			{
				edit.CurrentLine = ViewportFirstLine;
				edit.CurrentColumn = GetCharFromX(edit.OriginalX);
				ScrollToViewCaret();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				edit.CurrentLine = ViewportFirstLine;
				edit.CurrentColumn = GetCharFromX(edit.OriginalX);
				ScrollToViewCaret();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
		}

		/// <summary>
		/// Moves the caret to the bottom of the current page.
		/// </summary>
		internal void PageBottom()
		{
			SaveCurrentX();
			if (edit.HasSelection)
			{
				edit.UnSelect();
			}
			edit.CurrentLine = ViewportLastLine;
			edit.CurrentColumn = GetCharFromX(edit.OriginalX);
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret to the bottom of the current page and selects 
		/// the passed range.
		/// </summary>
		internal void PageBottomExtend()
		{
			SaveCurrentX();
			if (edit.HasSelection)
			{
				edit.CurrentLine = ViewportLastLine;
				edit.CurrentColumn = GetCharFromX(edit.OriginalX);
				ScrollToViewCaret();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				edit.CurrentLine = ViewportLastLine;
				edit.CurrentChar = GetCharFromX(edit.OriginalX);
				ScrollToViewCaret();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
		}

		/// <summary>
		/// Moves the caret to the first character of the document.
		/// </summary>
		internal void DocumentStart()
		{
			edit.OriginalX = -1;
			if (edit.HasSelection)
			{
				edit.UnSelect();
			}
			edit.CurrentLineChar = edit.FirstLineChar;
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret to the first character of the document and 
		/// selects the passed range.
		/// </summary>
		internal void DocumentStartExtend()
		{
			if (edit.HasSelection)
			{
				edit.CurrentLineChar = edit.FirstLineChar;
				ScrollToViewCaret();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				edit.CurrentLineChar = edit.FirstLineChar;
				ScrollToViewCaret();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
		}

		/// <summary>
		/// Moves the caret to after the last character of the document.
		/// </summary>
		internal void DocumentEnd()
		{
			edit.OriginalX = -1;
			if (edit.HasSelection)
			{
				edit.UnSelect();
			}
			edit.CurrentLineChar = edit.LastLineChar;
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret to after the last character of the document 
		/// and selects the passed range.
		/// </summary>
		internal void DocumentEndExtend()
		{
			if (edit.HasSelection)
			{
				edit.CurrentLineChar = edit.LastLineChar;
				ScrollToViewCaret();
				edit.ExtendSelection(edit.CurrentLineChar);
			}
			else
			{
				EditLocation lcOld = new EditLocation(edit.CurrentLineChar);
				edit.CurrentLineChar = edit.LastLineChar;
				ScrollToViewCaret();
				edit.Select(lcOld, edit.CurrentLineChar);
			}
		}

		#endregion

		#region Methods Related to Displaying White Spaces and Brace Matching.

		/// <summary>
		/// Draws the visible symbol for Tab in the specified rectangle.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="rect"></param>
		protected void DrawVisibleTab(Graphics g, Rectangle rect)
		{
			//			if (edit.VisibleWhiteSpaceBackColor != edit.TextBackColor)
			//			{
			//				g.FillRectangle(new 
			//					SolidBrush(edit.VisibleWhiteSpaceBackColor), rect);
			//			}
			Pen pen = new Pen(edit.VisibleWhiteSpaceForeColor, 1);
			g.DrawLine(pen, rect.Left, rect.Top + rect.Height/2, 
				rect.Left + 7, rect.Top + rect.Height/2);
			g.DrawLine(pen, rect.Left + 7 / 2 + 1, rect.Top 
				+ rect.Height/2 - 3, rect.Left + 7, rect.Top + rect.Height/2);
			g.DrawLine(pen, rect.Left + 7 / 2 + 1, rect.Top + rect.Height/2 
				+ 3, rect.Left + 7, rect.Top + rect.Height/2);
		}

		/// <summary>
		/// Draws the visible symbol for space in the specified rectangle.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="rect">The bounds for the symbol.</param>
		protected void DrawVisibleSpace(Graphics g, Rectangle rect)
		{
			//			if (edit.VisibleWhiteSpaceBackColor != edit.TextBackColor)
			//			{
			//				g.FillRectangle(new 
			//					SolidBrush(edit.VisibleWhiteSpaceBackColor), rect);
			//			}
			Rectangle rectDot = new Rectangle(rect.Left + rect.Width/2 
				- 1, rect.Top + rect.Height/2 - 1, 2, 2);
			g.FillRectangle(new 
				SolidBrush(edit.VisibleWhiteSpaceForeColor), rectDot);
		}

		/// <summary>
		/// Draws the visible symbol for the End of File location in the 
		/// specified rectangle.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="rect">The bounds for the symbol.</param>
		protected void DrawVisibleEOF(Graphics g, Rectangle rect)
		{
			//			if (edit.VisibleWhiteSpaceBackColor != edit.TextBackColor)
			//			{
			//				g.FillRectangle(new 
			//					SolidBrush(edit.VisibleWhiteSpaceBackColor), rect);
			//			}
			Pen pen = new Pen(edit.VisibleWhiteSpaceForeColor, 1);
			g.DrawLine(pen, rect.Left + 2, rect.Top + 6, 
				rect.Right - 2, rect.Top + 6);
			g.DrawLine(pen, rect.Right - 2, rect.Top + 6, 
				rect.Right - 2, rect.Top + rect.Height - 6);
			g.DrawLine(pen, rect.Right - 2, rect.Top + rect.Height - 6, 
				rect.Left + 2, rect.Top + rect.Height - 6);
			g.DrawLine(pen, rect.Left + 2, rect.Top + rect.Height - 6, 
				rect.Left + 2, rect.Top + 6);
		}

		/// <summary>
		/// Draws the brace matching at the specified line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln"></param>
		internal void DrawBraceMatching(Graphics g, int ln)
		{
			if ((edit.BraceMatchingBegin.Start.L == ln) 
				&& (edit.BraceMatchingBegin.End.L == ln))
			{
				if (edit.IsValidLineChar(edit.BraceMatchingBegin.Start)
					&& edit.IsValidLineChar(edit.BraceMatchingBegin.End))
				{
					RectangleF boundRect = 
						GetBoundRectangle(g, ln, edit.BraceMatchingBegin.Start.C, 
						edit.BraceMatchingBegin.End.C);
					Pen pen = new Pen(new SolidBrush(edit.BraceMatchingForeColor), 1);
					g.DrawRectangle(pen, (int)boundRect.Left, (int)boundRect.Top,
						(int)boundRect.Width, (int)boundRect.Height - 1);
				}
			}
			if ((edit.BraceMatchingEnd.Start.L == ln) 
				&& (edit.BraceMatchingEnd.End.L == ln))
			{
				if (edit.IsValidLineChar(edit.BraceMatchingEnd.Start)
					&& edit.IsValidLineChar(edit.BraceMatchingEnd.End))
				{
					RectangleF boundRect = 
						GetBoundRectangle(g, ln, edit.BraceMatchingEnd.Start.C, 
						edit.BraceMatchingEnd.End.C);
					Pen pen = new Pen(new SolidBrush(edit.BraceMatchingForeColor), 1);
					g.DrawRectangle(pen, (int)boundRect.Left, (int)boundRect.Top,
						(int)boundRect.Width, (int)boundRect.Height - 1);
				}
			}
		}

		#endregion 

		#region Methods Related to Drawing Wave Lines

		/// <summary>
		/// Draws the wave lines at the specified line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln"></param>
		internal void DrawWaveLine(Graphics g, int ln)
		{
			short [] id = editData.LineList[ln-1].IndicatorData;
			int startIndex = editData.GetWaveLineDataIndex(id);
			if (startIndex != -1)
			{
				for (int i = startIndex; i < id.Length; i +=3)
				{
					DrawWaveLine(g, ln, id[i], id[i+1], 
						editSettings.GetColorGroupForeColor(id[i+2]));
				}
			}
		}

		/// <summary>
		/// Draws a wave line for the specified character range at the 
		/// specified line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln"></param>
		/// <param name="chStart"></param>
		/// <param name="chEnd"></param>
		/// <param name="clr"></param>
		internal void DrawWaveLine(Graphics g, int ln, 
			int chStart, int chEnd, Color clr)
		{
			if ((editData.IsValidLineChar(ln, chStart)) 
				&& (editData.IsValidLineChar(ln, chEnd)))
			{
				RectangleF boundRect = 
					GetBoundRectangle(g, ln, chStart, chEnd + 1);
				Pen pen = new Pen(new SolidBrush(clr), 1);
				DrawWaveLine(g, pen, (int)(boundRect.Left), 
					(int)(boundRect.Bottom - 2),
					(int)(boundRect.Right), 
					(int)(boundRect.Bottom - 2));
			}
		}

		/// <summary>
		/// Draws a wave line between the specified starting and ending points.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="pen">The pen to draw with.</param>
		/// <param name="startX">The starting x coordinate for the wave line.</param>
		/// <param name="startY">The starting y coordinate for the wave line.</param>
		/// <param name="endX">The ending x coordinate of the wave line.</param>
		/// <param name="endY">The ending y coordinate of the wave line.</param>
		internal void DrawWaveLine(Graphics g, Pen pen, 
			int startX, int startY, int endX, int endY)
		{
			for (int i = startX; i < endX - 1; i++)
			{
				switch ((i - startX)%4)
				{
					case 0:
						g.DrawLine(pen, i, startY, i + 1, startY + 1);
						break;
					case 1:
						g.DrawLine(pen, i, startY + 1, i + 1, startY);
						break;
					case 2:
						g.DrawLine(pen, i, startY, i + 1, startY - 1);
						break;
					case 3:
						g.DrawLine(pen, i, startY - 1, i + 1, startY);
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Gets the bounding rectangle for the specifed character range 
		/// of a line.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="ln">The line of the bounding rectangle.</param>
		/// <param name="chStart">The starting char of the bounding rectangle.</param>
		/// <param name="chEnd">The ending char of the bounding rectangle.</param>
		/// <returns>The bounding rectangle for the specifed character range 
		/// of a line.</returns>
		internal RectangleF GetBoundRectangle(Graphics g, int ln, 
			int chStart, int chEnd)
		{
			float fStart = editViewportLocation.X + GetStringWidth(g, 
				editData.GetStringObject(ln).Substring(0, chStart - 1));
			float fEnd = editViewportLocation.X + GetStringWidth(g, 
				editData.GetStringObject(ln).Substring(0, chEnd - 1)); 
			return new RectangleF(fStart, GetLineY(ln), fEnd-fStart, edit.LineHeight);
		}

		/// <summary>
		/// Saves the absolut X position of the caret.
		/// </summary>
		internal void SaveCurrentX()
		{
			if (edit.OriginalX == -1)
			{
				edit.OriginalX = GetCurrentX();
			}
		}

		/// <summary>
		/// Gets the absolut X position of the caret.
		/// </summary>
		/// <returns></returns>
		internal float GetCurrentX()
		{
			int xTemp;
			int yTemp;
			GetPoint(edit.CurrentLineChar, out xTemp, out yTemp);
			return xTemp - editViewportLocation.X;
		}

		/// <summary>
		/// Gets the char index from the specified absolute X position.
		/// </summary>
		/// <param name="X">The absolute X position from which the char index 
		/// is to be obtained.</param>
		/// <returns>The char index for the absolute X position.</returns>
		internal int GetCharFromX(float X)
		{
			int C;
			int XTemp = (int)(X + editViewportLocation.X);
			string strLine = editData.GetStringObject(edit.CurrentLine);
			if (!edit.OutliningEnabled)
			{
				GetChar(strLine, XTemp, out C);
			}
			else
			{
				EditOutlining otln = editData.GetLeafOutlining(edit.CurrentLine);
				if (!otln.Collapsed)
				{
					GetChar(strLine, XTemp, out C);
				}
				else
				{
					int chStart = -1;
					int chEnd = -1;
					string strTemp;
					strTemp = otln.GetCollapsedString(ref chStart, ref chEnd);
					strLine = strLine.Substring(0, chStart - 1) + strTemp + 
						editData.GetStringObject(otln.EndLine).Substring(otln.End.C - 1);
					GetChar(strLine, XTemp, out C);
					if (C > chStart) 
					{
						if (C < (chStart + strTemp.Length))
						{
							C = otln.End.C;
						}
						else
						{
							C = otln.End.C + C - (chStart + strTemp.Length);
						}
					}
				}
			}
			return C;
		}

		/// <summary>
		/// Gets the column index from the char index for the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		/// <returns></returns>
		internal int GetColumnFromChar(int ln, int ch)
		{
			if (!editData.IsValidLine(ln))
			{
				return 1;
			}
			if (!edit.OutliningEnabled)
			{
				return GetColumnFromCharNoOutlining(ln, ch);
			}
			else
			{
				EditOutlining otln = editData.GetLeafOutlining(ln);
				if (otln.IsRoot)
				{
					return GetColumnFromCharNoOutlining(ln, ch);
				}
				else if (!otln.Collapsed)
				{
					return GetColumnFromCharNoOutlining(ln, ch);
				}
				else
				{
					int chStart = -1;
					int chEnd = -1;
					string strLine = editData.LineList[otln.Start.L-1].LineString;
					string strTemp = 
						otln.GetCollapsedString(ref chStart, ref chEnd);
					if (otln.Start.GreaterThanOrEqualTo(ln, ch))
					{
						return GetColumnCount(strLine.Substring(0, ch - 1));
					}
					else if (otln.End.LessThan(ln, ch))
					{
						string strLine2 = editData.LineList[otln.EndLine-1].LineString.
							Substring(otln.End.C - 1, ch - otln.End.C);
						return GetColumnCount(strLine.Substring(0, chStart - 1)
							+ strTemp + strLine2);
					}
					else
					{
						return GetColumnCount(strLine.Substring(0, chStart - 1)
							+ strTemp);
					}
				}
			}
		}

		/// <summary>
		/// Gets the column index from the char index for the specified line 
		/// without considering outlining.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="ch"></param>
		/// <returns></returns>
		internal int GetColumnFromCharNoOutlining(int ln, int ch)
		{
			if (ch == 1)
			{
				return 1;
			}
			int lnLengthPlusOne = editData.GetLineLengthPlusOne(ln);
			string strTemp;
			if (ch <= lnLengthPlusOne)
			{
				strTemp = editData.GetStringObject(ln).Substring(0, ch - 1);
			}
			else
			{
				strTemp = editData.GetStringObject(ln)
					+ new string(' ', ch - lnLengthPlusOne);
			}
			return GetColumnCount(strTemp);
		}

		/// <summary>
		/// Gets the char index from the column index at the specified line.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		internal int GetCharFromColumn(int ln, int col)
		{
			if (!editData.IsValidLine(ln))
			{
				return 1;
			}
			if (!edit.OutliningEnabled)
			{
				return GetCharFromColumnNoOutlining(ln, col);
			}
			else
			{
				EditOutlining otln = editData.GetLeafOutlining(ln);
				if (otln.IsRoot)
				{
					return GetCharFromColumnNoOutlining(ln, col);
				}
				else if (!otln.Collapsed)
				{
					return GetCharFromColumnNoOutlining(ln, col);
				}
				else
				{
					int chStart = -1;
					int chEnd = -1;
					string strLine = editData.LineList[otln.Start.L-1].LineString;
					string strTemp = 
						otln.GetCollapsedString(ref chStart, ref chEnd);
					string strLine1 = editData.LineList[otln.Start.L-1].LineString.
						Substring(0, chStart - 1);
					string strLine2 = editData.LineList[otln.EndLine-1].LineString.
						Substring(otln.End.C - 1);
					string strTotal = strLine + strTemp + strLine2;
					int col1 = GetColumnCount(strLine1);
					int col2 = GetColumnCount(strLine1 + strTemp);
					if (ln == otln.StartLine)
					{
						if (col < col1)
						{
							return GetCharFromColumnForString(col, strLine1);
						}
						else
						{
							return chStart;
						}
					}
					else if (ln == otln.EndLine)
					{
						if (col > col2)
						{
							return GetCharFromColumnForString(col, strTotal) 
								- chEnd + otln.End.C;
						}
						else
						{
							return otln.End.C;
						}
					}
					else
					{
						return 1;
					}
				}
			}
		}

		/// <summary>
		/// Gets the char index from the column index at the specified line
		/// without considering outlining.
		/// </summary>
		/// <param name="ln"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		internal int GetCharFromColumnNoOutlining(int ln, int col)
		{
			if (col == 1)
			{
				return 1;
			}
			return GetCharFromColumnForString(col, editData.LineList[ln-1].LineString);
		}

		/// <summary>
		/// Gets the char index at the specified column index for the 
		/// specified string.
		/// </summary>
		/// <param name="col"></param>
		/// <param name="str"></param>
		/// <returns></returns>
		internal int GetCharFromColumnForString(int col, string str)
		{
			int strLength = str.Length;
			if (strLength == 0)
			{
				return 1;
			}
			int colTemp = 1;
			for (int i = 0; i < strLength; i++)
			{
				if (colTemp == col)
				{
					return i + 1;
				}
				else if (colTemp > col)
				{
					return i;
				}
				if (str[i] != '\t')
				{
					colTemp++;
				}
				else
				{
					colTemp += edit.TabSize - (colTemp - 1)%edit.TabSize;
				}
			}
			return strLength + 1;
		}

		/// <summary>
		/// Gets a line/column from a line/char.
		/// </summary>
		/// <param name="lc"></param>
		/// <returns></returns>
		internal EditLocation LineColumnFromLineChar(EditLocation lc)
		{
			if (!editData.IsValidLineChar(lc.L, lc.C))
			{
				return EditLocation.None;
			}
			int col = GetColumnFromChar(lc.L, lc.C);
			if (col == -1)
			{
				return EditLocation.None;
			}
			else
			{
				return new EditLocation(lc.L, col);
			}
		}

		/// <summary>
		/// Gets a line/char from a line/column.
		/// </summary>
		/// <param name="lc"></param>
		/// <returns></returns>
		internal EditLocation LineCharFromLineColumn(EditLocation lc)
		{
			if (!IsValidLineColumn(lc.L, lc.C))
			{
				return EditLocation.None;
			}
			int ch = GetCharFromColumn(lc.L, lc.C);
			if (ch == -1)
			{
				return EditLocation.None;
			}
			else
			{
				return new EditLocation(lc.L, ch);
			}
		}

		#endregion

		#region Methods Related to Outlining

		/// <summary>
		/// Expands the collapsed outlining at the specified location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		/// <returns>true if an outlining is expanded; otherwise, false.
		/// </returns>
		internal bool ExpandOutlining(int ln, int ch)
		{
			EditOutlining otln = editData.GetCollapsedOutlining(ln, ch);
			if (otln != null)
			{
				edit.UnSelect();
				otln.Collapsed = false;
				UpdateVScrollBar();
				Redraw();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Draws the collapsed outlining at the specified location.
		/// </summary>
		/// <param name="g">The graphics object to draw on.</param>
		/// <param name="col">The colunm of the outlining.</param>
		/// <param name="xPos">The current X position of outlining in pixels.
		/// </param>
		/// <param name="yPos">The current pixel Y position of outlining in 
		/// pixels.</param>
		/// <param name="str">The string to draw for the outlining.</param>
		/// <param name="backColor">The background color of the outlining.
		/// </param>
		/// <returns>true if an outlining is drawn; otherwise, false.</returns>
		internal bool DrawCollapsedOutlining(Graphics g, ref int col, 
			ref float xPos, float yPos, string str, Color backColor)
		{
			if (str.Length == 0)
			{
				return false;
			}
			Color foreColor = edit.SelectionMarginForeColor;
			float xPosOld = xPos;
			DrawColorString(g, ref col, ref xPos, yPos, str, 
				foreColor, backColor, true,0);
			Pen pen = new Pen(foreColor, 1);
			Rectangle rectBound = new Rectangle((int)xPosOld, (int)yPos, 
				(int)(xPos - xPosOld), edit.LineHeight - 1);
			g.DrawRectangle(pen, rectBound);
			return true;
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
		/// Gets or sets a value indicating whether to show the splitter.
		/// </summary>
		internal bool SplitterButtonVisible
		{
			get
			{
				return bSplitterButtonVisible;
			}
			set
			{
				if (bSplitterButtonVisible != value)
				{
					bSplitterButtonVisible = value;
					UpdateControls();
				}
			}
		}

		/// <summary>
		/// Gets the starting line of the viewport.
		/// </summary>
		internal int ViewportFirstLine
		{
			get
			{
				return editViewportLineColumn.L;
			}
			set
			{
				if (editViewportLineColumn.L != value)
				{
					editViewportLineColumn.L = value;
					UpdateVScrollBarValue();
					Redraw();
				}
			}
		}

		/// <summary>
		/// Gets the subline of the starting line of the viewport.
		/// </summary>
		internal int ViewportFirstLineSubLine
		{
			get
			{
				return editViewportFirstLineSubLine;
			}
			set
			{
				if (editViewportFirstLineSubLine != value)
				{
					editViewportFirstLineSubLine = value;
					UpdateVScrollBarValue();
					Redraw();
				}
			}	
		}

		/// <summary>
		/// Gets the ending line of the viewport.
		/// </summary>
		internal int ViewportLastLine
		{
			get
			{
				return editData.GetVisibleLineByOffset(ViewportFirstLine, 
					ViewportLineCount - 1);
			}
		}

		/// <summary>
		/// The maximum value for the first visible line.
		/// </summary>
		internal int MaxViewportFirstLine
		{
			get
			{
				return editData.GetVisibleLineByOffset(editData.LastVisibleLine, 
					- ViewportLineCount + 1);
			}
		}

		/// <summary>
		/// Gets the total number of lines in the viewport.
		/// </summary>
		internal int ViewportLineCount
		{
			get
			{
				return Math.Max(1, editTextRect.Height/edit.LineHeight);
			}
		}

		/// <summary>
		/// Gets or sets the starting column of the viewport.
		/// </summary>
		internal int ViewportFirstColumn
		{
			get
			{
				return editViewportLineColumn.C;
			}
			set
			{
				if (editViewportLineColumn.C != value)
				{
					editViewportLineColumn.C = value;
					UpdateViewportX();
					UpdateHScrollBarValue();
					Redraw();
				}
			}
		}

		/// <summary>
		/// Gets the total number of columns in the viewport.
		/// </summary>
		internal int ViewportColumnCount
		{
			get
			{
				return GetColumnCountFromWidth(editTextRect.Width);
			}
		}

		/// <summary>
		/// Gets or sets the line/column location of the viewport.
		/// </summary>
		internal EditLocation ViewportLineColumn
		{
			get
			{
				return editViewportLineColumn;
			}
			set
			{
				if (editViewportLineColumn != value)
				{
					BeginUpdate();
					ViewportFirstLine = value.L;
					ViewportFirstColumn = value.C;
					EndUpdate();
					Redraw();
				}
			}
		}

		/// <summary>
		/// Gets or sets the panning offset of the viewport in Y direction.
		/// </summary>
		internal int PanOffsetY
		{
			get
			{
				return editPanOffset.Y;
			}
			set
			{
				if (editPanOffset.Y != value)
				{
					editPanOffset.Y = value;
					UpdateViewportY();
				}
			}
		}

		#endregion
	}
}
