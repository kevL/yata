Yata - yet another 2da editor for Neverwinter Nights 2

This app does not write to the Registry, nor does it write any files that you
don't tell it to. It can write 2da files.

2018 dec 9
kevL's
ver 2.5.9.0


File
- Open ... @ folder (presets for the Open ... dialog - does not appear if no presets have
                     been set)
- Open ... : Ctrl+o
- Reload   : Ctrl+r

- Save : Ctrl+s
- Save As ...
- Close
- Close all (this is a multi-tabbed application)

- Quit : Ctrl+q


Edit
- Find      : Ctrl+f (focuses the Search box)
- Find next : F3

- Goto : Ctrl+g (focuses the Goto box)
- Goto loadchanged (See note on Load below)

- copy cell   : Ctrl+c (copies a single cell)
- paste cell  : Ctrl+v (pastes a single cell)
- copy range  : Ctrl+Shift+c (copies a selected row or range of rows)
- paste range : Ctrl+Shift+v (pastes a selected row or range of rows)

- export copy to Clipboard (exports the current copy-list to the clipboard)
- import Clipboard to copy (imports any clipboard-text to the current copy-list
                            WARNING: No validity test is done on the clipboard
                            text; importing assumes that the text on your
                            clipboard contains valid 2da-row data)
- open Clipboard editor (accesses the Windows Clipboard for viewing/editing)


Goto box (type a row ID and press Enter)


Search box (type a string to search for)
Search options dropdown (substring or wholeword)


2da Ops
- order row ids (auto-orders the IDs of the currently displayed 2da)
- test row order (investigates the row-order of the currently displayed 2da)

- recolor rows (tables show with alternating row-colors. When rows are inserted/
  deleted (or sorted) the colors go out of sync to aid understanding of what
  just happened. The "recolor rows" operation makes row-colors alternate as
  usual)
- autosize cols (recalculates the display-width of all cols)

- freeze 1st col : F5 (causes the first col after the ID-col to remain
                   stationary)
- freeze 2nd col : F6 (causes the first and second cols after the ID-col to
                   remain stationary) One of those two cols typically contains
                   the row's "label" - so by freezing it you can scroll to the
                   right and still read what it is.

