Yata - yet another 2da editor for Neverwinter Nights 2

This app does not write to the Registry, nor does it write any files that you
don't tell it to. It can write 2da files.

2018 nov 8
kevL's
ver 2.1.0.0

File
- Open ...	: Ctrl+o
- Reload	: Ctrl+r
- Folders (presets for the Open ... dialog - does not appear if no presets have
  been set)

- Save			: Ctrl+s
- Save As ...	: Ctrl+a
- Close
- Close all (this is a multi-tabbed application)

- Quit : Ctrl+q

Edit
- Find		: Ctrl+f (focuses the Search box)
- Find next	: F3

Search box (type a string to search for)
Search options dropdown (substring or wholeword)

2da Ops
- test row order (investigates the row-order of the currently displayed 2da)
- order row ids (auto-orders the rows of the currently displayed 2da)

- recolor rows (tables show with alternating row-colors. When rows are inserted/
  deleted or sorted the colors go out of sync. to aid understanding of what just
  happened. The "recolor rows" operation makes row-colors alternate as usual)

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

Escape - deselects any selected cells if not currently editing a cell
       - if editing a cell it escapes the edit without changing the field

Enter - starts editing a cell if the table has focus and only one cell is
        currently selected
      - commits an edit if the editor box has focus
      - performs search if the Search box or Search Options dropdown has focus


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

OPERATIONS ON LARGE TABLES (tens of thousands of Rows) WILL TAKE A LONG TIME.
Example: loading Placeables.2da with ~25,000 rows takes 15-20 seconds, while
Sorting descending by ID (which involves shuffling every row) takes about 4
minutes on my decently fast computer.


Settings.Cfg file

the following variables ought be respected:

font=		a .NET string that represents the desired table-font (see 2da
			Ops->current font string)
font2=		a .NET string that represents a desired (usually smaller) font for
			menus
pathall=	a path without quotes to a valid directory to grope for 2da info
			for Crafting.2da
pathall=	another path for Crafting info
pathall=	etc. (the first pathall has lowest priority and any info found will
			be replaced by any info found in subsequent pathall directories;
			there can be as many or as few pathall directories as you like)
dirpreset=	a path without quotes to a valid directory for the Open ... dialog
dirpreset=	another path for the Open ... dialog
dirpreset=	etc. (there can be as many or as few dirpresets as you like)
x=			(integer) the desired x-position to start the app on your monitor
y=			(integer) the desired y-position to start the app on your monitor
w=			(integer) the desired starting width of the app on your monitor
h=			(integer) the desired starting height of the app on your monitor

The dirpresets appear on the File menu (if specified) and are a quick way to
show the Open ... dialog at your frequently used directory(s).

The pathall directories are for gathering Crafting.2da info that will appear
when mouseovering cells of Crafting.2da. (Yata was designed with Crafting in
mind and can show stuff like Encoded IPs as readable strings on the statusbar.)
