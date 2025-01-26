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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;

namespace Syncfusion.Windows.Forms.EditCustom
{
	/// <summary>
	/// The EditControl class is the main class of the Syntax Coloring Edit in
	/// the Essential Edit Library.
	/// </summary>
	[
	ToolboxItem(true),
	ToolboxBitmap(typeof(EditControl), "Resources.EDIT.BMP")
	]
	[LicenseProviderAttribute("Syncfusion.Licensing.FusionLicenseProvider, _FusionLic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=632609b4d040f6b4")]
	public class EditControl : System.Windows.Forms.UserControl
	{
		#region Data Members
		/// <summary>
		/// Boolean value as to whether or not copy is supported without selected text.
		/// </summary>
		private bool bCopyWithoutSelection;
		/// <summary>
		/// Boolean value as to whether or not the splitter is displayed.
		/// </summary>
		private bool bShowSplitterButton = true;
		
		/// <summary>
		/// Boolean value as to whether or not a FileDrop is allowed
		/// </summary>
		private bool bFileDropAllowed;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// The EditSettings object that contains syntax coloring settings.
		/// </summary>
		private EditSettings editSettings;
		/// <summary>
		/// The EditData object that contains text and color information.
		/// </summary>
		private EditData editData;

		/// <summary>
		/// The statusbar that tracks line, column, and char indexes.
		/// </summary>
		private StatusBar editStatusBar = new StatusBar();
		/// <summary>
		/// The array of panels in the status bar.
		/// </summary>
		private StatusBarPanel [] editStatusBarPanels = new StatusBarPanel[4];
		/// <summary>
		/// The array indicating whether text is automatically set in 
		/// status bar panels.
		/// </summary>
		private bool [] bCustomPanelText = new Boolean[4];

		/// <summary>
		/// The top EditView object.
		/// </summary>
		private EditView editViewTop;
		/// <summary>
		/// The bottom EditView object.
		/// </summary>
		private EditView editViewBottom;
		/// <summary>
		/// The splitter between the top and bottom EditView objects.
		/// </summary>
		private EditArea editSplitter = new EditArea();
		/// <summary>
		/// The view that is currently active;
		/// </summary>
		private EditView editActiveView;
		/// <summary>
		/// The view that was previously active;
		/// </summary>
		private EditView editOldActiveView;

		/// <summary>
		/// The EditUndoRedo object for redo/undo management.
		/// </summary>
		private EditUndoRedo editUndoRedo = new EditUndoRedo();
		/// <summary>
		/// The options dialog object.
		/// </summary>
		private OptionsDlg editOptionsDlg;
		/// <summary>
		/// The find/replace dialog object.
		/// </summary>
		private FindReplaceDlg editFindReplaceDlg = new FindReplaceDlg();

		/// <summary>
		/// The name of the currently opened file.
		/// </summary>
		private string editCurrentFile = null;
		/// <summary>
		/// A variable indicating whether content has been modified.
		/// </summary>
		private bool bModified = false;
		/// <summary>
		/// A variable indicating whether pasting content was available.
		/// </summary>
		private bool bCanPasteOld = false;

		/// <summary>
		/// The main context menu object.
		/// </summary>
		public ContextMenu editContextMenu = null;

		/// <summary>
		/// The Edit menu item.
		/// </summary>
		private MenuItem editMenuItemEdit;
		/// <summary>
		/// The Cut menu item.
		/// </summary>
		private MenuItem editMenuItemCut;
		/// <summary>
		/// The Copy menu item.
		/// </summary>
		private MenuItem editMenuItemCopy;
		/// <summary>
		/// The Paste menu item.
		/// </summary>
		private MenuItem editMenuItemPaste;
		/// <summary>
		/// The Delete menu item.
		/// </summary>
		private MenuItem editMenuItemDelete;
		/// <summary>
		/// The Undo menu item.
		/// </summary>
		private MenuItem editMenuItemUndo;
		/// <summary>
		/// The Redo menu item.
		/// </summary>
		private MenuItem editMenuItemRedo;
		/// <summary>
		/// The Find menu item.
		/// </summary>
		private MenuItem editMenuItemFind;
		/// <summary>
		/// The Replace menu item.
		/// </summary>
		private MenuItem editMenuItemReplace;
		/// <summary>
		/// The GoTo menu item.
		/// </summary>
		private MenuItem editMenuItemGoTo;
		/// <summary>
		/// The SelectAll menu item.
		/// </summary>
		private MenuItem editMenuItemSelectAll;
		/// <summary>
		/// The InsertFileAsText menu item.
		/// </summary>
		private MenuItem editMenuItemInsertFileAsText;
		/// <summary>
		/// The TimeDate menu item.
		/// </summary>
		private MenuItem editMenuItemTimeDate;

		/// <summary>
		/// The File menu item.
		/// </summary>
		private MenuItem editMenuItemFile;
		/// <summary>
		/// The New menu item.
		/// </summary>
		private MenuItem editMenuItemNew;
		/// <summary>
		/// The Open menu item.
		/// </summary>
		private MenuItem editMenuItemOpen;
		/// <summary>
		/// The Close menu item.
		/// </summary>
		private MenuItem editMenuItemClose;
		/// <summary>
		/// The Save menu item.
		/// </summary>
		private MenuItem editMenuItemSave;
		/// <summary>
		/// The SaveAs menu item.
		/// </summary>
		private MenuItem editMenuItemSaveAs;
		/// <summary>
		/// The PrintPreview menu item.
		/// </summary>
		private MenuItem editMenuItemPrintPreview;
		/// <summary>
		/// The Print menu item.
		/// </summary>
		private MenuItem editMenuItemPrint;

		/// <summary>
		/// The Advanced menu item.
		/// </summary>
		private MenuItem editMenuItemAdvanced;
		/// <summary>
		/// The TabifySelection menu item.
		/// </summary>
		private MenuItem editMenuItemTabifySelection;
		/// <summary>
		/// The UntabifySelection menu item.
		/// </summary>
		private MenuItem editMenuItemUntabifySelection;
		/// <summary>
		/// The CommentSelection menu item.
		/// </summary>
		private MenuItem editMenuItemCommentSelection;
		/// <summary>
		/// The UncommentSelection menu item.
		/// </summary>
		private MenuItem editMenuItemUncommentSelection;
		/// <summary>
		/// The MakeUppercase menu item.
		/// </summary>
		private MenuItem editMenuItemMakeUppercase;
		/// <summary>
		/// The MakeLowercase menu item.
		/// </summary>
		private MenuItem editMenuItemMakeLowercase;
		/// <summary>
		/// The DeleteHorizontalWhiteSpace menu item.
		/// </summary>
		private MenuItem editMenuItemDeleteHorizontalWhiteSpace;
		/// <summary>
		/// The IncreaseLineIndent menu item.
		/// </summary>
		private MenuItem editMenuItemIncreaseLineIndent;
		/// <summary>
		/// The DecreaseLineIndent menu item.
		/// </summary>
		private MenuItem editMenuItemDecreaseLineIndent;
		/// <summary>
		/// The ViewWhiteSpace menu item.
		/// </summary>
		private MenuItem editMenuItemViewWhiteSpace;
		/// <summary>
		/// The IncrementalSearch menu item.
		/// </summary>
		private MenuItem editMenuItemIncrementalSearch;

		/// <summary>
		/// The Bookmark menu item.
		/// </summary>
		private MenuItem editMenuItemBookmark;
		/// <summary>
		/// The ToggleBookmark menu item.
		/// </summary>
		private MenuItem editMenuItemToggleBookmark;
		/// <summary>
		/// The NextBookmark menu item.
		/// </summary>
		private MenuItem editMenuItemNextBookmark;
		/// <summary>
		/// The PreviousBookmark menu item.
		/// </summary>
		private MenuItem editMenuItemPreviousBookmark;
		/// <summary>
		/// The ClearBookmarks menu item.
		/// </summary>
		private MenuItem editMenuItemClearBookmarks;

		/// <summary>
		/// The Outlining menu item.
		/// </summary>
		private MenuItem editMenuItemOutlining;
		/// <summary>
		/// The HideSelection menu item.
		/// </summary>
		private MenuItem editMenuItemHideSelection;
		/// <summary>
		/// The ToggleOutliningExpansion menu item.
		/// </summary>
		private MenuItem editMenuItemToggleOutliningExpansion;
		/// <summary>
		/// The ToggleAllOutlining menu item.
		/// </summary>
		private MenuItem editMenuItemToggleAllOutlining;
		/// <summary>
		/// The StopOutlining menu item.
		/// </summary>
		private MenuItem editMenuItemStopOutlining;
		/// <summary>
		/// The StopHidingCurrent menu item.
		/// </summary>
		private MenuItem editMenuItemStopHidingCurrent;
		/// <summary>
		/// The CollapseToDefinitions menu item.
		/// </summary>
		private MenuItem editMenuItemCollapseToDefinitions;
		/// <summary>
		/// The StartAutomaticOutlining menu item.
		/// </summary>
		private MenuItem editMenuItemStartAutomaticOutlining;

		/// <summary>
		/// The Options menu item.
		/// </summary>
		private MenuItem editMenuItemOptions;

		/// <summary>
		/// An empty context menu object.
		/// </summary>
		private ContextMenu editEmptyContextMenu = new ContextMenu();

		/// <summary>
		/// The current caret location.
		/// </summary>
		private EditLocation editCurrentLineChar = new EditLocation();
		/// <summary>
		/// The current text selection.
		/// </summary>
		private EditSelection editSelection = new EditSelection();

		/// <summary>
		/// The line space in pixels.
		/// </summary>
		private int editLineSpace = 1;
		/// <summary>
		/// The caret width in pixels.
		/// </summary>
		private int editCaretWidth = 2;

		/// <summary>
		/// A variable indicating whether keyboard input is in inserting mode.
		/// </summary>
		private bool bInsertMode = true;

		/// <summary>
		/// A value indicating whether splitting is in progress.
		/// </summary>
		public bool bInSplitting = false;
		/// <summary>
		/// The temporary Y position for splitter.
		/// </summary>
		private int splitterY;
		/// <summary>
		/// The minimum height for the top view to avoid auto hiding.
		/// </summary>
		private int editTopViewHidingHeight;
		/// <summary>
		/// The minimum height (that will be maintained) for the bottom view.
		/// </summary>
		private int editBottomViewMinHeight;
		/// <summary>
		/// A helper variable that stores the height of the status bar;
		/// </summary>
		private int statusBarHeight;

		/// <summary>
		/// A value indicating whether the selection margin has been changed.
		/// </summary>
		private bool bSelectionMarginChanged;
		/// <summary>
		/// The timer for selection margin redrawing.
		/// </summary>
		private System.Timers.Timer editSelectionMarginTimer 
			= new System.Timers.Timer();

		/// <summary>
		/// The PrintDocument for printing managements.
		/// </summary>
		private PrintDocument editPrintDocument = new PrintDocument();
		/// <summary>
		/// The current line for printing.
		/// </summary>
		private int editPrintCurrentLine = 1;
		/// <summary>
		/// The current page for printing.
		/// </summary>
		private int editPrintCurrentPage = 1;
		/// <summary>
		/// A value indicating whether to print the selected text only.
		/// </summary>
		private bool bPrintSelection = false;
		/// <summary>
		/// The original X position before a caret navigation in up/down
		/// directions.
		/// </summary>
		private float editOriginalX;
		/// <summary>
		/// The first valid location of the caret.
		/// </summary>
		private readonly EditLocation editFirstLineChar 
			= new EditLocation(1, 1);
		/// <summary>
		/// The last valid location of the caret.
		/// </summary>
		private readonly EditLocation editLastLineChar 
			= new EditLocation();
		/// <summary>
		/// The bitmap for the panning anchor.
		/// </summary>
		private Bitmap editPanImage;
		/// <summary>
		/// The cursor for the line number and selection margins.
		/// </summary>
		private Cursor editNECursor;
		/// <summary>
		/// The cursor for the incremental search function.
		/// </summary>
		private Cursor editISearchCursor;
		/// <summary>
		/// A value indicating whether an incremental search is in progress.
		/// </summary>
		private bool bISearch = false;
		/// <summary>
		/// The current string for the incremental search.
		/// </summary>
		private string editISearchString;
		/// <summary>
		/// The cursor for drag-and-drop operations.
		/// </summary>
		private Cursor editDragMoveCursor;
		/// <summary>
		/// A value indicating whether the current drag-and-drop operation
		/// was initiated from inside.
		/// </summary>
		private bool bDragInit = false;
		/// <summary>
		/// A value indicating whether an action is being executed.
		/// </summary>
		private bool bInAction = false;
		/// <summary>
		/// The current composite action object.
		/// </summary>
		private EditCompositeAction editCompositeAction;
		/// <summary>
		/// The stack for composite action objects.
		/// </summary>
		private Stack editCompositeActionStack = new Stack();

		/// <summary>
		/// The style of the border.
		/// </summary>
		private BorderStyle editBorderStyle = BorderStyle.FixedSingle;
		/// <summary>
		/// The width of the border.
		/// </summary>
		private int editBorderWidth = 1;
		/// <summary>
		/// The locations for tab stops.
		/// </summary>
		private float [] editTabStops = new float[1];
		/// <summary>
		/// The widths for common chars.
		/// </summary>
		private float [] editCharWidth = new float[256];
		/// <summary>
		/// The stringformat used in text drawing.
		/// </summary>
		private StringFormat editStringFormatTab = new StringFormat(StringFormat.GenericTypographic);
		/// <summary>
		/// The stringformat used in text drawing without tabs.
		/// </summary>
		private StringFormat editStringFormatNoTab = new StringFormat(StringFormat.GenericTypographic);
		/// <summary>
		/// A value indicating whether char widths have been updated.
		/// </summary>
		private bool editCharWidthUpdated = false;

		/// <summary>
		/// The maximum number of columns among lines.
		/// </summary>
		private int editMaxColumn;
		/// <summary>
		/// The column number for the right margin line.
		/// </summary>
		private int editRightMarginLineColumn = 80;

		/// <summary>
		/// The location range of the begin symbol in brace matching.
		/// </summary>
		private EditLocationRange editBraceMatchingBegin = EditLocationRange.Empty;
		/// <summary>
		/// The location range of the end symbol in brace matching.
		/// </summary>
		private EditLocationRange editBraceMatchingEnd = EditLocationRange.Empty;

		/// <summary>
		/// The list of indicator objects.
		/// </summary>
		private EditIndicatorList editIndicatorList = new EditIndicatorList();

		/// <summary>
		/// Helper variables to speed up displaying.
		/// </summary>
		public short SelectedTextColorGroupIndex;
		public short InactiveSelectedTextColorGroupIndex;
		public Color TextForeColor;
		public Color TextBackColor;
		public Color IndicatorMarginForeColor;
		public Color IndicatorMarginBackColor;
		public Color LineNumberForeColor;
		public Color LineNumberBackColor;
		public Color SelectionMarginForeColor;
		public Color SelectionMarginBackColor;
		public Color CollapsedTextBackColor;
		public Color VisibleWhiteSpaceForeColor;
		public Color VisibleWhiteSpaceBackColor;
		public Color BraceMatchingForeColor;

		//		/// <summary>
		//		/// The imagelist for the toolbar buttons.
		//		/// </summary>
		//		private ImageList editToolBarBitmaps = new ImageList();
		//		/// <summary>
		//		/// Commonly used toolbar buttons.
		//		/// </summary>
		//		public ToolBarButton ToolBarButtonNew;
		//		public ToolBarButton ToolBarButtonOpen;
		//		public ToolBarButton ToolBarButtonSave;
		//		public ToolBarButton ToolBarButtonCut;
		//		public ToolBarButton ToolBarButtonCopy;
		//		public ToolBarButton ToolBarButtonPaste;
		//		public ToolBarButton ToolBarButtonFind;
		//		public ToolBarButton ToolBarButtonUndo;
		//		public ToolBarButton ToolBarButtonRedo;
		//		public ToolBarButton ToolBarButtonIncreaseIndent;
		//		public ToolBarButton ToolBarButtonDecreaseIndent;
		//		public ToolBarButton ToolBarButtonComment;
		//		public ToolBarButton ToolBarButtonUnComment;
		//		public ToolBarButton ToolBarButtonBookmark;
		//		public ToolBarButton ToolBarButtonNextBookmark;
		//		public ToolBarButton ToolBarButtonPreviousBookmark;
		//		public ToolBarButton ToolBarButtonClearBookmark;
		//		public ToolBarButton ToolBarButtonViewWhiteSpace;
		//		public ToolBarButton ToolBarButtonPrint;
		//		public ToolBarButton ToolBarButtonPrintPreview;

		/// <summary>
		/// String constants for built-in setting files.
		/// </summary>
		public static readonly string CPPIni = "*CPP.INI";
		public static readonly string CSharpIni = "*CSHARP.INI";
		//		public static readonly string HTMLIni = "*HTML.INI";
		public static readonly string IDLIni = "*IDL.INI";
		public static readonly string JavaIni = "*JAVA.INI";
		public static readonly string JScriptIni = "*JSCRIPT.INI";
		public static readonly string MFCIni = "*MFC.INI";
		public static readonly string PLSQLIni = "*PLSQL.INI";
		public static readonly string SettingsIni = "*SETTINGS.INI";
		public static readonly string VBIni = "*VB.INI";
		public static readonly string VBScriptIni = "*VBSCRIPT.INI";
		//		public static readonly string XMLIni = "*XML.INI";

		#endregion

		#region Methods Related to Construction/Destruction