- PropertyPanel on/off  : F7 (toggles the PropertyPanel on/off)
- PropertyPanel top/bot : F8 (toggles the panel's position top/bottom)


Font
- Font ... be patient (pick a font, any valid TrueType font on your system, to
  display the table-data with)
- current font string (a .NET string representing the current table font)
- default (sets the table-font to Yata's hardcoded default font)


Paths (see Appendix E: how to use Info paths)


Help
- ReadMe.txt : F1 (opens this document in a text-editor)
- About      : displays the version/build-type of the executable



keys:
- w/ only 1 cell selected
Home                      - selects cell at start of the row
End                       - selects cell at the end of the row
PageUp                    - selects cell a page above
PageDown                  - selects cell a page below
Ctrl+Home                 - selects first cell in the table
Ctrl+End                  - selects last cell in the table
Left/Right/Up/Down arrows - selects next cell in the direction

- w/out 1 cell selected (or more than one cell is selected)
Home                      - scrolls table all the way left
End                       - scrolls table all the way right
PageUp                    - scrolls table a page up
PageDown                  - scrolls table a page down
Ctrl+Home                 - scrolls table to top
Ctrl+End                  - scrolls table to bottom
Left/Right/Up/Down arrows - scrolls table in the direction

- w/ row selected
Home              - selects the top row
End               - selects the bottom row
PageUp            - selects the row a page above
PageDown          - selects the row a page below
Up/Down arrows    - selects the row in the direction
Left/Right arrows - scrolls table in the direction

Escape - deselects any selected cells/rows/cols if not currently editing a cell
       - if editing a cell it escapes the edit without changing the field

Enter - starts editing a cell if the table has focus and only one cell is
        currently selected
      - commits an edit if the editor box has focus
      - performs search if the Search box or Search Options dropdown has focus
      - performs goto if the Goto box has focus

Delete - when a row is selected (as indicated with a green field at the far left
of a row) the Delete-key deletes that row. Use Shift+LMB on another row, above
or below the selected row, to select a range of rows to delete. If all rows of a
table are deleted a single default row will be created.


mouse:
wheel - scrolls up/down if the vertical scrollbar is visible
      - scrolls left/right if the horizontal scrollbar is visible but the
        vertical bar is not

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
            the ID-header changes to a violet color as a warning to indicate
            that the table is not ordered correctly. Before sorting by cols, it
            is strongly suggested to check the row-IDs under 2da Ops->test row
            order, since re-sorting by row-IDs is the best way to get your table
            back into its correct order. Tables are saved in the order that they
            are sorted.)

click on the rowheads
RMB - opens the contextmenu for single-row editing

click on a table-cell
LMB      - selects a cell or if already selected then starts the cell-editor, or
           if editing a cell then a left-click on a different part of the table
           (either inside or outside the grid) accepts the edit
LMB+Ctrl - adds or subtracts a cell from the currently selected cells
RMB      - if editing a cell then a right-click on a different part of the table
           (either inside or outside the grid) cancels the edit

Note that frozen-col cells cannot be selected or edited. Additionally, note that
pressing Shift when clicking on a colhead or rowhead never selects a col or row;
instead, Shift-clicking on a colhead or rowhead always selects the cells in that
col or row, and will select a range of cols or rows if there is already another
selected col or row. Pressing Ctrl, or Ctrl+Shift, when clicking a colhead or
rowhead lends yet more permutations. But it's really not that complicated: just
start clicking with or without Ctrl and Shift ... technically you're dealing
with three items: cells, rows, and cols.

OPERATIONS THAT REFORMAT LARGE TABLES (tens of thousands of Rows) TAKE TIME.
Example: loading or changing the table-font of Placeables.2da with ~25,000 rows
takes about 15 seconds on my decently fast computer.


Settings.Cfg file (do not use double-quotes)

the following variables ought be respected:

font=      a .NET string that represents the desired table-font (see 2da
           Ops->current font string)
font2=     a .NET string that represents a desired (usually smaller) font for
           menus (To get a correct .NET font-string for menus, choose the font
           for the table and copy its string with 2da Ops->current font string,
           then change the table-font back to what you like and paste the copied
           string into Settings.Cfg "font2=". Yata must be reloaded before it
           will display with a changed menu-font.)
font3=     a .NET string that represents a desired font for the PropertyPanel
           (see notes at font2)
pathall=   a path without quotes to a valid directory to grope for 2da info
           for Crafting.2da or Spells.2da
pathall=   another path for Crafting or Spells info
pathall=   etc. (the first pathall has lowest priority and any info found will
           be replaced by any info found in subsequent pathall directories;
           there can be as many or as few pathall directories as you like)
dirpreset= a path without quotes to a valid directory for the Open ... dialog
dirpreset= another path for the Open ... dialog
dirpreset= etc. (there can be as many or as few dirpresets as you like)
x=         (integer) the desired x-position to start the app on your monitor
y=         (integer) the desired y-position to start the app on your monitor
w=         (integer) the desired starting width of the app on your monitor
h=         (integer) the desired starting height of the app on your monitor
strict=    "true" (without quotes) to show extra warnings when loading a
           2da-file (default false). Strict is intended for users who want to
           notice stuff that is by and large safe to disregard: (1) non alpha-
           numeric characters (other than underscore) in col headers [note that
           double-quotes are disallowed in col headers regardless] (2) a
           character on the 2nd line of a 2da [the 2nd line should be blank as
           far as Yata goes] (3) a tab-character in the version header instead
           of a space-character (4) and Strict also suppresses the tooltip that
           appears when a col is sorted by anything other than ID-ascending
           ("warn : Table is not sorted by ascending ID") - ie, persons who use
           Strict don't get a tooltip although the ID-header still turns to a
           violet color regardless of this setting
context=   a right-click on a rowhead displays the contextmenu at the mouse-
           cursor's location by default. It can be displayed in a static
           location at the topleft corner of the table instead of at the mouse-
           cursor by giving this variable a value of "static" (without quotes)
gradient=  "true" (without quotes) to draw the colhead bar with gradient colors

The dirpresets appear on the File menu (if specified) and are a quick way to
show the Open ... dialog at your frequently used directory(s).

The pathall directories are for gathering Info that will appear if Crafting.2da
or Spells.2da are loaded as the cursor is moved over their cells. (Yata was
designed with Crafting in mind and can show stuff like Encoded IPs as readable
strings on the statusbar, eg.)


Appendix A: note on Load

Yata is pretty strict when loading a 2da file. If it detects anything awkward it
lets the user know and will try to automatically fix cell-fields. Such cells
should then appear highlighted with pink - these are called "loadchanged" cells,
since they were changed when the 2da was loaded. An option under the Editmenu
can cycle through these cells if there are any, for your review and or manual
corrections.

Double-quote marks that are out of place can play havoc with loading a 2da. Yata
is pretty strict with double-quotes when either loading a 2da or editing a cell.
For example it will go so far as to replace two adjacent double-quotes with
"****" (without quotes). If you find issues that Yata doesn't deal well with
it's suggested that you close the file in Yata and try to fix it in a
text-editor (or a different 2da-editor).

Further, the only way to edit the ID col is with 2da Ops "order row ids". And if
you've chosen to Freeze the 1st or 2nd col, those cells can't be edited (or
searched) until Freeze is turned back off.


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
for whatever reason choose "export copy to Clipboard" on the Edit menu. To get
data that's on the Clipboard back into Yata's internal format choose "import
Clipboard to copy" on the Edit menu - but be warned that such data from the
Clipboard is not checked and could be garbage as far as a 2da is concerned, so
it's up to you to decide whether or not to proceed at that point. The contents
of the Clipboard can be viewed and edited by choosing "edit Clipboard contents"
on the Edit menu.

Once a range of rows has been copied it can be pasted into a table by choosing
"paste range" on the Edit menu. Its first row will be pasted at the currently
selected row, pushing that row along with all following rows down such that they
appear beneath the range that is pasted. Note that pasting rows never replaces
any row(s). To replace rows with a copied range of rows, use paste, re-select,
then delete or vice versa. And to paste rows after the last row, choose "paste
range" without having a currently selected row - pressing the Escape key will
clear all selections of rows and cells.


Appendix D: output

Yata outputs 2da-files as text. It uses a single space for the delimiter. It
does not align cols.


Appendix E: how to use Info paths

Yata is capable of displaying readable info about fields in Crafting.2da and
Spells.2da. Paths to various other 2da-files need to be set first, then info
ought be displayed on the statusbar when the mouse-cursor is moved over the
cells of certain cols like "CATEGORY" (displays the title of the trigger-spell)
or "EFFECTS" (displays the recipe's itemproperty in a readable way), etc. Note
that pathing to 2da-files can also be termed, groping ... that is, when a 2da-
file is pathed it will be groped for relevant info.

There are two ways to get such info: (a) Using the Paths menu when Crafting.2da
or Spells.2da is loaded, (b) Using "pathall=" entries in Settings.Cfg.

(a) Using the Paths menu

Paths appears on the menubar only when Crafting.2da or Spells.2da are loaded -
the filename without extension needs to be "crafting" or "spells" (case-
insensitive). The items under Paths are divided into four sections:

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
                           make use of data that's groped from Spells.2da,
                           Feat.2da, and Classes.2da above.

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

Yata cannot be used to create a new 2da-file from scratch. Nor is it able to
change the quantity of cols or the col labels. It is able to edit only rows and
cell-fields. So if you want a brand new 2da, at a minimum you should use a text-
editor to specify the 2da version header, followed by a blank line, followed by
a line with the col labels (preceeded by a space) - look at the top of any valid
2da, in a text-editor, to see what those first three lines should look like.
Yata ought then be able to open the 2da and assign a first blank row with
default cell-values.


Appendix G: Drag & Drop to open 2da-file(s)

Yata will open 2da-files that are dragged and dropped onto it.


Appendix H: opening 2da-files when the app starts

Yata allows multiple instances of itself to run at the same time. However if a
file is double-clicked in Windows Explorer, and Yata is associated with its
file-type, and an instance of Yata is already running, the file will be opened
as a new tab in the earliest instance of Yata that is already running. If you
want a second (third, etc.) instance of Yata to run alongside the first, the
executable needs to be run directly (without passing in any file arguments).

Note that when opening multiple files from Windows Explorer either by selecting
and pressing Enter or with right-click and Open on the contextmenu that appears,
the files will open in separate instances of Yata if there is not a running
instance of Yata. If there is a running instance of Yata they will open in the
earliest instance like opening a single file from Explorer does. This is a
.NET <-> Windows Explorer/OS limitation.


Appendix I: a note on associating file extensions in Windows Explorer

Yata can be associated to open files that have a file-extension aka file-type in
Windows Explorer. Yata doesn't do that automatically; it's for the user to set
it up if he/she wants to.


Appendix J: the Property Panel

Yata can display the content of a selected row, or of the row of a single
selected cell, vertically in a panel that appears at the right. Click the small
unlabeled button that's just beneath the Close button on the title bar to open
and close the Property Panel.

Cell-values can be edited by left-click. To accept an edit, press Enter or left-
click on the panel; to cancel an edit, press Escape or right-click on the panel.
Note that clicking elsewhere on the app will cause the panel to lose focus and
cancel your current edit.

Advanced: Note that it's possible to change the ID-value of a row in the
Property Panel. It is recommended however to use 2da Ops->order row ids instead,
unless you have special reason not to. The value of a currently frozen col's
cell can also be changed this way (further, note that such a value could be
changed by unfreezing the col and editing the cell directly).

A right-click on the Property Panel button will switch the panel's docked
position from the topright of the grid to the bottomright of the grid, or vice
versa. (This is noticeable only if the height of the panel is less than the
height of the grid.)
