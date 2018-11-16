Yata - yet another 2da editor for Neverwinter Nights 2

This app does not write to the Registry, nor does it write any files that you
don't tell it to. It can write 2da files.

2018 nov 8
kevL's
ver 2.1.0.0

File
- Open ... : Ctrl+o
- Reload   : Ctrl+r
- Folders (presets for the Open ... dialog - does not appear if no presets have
  been set)

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

- copy range (copies a range of selected rows)
- paste range (pastes a range of selected rows)

- export copy to clipboard (exports the current copy-list to the clipboard)
- import clipboard to copy (imports any clipboard-text to the current copy-list
                            WARNING: No validity test is done on the clipboard
                            text; importing assumes that the text on your
                            clipboard contains valid 2da-row data)
- view clipboard contents  (accesses the Windows Clipboard for viewing or
                            editing)


Goto box (type a row ID and press Enter)

Search box (type a string to search for)
Search options dropdown (substring or wholeword)

2da Ops
- test row order (investigates the row-order of the currently displayed 2da)
- order row ids (auto-orders the rows of the currently displayed 2da)

- recolor rows (tables show with alternating row-colors. When rows are inserted/
  deleted or sorted the colors go out of sync. to aid understanding of what just
  happened. The "recolor rows" operation makes row-colors alternate as usual)
- autosize cols (recalculates width of all cols)

- freeze 1st col : F5 (causes the first col after the ID to remain stationary)
- freeze 2nd col : F6 (causes the first and second cols after the ID to remain
                   stationary) One of those two cols typically contains the
                   row's label - so by freezing it you can scroll to the right
                   and still read what it is.

Font
- Font ... be patient (pick a font, any valid TrueType font on your system, to
  display the table-data with)
- current font string (a .NET string representing the current table font)
- default (sets the table-font to Yata's hardcoded default font)


keys:
- w/ only 1 cell selected
Home - selects cell at start of the row
End - selects cell at the end of the row
PageUp - selects cell a page above
PageDown - selects cell a page below
Ctrl+Home - selects first cell in the table
Ctrl+End - selects last cell in the table
Left/Right/Up/Down arrows - selects next cell in the direction

- w/out 1 cell selected (or more than one cell is selected)
Home - scrolls table all the way left
End - scrolls table all the way right
PageUp - scrolls table a page up
PageDown - scrolls table a page down
Left/Right/Up/Down arrows - scrolls table in the direction
Ctrl+Home - scrolls table to top
Ctrl+End - scrolls table to bottom

- w/ row selected
Home - selects the top row
End - selects the bottom row
PageUp - selects the row a page above
PageDown - selects the row a page below
Up/Down arrows - selects the row in the direction

Escape - deselects any selected cells if not currently editing a cell
       - if editing a cell it escapes the edit without changing the field

Enter - starts editing a cell if the table has focus and only one cell is
        currently selected
      - commits an edit if the editor box has focus
      - performs search if the Search box or Search Options dropdown has focus

Delete - when a row is selected (as indicated with a green field at the far left
of a row) the Delete-key deletes that row. Use Shift+LMB on another row, above
or below the selected row, to select a range of rows to delete.


mouse:
wheel - scrolls up/down if the vertical scrollbar is visible
      - scrolls left/right if the horizontal scrollbar is visible but the
        vertical bar is not

on the colheads or rowheads
LMB - selects the col or row
LMB+Ctrl - adds or subtracts a col/row from the currently selected cells
LMB+Shift - selects a range of cols/rows if a col/row is already selected
LMB+Ctrl+Shift - you get the idea ...

on the colheads
RMB+Shift - sorts the table by the col either ascending or descending

on a table-cell
LMB - selects a cell or if already selected then starts the cell-editor
LMB+Ctrl - adds or subtracts a cell from the currently selected cells

Note that frozen-col cells cannot be selected or edited.

OPERATIONS THAT REFORMAT LARGE TABLES (tens of thousands of Rows) TAKE TIME.
Example: loading or changing the table-font of Placeables.2da with ~25,000 rows
takes about 15 seconds on my decently fast computer.


Settings.Cfg file

the following variables ought be respected:

font=      a .NET string that represents the desired table-font (see 2da
           Ops->current font string)
font2=     a .NET string that represents a desired (usually smaller) font for
           menus
pathall=   a path without quotes to a valid directory to grope for 2da info
           for Crafting.2da
pathall=   another path for Crafting info
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
strict=    "true" to show extra warnings when loading a 2da file (default false)

The dirpresets appear on the File menu (if specified) and are a quick way to
show the Open ... dialog at your frequently used directory(s).

The pathall directories are for gathering Crafting.2da info that will appear
when mouseovering cells of Crafting.2da. (Yata was designed with Crafting in
mind and can show stuff like Encoded IPs as readable strings on the statusbar.)


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
you've chosen to Freeze the 1st or 2nd col, those cells can't be edited until
Freeze is turned back off.


Appendix B: copy/paste range

To copy a range of rows there first has to be a selected row (as indicated with
a green field at the far left of a row) and then the Shift key must be used to
select a range of rows. (Using the Control key to select multiple rows does not
work for this.)

A range of copied rows can be pasted/inserted at a currently selected row (as
indicated with a green field at the far left of a row) or to the end of the
table if there is not a currently selected row. Pasting a range does not
overwrite any currently selected row(s) - it rather inserts the copy-range,
starting at a selected row.


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
necessarily a selected or flagged row!), (c) to copy a range of rows (a selected
row along with its flagged rows) choose "copy range" on the Edit menu.

Currently flagged rows are indicated with a pale green field at the far left of
their rows.

Copying rows does not use the Windows Clipboard. The copy of such data is
instead maintained internally by Yata. If you want this data on the Clipboard
for whatever reason choose "export copy to clipboard" on the Edit menu. To get
data that's on the Clipboard back into Yata's internal format choose "import
clipboard to copy" on the Edit menu - but be warned that such data from the
Clipboard is not checked and could be garbage as far as a 2da is concerned, so
it's up to you to decide whether or not to proceed at that point. The contents
of the Clipboard can be viewed and edited by choosing "view clipboard contents"
on the Edit menu.

Once a range of rows has been copied it can be pasted into a table by choosing
"paste range" on the Edit menu. Its first row will be pasted at the currently
selected row, pushing that row along with all following rows down such that they
appear beneath the range that is pasted. Note that pasting rows never replaces
any row(s). To replace rows with a copied range of rows, use paste, re-select,
then delete or vice versa. And to paste rows after the last row, choose "paste
range" without having a currently selected row - pressing the Escape key will
clear all selections of rows and cells.