		/// <override/>
		public override System.ComponentModel.ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				if(this.DesignMode == true)LicenseManager.Validate(typeof(EditControl));
			}
		}

		/// <summary>
		/// The default constructor.
		/// </summary>
		public EditControl()
		{
			
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			// TODO: Add any initialization after the InitForm call
			PostInitializeComponent();
		}

		/// <summary>
		/// Does some additional initializations after InitializeComponent.
		/// </summary>
		protected virtual void PostInitializeComponent()
		{
			editSettings = new EditSettings(this);
			UseResourceStringFile(typeof(EditControl), "Resources.STRING_EN");
			// Gets the minimum size for the top view.
			editTopViewHidingHeight 
				= SystemInformation.VerticalScrollBarArrowHeight;
			// Gets the minimum height for the bottom view.
			editBottomViewMinHeight 
				= 3 * SystemInformation.VerticalScrollBarArrowHeight + 2;
			SetupStatusBar();
			Font = editSettings.EditFont;
			editMaxColumn = 1;
			editData = new EditData(this);
			editSplitter.Height = new Splitter().Height;
			editSplitter.Cursor = Cursors.HSplit;
			editViewTop = new EditView(this, false);
			editViewBottom = new EditView(this, true);
			editActiveView = editViewBottom;
			editOldActiveView = editViewBottom;
			editViewTop.Dock = System.Windows.Forms.DockStyle.Top;
			editSplitter.Dock = System.Windows.Forms.DockStyle.Top;
			editStatusBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			editViewTop.Height = 0;
			editOriginalX = -1;
			this.Controls.AddRange(new Control[]{editViewBottom, 
													editStatusBar,
													editSplitter, 
													editViewTop});
			this.AllowDrop = true;
			this.AllowFileDrop = true;
			this.CopyWithoutSelection = true;
			SetupDefaultIndicators();
			SetupContextMenu();
			editPanImage = new Bitmap(typeof(EditControl), 
				"Resources.NSIMAGE.BMP");
			editNECursor = new Cursor(typeof(EditControl), 
				"Resources.NECURSOR.CUR");
			editISearchCursor = new Cursor(typeof(EditControl), 
				"Resources.ISEARCH.CUR");
			editDragMoveCursor = new Cursor(typeof(EditControl),
				"Resources.DRAGMOVE.CUR");
			CurrentLine = 1;
			CurrentChar = 1;

			// Events handling.
			this.SizeChanged += new EventHandler(This_SizeChanged);
			this.GotFocus += new EventHandler(This_GotFocus);
			this.LostFocus += new EventHandler(This_LostFocus);
			this.FontChanged += new EventHandler(This_FontChanged);

			editStatusBar.GotFocus += new EventHandler(This_GotFocus);
			editSplitter.GotFocus += new EventHandler(This_GotFocus);
			editSplitter.MouseDown += new MouseEventHandler(Splitter_MouseDown);
			editSplitter.MouseMove += new MouseEventHandler(Splitter_MouseMove);
			editSplitter.MouseUp += new MouseEventHandler(Splitter_MouseUp);
			// Selection margin redrawing timer settings.
			editSelectionMarginTimer.Interval = 400;
			editSelectionMarginTimer.Elapsed += 
				new ElapsedEventHandler(SelectionMarginTimer_Elapsed);
			// The printing processing.
			editPrintDocument.BeginPrint += new PrintEventHandler(Pd_BeginPrint);
			editPrintDocument.PrintPage += new PrintPageEventHandler(Pd_PrintPage);
			editPrintDocument.EndPrint += new PrintEventHandler(Pd_EndPrint);

			SetupCommonColors();
			statusBarHeight = StatusBarVisible ? editStatusBar.Height : 0;
			//			SetupToolBarButtons();
			UpdateLayout();
			SetupMessageRelay();
		}
	
		/// <summary> 
		/// Cleans up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				editData.StopOutliningThread();
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Sets up the relay for common messages from internal controls 
		/// to EditControl.
		/// </summary>
		protected void SetupMessageRelay()
		{
			editViewTop.MouseDown += new MouseEventHandler(Relay_MouseDown);
			editSplitter.MouseDown += new MouseEventHandler(Relay_MouseDown);
			editViewBottom.MouseDown += new MouseEventHandler(Relay_MouseDown);
			editStatusBar.MouseDown += new MouseEventHandler(Relay_MouseDown);
			editViewTop.MouseUp += new MouseEventHandler(Relay_MouseUp);
			editSplitter.MouseUp += new MouseEventHandler(Relay_MouseUp);
			editViewBottom.MouseUp += new MouseEventHandler(Relay_MouseUp);
			editStatusBar.MouseUp += new MouseEventHandler(Relay_MouseUp);
			editViewTop.MouseMove += new MouseEventHandler(Relay_MouseMove);
			editSplitter.MouseMove += new MouseEventHandler(Relay_MouseMove);
			editViewBottom.MouseMove += new MouseEventHandler(Relay_MouseMove);
			editStatusBar.MouseMove += new MouseEventHandler(Relay_MouseMove);
			editViewTop.MouseEnter += new EventHandler(Relay_MouseEnter);
			editSplitter.MouseEnter += new EventHandler(Relay_MouseEnter);
			editViewBottom.MouseEnter += new EventHandler(Relay_MouseEnter);
			editStatusBar.MouseEnter += new EventHandler(Relay_MouseEnter);
			editViewTop.MouseHover += new EventHandler(Relay_MouseHover);
			editSplitter.MouseHover += new EventHandler(Relay_MouseHover);
			editViewBottom.MouseHover += new EventHandler(Relay_MouseHover);
			editStatusBar.MouseHover += new EventHandler(Relay_MouseHover);
			editViewTop.MouseLeave += new EventHandler(Relay_MouseLeave);
			editSplitter.MouseLeave += new EventHandler(Relay_MouseLeave);
			editViewBottom.MouseLeave += new EventHandler(Relay_MouseLeave);
			editStatusBar.MouseLeave += new EventHandler(Relay_MouseLeave);

			editViewTop.Click += new EventHandler(Relay_Click);
			editViewBottom.Click += new EventHandler(Relay_Click);
			editViewTop.DoubleClick += new EventHandler(Relay_DoubleClick);
			editViewBottom.DoubleClick += new EventHandler(Relay_DoubleClick);

			editViewTop.DragEnter += new DragEventHandler(Relay_DragEnter);
			editViewBottom.DragEnter += new DragEventHandler(Relay_DragEnter);
			editViewTop.DragOver += new DragEventHandler(Relay_DragOver);
			editViewBottom.DragOver += new DragEventHandler(Relay_DragOver);
			editViewTop.DragDrop += new DragEventHandler(Relay_DragDrop);
			editViewBottom.DragDrop += new DragEventHandler(Relay_DragDrop);
			editViewTop.DragLeave += new EventHandler(Relay_DragLeave);
			editViewBottom.DragLeave += new EventHandler(Relay_DragLeave);
		}

		/// <summary> 
		/// Gets the default size of the control. 
		/// </summary> 
		protected override Size DefaultSize 
		{ 
			get 
			{ 
				return new Size(150, 150); 
			}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		/// <summary>
		/// Sets up the statusbar.
		/// </summary>
		private void SetupStatusBar()
		{
			editStatusBar.Font = SystemInformation.MenuFont;
			editStatusBar.ShowPanels = true;
			editStatusBar.SizingGrip = false;
			editStatusBar.ContextMenu = editEmptyContextMenu;
			editStatusBar.Cursor = Cursors.Arrow;
			for (int i = 0; i < editStatusBarPanels.Length; i++)
			{
				editStatusBarPanels[i] = new StatusBarPanel();
				bCustomPanelText[i] = false;
			}
			editStatusBarPanels[0].AutoSize = StatusBarPanelAutoSize.Spring;
			editStatusBarPanels[1].Width = 64;
			editStatusBarPanels[1].Text = GetResourceString("StatusLine");
			editStatusBarPanels[2].Width = 64;
			editStatusBarPanels[2].Text = GetResourceString("StatusColumn");
			editStatusBarPanels[3].Width = 64;
			editStatusBarPanels[3].Text = GetResourceString("StatusChar");
			editStatusBar.Panels.AddRange(editStatusBarPanels);
		}

		//		/// <summary>
		//		/// Sets up the toolbar buttons. 
		//		/// </summary>
		//		private void SetupToolBarButtons()
		//		{
		//			editToolBarBitmaps.ImageSize = new Size(16, 16);
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.NEW.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.OPEN.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.SAVE.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.CUT.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.COPY.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.PASTE.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.FIND.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.UNDO.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.REDO.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.INCIDT.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.DECIDT.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.CMTOUT.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.UNCMT.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.BOOKMARK.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.NEXTBM.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.PREVBM.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.DELBM.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.VWSPACE.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.PRINT.BMP"));
		//			editToolBarBitmaps.Images.Add(new Bitmap(typeof(EditControl), 
		//				"Resources.PREVIEW.BMP"));
		//
		//			string [] strToolTip = {   "New File",
		//									   "Open File",
		//									   "Save File",
		//									   "Cut",
		//									   "Copy",
		//									   "Paste",
		//									   "Find",
		//									   "Undo",
		//									   "Redo",
		//									   "Increase Indent",
		//									   "Decrease Indent",
		//									   "Comment out the selected lines",
		//									   "Uncomment the selected lines",
		//									   "Toggle bookmark on the current line",
		//									   "Move the caret to the next bookmark",
		//									   "Move the caret to the previous bookmark",
		//									   "Clear all bookmarks",
		//									   "View White Spaces",
		//									   "Print",
		//									   "Print Preview"
		//								   };
		//
		//			int i = 0;
		//			ToolBarButtonNew = new ToolBarButton();
		//			ToolBarButtonNew.ImageIndex = i;
		//			ToolBarButtonNew.ToolTipText = strToolTip[i++];
		//			ToolBarButtonOpen = new ToolBarButton();
		//			ToolBarButtonOpen.ImageIndex = i;
		//			ToolBarButtonOpen.ToolTipText = strToolTip[i++];
		//			ToolBarButtonSave = new ToolBarButton();
		//			ToolBarButtonSave.ImageIndex = i;
		//			ToolBarButtonSave.ToolTipText = strToolTip[i++];
		//			ToolBarButtonCut = new ToolBarButton();
		//			ToolBarButtonCut.ImageIndex = i;
		//			ToolBarButtonCut.ToolTipText = strToolTip[i++];
		//			ToolBarButtonCopy = new ToolBarButton();
		//			ToolBarButtonCopy.ImageIndex = i;
		//			ToolBarButtonCopy.ToolTipText = strToolTip[i++];
		//			ToolBarButtonPaste = new ToolBarButton();
		//			ToolBarButtonPaste.ImageIndex = i;
		//			ToolBarButtonPaste.ToolTipText = strToolTip[i++];
		//			ToolBarButtonFind = new ToolBarButton();
		//			ToolBarButtonFind.ImageIndex = i;
		//			ToolBarButtonFind.ToolTipText = strToolTip[i++];
		//			ToolBarButtonUndo = new ToolBarButton();
		//			ToolBarButtonUndo.ImageIndex = i;
		//			ToolBarButtonUndo.ToolTipText = strToolTip[i++];
		//			ToolBarButtonRedo = new ToolBarButton();
		//			ToolBarButtonRedo.ImageIndex = i;
		//			ToolBarButtonRedo.ToolTipText = strToolTip[i++];
		//			ToolBarButtonIncreaseIndent = new ToolBarButton();
		//			ToolBarButtonIncreaseIndent.ImageIndex = i;
		//			ToolBarButtonIncreaseIndent.ToolTipText = strToolTip[i++];
		//			ToolBarButtonDecreaseIndent = new ToolBarButton();
		//			ToolBarButtonDecreaseIndent.ImageIndex = i;
		//			ToolBarButtonDecreaseIndent.ToolTipText = strToolTip[i++];
		//			ToolBarButtonComment = new ToolBarButton();
		//			ToolBarButtonComment.ImageIndex = i;
		//			ToolBarButtonComment.ToolTipText = strToolTip[i++];
		//			ToolBarButtonUnComment = new ToolBarButton();
		//			ToolBarButtonUnComment.ImageIndex = i;
		//			ToolBarButtonUnComment.ToolTipText = strToolTip[i++];
		//			ToolBarButtonBookmark = new ToolBarButton();
		//			ToolBarButtonBookmark.ImageIndex = i;
		//			ToolBarButtonBookmark.ToolTipText = strToolTip[i++];
		//			ToolBarButtonNextBookmark = new ToolBarButton();
		//			ToolBarButtonNextBookmark.ImageIndex = i;
		//			ToolBarButtonNextBookmark.ToolTipText = strToolTip[i++];
		//			ToolBarButtonPreviousBookmark = new ToolBarButton();
		//			ToolBarButtonPreviousBookmark.ImageIndex = i;
		//			ToolBarButtonPreviousBookmark.ToolTipText = strToolTip[i++];
		//			ToolBarButtonClearBookmark = new ToolBarButton();
		//			ToolBarButtonClearBookmark.ImageIndex = i;
		//			ToolBarButtonClearBookmark.ToolTipText = strToolTip[i++];
		//			ToolBarButtonViewWhiteSpace = new ToolBarButton();
		//			ToolBarButtonViewWhiteSpace.ImageIndex = i;
		//			ToolBarButtonViewWhiteSpace.ToolTipText = strToolTip[i++];
		//			ToolBarButtonPrint = new ToolBarButton();
		//			ToolBarButtonPrint.ImageIndex = i;
		//			ToolBarButtonPrint.ToolTipText = strToolTip[i++];
		//			ToolBarButtonPrintPreview = new ToolBarButton();
		//			ToolBarButtonPrintPreview.ImageIndex = i;
		//			ToolBarButtonPrintPreview.ToolTipText = strToolTip[i++];
		//		}

		//		private void EditButtonClicked(object sender, ToolBarButtonClickEventArgs e)
		//		{
		//			if (e.Button == ToolBarButtonNew)
		//			{
		//				NewFile();
		//			}
		//			else if (e.Button == ToolBarButtonOpen)
		//			{
		//				Open();
		//			}
		//			else if (e.Button == ToolBarButtonSave)
		//			{
		//				Save();
		//			}
		//			else if (e.Button == ToolBarButtonCut)
		//			{
		//				Cut();
		//			}
		//			else if (e.Button == ToolBarButtonCopy)
		//			{
		//				Copy();
		//			}
		//			else if (e.Button == ToolBarButtonPaste)
		//			{
		//				Paste();
		//			}
		//			else if (e.Button == ToolBarButtonFind)
		//			{
		//				FindAndReplace(false);
		//			}
		//			else if (e.Button == ToolBarButtonUndo)
		//			{
		//				Undo();
		//			}
		//			else if (e.Button == ToolBarButtonRedo)
		//			{
		//				Redo();
		//			}
		//			else if (e.Button == ToolBarButtonIncreaseIndent)
		//			{
		//				IncreaseLineIndent();
		//			}
		//			else if (e.Button == ToolBarButtonDecreaseIndent)
		//			{
		//				DecreaseLineIndent();
		//			}
		//			else if (e.Button == ToolBarButtonComment)
		//			{
		//				CommentSelection();
		//			}
		//			else if (e.Button == ToolBarButtonUnComment)
		//			{
		//				UncommentSelection();
		//			}
		//			else if (e.Button == ToolBarButtonBookmark)
		//			{
		//				ToggleBookmark();
		//			}
		//			else if (e.Button == ToolBarButtonNextBookmark)
		//			{
		//				NextBookmark();
		//			}
		//			else if (e.Button == ToolBarButtonPreviousBookmark)
		//			{
		//				PreviousBookmark();
		//			}
		//			else if (e.Button == ToolBarButtonClearBookmark)
		//			{
		//				ClearBookmarks();
		//			}
		//			else if (e.Button == ToolBarButtonViewWhiteSpace)
		//			{
		//				WhiteSpaceVisible = !WhiteSpaceVisible;
		//			}
		//			else if (e.Button == ToolBarButtonPrint)
		//			{
		//				Print();
		//			}
		//			else if (e.Button == ToolBarButtonPrintPreview)
		//			{
		//				PrintPreview();
		//			}
		//		}

		/// <summary>
		/// Sets up context menus.
		/// </summary>
		private void SetupContextMenu()
		{
			editMenuItemEdit = new MenuItem(string.Empty);
			editMenuItemCut = new MenuItem(string.Empty, 
				new EventHandler(MenuItem_Click), Shortcut.CtrlX);
			editMenuItemCopy = new MenuItem(string.Empty, 
				new EventHandler(MenuItem_Click), Shortcut.CtrlC);
			editMenuItemPaste = new MenuItem(string.Empty, 
				new EventHandler(MenuItem_Click), Shortcut.CtrlV);
			editMenuItemDelete = new MenuItem(string.Empty, 
				new EventHandler(MenuItem_Click), Shortcut.Del);
			editMenuItemUndo = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlZ);
			editMenuItemRedo = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlY);
			editMenuItemFind = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlF);
			editMenuItemReplace = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlH);
			editMenuItemGoTo = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlG);
			editMenuItemSelectAll = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlA);
			editMenuItemInsertFileAsText = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemTimeDate = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.F5);
			editMenuItemEdit.MenuItems.Add(editMenuItemCut);
			editMenuItemEdit.MenuItems.Add(editMenuItemCopy);
			editMenuItemEdit.MenuItems.Add(editMenuItemPaste);
			editMenuItemEdit.MenuItems.Add(editMenuItemDelete);
			editMenuItemEdit.MenuItems.Add(new MenuItem("-"));
			editMenuItemEdit.MenuItems.Add(editMenuItemUndo);
			editMenuItemEdit.MenuItems.Add(editMenuItemRedo);
			editMenuItemEdit.MenuItems.Add(new MenuItem("-"));
			editMenuItemEdit.MenuItems.Add(editMenuItemFind);
			editMenuItemEdit.MenuItems.Add(editMenuItemReplace);
			editMenuItemEdit.MenuItems.Add(editMenuItemGoTo);
			editMenuItemEdit.MenuItems.Add(new MenuItem("-"));
			editMenuItemEdit.MenuItems.Add(editMenuItemSelectAll);
			editMenuItemEdit.MenuItems.Add(editMenuItemInsertFileAsText);
			editMenuItemEdit.MenuItems.Add(editMenuItemTimeDate);

			editMenuItemFile = new MenuItem(string.Empty);
			editMenuItemNew = new MenuItem(string.Empty, 
				new EventHandler(MenuItem_Click), Shortcut.CtrlN);
			editMenuItemOpen = new MenuItem(string.Empty, 
				new EventHandler(MenuItem_Click), Shortcut.CtrlO);
			editMenuItemClose = new MenuItem(string.Empty, 
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemSave = new MenuItem(string.Empty, 
				new EventHandler(MenuItem_Click), Shortcut.CtrlS);
			editMenuItemSaveAs = new MenuItem(string.Empty, 
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemPrintPreview = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemPrint = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlP);
			editMenuItemFile.MenuItems.Add(editMenuItemNew);
			editMenuItemFile.MenuItems.Add(editMenuItemOpen);
			editMenuItemFile.MenuItems.Add(editMenuItemClose);
			editMenuItemFile.MenuItems.Add(new MenuItem("-"));
			editMenuItemFile.MenuItems.Add(editMenuItemSave);
			editMenuItemFile.MenuItems.Add(editMenuItemSaveAs);
			editMenuItemFile.MenuItems.Add(new MenuItem("-"));
			editMenuItemFile.MenuItems.Add(editMenuItemPrintPreview);
			editMenuItemFile.MenuItems.Add(editMenuItemPrint);

			editMenuItemAdvanced = new MenuItem(string.Empty);
			editMenuItemTabifySelection = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemUntabifySelection = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemCommentSelection = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemUncommentSelection = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemMakeUppercase = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlShiftU);
			editMenuItemMakeLowercase = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlU);
			editMenuItemDeleteHorizontalWhiteSpace = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemIncreaseLineIndent = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemDecreaseLineIndent = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemViewWhiteSpace = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemIncrementalSearch = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.CtrlI);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemTabifySelection);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemUntabifySelection);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemCommentSelection);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemUncommentSelection);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemMakeUppercase);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemMakeLowercase);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemDeleteHorizontalWhiteSpace);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemIncreaseLineIndent);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemDecreaseLineIndent);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemViewWhiteSpace);
			editMenuItemAdvanced.MenuItems.Add(editMenuItemIncrementalSearch);

			editMenuItemBookmark = new MenuItem(string.Empty);
			editMenuItemToggleBookmark = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemNextBookmark = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemPreviousBookmark = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemClearBookmarks = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemBookmark.MenuItems.Add(editMenuItemToggleBookmark);
			editMenuItemBookmark.MenuItems.Add(editMenuItemNextBookmark);
			editMenuItemBookmark.MenuItems.Add(editMenuItemPreviousBookmark);
			editMenuItemBookmark.MenuItems.Add(editMenuItemClearBookmarks);

			//			MenuItem editBreakpointMenu = new MenuItem(GetResourceString("MenuItemBreakpoints"));
			//			editBreakpointMenu.MenuItems.Add(
			//				new MenuItem(GetResourceString("MenuItemToggleBreakpoint"),
			//				new EventHandler(MenuItem_Click), Shortcut.None));
			//			editBreakpointMenu.MenuItems.Add(
			//				new MenuItem(GetResourceString("MenuItemNewBreakpoint"),
			//				new EventHandler(MenuItem_Click), Shortcut.CtrlB));

			editMenuItemOutlining = new MenuItem(string.Empty);
			editMenuItemHideSelection = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemToggleOutliningExpansion = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemToggleAllOutlining = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemStopOutlining = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemStopHidingCurrent = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemCollapseToDefinitions = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemStartAutomaticOutlining = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);
			editMenuItemOutlining.MenuItems.Add(editMenuItemHideSelection);
			editMenuItemOutlining.MenuItems.Add(editMenuItemToggleOutliningExpansion);
			editMenuItemOutlining.MenuItems.Add(editMenuItemToggleAllOutlining);
			editMenuItemOutlining.MenuItems.Add(editMenuItemStopOutlining);
			editMenuItemOutlining.MenuItems.Add(editMenuItemStopHidingCurrent);
			editMenuItemOutlining.MenuItems.Add(editMenuItemCollapseToDefinitions);
			editMenuItemOutlining.MenuItems.Add(editMenuItemStartAutomaticOutlining);

			editMenuItemOptions = new MenuItem(string.Empty,
				new EventHandler(MenuItem_Click), Shortcut.None);

			editContextMenu = new ContextMenu();
			editContextMenu.MenuItems.Add(editMenuItemEdit);
			editContextMenu.MenuItems.Add(new MenuItem("-"));
			editContextMenu.MenuItems.Add(editMenuItemFile);
			editContextMenu.MenuItems.Add(new MenuItem("-"));
			editContextMenu.MenuItems.Add(editMenuItemAdvanced);
			//			editContextMenu.MenuItems.Add(editBreakpointMenu);
			editContextMenu.MenuItems.Add(editMenuItemBookmark);
			editContextMenu.MenuItems.Add(editMenuItemOutlining);
			editContextMenu.MenuItems.Add(new MenuItem("-"));
			editContextMenu.MenuItems.Add(editMenuItemOptions);

			SetMenuItemText();

			ContextMenu = editContextMenu;
			
			editSplitter.ContextMenu = EmptyContextMenu;
			// The processing of the Popup event of context menus.
			ContextMenu.Popup += new EventHandler(ContextMenu_Popup);
			editMenuItemFile.Popup += new EventHandler(FileMenu_Popup);
			editMenuItemEdit.Popup += new EventHandler(EditMenu_Popup);
			editMenuItemOutlining.Popup += new EventHandler(OutliningMenu_Popup);
		}

		/// <summary>
		/// Sets text for menu items.
		/// </summary>
		private void SetMenuItemText()
		{
			editMenuItemEdit.Text = GetResourceString("MenuItemEdit");
			editMenuItemCut.Text = GetResourceString("MenuItemCut"); 
			editMenuItemCopy.Text = GetResourceString("MenuItemCopy"); 
			editMenuItemPaste.Text = GetResourceString("MenuItemPaste");
			editMenuItemDelete.Text = GetResourceString("MenuItemDelete");
			editMenuItemUndo.Text = GetResourceString("MenuItemUndo");
			editMenuItemRedo.Text = GetResourceString("MenuItemRedo");
			editMenuItemFind.Text = GetResourceString("MenuItemFind");
			editMenuItemReplace.Text = GetResourceString("MenuItemReplace");
			editMenuItemGoTo.Text = GetResourceString("MenuItemGoTo");
			editMenuItemSelectAll.Text = GetResourceString("MenuItemSelectAll");
			editMenuItemInsertFileAsText.Text = GetResourceString("MenuItemInsertFileAsText");
			editMenuItemTimeDate.Text = GetResourceString("MenuItemTimeDate");

			editMenuItemFile.Text = GetResourceString("MenuItemFile");
			editMenuItemNew.Text = GetResourceString("MenuItemNew");
			editMenuItemOpen.Text = GetResourceString("MenuItemOpen");
			editMenuItemClose.Text = GetResourceString("MenuItemClose");
			editMenuItemSave.Text = GetResourceString("MenuItemSave");
			editMenuItemSaveAs.Text = GetResourceString("MenuItemSaveAs");
			editMenuItemPrintPreview.Text = GetResourceString("MenuItemPrintPreview");
			editMenuItemPrint.Text = GetResourceString("MenuItemPrint");

			editMenuItemAdvanced.Text = GetResourceString("MenuItemAdvanced");
			editMenuItemTabifySelection.Text = GetResourceString("MenuItemTabifySelection");
			editMenuItemUntabifySelection.Text = GetResourceString("MenuItemUntabifySelection");
			editMenuItemCommentSelection.Text = GetResourceString("MenuItemCommentSelection");
			editMenuItemUncommentSelection.Text = GetResourceString("MenuItemUncommentSelection");
			editMenuItemMakeUppercase.Text = GetResourceString("MenuItemMakeUppercase");
			editMenuItemMakeLowercase.Text = GetResourceString("MenuItemMakeLowercase");
			editMenuItemDeleteHorizontalWhiteSpace.Text = GetResourceString("MenuItemDeleteHorizontalWhiteSpace");
			editMenuItemIncreaseLineIndent.Text = GetResourceString("MenuItemIncreaseLineIndent");
			editMenuItemDecreaseLineIndent.Text = GetResourceString("MenuItemDecreaseLineIndent");
			editMenuItemViewWhiteSpace.Text = GetResourceString("MenuItemViewWhiteSpace");
			editMenuItemIncrementalSearch.Text = GetResourceString("MenuItemIncrementalSearch");

			editMenuItemBookmark.Text = GetResourceString("MenuItemBookmarks");
			editMenuItemToggleBookmark.Text = GetResourceString("MenuItemToggleBookmark");
			editMenuItemNextBookmark.Text = GetResourceString("MenuItemNextBookmark");
			editMenuItemPreviousBookmark.Text = GetResourceString("MenuItemPreviousBookmark");
			editMenuItemClearBookmarks.Text = GetResourceString("MenuItemClearBookmarks");

			editMenuItemOutlining.Text = GetResourceString("MenuItemOutlining");
			editMenuItemHideSelection.Text = GetResourceString("MenuItemHideSelection");
			editMenuItemToggleOutliningExpansion.Text = GetResourceString("MenuItemToggleOutliningExpansion");
			editMenuItemToggleAllOutlining.Text = GetResourceString("MenuItemToggleAllOutlining");
			editMenuItemStopOutlining.Text = GetResourceString("MenuItemStopOutlining");
			editMenuItemStopHidingCurrent.Text = GetResourceString("MenuItemStopHidingCurrent");
			editMenuItemCollapseToDefinitions.Text = GetResourceString("MenuItemCollapseToDefinitions");
			editMenuItemStartAutomaticOutlining.Text = GetResourceString("MenuItemStartAutomaticOutlining");

			editMenuItemOptions.Text = GetResourceString("MenuItemOptions");
		}

		/// <summary>
		/// Enables/Disables Context menu items
		/// </summary>
		private void UpdateContextMenu()
		{
			for (int i = 0; i < ContextMenu.MenuItems.Count; i++)
			{
				string itemText = 
					ContextMenu.MenuItems[i].Text.Replace("&", string.Empty);
				switch (itemText)
				{
					case "Edit":
						ContextMenu.MenuItems[i].Enabled = Updateable;
						break;
					case "Advanced":
						ContextMenu.MenuItems[i].Enabled = Updateable;
						break;
						//					case "Breakpoints":
						//						ContextMenu.MenuItems[i].Enabled = HasContent;
						//						break;
					case "Bookmarks":
						ContextMenu.MenuItems[i].Enabled = HasContent;
						break;
					case "Outlining":
						ContextMenu.MenuItems[i].Visible = OutliningEnabled;
						ContextMenu.MenuItems[i].Enabled = HasContent; 
						break;
					case "Options...":
						ContextMenu.MenuItems[i].Enabled = HasContent;
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Handles the Popup event of the main context menu.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void ContextMenu_Popup(object sender, EventArgs e)
		{
			if(this.ContextMenu == this.editContextMenu)
			{
				this.Focus();
				UpdateContextMenu();
			}
		}

		/// <summary>
		/// Handles the popup event of the File submenu.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void FileMenu_Popup(object sender, EventArgs e)
		{
			for (int i = 0; i < editMenuItemFile.MenuItems.Count; i++)
			{
				string itemText = editMenuItemFile.MenuItems[i].Text.
					Replace("&", string.Empty);
				switch (itemText)
				{
					case "Save":
						editMenuItemFile.MenuItems[i].Enabled = Modified;
						break;
					case "Save As...":
						editMenuItemFile.MenuItems[i].Enabled = HasContent;
						break;
					case "Close":
						editMenuItemFile.MenuItems[i].Enabled = HasContent;
						break;
					case "Print Preview...":
						editMenuItemFile.MenuItems[i].Enabled = HasContent;
						break;
					case "Print...":
						editMenuItemFile.MenuItems[i].Enabled = HasContent;
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Handles the popup event of the Edit submenu.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void OutliningMenu_Popup(object sender, EventArgs e)
		{
			for (int i = 0; i < editMenuItemOutlining.MenuItems.Count; i++)
			{
				string itemText = editMenuItemOutlining.MenuItems[i].Text.
					Replace("&", string.Empty);
				switch (itemText)
				{
					case "Hide Selection":
						editMenuItemOutlining.MenuItems[i].Visible = 
							OutliningEnabled && (!AutomaticOutliningEnabled);
						break;
					case "Stop Hiding Current":
						editMenuItemOutlining.MenuItems[i].Visible = 
							OutliningEnabled && (!AutomaticOutliningEnabled);
						break;
					case "Collapse to Definitions":
						editMenuItemOutlining.MenuItems[i].Visible = 
							OutliningEnabled && AutomaticOutliningEnabled;
						break;
					case "Start Automatic Outlining":
						editMenuItemOutlining.MenuItems[i].Visible = false;
						//							OutliningEnabled && (!AutomaticOutliningEnabled);
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Handles the popup event of the Edit submenu.
		/// </summary>
		/// <param name="sender">The source of the evet.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void EditMenu_Popup(object sender, EventArgs e)
		{
			for (int i = 0; i < editMenuItemEdit.MenuItems.Count; i++)
			{
				string itemText = editMenuItemEdit.MenuItems[i].Text.
					Replace("&", string.Empty);
				switch (itemText)
				{
					case "Cut":
						editMenuItemEdit.MenuItems[i].Enabled = Updateable;
						break;
					case "Copy":
						editMenuItemEdit.MenuItems[i].Enabled = HasContent;
						break;
					case "Paste":
						editMenuItemEdit.MenuItems[i].Enabled = CanPaste;
						break;
					case "Delete":
						editMenuItemEdit.MenuItems[i].Enabled = Updateable;
						break;
					case "Undo":
						editMenuItemEdit.MenuItems[i].Enabled = CanUndo;
						break;
					case "Redo":
						editMenuItemEdit.MenuItems[i].Enabled = CanRedo;
						break;
					case "Find...":
						editMenuItemEdit.MenuItems[i].Enabled = HasContent;
						break;
					case "Replace...":
						editMenuItemEdit.MenuItems[i].Enabled = HasContent;
						break;
					case "Go To...":
						editMenuItemEdit.MenuItems[i].Enabled = HasContent;
						break;
					case "Select All":
						editMenuItemEdit.MenuItems[i].Enabled = HasContent;
						break;
					case "Insert File As Text...":
						editMenuItemEdit.MenuItems[i].Enabled = Updateable;
						break;
					case "Time/Date":
						editMenuItemEdit.MenuItems[i].Enabled = Updateable;
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Processes the click event of menu items.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		public void MenuItem_Click(Object sender, EventArgs e)
		{
			string itemText = ((MenuItem)sender).Text.Replace("&", string.Empty);
			switch (itemText)
			{
				case "Tabify Selection":
					TabifySelection();
					break;
				case "Untabify Selection":
					UntabifySelection();
					break;
				case "Comment Selection":
					CommentSelection();
					break;
				case "Uncomment Selection":
					UncommentSelection();
					break;
				case "Make Uppercase":
					MakeUpperCase();
					break;
				case "Make Lowercase":
					MakeLowerCase();
					break;
				case "Delete Horizontal White Space":
					DeleteHorizontalWhiteSpace();
					break;
				case "Increase Line Indent":
					IncreaseLineIndent();
					break;
				case "Decrease Line Indent":
					DecreaseLineIndent();
					break;
				case "View White Space":
					WhiteSpaceVisible = !WhiteSpaceVisible;
					break;
				case "Incremental Search":
					IncrementalSearch();
					break;
				case "Options...":
					ShowOptions();
					break;
				case "Toggle Bookmark":
					ToggleBookmark();
					break;
				case "Next Bookmark":
					NextBookmark();
					break;
				case "Previous Bookmark":
					PreviousBookmark();
					break;
				case "Clear Bookmarks":
					ClearBookmarks();
					break;
				case "Toggle Breakpoint":
					ToggleBreakpoint();
					break;
				case "New Breakpoint":
					NewBreakpoint();
					break;
				case "Hide Selection":
					HideAsOutlining();
					break;
				case "Toggle Outlining Expansion":
					ToggleOutliningExpansion();
					break;
				case "Toggle All Outlining":
					ToggleAllOutlining();
					break;
				case "Stop Outlining":
					StopOutlining();
					break;
				case "Stop Hiding Current":
					StopHidingCurrent();
					break;
				case "Collapse to Definitions":
					CollapseToDefinitions();
					break;
				case "Start Automatic Outlining":
					StartAutomaticOutlining();
					break;
				case "New":
					NewFile();
					break;
				case "Open...":
					Open();
					break;
				case "Close":
					Close();
					break;
				case "Save":
					Save();
					break;
				case "Save As...":
					SaveAs();
					break;
				case "Print...":
					Print();
					break;
				case "Print Preview...":
					PrintPreview();
					break;
				case "Cut":
					Cut();
					break;
				case "Copy":
					Copy();
					break;
				case "Paste":
					Paste();
					break;
				case "Delete":
					DeleteKey();
					break;
				case "Select All":
					SelectAll();
					break;
				case "Undo":
					Undo();
					break;
				case "Redo":
					Redo();
					break;
				case "Find...":
					FindAndReplace(false);
					break;
				case "Replace...":
					FindAndReplace(true);
					break;
				case "Go To...":
					GoTo();
					break;
				case "Insert File As Text...":
					InsertFile();
					break;
				case "Time/Date":
					TimeDate();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Raises the KeyDown event.
		/// </summary>
		/// <param name="e">A KeyEventArgs that contains the event data.</param>
		internal void RaiseKeyDown(KeyEventArgs e)
		{
			OnKeyDown(e);
		}

		/// <summary>
		/// Raises the KeyUp event.
		/// </summary>
		/// <param name="e">A KeyEventArgs that contains the event data.</param>
		internal void RaiseKeyUp(KeyEventArgs e)
		{
			OnKeyUp(e);
		}

		/// <summary>
		/// Raises the KeyPress event.
		/// </summary>
		/// <param name="e">A KeyPressEventArgs that contains the event data.</param>
		internal void RaiseKeyPress(KeyPressEventArgs e)
		{
			OnKeyPress(e);
		}

		/// <summary>
		/// Relays the MouseDown event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		private void Relay_MouseDown(object sender, MouseEventArgs e)
		{
			MouseEventArgs eNew = new MouseEventArgs(e.Button, e.Clicks, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.Delta);
			OnMouseDown(eNew);
		}

		/// <summary>
		/// Relays the MouseUp event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		private void Relay_MouseUp(object sender, MouseEventArgs e)
		{
			MouseEventArgs eNew = new MouseEventArgs(e.Button, e.Clicks, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.Delta);
			OnMouseUp(eNew);
		}

		/// <summary>
		/// Relays the MouseMove event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		private void Relay_MouseMove(object sender, MouseEventArgs e)
		{
			MouseEventArgs eNew = new MouseEventArgs(e.Button, e.Clicks, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.Delta);
			OnMouseMove(eNew);
		}

		/// <summary>
		/// Relays the MouseEnter event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Relay_MouseEnter(object sender, EventArgs e)
		{
			OnMouseEnter(e);
		}

		/// <summary>
		/// Relays the MouseHover event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Relay_MouseHover(object sender, EventArgs e)
		{
			OnMouseHover(e);
		}

		/// <summary>
		/// Relays the MouseLeave event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Relay_MouseLeave(object sender, EventArgs e)
		{
			OnMouseLeave(e);
		}

		/// <summary>
		/// Relays the Click event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Relay_Click(object sender, EventArgs e)
		{
			OnClick(e);
		}

		/// <summary>
		/// Relays the DoubleClick event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Relay_DoubleClick(object sender, EventArgs e)
		{
			OnDoubleClick(e);
		}

		/// <summary>
		/// Raises the DragEnter event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		private void Relay_DragEnter(object sender, DragEventArgs e)
		{
			DragEventArgs eNew = new DragEventArgs(e.Data, e.KeyState, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.AllowedEffect, e.Effect);
			OnDragEnter(eNew);
		}

		/// <summary>
		/// Relays the DragOver event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		private void Relay_DragOver(object sender, DragEventArgs e)
		{
			DragEventArgs eNew = new DragEventArgs(e.Data, e.KeyState, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.AllowedEffect, e.Effect);
			OnDragOver(eNew);
		}

		/// <summary>
		/// Relays the DragDrop event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A DragEventArgs that contains the event data.</param>
		private void Relay_DragDrop(object sender, DragEventArgs e)
		{
			DragEventArgs eNew = new DragEventArgs(e.Data, e.KeyState, 
				e.X + ((Control)sender).Left, e.Y + ((Control)sender).Top,
				e.AllowedEffect, e.Effect);
			OnDragDrop(eNew);
		}

		/// <summary>
		/// Relays the DragLeave event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Relay_DragLeave(object sender, EventArgs e)
		{
			OnDragLeave(e);
		}

		/// <summary>
		/// Handles the LostFocus event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_LostFocus(object sender, EventArgs e)
		{
			bCanPasteOld = CanPaste;
		}

		/// <summary>
		/// Handles the GotFocus event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_GotFocus(object sender, EventArgs e)
		{
			if (editOldActiveView != null)
			{
				editOldActiveView.Focus();
			}
			else
			{
				editViewBottom.Focus();
			}
			if (CanPaste != bCanPasteOld)
			{
				bCanPasteOld = CanPaste;
				OnCanPasteChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Handles the FontChanged event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_FontChanged(object sender, EventArgs e)
		{
			UpdateCharWidth();
			UpdateAll();
		}

		/// <summary>
		/// Handles the MouseDown event of the splitter.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Splitter_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				splitterY = e.Y;
				editSplitter.Capture = true;
				bInSplitting = true;
			}
		}

		/// <summary>
		/// Handles the MouseUp event of the splitter.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Splitter_MouseUp(object sender, MouseEventArgs e)
		{
			editSplitter.Capture = false;
			bInSplitting = false;
			int editNewTopViewHeight = editViewTop.Height + (e.Y - splitterY);
			if (editNewTopViewHeight < editTopViewHidingHeight)
			{
				HideTopView();
			}
			else
			{
				if (editNewTopViewHeight != editViewTop.Height)
				{
					editViewTop.Height = Math.Min(editNewTopViewHeight, 
						ClientRectangle.Height - editSplitter.Height  
						- statusBarHeight - editBottomViewMinHeight);
					editViewBottom.Top = editSplitter.Bottom;
					editViewBottom.Height = ClientRectangle.Bottom 
						- editSplitter.Bottom - statusBarHeight;
				}
			}
		}

		/// <summary>
		/// Handles the MouseMove event of the splitter.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Splitter_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				int editNewTopViewHeight = editViewTop.Height + (e.Y - splitterY);
				if (editNewTopViewHeight != editViewTop.Height)
				{
					editViewTop.Height = Math.Min(editNewTopViewHeight, 
						ClientRectangle.Height - editSplitter.Height  
						- statusBarHeight - editBottomViewMinHeight);
					editViewBottom.Top = editSplitter.Bottom;
					editViewBottom.Height = ClientRectangle.Bottom 
						- editSplitter.Bottom - statusBarHeight;
				}
			}
		}

		/// <summary>
		/// Shows the splitter between EditView objects.
		/// </summary>
		internal void DisplaySplitter()
		{
			editSplitter.Capture = true;
			editSplitter.Visible = true;
			editSplitter.Left = ClientRectangle.Left;
			editSplitter.Top = ClientRectangle.Top;
			editViewBottom.Left = editSplitter.Left;
			editViewBottom.Top = editSplitter.Bottom;
			editViewTop.ViewportFirstLine = editViewBottom.ViewportFirstLine;
			editViewTop.ViewportFirstColumn = 1;
			editViewBottom.Height = (StatusBarVisible) ? 
				(editStatusBar.Top - editSplitter.Bottom) : 
				(ClientRectangle.Bottom - editSplitter.Bottom);
			bInSplitting = true;
		}

		/// <summary>
		/// Handles the Elapsed event of the selection margin timer.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void SelectionMarginTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			editSelectionMarginTimer.Stop();
			RedrawSelectionMargin();
		}

		/// <summary>
		/// Updates the layout of all internal controls.
		/// </summary>
		private void UpdateLayout()
		{
			editViewTop.BeginUpdate();
			editViewBottom.BeginUpdate();
			if (editViewTop.Height == 0)
			{
				editViewBottom.Left = ClientRectangle.Left;
				editViewBottom.Top = ClientRectangle.Top;
				editViewBottom.Width = ClientRectangle.Width;
				editViewBottom.Height = ClientRectangle.Height - statusBarHeight;
				editViewBottom.SplitterButtonVisible = bShowSplitterButton;
			}
			else
			{
				if (StatusBarVisible)
				{
					if (editStatusBar.Top - editSplitter.Bottom 
						>= editBottomViewMinHeight)
					{
						editViewBottom.Height = editStatusBar.Top 
							- editSplitter.Bottom;
					}
					else
					{
						editViewBottom.Height = editBottomViewMinHeight;
						editViewTop.Height = ClientRectangle.Height 
							- editSplitter.Height
							- editViewBottom.Height - statusBarHeight;
					}
				}
				else
				{
					if (ClientRectangle.Bottom - editSplitter.Bottom 
						>= editBottomViewMinHeight)
					{
						editViewBottom.Height = ClientRectangle.Bottom 
							- editSplitter.Bottom;
					}
					else
					{
						editViewBottom.Height = editBottomViewMinHeight;
						editViewTop.Height = ClientRectangle.Height 
							- editSplitter.Height
							- editViewBottom.Height - statusBarHeight;
					}
				}
				editViewBottom.Left = editSplitter.Left; 
				editViewBottom.Top = editSplitter.Bottom;
				editViewBottom.Width = ClientRectangle.Width;
			}
			editViewTop.EndUpdate();
			editViewBottom.EndUpdate();
			UpdateStatusBarLayout();
			Redraw();
		}

		/// <summary>
		/// Updates the layout of the status bar.
		/// </summary>
		private void UpdateStatusBarLayout()
		{
			int sbWidth = Math.Max(editStatusBar.Width, 
				4 * editStatusBarPanels[1].MinWidth);
			if (sbWidth > 256) 
			{
				editStatusBarPanels[1].Width = 64;
				editStatusBarPanels[2].Width = 64;
				editStatusBarPanels[3].Width = 64;
			}
			else
			{
				editStatusBarPanels[1].Width = sbWidth/4;
				editStatusBarPanels[2].Width = sbWidth/4;
				editStatusBarPanels[3].Width = sbWidth/4;
			}
		}

		/// <summary>
		/// Handles the Resize event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void This_SizeChanged(object sender, EventArgs e)
		{
			UpdateLayout();
		}

		#endregion

		#region Generic Methods

		/// <summary>
		/// Hides the top view.
		/// </summary>
		public void HideTopView()
		{
			editViewTop.Height = 0;
			editViewBottom.Left = ClientRectangle.Left;
			editViewBottom.Top = ClientRectangle.Top;
			editViewBottom.Height = ClientRectangle.Height - statusBarHeight;
			editViewBottom.SplitterButtonVisible = VScrollBarVisible;
			if (editOldActiveView == editViewTop)
			{
				editViewBottom.Focus();
			}
		}

		/// <summary>
		/// Sets up a normal text edit control.
		/// </summary>
		public void SetupTextEdit()
		{
			SettingFile = null;
			ContextMenuVisible = true;
			IndicatorMarginVisible = false;
			LineNumberMarginVisible = false;
			SelectionMarginVisible = false;
			ScrollBars = ScrollBars.None;
			IndentType = EditIndentType.None;
			CaretWidth = 1;
		}

		/// <summary>
		/// Starts the incremental search.
		/// </summary>
		public void IncrementalSearch()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.IncrementalSearch();
			}
		}

		/// <summary>
		/// Aborts the incremental search.
		/// </summary>
		public void AbortIncrementalSearch()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.AbortIncrementalSearch();
			}
		}

		/// <summary>
		/// Displays the Options dialog.
		/// </summary>
		public void ShowOptions()
		{
			if (editOptionsDlg == null)
			{
				editOptionsDlg = new OptionsDlg(this, editSettings);
			}
			if (editOptionsDlg.ShowDialog() != DialogResult.Cancel)
			{
				SetupCommonColors();
				UpdateCharWidth();
				UpdateAll();
			}
		}

		/// <summary>
		/// Sets the text of the specified status bar panel.
		/// </summary>
		/// <param name="index">The index of the panel.</param>
		/// <param name="str">The text for the panel.</param>
		internal void SetPanelTextInternal(int index, string str)
		{
			if (StatusBarVisible)
			{
				if (!bCustomPanelText[index] && !InDesignMode)
				{
					editStatusBarPanels[index].Text = str;
				}
			}
		}

		/// <summary>
		/// Sets the specified status bar panel to display the specified text.
		/// </summary>
		/// <param name="index">The index of the panel for which text is to 
		/// be set.</param>
		/// <param name="str">The text for the panel.</param>
		public void SetPanelText(int index, string str)
		{
			if (StatusBarVisible)
			{
				editStatusBarPanels[index].Text = str;
				bCustomPanelText[index] = true;
			}
		}

		/// <summary>
		/// Restores the automatic setting of the specified panel text.
		/// </summary>
		/// <param name="index">The index of the panel for which the automatic
		/// panel text is to be set.</param>
		public void EnableAutoPanelText(int index)
		{
			bCustomPanelText[index] = false;
		}

		/// <summary>
		/// Gets the text of the specified status bar panel.
		/// </summary>
		/// <param name="index">The index of the panel.</param>
		/// <returns>The text of the panel.</returns>
		public string GetPanelText(int index)
		{
			if (index < editStatusBarPanels.Length)
			{
				return editStatusBarPanels[index].Text;
			}
			return string.Empty;
		}

		/// <summary>
		/// Cuts the selected text.
		/// </summary>
		public void Cut()
		{
			if (!Updateable)
			{
				return;
			}
			BeginLogCompositeAction(EditActionType.Cut);
			if (!HasSelection) 
			{
				Clipboard.SetDataObject(GetWholeString(CurrentLine), true);
				DeleteLine(CurrentLine);
			}
			else
			{
				if ((LineCount == 1) && (GetLineLength(1) == 0))
				{
					return;
				}
				Clipboard.SetDataObject(SelectedText, true);
				DeleteSelection();
			}
			EndLogCompositeAction();
			if (CanPaste != bCanPasteOld)
			{
				bCanPasteOld = CanPaste;
				OnCanPasteChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Copies the selected text.
		/// </summary>
		public void Copy()
		{
			if (!HasContent)
			{
				return;
			}
			if(!CopyWithoutSelection)
			{
				if(!HasSelection)
					return;
			}
			Clipboard.SetDataObject(SelectedText, true);
			if (CanPaste != bCanPasteOld)
			{
				bCanPasteOld = CanPaste;
				OnCanPasteChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Pastes the clipboard content.
		/// </summary>
		public void Paste()
		{
			if (!Updateable)
			{
				return;
			}
			BeginLogCompositeAction(EditActionType.Paste);
			DeleteSelection();
			IDataObject data = Clipboard.GetDataObject();
			if (data.GetDataPresent(DataFormats.Text) 
				|| data.GetDataPresent(DataFormats.UnicodeText))
			{
				if (KeepTabs)
				{
					Insert(data.GetData(DataFormats.UnicodeText).ToString());
				}
				else
				{
					EditLocationRange lcr = 
						Insert(data.GetData(DataFormats.UnicodeText).ToString());
					ConvertTabsSpaces(lcr, true);
				}
			}
			EndLogCompositeAction();
		}

		/// <summary>
		/// Automatically indents the smart-indent character at the specified 
		/// location.
		/// </summary>
		/// <param name="c">The character for smart indent.</param>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		public void SmartIndent(char c, int ln, int ch)
		{
			if ((!Updateable) ||(ln == 1))
			{
				return;
			}
			if (c == editSettings.SmartIndentBeginChar)
			{
				if (ch != GetFirstNonSpaceChar(ln - 1))
				{
					BeginLogCompositeAction(EditActionType.AutoIndent);
					Delete(ln, 1, ln, ch + 1);
					Insert(GetIndentString(GetFirstNonSpaceColumn(ln - 1)) 
						+ editSettings.SmartIndentBeginChar);
					EndLogCompositeAction();
				}
			}
			else
			{
				int newCh = ch;
				string indentString = string.Empty;
				int level = 0;
				for (int i = ln - 1; i > 0; i--)
				{
					if (IsFirstNonSpaceChar(i, editSettings.SmartIndentBeginChar))
					{
						if (level == 0)
						{
							newCh = GetFirstNonSpaceChar(i);
							indentString = GetIndentString(GetFirstNonSpaceColumn(i));
							break;
						}
						else
						{
							level++;
						}
					}
					else if (IsFirstNonSpaceChar(i, editSettings.SmartIndentEndChar))
					{
						level--;
					}
				}
				if (newCh != ch)
				{
					BeginLogCompositeAction(EditActionType.AutoIndent);
					Delete(ln, 1, ln, ch + 1);
					Insert(indentString + editSettings.SmartIndentEndChar);
					EndLogCompositeAction();
				}
			}
		}

		/// <summary>
		/// Processes the input of the specified character.
		/// </summary>
		/// <param name="ch">The character to be processed.</param>
		public void HandleKeyInput(char ch)
		{
			if (!Updateable)
			{
				return;
			}
			char chTemp = ch;
			if (CharacterCasing == CharacterCasing.Upper)
			{
				chTemp = Char.ToUpper(ch);
			}
			else if (CharacterCasing == CharacterCasing.Lower)
			{
				chTemp = Char.ToLower(ch);
			}
			BeginLogCompositeAction(EditActionType.Type);
			DeleteSelection();
			int lnLength = GetLineLength(CurrentLine);
			if (!InsertMode)
			{
				if (CurrentChar < lnLength + 1)
				{
					Delete(CurrentLineChar, 
						new EditLocation(CurrentLine, CurrentChar + 1));
				}
			}
			EditLocationRange lcr = Insert(chTemp.ToString());
			EndLogCompositeAction();
			if (IndentType == EditIndentType.Smart)
			{
				if (((lcr.End.C - 1) == GetFirstNonSpaceChar(CurrentLine))
					&& ((chTemp == editSettings.SmartIndentBeginChar) 
					|| (chTemp == editSettings.SmartIndentEndChar)))
				{
					SmartIndent(chTemp, CurrentLine, lcr.End.C - 1);
				}
			}
		}

		/// <summary>
		/// Handles the input of the Tab key.
		/// </summary>
		public void TabKey()
		{
			if (!Updateable)
			{
				return;
			}
			if (HasSelection)
			{
				if (Selection.Start.L == Selection.End.L)
				{
					BeginLogCompositeAction(EditActionType.Tab);
					DeleteSelection();
					Insert(GetTabString(CurrentColumn));
					EndLogCompositeAction();
				}
				else
				{
					IncreaseLineIndent();
				}
			}
			else
			{
				BeginLogCompositeAction(EditActionType.Tab);
				Insert(GetTabString(CurrentColumn));
				EndLogCompositeAction();
			}
		}

		/// <summary>
		/// Handles the input of the Shift+Tab key.
		/// </summary>
		public void ShiftTabKey()
		{
			if (!Updateable)
			{
				return;
			}
			if (HasSelection)
			{
				if (Selection.Start.L == Selection.End.L)
				{
					BeginLogCompositeAction(EditActionType.Tab);
					EditLocation selStart = new EditLocation(Selection.GetStart());
					UnSelect();
					RemovePreviousTabSpaces(selStart);
					EndLogCompositeAction();
				}
				else
				{
					DecreaseLineIndent();
				}
			}
			else
			{
				BeginLogCompositeAction(EditActionType.Tab);
				RemovePreviousTabSpaces(CurrentLineChar);
				EndLogCompositeAction();
			}
		}

		/// <summary>
		/// Removes tab spaces before the specified location.
		/// </summary>
		/// <param name="lc">The location before which tab spaces are to be 
		/// removed.</param>
		/// <returns>The location range of the removed tab spaces.</returns>
		internal EditLocationRange RemovePreviousTabSpaces(EditLocation lc)
		{
			return RemovePreviousTabSpaces(lc.L, lc.C);
		}

		/// <summary>
		/// Removes tab spaces before the specified location.
		/// </summary>
		/// <param name="ln">The line of the location before which tab spaces 
		/// are to be removed.</param>
		/// <param name="ch">The char of the location before which tab spaces 
		/// are to be removed.</param>
		/// <returns>The location range of the removed tab spaces.</returns>
		internal EditLocationRange RemovePreviousTabSpaces(int ln, int ch)
		{
			string strLine = GetStringObject(ln);
			if (ch == 1)
			{
				return EditLocationRange.Empty;
			}
			else if ((strLine[ch-2] != ' ') && (strLine[ch-2] != '\t'))
			{
				return EditLocationRange.Empty;
			}
			else 
			{
				int colOrig = GetColumnFromChar(ln, ch);
				int nTemp = (colOrig - 1)%TabSize;
				int colTemp = colOrig - ((nTemp == 0) ? TabSize : nTemp);
				int chTemp0 = GetCharFromColumn(ln, colTemp);
				int chTemp1 = ch;
				for (int j = ch - 1; j >= chTemp0; j--)
				{
					if ((strLine[j-1] == ' ') || (strLine[j-1] == '\t'))
					{
						chTemp1 = j;
					}
					else
					{
						break;
					}
				}
				Delete(ln, chTemp1, ln, ch);
				return new EditLocationRange(ln, chTemp1, ln, ch);
			}
		}

		/// <summary>
		/// Handles the input of the Enter key.
		/// </summary>
		public void EnterKey()
		{
			bool bEndOfLine = false;
			if (!Updateable)
			{
				return;
			}
			if (!Multiline)
			{
				return;
			}
			if (bISearch)
			{
				AbortIncrementalSearch();
				return;
			}
			BeginLogCompositeAction(EditActionType.Enter);
			DeleteSelection();
			int lnLength = GetLineLength(CurrentLine);
			int indentCol = -1;
			if ((lnLength == 0) && (CurrentChar > 1))
			{
				indentCol = CurrentChar;
			}
			if (CurrentChar >= lnLength + 1) 
			{
				if (CurrentChar > lnLength + 1)
				{
					MoveCaret(CurrentLineChar, new EditLocation(CurrentLine, lnLength + 1));
				}
				Insert(new EditLocation(CurrentLine, lnLength + 1), 
					editSettings.NewLine);
				bEndOfLine = true;
			}
			else 
			{
				Insert(CurrentLineChar, editSettings.NewLine);
			}
			EndLogCompositeAction();
			if (IndentType == EditIndentType.Block)
			{
				if (indentCol == -1)
				{
					indentCol = GetFirstNonSpaceColumn(CurrentLine - 1);
				}
				if ((indentCol > 1) && (!bEndOfLine))
				{
					Insert(GetIndentString(indentCol));
				}
				else if (indentCol > 1)
				{
					CurrentChar = indentCol;
				}
			}
			else if (IndentType == EditIndentType.Smart)
			{
				if (IsFirstNonSpaceChar(CurrentLine - 1, editSettings.SmartIndentBeginChar))
				{
					BeginLogCompositeAction(EditActionType.AutoIndent);
					Insert(GetIndentString(GetFirstNonSpaceColumn(CurrentLine - 1) 
						+ IndentSize));
					EndLogCompositeAction();
				}
				else
				{
					string strTemp = GetSmartRepeatString(CurrentLine - 1);
					if (strTemp != string.Empty)
					{
						BeginLogCompositeAction(EditActionType.AutoIndent);
						Insert(strTemp);
						EndLogCompositeAction();
					}
					else
					{
						if (indentCol == -1)
						{
							indentCol = GetFirstNonSpaceColumn(CurrentLine - 1);
						}
						if (indentCol > 1)
						{
							Insert(GetIndentString(indentCol));
						}
					}
				}
			}
		}

		/// <summary>
		/// Handles the input of the backspace key.
		/// </summary>
		public void BackSpaceKey()
		{
			if (!Updateable)
			{
				return;
			}
			if (bISearch)
			{
				return;
			}
			BeginLogCompositeAction(EditActionType.Backspace);
			if (!DeleteSelection())
			{
				DeleteCharBeforeCaret();
			}
			EndLogCompositeAction();
		}

		/// <summary>
		/// Handles the input of the Escape key.
		/// </summary>
		public void EscapeKey()
		{
			if (bISearch)
			{
				AbortIncrementalSearch();
				return;
			}
			if (HasSelection)
			{
				UnSelect();
			}
		}

		/// <summary>
		/// Handles the input of the Delete key.
		/// </summary>
		public void DeleteKey()
		{
			if (!Updateable)
			{
				return;
			}
			if (!DeleteSelection())
			{
				DeleteCharAfterCaret();
				if (!InsertMode)
				{
					UpdateCaretSize();
				}
			}
		}

		/// <summary>
		/// Moves the caret one char right and redraws the caret.
		/// </summary>
		public void CharRight()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.CharRight();
			}
		}

		/// <summary>
		/// Moves the caret one char right and select the passed range.
		/// </summary>
		public void CharRightExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.CharRightExtend();
			}
		}

		/// <summary>
		/// Moves the caret one char left and redraws the caret.
		/// </summary>
		public void CharLeft()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.CharLeft();
			}
		}

		/// <summary>
		/// Moves the caret one char left and selects the passed range.
		/// </summary>
		public void CharLeftExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.CharLeftExtend();
			}
		}

		/// <summary>
		/// Moves the caret one word left.
		/// </summary>
		public void WordLeft()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.WordLeft();
			}
		}

		/// <summary>
		/// Moves the caret one word left and selects the passed range.
		/// </summary>
		public void WordLeftExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.WordLeftExtend();
			}
		}

		/// <summary>
		/// Moves the caret one word right.
		/// </summary>
		public void WordRight()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.WordRight();
			}
		}

		/// <summary>
		/// Moves the caret one word right and selects the passed range.
		/// </summary>
		public void WordRightExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.WordRightExtend();
			}
		}

		/// <summary>
		/// Moves the caret up one line.
		/// </summary>
		public void LineUp()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.LineUp();
			}
		}

		/// <summary>
		/// Moves the caret up one line and selects the passed range.
		/// </summary>
		public void LineUpExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.LineUpExtend();
			}
		}

		/// <summary>
		/// Moves the caret down one line.
		/// </summary>
		public void LineDown()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.LineDown();
			}
		}

		/// <summary>
		/// Moves the caret down one line and selects the passed range.
		/// </summary>
		public void LineDownExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.LineDownExtend();
			}
		}

		/// <summary>
		/// Scrolls the contents up by one page.
		/// </summary>
		public void PageUp()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.PageUp();
			}
		}

		/// <summary>
		/// Scrolls the contents up by one page and selects the passed range.
		/// </summary>
		public void PageUpExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.PageUpExtend();
			}
		}

		/// <summary>
		/// Scrolls the contents down by one page.
		/// </summary>
		public void PageDown()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.PageDown();
			}
		}

		/// <summary>
		/// Scrolls the contents down by one page and selects the passed 
		/// range.
		/// </summary>
		public void PageDownExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.PageDownExtend();
			}
		}

		/// <summary>
		/// Moves the caret to the top of the current page.
		/// </summary>
		public void PageTop()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.PageTop();
			}
		}

		/// <summary>
		/// Moves the caret to the top of the current page and selects 
		/// the passed range.
		/// </summary>
		public void PageTopExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.PageTopExtend();
			}
		}

		/// <summary>
		/// Moves the caret to the bottom of the current page.
		/// </summary>
		public void PageBottom()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.PageBottom();
			}
		}

		/// <summary>
		/// Moves the caret to the bottom of the current page and selects
		/// the passed range.
		/// </summary>
		public void PageBottomExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.PageBottomExtend();
			}
		}

		/// <summary>
		/// Moves the caret to the start of the line.
		/// </summary>
		public void LineStart()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.LineStart();
			}
		}

		/// <summary>
		/// Moves the caret to the start of the line and selects the passed 
		/// range.
		/// </summary>
		public void LineStartExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.LineStartExtend();
			}
		}

		/// <summary>
		/// Moves the caret to the end of the line.
		/// </summary>
		public void LineEnd()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.LineEnd();
			}
		}

		/// <summary>
		/// Moves the caret to the end of the line and selects the passed 
		/// range.
		/// </summary>
		public void LineEndExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.LineEndExtend();
			}
		}

		/// <summary>
		/// Moves the caret to the start of the document.
		/// </summary>
		public void DocumentStart()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.DocumentStart();
			}
		}

		/// <summary>
		/// Moves the caret to the start of the document and selects 
		/// the passed range.
		/// </summary>
		public void DocumentStartExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.DocumentStartExtend();
			}
		}

		/// <summary>
		/// Moves the caret to the end of the document.
		/// </summary>
		public void DocumentEnd()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.DocumentEnd();
			}
		}

		/// <summary>
		/// Moves the caret to the end of the document and selects the 
		/// passed range.
		/// </summary>
		public void DocumentEndExtend()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.DocumentEndExtend();
			}
		}

		/// <summary>
		/// Invokes the GoTo dialog.
		/// </summary>
		public void GoTo()
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.GoTo();
			}
		}

		/// <summary>
		/// Moves the caret to the specified line.
		/// </summary>
		public void GoToLine(int ln)
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.GoToLine(ln);
			}
		}

		/// <summary>
		/// Makes the specified line display in the center of the viewport.
		/// </summary>
		/// <param name="ln">The line to be centered.</param>
		public void CenterLine(int ln)
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.CenterLine(ln);
			}
		}

		/// <summary>
		/// Clears the contents of the edit.
		/// </summary>
		public void ClearContents()
		{
			UnSelect();
			ClearUndo();
			ClearRedo();
			ClearBraceMatching();
			editData.Clear();
			editMaxColumn = 1;
			CurrentLine = 1;
			CurrentChar = 1;
		}

		/// <summary>
		/// Clears all text from the control.
		/// </summary>
		public void Clear()
		{
			Delete(FirstLineChar, LastLineChar);
		}

		/// <summary>
		/// Appends the specified text to the current contents.
		/// </summary>
		/// <param name="str">The text to be added.</param>
		public void AppendText(string str)
		{
			Insert(LastLineChar, str);
		}

		/// <summary>
		/// Gets the user data for the specified line.
		/// </summary>
		/// <param name="ln">The line at which the user data is to be 
		/// obtained.</param>
		/// <returns>The object of the user data.</returns>
		public object GetUserData(int ln)
		{
			return editData.GetUserData(ln);
		}

		/// <summary>
		/// Sets the user data for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the user data is to be set.</param>
		/// <param name="data">The object of the user data.</param>
		public void SetUserData(int ln, object data)
		{
			editData.SetUserData(ln, data);
		}

		/// <summary>
		/// Tests if the specified line is highlighted.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <returns>true if the line is highlighted; otherwise, false.</returns>
		public bool IsHighlighted(int ln)
		{
			return editData.IsHighlighted(ln);
		}

		/// <summary>
		/// Highlights the specified line.
		/// </summary>
		/// <param name="ln">The line to be highlighted.</param>
		public void AddHighlight(int ln)
		{
			editData.AddHighlight(ln);
			RedrawLine(ln);
		}

		/// <summary>
		/// Highlights the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range to 
		/// be highlighted.</param>
		/// <param name="lnEnd">The ending line of the line range to be 
		/// highlighted.</param>
		public void AddHighlight(int lnStart, int lnEnd)
		{
			editData.AddHighlight(lnStart, lnEnd);
			RedrawLines(lnStart, lnEnd);
		}

		/// <summary>
		/// Removes highlight for the specified line.
		/// </summary>
		/// <param name="ln">The line for which highlight is to be 
		/// removed.</param>
		public void RemoveHighlight(int ln)
		{
			editData.RemoveHighlight(ln);
			RedrawLine(ln);
		}

		/// <summary>
		/// Removes highlight for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for  
		/// which highlight is to be removed.</param>
		/// <param name="lnEnd">The ending line of the line range for which 
		/// highlight is to be removed.</param>
		public void RemoveHighlight(int lnStart, int lnEnd)
		{
			editData.RemoveHighlight(lnStart, lnEnd);
			RedrawLines(lnStart, lnEnd);
		}

		/// <summary>
		/// Removes all highlight for lines.
		/// </summary>
		public void RemoveAllHighlight()
		{
			editData.RemoveAllHighlight();
			Redraw();
		}

		/// <summary>
		/// Tests if the specified line has a custom foreground color.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <returns>true if the line has a custom foreground color; 
		/// otherwise, false.</returns>
		public bool IsCustomForeColor(int ln)
		{
			return editData.IsCustomForeColor(ln);
		}

		/// <summary>
		/// Sets the foreground color of the specified line to be a custom 
		/// color.
		/// </summary>
		/// <param name="ln">The line for which the foreground color is to 
		/// be set.</param>
		/// <param name="clr">The custom foreground color for the line.</param>
		public void SetCustomForeColor(int ln, Color clr)
		{
			editData.SetCustomForeColor(ln, clr);
			RedrawLine(ln);
		}

		/// <summary>
		/// Sets the foreground color of the specified line range to be a 
		/// custom color.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for 
		/// which the custom foreground color is to be set.</param>
		/// <param name="lnEnd">The ending line of the line range for which 
		/// the custom foreground color is to be set.</param>
		/// <param name="clr">The custom foreground color for the lines.</param>
		public void SetCustomForeColor(int lnStart, int lnEnd, Color clr)
		{
			editData.SetCustomForeColor(lnStart, lnEnd, clr);
			RedrawLines(lnStart, lnEnd);
		}

		/// <summary>
		/// Removes the custom foreground color for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the custom foreground color 
		/// is to be removed.</param>
		public void RemoveCustomForeColor(int ln)
		{
			editData.RemoveCustomForeColor(ln);
			RedrawLine(ln);
		}

		/// <summary>
		/// Removes custom foreground colors for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for 
		/// which custom foreground colors is to be removed.</param>
		/// <param name="lnEnd">The ending line of the line range for which 
		/// custom foreground colors is to be removed.</param>
		public void RemoveCustomForeColor(int lnStart, int lnEnd)
		{
			editData.RemoveCustomForeColor(lnStart, lnEnd);
			RedrawLines(lnStart, lnEnd);
		}

		/// <summary>
		/// Removes all the custom foreground colors for lines.
		/// </summary>
		public void RemoveAllCustomForeColor()
		{
			editData.RemoveAllCustomForeColor();
			Redraw();
		}

		/// <summary>
		/// Tests if the specified line has a custom background color.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <returns>true if the line has a custom background color; 
		/// otherwise, false.</returns>
		public bool IsCustomBackColor(int ln)
		{
			return editData.IsCustomBackColor(ln);
		}

		/// <summary>
		/// Sets the background color of the specified line to be a custom 
		/// color.
		/// </summary>
		/// <param name="ln">The line for which the background color is to 
		/// be set.</param>
		/// <param name="clr">The custom background color for the line.</param>
		public void SetCustomBackColor(int ln, Color clr)
		{
			editData.SetCustomBackColor(ln, clr);
			RedrawLine(ln);
		}

		/// <summary>
		/// Sets the background color of the specified line range to be a 
		/// custom color.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for 
		/// which the custom background color is to be set.</param>
		/// <param name="lnEnd">The ending line of the line range for which 
		/// the custom background color is to be set.</param>
		/// <param name="clr">The custom background color for the lines.</param>
		public void SetCustomBackColor(int lnStart, int lnEnd, Color clr)
		{
			editData.SetCustomBackColor(lnStart, lnEnd, clr);
			RedrawLines(lnStart, lnEnd);
		}

		/// <summary>
		/// Removes the custom background color for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the custom background color 
		/// is to be removed.</param>
		public void RemoveCustomBackColor(int ln)
		{
			editData.RemoveCustomBackColor(ln);
			RedrawLine(ln);
		}

		/// <summary>
		/// Removes custom background colors for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for 
		/// which custom background colors is to be removed.</param>
		/// <param name="lnEnd">The ending line of the line range for which 
		/// custom background colors is to be removed.</param>
		public void RemoveCustomBackColor(int lnStart, int lnEnd)
		{
			editData.RemoveCustomBackColor(lnStart, lnEnd);
			RedrawLines(lnStart, lnEnd);
		}

		/// <summary>
		/// Removes all the custom background colors for lines.
		/// </summary>
		public void RemoveAllCustomBackColor()
		{
			editData.RemoveAllCustomBackColor();
			Redraw();
		}

		/// <summary>
		/// Sets up the default indicators.
		/// </summary>
		private void SetupDefaultIndicators()
		{
			AddIndicator(new EditBookmark());
			AddIndicator(new EditBreakpoint());
		}

		/// <summary>
		/// Adds the specified indicator to the indicator list.
		/// </summary>
		/// <param name="idc">The name of the indicator to be added.</param>
		public void AddIndicator(EditIndicator idc)
		{
			editIndicatorList.Add(idc);
		}

		/// <summary>
		/// Gets the indicator index for the specified indicator name.
		/// </summary>
		/// <param name="indicatorName">The name of the indicator.</param>
		/// <returns>The index of the indicator.</returns>
		internal int GetIndicatorIndex(string indicatorName)
		{
			return editIndicatorList.GetIndicatorIndex(indicatorName);
		}

		/// <summary>
		/// Gets the indicator name for the specified indicator index.
		/// </summary>
		/// <param name="indicatorIndex">The index of the indicator.</param>
		/// <returns>The name of the indicator.</returns>
		internal string GetIndicatorName(int indicatorIndex)
		{
			return editIndicatorList[indicatorIndex].GetName();
		}

		/// <summary>
		/// Tests if the specified indicator appears at the specified line.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <param name="indicatorName">The name of the indicator.</param>
		/// <returns>true if the indicator appears at the line; otherwise,
		/// false.</returns>
		public bool HasIndicator(int ln, string indicatorName)
		{
			return editData.HasIndicator(ln, indicatorName);
		}

		/// <summary>
		/// Adds an indicator at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the indicator is to be 
		/// added.</param>
		/// <param name="indicatorName">The name of the indicator.</param>
		public void AddIndicator(int ln, string indicatorName)
		{
			if (IsValidLine(ln))
			{
				editData.AddIndicator(ln, indicatorName);
				Redraw();
			}
		}

		/// <summary>
		/// Removes an indicator at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the indicator is to be 
		/// removed.</param>
		/// <param name="indicatorName">The name of the indicator.</param>
		public void RemoveIndicator(int ln, string indicatorName)
		{
			if (IsValidLine(ln))
			{
				editData.RemoveIndicator(ln, indicatorName);
				Redraw();
			}
		}

		/// <summary>
		/// Toggles the specified indicator at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the indicator is to be 
		/// toggled.</param>
		/// <param name="indicatorName">The name of the indicator.</param>
		public void ToggleIndicator(int ln, string indicatorName)
		{
			if (HasIndicator(ln, indicatorName))
			{
				RemoveIndicator(ln, indicatorName);
			}
			else
			{
				AddIndicator(ln, indicatorName);
			}
		}

		/// <summary>
		/// Goes to the previous indicator specified.
		/// </summary>
		/// <param name="indicatorName">The name of the indicator.</param>
		public void PreviousIndicator(string indicatorName)
		{
			int prevLn = editData.GetPreviousIndicator(CurrentLine, indicatorName);
			if (prevLn != -1)
			{
				GoToLine(prevLn);
			}
		}

		/// <summary>
		/// Goes to the previous indicator specified.
		/// </summary>
		/// <param name="indicatorName">The name of the indicator.</param>
		public void NextIndicator(string indicatorName)
		{
			int nextLn = editData.GetNextIndicator(CurrentLine, indicatorName);
			if (nextLn != -1)
			{
				GoToLine(nextLn);
			}
		}

		/// <summary>
		/// Clears all the indicators with the specified name.
		/// </summary>
		/// <param name="indicatorName">The name of the indicator.</param>
		public void ClearIndicators(string indicatorName)
		{
			int lnLast = LineCount;
			for (int i = 1; i <= lnLast; i++)
			{
				RemoveIndicator(i, indicatorName);
			}
		}

		/// <summary>
		/// Toggles the bookmark at the current line.
		/// </summary>
		public void ToggleBookmark()
		{
			ToggleBookmark(CurrentLine);
		}

		/// <summary>
		/// Toggles the bookmark at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the bookmark is to be 
		/// toggled.</param>
		public void ToggleBookmark(int ln)
		{
			ToggleIndicator(ln, "Bookmark");
		}

		/// <summary>
		/// Adds a bookmark at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the bookmark is to be 
		/// added.</param>
		public void AddBookmark(int ln)
		{
			AddIndicator(ln, "Bookmark");
		}

		/// <summary>
		/// Removes the bookmark at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the bookmark is to be 
		/// removed.</param>
		public void RemoveBookmark(int ln)
		{
			RemoveIndicator(ln, "Bookmark");
		}

		/// <summary>
		/// Goes to the next bookmark.
		/// </summary>
		public void NextBookmark()
		{
			NextIndicator("Bookmark");
		}

		/// <summary>
		/// Goes to the previous bookmark.
		/// </summary>
		public void PreviousBookmark()
		{
			PreviousIndicator("Bookmark");
		}

		/// <summary>
		/// Clears all the bookmarks.
		/// </summary>
		public void ClearBookmarks()
		{
			ClearIndicators("Bookmark");
		}

		/// <summary>
		/// Creates a new breakpoint.
		/// </summary>
		public void NewBreakpoint()
		{
			AddIndicator(CurrentLine, "Breakpoint");
		}

		/// <summary>
		/// Adds a breakpoint at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the breakpoint is to be 
		/// added.</param>
		public void AddBreakpoint(int ln)
		{
			AddIndicator(ln, "Breakpoint");
		}

		/// <summary>
		/// Toggles the breakpoint at the current line.
		/// </summary>
		public void ToggleBreakpoint()
		{
			ToggleBreakpoint(CurrentLine);
		}

		/// <summary>
		/// Toggles the breakpoint at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the breakpoint is to be 
		/// toggled.</param>
		public void ToggleBreakpoint(int ln)
		{
			ToggleIndicator(ln, "Breakpoint");
		}

		/// <summary>
		/// Removes the bookmark at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the breakpoint is to be 
		/// removed.</param>
		public void RemoveBreakpoint(int ln)
		{
			RemoveIndicator(ln, "Breakpoint");
		}

		/// <summary>
		/// Clears all the breakpoints.
		/// </summary>
		public void ClearBreakpoints()
		{
			editData.ClearIndicator("Breakpoint");
			Redraw();
		}

		/// <summary>
		/// Hides the selected text as an outlining block.
		/// </summary>
		public void HideAsOutlining()
		{
			if (!OutliningEnabled)
			{
				return;
			}
			if (HasSelection)
			{
				EditLocationRange lcrNorm = editSelection.Normalize();
				if (editData.HideAsOutlining(lcrNorm))
				{
					UpdateVScrollBar();
					ScrollToViewCaret();
					Redraw();
				}
			}
		}
		
		/// <summary>
		/// Hides the given range as an outlining block.
		/// </summary>
		/// /// <param name="ln">The range to hide
		/// </param>
		public void HideAsOutlining(EditLocationRange lcr)
		{
			if (!OutliningEnabled)
			{
				return;
			}
			
			EditLocationRange lcrNorm = lcr.Normalize();
			if (editData.HideAsOutlining(lcrNorm))
			{
				UpdateVScrollBar();
				ScrollToViewCaret();
				Redraw();
			}
		}

		/// <summary>
		/// Removes the outlining block at the current line.
		/// </summary>
		public void StopHidingCurrent()
		{
			if (!OutliningEnabled)
			{
				return;
			}
			ExpandOutlining(CurrentLine);
			editData.StopHidingCurrent(CurrentLine);
			Redraw();
		}

		/// <summary>
		/// Toggles the outlining at the current line.
		/// </summary>
		public void ToggleOutliningExpansion()
		{
			if (!OutliningEnabled)
			{
				return;
			}
			ToggleOutliningExpansion(CurrentLine);
		}

		/// <summary>
		/// Toggles the outlining at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the outlining is to be toggled.
		/// </param>
		/// <returns>true if an outlining has been toggled; otherwise, false.
		/// </returns>
		public bool ToggleOutliningExpansion(int ln)
		{
			if (!OutliningEnabled)
			{
				return false;
			}
			EditOutlining otln = editData.GetLeafOutlining(ln);
			if (!otln.IsRoot)
			{
				UnSelect();
				otln.Collapsed = !otln.Collapsed;
				OutliningCollapsedChangedEventArgs ocea = 
					new OutliningCollapsedChangedEventArgs(otln, otln.Collapsed);
				OnOutliningCollapsedChanged(ocea);
				UpdateVScrollBar();
				ScrollToViewCaret();
				Redraw();
				return true;
			}
			return false;
		}

		/// <summary>
		/// Expands the outlining at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the outlining is to be expanded.
		/// </param>
		public void ExpandOutlining(int ln)
		{
			if (!OutliningEnabled)
			{
				return;
			}
			EditOutlining otln = editData.GetLeafOutlining(ln);
			if ((!otln.IsRoot) && otln.Collapsed)
			{
				UnSelect();
				if (otln.Collapsed)
				{
					otln.Collapsed = false;
					OutliningCollapsedChangedEventArgs ocea = 
						new OutliningCollapsedChangedEventArgs(otln, otln.Collapsed);
					OnOutliningCollapsedChanged(ocea);
				}
				UpdateVScrollBar();
				ScrollToViewCaret();
				Redraw();
			}
		}

		/// <summary>
		/// Collapses the outlining at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the outlining is to be 
		/// collapsed.</param>
		public void CollapseOutlining(int ln)
		{
			if (!OutliningEnabled)
			{
				return;
			}
			EditOutlining otln = editData.GetLeafOutlining(ln);
			if ((!otln.IsRoot) && (!otln.Collapsed))
			{
				UnSelect();
				if (!otln.Collapsed)
				{
					otln.Collapsed = true;
					OutliningCollapsedChangedEventArgs ocea = 
						new OutliningCollapsedChangedEventArgs(otln, otln.Collapsed);
					OnOutliningCollapsedChanged(ocea);
				}
				UpdateVScrollBar();
				ScrollToViewCaret();
				Redraw();
			}
		}

		/// <summary>
		/// Toggles all the outlinings.
		/// </summary>
		public void ToggleAllOutlining()
		{
			if (!OutliningEnabled)
			{
				return;
			}
			if (editData.OutliningRoot.HasExpandedDescendant)
			{
				editData.OutliningRoot.CollapseAllDescendants();
			}
			else
			{
				editData.OutliningRoot.ExpandAllDescendants();
			}
			Redraw();
		}

		/// <summary>
		/// Collapses all the outlinings.
		/// </summary>
		public void CollapseToDefinitions()
		{
			if (!OutliningEnabled)
			{
				return;
			}
			editData.OutliningRoot.CollapseAllDescendants();
			Redraw();
		}

		/// <summary>
		/// Stops the outlining function.
		/// </summary>
		public void StopOutlining()
		{
			if (!OutliningEnabled)
			{
				return;
			}
			editData.OutliningRoot.ExpandAllDescendants();
			editData.RemoveAllOutlining();
			Redraw();
			AutomaticOutliningEnabled = false;
		}

		/// <summary>
		/// Starts the outlining function.
		/// </summary>
		public void StartAutomaticOutlining()
		{
			if (!OutliningEnabled)
			{
				return;
			}
			editData.OutliningRoot.ExpandAllDescendants();
			editData.RemoveAllOutlining();
			Redraw();
			AutomaticOutliningEnabled = true;
			editData.StartOutliningThread();
		}

		/// <summary>
		/// Marks all the occurances of the specified string in the document.
		/// </summary>
		/// <param name="str">The string to be searched.</param>
		/// <param name="bMatchCase">A value indicating whether matching is 
		/// case-sensitive.</param>
		/// <param name="bMatchWholeWord">A value indicating whether matching
		/// is whole-word based.</param>
		public void MarkAll(string str, bool bMatchCase, bool bMatchWholeWord)
		{
			bool bFound = false;
			EditLocation lcTemp = new EditLocation(1, 1);
			EditLocationRange lcrTemp;
			EditLocationRange lcrFoundOld = EditLocationRange.Empty;
			BeginUpdate();
			while ((lcrTemp = Find(lcTemp, str, 
				bMatchCase, bMatchWholeWord, true, false, false, false, true)) 
				!= EditLocationRange.Empty)
			{
				bFound = true;
				if (lcrTemp == lcrFoundOld)
				{
					break;
				}
				if (lcrFoundOld == EditLocationRange.Empty)
				{
					lcrFoundOld = lcrTemp;
				}
				AddBookmark(lcrTemp.Start.L);
				lcTemp = lcrTemp.End;
			}
			EndUpdate();
			if (bFound)
			{
				Redraw();
			}
			else
			{
				ShowInfoMessage(GetResourceString("TextNotFound"), 
					GetResourceString("MarkAll"));
			}
		}

		/// <summary>
		/// Replaces all the occurances of the specified string with a new 
		/// string in the document.
		/// </summary>
		/// <param name="strOld">The string to be replaced.</param>
		/// <param name="strNew">The string replaced to.</param>
		/// <param name="bMatchCase">A value indicating whether matching is 
		/// case-sensitive.</param>
		/// <param name="bMatchWholeWord">A value indicating whether matching
		/// is whole-word based.</param>
		public void ReplaceAll(string strOld, string strNew, 
			bool bMatchCase, bool bMatchWholeWord)
		{
			int count = 0;
			EditLocation lcTemp = new EditLocation(1, 1);
			EditLocationRange lcrTemp;
			if (LogReplaceAllEnabled)
			{
				BeginLogCompositeAction(EditActionType.ReplaceAll);
			}
			while ((lcrTemp = Find(lcTemp, strOld, 
				bMatchCase, bMatchWholeWord, true, false, false, false, false)) 
				!= EditLocationRange.Empty)
			{
				count++;
				Select(lcrTemp);
				ReplaceSelection(strNew);
				lcTemp = CurrentLineChar;
			}
			if (LogReplaceAllEnabled)
			{
				EndLogCompositeAction();
			}
			if (count > 0)
			{
				ShowInfoMessage(count.ToString() + 
					GetResourceString("OccurrenceReplaced"), 
					GetResourceString("ReplaceAll"));
			}
			else
			{
				ShowInfoMessage(GetResourceString("TextNotFound"), 
					GetResourceString("ReplaceAll"));
			}
		}

		/// <summary>
		/// Shows a messagebox with an information icon.
		/// </summary>
		/// <param name="msg">The message to be shown.</param>
		/// <param name="title">The title of the messagebox.</param>
		internal static void ShowInfoMessage(string msg, string title)
		{
			MessageBox.Show(msg, title, MessageBoxButtons.OK, 
				MessageBoxIcon.Information);
		}

		/// <summary>
		/// Shows a messagebox with an error icon.
		/// </summary>
		/// <param name="msg">The message to be shown.</param>
		/// <param name="title">The title of the messagebox.</param>
		internal static void ShowErrorMessage(string msg, string title)
		{
			MessageBox.Show(msg, title, MessageBoxButtons.OK, 
				MessageBoxIcon.Error);
		}

		/// <summary>
		/// Gets the file filter for all the default input file types.
		/// </summary>
		/// <returns>The file filters for the Open File dialog.</returns>
		internal string GetOpenFileFilters()
		{
			return GetFileFilter() +
				"Text Files (*.txt)|*.txt" + "|" +
				"All Files (*.*)|*.*";
		}

		/// <summary>
		/// Gets the file filter for all the default output file types.
		/// </summary>
		/// <returns>The file filters for the Save File dialog.</returns>
		internal string GetSaveFileFilters()
		{
			return GetFileFilter() + 
				"Text files (*.txt)|*.txt" + "|" + 
				"HTML files (*.htm)|*.htm" + "|" + 
				"RTF files (*.rtf)|*.rtf" + "|" + 
				"All files|*";
		}

		/// <summary>
		/// Gets the file filter for the current language.
		/// </summary>
		/// <returns>The file filters for the current language.</returns>
		internal string GetFileFilter()
		{
			string strTemp = string.Empty;
			if (FileExtension != string.Empty)
			{
				strTemp = (FileDescription == string.Empty) ? "Default" :
					FileDescription;
				return strTemp + " (" + "*." + FileExtension + ")"
					+ "|*." + FileExtension + "|";
			}
			return strTemp;
		}

		/// <summary>
		/// Tests if the specified string is a color group name.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <returns>true if the string is a color group; otherwise, false.</returns>
		public bool IsColorGroup(string str)
		{
			return editSettings.IsColorGroup(str);
		}

		/// <summary>
		/// Adds a new color group.
		/// </summary>
		/// <param name="groupName">The name of the color group to be added.
		/// </param>
		/// <param name="foreColor">The foreground color of the color group 
		/// to be added.</param>
		/// <param name="backColor">The background color fo the color group 
		/// to be added.</param>
		/// <param name="isAutoForeColor">A value indicating whether the 
		/// foreground color is automatically set to the foreground color 
		/// of normal text.</param>
		/// <param name="isAutoBackColor">A value indicating whether the 
		/// background color is automatically set to the background color 
		/// of normal text.</param>
		/// <param name="colorGroupType">The type of the color group to be 
		/// added.</param>
		public void AddColorGroup(string groupName, Color foreColor, 
			Color backColor, bool isAutoForeColor, bool isAutoBackColor, 
			EditColorGroupType colorGroupType)
		{
			if (editSettings.AddColorGroup(groupName, foreColor, backColor, 
				isAutoForeColor, isAutoBackColor, colorGroupType))
			{
				InvalidateColoring();
			}
		}

		/// <summary>
		/// Tests if the specified string is a keyword.
		/// </summary>
		/// <param name="str">The string to be tested.</param>
		/// <returns>true if the string is a keyword; otherwise, false.</returns>
		public bool IsKeyword(string str)
		{
			return editSettings.IsKeyword(str);
		}

		/// <summary>
		/// Gets the list of keywords for the specified color group.
		/// </summary>
		/// <param name="colorGroup">The color group for the keywords.</param>
		/// <returns>The list of keywords for the color group.</returns>
		public ArrayList GetKeywordList(string colorGroup)
		{
			ArrayList keywords = new ArrayList();
			EditKeywordInfo ki;
			for (int i = 0; i < editSettings.KeywordInfoList.Count; i++)
			{
				ki = (EditKeywordInfo)editSettings.KeywordInfoList[i];
				if (ki.ColorGroup == colorGroup)
				{
					keywords.Add(ki.Keyword);
				}
			}
			return keywords;
		}

		/// <summary>
		/// Adds a new keyword for syntax coloring.
		/// </summary>
		/// <param name="keyword">The keyword to be added.</param>
		/// <param name="colorGroup">The color group of the keyword.</param>
		public void AddKeyword(string keyword, string colorGroup)
		{
			if (editSettings.AddKeyword(keyword, colorGroup))
			{
				InvalidateColoring();
			}
		}

		/// <summary>
		/// Removes a keyword.
		/// </summary>
		/// <param name="keyword">The keyword to be removed.</param>
		public void RemoveKeyword(string keyword)
		{
			if (editSettings.RemoveKeyword(keyword))
			{
				InvalidateColoring();
			}
		}

		/// <summary>
		/// Adds a new tag with the specified attributes.
		/// </summary>
		/// <param name="beginTag">The beginning characters for the tag.</param>
		/// <param name="endTag">The ending characters for the tag.</param>
		/// <param name="escapeChar">The escape char for the tag.</param>
		/// <param name="bMultiLine">A value indicating whether the tag is 
		/// multiline.</param>
		/// <param name="colorGroup">The color group for the tag.</param>
		public void AddTag(string beginTag, string endTag,
			string escapeChar, bool bMultiLine, string colorGroup)
		{
			if (editSettings.AddTag(beginTag, endTag, escapeChar, 
				bMultiLine, colorGroup))
			{
				InvalidateColoring();
			}
		}

		/// <summary>
		/// Removes a tag with the specified begin tag and end tag.
		/// </summary>
		/// <param name="beginTag">The beginning characters for the tag.</param>
		/// <param name="endTag">The ending characters for the tag.</param>
		public void RemoveTag(string beginTag, string endTag)
		{
			if (editSettings.RemoveTag(beginTag, endTag))
			{
				InvalidateColoring();
			}
		}

		/// <summary>
		/// Update the parser for syntax coloring.
		/// </summary>
		internal void UpdateParser()
		{
			if (editSettings.KeywordChanged || editSettings.TagChanged)
			{
				string langRegExp = editSettings.GetLangRegExp();
				if (langRegExp.Length < 2048)
				{
					editData.LangRegex = new Regex(langRegExp, 
						editData.RegExpOptions | RegexOptions.Compiled);
					editData.LangRegex.Match(string.Empty);
				}
				else
				{
					editData.LangRegex = new Regex(langRegExp, 
						editData.RegExpOptions);
				}
				if (editSettings.TagChanged)
				{
					editData.BeginTagCheckingRegex = new Regex(
						editSettings.GetBeginTagCheckingRegexp(), 
						editData.RegExpOptions | RegexOptions.Compiled);
					editData.MultiLineBeginTagsRegex = new Regex(
						editSettings.GetMultiLineBeginTagsRegExp(),
						editData.RegExpOptions | RegexOptions.Compiled);
					string strTemp;
					if ((strTemp = editSettings.GetSingleLineCommentRegExp()) != string.Empty)
					{
						editData.SingleLineCommentRegex = new Regex(
							strTemp, editData.RegExpOptions | RegexOptions.Compiled);
						editData.LangRegex.Match(string.Empty);
					}
					else
					{
						editData.SingleLineCommentRegex = null;
					}
					if ((strTemp = editSettings.GetSingleLineOutliningRegExp()) != string.Empty)
					{
						editData.SingleLineOutliningRegex = new Regex(
							strTemp, editData.RegExpOptions | RegexOptions.Compiled);
						editData.LangRegex.Match(string.Empty);
					}
					else
					{
						editData.SingleLineOutliningRegex = null;
					}
					if ((strTemp = editSettings.GetMultiLineOutlingBeginTagCheckingRegExp()) 
						!= string.Empty)
					{
						editData.MultiLineOutliningBeginTagCheckingRegex = new Regex(
							strTemp, editData.RegExpOptions | RegexOptions.Compiled);
						editData.LangRegex.Match(string.Empty);
					}
					else
					{
						editData.MultiLineOutliningBeginTagCheckingRegex = null;
					}
					SyntaxColoringEnabled = true;
				}
				editSettings.KeywordChanged = false;
				editSettings.TagChanged = false;
			}
		}

		/// <summary>
		/// Updates edit states based on settings.
		/// </summary>
		internal void ProcessSettings()
		{
			editData.RegExpOptions = RegexOptions.Singleline;
			if (!MatchCase)
			{
				editData.RegExpOptions |= RegexOptions.IgnoreCase;
			}
			Font = editSettings.EditFont;
			InvalidateColoring();
		}

		/// <summary>
		/// Invalidates the current syntax coloring.
		/// </summary>
		internal void InvalidateColoring()
		{
			UpdateParser();
			editData.ClearMultiLineBlockFrom(1, 1);
			editData.InvalidateRangeInfo(FirstLineChar, LastLineChar);
			Redraw();
		}

		/// <summary>
		/// Redraws the views.
		/// </summary>
		public void Redraw()
		{
			if (editViewTop != null)
			{
				editViewTop.Redraw();
			}
			if (editViewBottom != null)
			{
				editViewBottom.Redraw();
			}
		}

		/// <summary>
		/// Updates the widths for common chars.
		/// </summary>
		internal void UpdateCharWidth()
		{
			editCharWidthUpdated = false;
			if (editViewTop != null)
			{
				editViewTop.MeasureCharWidth();
			}
			else if (editViewBottom != null)
			{
				editViewBottom.MeasureCharWidth();
			}
		}

		/// <summary>
		/// Redraws the selection margin.
		/// </summary>
		public void RedrawSelectionMargin()
		{
			if (editViewTop != null)
			{
				editViewTop.RedrawSelectionMargin();
			}
			if (editViewBottom != null)
			{
				editViewBottom.RedrawSelectionMargin();
			}
		}

		/// <summary>
		/// Resets the views.
		/// </summary>
		public void ResetView()
		{
			if (editViewTop != null)
			{
				editViewTop.ResetView();
			}
			if (editViewBottom != null)
			{
				editViewBottom.ResetView();
			}
		}

		/// <summary>
		/// Updates all the controls and contents in the views.
		/// </summary>
		public void UpdateAll()
		{
			if (editViewTop != null)
			{
				editViewTop.UpdateControls();
			}
			if (editViewBottom != null)
			{
				editViewBottom.UpdateControls();
			}
		}

		/// <summary>
		/// Updates the caret size.
		/// </summary>
		internal void UpdateCaretSize()
		{
			if (editActiveView != null)
			{
				editActiveView.UpdateCaretSize();
			}
		}

		/// <summary>
		/// Saves the absolut X position of the caret.
		/// </summary>
		internal void SaveCurrentX()
		{
			if (editActiveView != null)
			{
				editActiveView.SaveCurrentX();
			}
		}

		/// <summary>
		/// Loads the contents of the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file to load.</param>
		public void LoadFile(string fileName)
		{
			if (fileName != null)
			{
				ClearContents();
				CurrentFile = fileName;
				if (editData.LoadFile(fileName))
				{
					Modified = false;
					ResetView();
					Focus();
				}
				else
				{
					Close();
				}
			}
			else
			{
				ShowInfoMessage(GetResourceString("NoSourceFile"), 
					GetResourceString("StrLoadFile"));
			}
		}

		/// <overload>
		/// Adds a wave line with the specified color group name.
		/// </overload>
		/// <summary>
		/// Adds a wave line for the whole range of the specified line.
		/// </summary>
		/// <param name="ln">The line at which to draw the wave line.</param>
		/// <param name="colorGroup">The color group name of the wave 
		/// line.</param>
		public void AddWaveLine(int ln, string colorGroup)
		{
			AddWaveLine(ln, 1, GetLineLengthPlusOne(ln), colorGroup);
		}

		/// <summary>
		/// Adds a wave line for the specified location range of a line.
		/// </summary>
		/// <param name="ln">The line at which to draw the wave line.</param>
		/// <param name="chStart">The starting char of the wave line.</param>
		/// <param name="chEnd">The ending char (exclusive) of the wave
		/// line.</param>
		/// <param name="colorGroup">The color group name of the wave line.
		/// </param>
		public void AddWaveLine(int ln, int chStart, int chEnd, string colorGroup)
		{
			if (IsValidLine(ln))
			{
				editData.AddWaveLine(ln, chStart, chEnd, colorGroup);
				RedrawLine(ln);
			}
		}

		/// <summary>
		/// Adds wave lines for the specified location range.
		/// </summary>
		/// <param name="lnStart">The line of the starting location of the 
		/// location range for which wave lines are to be added.</param>
		/// <param name="chStart">The char of the starting location of the 
		/// location range for which wave lines are to be added.</param>
		/// <param name="lnEnd">The line of the ending location of the 
		/// location range for which wave lines are to be added.</param>
		/// <param name="chEnd">The char of the ending location of the 
		/// location range for which wave lines are to be added.</param>
		/// <param name="colorGroup">The color group name of wave lines.</param>
		public void AddWaveLine(int lnStart, int chStart, int lnEnd, int chEnd, 
			string colorGroup)
		{
			AddWaveLine(new EditLocationRange(lnStart, chStart, lnEnd, chEnd), 
				colorGroup);
		}

		/// <summary>
		/// Adds wave lines for the specified location range.
		/// </summary>
		/// <param name="lcStart">The starting location of the location range 
		/// for which wave lines are to be added.</param>
		/// <param name="lcEnd">The ending location of the location range 
		/// for which wave lines are to be added.</param>
		/// <param name="colorGroup">The color group name of wave lines.</param>
		public void AddWaveLine(EditLocation lcStart, EditLocation lcEnd, 
			string colorGroup)
		{
			AddWaveLine(new EditLocationRange(lcStart, lcEnd), colorGroup);
		}

		/// <summary>
		/// Adds wave lines for the specified location range.
		/// </summary>
		/// <param name="lcr">The location range for which wave lines are to 
		/// be added.</param>
		/// <param name="colorGroup">The color group name of wave lines.</param>
		public void AddWaveLine(EditLocationRange lcr, string colorGroup)
		{
			EditLocationRange lcrNorm = lcr.Normalize();
			editData.AddWaveLine(lcrNorm, colorGroup);
			RedrawLines(lcrNorm.Start.L, lcrNorm.End.L);
		}

		/// <overload>
		/// Removes all the wave lines at the specified location.
		/// </overload>
		/// <summary>
		/// Removes all the wave lines at the specified line.
		/// </summary>
		/// <param name="ln">The line at which wave lines are to be removed.
		/// </param>
		public void RemoveWaveLines(int ln)
		{
			RemoveWaveLines(ln, true);
		}

		/// <summary>
		/// Removes all the wave lines for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range for 
		/// which wave lines are to be added.</param>
		/// <param name="lnEnd">The ending line of the line range for 
		/// which wave lines are to be added.</param>
		public void RemoveWaveLines(int lnStart, int lnEnd)
		{
			for (int i = lnStart; i < lnEnd; i++)
			{
				RemoveWaveLines(i, false);
			}
			RedrawLines(lnStart, lnEnd);
		}

		/// <summary>
		/// Removes all the wave lines at the specified line.
		/// </summary>
		/// <param name="ln">The line at which the wave lines are to be 
		/// removed.</param>
		/// <param name="bRedraw">A value indicating whether to redraw 
		/// the line after removing wave lines.</param>
		internal void RemoveWaveLines(int ln, bool bRedraw)
		{
			if (IsValidLine(ln))
			{
				editData.RemoveWaveLines(ln);
				if (bRedraw)
				{
					RedrawLine(ln);
				}
			}
		}

		/// <summary>
		/// Removes wave lines for the specified range of a line.
		/// </summary>
		/// <param name="ln">The line at which the wave lines are to be 
		/// removed.</param>
		/// <param name="chStart">The starting char for wave line removal.</param>
		/// <param name="chEnd">The ending char (exclusive) for wave line 
		/// removal.</param>
		public void RemoveWaveLines(int ln, int chStart, int chEnd)
		{
			if (IsValidLine(ln))
			{
				editData.RemoveWaveLines(ln, chStart, chEnd);
				RedrawLine(ln);
			}
		}

		/// <summary>
		/// Removes wave lines for the specified location range.
		/// </summary>
		/// <param name="lnStart">The line of the starting location.</param>
		/// <param name="chStart">The char of the starting location.</param>
		/// <param name="lnEnd">The line of the ending location.</param>
		/// <param name="chEnd">The char of the ending location.</param>
		public void RemoveWaveLines(int lnStart, int chStart, int lnEnd, int chEnd) 
		{
			RemoveWaveLines(new EditLocationRange(lnStart, chStart, lnEnd, chEnd)); 
		}

		/// <summary>
		/// Removes wave lines for the specified location range.
		/// </summary>
		/// <param name="lcStart">The starting location.</param>
		/// <param name="lcEnd">The ending location.</param>
		public void RemoveWaveLines(EditLocation lcStart, EditLocation lcEnd) 
		{
			RemoveWaveLines(new EditLocationRange(lcStart, lcEnd));
		}

		/// <summary>
		/// Removes wave lines for the specified location range.
		/// </summary>
		/// <param name="lcr">The location range of wave lines.</param>
		public void RemoveWaveLines(EditLocationRange lcr)
		{
			EditLocationRange lcrNorm = lcr.Normalize();
			editData.RemoveWaveLines(lcrNorm);
			RedrawLines(lcrNorm.Start.L, lcrNorm.End.L);
		}

		/// <summary>
		/// Sets up the colors and color indexes that are frequently used.
		/// </summary>
		internal void SetupCommonColors()
		{
			SelectedTextColorGroupIndex = 
				editSettings.GetColorGroupIndex("Selected Text");
			InactiveSelectedTextColorGroupIndex = 
				editSettings.GetColorGroupIndex("Inactive Selected Text");
			TextForeColor = editSettings.GetColorGroupForeColor("Text");
			TextBackColor = editSettings.GetColorGroupBackColor("Text");
			IndicatorMarginForeColor = 
				editSettings.GetColorGroupForeColor("Indicator Margin");
			IndicatorMarginBackColor = 
				editSettings.GetColorGroupBackColor("Indicator Margin");
			LineNumberForeColor = 
				editSettings.GetColorGroupForeColor("Line Numbers");
			LineNumberBackColor = 
				editSettings.GetColorGroupBackColor("Line Numbers");
			SelectionMarginForeColor = 
				editSettings.GetColorGroupForeColor("Collapsible Text");
			SelectionMarginBackColor = TextBackColor;
			CollapsedTextBackColor = 
				editSettings.GetColorGroupBackColor("Collapsible Text");
			VisibleWhiteSpaceForeColor = 
				editSettings.GetColorGroupForeColor("Visible White Space");
			VisibleWhiteSpaceBackColor = 
				editSettings.GetColorGroupBackColor("Visible White Space");
			BraceMatchingForeColor =
				editSettings.GetColorGroupForeColor("Brace Matching");
			ForeColor = TextForeColor;
			BackColor = TextBackColor;
		}

		/// <summary>
		/// Gets the string object for the specified line.
		/// </summary>
		/// <param name="ln">The line at which the string is located.</param>
		/// <returns>The string object of the line.</returns>
		public string GetStringObject(int ln)
		{
			return editData.GetStringObject(ln);
		}

		/// <overload>
		/// Gets the string at the specified location.
		/// </overload>
		/// <summary>
		/// Gets the string at the specified line (without the trailing newline
		/// characters).
		/// </summary>
		/// <param name="ln">The line at which the string is located.</param>
		/// <returns>The string at the line.</returns>
		public string GetString(int ln)
		{
			return editData.GetString(ln);
		}

		/// <summary>
		/// Gets the line string with the trailing newline characters.
		/// </summary>
		/// <param name="ln">The line at which the string is located.</param>
		/// <returns>The whole string at the line.</returns>
		public string GetWholeString(int ln)
		{
			return editData.GetWholeString(ln);
		}

		/// <summary>
		/// Gets the length of the string at the specified line.
		/// </summary>
		/// <param name="ln">The line for which the string length is to be 
		/// obtained.</param>
		/// <returns>The string length of the line.</returns>
		public int GetLineLength(int ln)
		{
			return editData.GetLineLength(ln);
		}

		/// <summary>
		/// Gets the length of the whole string at the specified line.
		/// </summary>
		/// <param name="ln">The line for which the whole string length is 
		/// to be obtained.</param>
		/// <returns>The whole string length of the line.</returns>
		public int GetWholeLineLength(int ln)
		{
			return editData.GetWholeLineLength(ln);
		}

		/// <summary>
		/// Gets the ending char index for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the ending char index 
		/// is to be obtained.</param>
		/// <returns>The ending char index.</returns>
		public int GetLineLengthPlusOne(int ln)
		{
			return editData.GetLineLengthPlusOne(ln);
		}

		/// <summary>
		/// Gets the text in the specified location range.
		/// </summary>
		/// <param name="lcr">The location range for which the text is to 
		/// be obtained.</param>
		/// <returns>The text in the location range.</returns>
		public string GetString(EditLocationRange lcr)
		{
			return editData.GetString(lcr.Start.L, lcr.Start.C, lcr.End.L, lcr.End.C);
		}

		/// <summary>
		/// Gets the text in the specified location range.
		/// </summary>
		/// <param name="lcStart">The starting location of the location range 
		/// for which the text is to be obtained.</param>
		/// <param name="lcEnd">The ending location of the location range 
		/// for which the text is to be obtained.</param>
		/// <returns>The text in the location range.</returns>
		public string GetString(EditLocation lcStart, EditLocation lcEnd)
		{
			return editData.GetString(lcStart.L, lcStart.C, lcEnd.L, lcEnd.C);
		}

		/// <summary>
		/// Gets the text in the specified location range.
		/// </summary>
		/// <param name="lnStart">The line of the starting location of the 
		/// location range for which the text is to be obtained.</param>
		/// <param name="chStart">The char of the starting location of the 
		/// location range for which the text is to be obtained.</param>
		/// <param name="lnEnd">The line of the ending location of the 
		/// location range for which the text is to be obtained.</param>
		/// <param name="chEnd">The char of the ending location of the 
		/// location range for which the text is to be obtained.</param>
		/// <returns>The text in the location range.</returns>
		public string GetString(int lnStart, int chStart, int lnEnd, int chEnd)
		{
			return editData.GetString(lnStart, chStart, lnEnd, chEnd);
		}

		/// <summary>
		/// Gets all the text.
		/// </summary>
		/// <returns>All the text in the edit.</returns>
		public string GetAllText()
		{
			if (!HasContent)
			{
				return string.Empty;
			}
			return GetString(FirstLineChar, LastLineChar);
		}

		#endregion

		#region Methods Related to Location Manipulating.
		/// <summary>
		/// Gets the point from the specified location.
		/// </summary>
		/// <param name="lc"></param>
		/// <param name="X"></param>
		/// <param name="Y"></param>
		public Point GetPoint(EditLocation lc)
		{
			int X;
			int Y;
			ActiveView.GetPoint(lc,out X,out Y);
			return new Point(X, Y);
		}

		/// <summary>
		/// Updates the maximum column number based on the column number of the
		/// specified line.
		/// </summary>
		/// <param name="ln"></param>
		internal void UpdateMaxColumn(int ln)
		{
			int iTemp = GetColumnCount(ln);
			if (iTemp > editMaxColumn)
			{
				editMaxColumn = iTemp;
				UpdateHScrollBar();
			}
		}

		/// <summary>
		/// Tests if the specified line is valid.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <returns>true if line is valid; otherwise false.</returns>
		public bool IsValidLine(int ln)
		{
			return editData.IsValidLine(ln);
		}

		/// <overload>
		/// Tests if the specified location is valid.
		/// </overload>
		/// <summary>
		/// Tests if the specified location is valid.
		/// </summary>
		/// <param name="ln">The line of the location to be tested.</param>
		/// <param name="ch">The char of the location to be tested.</param>
		/// <returns>true if the location is valid; otherwise, false.</returns>
		public bool IsValidLineChar(int ln, int ch)
		{
			return editData.IsValidLineChar(ln, ch);
		}

		/// <summary>
		/// Tests if the specified location is valid.
		/// </summary>
		/// <param name="lc">The location to be tested.</param>
		/// <returns>true if the location is valid; otherwise, false.</returns>
		public bool IsValidLineChar(EditLocation lc)
		{
			return editData.IsValidLineChar(lc.L, lc.C);
		}

		/// <summary>
		/// Tests if the specified line/column location is valid.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="col">The column of the location.</param>
		/// <returns>true if the line/column location is valid; otherwise, 
		/// false.</returns>
		public bool IsValidLineColumn(int ln, int col)
		{
			return editViewBottom.IsValidLineColumn(ln, col);
		}

		/// <summary>
		/// Gets the total number of characters for the specified line, 
		/// including ending newline characters.
		/// </summary>
		/// <param name="ln">The line for which the character count is to 
		/// be obtained.</param>
		/// <returns>The total number of characters in the line.</returns>
		public int GetCharCount(int ln)
		{
			return GetWholeString(ln).Length;
		}

		/// <summary>
		/// Gets the location before the sepcified location.
		/// </summary>
		/// <param name="lc">The location for which the previous location 
		/// is to be obtained.</param>
		/// <returns>The location before the given location. </returns>
		public EditLocation GetPreviousLineChar(EditLocation lc)
		{
			return editData.GetPreviousLineChar(lc.L, lc.C);
		}

		/// <summary>
		/// Gets the location after the sepcified location.
		/// </summary>
		/// <param name="lc">The location for which the next location 
		/// is to be obtained.</param>
		/// <returns>The next location after the location.</returns>
		public EditLocation GetNextLineChar(EditLocation lc)
		{
			return editData.GetNextLineChar(lc.L, lc.C);
		}

		/// <summary>
		/// Gets the total number of columns for the specified string.
		/// </summary>
		/// <param name="str">The string for which the column count is to 
		/// be obtained.</param>
		/// <returns>The column count for the string.</returns>
		internal int GetColumnCount(string str)
		{
			return editViewBottom.GetColumnCount(str);
		}

		/// <summary>
		/// Gets the total number of columns for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the total number of columns 
		/// is to be obtained.</param>
		/// <returns>The column count of the line.</returns>
		public int GetColumnCount(int ln)
		{
			return editViewBottom.GetColumnCount(ln);
		}

		/// <summary>
		/// Gets the column for the specified line/char location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		/// <returns>The column of the location.</returns>
		public int GetColumnFromChar(int ln, int ch)
		{
			return editViewBottom.GetColumnFromChar(ln, ch);
		}

		/// <summary>
		/// Gets the char from the specified line/column location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="col">The column of the location.</param>
		/// <returns>The char of the location.</returns>
		public int GetCharFromColumn(int ln, int col)
		{
			return editViewBottom.GetCharFromColumn(ln, col);
		}

		/// <summary>
		/// Gets a line/column location from the specified line/char location.
		/// </summary>
		/// <param name="lc">The line/char location from which the line/column 
		/// location is to be obtained.</param>
		/// <returns>The line/column location for the line/char location.</returns>
		public EditLocation LineColumnFromLineChar(EditLocation lc)
		{
			return editViewBottom.LineColumnFromLineChar(lc);
		}

		/// <summary>
		/// Gets a line/char location from the specified line/column location.
		/// </summary>
		/// <param name="lc">The line/column location from which the line/char 
		/// location is to be obtained.</param>
		/// <returns>The line/char location for the line/column location.</returns>
		public EditLocation LineCharFromLineColumn(EditLocation lc)
		{
			return editViewBottom.LineCharFromLineColumn(lc);
		}

		/// <summary>
		/// Updates a location due to an insertion.
		/// </summary>
		/// <param name="lc">The location to be updated.</param>
		/// <param name="lcr">The location range of the insertion.</param>
		/// <returns>The location after the insertion.</returns>
		public EditLocation UpdateFromInsertion(EditLocation lc, EditLocationRange lcr)
		{
			EditLocation lcTemp = new EditLocation(lc);
			editData.UpdateFromInsertion(ref lcTemp, lcr, true);
			return lcTemp;
		}

		/// <summary>
		/// Updates a location due to a deletion.
		/// </summary>
		/// <param name="lc">The location to be updated.</param>
		/// <param name="lcr">The location range of the deletion.</param>
		/// <returns>The location after the deletion.</returns>
		public EditLocation UpdateFromDeletion(EditLocation lc, EditLocationRange lcr)
		{
			EditLocation lcTemp = new EditLocation(lc);
			editData.UpdateFromDeletion(ref lcTemp, lcr);
			return lcTemp;
		}

		#endregion

		#region Methods Related to Inserting

		/// <summary>
		/// Inserts an empty line at the start of the document.
		/// </summary>
		/// <returns>true if an empty first line has been inserted; otherwise, 
		/// false.</returns>
		internal bool InsertEmptyFirstLine()
		{
			return editData.InsertLine(1, string.Empty);
		}

		/// <overload>
		/// Inserts a string or string arraylist to the contents.
		/// </overload>
		/// <summary>
		/// Inserts the specified string at the current caret location.
		/// </summary>
		/// <param name="str">The string to be inserted.</param>
		/// <returns>The location range of the inserted string.</returns>
		public EditLocationRange Insert(string str)
		{
			return Insert(CurrentLineChar, str);
		}

		/// <summary>
		/// Inserts the specified string at the specified location.
		/// </summary>
		/// <param name="lc">The location for insertion.</param>
		/// <param name="str">The string to be inserted.</param>
		/// <returns>The location range of the inserted string.</returns>
		public EditLocationRange Insert(EditLocation lc, string str)
		{
			return Insert(lc, str, true);
		}

		/// <summary>
		/// Inserts the specified string at the specified location.
		/// </summary>
		/// <param name="lc">The location for insertion.</param>
		/// <param name="str">The string to be inserted.</param>
		/// <param name="bViewCaret">A value indicating whether to scroll to 
		/// view caret.</param>
		/// <returns>The location range of the inserted string.</returns>
		public EditLocationRange Insert(EditLocation lc, string str, 
			bool bViewCaret)
		{
			if(HasContent)
			{
				if ((str == null) || (str.Length == 0))
				{
					return new EditLocationRange(lc, lc);
				}
				string strTemp = str;
				if (Multiline)
				{
					return Insert(lc, GetStringArrayList(strTemp), bViewCaret);
				}
				else
				{
					strTemp = strTemp.Replace("\r", "");
					strTemp = strTemp.Replace("\n", "");
					ArrayList strList = new ArrayList();
					strList.Add(strTemp);
					return Insert(lc, strList, bViewCaret);
				}
			}
			else
				return null;
		}

		/// <summary>
		/// Inserts an array list of strings at the specified location.
		/// </summary>
		/// <param name="lc">The location for insertion.</param>
		/// <param name="strList">The string arraylist to be inserted.</param>
		/// <returns>The location range of the inserted string arraylist.
		/// </returns>
		public EditLocationRange Insert(EditLocation lc, ArrayList strList)
		{
			return Insert(lc, strList, true);
		}

		/// <summary>
		/// Inserts an array list of strings at the specified location.
		/// </summary>
		/// <param name="lc">The location for insertion.</param>
		/// <param name="strList">The string arraylist to be inserted.</param>
		/// <param name="bViewCaret">A value indicating whether to scroll to 
		/// view caret.</param>
		/// <returns>The location range of the inserted string arraylist.</returns>
		public EditLocationRange Insert(EditLocation lc, ArrayList strList, 
			bool bViewCaret)
		{
			if(HasContent)
			{
				BeginUpdate();
				int lnLengthPlusOne = GetLineLengthPlusOne(lc.L);
				EditLocation lcTemp = new EditLocation(lc);
				if (lc.C > lnLengthPlusOne)
				{
					lcTemp.C = lnLengthPlusOne;
					MoveCaret(lc, lcTemp);
					strList[0] = GetSpaceString(GetColumnFromChar(lc.L, lnLengthPlusOne), 
						GetColumnFromChar(lc.L, lc.C)) + ((string)strList[0]);
				}
				EditLocationRange lcr = editData.Insert(lcTemp, strList);
				if (OutliningEnabled)
				{
					editData.UpdateOutliningFromInsertion(lcr);
				}
				if (!bInAction)
				{
					if (editCompositeAction != null)
					{
						editCompositeAction.AddAction(new EditActionInsert(this, 
							lcr.Start, lcr.End));
					}
					else
					{
						LogAction(new EditActionInsert(this, lcr.Start, lcr.End));
					}
				}
				if (ContentChangedActive)
				{
					ContentChangedEventArgs ccArgs = new ContentChangedEventArgs(
						GetString(lcr), lcr, true);
					OnContentChanged(ccArgs);
				}
				editData.InvalidateRangeInfo(lcr.Start, lcr.End);
				UpdateScrollBars();
				CurrentLineChar = lcr.End;
				EndUpdate();
				RedrawFromLine(lcr.Start.L);
				if (bViewCaret)
				{
					ScrollToViewCaret();
				}
				Modified = true;
				return lcr;
			}
			else
				return null;
		}

		/// <summary>
		/// Moves the caret to a new location.
		/// </summary>
		/// <param name="lcOld">The old location of the caret.</param>
		/// <param name="lcNew">The new location of the caret.</param>
		public void MoveCaret(EditLocation lcOld, EditLocation lcNew)
		{
			if (!bInAction)
			{
				if (editCompositeAction != null)
				{
					editCompositeAction.AddAction(new EditActionCaretMove(this, 
						lcOld, lcNew));
				}
				else
				{
					LogAction(new EditActionCaretMove(this, lcOld, lcNew));
				}
			}
		}

		/// <summary>
		/// Inserts the contents of the specified file at the current 
		/// caret location.
		/// </summary>
		/// <param name="fileName">The name of the file to be inserted.</param>
		public void InsertFile(string fileName)
		{
			if ((fileName != null) && (fileName != string.Empty))
			{
				editData.InsertFile(fileName);
			}
			else
			{
				ShowInfoMessage(GetResourceString("NoSourceFile"), 
					GetResourceString("InsertFile"));
			}
		}

		/// <summary>
		/// Parses the specified string into a string arraylist based on line 
		/// breaks.
		/// </summary>
		/// <param name="str">The string to be parsed.</param>
		/// <returns>The arraylist of strings parsed from the string.</returns>
		public static ArrayList GetStringArrayList(string str)
		{
			ArrayList strList = new ArrayList();
			if ((str == null) || (str.Length == 0))
			{
				strList.Add(string.Empty);
				return strList;
			}
			StringBuilder strBdr = new StringBuilder(string.Empty);
			for (int i = 0; i < str.Length; i++)
			{
				if (str[i] == '\r')
				{
					if(i < str.Length - 1)
					{
						if (str[i+1] == '\n') 
						{
							i++;
						}
					}
					strList.Add(strBdr.ToString());
					strBdr.Remove(0, strBdr.Length);
				}
				else if (str[i] == '\n')
				{
					strList.Add(strBdr.ToString());
					strBdr.Remove(0, strBdr.Length);
				}
				else 
				{
					strBdr.Append(str[i]);
				}
			}
			strList.Add(strBdr.ToString());
			return strList;
		}

		#endregion

		#region Methods Related to Deleting

		/// <summary>
		/// Deletes the specified line.
		/// </summary>
		/// <param name="ln">The line to be deleted.</param>
		/// <returns>true if the line is deleted; otherwise, false.</returns>
		public bool DeleteLine(int ln)
		{
			if (!IsValidLine(ln))
			{
				return false;
			}
			if (ln == LineCount)
			{
				if (GetLineLength(LineCount) == 0)
				{
					return true;
				}
				else
				{
					Delete(new EditLocationRange(ln, 1, ln, 
						GetLineLengthPlusOne(LineCount)));
					return true;
				}
			}
			Delete(new EditLocationRange(ln, 1, ln + 1, 1));
			return true;
		}

		/// <overload>
		/// Deletes the text in the specified range.
		/// </overload>
		/// <summary>
		/// Deletes the text in the specified range.
		/// </summary>
		/// <param name="lcStart">The starting location for deletion.</param>
		/// <param name="lcEnd">The ending location for deletion.</param>
		/// <param name="bViewCaret">A value indicating whether to scroll to 
		/// view caret.</param>
		/// <returns>true if some characters are deleted; otherwise, false.
		/// </returns>
		public bool Delete(EditLocation lcStart, EditLocation lcEnd, bool bViewCaret)
		{
			if (lcStart == lcEnd)
			{
				if (bViewCaret)
				{
					ScrollToViewCaret();
				}
				return true;
			}
			BeginLogCompositeAction(EditActionType.Delete);
			EditLocation lcStartTemp = (lcStart < lcEnd) ? lcStart : lcEnd;
			EditLocation lcEndTemp = (lcStart > lcEnd) ? lcStart : lcEnd;
			EditLocation lcTemp = new EditLocation(lcStartTemp);
			int lnLengthPlusOne = GetLineLengthPlusOne(lcStartTemp.L);
			if (lcStartTemp.C > lnLengthPlusOne)
			{
				lcStartTemp.C = lnLengthPlusOne;
				MoveCaret(lcTemp, lcStartTemp);
				Insert(lcStartTemp, new string(' ', (lcTemp.C - lcStartTemp.C)));
			}
			bool bResult = DeleteOnly(lcStartTemp, lcEndTemp, bViewCaret);
			EndLogCompositeAction();
			return bResult;
		}

		private bool DeleteOnly(EditLocation lcStart, EditLocation lcEnd, bool bViewCaret)
		{
			if (!InAction)
			{
				if (editCompositeAction != null)
				{
					editCompositeAction.AddAction(new EditActionDelete(this, 
						lcStart, lcEnd));
				}
				else
				{
					LogAction(new EditActionDelete(this, lcStart, lcEnd));
				}
			}
			string strTemp = string.Empty;
			if (ContentChangedActive)
			{
				strTemp = GetString(lcStart, lcEnd);
			}
			if (!editData.Delete(lcStart, lcEnd))
			{
				return false;
			}
			if (ContentChangedActive)
			{
				ContentChangedEventArgs ccArgs = new ContentChangedEventArgs(
					strTemp, new EditLocationRange(lcStart, lcEnd), false);
				OnContentChanged(ccArgs);
			}
			bool bRedrawAll = false;
			if (OutliningEnabled)
			{
				bRedrawAll = editData.UpdateOutliningFromDeletion(new 
					EditLocationRange(lcStart, lcEnd));
			}
			CurrentLineChar = lcStart;
			CheckBraceMatching();
			if (HasMultiLineTag)
			{
				editData.InvalidateRangeInfo(lcStart, lcEnd);
			}
			else
			{
				editData.InvalidateRangeInfo(lcStart, lcStart);
			}
			UpdateVScrollBar();
			if (bRedrawAll)
			{
				Redraw();
			}
			else
			{
				RedrawFromLine(lcStart.L);
			}
			if (bViewCaret)
			{
				ScrollToViewCaret();
			}
			Modified = true;
			return true;
		}

		/// <summary>
		/// Deletes the text in the specified range.
		/// </summary>
		/// <param name="lcStart">The starting location for deletion.</param>
		/// <param name="lcEnd">The ending location for deletion.</param>
		/// <returns>true if some characters are deleted; otherwise, false.
		/// </returns>
		public bool Delete(EditLocation lcStart, EditLocation lcEnd)
		{
			return Delete(lcStart, lcEnd, true);
		}

		/// <summary>
		/// Deletes the text in the specified range.
		/// </summary>
		/// <param name="lcr">The location range to be deleted.</param>
		/// <returns>true if some characters are deleted; otherwise, false.
		/// </returns>
		public bool Delete(EditLocationRange lcr)
		{
			return Delete(lcr.Start, lcr.End, true);
		}

		/// <summary>
		/// Deletes the text in the specified range.
		/// </summary>
		/// <param name="lnStart">The line of the starting location for 
		/// deletion.</param>
		/// <param name="chStart">The char of the starting location for 
		/// deletion.</param>
		/// <param name="lnEnd">The line of the ending location for 
		/// deletion.</param>
		/// <param name="chEnd">The char of the ending location for 
		/// deletion.</param>
		/// <returns>true if some characters are deleted; otherwise, false.
		/// </returns>
		public bool Delete(int lnStart, int chStart, int lnEnd, int chEnd)
		{
			return Delete(new EditLocation(lnStart, chStart), 
				new EditLocation(lnEnd, chEnd), true);
		}

		/// <summary>
		/// Deletes the selection.
		/// </summary>
		/// <returns>true if the selected text is deleted; otherwise, false.
		/// </returns>
		public bool DeleteSelection()
		{
			if (HasSelection)
			{
				EditLocationRange lcrNorm = editSelection.Normalize();
				UnSelect();
				Delete(lcrNorm.Start, lcrNorm.End);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Deletes one character after the caret.
		/// </summary>
		public void DeleteCharAfterCaret()
		{
			EditLocation lcNew;
			if (CurrentChar > GetLineLengthPlusOne(CurrentLine) 
				&& (CurrentLine < LineCount))
			{
				lcNew = new EditLocation(CurrentLine + 1, 1);
			}
			else
			{
				lcNew = GetNextLineChar(CurrentLineChar);
			}
			Delete(CurrentLineChar, lcNew);
		}

		/// <summary>
		/// Deletes one character before the caret.
		/// </summary>
		public void DeleteCharBeforeCaret()
		{
			int lnLengthPlusOne = GetLineLengthPlusOne(CurrentLine);
			if (CurrentChar <= lnLengthPlusOne)
			{
				EditLocation lcOld = new EditLocation(CurrentLineChar);
				EditLocation lcNew = editData.GetPreviousLineChar(lcOld.L, lcOld.C);
				Delete(lcNew, lcOld);
			}
			else
			{
				if ((CurrentChar - IndentSize) > lnLengthPlusOne)
				{
					CurrentChar -= IndentSize;
				}
				else
				{
					CurrentChar = lnLengthPlusOne;
				}
				UpdateCaretPos();
			}
		}

		#endregion

		#region Methods Related to Selecting

		/// <summary>
		/// Selects all the text in the edit.
		/// </summary>
		public void SelectAll()
		{
			Select(FirstLineChar, LastLineChar);
			ScrollToViewCaret();
		}

		/// <summary>
		/// Sets the selection to the specified range and type.
		/// </summary>
		/// <param name="lnStart">The line of the starting location.</param>
		/// <param name="chStart">The char of the starting location.</param>
		/// <param name="lnEnd">The line of the ending location.</param>
		/// <param name="chEnd">The char of the ending location.</param>
		/// <param name="isLinewise">A value indicating whether the selection 
		/// is linewise.</param>
		public void Select(int lnStart, int chStart, int lnEnd, int chEnd, bool isLinewise)
		{
			if (!HasContent)
			{
				return;
			}
			CurrentLine = lnEnd;
			CurrentChar = chEnd;
			editSelection.Select(lnStart, chStart, lnEnd, chEnd, isLinewise);
			Redraw();
		}

		/// <summary>
		/// Sets the selection to the specified range with the linewise 
		/// selection type.
		/// </summary>
		/// <param name="lnStart">The line of the starting location for 
		/// selection.</param>
		/// <param name="chStart">The char of the starting location for 
		/// selection.</param>
		/// <param name="lnEnd">The line of the ending location for 
		/// selection.</param>
		/// <param name="chEnd">The char of the ending location for 
		/// selection.</param>
		public void Select(int lnStart, int chStart, int lnEnd, int chEnd)
		{
			Select(lnStart, chStart, lnEnd, chEnd, true);
		}

		/// <summary>
		/// Sets the selection to the specified range and type.
		/// </summary>
		/// <param name="lcStart">The starting location for selection.</param>
		/// <param name="lcEnd">The ending location for selection.</param>
		/// <param name="isLinewise">A value indicating whether the selection 
		/// is linewise.</param>
		public void Select(EditLocation lcStart, EditLocation lcEnd, bool isLinewise)
		{
			Select(lcStart.L, lcStart.C, lcEnd.L, lcEnd.C, isLinewise);
		}

		/// <summary>
		/// Sets the selection to the specified range with the linewise 
		/// selection type.
		/// </summary>
		/// <param name="lcStart">The starting location for selection.</param>
		/// <param name="lcEnd">The ending location for selection.</param>
		public void Select(EditLocation lcStart, EditLocation lcEnd)
		{
			Select(lcStart.L, lcStart.C, lcEnd.L, lcEnd.C, true);
		}

		/// <summary>
		/// Sets the selection to the specified range and type.
		/// </summary>
		/// <param name="lcr">The location range for the selection.</param>
		/// <param name="isLinewise">A value indicating whether the selection 
		/// is linewise.</param>
		public void Select(EditLocationRange lcr, bool isLinewise)
		{
			Select(lcr.Start.L, lcr.Start.C, lcr.End.L, lcr.End.C, isLinewise);
		}

		/// <summary>
		/// Sets the selection to the specified range with the linewise 
		/// selection type.
		/// </summary>
		/// <param name="lcr">The location range for the selection.</param>
		public void Select(EditLocationRange lcr)
		{
			Select(lcr.Start.L, lcr.Start.C, lcr.End.L, lcr.End.C, true);
		}

		/// <summary>
		/// Extends the selection to the specified location.
		/// </summary>
		/// <param name="lc">The location to which the seletion is 
		/// extended.</param>
		public void ExtendSelection(EditLocation lc)
		{
			int lnOld = editSelection.End.L;
			editSelection.ExtendTo(lc);
			RedrawLines(lnOld, lc.L);
		}

		/// <summary>
		/// Unselects the current selection.
		/// </summary>
		public void UnSelect()
		{
			EditLocationRange lcr = editSelection.Normalize();
			editSelection.UnSelect();
			if (!InsertMode)
			{
				UpdateCaretSize();
			}
			RedrawLines(lcr.Start.L, lcr.End.L);
		}

		/// <overload>
		/// Starts the selecting process at the specified location.
		/// </overload>
		/// <summary>
		/// Starts the selecting process at the specified location.
		/// </summary>
		/// <param name="lnStart">The line of the starting location for 
		/// selecting.</param>
		/// <param name="chStart">The char of the starting location for 
		/// selecting.</param>
		/// <param name="isLineWise">A value indicating the selection 
		/// type.</param>
		public void StartSelecting(int lnStart, int chStart, bool isLineWise)
		{
			editSelection.StartSelecting(lnStart, chStart, isLineWise);
		}

		/// <summary>
		/// Starts the selecting process at the specified location.
		/// </summary>
		/// <param name="lc">The starting location for selecting.</param>
		/// <param name="isLineWise">A value indicating the selection 
		/// type.</param>
		public void StartSelecting(EditLocation lc, bool isLineWise)
		{
			editSelection.StartSelecting(lc, isLineWise);
		}

		/// <summary>
		/// Selects the specified line.
		/// </summary>
		/// <param name="ln">The line to be selected.</param>
		/// <returns>true if the line is selected; otherwise, false</returns>
		public bool SelectLine(int ln)
		{
			if (!IsValidLine(ln))
			{
				return false;
			}
			if (ln < LineCount)
			{
				Select(ln, 1, ln + 1, 1);
			}
			else
			{
				Select(ln, 1, ln, GetLineLengthPlusOne(ln));
			}
			return true;
		}

		#endregion

		#region Methods Related to Editing

		/// <overload>
		/// Moves the selected text to the specified location.
		/// </overload>
		/// <summary>
		/// Moves the selected text to the specified location.
		/// </summary>
		/// <param name="ln">The line of the destination location.</param>
		/// <param name="ch">The char of the destination location.</param>
		public void MoveSelection(int ln, int ch)
		{
			BeginLogCompositeAction(EditActionType.DragDrop);
			BeginUpdate();
			string strTemp = SelectedText;
			EditLocationRange lcrNorm = editSelection.Normalize();
			UnSelect();
			if (lcrNorm.End.LessThanOrEqualTo(ln, ch))
			{
				int lnNew = ln;
				int chNew = ch;
				if (ln == lcrNorm.End.L)
				{
					lnNew -= (lcrNorm.End.L - lcrNorm.Start.L);
					chNew -= lcrNorm.End.C - lcrNorm.Start.C;
				}
				else // ((ln > lcrNorm.End.L)
				{
					if (lcrNorm.Start.L != lcrNorm.End.L)
					{
						lnNew -= (lcrNorm.End.L - lcrNorm.Start.L);
					}
				}
				Delete(lcrNorm.Start, lcrNorm.End, false);
				CurrentLine = lnNew;
				CurrentChar = chNew;
			}
			else
			{
				Delete(lcrNorm.Start, lcrNorm.End, false);
				CurrentLine = ln;
				CurrentChar = ch;
			}
			EditLocationRange lcrNew = Insert(strTemp);
			Select(lcrNew);
			EndUpdate();
			EndLogCompositeAction();
			Redraw();
		}

		/// <summary>
		/// Moves the selected text to the specified location.
		/// </summary>
		/// <param name="lc">The destination location.</param>
		public void MoveSelection(EditLocation lc)
		{
			MoveSelection(lc.L, lc.C);
		}

		/// <summary>
		/// Replaces the selected text with the specified string.
		/// </summary>
		/// <param name="str">The text replaced to.</param>
		/// <returns>true if the selection has been replaced; false if no 
		/// selection is presented.</returns>
		public bool ReplaceSelection(string str)
		{
			if (!HasSelection)
			{
				return false;
			}
			EditLocationRange lcr = editSelection.Normalize();
			UnSelect();
			Replace(lcr, str);
			return true;
		}

		/// <summary>
		/// Replaces the text in the specified range with the specified 
		/// string.
		/// </summary>
		/// <param name="lcr">The location range in which the text is to 
		/// be replaced.</param>
		/// <param name="str">The text replaced to.</param>
		public void Replace(EditLocationRange lcr, string str)
		{
			BeginLogCompositeAction(EditActionType.Replace);
			Delete(lcr);
			Insert(str);
			EndLogCompositeAction();
		}

		/// <summary>
		/// Invokes the Find and Replace dialog.
		/// </summary>
		/// <param name="bReplace">A value indicating whether the dialog starts 
		/// up as a Replace dialog.</param>
		public void FindAndReplace(bool bReplace)
		{
			if (!HasContent)
			{
				return;
			}
			if (editActiveView != null)
			{
				editActiveView.FindAndReplace(bReplace);
			}
		}

		/// <summary>
		/// Finds the next occurance of the last searched string or invokes 
		/// the Find and Replace dialog if there are no searched strings.
		/// </summary>
		public void FindNext()
		{
			if (!HasContent)
			{
				return;
			}
			if(FRDlg.TextSearched == string.Empty)
			{
				FindAndReplace(false);
			}
			else
			{
				if (editActiveView != null)
				{
					editActiveView.FindNext(FRDlg.TextSearched, false);
				}
			}
		}

		/// <summary>
		/// Finds the first occurance of the specified string.
		/// </summary>
		/// <param name="lc">The starting location of the search.</param>
		/// <param name="str">The string to be searched.</param>
		/// <param name="bSearchUp">A value indicating whether the search is 
		/// upward.</param>
		/// <returns>The location of the string found.</returns>
		public EditLocationRange Find(EditLocation lc, string str, bool bSearchUp)
		{
			return editData.Find(lc, str, false, false, false, bSearchUp, 
				false, false, true);
		}

		/// <summary>
		/// Finds the first occurance of the specified string.
		/// </summary>
		/// <param name="lc">The starting location of the search.</param>
		/// <param name="str">The string to be searched.</param>
		/// <param name="bMatchCase">A value indicating whether matching is 
		/// case-sensitive.</param>
		/// <param name="bMatchWholeWord">A value indicating whether matching
		/// is whole-word based.</param>
		/// <param name="bSearchHiddenText">A value indicating whether hidden 
		/// text should be searched.</param>
		/// <param name="bSearchUp">A value indicating whether the search is 
		/// upward.</param>
		/// <param name="bUseRegex">A value indicating whether regular 
		/// expressions are used.</param>
		/// <param name="bUseWildcards">A value indicating whether wild cards 
		/// are used.</param>
		/// <param name="bWholeRange">A value indicating whether to search 
		/// in the whole document.</param>
		/// <returns>The location of the string found.</returns>
		public EditLocationRange Find(EditLocation lc, string str, 
			bool bMatchCase, bool bMatchWholeWord, 
			bool bSearchHiddenText, bool bSearchUp, 
			bool bUseRegex, bool bUseWildcards, bool bWholeRange)
		{
			return editData.Find(lc, str, bMatchCase, bMatchWholeWord, 
				bSearchHiddenText, bSearchUp, bUseRegex, bUseWildcards, 
				bWholeRange);
		}

		/// <overload>
		/// Sets the displayed lines for the specified line.
		/// </overload>
		/// <summary>
		/// Sets the displayed lines for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the displayed lines are to be 
		/// set.</param>
		/// <param name="lnOccupied">The displayed lines for the line.</param>
		public void SetLinesOccupied(int ln, int lnOccupied)
		{
			SetLinesOccupied(ln, (short)lnOccupied);
			Redraw();
		}

		/// <summary>
		/// Sets the displayed lines for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the displayed lines are to 
		/// be set.</param>
		/// <param name="lnOccupied">The displayed lines for the line.</param>
		public void SetLinesOccupied(int ln, short lnOccupied)
		{
			editData.SetLinesOccupied(ln, lnOccupied);
			Redraw();
		}

		/// <summary>
		/// Gets the displayed lines for the specified line.
		/// </summary>
		/// <returns>The displayed lines for the specified line.</returns>
		public int GetLinesOccupied(int ln)
		{
			return editData.GetLinesOccupied(ln);
		}

		/// <summary>
		/// Moves the caret to the specified location.
		/// </summary>
		/// <param name="lc">The destination caret location.</param>
		public void GoToLineChar(EditLocation lc)
		{
			if (lc < FirstLineChar)
			{
				CurrentLineChar = FirstLineChar;
			}
			else if (lc > LastLineChar)
			{
				CurrentLineChar = LastLineChar;
			}
			else
			{
				CurrentLineChar = lc;
			}
			ScrollToViewCaret();
		}

		/// <summary>
		/// Moves the caret to the specified line/column location.
		/// </summary>
		/// <param name="lc">The destination caret location.</param>
		public void GoToLineColumn(EditLocation lc)
		{
			GoToLineChar(LineCharFromLineColumn(lc));
		}

		/// <summary>
		/// Increases the indent of the specified line.
		/// </summary>
		/// <param name="ln">The line for which the indent is to be 
		/// increased.</param>
		public void IncreaseLineIndent(int ln)
		{
			if (!Updateable)
			{
				return;
			}
			CurrentLine = ln;
			CurrentChar = 1;
			Insert(GetTabString(CurrentColumn));
		}

		/// <summary>
		/// Increases the indent for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range.</param>
		/// <param name="lnEnd">The ending line of the line range.</param>
		private void IncreaseLineIndent(int lnStart, int lnEnd)
		{
			if (!Updateable)
			{
				return;
			}
			BeginLogCompositeAction(EditActionType.Indent);
			BeginUpdate();
			for (int i = lnStart; i <= lnEnd; i++)
			{
				IncreaseLineIndent(i);
			}
			EndUpdate();
			EndLogCompositeAction();
			Redraw();
		}

		/// <summary>
		/// Increases the indent of the selected lines.
		/// </summary>
		public void IncreaseLineIndent()
		{
			if (!Updateable)
			{
				return;
			}
			if (!HasSelection)
			{
				IncreaseLineIndent(CurrentLine, CurrentLine);
			}
			else
			{
				int startLn = editSelection.GetStart().L;
				int endLn =  editSelection.GetEnd().L;
				if (editSelection.GetEnd().C == 1) 
				{
					endLn -= 1;
				}
				IncreaseLineIndent(startLn, endLn);
			}
		}

		/// <summary>
		/// Decreases the indent of the specified line.
		/// </summary>
		/// <param name="ln">The line for which the indent is to be 
		/// decreased.</param>
		public void DecreaseLineIndent(int ln)
		{
			if (!Updateable)
			{
				return;
			}
			if (GetLineLength(ln) == 0)
			{
				return;
			}
			string strLine = GetString(ln);
			if (strLine[0] != '\t')
			{
				if (strLine.Length >= TabSize)
				{
					if (strLine.Substring(0, TabSize) == new string(' ', TabSize))
					{
						Delete(ln, 1, ln, 1 + TabSize);
					}
				}
			}
			else 
			{
				Delete(ln, 1, ln, 2);
			}
		}

		/// <summary>
		/// Decreases the indent for the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range.</param>
		/// <param name="lnEnd">The ending line of the line range.</param>
		public void DecreaseLineIndent(int lnStart, int lnEnd)
		{
			if (!Updateable)
			{
				return;
			}
			BeginLogCompositeAction(EditActionType.Unindent);
			BeginUpdate();
			for (int i = lnStart; i <= lnEnd; i++)
			{
				DecreaseLineIndent(i);
			}
			EndUpdate();
			EndLogCompositeAction();
			Redraw();
		}

		/// <summary>
		/// Decreases the indent of the selected lines.
		/// </summary>
		public void DecreaseLineIndent()
		{
			if (!Updateable)
			{
				return;
			}
			if (!HasSelection)
			{
				DecreaseLineIndent(CurrentLine, CurrentLine);
			}
			else
			{
				DecreaseLineIndent(editSelection.GetStart().L, editSelection.GetEnd().L);
			}
		}

		/// <summary>
		/// Makes the text in the specified location range uppercase.
		/// </summary>
		/// <param name="lcr">The location range for which the character case 
		/// is to be changed.</param>
		/// <param name="bToUpper">A value indicating whether the character 
		/// case is to be changed into upper case.</param>
		public void ChangeCase(EditLocationRange lcr, bool bToUpper)
		{
			EditLocationRange lcrNorm = lcr.Normalize();
			BeginLogCompositeAction(EditActionType.ChangeCase);
			BeginUpdate();
			string strTemp = GetString(lcrNorm);
			Delete(lcrNorm);
			if (bToUpper)
			{
				Insert(lcrNorm.Start, strTemp.ToUpper());
			}
			else
			{
				Insert(lcrNorm.Start, strTemp.ToLower());
			}
			EndUpdate();
			EndLogCompositeAction();
			editData.InvalidateRangeInfo(lcrNorm.Start, lcrNorm.End);
			RedrawFromLine(lcrNorm.Start.L);
		}

		/// <summary>
		/// Makes the selection uppercase.
		/// </summary>
		public void MakeUpperCase()
		{
			if (!Updateable)
			{
				return;
			}
			if (HasSelection)
			{
				EditLocationRange lcrNorm = editSelection.Normalize();
				ChangeCase(lcrNorm, true);
			}
			else
			{
				if (CurrentChar < GetLineLength(CurrentLine))
				{
					ChangeCase(new EditLocationRange(CurrentLineChar, 
						new EditLocation(CurrentLine, CurrentChar + 1)), true);
				}
				CharRight();
			}
		}

		/// <summary>
		/// Makes the selection lowercase.
		/// </summary>
		public void MakeLowerCase()
		{
			if (!Updateable)
			{
				return;
			}
			if (HasSelection)
			{
				EditLocationRange lcrNorm = editSelection.Normalize();
				ChangeCase(lcrNorm, false);
			}
			else
			{
				if (CurrentChar < GetLineLength(CurrentLine))
				{
					ChangeCase(new EditLocationRange(CurrentLineChar, 
						new EditLocation(CurrentLine, CurrentChar + 1)), false);
				}
				CharRight();
			}
		}

		/// <summary>
		/// Gets the string of leading spaces for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the string of leading spaces 
		/// is to be obtained.</param>
		/// <returns>The string of the leading spaces of the line.</returns>
		public string GetLeadingSpace(int ln)
		{
			int NonSpaceChar = GetFirstNonSpaceChar(ln);
			return GetString(ln).Substring(0, NonSpaceChar - 1);
		}

		/// <summary>
		/// Comments out the specified line.
		/// </summary>
		/// <param name="ln">The line to be commented out.</param>
		public void CommentLine(int ln)
		{
			// Do nothing for empty line comment characters.
			if ((LineComment == null) || (LineComment == string.Empty))
			{
				return;
			}
			// Insert the line comment characters.
			CurrentLine = ln;
			CurrentChar = 1;
			Insert(LineComment);
		}

		/// <summary>
		/// Comments out the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range.</param>
		/// <param name="lnEnd">The ending line of the line range.</param>
		public void CommentLines(int lnStart, int lnEnd)
		{
			if (!Updateable)
			{
				return;
			}
			BeginLogCompositeAction(EditActionType.CommentLines);
			BeginUpdate();
			for (int i = lnStart; i <= lnEnd; i++)
			{
				CommentLine(i);
			}
			EndUpdate();
			EndLogCompositeAction();
			Redraw();
		}

		/// <summary>
		/// Comments out the selected lines.
		/// </summary>
		public void CommentSelection()
		{
			if (!Updateable)
			{
				return;
			}
			if (!HasSelection)
			{
				CommentLines(CurrentLine, CurrentLine);
			}
			else
			{
				CommentLines(editSelection.GetStart().L, editSelection.GetEnd().L);
			}
		}

		/// <summary>
		/// Uncomments the specified line.
		/// </summary>
		/// <param name="ln">The line to be uncommented.</param>
		public void UncommentLine(int ln)
		{
			// Do nothing for empty line comment characters.
			if ((LineComment == null) || (LineComment == string.Empty))
			{
				return;
			}
			string strLine = GetString(ln);
			int loc = strLine.IndexOf(LineComment);
			if (loc == -1)
			{
				// Do nothing if no comment symbols are found.
				return;
			}
			else 
			{
				// Only when the comment symbols are the first no space characters.
				if (loc == GetFirstNonSpaceChar(ln) - 1)
				{
					Delete(ln, loc + 1, ln, loc + 1 + LineComment.Length);
				}
			}
		}

		/// <summary>
		/// Uncomments the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range.</param>
		/// <param name="lnEnd">The ending line of the line range.</param>
		public void UncommentLines(int lnStart, int lnEnd)
		{
			if (!Updateable)
			{
				return;
			}
			BeginLogCompositeAction(EditActionType.UncommentLines);
			BeginUpdate();
			for (int i = lnStart; i <= lnEnd; i++)
			{
				UncommentLine(i);
			}
			EndUpdate();
			EndLogCompositeAction();
			Redraw();
		}

		/// <summary>
		/// Uncomments the selected lines.
		/// </summary>
		public void UncommentSelection()
		{
			if (!Updateable)
			{
				return;
			}
			if (!HasSelection)
			{
				UncommentLines(CurrentLine, CurrentLine);
			}
			else
			{
				UncommentLines(editSelection.GetStart().L, editSelection.GetEnd().L);
			}
		}

		/// <summary>
		/// Replaces spaces with equivalent tabs for the selected text.
		/// </summary>
		public void TabifySelection()
		{
			if (!Updateable)
			{
				return;
			}
			if (HasSelection)
			{
				EditLocationRange lcrNorm = editSelection.Normalize();
				UnSelect();
				EditLocationRange newSel = ConvertTabsSpaces(lcrNorm, false);
				Select(newSel);
			}
			else
			{
				ConvertTabsSpaces(new EditLocationRange(CurrentLine, 1, 
					CurrentLine, GetLineLengthPlusOne(CurrentLine)), false);
			}
		}

		/// <summary>
		/// Replaces tabs with equivalent spaces for the selected text.
		/// </summary>
		public void UntabifySelection()
		{
			if (!Updateable)
			{
				return;
			}
			if (HasSelection)
			{
				EditLocationRange lcrNorm = editSelection.Normalize();
				UnSelect();
				EditLocationRange newSel = ConvertTabsSpaces(lcrNorm, true);
				Select(newSel);
			}
			else
			{
				ConvertTabsSpaces(new EditLocationRange(CurrentLine, 1, 
					CurrentLine, GetLineLengthPlusOne(CurrentLine)), true);
			}
		}

		/// <summary>
		/// Converts the tabs/spaces for the specified location range.
		/// </summary>
		/// <param name="lcr">The location range for the conversion.</param>
		/// <param name="bToSpaces">A value indicating whether the convertion 
		/// is from tabs to spaces.</param>
		public EditLocationRange ConvertTabsSpaces(EditLocationRange lcr, bool bToSpaces)
		{
			EditLocationRange lcrNorm = lcr.Normalize();
			BeginLogCompositeAction(EditActionType.ConvertTabsSpaces);
			BeginUpdate();
			string strTemp = editData.ConvertTabsSpaces(lcrNorm, bToSpaces);
			Delete(lcrNorm);
			EditLocationRange lcrTemp = Insert(lcrNorm.Start, strTemp);
			EndUpdate();
			EndLogCompositeAction();
			editData.InvalidateRangeInfo(lcrTemp.Start, lcrTemp.End);
			RedrawFromLine(lcrNorm.Start.L);
			return lcrTemp;
		}

		/// <summary>
		/// Deletes white space around the current caret location.
		/// </summary>
		public void DeleteHorizontalWhiteSpace()
		{
			if (GetLineLength(CurrentLine) == 0)
			{
				return;
			}
			BeginLogCompositeAction(EditActionType.DeleteHorizontalWhiteSpace);
			if (!HasSelection) 
			{
				DeleteHorizontalWhiteSpace(CurrentLine, CurrentChar);
			}
			else
			{
				DeleteHorizontalWhiteSpace(editSelection);
			}
			EndLogCompositeAction();
		}

		/// <overload>
		/// Deletes redundant white spaces in the specified location range.
		/// </overload>
		/// <summary>
		/// Deletes redundant white spaces in the specified location range.
		/// </summary>
		/// <param name="lcr">The location range for which the redundant 
		/// white spaces are to be deleted.</param>
		/// <returns>The resulted location range after deletion of redundant 
		/// white spaces.</returns>
		public EditLocationRange DeleteHorizontalWhiteSpace(EditLocationRange lcr)
		{
			EditLocationRange lcrNorm = lcr.Normalize();
			BeginUpdate();
			string strTemp = editData.DeleteHorizontalWhiteSpace(lcrNorm);
			Delete(lcrNorm);
			EditLocationRange lcrTemp = Insert(lcrNorm.Start, strTemp);
			EndUpdate();
			editData.InvalidateRangeInfo(lcrTemp.Start, lcrTemp.End);
			RedrawFromLine(lcrNorm.Start.L);
			return lcrTemp;
		}

		/// <summary>
		/// Deletes redundant white spaces in the specified location range.
		/// </summary>
		/// <param name="lcStart">The starting location of the location 
		/// range.</param>
		/// <param name="lcEnd">The ending location of the location range.
		/// </param>
		/// <returns>The resulted location range after deletion of redundant 
		/// white spaces.</returns>
		public EditLocationRange DeleteHorizontalWhiteSpace(EditLocation lcStart,
			EditLocation lcEnd)
		{
			return DeleteHorizontalWhiteSpace(new EditLocationRange(lcStart, lcEnd));
		}

		/// <summary>
		/// Deletes redundant white spaces in the specified location range.
		/// </summary>
		/// <param name="lnStart">The line of the starting location.</param>
		/// <param name="chStart">The char of the starting location.</param>
		/// <param name="lnEnd">The line of the ending location.</param>
		/// <param name="chEnd">The char of the ending location.</param>
		/// <returns>The resulted location range after deletion of redundant 
		/// white spaces.</returns>
		public EditLocationRange DeleteHorizontalWhiteSpace(int lnStart, int chStart,
			int lnEnd, int chEnd)
		{
			return DeleteHorizontalWhiteSpace(new EditLocationRange(lnStart, chStart,
				lnEnd, chEnd));
		}

		/// <overload>
		/// Deletes white spaces around the specified location.
		/// </overload>
		/// <summary>
		/// Deletes white spaces around the specified location.
		/// </summary>
		/// <param name="ln">The line of the location.</param>
		/// <param name="ch">The char of the location.</param>
		/// <returns>true if any white space has been deleted; otherwise, 
		/// false.</returns>
		public bool DeleteHorizontalWhiteSpace(int ln, int ch)
		{
			int chTemp1 = GetFirstNonSpaceChar(ln);
			int chTemp2 = GetLastNonSpaceChar(ln);
			if ((chTemp1 > 1) && (ch <= chTemp1))
			{
				Delete(ln, 1, ln, chTemp1);
				return true;
			}
			else if ((chTemp2 < GetLineLength(ln)) && (ch > chTemp2))
			{
				Delete(ln, chTemp2 + 1, ln, GetLineLengthPlusOne(ln));
				return true;
			}
			else if ((ch > chTemp1) && ch <= chTemp2)
			{
				int ch1;
				int ch2;
				editData.GetWhiteSpaceRange(ln, ch, out ch1, out ch2);
				if ((ch2 - ch1) > 1)
				{
					Delete(ln, ch1 + 1, ln, ch2);
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Deletes white spaces around the specified location.
		/// </summary>
		/// <param name="lc">The location around which the white spaces 
		/// are to be removed.</param>
		/// <returns>true if any white space has been deleted; otherwise, 
		/// false.</returns>
		public bool DeleteHorizontalWhiteSpace(EditLocation lc)
		{
			return DeleteHorizontalWhiteSpace(lc.L, lc.C);
		}

		/// <summary>
		/// Inserts the string of current time/date at the current caret 
		/// location.
		/// </summary>
		public void TimeDate()
		{
			Insert(CurrentTimeDate);
		}

		/// <summary>
		/// Gets the first non-white-space char index for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the first non-white-space 
		/// char index is to be obtained.</param>
		/// <returns>The first non-white-space char index of the line.</returns>
		public int GetFirstNonSpaceChar(int ln)
		{
			return editData.GetFirstNonSpaceChar(ln);
		}

		/// <summary>
		/// Gets the next non-white-space char index after the specified 
		/// location.
		/// </summary>
		/// <param name="ln">The line of the location after which the 
		/// next non-white-space char index is to be obtained.</param>
		/// <param name="ch">The char of the location after which the 
		/// next non-white-space char index is to be obtained.</param>
		/// <returns>The next non-white-space char index after the 
		/// location.</returns>
		public int GetNextNonSpaceChar(int ln, int ch)
		{
			return editData.GetNextNonSpaceChar(ln, ch);
		}

		/// <summary>
		/// Gets the last non-white-space char index for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the last non-white-space 
		/// char index is to be obtained.</param>
		/// <returns>The last non-white-space char index of the line.</returns>
		public int GetLastNonSpaceChar(int ln)
		{
			return editData.GetLastNonSpaceChar(ln);
		}

		/// <summary>
		/// Gets the previous non-white-space char index before the specified 
		/// location.
		/// </summary>
		/// <param name="ln">The line of the location before which the 
		/// previous non-white-space char index is to be obtained.</param>
		/// <param name="ch">The char of the location before which the 
		/// previous non-white-space char index is to be obtained.</param>
		/// <returns>The previous non-white-space char index before the 
		/// location.</returns>
		public int GetPreviousNonSpaceChar(int ln, int ch)
		{
			return editData.GetPreviousNonSpaceChar(ln, ch);
		}

		/// <summary>
		/// Tests if the specified line starts with the specified char as 
		/// the first non-space char.
		/// </summary>
		/// <param name="ln">The line to be tested.</param>
		/// <param name="c">The char to be tested.</param>
		/// <returns>true if the char is the first non-space char of the 
		/// line; otherwise, false.</returns>
		public bool IsFirstNonSpaceChar(int ln, char c)
		{
			string strLine = GetStringObject(ln);
			int index = GetFirstNonSpaceChar(ln) - 1;
			if (index == strLine.Length)
			{
				return false;
			}
			else
			{
				return (strLine[index] == c);
			}
		}

		/// <summary>
		/// Gets the smart repeating string for the specified line.
		/// </summary>
		/// <param name="ln">The line for which the smart repeating string 
		/// is to be obtained.</param>
		/// <returns>The smart repeating string for the line.</returns>
		public string GetSmartRepeatString(int ln)
		{
			string strLine = GetStringObject(ln);
			int index = GetFirstNonSpaceChar(ln) - 1;
			if (strLine.Substring(index).StartsWith(editSettings.SmartRepeatTag))
			{
				int prefixLength = index + editSettings.SmartRepeatTag.Length;
				for (int j = prefixLength; j < strLine.Length; j++)
				{
					if ((strLine[j] == ' ') || (strLine[j] == '\t'))
					{
						prefixLength = j + 1;
					}
					else
					{
						break;
					}
				}
				return strLine.Substring(0, prefixLength);
			}
			return string.Empty;
		}

		/// <summary>
		/// Gets the first non-white-space column index at the specified line.
		/// </summary>
		/// <param name="ln">The line for which the the first non-white-space 
		/// column index is to be obtained.</param>
		/// <returns>The first non-white-space column index of the line.</returns>
		public int GetFirstNonSpaceColumn(int ln)
		{
			return GetColumnFromChar(ln, GetFirstNonSpaceChar(ln));
		}

		/// <summary>
		/// Gets the indenting string for the specified column.
		/// </summary>
		/// <param name="col">The total number of column for the indenting 
		/// string.</param>
		/// <returns>The indenting string for the column.</returns>
		public string GetIndentString(int col)
		{
			return GetSpaceString(1, col);
		}

		/// <summary>
		/// Gets the string of white spaces for the specified column range.
		/// </summary>
		/// <param name="colStart">The starting column.</param>
		/// <param name="colEnd">The ending column.</param>
		/// <returns>The string of white spaces for the column range.</returns>
		internal string GetSpaceString(int colStart, int colEnd)
		{
			if (colEnd <= colStart)
			{
				return string.Empty;
			}
			if (KeepTabs)
			{
				string strTemp = string.Empty;
				int nTemp1 = (colStart - 1)%TabSize;
				int nTemp2 = (colEnd - colStart + nTemp1)/TabSize;
				if (nTemp2 > 0)
				{
					strTemp += new string('\t', nTemp2);
					nTemp2 = (colEnd - colStart + nTemp1)%TabSize;
					if (nTemp2 > 0)
					{
						strTemp += new string(' ', nTemp2);
					}
				}
				else
				{
					nTemp2 = (colEnd - colStart + nTemp1)%TabSize - nTemp1;
					if (nTemp2 > 0)
					{
						strTemp += new string(' ', nTemp2);
					}
				}
				return strTemp;
			}
			else
			{
				return new string(' ', colEnd - colStart);
			}
		}

		/// <summary>
		/// Gets the equivalent string of spaces for a tab at the specified 
		/// column.
		/// </summary>
		/// <param name="col">The column at which the equivalent string of
		/// spaces for a tab is to be obtained.</param>
		/// <returns>The equivalent string of spaces for a tab at the column.
		/// </returns>
		internal string GetTabString(int col)
		{
			if (KeepTabs)
			{
				int nIndent = IndentSize - (col - 1)%IndentSize;
				int nTab = TabSize - (col - 1)%TabSize;
				if (nIndent < nTab)
				{
					return new string(' ', nIndent);
				}
				else if (nIndent == nTab)
				{
					return "\t";
				}
				else
				{
					string strTemp = string.Empty;
					int colTemp = col;
					while (nTab <= nIndent)
					{
						strTemp += "\t";
						nIndent -= nTab;
						colTemp += nTab;
						nTab = TabSize - (colTemp - 1)%TabSize;
					}
					if (nIndent > 0)
					{
						strTemp += new string(' ', nIndent);
					}
					return strTemp;
				}
			}
			else
			{
				return new string(' ', IndentSize - (col - 1)%IndentSize);
			}
		}

		/// <summary>
		/// Deletes the word at the caret location.
		/// </summary>
		public void DeleteCurrentWord()
		{
			Delete(GetWordLocationRange(CurrentLineChar));
		}

		/// <summary>
		/// Gets the word at the caret location.
		/// </summary>
		/// <returns>The word at the caret location.</returns>
		public string GetCurrentWord()
		{
			return GetWord(CurrentLine, CurrentChar);
		}

		/// <summary>
		/// Gets the word at the specified location.
		/// </summary>
		/// <param name="lc">The location at which the word is to be 
		/// obtained.</param>
		/// <returns>The word at the location.</returns>
		public string GetWord(EditLocation lc)
		{
			return editData.GetWord(lc.L, lc.C);
		}

		/// <summary>
		/// Gets the word at the specified location.
		/// </summary>
		/// <param name="ln">The line of the location at which the word 
		/// is to be obtained.</param>
		/// <param name="ch">The char of the location at which the word 
		/// is to be obtained.</param>
		/// <returns>The word at the location.</returns>
		public string GetWord(int ln, int ch)
		{
			return editData.GetWord(ln, ch);
		}

		/// <summary>
		/// Gets the location range for the word at the specified location.
		/// </summary>
		/// <param name="ln">The line of the location at which the word is 
		/// located.</param>
		/// <param name="ch">The char of the location at which the word is 
		/// located.</param>
		/// <returns>The location range for the word.</returns>
		public EditLocationRange GetWordLocationRange(int ln, int ch)
		{
			return editData.GetWordLocationRange(ln, ch);
		}

		/// <summary>
		/// Gets the location range for the word at the specified location.
		/// </summary>
		/// <param name="lc">The location at which the word is located.</param>
		/// <returns>The location range for the word.</returns>
		public EditLocationRange GetWordLocationRange(EditLocation lc)
		{
			return editData.GetWordLocationRange(lc.L, lc.C);
		}

		/// <summary>
		/// Gets the current word location range.
		/// </summary>
		/// <returns>The location range for the word at the current caret 
		/// location</returns>
		public EditLocationRange GetCurrentWordLocationRange()
		{
			return GetWordLocationRange(CurrentLineChar);
		}

		/// <summary>
		/// Checks if the displaying of brace matching needs to be updated.
		/// </summary>
		internal void CheckBraceMatching()
		{
			BraceMatchingBegin = EditLocationRange.Empty;
			BraceMatchingEnd = EditLocationRange.Empty;
			EditLocationRange lcr = GetWordLocationRange(CurrentLineChar);
			if (lcr != EditLocationRange.Empty)
			{
				string strWord = GetString(lcr);
				if (strWord == "(")
				{
					BraceMatchingBegin = lcr;
					BraceMatchingEnd = editData.FindMatch(lcr.End, "(", ")", true);
				}
				else if (strWord == "[")
				{
					BraceMatchingBegin = lcr;
					BraceMatchingEnd = editData.FindMatch(lcr.End, "[", "]", true);
				}
				else if (strWord == "{")
				{
					BraceMatchingBegin = lcr;
					BraceMatchingEnd = editData.FindMatch(lcr.End, "{", "}", true);
				}
				else if (strWord == ")")
				{
					BraceMatchingEnd = lcr;
					BraceMatchingBegin = editData.FindMatch(lcr.Start, "(", ")", false);
				}
				else if (strWord == "]")
				{
					BraceMatchingEnd = lcr;
					BraceMatchingBegin = editData.FindMatch(lcr.Start, "[", "]", false);
				}
				else if (strWord == "}")
				{
					BraceMatchingEnd = lcr;
					BraceMatchingBegin = editData.FindMatch(lcr.Start, "{", "}", false);
				}
			}
		}

		/// <summary>
		/// Gets the string enclosed by the specified starting char and ending 
		/// char at the the specified location, or the current word.
		/// </summary>
		/// <param name="lc">The location of the string to be obtained.</param>
		/// <param name="chStart">The starting char for the string.</param>
		/// <param name="chEnd">The ending char for the string.</param>
		/// <param name="chEscape">The escape char for the ending char.</param>
		/// <returns>The string enclosed with the specified starting char and 
		/// ending char, or the current word.
		/// </returns>
		public EditLocationRange GetEnclosedString(EditLocation lc, string beginTag, 
			string endTag, string escapeChar)
		{
			EditLocationRange lcr = GetWordLocationRange(lc);
			if (lcr == EditLocationRange.Empty)
			{
				return lcr;
			}
			string strLine = GetString(CurrentLine);
			EditLocation lcTemp = new EditLocation(lc);
			if (lc.C == strLine.Length + 1)
			{
				lcTemp.C = strLine.Length;
			}
			string regexStr;
			if (endTag == string.Empty)
			{
				regexStr = Regex.Escape(beginTag) + ".*$";
			}
			else
			{
				if (escapeChar == string.Empty)
				{
					regexStr = Regex.Escape(beginTag) + ".*?" + Regex.Escape(endTag);
				}
				else
				{
					regexStr = Regex.Escape(beginTag) + ".*?" + "(?<!" 
						+ Regex.Escape(escapeChar) + ")" + Regex.Escape(endTag);
				}
				regexStr += "|" + Regex.Escape(beginTag) + ".*$";
			}
			RegexOptions ro = RegexOptions.Singleline;
			if (!MatchCase)
			{
				ro |= RegexOptions.IgnoreCase;
			}
			MatchCollection matches = Regex.Matches(strLine, regexStr, ro);
			for (int i = 0; i < matches.Count; i++)
			{
				if ((lcTemp.C >= matches[i].Index + 1) && 
					(lcTemp.C < matches[i].Index + matches[i].Length + 1))
				{
					return new EditLocationRange(lcTemp.L, matches[i].Index + 1, 
						lcTemp.L, matches[i].Index + matches[i].Length + 1);
				}
			}
			return lcr;
		}

		#endregion

		#region Methods Related to Displaying

		/// <summary>
		/// Sets the context menu to be enabled or disabled.
		/// </summary>
		/// <param name="bEnabled">A value indicating whether the main 
		/// context menu is to be enabled.</param>
		public void EnableContextMenu(bool bEnabled)
		{
			if (bEnabled)
			{
				editViewTop.ContextMenu = ContextMenu;
				editViewBottom.ContextMenu = ContextMenu;
			}
			else
			{
				editViewTop.ContextMenu = editEmptyContextMenu;
				editViewBottom.ContextMenu = editEmptyContextMenu;
			}
		}

		/// <summary>
		/// Restarts the SelectionMarginTimer to draw the selection margin 
		/// later.
		/// </summary>
		internal void RedrawSelectionMarginLater()
		{
			if (!AutomaticOutliningEnabled)
			{
				return;
			}
			bSelectionMarginChanged = true;
			editSelectionMarginTimer.Stop();
			editSelectionMarginTimer.Start();
		}

		/// <summary>
		/// Disables painting in the views.
		/// </summary>
		public void BeginUpdate()
		{
			if (editViewTop != null)
			{
				editViewTop.BeginUpdate();
			}
			if (editViewBottom != null)
			{
				editViewBottom.BeginUpdate();
			}
		}

		/// <summary>
		/// Enable painting in the views.
		/// </summary>
		public void EndUpdate()
		{
			if (editViewBottom != null)
			{
				editViewBottom.EndUpdate();
			}
			if (editViewTop != null)
			{
				editViewTop.EndUpdate();
			}
		}

		/// <summary>
		/// Updates the ScrollBars.
		/// </summary>
		public void UpdateScrollBars()
		{
			UpdateHScrollBar();
			UpdateVScrollBar();
		}

		/// <summary>
		/// Updates the horizontal scrollbar.
		/// </summary>
		public void UpdateHScrollBar()
		{
			if (editViewTop != null)
			{
				editViewTop.UpdateHScrollBar();
			}
			if (editViewBottom != null)
			{
				editViewBottom.UpdateHScrollBar();
			}
		}

		/// <summary>
		/// Updates the vertical scrollbar.
		/// </summary>
		public void UpdateVScrollBar()
		{
			if (editViewTop != null)
			{
				editViewTop.UpdateVScrollBar();
			}
			if (editViewBottom != null)
			{
				editViewBottom.UpdateVScrollBar();
			}
		}

		/// <summary>
		/// Redraws from the specified line.
		/// </summary>
		/// <param name="ln">The line from which redrawing starts.</param>
		public void RedrawFromLine(int ln)
		{
			bool bRedrawFrom = false;
			if (editData.MultiLineBlockList.LocationUpdated.L < ln)
			{
				bRedrawFrom = true;
			}
			else
			{
				EditAdvTagInfo ati;
				for (int i = 0; i < editSettings.AdvTagInfoList.Count; i++)
				{
					ati = (EditAdvTagInfo)editSettings.AdvTagInfoList[i];
					if (ati.SubMultiLineBlockList.LocationUpdated.L < ln)
					{
						bRedrawFrom = true;
						break;
					}
				}
			}
			if (bRedrawFrom)
			{
				if (editViewTop != null)
				{
					editViewTop.RedrawFromLine(ln);
				}
				if (editViewBottom != null)
				{
					editViewBottom.RedrawFromLine(ln);
				}
			}
			else
			{
				if (editViewTop != null)
				{
					editViewTop.RedrawLine(ln);
				}
				if (editViewBottom != null)
				{
					editViewBottom.RedrawLine(ln);
				}
			}
		}

		/// <summary>
		/// Redraws the specified line.
		/// </summary>
		/// <param name="ln">The line to be redrawn.</param>
		public void RedrawLine(int ln)
		{
			if (editViewTop != null)
			{
				editViewTop.RedrawLine(ln);
			}
			if (editViewBottom != null)
			{
				editViewBottom.RedrawLine(ln);
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
			if (editViewTop != null)
			{
				editViewTop.RedrawLine(ln, bForced);
			}
			if (editViewBottom != null)
			{
				editViewBottom.RedrawLine(ln, bForced);
			}
		}

		/// <summary>
		/// Redraws the specified line range.
		/// </summary>
		/// <param name="lnStart">The starting line of the line range.</param>
		/// <param name="lnEnd">The ending line of the line range.</param>
		public void RedrawLines(int lnStart, int lnEnd)
		{
			if (editViewTop != null)
			{
				editViewTop.RedrawLines(lnStart, lnEnd);
			}
			if (editViewBottom != null)
			{
				editViewBottom.RedrawLines(lnStart, lnEnd);
			}
		}

		/// <summary>
		/// Gets an arraylist of all the color group names.
		/// </summary>
		/// <returns>The arraylist of all the color group names.</returns>
		public ArrayList GetColorGroupList()
		{
			ArrayList alTemp = new ArrayList();
			for (int i = 0; i < editSettings.ColorGroupList.Count; i++)
			{
				alTemp.Add(editSettings.ColorGroupList[i].GroupName);
			}
			return alTemp;
		}

		#endregion

		#region Methods Related to Caret Handling

		/// <summary>
		/// Scrolls to the viewport that contains the caret.
		/// </summary>
		/// <returns>true if the current view is not null and viewport has 
		/// been scrolled to show caret; otherwise, false.</returns>
		public bool ScrollToViewCaret()
		{
			if (editActiveView != null)
			{
				return editActiveView.ScrollToViewCaret();
			}
			return false;
		}

		/// <summary>
		/// Goes to the viewport that contains the caret without redrawing.
		/// </summary>
		/// <returns>true if the viewport has been scrolled to show caret; 
		/// otherwise, false.</returns>
		public bool GoToCaretViewport()
		{
			if (editActiveView != null)
			{
				return editActiveView.GoToCaretViewport();
			}
			return false;
		}

		/// <summary>
		/// Update the caret location in the current active view.
		/// </summary>
		public void UpdateCaretPos()
		{
			if (editActiveView != null)
			{
				editActiveView.UpdateCaretPos();
			}
		}

		/// <summary>
		/// Hides the caret.
		/// </summary>
		public void HideCaret()
		{
			if (editActiveView != null)
			{
				editActiveView.HideCaret();
			}
		}

		/// <summary>
		/// Shows the caret in the current active view.
		/// </summary>
		public void ShowCaret()
		{
			if (editActiveView != null)
			{
				editActiveView.ShowCaret();
			}
		}

		/// <summary>
		/// Shows the caret in the current active view (equivalent to ShowCaret).
		/// </summary>
		public void DisplayCaret()
		{
			ShowCaret();
		}

		#endregion

		#region Methods Related to Redo/Undo Handling

		/// <summary>
		/// Logs the specified action for undo/redo.
		/// </summary>
		/// <param name="act">The action to be logged.</param>
		public void LogAction(EditAction act)
		{
			ClearRedo();
			if (act.ActionType == EditActionType.Type)
			{
				if (editUndoRedo.CanUndo)
				{
					EditAction lastAction = editUndoRedo.PeekUndoAction();
					if (lastAction.ActionType == EditActionType.Type)
					{
						if (MergeTypeActions((EditCompositeAction)lastAction, 
							(EditCompositeAction)act))
						{
							return;
						}
					}
				}
			}
			int prevUndoCount = editUndoRedo.UndoableActionCount;
			editUndoRedo.AddUndoAction(act);
			if (prevUndoCount == 0)
			{
				OnCanUndoChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Merges two typing actions into one typing action.
		/// </summary>
		/// <param name="act1">The first action.</param>
		/// <param name="act2">The second action.</param>
		/// <returns>true if two typing actions have been merged into one 
		/// typing action; otherwise, false.</returns>
		protected bool MergeTypeActions(EditCompositeAction act1, EditCompositeAction act2)
		{
			if ((act1.ActionType != EditActionType.Type)
				||(act2.ActionType != EditActionType.Type))
			{
				return false;
			}
			if (((EditCompositeAction)act2).Count != 1)
			{
				return false;
			}
			EditActionInsert act1Last = (EditActionInsert)act1.ActionList[act1.Count-1];
			EditActionInsert act2First = (EditActionInsert)act2.ActionList[0];
			if (act1Last.LcEnd == act2First.LcStart)
			{
				act1Last.LcEnd = act2First.LcEnd;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Begins logging the specified composite action.
		/// </summary>
		/// <param name="at">The composite action to be logged.</param>
		public void BeginLogCompositeAction(EditActionType at)
		{
			if (editCompositeAction != null)
			{
				editCompositeActionStack.Push(editCompositeAction);
			}
			editCompositeAction = new EditCompositeAction(at, this);
		}

		/// <summary>
		/// Ends logging the current composite action.
		/// </summary>
		public void EndLogCompositeAction()
		{
			if (editCompositeAction.Count > 0)
			{
				editCompositeAction.ActionList.TrimToSize();
				if (editCompositeActionStack.Count == 0)
				{
					LogAction(editCompositeAction);
					editCompositeAction = null;
				}
				else
				{
					EditCompositeAction editCompositeAction0 = editCompositeAction;
					editCompositeAction = (EditCompositeAction)editCompositeActionStack.Pop();
					editCompositeAction.AddAction(editCompositeAction0);
				}
			}
			else
			{
				if (editCompositeActionStack.Count == 0)
				{
					editCompositeAction = null;
				}
				else
				{
					editCompositeAction = (EditCompositeAction)editCompositeActionStack.Pop();
				}
			}
		}

		/// <summary>
		/// Undoes the last action.
		/// </summary>
		public void Undo()
		{
			int prevUndoCount = editUndoRedo.UndoableActionCount;
			int prevRedoCount = editUndoRedo.RedoableActionCount;
			UnSelect();
			editUndoRedo.Undo();
			if ((prevUndoCount > 0) && (editUndoRedo.UndoableActionCount == 0))
			{
				OnCanUndoChanged(new EventArgs());
			}
			if ((prevRedoCount == 0) && (editUndoRedo.RedoableActionCount > 0))
			{
				OnCanRedoChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Redoes the last action.
		/// </summary>
		public void Redo()
		{
			int prevUndoCount = editUndoRedo.UndoableActionCount;
			int prevRedoCount = editUndoRedo.RedoableActionCount;
			editUndoRedo.Redo();
			if ((prevRedoCount > 0) && (editUndoRedo.RedoableActionCount == 0))
			{
				OnCanRedoChanged(new EventArgs());
			}
			if ((prevUndoCount == 0) && (editUndoRedo.UndoableActionCount > 0))
			{
				OnCanUndoChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Clears the undo stack.
		/// </summary>
		public void ClearUndo()
		{
			int prevUndoCount = editUndoRedo.UndoableActionCount;
			editUndoRedo.ClearUndo();
			if (prevUndoCount > 0)
			{
				OnCanUndoChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Clears the redo stack.
		/// </summary>
		public void ClearRedo()
		{
			int prevRedoCount = editUndoRedo.RedoableActionCount;
			editUndoRedo.ClearRedo();
			if (prevRedoCount > 0)
			{
				OnCanRedoChanged(new EventArgs());
			}
		}

		/// <summary>
		/// Clears information of brace matching.
		/// </summary>
		public void ClearBraceMatching()
		{
			editBraceMatchingBegin = EditLocationRange.Empty;
			editBraceMatchingEnd = EditLocationRange.Empty;
		}

		/// <summary>
		/// Gets the description of the specified action object.
		/// </summary>
		/// <param name="act">The action object for which the description 
		/// is to be obtained.</param>
		/// <returns>The description of the action object</returns>
		public static string GetActionDescription(EditAction act)
		{
			string strTemp;
			switch (act.ActionType)
			{
				case EditActionType.None:
					strTemp = "Not an action";
					break;
				case EditActionType.UserAction:
					strTemp = "User action";
					break;
				case EditActionType.Insert:
					strTemp = "Insert";
					break;
				case EditActionType.Delete:
					strTemp = "Delete";
					break;
				case EditActionType.MoveCaret:
					strTemp = "Move caret";
					break;
				case EditActionType.SelectText:
					strTemp = "Select text";
					break;
				case EditActionType.Cut:
					strTemp = "Cut";
					break;
				case EditActionType.Paste:
					strTemp = "Paste";
					break;
				case EditActionType.Type:
					strTemp = "Type";
					break;
				case EditActionType.Tab:
					strTemp = "Tab";
					break;
				case EditActionType.Enter:
					strTemp = "Enter";
					break;
				case EditActionType.Backspace:
					strTemp = "Backspace";
					break;
				case EditActionType.Indent:
					strTemp = "Indent";
					break;
				case EditActionType.Unindent:
					strTemp = "Unindent";
					break;
				case EditActionType.CommentLines:
					strTemp = "Comment lines";
					break;
				case EditActionType.UncommentLines:
					strTemp = "Uncomment lines";
					break;
				case EditActionType.DragDrop:
					strTemp = "Drag/Drop";
					break;
				case EditActionType.ConvertTabsSpaces:
					strTemp = "Convet tabs/spaces";
					break;
				case EditActionType.Replace:
					strTemp = "Replace";
					break;
				case EditActionType.ReplaceAll:
					strTemp = "Replace all";
					break;
				case EditActionType.ChangeCase:
					strTemp = "Change case";
					break;
				case EditActionType.DeleteHorizontalWhiteSpace:
					strTemp = "Delete horizontal white space";
					break;
				default:
					strTemp = "Unknown action";
					break;
			}
			return strTemp;
		}

		#endregion

		#region Methods Related to File Operations

		/// <summary>
		/// Restores the default settings.
		/// </summary>
		internal void ReloadSettings()
		{
			string strTemp = editSettings.SettingFile;
			editSettings.ResetSettings();
			SettingFile = strTemp;
		}

		/// <summary>
		/// Initializes settings from the specified stream.
		/// </summary>
		/// <param name="stream">The stream for settings.</param>
		/// <returns>true if settings are set successfully; otherwise,
		/// false.</returns>
		public bool UseSettingStream(Stream stream)
		{
			StringBuilder strBdr = new StringBuilder(string.Empty);
			if(stream != null)
			{
				int chTemp;
				while((chTemp = stream.ReadByte()) != -1)
				{
					strBdr.Append((char)chTemp);
				}
				if(editSettings.ReadFromString(strBdr.ToString()))
				{
					ProcessSettings();
					return true;
				}
			}
			else
			{
				ShowInfoMessage(GetResourceString("SettingStreamNull"), 
					GetResourceString("ReadSettings"));
			}
			SyntaxColoringEnabled = false;
			return false;
		}

		/// <summary>
		/// Initializes settings from the specified embedded resource file.
		/// </summary>
		/// <param name="type">The class used to extract the resource.</param>
		/// <param name="resource">The name of the resource.</param>
		/// <returns>true if settings are set successfully; otherwise,
		/// false.</returns>
		public bool UseEmbeddedSettingFile(Type type, string resource)
		{
			StringBuilder strBdr = new StringBuilder(string.Empty);
			Assembly assembly = type.Module.Assembly;
			Stream stream = assembly.GetManifestResourceStream(type, resource);
			if(stream != null)
			{
				int chTemp;
				while((chTemp = stream.ReadByte()) != -1)
				{
					strBdr.Append((char)chTemp);
				}
				if(editSettings.ReadFromString(strBdr.ToString()))
				{
					ProcessSettings();
					return true;
				}
			}
			else
			{
				ShowInfoMessage(GetResourceString("SettingFileNotFound"), 
					GetResourceString("ReadSettings"));
			}
			SyntaxColoringEnabled = false;
			return false;
		}
		/// <summary>
		/// Initializes settings from the specified embedded resource file.
		/// </summary>
		/// <param name="resource">The fully qualified name of the resource in the calling assembly.</param>
		/// <returns>true if settings are set successfully; otherwise,
		/// false.</returns>
		public bool UseEmbeddedSettingFile(string resource)
		{
			StringBuilder strBdr = new StringBuilder(string.Empty);
			Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resource);
			if(stream != null)
			{
				int chTemp;
				while((chTemp = stream.ReadByte()) != -1)
				{
					strBdr.Append((char)chTemp);
				}
				if(editSettings.ReadFromString(strBdr.ToString()))
				{
					ProcessSettings();
					return true;
				}
			}
			else
			{
				ShowInfoMessage(GetResourceString("SettingFileNotFound"), 
					GetResourceString("ReadSettings"));
			}
			SyntaxColoringEnabled = false;
			return false;
		}
		/// <summary>
		/// Initializes settings from the specified built-in Ini file.
		/// </summary>
		/// <param name="fileName">The name of the built-in Ini file.</param>
		/// <returns>true if settings are set successfully; otherwise,
		/// false.</returns>
		internal bool UseBuiltInSettingFile(string fileName)
		{
			return UseEmbeddedSettingFile(typeof(EditControl), 
				"Resources." + fileName.ToUpper());
		}

		/// <summary>
		/// Initializes resource strings from the specified embedded resource 
		/// file.
		/// </summary>
		/// <param name="type">The class used to extract the resource strings.
		/// </param>
		/// <param name="resource">The name of the resource.</param>
		/// <returns>true if resource strings are set successfully; otherwise,
		/// false.</returns>
		public bool UseResourceStringFile(Type type, string resource)
		{
			StringBuilder strBdr = new StringBuilder(string.Empty);
			Assembly assembly = type.Module.Assembly;
			Stream stream = assembly.GetManifestResourceStream(type, resource);
			if(stream != null)
			{
				int chTemp;
				while((chTemp = stream.ReadByte()) != -1)
				{
					strBdr.Append((char)chTemp);
				}
				if (editSettings.ReadResourceStrings(strBdr.ToString()))
				{
					if (editContextMenu != null)
					{
						SetMenuItemText();
					}
					if (editFindReplaceDlg != null)
					{
						editFindReplaceDlg.SetDialogItemText(this);
					}
					if (editOptionsDlg != null)
					{
						editOptionsDlg.SetDialogItemText(this);
					}
					return true;
				}
				else
				{
					return false;
				}
			}
			return false;
		}

		/// <summary>
		/// Gets the resource string for the specified string name.
		/// </summary>
		/// <param name="strName">The string name for which the resource string 
		/// is to be obtained.</param>
		/// <returns>The resource string for the given string name.</returns>
		internal string GetResourceString(string strName)
		{
			for (int i = 0; i < editSettings.ResourceStringList.Count; i += 2)
			{
				if (((string)editSettings.ResourceStringList[i]).CompareTo(strName) == 0)
				{
					return (string)editSettings.ResourceStringList[i+1];
				}
			}
			return string.Empty;
		}

		/// <summary>
		/// Creates a new empty file.
		/// </summary>
		public void NewFile()
		{
			if (!SaveModified())
			{
				return;
			}
			ClearContents();
			CurrentFile = "Untitled.txt";
			InsertEmptyFirstLine();
			Modified = false;
			UpdateAll();
			ResetView();
			UpdateContextMenu();
			Focus();
		}

		/// <summary>
		/// Closes the current file.
		/// </summary>
		public void Close()
		{
			if (!HasContent)
			{
				return;
			}
			if (!SaveModified())
			{
				return;
			}
			ClearContents();
			CurrentFile = null;
			Modified = false;
			HideTopView();
			HideCaret();
			UpdateAll();
			ResetView();
		}

		/// <summary>
		/// Prompts the user with a save dialog if the current file was 
		/// modified.
		/// </summary>
		/// <result> true if the file has been saved or the modifications 
		/// are OK to be neglected; otherwise, false. </result>
		public bool SaveModified()
		{
			if ((!ReadOnly) && Modified)
			{
				DialogResult result;
				result = MessageBox.Show(GetResourceString("SaveChanges"),
					GetResourceString("SaveFile"), 
					MessageBoxButtons.YesNoCancel, 
					MessageBoxIcon.Question);
				if (result == DialogResult.Yes)
				{
					// Save the current file.
					Save();
					return true;
				}
				else if (result == DialogResult.No)
				{
					// Do not save the current file.
					return true;
				}
				else if (result == DialogResult.Cancel)
				{
					// Stay at the current state.
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Saves the current file.
		/// </summary>
		public void Save()
		{
			if (CurrentFile == "Untitled.txt")
			{
				SaveAs();
			}
			else
			{
				SaveFile(CurrentFile);
			}
		}

		/// <summary>
		/// Displays the OpenFile dialog.
		/// </summary>
		public virtual void Open()
		{
			if (!SaveModified())
			{
				return;
			}
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = GetOpenFileFilters();
			dlg.CheckFileExists = true;
			dlg.CheckPathExists = true;
			dlg.FilterIndex = 1;
			if (dlg.ShowDialog() ==  DialogResult.OK)
			{
				LoadFile(dlg.FileName);
			}
		}

		/// <summary>
		/// Saves the text contents in the edit to a new file.
		/// </summary>
		public void SaveAs()
		{
			if (!HasContent)
			{
				ShowInfoMessage(GetResourceString("NoContentsToSave"), 
					GetResourceString("SaveAs"));
				return;
			}
			SaveFileDialog dlg = new SaveFileDialog();
			dlg.Filter = GetSaveFileFilters();
			dlg.FilterIndex = 1;
			if (dlg.ShowDialog() ==  DialogResult.OK)
			{
				CurrentFile = dlg.FileName;
				if (CurrentFile.LastIndexOf(".htm") == (CurrentFile.Length - 4))
				{
					editData.SaveAsHTML(CurrentFile);
				}
				else if (CurrentFile.LastIndexOf(".rtf") == (CurrentFile.Length - 4))
				{
					editData.SaveAsRTF(CurrentFile);
				}
				else
				{
					SaveFile(CurrentFile);
				}
			}
		}

		/// <summary>
		/// Saves the text contents in the edit to the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file to which the text 
		/// contents in the edit will be saved to.</param>
		public void SaveFile(string fileName)
		{
			if (!HasContent)
			{
				ShowInfoMessage(GetResourceString("NoContentsToSave"), 
					GetResourceString("SaveFile"));
				return;
			}
			if (editData.SaveFile(fileName))
			{
				Modified = false;
			}
		}

		/// <summary>
		/// Inserts the contents of a file to the current caret location.
		/// </summary>
		public void InsertFile()
		{
			if (!Updateable)
			{
				return;
			}
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.CheckFileExists = true;
			dlg.CheckPathExists = true;
			dlg.Filter = GetFileFilter() 
				+ "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
			dlg.FilterIndex = 1;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				InsertFile(dlg.FileName);
			}
		}

		/// <summary>
		/// Saves text and color information to an HTML file.
		/// </summary>
		/// <param name="fileName">The name of the HTML file to which text 
		/// and color information will be saved.</param>
		public void SaveAsHTML(string fileName)
		{
			editData.SaveAsHTML(fileName);
		}

		/// <summary>
		/// Saves text and color information to a RTF file.
		/// </summary>
		/// <param name="fileName">The name of the RTF file to which text 
		/// and color information will be saved.</param>
		public void SaveAsRTF(string fileName)
		{
			editData.SaveAsRTF(fileName);
		}

		/// <summary>
		/// Reads the syntax coloring settings from the specified file.
		/// </summary>
		/// <param name="fileName">The name of the file from which the syntax 
		/// coloring settings will be read.</param>
		internal void ReadSettingsFromFile(string fileName)
		{
			if (editSettings.ReadFromFile(fileName))
			{
				ProcessSettings();
			}
			else
			{
				SyntaxColoringEnabled = false;
			}
		}

		/// <summary>
		/// Writes the syntax coloring settings to a file.
		/// </summary>
		/// <param name="fileName">The name of the file to which the syntax
		/// coloring settings will be saved.</param>
		public void SaveSettingsToFile(string fileName)
		{
			editSettings.WriteToFile(fileName);
		}

		#endregion

		#region Methods Related to Printing

		/// <summary>
		/// Prints the current text contents in the edit.
		/// </summary>
		public void Print()
		{
			if (!HasContent)
			{
				ShowInfoMessage(GetResourceString("NoContentsToPrint"), 
					GetResourceString("Print"));
				return;
			}
			try 
			{
				// Assumes the default printer.
				editPrintCurrentLine = 1;
				editPrintCurrentPage = 1;
				if (editPrintDocument.PrinterSettings.SupportsColor) 
				{
					// Sets the page default's to not print in color.
					editPrintDocument.DefaultPageSettings.Color = false;
				}
				editPrintDocument.DocumentName = CurrentFile;
				PrintDialog pDlg = new PrintDialog();
				pDlg.Document = editPrintDocument;
				if (HasSelection)
				{
					pDlg.AllowSelection = true;
				}
				DialogResult result;
				result = pDlg.ShowDialog();
				if (result == DialogResult.OK)
				{
					bPrintSelection = pDlg.AllowSelection;
					editPrintDocument.Print();
				}
			}
			catch(Exception e) 
			{
				ShowErrorMessage(e.Message, GetResourceString("Print"));
			}
		}

		/// <summary>
		/// Shows the Print Preview dialog.
		/// </summary>
		public void PrintPreview()
		{
			if (!HasContent)
			{
				ShowInfoMessage(GetResourceString("NoContentsToPrintPreview"), 
					GetResourceString("PrintPreview"));
				return;
			}
			try 
			{
				PrintPreviewDialog ppDlg = new PrintPreviewDialog();
				ppDlg.Document = editPrintDocument;
				editPrintCurrentLine = 1;
				ppDlg.ShowDialog();
			}
			catch(Exception e) 
			{
				ShowErrorMessage(e.Message, GetResourceString("PrintPreview"));
			}
		}

		/// <summary>
		/// Handles the BeginPrint event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An EventArgs that contains the event data.</param>
		private void Pd_BeginPrint(object sender, PrintEventArgs e)
		{
			editPrintCurrentLine = bPrintSelection ? 
				editSelection.GetStart().L : 1;
		}

		/// <summary>
		/// Handles the EndPrint event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A PrintEventArgs that contains the event data.</param>
		private void Pd_EndPrint(object sender, PrintEventArgs e)
		{
			editPrintCurrentLine = 1;
			editPrintCurrentPage = 1;
		}

		/// <summary>
		/// Handles the PrintPage event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A PrintPageEventArgs that contains the event 
		/// data.</param>
		private void Pd_PrintPage(object sender, PrintPageEventArgs e)
		{
			float YCoord = e.MarginBounds.Top;
			float YCoordTemp;
			float leftMargin = e.MarginBounds.Left;
			float topMargin = e.MarginBounds.Top;
			string strTemp;
			SizeF sizeLine;
			RectangleF rectF;
			sizeLine = e.Graphics.MeasureString(editCurrentFile,
				Font, e.MarginBounds.Width, StringFormatTab);
			rectF = new RectangleF(e.MarginBounds.Left, YCoord - sizeLine.Height 
				- Font.Height, sizeLine.Width, sizeLine.Height);
			if (FileNameVisible)
			{
				e.Graphics.DrawString(editCurrentFile, Font, 
					Brushes.Black, rectF, StringFormatTab);
			}
			e.Graphics.DrawString(editPrintCurrentPage.ToString(), Font, 
				Brushes.Black, new PointF(e.MarginBounds.Right, rectF.Top), 
				StringFormatTab);
			e.Graphics.DrawLine(new Pen(Color.Black, 2), 
				e.MarginBounds.Left, rectF.Bottom + 4, 
				e.MarginBounds.Right, rectF.Bottom + 4);
			// Iterates through the text buffer, prints each line.
			if (bPrintSelection)
			{
				while (editPrintCurrentLine <= editSelection.GetEnd().L)
				{
					if (editPrintCurrentLine == editSelection.GetStart().L)
					{
						strTemp = GetString(editPrintCurrentLine).
							Substring(editSelection.GetStart().C - 1);
					}
					else if (editPrintCurrentLine == editSelection.GetEnd().L)
					{
						strTemp = GetString(editPrintCurrentLine).
							Substring(0, editSelection.GetStart().C - 1);
					}
					else
					{
						strTemp = GetString(editPrintCurrentLine);
					}
					if (strTemp == string.Empty)
					{
						sizeLine = e.Graphics.MeasureString(" ",
							Font, e.MarginBounds.Width, StringFormatTab);
					}
					else
					{
						sizeLine = e.Graphics.MeasureString(GetString(editPrintCurrentLine),
							Font, e.MarginBounds.Width, StringFormatTab);
					}
					YCoordTemp = YCoord + sizeLine.Height;
					if (YCoordTemp < e.MarginBounds.Bottom)
					{
						rectF = new RectangleF(e.MarginBounds.Left, 
							YCoord, sizeLine.Width, sizeLine.Height);
						e.Graphics.DrawString (strTemp, Font, 
							Brushes.Black, rectF, StringFormatTab);
						editPrintCurrentLine++;
						YCoord = YCoordTemp;
					}
					else
					{
						editPrintCurrentPage++;
						break;
					}
				}
				// If more lines exist, print another page.
				e.HasMorePages = (editPrintCurrentLine < editSelection.GetEnd().L);
				return;
			}
			int lnLast = LineCount;
			while (editPrintCurrentLine <= lnLast)
			{
				strTemp = GetString(editPrintCurrentLine);
				if (strTemp == string.Empty)
				{
					sizeLine = e.Graphics.MeasureString(" ",
						Font, e.MarginBounds.Width, StringFormatTab);
				}
				else
				{
					sizeLine = e.Graphics.MeasureString(GetString(editPrintCurrentLine),
						Font, e.MarginBounds.Width, StringFormatTab);
				}
				YCoordTemp = YCoord + sizeLine.Height;
				if (YCoordTemp < e.MarginBounds.Bottom)
				{
					rectF = new RectangleF(e.MarginBounds.Left, YCoord, sizeLine.Width, sizeLine.Height);
					e.Graphics.DrawString (strTemp, Font, 
						Brushes.Black, rectF, StringFormatTab);
					editPrintCurrentLine++;
					YCoord = YCoordTemp;
				}
				else
				{
					editPrintCurrentPage++;
					break;
				}
			}
			// If more lines exist, print another page.
			e.HasMorePages = (editPrintCurrentLine < LineCount);
		}

		#endregion 

		#region Internal Properties
		/// <override/>
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			this.ActiveControl = this.editActiveView;
		}


		/// <summary>
		/// Gets or sets the current selection.
		/// </summary>
		internal EditSelection Selection
		{
			get
			{
				return editSelection;
			}
			set
			{
				editSelection = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether an action is being executed.
		/// </summary>
		internal bool InAction
		{
			get
			{
				return bInAction;
			}
			set
			{
				bInAction = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether splitting is in progress.
		/// </summary>
		internal bool InSplitting
		{
			get
			{
				return bInSplitting;
			}
		}

		/// <summary>
		/// Gets or sets the original caret X position.
		/// </summary>
		internal float OriginalX
		{
			get
			{
				return editOriginalX;
			}
			set
			{
				editOriginalX = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether selection margin redrawing is 
		/// needed.
		/// </summary>
		internal bool SelectionMarginRedrawWaiting
		{
			get
			{
				return editSelectionMarginTimer.Enabled;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the selection margin has been 
		/// changed and thus should be redrawn.
		/// </summary>
		internal bool SelectionMarginChanged
		{
			get
			{
				return bSelectionMarginChanged;
			}
			set
			{
				bSelectionMarginChanged = value;
			}
		}

		/// <summary>
		/// Gets or sets the string format for string drawing with Tabs.
		/// </summary>
		internal StringFormat StringFormatTab
		{
			get
			{
				return editStringFormatTab;
			}
			set
			{
				editStringFormatTab = value;
			}
		}

		/// <summary>
		/// Gets or sets the string format for string drawing without Tabs.
		/// </summary>
		internal StringFormat StringFormatNoTab
		{
			get
			{
				return editStringFormatNoTab;
			}
			set
			{
				editStringFormatNoTab = value;
			}
		}

		/// <summary>
		/// Gets the char width array.
		/// </summary>
		internal float [] CharWidth
		{
			get
			{
				return editCharWidth;
			}
		}

		/// <summary>
		/// Gets or sets the width of the border.
		/// </summary>
		internal int BorderWidth
		{
			get
			{
				return editBorderWidth;
			}
			set
			{
				editBorderWidth = value;
			}
		}

		/// <summary>
		/// Gets the array of tab stops.
		/// </summary>
		internal float [] TabStops
		{
			get
			{
				return editTabStops;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the char widths have been 
		/// updated.
		/// </summary>
		internal bool CharWidthUpdated
		{
			get
			{
				return editCharWidthUpdated;
			}
			set
			{
				editCharWidthUpdated = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the control is in the design mode.
		/// </summary>
		internal bool InDesignMode
		{
			get
			{
				return this.DesignMode;
			}
		}

		/// <summary>
		/// Gets or sets the maximum number of columns among all lines.
		/// </summary>
		internal int MaxColumn
		{
			get
			{
				return editMaxColumn;
			}
			set
			{
				editMaxColumn = value;
			}
		}

		/// <summary>
		/// Gets the bitmap of the panning image.
		/// </summary>
		internal Bitmap PanImage
		{
			get
			{
				return editPanImage;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating whether an incremental search 
		/// is in progress.
		/// </summary>
		internal bool ISearch
		{
			get
			{
				return bISearch;
			}
			set
			{
				bISearch = value;
			}
		}

		/// <summary>
		/// Gets or sets the string for the incremental search.
		/// </summary>
		internal string ISearchString
		{
			get
			{
				return editISearchString;
			}
			set
			{
				editISearchString = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating whether a drag-and-drop is 
		/// initiated inside.
		/// </summary>
		internal bool DragInit
		{
			get
			{
				return bDragInit;
			}
			set
			{
				bDragInit = value;
			}
		}

		/// <summary>
		/// Gets the north-eastern direction cursor.
		/// </summary>
		internal Cursor NECursor
		{
			get
			{
				return editNECursor;
			}
		}

		/// <summary>
		/// Gets the incremental search cursor.
		/// </summary>
		internal Cursor ISearchCursor
		{
			get
			{
				return editISearchCursor;
			}
		}

		/// <summary>
		/// Gets the drag-and-drop cursor.
		/// </summary>
		internal Cursor DragMoveCursor
		{
			get
			{
				return editDragMoveCursor;
			}
		}

		/// <summary>
		/// Gets the Find and Replace dialog.
		/// </summary>
		internal FindReplaceDlg FRDlg
		{
			get
			{
				return editFindReplaceDlg;
			}
		}

		/// <summary>
		/// Gets the EditData object.
		/// </summary>
		internal EditData Data
		{
			get
			{
				return editData;
			}
		}

		/// <summary>
		/// Gets the EditSettings object.
		/// </summary>
		internal EditSettings Settings
		{
			get
			{
				return editSettings;
			}
		}

		/// <summary>
		/// Gets an empty context menu.
		/// </summary>
		internal ContextMenu EmptyContextMenu
		{
			get
			{
				return editEmptyContextMenu;
			}
		}

		/// <summary>
		/// Gets or sets the active view.
		/// </summary>
		internal EditView ActiveView
		{
			get
			{
				return editActiveView;
			}
			set
			{
				editActiveView = value;
			}
		}

		/// <summary>
		/// Gets or sets the old active view.
		/// </summary>
		internal EditView OldActiveView
		{
			get
			{
				return editOldActiveView;
			}
			set
			{
				editOldActiveView = value;
			}
		}

		/// <summary>
		/// Gets the height of the splitter.
		/// </summary>
		internal int SplitterHeight
		{
			get
			{
				return editSplitter.Height;
			}
		}

		/// <summary>
		/// Gets the string of the current time/date.
		/// </summary>
		internal string CurrentTimeDate
		{
			get
			{
				return DateTime.Now.ToShortTimeString() + " " 
					+ DateTime.Now.Month + "/"
					+ DateTime.Now.Day + "/"
					+ DateTime.Now.Year;
			}
		}

		/// <summary>
		/// Gets or sets the location range of the begin symbol in brace 
		/// matching.
		/// </summary>
		internal EditLocationRange BraceMatchingBegin
		{
			get
			{
				return editBraceMatchingBegin;
			}
			set
			{
				if (editBraceMatchingBegin != value)
				{
					if (editBraceMatchingBegin != EditLocationRange.Empty)
					{
						RedrawLine(editBraceMatchingBegin.Start.L, true);
					}
					editBraceMatchingBegin = new EditLocationRange(value);
					if (editBraceMatchingBegin != EditLocationRange.Empty)
					{
						RedrawLine(editBraceMatchingBegin.Start.L, true);
					}				
				}
			}
		}

		/// <summary>
		/// Gets or sets the location range of the end symbol in brace 
		/// matching.
		/// </summary>
		internal EditLocationRange BraceMatchingEnd
		{
			get
			{
				return editBraceMatchingEnd;
			}
			set
			{
				if (editBraceMatchingEnd != value)
				{
					if (editBraceMatchingEnd != EditLocationRange.Empty)
					{
						RedrawLine(editBraceMatchingEnd.Start.L, true);
					}
					editBraceMatchingEnd = new EditLocationRange(value);
					if (editBraceMatchingEnd != EditLocationRange.Empty)
					{
						RedrawLine(editBraceMatchingEnd.Start.L, true);
					}				
				}			
			}
		}

		/// <summary>
		/// Gets the list of available indicators.
		/// </summary>
		internal EditIndicatorList IndicatorList
		{
			get
			{
				return editIndicatorList;
			}
		}

		/// <summary>
		/// Gets a value indicating whether any ContextTooltip handler is 
		/// active.
		/// </summary>
		internal bool UserMarginPaintActive
		{
			get
			{
				return (UserMarginPaint != null);
			}
		}

		/// <summary>
		/// Gets a value indicating whether any ContextTooltip handler is 
		/// active.
		/// </summary>
		internal bool ContextTooltipPopupActive
		{
			get
			{
				return (ContextTooltipEnabled && (ContextTooltipPopup != null));
			}
		}

		/// <summary>
		/// Gets a value indicating whether any ContextChoice handler is 
		/// active.
		/// </summary>
		internal bool ContextChoicePopupActive
		{
			get
			{
				return (ContextChoiceEnabled && (ContextChoicePopup != null));
			}
		}

		/// <summary>
		/// Gets a value indicating whether any ContextPrompt handler is 
		/// active.
		/// </summary>
		internal bool ContextPromptPopupActive
		{
			get
			{
				return (ContextPromptEnabled && (ContextPromptPopup != null));
			}
		}

		/// <summary>
		/// Gets a value indicating whether any ContextPrompt handler is 
		/// active.
		/// </summary>
		internal bool ContentChangedActive
		{
			get
			{
				return (ContentChangedEventEnabled && (ContentChanged != null));
			}
		}

		/// <summary>
		/// Gets a value indicating whether any ContextPrompt handler is 
		/// active.
		/// </summary>
		internal bool DoubleClickSelectActive
		{
			get
			{
				return (DoubleClickSelect != null);
			}
		}

		/// <summary>
		/// Gets a value indicating whether any LineInfoUpdate handler is 
		/// active.
		/// </summary>
		internal bool LineInfoUpdateActive
		{
			get
			{
				return (LineInfoUpdate != null);
			}
		}

		#endregion

		#region Public Properties
		/// <summary>
		/// Gets or sets the display property for the splitter.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The display property for the splitter."),
		Browsable(true),
		DefaultValue(true)
		]
		public bool ShowSplitterButton
		{
			get
			{
				return bShowSplitterButton;//editViewBottom.SplitterButtonVisible;
			}
			set
			{
				bShowSplitterButton = value;
				editViewBottom.SplitterButtonVisible = value;
			}
		}
		
		/// <summary>
		/// Enables/Disables copy without selection.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Enable/Disable copy without selection."),
		DefaultValue(true)
		]
		public bool CopyWithoutSelection
		{
			get
			{
				return bCopyWithoutSelection;
			}
			set
			{
				bCopyWithoutSelection = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the text in the edit control.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The text in the edit control."),
		Browsable(false)
		]
		public override string Text
		{
			get
			{
				return GetAllText();
			}
			set
			{
				if (!HasContent)
				{
					if (value != null)
					{
						NewFile();
						if (value != string.Empty)
						{
							AppendText(value);
						}
					}
				}
				else
				{
					if ((value != null) && (value != GetAllText()))
					{
						Clear();
						AppendText(value);
					}
				}
			}
		}

		/// <summary>
		/// Gets the length of text in the edit control.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The length of text in the edit control."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public int TextLength
		{
			get
			{
				return Text.Length;
			}
		}

		/// <summary>
		/// Gets the location range of the current selection.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The location range of the current selection."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public EditLocationRange SelectionLocationRange
		{
			get
			{
				return editSelection;
			}
		}

		/// <summary>
		/// Gets or sets the column for the right margin line.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The column for the right margin line."),
		Browsable(false)
		]
		public int RightMarginLineColumn
		{
			get
			{
				return editRightMarginLineColumn;
			}
			set
			{
				editRightMarginLineColumn = value;
			}
		}

		/// <summary>
		/// Gets or sets the string that a comment block collapses as.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The string that a comment block collapses as."),
		Browsable(false), 
		DefaultValue("/**/")
		]
		public string CollapsedComments
		{
			get
			{
				return editSettings.CollapsedComments;
			}
			set
			{
				editSettings.CollapsedComments = value;
			}
		}

		/// <summary>
		/// Gets or sets the default string that an outlining block 
		/// collapses as.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The default string that an outlining block collapses as."),
		Browsable(false), 
		DefaultValue("...")
		]
		public string CollapsedDefault
		{
			get
			{
				return editSettings.CollapsedDefault;
			}
			set
			{
				editSettings.CollapsedDefault = value;
			}
		}

		/// <summary>
		/// Gets or sets the char that causes a ContextChoice window to be
		/// popped up.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The char that causes a ContextChoice window to be popped up."),
		Browsable(false), 
		DefaultValue(new char[]{'.'})
		]
		public char[] ContextChoiceChar
		{
			get
			{
				return editSettings.ContextChoiceChar;
			}
			set
			{
				editSettings.ContextChoiceChar = value;
			}
		}

		/// <summary>
		/// Gets or sets the char that causes a ContextPrompt window to be 
		/// popped up.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The char that causes a ContextPrompt window to be popped up."),
		Browsable(false), 
		DefaultValue('(')
		]
		public char ContextPromptBeginChar
		{
			get
			{
				return editSettings.ContextPromptBeginChar;
			}
			set
			{
				editSettings.ContextPromptBeginChar = value;
			}
		}

		/// <summary>
		/// Gets or sets the char that causes a ContextPrompt window to be 
		/// hidden.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The char that causes a ContextPrompt window to be hidden."),
		Browsable(false), 
		DefaultValue(')')
		]
		public char ContextPromptEndChar
		{
			get
			{
				return editSettings.ContextPromptEndChar;
			}
			set
			{
				editSettings.ContextPromptEndChar = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the language is 
		/// case-sensitive.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether the language is case-sensitive."),
		Browsable(false),
		DefaultValue(true)
		]
		public bool MatchCase
		{
			get
			{
				return !editSettings.IgnoreCase;
			}
			set
			{
				editSettings.IgnoreCase = !value;
			}
		}

		/// <summary>
		/// Gets the Advanced menu item.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The Advanced menu item."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public MenuItem AdvancedMenu
		{
			get
			{
				return editMenuItemAdvanced;
			}
		}

		/// <summary>
		/// Gets the Bookmark menu item.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The Bookmark menu item."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public MenuItem BookmarkMenu
		{
			get
			{
				return editMenuItemBookmark;
			}
		}

		/// <summary>
		/// Gets the Outlining menu item.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The Outlining menu item."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public MenuItem OutliningMenu
		{
			get
			{
				return editMenuItemOutlining;
			}
		}

		/// <summary>
		/// Gets the File menu item.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The File menu item."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public MenuItem FileMenu
		{
			get
			{
				return editMenuItemFile;
			}
		}

		/// <summary>
		/// Gets the Edit menu item.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The Edit menu item."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public MenuItem EditMenu
		{
			get
			{
				return editMenuItemEdit;
			}
		}

		/// <summary>
		/// Gets or sets the starting line of the current viewport.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The starting line of the current viewport."),
		Browsable(false), 
		DefaultValue(1)
		]
		public int ViewportFirstLine
		{
			get
			{
				if (editActiveView != null)
				{
					return editActiveView.ViewportFirstLine;
				}
				return 1;
			}
			set
			{
				if (editActiveView != null)
				{
					editActiveView.ViewportFirstLine = value;
				}
			}
		}

		/// <summary>
		/// Gets the ending line of the current viewport.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The ending line of the current viewport."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public int ViewportLastLine
		{
			get
			{
				if (editActiveView != null)
				{
					return editActiveView.ViewportLastLine;
				}
				return -1;
			}
		}

		/// <summary>
		/// Gets or sets the starting column of the current viewport.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The starting column of the current viewport."),
		Browsable(false), 
		DefaultValue(1)
		]
		public int ViewportFirstColumn
		{
			get
			{
				if (editActiveView != null)
				{
					return editActiveView.ViewportFirstColumn;
				}
				return 1;
			}
			set
			{
				if (editActiveView != null)
				{
					editActiveView.ViewportFirstColumn = value;
				}
			}
		}

		/// <summary>
		/// Gets the total number of lines in the currentviewport.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The total number of lines in the current viewport."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public int ViewportLineCount
		{
			get
			{
				if (editActiveView != null)
				{
					return editActiveView.ViewportLineCount;
				}
				return -1;
			}
		}

		/// <summary>
		/// Gets the total number of columns in the current viewport.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The total number of columns in the current viewport."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public int ViewportColumnCount
		{
			get
			{
				if (editActiveView != null)
				{
					return editActiveView.ViewportColumnCount;
				}
				return -1;
			}
		}

		/// <summary>
		/// Gets the total number of lines in the edit control.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The total number of lines in the edit control."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public int LineCount
		{
			get
			{
				return editData.LineList.Count;
			}
		}

		/// <summary>
		/// Gets the total number of visible lines in the edit control.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The total number of visible lines in the edit control."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public int VisibleLineCount
		{
			get
			{
				return (!OutliningEnabled) ? 
					editData.LineList.Count : editData.VisibleLineCount;
			}
		}

		/// <summary>
		/// Gets the first valid location of the document.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The first valid location of the document."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public EditLocation FirstLineChar
		{
			get
			{
				return editFirstLineChar;
			}
		}

		/// <summary>
		/// Gets the last valid location of the document.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The last valid location of the document."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public EditLocation LastLineChar
		{
			get
			{
				if (LineCount == 0)
				{
					editLastLineChar.L = 1;
					editLastLineChar.C = 1;
				}
				else
				{
					editLastLineChar.L = LineCount;
					editLastLineChar.C = GetLineLengthPlusOne(LineCount);
				}
				return editLastLineChar;
			}
		}

		/// <summary>
		/// Gets a value indicating whether there is any text selected.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether there is any text selected."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool HasSelection
		{
			get
			{
				return editSelection.HasSelection;
			}
		}

		/// <summary>
		/// Gets or sets the value of the current line number.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The value of the current line number."),
		Browsable(false), 
		DefaultValue(1)
		]
		public int CurrentLine
		{
			get
			{
				return editCurrentLineChar.L;
			}
			set
			{
				if (editCurrentLineChar.L != value)
				{
					editCurrentLineChar.L = value;
					if (!InsertMode)
					{
						UpdateCaretSize();
					}
					if (BraceMatchingEnabled)
					{
						CheckBraceMatching();
					}
					SetPanelTextInternal(1, "Ln " + value.ToString());
					OnCurrentLineChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the value of the current char number.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The value of the current char number."),
		Browsable(false), 
		DefaultValue(1)
		]
		public int CurrentChar
		{
			get
			{
				return editCurrentLineChar.C;
			}
			set
			{
				if (editCurrentLineChar.C != value)
				{
					editCurrentLineChar.C = value;
					if (value > 0)
					{
						SetPanelTextInternal(3, "Ch " + value.ToString());
					}
					if (!InsertMode)
					{
						UpdateCaretSize();
					}
					if (BraceMatchingEnabled)
					{
						CheckBraceMatching();
					}
					OnCurrentCharChanged(new EventArgs());
				}
				if (HasContent)
				{
					SetPanelTextInternal(2, GetResourceString("StatusColumn") 
						+ " " + GetColumnFromChar(CurrentLine, value));
				}
			}
		}

		/// <summary>
		/// Gets or sets the current location of the caret.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The current location of the caret."),
		Browsable(false)
		]
		public EditLocation CurrentLineChar
		{
			get
			{
				return editCurrentLineChar;
			}
			set
			{
				CurrentLine = value.L;
				CurrentChar = value.C;
			}
		}

		/// <summary>
		/// Gets the selected text.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The selected text."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public string SelectedText
		{
			get
			{
				if (LineCount == 0)
				{
					return string.Empty;
				}
				if (!HasSelection) 
				{
					return GetWholeString(CurrentLine);
				}
				else
				{
					return GetString(editSelection);
				}
			}
		}

		/// <summary>
		/// Gets the length of the selected text.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The length of the selected text."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public int SelectionLength
		{
			get
			{
				return SelectedText.Length;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the selection when 
		/// focus is lost.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether to show selection when focus is lost."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool HideSelection
		{
			get
			{
				return editSettings.HideSelection;
			}
			set
			{
				editSettings.HideSelection = value;
			}
		}

		/// <summary>
		/// Gets a value indicating whether there is any single-line tag 
		/// presented.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether there is any single-line tag presented."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool HasSingleLineTag
		{
			get
			{
				return editSettings.HasSingleLineTag;
			}
		}

		/// <summary>
		/// Gets a value indicating whether there is any multiline tag presented.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether there is any multiline tag presented."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool HasMultiLineTag
		{
			get
			{
				return editSettings.HasMultiLineTag;
			}
		}

		/// <summary>
		/// Gets or sets the description of the file type.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The description of the file type."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public string FileDescription
		{
			get
			{
				return editSettings.FileDescription;
			}
		}

		/// <summary>
		/// Gets or sets the extension of the file type.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The extension of the file type."),
		Browsable(false),
		DefaultValue("")
		]
		public string FileExtension
		{
			get
			{
				return editSettings.FileExtension;
			}
			set
			{
				editSettings.FileExtension = value;
			}
		}

		/// <summary>
		/// Gets or sets the extension of the file type.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The extension of the file type (obselete, please use FileExtension instead)"),
		Browsable(false),
		DefaultValue("")
		]
		public string FileSuffix
		{
			get
			{
				return editSettings.FileExtension;
			}
			set
			{
				editSettings.FileExtension = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether syntax coloring is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether syntax coloring is enabled."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool SyntaxColoringEnabled
		{
			get
			{
				return editSettings.SyntaxColoringEnabled;
			}
			set
			{
				if (editSettings.SyntaxColoringEnabled != value)
				{
					editSettings.SyntaxColoringEnabled = value;
					Redraw();
					OnSyntaxColoringEnabledChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether outlining is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether outlining is enabled."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool OutliningEnabled
		{
			get
			{
				return editSettings.OutliningEnabled;
			}
			set
			{
				if (editSettings.OutliningEnabled != value)
				{
					editSettings.OutliningEnabled = value;
					if (!editSettings.OutliningEnabled)
					{
						editSettings.SelectionMarginVisible = false;
						editSettings.AutomaticOutliningEnabled = false;
					}
					else
					{
						editSettings.SelectionMarginVisible = true;
						WordWrap = false;
					}
					UpdateAll();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether automatic outlining is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether automatic outlining is enabled."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool AutomaticOutliningEnabled
		{
			get
			{
				return editSettings.AutomaticOutliningEnabled;
			}
			set
			{
//				if (editSettings.AutomaticOutliningEnabled != value)
//				{
//					editSettings.AutomaticOutliningEnabled = value;
//					if (editSettings.AutomaticOutliningEnabled)
//					{
//						editSettings.OutliningEnabled = true;
//					}
//					UpdateAll();
//				}
			}
		}

		/// <summary>
		/// Gets or sets the tag that starts line comments.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The tag that starts line comments."),
		Browsable(false), 
		DefaultValue("")
		]
		public string LineComment
		{
			get
			{
				return editSettings.LineComment;
			}
			set
			{
				editSettings.LineComment = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the current file has  
		/// been modified.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether the current file has been modified."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool Modified
		{
			get
			{
				return bModified;
			}
			set
			{
				if (bModified != value)
				{
					bModified = value;
					OnModifiedChanged(new EventArgs());
				}
				if (bModified)
				{
					OnFileModified(new EventArgs());
					OnTextChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the name of the currently opened file.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The name of the currently opened file."),
		Browsable(false), 
		DefaultValue(null)
		]
		public string CurrentFile
		{
			get
			{
				return editCurrentFile;
			}
			set
			{
				string prevFile = editCurrentFile;
				if (editCurrentFile != value)
				{
					editCurrentFile = value;
					if (FileNameVisible)
					{
						SetPanelTextInternal(0, value);
					}
					else
					{
						SetPanelTextInternal(0, string.Empty);
					}
					if (value == null)
					{
						SetPanelTextInternal(1, GetResourceString("StatusLine"));
						SetPanelTextInternal(2, GetResourceString("StatusColumn"));
						SetPanelTextInternal(3, GetResourceString("StatusChar"));
					}
					else
					{
						SetPanelTextInternal(1, GetResourceString("StatusLine") + " 1");
						SetPanelTextInternal(2, GetResourceString("StatusColumn") + " 1");
						SetPanelTextInternal(3, GetResourceString("StatusChar") + " 1");
					}
					OnFileNameChanged(new EventArgs());
				}
				if ((((prevFile == null) || (prevFile == string.Empty))
					&& (editCurrentFile != prevFile)) ||
					(((editCurrentFile == null) || (editCurrentFile == string.Empty))
					&& (editCurrentFile != prevFile)))
				{
					OnHasContentChanged(new EventArgs());
					OnHasWorkingFileChanged(new EventArgs());
					OnUpdateableChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether whether there is any text content
		/// in the edit control.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether there is any text content in the edit control."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool HasContent
		{
			get
			{
				return (CurrentFile != null);
			}
		}

		/// <summary>
		/// Gets a value indicating whether a working file is present (obselete, 
		/// please use HasContent instead).
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether a working file is present (obselete, please use HasContent instead)."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool HasWorkingFile
		{
			get
			{
				return HasContent;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the text context in the edit control 
		/// is updateable.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether the text content in the edit control is updateable."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool Updateable
		{
			get
			{
				return HasContent && (!ReadOnly);
			}
		}

		/// <summary>
		/// Gets or sets the width of the user margin.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The width of the user margin."),
		Browsable(false), 
		DefaultValue(50)
		]
		public int UserMarginWidth
		{
			get
			{
				return editSettings.UserMarginWidth;
			}
			set
			{
				editSettings.UserMarginWidth = value;
			}
		}

		/// <summary>
		/// Gets or sets the foreground color of the user margin.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The foreground color of the user margin."),
		Browsable(false),
		DefaultValue(typeof(System.Drawing.Color), "Black")
		]
		public Color UserMarginForeColor
		{
			get
			{
				return editSettings.UserMarginForeColor;
			}
			set
			{
				editSettings.UserMarginForeColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color of the user margin.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The background color of the user margin."),
		Browsable(false),
		DefaultValue(typeof(System.Drawing.Color), "White")
		]
		public Color UserMarginBackColor
		{
			get
			{
				return editSettings.UserMarginBackColor;
			}
			set
			{
				editSettings.UserMarginBackColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the inserting mode.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The inserting mode."),
		Browsable(false), 
		DefaultValue(true)
		]
		public bool InsertMode
		{
			get
			{
				return bInsertMode;
			}
			set
			{
				if (bInsertMode != value)
				{
					bInsertMode = value;
					UpdateCaretSize();
					OnInsertModeChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets the current column.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The current column."),
		Browsable(false), 
		DefaultValue(1)
		]
		public int CurrentColumn
		{
			get
			{
				return GetColumnFromChar(CurrentLine, CurrentChar);
			}
			set
			{
				CurrentChar = GetCharFromColumn(CurrentLine, value);
			}
		}

		/// <summary>
		/// Gets a value indicating whether Paste can be processed.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether Paste can be processed."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool CanPaste
		{
			get
			{
				if (!Updateable)
				{
					return false;
				}
				else
				{
					IDataObject data = Clipboard.GetDataObject();
					// If the data is text, then enable the menu.
					if (data.GetDataPresent(DataFormats.Text)
						|| data.GetDataPresent(DataFormats.UnicodeText))
					{
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether there is any undoable action.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether there is any undoable action."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool CanUndo
		{
			get
			{
				return editUndoRedo.CanUndo;
			}
		}

		/// <summary>
		/// Gets a value indicating whether there is any redoable action.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether there is any redoable action."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool CanRedo
		{
			get
			{
				return editUndoRedo.CanRedo;
			}
		}

		/// <summary>
		/// Gets the maximum number of characters among lines.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The maximum number of characters among lines."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public int MaxCharCount
		{
			get
			{
				int maxChar = 0;
				int lnLast = LineCount;
				for (int ln = 1; ln <= lnLast; ln++)
				{
					maxChar = Math.Max(maxChar, GetCharCount(ln));
				}
				return maxChar;
			}
		}

		/// <summary>
		/// Gets the array of line strings.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The array of line strings."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public string[] Lines
		{
			get
			{
				string[] strArray = new string [LineCount];
				int lnLast = LineCount;
				for (int i = 1; i <= lnLast; i++)
				{
					strArray[i-1] = GetString(i);
				}
				return strArray;
			}
		}

		/// <summary>
		/// Gets a value indicating whether to adjust the control size 
		/// when the font is changed.  It is always false now.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("A value indicating whether to adjust the control size with font size."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public bool AutoSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether multiline is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether multiline is enabled."),
		Browsable(false),
		DefaultValue(true)
		]
		public bool Multiline
		{
			get
			{
				return editSettings.Multiline;
			}
			set
			{
				if (editSettings.Multiline != value)
				{
					editSettings.Multiline = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the ContextChoice feature 
		/// is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether the ContextChoice feature is enabled."),
		Browsable(false),
		DefaultValue(true)
		]
		public bool ContextChoiceEnabled
		{
			get
			{
				return editSettings.ContextChoiceEnabled;
			}
			set
			{
				if (editSettings.ContextChoiceEnabled != value)
				{
					editSettings.ContextChoiceEnabled = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the ContextPrompt feature 
		/// is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether the ContextPrompt feature is enabled."),
		Browsable(false),
		DefaultValue(true)
		]
		public bool ContextPromptEnabled
		{
			get
			{
				return editSettings.ContextPromptEnabled;
			}
			set
			{
				if (editSettings.ContextPromptEnabled != value)
				{
					editSettings.ContextPromptEnabled = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the ContextTooltip feature 
		/// is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether the ContextTooltip feature is enabled."),
		Browsable(false),
		DefaultValue(true)
		]
		public bool ContextTooltipEnabled
		{
			get
			{
				return editSettings.ContextTooltipEnabled;
			}
			set
			{
				if (editSettings.ContextTooltipEnabled != value)
				{
					editSettings.ContextTooltipEnabled = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the ContextChanged event 
		/// is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether the ContextChanged event is enabled."),
		Browsable(false), 
		DefaultValue(true)
		]
		public bool ContentChangedEventEnabled
		{
			get
			{
				return editSettings.ContentChangedEventEnabled;
			}
			set
			{
				if (editSettings.ContentChangedEventEnabled != value)
				{
					editSettings.ContentChangedEventEnabled = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether virtual space is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether virtual space is enabled."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool VirtualSpace
		{
			get
			{
				return (editSettings.VirtualSpace == "1");
			}
			set
			{
				//editSettings.VirtualSpace = value ? "1" : "0";
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether word wrap is enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether word wrap is enabled."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool WordWrap
		{
			get
			{
				return (editSettings.WordWrap == "1");
			}
			set
			{
				editSettings.WordWrap = value ? "1" : "0";
				HScrollBarVisible = !value;
				if (value)
				{
					OutliningEnabled = false;
				}
			}
		}

		/// <summary>
		/// Gets the line height in pixels.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The line height in pixels."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		Browsable(false)
		]
		public int LineHeight
		{
			get
			{
				return Font.Height + editLineSpace;
			}
		}

		/// <summary>
		/// Gets or sets the value of the space between lines.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The space between lines in pixels."),
		Browsable(false), 
		DefaultValue(1)
		]
		public int LineSpace
		{
			get
			{
				return editLineSpace;
			}
			set
			{
				editLineSpace = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to draw the grid lines 
		/// or not.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether to draw the grid lines or not."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool GridLinesVisible
		{
			get
			{
				return editSettings.GridLinesVisible;
			}
			set
			{
				if (editSettings.GridLinesVisible != value)
				{
					editSettings.GridLinesVisible = value;
					Redraw();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to draw the right margin 
		/// line or not.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether to draw the right margin line or not."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool RightMarginLineVisible
		{
			get
			{
				return editSettings.RightMarginLineVisible;
			}
			set
			{
				if (editSettings.RightMarginLineVisible != value)
				{
					editSettings.RightMarginLineVisible = value;
					Redraw();
				}
			}
		}

		/// <summary>
		/// Gets or sets the number of spaces for a tab character.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The number of spaces for a tab character."),
		Browsable(false), 
		DefaultValue(4)
		]
		public int TabSize
		{
			get
			{
				return Int32.Parse(editSettings.TabSize);
			}
			set
			{
				editSettings.TabSize = value.ToString();
			}
		}

		/// <summary>
		/// Gets or sets the number of spaces for indenting.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The number of spaces for indenting."),
		Browsable(false), 
		DefaultValue(4)
		]
		public int IndentSize
		{
			get
			{
				return Int32.Parse(editSettings.IndentSize);
			}
			set
			{
				editSettings.IndentSize = value.ToString();
			}
		}

		/// <summary>
		/// Gets or sets the type of indenting.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The type of the indenting."),
		Browsable(false), 
		DefaultValue(EditIndentType.Block)
		]
		public EditIndentType IndentType
		{
			get
			{
				return editSettings.IndentType;
			}
			set
			{
				editSettings.IndentType = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to display white spaces or 
		/// not.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether to display white spaces or not."),
		Browsable(false), 
		DefaultValue(false)
		]
		public bool WhiteSpaceVisible
		{
			get
			{
				return editSettings.WhiteSpaceVisible;
			}
			set
			{
				if (editSettings.WhiteSpaceVisible != value)
				{
					editSettings.WhiteSpaceVisible = value;
					Redraw();
				}
			}
		}

		/// <summary>
		/// Gets or sets the width of the caret.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The width of the caret."),
		Browsable(false), 
		DefaultValue(2)
		]
		public int CaretWidth
		{
			get
			{
				return editCaretWidth;
			}
			set
			{
				if (editCaretWidth != value)
				{
					editCaretWidth = value;
					UpdateCaretSize();
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the border line.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The color of the border line."),
		Browsable(false),
		DefaultValue(typeof(System.Drawing.Color), "Gray")
		]
		public Color BorderColor
		{
			get
			{
				return editSettings.BorderColor;
			}
			set
			{
				editSettings.BorderColor = value;
				Redraw();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to draw the horizontal 
		/// ScrollBar or not.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to draw the horizontal ScrollBar or not."),
		Browsable(false),
		DefaultValue(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool HScrollBarVisible
		{
			get
			{
				return editSettings.HScrollBarVisible;
			}
			set
			{
				if (editSettings.HScrollBarVisible != value)
				{
					editSettings.HScrollBarVisible = value;
					UpdateAll();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to draw the vertical 
		/// ScrollBar or not.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to draw the vertical ScrollBar or not."),
		Browsable(false),
		DefaultValue(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool VScrollBarVisible
		{
			get
			{
				return editSettings.VScrollBarVisible;
			}
			set
			{
				if (editSettings.VScrollBarVisible != value)
				{
					editSettings.VScrollBarVisible = value;
					editViewBottom.SplitterButtonVisible = value;
					UpdateAll();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to log ReplaceAll as a 
		/// composite action.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to log ReplaceAll as a composite action."),
		Browsable(false),
		DefaultValue(true),
		]
		public bool LogReplaceAllEnabled
		{
			get
			{
				return editSettings.LogReplaceAllEnabled;
			}
			set
			{
				if (editSettings.LogReplaceAllEnabled != value)
				{
					editSettings.LogReplaceAllEnabled = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating the encoding used for text.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines the encoding used for text."),
		Browsable(false),
		DefaultValue(typeof(System.Text.Encoding), "System.Text.UTF8Encoding"),
		]
		public Encoding TextEncoding
		{
			get
			{
				return editSettings.TextEncoding;
			}
			set
			{
				if (editSettings.TextEncoding != value)
				{
					editSettings.TextEncoding = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value determining whether to display brace matching.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to display brace matching."),
		Browsable(false),
		DefaultValue(false),
		]
		public bool BraceMatchingEnabled
		{
			get
			{
				return editSettings.BraceMatchingEnabled;
			}
			set
			{
				if (editSettings.BraceMatchingEnabled != value)
				{
					editSettings.BraceMatchingEnabled = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets a value determining whether to show the file name in 
		/// the status bar and print/print preview.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to show the file name in the status bar and print/print preview."),
		Browsable(false),
		DefaultValue(true),
		]
		public bool FileNameVisible
		{
			get
			{
				return editSettings.FileNameVisible;
			}
			set
			{
				if (editSettings.FileNameVisible != value)
				{
					editSettings.FileNameVisible = value;
					if (editSettings.FileNameVisible)
					{
						SetPanelTextInternal(0, editCurrentFile);
					}
					else
					{
						SetPanelTextInternal(0, string.Empty);
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the maximum line interval for brace matching.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("The maximum line interval for brace matching."),
		Browsable(false),
		DefaultValue(100),
		]
		public int MaxBraceMatchingLineInterval
		{
			get
			{
				return editSettings.MaxBraceMatchingLineInterval;
			}
			set
			{
				if (editSettings.MaxBraceMatchingLineInterval != value)
				{
					editSettings.MaxBraceMatchingLineInterval = value;
				}
			}
		}

		#endregion

		#region Public Browsable Properties
		/// <summary>
		/// Gets or sets whether FileDrop is supported when working with drag-drop.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines if a FileDrop type of drag-drop is allowed."),
		DefaultValue(true)
		]
		public bool AllowFileDrop
		{
			get{return bFileDropAllowed;}

			set{bFileDropAllowed = value;}
		}

		/// <summary>
		/// Gets or sets the border style of the viewport.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines the border style of the viewport."),
		DefaultValue(BorderStyle.FixedSingle)
		]
		public BorderStyle BorderStyle
		{
			get
			{
				return editBorderStyle;
			}
			set
			{
				if (editBorderStyle != value)
				{
					editBorderStyle = value;
					if (editBorderStyle == BorderStyle.None)
					{
						editBorderWidth = 0;
					}
					else if (editBorderStyle == BorderStyle.FixedSingle)
					{
						editBorderWidth = 1;
					}
					else
					{
						editBorderWidth = SystemInformation.Border3DSize.Width;
					}
					UpdateAll();
				}
			}
		}

		/// <summary>
		/// Gets or sets the font for text displayed in the edit control.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The font used to display text."),
		DefaultValue(typeof(System.Drawing.Font), "Courier New, 10pt")
		]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				editSettings.EditFont = value;
				base.Font = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the edit control will 
		/// receive drag-drop notifications.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether the edit control will receive drag-drop notifications"),
		DefaultValue(true)
		]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
				editViewTop.AllowDrop = value;
				editViewBottom.AllowDrop = value;
			}
		}

		/// <summary>
		/// Gets or sets the foreground color for text displayed in the edit 
		/// control.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The foreground color used to display text."),
		DefaultValue(typeof(System.Drawing.Color), "WindowText")
		]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				editSettings.SetColorGroupForeColor("Text", value);
				TextForeColor = value;
				base.ForeColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color for text displayed in the edit 
		/// control.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("The background color used to display text."),
		DefaultValue(typeof(System.Drawing.Color), "Window")
		]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				editSettings.SetColorGroupBackColor("Text", value);
				TextBackColor = value;
				base.BackColor = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to start with a blank 
		/// document or not.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to start with a blank document or not."),
		DefaultValue(false)
		]
		public bool StartWithNewFile
		{
			get
			{
				return editSettings.StartWithNewFile;
			}
			set
			{
				if (editSettings.StartWithNewFile != value)
				{
					editSettings.StartWithNewFile = value;
					if (!InDesignMode)
					{
						NewFile();
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the statusbar or not.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to show the statusbar or not."),
		DefaultValue(true)
		]
		public bool StatusBarVisible
		{
			get
			{
				return editSettings.StatusBarVisible;
			}
			set
			{
				if (editSettings.StatusBarVisible != value)
				{
					editSettings.StatusBarVisible = value;
					statusBarHeight = StatusBarVisible ? editStatusBar.Height : 0;
					editStatusBar.Visible = StatusBarVisible;
					UpdateLayout();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the indicator 
		/// margin or not.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to show the indicator margin or not."),
		DefaultValue(true)
		]
		public bool IndicatorMarginVisible
		{
			get
			{
				return editSettings.IndicatorMarginVisible;
			}
			set
			{
				if (editSettings.IndicatorMarginVisible != value)
				{
					editSettings.IndicatorMarginVisible = value;
					UpdateAll();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the line number 
		/// margin or not.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to show the line number margin or not."),
		DefaultValue(true)
		]
		public bool LineNumberMarginVisible
		{
			get
			{
				return editSettings.LineNumberMarginVisible;
			}
			set
			{
				if (editSettings.LineNumberMarginVisible != value)
				{
					editSettings.LineNumberMarginVisible = value;
					UpdateAll();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the selection 
		/// margin or not.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to show the selection margin or not."),
		DefaultValue(false)
		]
		public bool SelectionMarginVisible
		{
			get
			{
				return editSettings.SelectionMarginVisible;
			}
			set
			{
				if (editSettings.SelectionMarginVisible != value)
				{
					editSettings.SelectionMarginVisible = value;
					UpdateAll();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating which scroll bars should appear.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines which scroll bars should appear."),
		DefaultValue(ScrollBars.Both)
		]
		public ScrollBars ScrollBars 
		{
			get
			{
				if (HScrollBarVisible && VScrollBarVisible)
				{
					return ScrollBars.Both;
				}
				else if (HScrollBarVisible)
				{
					return ScrollBars.Horizontal;
				}
				else if (VScrollBarVisible)
				{
					return ScrollBars.Vertical;
				}
				else
				{
					return ScrollBars.None;
				}
			}
			set
			{
				if (value == ScrollBars.Both)
				{
					HScrollBarVisible = true;
					VScrollBarVisible = true;
				}
				else if (value == ScrollBars.Horizontal)
				{
					HScrollBarVisible = true;
					VScrollBarVisible = false;
				}
				else if (value == ScrollBars.Vertical)
				{
					HScrollBarVisible = false;
					VScrollBarVisible = true;
				}
				else
				{
					HScrollBarVisible = false;
					VScrollBarVisible = false;
				}
				UpdateAll();
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the context menu is enabled. 
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether the context menu is enabled."),
		DefaultValue(true)
		]
		public bool ContextMenuVisible
		{
			get
			{
				return editSettings.ContextMenuVisible;
			}
			set
			{
				editSettings.ContextMenuVisible = value;
				EnableContextMenu(value);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the user margin 
		/// or not.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether to show the user margin or not."),
		DefaultValue(false)
		]
		public bool UserMarginVisible
		{
			get
			{
				return editSettings.UserMarginVisible;
			}
			set
			{
				if (editSettings.UserMarginVisible != value)
				{
					editSettings.UserMarginVisible = value;
					UpdateAll();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating the readonly status.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("Determines whether the EditControl object is readonly."),
		DefaultValue(false)
		]
		public bool ReadOnly
		{
			get
			{
				return editSettings.ReadOnly;
			}
			set
			{
				if (editSettings.ReadOnly != value)
				{
					editSettings.ReadOnly = value;
					OnReadOnlyChanged(new EventArgs());
					OnUpdateableChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to insert spaces instead of 
		/// tabs when indenting.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether to insert spaces instead of tabs when indenting."),
		DefaultValue(true)
		]
		public bool KeepTabs
		{
			get
			{
				return editSettings.KeepTabs;
			}
			set
			{
				editSettings.KeepTabs = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the file for syntax coloring settings.
		/// </summary>
		[
		Category("Essential Edit"), 
		Description("The name of the file for syntax coloring settings."),
		DefaultValue("")
		]
		public string SettingFile
		{
			get
			{
				return editSettings.SettingFile;
			}
			set
			{
				string oldSettingFile = editSettings.SettingFile;
				if (((value == null) || (value == string.Empty)) 
					&& (value != oldSettingFile))
				{
					//editSettings.ResetSettings();
					SyntaxColoringEnabled = false;
					InvalidateColoring();
				}
				else if (value != oldSettingFile)
				{
					//editSettings.ResetSettings();
					editSettings.SettingFile = value;
					if (!InDesignMode)
					{
						if (value.StartsWith("*"))
						{
							UseBuiltInSettingFile(value.Substring(1, 
								value.Length - 1));
						}
						else
						{
							ReadSettingsFromFile(value);
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to accept the Tab key.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether to accept the Tab key."),
		DefaultValue(true)
		]
		public bool AcceptsTab
		{
			get
			{
				return editSettings.AcceptsTab;
			}
			set
			{
				if (editSettings.AcceptsTab != value)
				{
					editSettings.AcceptsTab = value;
					OnAcceptsTabChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to accept the Return key.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether to accept the Return key."),
		DefaultValue(true)
		]
		public bool AcceptsReturn
		{
			get
			{
				return editSettings.AcceptsReturn;
			}
			set
			{
				if (editSettings.AcceptsReturn != value)
				{
					editSettings.AcceptsReturn = value;
					OnAcceptsReturnChanged(new EventArgs());
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to modify the case of 
		/// characters as they are typed.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Determines whether to modify the case of characters as they are typed."),
		DefaultValue(CharacterCasing.Normal)
		]
		public CharacterCasing CharacterCasing
		{
			get
			{
				return editSettings.CharacterCasing;
			}
			set
			{
				editSettings.CharacterCasing = value;
			}
		}

		#endregion

		#region Public Events

		/// <summary>
		/// Event for the file name change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the file name is changed.")
		]
		public event EventHandler FileNameChanged;

		/// <summary>
		/// Event for the file content change (obselete, please use TextChanged 
		/// instead).
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the file content is changed (obselete, please use TextChanged instead)."),
		Browsable(false)
		]
		public event EventHandler FileModified;

		/// <summary>
		/// Event for the Modified status change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of Modified property is changed.")
		]
		public event EventHandler ModifiedChanged;

		/// <summary>
		/// Event for the AcceptsTab status change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of AcceptsTab property is changed.")
		]
		public event EventHandler AcceptsTabChanged;

		/// <summary>
		/// Event for the AcceptsReturn status change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of AcceptsReturn property is changed.")
		]
		public event EventHandler AcceptsReturnChanged;

		/// <summary>
		/// Event for the CanPaste status change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of CanPaste property is changed.")
		]
		public event EventHandler CanPasteChanged;

		/// <summary>
		/// Event for the CanUndo status change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of CanUndo property is changed.")
		]
		public event EventHandler CanUndoChanged;

		/// <summary>
		/// Event for the CanRedo status change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of CanRedo property is changed.")
		]
		public event EventHandler CanRedoChanged;

		/// <summary>
		/// Event for the InsertMode change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of InsertMode property is changed.")
		]
		public event EventHandler InsertModeChanged;

		/// <summary>
		/// Event for the Readonly mode change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of ReadOnly property is changed.")
		]
		public event EventHandler ReadOnlyChanged;

		/// <summary>
		/// Event for the HasContent status change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of HasContent property is changed.")
		]
		public event EventHandler HasContentChanged;

		/// <summary>
		/// Event for the HasWorkingFile status change (obselete, please use
		/// HasContentChanged instead). 
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of HasWorkingFile property is changed (obselete, please use HasContentChanged instead)."),
		Browsable(false)
		]
		public event EventHandler HasWorkingFileChanged;

		/// <summary>
		/// Event for the Updateable status change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of Updateable property is changed.")
		]
		public event EventHandler UpdateableChanged;

		/// <summary>
		/// Event for the SyntaxColoringEnabled status change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of SyntaxColoringEnabled property is changed.")
		]
		public event EventHandler SyntaxColoringEnabledChanged;

		/// <summary>
		/// Event for the current line change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of CurrentLine property is changed."),
		]
		public event EventHandler CurrentLineChanged;

		/// <summary>
		/// Event for the current char change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of CurrentChar property is changed."),
		]
		public event EventHandler CurrentCharChanged;

		/// <summary>
		/// Event for the current column change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the value of CurrentColumn property is changed."),
		]
		public event EventHandler CurrentColumnChanged;

		/// <summary>
		/// Event for user margin painting.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Occurs when the user margin needs repainting."),
		]
		public event UserMarginPaintEventHandler UserMarginPaint;

		/// <summary>
		/// Event for the context tooltip popup.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Occurs when the ContextTooltip window is to be popped up."),
		]
		public event ContextTooltipPopupEventHandler ContextTooltipPopup;

		/// <summary>
		/// Event for the context choice popup.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Occurs when the ContextChoice window is to be popped up."),
		]
		public event ContextChoicePopupEventHandler ContextChoicePopup;

		/// <summary>
		/// Event for the context prompt popup.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Occurs when the ContextPrompt window is to be popped up."),
		]
		public event ContextPromptPopupEventHandler ContextPromptPopup;

		/// <summary>
		/// Event for the content change. This event is active only after it is 
		/// enabled.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the current content is changed. This event is active only after it is enabled."),
		]
		public event ContentChangedEventHandler ContentChanged;

		/// <summary>
		/// Event for the outlining collapsed state change.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Event fired when the an outlining is expanded/collapsed."),
		]
		public event OutliningCollapsedChangedEventHandler OutliningCollapsedChanged;

		/// <summary>
		/// Event for the double-click select.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Occurs when double-clicked text needs to be selected."),
		]
		public event DoubleClickSelectEventHandler DoubleClickSelect;

		/// <summary>
		/// Event for the line information update.
		/// </summary>
		[
		Category("Essential Edit"),
		Description("Occurs when the line information is updated."),
		]
		public event LineInfoUpdateEventHandler LineInfoUpdate;

		#endregion

		#region Helper Methods for Event Rising

		/// <summary>
		/// Raises the FileNameChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnFileNameChanged(EventArgs e)
		{
			if (FileNameChanged != null)
			{
				FileNameChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the FileModified event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnFileModified(EventArgs e)
		{
			if (FileModified != null)
			{
				FileModified(this, e);
			}
		}

		/// <summary>
		/// Raises the ModifiedChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnModifiedChanged(EventArgs e)
		{
			if (ModifiedChanged != null)
			{
				ModifiedChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the AcceptsTabChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnAcceptsTabChanged(EventArgs e)
		{
			if (AcceptsTabChanged != null)
			{
				AcceptsTabChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the AcceptsReturnChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnAcceptsReturnChanged(EventArgs e)
		{
			if (AcceptsReturnChanged != null)
			{
				AcceptsReturnChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the CanPasteChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCanPasteChanged(EventArgs e)
		{
			if (CanPasteChanged != null)
			{
				CanPasteChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the CanUndoChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCanUndoChanged(EventArgs e)
		{
			if (CanUndoChanged != null)
			{
				CanUndoChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the CanRedoChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCanRedoChanged(EventArgs e)
		{
			if (CanRedoChanged != null)
			{
				CanRedoChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the InsertModeChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnInsertModeChanged(EventArgs e)
		{
			if (InsertModeChanged != null)
			{
				InsertModeChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the ReadOnlyChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnReadOnlyChanged(EventArgs e)
		{
			if (ReadOnlyChanged != null)
			{
				ReadOnlyChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the OnHasContentChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnHasContentChanged(EventArgs e)
		{
			if (HasContentChanged != null)
			{
				HasContentChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the HasWorkingFileChanged event (obselete, please use 
		/// OnHasContentChanged instead).
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnHasWorkingFileChanged(EventArgs e)
		{
			if (HasWorkingFileChanged != null)
			{
				HasWorkingFileChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the UpdateableChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnUpdateableChanged(EventArgs e)
		{
			if (UpdateableChanged != null)
			{
				UpdateableChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the SyntaxColoringEnabledChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnSyntaxColoringEnabledChanged(EventArgs e)
		{
			if (SyntaxColoringEnabledChanged != null)
			{
				SyntaxColoringEnabledChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the UserMarginPaint event.
		/// </summary>
		/// <param name="e">A UserMarginPaintEventArgs that contains the event 
		/// data.</param>
		internal void RaiseUserMarginPaint(UserMarginPaintEventArgs e)
		{
			OnUserMarginPaint(e);
		}

		/// <summary>
		/// Raises the UserMarginPaint event.
		/// </summary>
		/// <param name="e">A UserMarginPaintEventArgs that contains the event 
		/// data.</param>
		protected virtual void OnUserMarginPaint(UserMarginPaintEventArgs e)
		{
			if (UserMarginPaint != null)
			{
				UserMarginPaint(this, e);
			}
		}

		/// <summary>
		/// Raises the ContextTooltipPopup event.
		/// </summary>
		/// <param name="e">A ContextTooltipPopupEventArgs that contains the 
		/// event data.</param>
		internal void RaiseContextTooltipPopup(ContextTooltipPopupEventArgs e)
		{
			OnContextTooltipPopup(e);
		}

		/// <summary>
		/// Raises the ContextTooltipPopup event.
		/// </summary>
		/// <param name="e">A ContextTooltipPopupEventArgs that contains the 
		/// event data.</param>
		protected virtual void OnContextTooltipPopup(ContextTooltipPopupEventArgs e)
		{
			if (ContextTooltipPopup != null)
			{
				ContextTooltipPopup(this, e);
			}
		}

		/// <summary>
		/// Raises the ContextChoicePopup event.
		/// </summary>
		/// <param name="e">A ContextChoicePopupEventArgs that contains the 
		/// event data.</param>
		internal void RaiseContextChoicePopup(ContextChoicePopupEventArgs e)
		{
			OnContextChoicePopup(e);
		}

		/// <summary>
		/// Raises the ContextChoicePopup event.
		/// </summary>
		/// <param name="e">A ContextChoicePopupEventArgs that contains the 
		/// event data.</param>
		protected virtual void OnContextChoicePopup(ContextChoicePopupEventArgs e)
		{
			if (ContextChoicePopup != null)
			{
				ContextChoicePopup(this, e);
			}
		}

		/// <summary>
		/// Raises the ContextPromptPopup event.
		/// </summary>
		/// <param name="e">A ContextPromptPopupEventArgs that contains the 
		/// event data.</param>
		internal void RaiseContextPromptPopup(ContextPromptPopupEventArgs e)
		{
			OnContextPromptPopup(e);
		}

		/// <summary>
		/// Raises the ContextPromptPopup event.
		/// </summary>
		/// <param name="e">A ContextPromptPopupEventArgs that contains the 
		/// event data.</param>
		protected virtual void OnContextPromptPopup(ContextPromptPopupEventArgs e)
		{
			if (ContextPromptPopup != null)
			{
				ContextPromptPopup(this, e);
			}
		}

		/// <summary>
		/// Raises the ContentChanged event.
		/// </summary>
		/// <param name="e">A ContentChangedEventArgs that contains the event 
		/// data.</param>
		protected virtual void OnContentChanged(ContentChangedEventArgs e)
		{
			if (ContentChanged != null)
			{
				ContentChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the CurrentLineChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCurrentLineChanged(EventArgs e)
		{
			if (CurrentLineChanged != null)
			{
				CurrentLineChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the CurrentCharChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCurrentCharChanged(EventArgs e)
		{
			if (CurrentCharChanged != null)
			{
				CurrentCharChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the CurrentColumnChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data.</param>
		protected virtual void OnCurrentColumnChanged(EventArgs e)
		{
			if (CurrentColumnChanged != null)
			{
				CurrentColumnChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the OutliningCollapsedChanged event.
		/// </summary>
		/// <param name="e">An OutliningCollapsedChangedEventArgs that contains 
		/// the event data.</param>
		protected virtual void OnOutliningCollapsedChanged(OutliningCollapsedChangedEventArgs e)
		{
			if (OutliningCollapsedChanged != null)
			{
				OutliningCollapsedChanged(this, e);
			}
		}

		/// <summary>
		/// Raises the DoubleClickSelect event.
		/// </summary>
		/// <param name="e">A DoubleClickSelectEventArgs that contains the event 
		/// data.</param>
		internal void RaiseDoubleClickSelect(DoubleClickSelectEventArgs e)
		{
			OnDoubleClickSelect(e);
		}

		/// <summary>
		/// Raises the DoubleClickSelect event.
		/// </summary>
		/// <param name="e">A DoubleClickSelectEventArgs that contains the event 
		/// data.</param>
		protected virtual void OnDoubleClickSelect(DoubleClickSelectEventArgs e)
		{
			if (DoubleClickSelect != null)
			{
				DoubleClickSelect(this, e);
			}
		}

		/// <summary>
		/// Raises the LineInfoUpdate event.
		/// </summary>
		/// <param name="e">A LineInfoUpdateEventArgs that contains the event 
		/// data.</param>
		internal void RaiseLineInfoUpdate(LineInfoUpdateEventArgs e)
		{
			OnLineInfoUpdate(e);
		}

		/// <summary>
		/// Raises the LineInfoUpdate event.
		/// </summary>
		/// <param name="e">A LineInfoUpdateEventArgs that contains the event 
		/// data.</param>
		protected virtual void OnLineInfoUpdate(LineInfoUpdateEventArgs e)
		{
			if (LineInfoUpdate != null)
			{
				LineInfoUpdate(this, e);
			}
		}

		#endregion
	}
}
