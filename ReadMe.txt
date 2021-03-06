Yata - yet another 2da editor for Neverwinter Nights 2

This app does not write to the Registry, nor does it write any files that you
don't tell it to. It can write 2da files. Various settings for Yata can be
changed in the Settings.Cfg textfile.

2021 june 4
kevL's
ver 4.1.2.1


File
- Open ... @ folder (presets for the Open ... dialog - appears only if at least
                     one "dirpreset=" has been set in Settings.Cfg - see the
                     Settings.Cfg file options below)
- Open ... : Ctrl+o
- Reload   : Ctrl+r
- Recent (lists recently opened files - appears only if "recent=" has been set
          in Settings.Cfg and at least one valid filepath exists in the list -
          see the Settings.Cfg file options below)

- Readonly : F12 (toggles the currently focused table's Readonly flag)

- Save        : Ctrl+s
- Save As ... : Ctrl+e
- Save All    : Ctrl+a

- Close : F4
- Close all (this is a multi-tabbed application)

- Quit : Ctrl+q


Edit
- Undo : Ctrl+z
- Redo : Ctrl+y

- Find      : Ctrl+f (focuses the Search box)
- Find next : F3 (key Shift to find previous)

- Goto             : Ctrl+g (focuses the Goto box)
- Goto loadchanged : Ctrl+n (key Shift to goto previous. See Appendix A: note on
                             Load)

- Copy cell  : Ctrl+c (copies a single cell if only 1 cell is selected)
- Paste cell : Ctrl+v (pastes a single cell if only 1 cell is selected)

- ****                       : replaces all selected cells with "****"
- Lowercase                  : converts all selected cells to lowercase
- Uppercase                  : converts all selected cells to uppercase
- Apply text to selected ... : opens a dialog that replaces all selected cells

- Copy row(s)  : Shift+Ctrl+c (copies a selected row or range of rows)
- Paste row(s) : Shift+Ctrl+v (pastes a copied row or range of copied rows)

- Create row(s) : F2 (opens a dialog that inserts 1+ rows at a given id with
                      options to use (a) a selected row or (b) the first of any
                      currently copied rows or (c) "****" to fill the fields)


Editcol
- create head ...  : creates a col. This clears Undo/Redo, etc. The col will be
                     created at the position of a selected col, shifting the
                     table to the right; press [Esc] to deselect any selected
                     col to create a col at the far right of the table
- delete head ...  : deletes a selected col. This clears Undo/Redo, etc.
- relabel head ... : relabels the head of a selected col

- copy col  : copies the cells of a selected col
- paste col : pastes copied cells into a selected col


Goto box (type a row ID and press Enter)


Search box (type a string to search for and press Enter or F3. Note that Enter
            keeps the searchbox focused while F3 switches focus to the table)
Search options dropdown (substring or wholeword)


Clipboard
- Export copied row(s) : F9  (exports the internal copy-list to the clipboard)
- Import copied row(s) : F10 (imports any clipboard text to the internal
                              copy-list WARNING: No validity test is done on the
                              clipboard text; importing assumes that the text on
                              your clipboard contains valid 2da-row data)

- Open clip editor : F11 (accesses the Windows Clipboard for viewing/editing -
                          only clips in text format are displayed or handled)


2da Ops
- Order row ids  : Ctrl+d (auto-orders the IDs of the currently displayed 2da)
- Test row order : Ctrl+t (tests the row-order of the currently displayed 2da)

- Autosize cols : Ctrl+i (recalculates the display-width of all cols)
- Recolor rows  : Ctrl+l (tables show with alternating row-colors. When rows are
                  inserted/deleted (or sorted) the colors go out of sync to aid
                  understanding of what just happened. The "recolor rows"
                  operation makes row-colors alternate as usual)

- Freeze 1st col : F5 (causes the first col after the ID-col to remain
                   stationary)
- Freeze 2nd col : F6 (causes the first and second cols after the ID-col to
                   remain stationary) One of those two cols typically contains
                   the row's "label" - so by freezing it you can scroll to the
                   right and still read what it is.

- PropertyPanel          : F7 (toggles the PropertyPanel on/off)
- PropertyPanel location : F8 (key Shift to reverse direction. Cycles the
                           panel's location through the four corners of the
                           table)

- External diff/merger : Ctrl+m (starts an external diff program with 2 diffed
                         files open. See Appendix M: WinMerge)

- Clear undo/redo (the undo/redo stacks can stack up over a long session; if so
                   clear this when you're running low on RAM - note that each
                   table has its own undo/redo stacks)


TalkTable
- Path to ... (opens a file-browser to select a Dialog.Tlk file. THE PATH WILL
               NOT BE SAVED. But see the Settings.Cfg "dialog=" option. A check
               will appear if the file is loaded successfully - clicking this
               operation when checked clears loaded talktable entries. Note that
               if the file changes on disk - Yata does not write to the
               talktable, it only reads it - then it needs to be re-pathed for
               those changes to be reflected on the statusbar or in a selected
               cell's "strref" dialog)
- Path to custom ... (opens a file-browser to select a custom talktable - see
                      Path to ... and the Settings.Cfg "dialogalt=" option)


Paths (appears only when a 2da called "crafting", "spells", or "feat" is loaded
       - see Appendix E: how to use Info paths)


Font
- Font ... be patient (pick a font, any valid TrueType font on your system, to
                       display the table-data. THE CHOICE OF FONT WILL NOT BE
                       SAVED. But note that along the bottom of the dialog is a
                       .NET font-string that can be copy/pasted to Settings.Cfg
                       for any of the "font=" variables. See the section below
                       about the Settings.Cfg file)
- Load default font (sets the table-font to Yata's hardcoded default font)


Help
- ReadMe.txt : F1 (opens this document in a text-editor)
- About (displays the version+config of the executable)



KEYBOARD:
- w/ only 1 cell selected
Home                      - selects cell at start of the row
End                       - selects cell at the end of the row
PageUp                    - selects cell a page above
PageDown                  - selects cell a page below
Ctrl+Home                 - selects first cell in the table
Ctrl+End                  - selects first cell in the last row of the table
Left/Right/Up/Down arrows - selects next cell in the direction
Shift+Left                - selects cell left by visible width
Shift+Right               - selects cell right by visible width

- w/out 1 cell selected (or more than one cell is selected)
Home                      - scrolls table all the way left
End                       - scrolls table all the way right
PageUp                    - scrolls table a page up
PageDown                  - scrolls table a page down
Ctrl+Home                 - scrolls table to top
Ctrl+End                  - scrolls table to bottom
Left/Right/Up/Down arrows - scrolls table in the direction
Shift+Left                - scrolls table left by visible width
Shift+Right               - scrolls table right by visible width

- w/ row selected
Home              - scrolls table all the way left
End               - scrolls table all the way right
PageUp            - selects the row a page above
PageDown          - selects the row a page below
Ctrl+Home         - selects the top row
Ctrl+End          - selects the bottom row
Up/Down arrows    - selects the row in the direction
Left/Right arrows - scrolls table in the direction
Shift+Left        - scrolls table left by visible width
Shift+Right       - scrolls table right by visible width

Escape - deselects any selected cells/rows/cols if not currently editing a cell
       - if editing a cell it escapes the edit without changing the field
       - if the tabcontrol is focused it switches focus to the table

Enter - starts editing a cell if the table has focus and only one cell is
        currently selected
      - commits an edit if the editor box has focus
      - performs search if the Search box or Search Options dropdown has focus
      - performs goto if the Goto box has focus

Delete - when a row is selected (as indicated with a green field at the far left
         of a row) the Delete-key deletes that row. Use Shift+LMB on another
         row, above or below the selected row, to select a range of rows to
         delete. If all rows of a table are deleted a single default row will be
         created.
       - if a row is not selected and only 1 cell is selected, Delete clears its
         celltext ("****")

Space - focuses the table and selects the first cell. If cell(s) are already
        selected the table will scroll to ensure that the first selected cell is
        visible.

MOUSE:
wheel - scrolls up/down if the vertical scrollbar is visible and either of
        (a) the horizontal bar is disabled or (b) Ctrl is not pressed
      - scrolls left/right if the horizontal scrollbar is visible and either of
        (a) the vertical bar is disabled or (b) Ctrl is pressed
Shift+wheel - scrolls the table by its visible height/width if applicable; the
              Ctrl-key is also respected

click on the colheads or rowheads
LMB            - selects the col or row
LMB+Ctrl       - adds or subtracts a col/row from the currently selected cells
LMB+Shift      - selects a range of cols/rows if a col/row is already selected
LMB+Ctrl+Shift - you get the idea ...

click on the colheads
LMB       - click-drag col-boundary to re-width a col (The text of a colhead
            ought appear slightly grayed if its col has been user-sized.)
RMB       - click a col-boundary to auto-width a col (Note that frozen cols
            can't be re-sized.)
RMB+Shift - sorts the table by the col either ascending or descending (Note that
            the ID-header changes to a red color as a warning to indicate that
            the table is not ordered correctly. Before sorting by cols, it is
            strongly suggested to check the row-IDs under 2da Ops->Test row
            order, since re-sorting by row-IDs is the best way to get your table
            back into its correct order. Tables are saved in the order that they
            are sorted.)

click on the rowheads
RMB - opens the context for single-row editing (This handles single-row editing
      only - for multi-row editing see the Edit menu.)

click on a table-cell
LMB       - selects a cell or if already selected then starts the cell-editor,
            or if editing a cell then a left-click on a different part of the
            table either inside or outside the grid accepts the edit
LMB+Ctrl  - adds or subtracts a cell from the currently selected cells
LMB+Shift - selects a block of cells if there is only one currently selected
            cell
RMB       - selects a cell and opens the cell context (Note that if editing a
            cell then a right-click on a different part of the table either
            inside or outside the grid also cancels the edit.)

Note that frozen-col cells cannot be directly selected or edited. Additionally,
note that pressing Shift when clicking on a colhead or rowhead never selects a
col or row; instead, Shift-clicking on a colhead or rowhead always selects the
cells in that col or row, and will select a range of cols or rows if there is
already another selected col or row. Pressing Ctrl, or Ctrl+Shift, when clicking
a colhead or rowhead lends yet more permutations. But it's really not that
complicated: just start clicking with or without Ctrl and Shift ... technically
you're dealing with three items: cells, rows, and cols.

OPERATIONS THAT REFORMAT LARGE TABLES (tens of thousands of Rows) TAKE TIME.
Example: loading or changing the table-font of Placeables.2da with ~25,000 rows
takes ~20 seconds on my decently fast computer.


Settings.Cfg file (do not use double-quotes)

Any change to settings requires a restart.

the following variables ought be respected:

font=        a .NET string that represents the desired table-font (see Font->
             Font ... be patient)
font2=       a .NET string that represents a desired (usually smaller) font for
             menus (Yata needs to be reloaded before it will display a changed
             menu-font)
font3=       a .NET string that represents a desired font for the PropertyPanel
             (Yata needs to be reloaded before it will display a changed
             PropertyPanel font)
fontf=       a .NET string that represents a desired fixed-width font (Yata
             needs to be reloaded before it will display a changed fixed-width
             font)
pathall=     a path without quotes to a valid directory to grope for 2da info
             for Crafting.2da, Spells.2da, or Feat.2da (see Appendix E: how to
             use Info paths)
pathall=     another path for Crafting, Spells, and Feat info
pathall=     etc. (the first pathall has lowest priority and any info found will
             be replaced by any info found in subsequent pathall directories;
             there can be as many or as few pathall directories as you like)
dirpreset=   a path without quotes to a valid directory for the
             Open ... @ folder dialog
dirpreset=   another path for the Open ... @ folder dialog
dirpreset=   etc. (there can be as many or as few dirpresets as you like)
x=           (integer) the desired x-position to start the app on your monitor
y=           (integer) the desired y-position to start the app on your monitor
w=           (integer) the desired starting width of the app on your monitor
h=           (integer) the desired starting height of the app on your monitor
strict=      "true" (without quotes) to show extra warnings when loading a
             2da-file (default false). Strict is intended for users who want to
             notice stuff that is by and large safe to disregard: (1) non alpha-
             numeric characters (other than underscore) in col headers [note
             that double-quotes are disallowed in col headers regardless] (2) a
             character on the 2nd line of a 2da [the 2nd line should be blank as
             far as Yata goes] (3) a tab-character in the version header instead
             of a space-character (4) and Strict also suppresses the tooltip
             that appears when a col is sorted by anything other than ID-
             ascending ("warn : Table is not sorted by ascending ID") - ie,
             persons who use Strict don't get a tooltip although the ID-header
             still turns to a red color regardless of this setting
gradient=    "true" (without quotes) to draw the colhead bar with gradient
             colors
context=     a right-click on a rowhead displays the contextmenu at the mouse-
             cursor's location by default. It can be displayed in a static
             location at the topleft corner of the table instead of at the
             mouse-cursor by giving this variable a value of "static" (without
             quotes)
recent=      (integer) a count of recently opened file-paths to store. If left
             blank or a value less than 1 is specified, recently opened files
             will not be tracked, while 16 is the hardcoded upper limit. SETTING
             "recent=" TO A VALID VALUE ENABLES YATA TO WRITE THE FILE
             Recent.Cfg TO ITS APPLICATION DIRECTORY. Recent.Cfg stores
             filepaths (without quotes). THE WRITE-OPERATION CAN FAIL for a
             variety of reasons that I really don't want to be arshed with -
             hence the option so you can decide if it works on your OS.
diff=        a path without quotes to your WinMerge executable for diffing and
             merging two 2da files (if desired). See Appendix L: WinMerge
dialog=      a path without quotes to your Dialog.Tlk file. Strrefs can often
             print their string values to the statusbar if Dialog.Tlk has been
             pathed
dialogalt=   as "dialog=" but for a custom talktable
maximized=   "true" (without quotes) to start Yata in a maximized window. A true
             setting takes precedence over the x/y/w/h settings although the
             latter are recalled if the window is restored
instantgoto= "true" (without quotes) causes the current table to select a row as
             digits are typed in the goto-box. If false [Enter] needs to be
             pressed to select a row after digits are typed
casesort=    "true" (without quotes) for case-sensitive sorting

The dirpresets appear on the File menu (if specified) and are a quick way to
show an open-file-dialog at your frequently used directory(s).

The pathall directories are for gathering Info that will appear on the statusbar
if Crafting.2da, Spells.2da, or Feat.2da are loaded as the cursor is moved over
their cells. (Yata was designed with Crafting in mind and can show stuff like
Encoded IPs as readable strings on the statusbar, eg.)

To bypass a setting without deleting it, prefix its line with any character you
want. The parser considers only lines that begin with the string-variables
above; any line that doesn't begin exactly with one of those strings is ignored.


Appendix A: note on Load

Yata is pretty strict when loading a 2da file. If it detects anything awkward it
lets the user know and will try to automatically fix cell-fields. Such cells
should then appear highlighted with pink - these are called "loadchanged" cells,
since they were changed when the 2da was loaded. An option under the Edit menu
can cycle through these cells if there are any, for your review and or manual
corrections.

Double-quote marks that are out of place can play havoc with loading a 2da. Yata
is pretty strict with double-quotes when either loading a 2da or editing a cell.
For example it will go so far as to replace two adjacent double-quotes with
"****" (without quotes). If you find issues that Yata doesn't deal well with
it's suggested that you close the file in Yata and try to fix it in a
text-editor (or a different 2da-editor).


Appendix B: copy/paste range

To copy a range of rows there first has to be a selected row (as indicated with
a green field at the far left of a row) and then the Shift key must be used to
select a range of rows. (Using the Control key to select multiple rows does not
work for this.)

A range of copied rows can be pasted/inserted at a currently selected row (as
indicated with a green field at the far left of a row) or to the end of the
table if there is not a currently selected row. Pasting a range does not
overwrite any currently selected row(s) - it rather inserts the copy-range,
starting at the selected row.


Appendix C: edit operations on rows

Since editing by row(s) is not intuitive I should say a few words here.

Edits that involve only one row ought be straightforward; right-click on the
row at the far left and a popup will appear, and that row will be selected. All
edit-operations that are possible on a single row are shown on the popup.

Edits that involve more than one row are more complicated. First you need to get
familiar with these facts: (a) only one row can ever be currently selected (as
indicated with a green field at the far left of that row), (b) to flag multiple
rows for copying (or deleting w/ the Delete key) hold down Shift and click at
the far left of another row (Control will not work for this, although such
row(s)'s cells would appear to be selected; a row of selected cells is *not*
necessarily/technically a selected or flagged row itself!), (c) to copy a range
of rows (a selected row along with its flagged rows) choose "copy range" on the
Edit menu.

Currently flagged rows are indicated with a pale green field at the far left of
their rows.

Copying rows does not use the Windows Clipboard. The copy of such data is
instead maintained internally by Yata. If you want this data on the Clipboard
for whatever reason choose "Export copied row(s)" on the Clipboard menu. To get
data that's on the Clipboard back into Yata's internal format choose "Import
copied row(s)" on the Clipboard menu - but be warned that such data from the
Clipboard is not checked and could be garbage as far as a 2da is concerned, so
it's up to you to decide whether or not to proceed at that point. The contents
of the Clipboard can be viewed and edited by choosing "Open clip editor" on the
Clipboard menu.

Once a range of rows has been copied it can be pasted into a table by choosing
"Paste row(s)" on the Edit menu. Its first row will be pasted at the currently
selected row, pushing that row along with all following rows down such that they
appear beneath the range that is pasted. Note that pasting rows never replaces
any row(s). To replace rows with a copied range of rows, use paste, re-select,
then delete or vice versa. And to paste rows after the last row, choose "Paste
row(s)" without having a currently selected row - pressing the Escape key will
clear all selections of rows and cells.


Appendix D: output

Yata outputs 2da-files as text. It uses a single space for the delimiter. It
does not align cols.


Appendix E: how to use Info paths

Yata is capable of displaying readable info about fields in Crafting.2da,
Spells.2da, and Feat.2da. Paths to various other 2da-files need to be set first,
then info ought be displayed on the statusbar when the mouse-cursor is moved
over the cells of certain cols like "CATEGORY" (displays the title of the
trigger-spell) or "EFFECTS" (displays the recipe's itemproperty in a readable
way), etc. Note that pathing to 2da-files can also be termed, groping ... that
is, when a 2da-file is pathed it will be groped for relevant info.

There are two ways to get such info: (a) Using the Paths menu when Crafting.2da,
Spells.2da, or Feat.2da is loaded, (b) Using "pathall=" entries in Settings.Cfg.

(a) Using the Paths menu

Paths appears on the menubar only when Crafting.2da, Spells.2da, or Feat.2da are
loaded - the filename without extension needs to be "crafting", "spells", or
"feat" (case-insensitive). The items under Paths are divided into five sections:

Path all ...             : this item opens a folder browser dialog to search for
                           any/all applicable 2da-file(s). It can be used more
                           than once and if so, each time another folder is
                           selected any applicable 2da-file(s) will be
                           additionally groped for info. In case of two files
                           with the same filename in two different directories
                           info from the latest groping will be used.
--
path BaseItems.2da       : these items open a file browser dialog. Use them to
path Feat.2da              path to a specific 2da-file. A check will appear next
path ItemPropDef.2da       to the entry on the menu; selecting the item a second
path Skills.2da            time would clear the info. These are used by
path Spells.2da            Crafting.2da
--
path Classes.2da         : these items open a file browser dialog. Use them to
path Disease.2da           path to a specific 2da-file. A check will appear next
path Iprp_AmmoCost.2da     to the entry on the menu; selecting the item a second
path Iprp_Feats.2da        time would clear the info. These are used to
path Iprp_OnHitSpell.2da   interpret EncodedIPs in Crafting.2da
path Iprp_Spells.2da
path RacialTypes.2da
--
path Categories.2da      : these items open a file browser dialog. Use them to
path Ranges.2da            path to a specific 2da-file. A check will appear next
path SpellTarget.2da       to the entry on the menu; selecting the item a second
                           time would clear the info. These are used by
                           Spells.2da - note that Info for Spells.2da can also
                           make use of data that's groped from Feat.2da,
                           Spells.2da, and Classes.2da above.
--
path MasterFeats.2da     : this item opens a file browser dialog. Use it to path
                           to a specific 2da-file. A check will appear next to
                           the entry on the menu; selecting the item a second
                           time would clear the info. This is used by Feat.2da -
                           note that Info for Feat.2da can also make use of data
                           that's groped from Feat.2da, Skills.2da, Spells.2da
                           and Categories.2da above.

(b) Using "pathall=" entries in Settings.Cfg

You can specify one (or more) "pathall=" directories in Settings.Cfg (without
quotes), and each will operate just like "Path all ..." does under the menubar.

Please note that the path-feature in Yata does not look inside zipped archives.
This is to say that the 2da-files that are part of the stock installation are
invalid targets; the files first need to be copied out of the /Data folder to a
different directory (ie, as unarchived/uncompressed files).

If you have custom versions of those files they should be groped *after* the
stock files.


Appendix F: creating a 2da-file

Yata cannot be used to create a new 2da-file from scratch. So if you want a
brand new 2da, at a minimum you should use a text-editor to specify the 2da
version header, followed by a blank line, followed by a line with the col labels
(preceeded by a space) - look at the top of any valid 2da, in a text-editor, to
see what those first three lines should look like. Yata ought then be able to
open the 2da and assign a first blank row with default cell-values.


Appendix G: Drag & Drop to open 2da-file(s)

Yata will open 2da-files that are dragged and dropped onto it.


Appendix H: opening 2da-files as the app starts

Yata allows multiple instances of itself to run at the same time. However if a
file is double-clicked in Windows Explorer, and Yata is associated with its
file-type, and an instance of Yata is already running, the file will be opened
as a new tab in the earliest instance of Yata that is already running. If you
want a second (third, etc.) instance of Yata to run alongside the first, the
executable needs to be run directly (without passing in any file arguments).

Note that when opening multiple files from Windows Explorer either by selecting
and pressing Enter or with right-click and Open on the contextmenu that appears,
the files will open in separate instances of Yata if there is not a running
instance of Yata. If there is one or more running instances of Yata they will
open in the earliest instance. This is a .NET/Windows Explorer/OS limitation.

IT IS HIGHLY RECOMMENDED TO OPEN ONLY ONE FILE AT A TIME FROM WINDOWS EXPLORER.
To open multiple files from Windows Explorer reliably would require a special
Open with Yata entry on Explorer's contextmenu and I don't want to do that.
Selecting a single file (or two ...) to open from Explorer should be okay -
however if there are too many some of them tend to get ignored.


Appendix I: a note on associating file extensions in Windows Explorer

Yata can be associated to open files that have a file-extension aka file-type in
Windows Explorer. Yata doesn't do that automatically; it's for the user to set
it up if he/she wants to. Note that associating file-types will change your
Windows Registry.


Appendix J: the Property Panel

Yata can display the content of a selected row, or of the row of a single
selected cell, vertically in a panel that appears at the right. Click the small
unlabeled button that's just beneath the Close button on the title bar to open
and close the Property Panel.

Cell-values can be edited in the Property Panel by left-clicking a value field.
To accept an edit, press Enter or left-click on the panel; to cancel an edit,
press Escape or right-click on the panel. Note that clicking elsewhere on the
app causes the panel to lose focus and cancel your current edit.

Advanced: Note that it is possible to change the ID-value of a row in the
Property Panel. It is recommended however to use 2da Ops->Order row ids instead
unless you have special reason not to. The value of a currently frozen col's
cell can also be changed this way (but, note that such a value could be
changed by unfreezing the col and editing the cell directly).

A right-click on the Property Panel button will cycle the panel's docked
location through the four corners of the page in a clockwise direction, or
counter-clockwise if the Shift key is pressed. If the height of the panel is
greater than the height of the page, the panel simply shifts to the left or
right side instead.


Appendix K: Undo/Redo

Yata stores the state of cell-text changes and row insertions and deletions for
Undo and Redo operations.

IMPORTANT: The following operations cannot be Undone/Redone
- create or delete or relabel a col; paste col-cells
- order row ids
- recolor rows
- font changes
- etc.


Appendix L: Yata diff

Yata version 3+ has an internal differ. A right-click on a table's tab shows a
popup with several operations including these four:

- Select diff1 (selects the currently displayed table as diff1)
- Select diff2 (selects the currently displayed table as diff2)
- Reset diffs  (clears tables of their diff flags)
- Sync tables  (re-widths the cols of the two diffed tables so they are visually
                aligned - this is done auto when diff2 is selected. Its only use
                as far as I can see is if you want to re-sync the tables after
                sizing cols yourself)

Notes: diff1 must be selected before diff2. Diff2 can be re-selected, but
re-selecting diff1 causes diff2 to be cleared; diff2 must be selected *after*
diff1. Reloading or sorting a 2da causes its diff to be cleared.

Select diff2 causes four noticable things to happen:
1. cells with texts that differ will be rendered with a teal background color
2. the tables' cols will be justified automatically
3. the tables will scroll in unison if possible
4. the DifferDialog will appear. It lists any differences between col headers
   and row counts. Additionally a Goto button in the lower left corner of the
   dialog can be used to cycle through cell texts that are different between the
   two tables (key Shift to goto previous). Note that closing the DifferDialog
   with its Okay button (or its Close icon or the [Esc] key) does not reset
   diffs; the tables will still be sync'd and the backgrounds of any diff'd
   cells will still be colored teal unless reset by either the Reset button or
   via the tab menu.

A right-click on a diffed cell shows a popup with several operations including
these two:

- merge to other - Ce (copies the text in the selected cell to the other table)
- merge to other - Ro (copies the texts of the row of the selected cell to the
                       other table)

The selected cell's position (x/y) must be present in both tables. Rows do not
have to be the same length; any cells that overflow will be filled with "****".

Merges of multiple cells or rows is not allowed. That's what Appendix M:
WinMerge is for ...


Appendix M: WinMerge

The operation under 2da Ops->External diff/merger will be enabled if the path to
your WinMerge executable is specified in Settings.Cfg by the variable "diff="

- eg
diff=C:\Program Files (x86)\WinMerge\WinMergeU.exe

A diff is performed in Yata from its Tab menu (see Appendix L: Yata diff). If
you are doing a diff and the External diff/merger is invoked, WinMerge should
start with the two diffed files loaded.

Note that if you're not doing a diff in Yata and select the operation, WinMerge
should start with a dialog asking what file you'd like to diff against the
currently displayed 2da.

Also note that kdiff3 accepts the same commandline file arguments as WinMerge,
so the path to its executable can be assigned in Settings.Cfg if preferred.

If a file is saved in the external differ, Yata's file monitor should pop up
asking what to do (Reload file, Close 2da, or Cancel).

WinMerge can be downloaded at
http://winmerge.org/

kdiff3 (which is overkill for 2da files) can be downloaded at
http://kdiff3.sourceforge.net/
