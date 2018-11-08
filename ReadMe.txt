Yata - yet another 2da editor for Neverwinter Nights 2

2018 nov 8
kevL's
ver 2.1.0.0

File
- Open ...	: Ctrl+o
- Reload	: Ctrl+r
- Folders

- Save			: Ctrl+s
- Save As ...	: Ctrl+a
- Close
- Close all

- Quit : Ctrl+q

Edit
- Find		: Ctrl+f
- Find next	: F3

Search box
Search options dropdown

2da Ops
- test row order
- order row ids

- recolor rows

- freeze 1st col : F5
- freeze 2nd col : F6

Font
- Font ... be patient
- current font string
- default


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


mouse:
wheel - scrolls up/down if the vertical scrollbar is visible
      - scrolls left/right if the horizontal scrollbar is visible but the
		vertical bar is not

on the colheads or rowheads
LMB - selects the col or row
LMB+Ctrl - adds or subtracts a col/row from the currently selected cells
LMB+Shift - selects a range of cols/rows if a col/row is already selected

on the colheads
RMB+Shift - sorts the table by the col either ascending or descending

on a table-cell
LMB - selects a cell or if already selected then starts the cell-editor
LMB+Ctrl - adds or subtracts a cell from the currently selected cells


Note that frozen-col cells cannot be selected or edited.

OPERATIONS ON LARGE TABLES (tens of thousands of Rows) WILL TAKE A LONG TIME.
Example: loading Placeables.2da with ~25,000 rows takes over 50 seconds. Sorting
descending by ID (which involves shuffling every row) takes about 6 minutes on
my decently fast computer.


Settings.Cfg file

the following variables will be respected:

font=		a .NET string that represents the desired table-font (see 2da
			Ops->current font string)
font2=		a .NET string that represents the desired small-font for menus
pathall=	a path without quotes to a valid directory to grope for 2da info
			for Crafting.2da
pathall=	another path for Crafting info
pathall=	etc. (the first pathall has lowest priority and any info found will
			be replaced by any info found in subsequent pathall directories;
			there can be as many or as few pathall directories as you like)
dirpreset=	a path without quotes to a valid directory for the Open ... dialog
dirpreset=	another path for the Open ... dialog
dirpreset=	etc.
x=			(integer) the desired x-position to start the app on your monitor
y=			(integer) the desired y-position to start the app on your monitor
w=			(integer) the desired starting width of the app on your monitor
h=			(integer) the desired starting height of the app on your monitor

The dirpresets appear on the File menu (if specified) and are a quick way to
show the Open ... dialog at your frequently used directory(s).

The pathall directories are for gathering Crafting.2da info that will appear
when mouseovering cells of Crafting.2da. (Yata was designed with Crafting in
mind and can show stuff like Encoded IPs as readable strings on the statusbar.)
