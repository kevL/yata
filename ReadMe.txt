Yata - yet another 2da editor for Neverwinter Nights 2

This app does not write to the Registry, nor does it write any files that you
don't tell it to. It can write 2da files. Various settings for Yata can be
changed in the Settings.Cfg textfile.

2023 february 21
kevL's
ver 5.4.4.0

c# source .net 3.5
https://github.com/kevL/yata


Table of Contents
-----------------
1. MainMenu bar
2. Keyboard input
3. Mouse input
4. Settings.Cfg file

Appendix A: note on Load
Appendix B: copy/paste range
Appendix C: edit operations on rows
Appendix D: output
Appendix E: how to use Info paths
Appendix F: creating a 2da-file
Appendix G: Drag & Drop to open 2da-file(s)
Appendix H: opening 2da-files as the app starts
Appendix I: a note on associating file extensions in Windows Explorer
Appendix J: the Property Panel
Appendix K: Undo/Redo
Appendix L: Yata diff
Appendix M: WinMerge
Appendix N: Codepages



1. MainMenu bar

File
- Create ... creates a basic 2da table

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

- Close     : F4
- Close all : Ctrl+F4 (this is a multi-tabbed application)

- Quit : Ctrl+q


Edit
- Undo : Ctrl+z (see Appendix K: Undo/Redo)
- Redo : Ctrl+y

- Deselect all : deselects selected cells, rows, and cols

- Goto : Ctrl+g (focuses the Goto box)

- Find      : Ctrl+f (focuses the Search box)
- Find next : F3
- Find pre  : Shift+F3

- Replace text ...   : opens a dialog to find and replace fields or subfields
- Goto replaced next : selects the next cell that has been changed after a
                       replace-all operation
- Goto replaced pre  : selects the previous cell that has been changed after a
                       replace-all operation
- Clear replaced     : clears all cells' replaced flags

- Goto loadchanged next : Ctrl+n (see Appendix A: note on Load)
- Goto loadchanged pre  : Ctrl+Shift+n
- Clear loadchanged     : clears all cells' loadchanged flags

- Default value ... : opens a dialog to edit the 2da-file's Default value (apps
                      that try to access an invalid row or col should return
                      failure and use this value as a default string, or convert
                      it to a default int or float. This is rarely used if ever;
                      2da-readers typically don't try to access an invalid cell
                      ... or if they do they assign their own default value).
                      Entering a blank string will clear the Default; enter ""
                      (with quotes) to specify that a blank string should be the
                      Default value
- Clear Default     : deletes a 2da's Default value if it exists


Cells
- Deselect : deselects selected cells as well as any selected rows/cols

- Cut    : Ctrl+x (cuts selected cell(s) if they are in a contiguous block)
- Copy   : Ctrl+c (copies selected cell(s) if they are in a contiguous block)
- Paste  : Ctrl+v (pastes copied cell(s) if only 1 cell is selected; that cell
           will be at the top left if pasting a block of cells; pasted cells
           that don't fit in the table get discarded)
- Delete : Delete (clears selected cell(s))

- Lowercase : converts selected cell(s) to lowercase
- Uppercase : converts selected cell(s) to uppercase

- Apply text ... : F11 (opens a dialog that replaces selected cell(s))


Rows
- Deselect : deselects selected row and any subselected rows along with their
             selected cells

- Cut    : Shift+Ctrl+x (cuts selected row and any selected subrow(s))
- Copy   : Shift+Ctrl+c (copies selected row and any selected subrow(s))
- Paste  : Shift+Ctrl+v (pastes copied row(s))
- Delete : Shift+Delete (deletes selected row and any selected subrow(s))

- Create ... : F2 (opens a dialog that inserts 1+ rows with options to fill the
               fields. (a) selected row (b) first copied row (c) ****)


Col
- deselect : deselects selected col and its selected cells

- create head ...  : creates a col. This clears Undo/Redo, etc. The col will be
                     created at the position of a selected col, shifting the
                     table to the right; press [Esc] to deselect any selected
                     col to create a col at the far right of the table *
- delete head ...  : deletes a selected col. This clears Undo/Redo, etc. This
                     operation is disabled if the table has only 1 colhead.
- relabel head ... : relabels the head of a selected col *

- copy cells  : copies the cells of a selected col
- paste cells : pastes copied cells into a selected col

* Creating or Relabeling a colhead disallows Unicode characters (non-ASCII
  characters). This is further restricted to only alpha-numeric digits and/or
  the underscore character if the setting "strict=" is true. The double-quote
  character is disallowed in a colhead label regardless.


Goto box (type a row ID and press Enter. See also Settings.Cfg "instantgoto=")


Search box (type a string to search for and press [Enter] or [F3]. Note that
            [F3] keeps the searchbox focused while [Enter] switches focus to the
            table. [Shift] reverses the search direction
Search options dropdown (subfield or wholefield)


Clipboard
- Export copied row(s) to clipboard : F9 (exports the internal row-copy buffer
                                      to the clipboard)
- Import clipboard to copied row(s) : F10 (imports any clipboard text to the
                                      internal row-copy buffer WARNING: No
                                      validity test is done on the clipboard
                                      text; importing assumes that the text on
                                      your clipboard contains valid 2da-row
                                      data)

- Open clipboard editor : Ctrl+p (accesses the Windows Clipboard for viewing or
                          editing - only clips in text format are available. The
                          copied rows, copied cells, and copied col buffers can
                          also be previewed in the editor but are readonly)


2da Ops
- Order row ids  : Ctrl+d (auto-orders the IDs of the currently displayed 2da)
- Test row order : Ctrl+t (tests the row-order of the currently displayed 2da)

- Recolor rows  : Ctrl+l (tables show with alternating row-colors. When rows are
                  inserted/deleted (or sorted) the colors go out of sync to aid
                  understanding of what just happened. The "recolor rows"
                  operation makes row-colors alternate as usual)
- Autosize cols : Ctrl+i (recalculates the display-width of all cols)

- Freeze 1st col : F5 (causes the first col after the ID-col to remain
                   stationary)
- Freeze 2nd col : F6 (causes the first and second cols after the ID-col to
                   remain stationary) One of those two cols typically contains
                   the row's "label" - so by freezing it you can scroll to the
                   right and still read what it is.

- Propanel               : F7 (toggles the PropertyPanel on/off)
- Propanel location next : F8 (cycles the panel's location through the four
                           corners of the table)
- Propanel location pre  : Shift+F8 (reverse direction)

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


Paths (appears only when a 2da called "crafting", "spells", "feat", "classes",
       or "baseitems" is loaded - see Appendix E: how to use Info paths)


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

- Options file (opens an internal texteditor to edit Settings.Cfg)


2. Keyboard input

If a row is selected then key-input for rows takes precedence. Else cells.

- w/ row selected -
Up             - selects the row above
Down           - selects the row below
PageUp         - selects the row a page above
PageDown       - selects the row a page below

Ctrl+Home      - selects the top row
Ctrl+End       - selects the bottom row

Shift+Home     - subselects rows to the top of the table
Shift+End      - subselects rows to the bottom of the table
Shift+Up       - adjusts subselected rows up
Shift+Down     - adjusts subselected rows down
Shift+PageUp   - adjusts subselected rows up by visible height
Shift+PageDown - adjusts subselected rows down by visible height

Left           - scrolls table left
Right          - scrolls table right
Shift+Left     - scrolls table left by visible width
Shift+Right    - scrolls table right by visible width
Home           - scrolls table all the way left
End            - scrolls table all the way right

- w/ only 1 cell selected -
Left           - selects cell to the left
Right          - selects cell to the right
Up             - selects cell above
Down           - selects cell below
PageUp         - selects cell a page above
PageDown       - selects cell a page below
Home           - selects cell at start of the row
End            - selects cell at the end of the row

Ctrl+Up        - selects first cell in the col
Ctrl+Down      - selects last cell in the col
Ctrl+Home      - selects first cell in the table
Ctrl+End       - selects first cell in last row of the table

- w/out cell selected or 2+ cells selected -
Left           - scrolls table left
Right          - scrolls table right
Up             - scrolls table up
Down           - scrolls table down
PageUp         - scrolls table a page up
PageDown       - scrolls table a page down
Home           - scrolls table all the way left
End            - scrolls table all the way right

Ctrl+Home      - scrolls table to top
Ctrl+End       - scrolls table to bottom

Shift+Left     - scrolls table left by visible width if no cell is selected
                 or selected cells are not contiguous
Shift+Right    - scrolls table right by visible width if no cell is selected
                 or selected cells are not contiguous

- extended keyboard handling when only 1 cell is selected or if selected cells
  are in a contiguous block. These operations are based on an anchorcell ... the
  primary method of choosing an anchorcell is to click on a cell to select it or
  press [Space]. Or when a row/col is selected the first cell in the row/col
  replaces the current anchorcell -

Shift+Left          - adjusts block selection left
Shift+Right         - adjusts block selection right
Shift+Up            - adjusts block selection up
Shift+Down          - adjusts block selection down

Shift+PageUp        - adjusts block selection up by visible height
Shift+PageDown      - adjusts block selection down by visible height

Shift+Ctrl+PageUp   - adjusts block selection to the top of the table
Shift+Ctrl+PageDown - adjusts block selection to the bottom of the table

Shift+Home          - adjusts block selection to the left edge of the table
Shift+End           - adjusts block selection to the right edge of the table

Shift+Ctrl+Home     - adjusts block selection to the first cell in the table
Shift+Ctrl+End      - adjusts block selection to the last cell in the table


Space - focuses the table and selects the first cell if no cells are currently
        selected. If cell(s) are already selected all cells except the
        anchorcell will be deselected. An anchorcell is used to determine the
        area of a contiguous block of selected cells; technically the anchorcell
        can be invalid in which case the cell that gets selected shall be set as
        the anchorcell. The table will scroll to ensure that the cell that gets
        selected is visible.

Ctrl+Space - focuses the table and selects the first row if no rows or cells are
             currently selected. If a row is already selected all cells that are
             not in the row will be deselected. If a row is not already selected
             and there are any cell(s) currently selected, the row of the first
             selected cell will be selected. The table will scroll to ensure
             that the row that gets selected is visible.

Escape - deselects any selected cells/rows/cols if not currently editing a cell
       - if editing a cell it cancels the edit without changing the field
       - if the tabcontrol is focused it switches focus to the table

Enter - starts editing a cell if the table has focus and only one cell is
        currently selected
      - accepts an edit if the editor box has focus
      - performs search if the Search box or Search Options dropdown has focus
      - performs goto if the Goto box has focus

Delete       - clears the text of selected cells
Shift+Delete - when a row is selected (as indicated with a green field at the
               far left of a row) [Shift+Delete] deletes that row. Use Shift+LMB
               on another row, above or below the selected row, to select a
               range of rows to delete. If all rows of a table are deleted a
               single default row will be created.

Note that Cut, Copy, Paste operations have their keystrokes listed under the
Cells and Rows descriptions above. The Col operations do not have shortcuts.

- Tab fastedit -

When a cell is being edited the following keys are accepted.

Tab                  - accept text and edit the cell to the right
Shift+Tab            - accept text and edit the cell to the left
Ctrl+Tab or Down     - accept text and edit the cell below
Ctrl+Shift+Tab or Up - accept text and edit the cell above
PageDown             - accept text and edit the cell a page below
PageUp               - accept text and edit the cell a page above

Note that [Tab]/[Down] and [Shift+Tab]/[Up] also work when editing a cell in the
PropertyPanel.


3. Mouse input

wheel - scrolls up/down if the vertical scrollbar is visible and either of
        (a) the horizontal bar is disabled or (b) Ctrl is not pressed
      - scrolls left/right if the horizontal scrollbar is visible and either of
        (a) the vertical bar is disabled or (b) Ctrl is pressed
Shift+wheel - scrolls the table by its visible height/width if applicable; the
              Ctrl-key is also respected

- click on the colheads or rowheads -
LMB            - selects or deselects the col/row and clears other cells. If an
                 editbox is visible it will be canceled or accepted depending on
                 the Setting "acceptedit="
LMB+Ctrl       - selects or deselects the col/row
LMB+Shift      - selects a range of cols/rows if a col/row is already selected
LMB+Ctrl+Shift - you get the idea ...
RMB            - if an editbox is visible it will be canceled or accepted
                 depending on the Setting "acceptedit="

- click on the colheads -
LMB       - click-drag col-boundary to re-width a col. The text of a colhead
            will appear grayed if its col has been user-sized
RMB       - click a col-boundary to auto-width a col. Frozen cols can't be
            re-sized
RMB+Shift - sorts the table by the col either ascending or descending. The
            ID-header changes to a red color as a warning to indicate that the
            table is not ordered correctly. Before sorting by cols, it is
            strongly suggested to check the row-IDs under 2da Ops->Test row
            order, since re-sorting by row-IDs is the best way to get your table
            back into its correct order. Tables are saved in the order that they
            are sorted

- click on the rowheads -
RMB - opens the context for single-row editing. This handles single-row editing
      only. For multi-row editing see the Rows menu

- click on a table-cell -
LMB        - selects a cell or if already selected then starts the cell-editor,
             or if editing a cell then a left-click on another cell accepts the
             edit
LMB double - selects a cell and starts the cell-editor. If a different cell is
             being edited changes to that cell are accepted
LMB+Ctrl   - adds or subtracts a cell from the currently selected cells
LMB+Shift  - selects a block of contiguous cells if there is only one currently
             selected cell, or resizes a currently selected contiguous block of
             cells. A contiguous block is required for multicell cut and copy
             operations
RMB        - selects a cell and opens the cell context If a different cell is
             being editing then a right-click cancels or accepts the edit
             depending on the Setting "acceptedit="

- click outside the grid -
LMB - accepts an edit if editing a cell
RMB - cancels an edit if editing a cell, otherwise deselects all cells, rows,
      and cols

- click on the frozen panel -
LMB - accepts an edit
RMB - cancels an edit
MMB - accepts or cancels an edit depending on the Setting "acceptedit="

Note that frozen-col cells cannot be directly selected or edited. Additionally,
note that pressing Shift when clicking on a colhead or rowhead never selects a
col or row; instead, Shift-clicking on a colhead or rowhead always selects the
cells in that col or row, and will subselect a range of rows if there is already
another selected row. Pressing Ctrl, or Ctrl+Shift, when clicking a colhead or
rowhead lends yet more permutations. But it's really not that complicated: just
start clicking with or without Ctrl and Shift ... technically you're dealing
with three items: cells, rows, and cols.

OPERATIONS THAT REFORMAT LARGE TABLES (tens of thousands of Rows) TAKE TIME.
Example: loading or changing the table-font of Placeables.2da with ~25,000 rows
takes ~20 seconds on my decently fast computer. Fortunately this has been
reduced to ~4 seconds if a table has a lot of **** fields.


4. Settings.Cfg file (do not use double-quotes)

The file Settings.Cfg is not distributed; the package contains only the
executable and help files. see Help|Options file. If Settings.Cfg is not found
in the application directory you're asked if you want to create one; if the file
is found an internal texteditor opens where edits can be done. If I hardcode
additional settings for a new release a button appears in the lower left corner
of the editor that adds any variables that are not found to the text.

Note that your OS could throw a hissy fit depending on its security settings. If
so you have two options: install Yata to a directory that you have read/write
privileges for, or create and/or edit Settings.Cfg by hand in your favorite
text editor.

Any change to settings requires a restart.

the following variables ought be respected:

font=        a .NET string that represents the desired table-font (see Font->
             Font ... be patient)
font2=       a .NET string that represents a desired (usually smaller) font for
             Menus (Yata needs to be reloaded before it will display a changed
             Menu font)
font3=       a .NET string that represents a desired font for the PropertyPanel
             (Yata needs to be reloaded before it will display a changed
             PropertyPanel font)
fontf=       a .NET string that represents a desired fixed-width font (Yata
             needs to be reloaded before it will display a changed fixed-width
             font)
fonti=       a .NET string that represents a desired font for the Infobox (Yata
             needs to be reloaded before it will display a changed Infobox font)
x=           (integer) the desired x-position to start the app on your monitor
y=           (integer) the desired y-position to start the app on your monitor
w=           (integer) the desired starting width of the app on your monitor
h=           (integer) the desired starting height of the app on your monitor
maximized=   "true" (without quotes) to start Yata in a maximized window. A true
             setting takes precedence over the x/y/w/h settings although the
             latter are recalled if the window is restored
acceptedit=  when editing a cell Yata's default behavior is to reject an edit
             when the editbox loses focus unless the edit is explicitly accepted
             by pressing [Enter] or leftclick on or around the grid or by any of
             the TabFastedit keys. "true" (without quotes) inverts this behavior
             so that any edit to the celltext will be accepted when the editbox
             loses focus unless an edit is explicitly rejected by pressing [Esc]
             or by a rightclick outside the grid or on the FrozenPanel. Note
             that closing a table (or quitting Yata) with an editbox visible
             always discards the edit (without warning about changed text)
alignoutput= "true" (without quotes) to align the cols of 2da files with spaces;
             "tabs" (without quotes) to align cols with tabs. Note that using
             tab-characters in 2da files is not officially supported and could
             break in other applications. "electron" (without quotes) to style
             output like the Electron toolset (for version control if req'd)
allowdupls=  "true" (without quotes) to allow Yata to open multiple instances of
             the same file. By default Yata will activate the tab that already
             has the file open, but this allows you to keep another instance
             open for visual comparison when doing particularly intricate 2da
             changes (without first making a copy of the file on your hardrive
             and opening that). Note that if the changed instance is saved the
             FileWatcher will ask what you want to do with the other
             instance(s). SaveAll will be disabled while multiple instances of a
             file are open because Yata doesn't know which instance you want to
             save as the current version. WARNING: Enable this at your peril
             because it will likely get confusing to have two or more instances
             of one 2da-file open simultaneously
autorder=    "true" (without quotes) to automatically reorder row-ids after row
             and cell alterations. Note that since ordering row-ids is not
             tracked by Undo/Redo the Changed asterisk will not be cleared when
             undoing or redoing to the table's saved state; also note that the
             PropertyPanel ignores this rule in case you want to force a row to
             have a specific id for whatever reason
casesort=    "true" (without quotes) for case-sensitive sorting
clearquotes= "true" (without quotes) to clear quotes around celltext that does
             not contain whitespace
codepage=    (integer) see Appendix N: Codepages
context=     a right-click on a rowhead displays the contextmenu at the mouse-
             cursor's location by default. It can be displayed in a static
             location at the topleft corner of the table instead of at the
             mouse-cursor by giving this variable a value of "static" (without
             quotes)
dialog=      a path without quotes to your Dialog.Tlk file. Strrefs can often
             print their string values to the statusbar if Dialog.Tlk has been
             pathed
dialogalt=   as "dialog=" but for a custom talktable
diff=        a path without quotes to your WinMerge executable for diffing and
             merging two 2da files (if desired). See Appendix L: WinMerge
dirpreset=   a path without quotes to a valid directory for the
             Open ... @ folder dialog
dirpreset=   another path for the Open ... @ folder dialog
dirpreset=   etc. (there can be as many or as few dirpresets as you like)
gradient=    "true" (without quotes) to draw the colhead bar with gradient
             colors
instantgoto= "true" (without quotes) causes the current table to select a row as
             digits are typed in the goto-box. If false [Enter] needs to be
             pressed to select a row after digits are typed
pathall=     a path without quotes to a valid directory to grope for 2da info
             for Crafting.2da, Spells.2da, Feat.2da, Classes.2da, or
             BaseItems.2da (see Appendix E: how to use Info paths)
pathall=     another path for Crafting, Spells, and Feat info
pathall=     etc. (the first pathall has lowest priority and any info found will
             be replaced by any info found in subsequent pathall directories;
             there can be as many or as few pathall directories as you like)
pathzipdata= a path without quotes to the stock NwN2 Data installation directory
             to grope for 2da info for Crafting.2da, Spells.2da, Feat.2da,
             Classes.2da, or BaseItems.2da (see Appendix E: how to use Info
             paths) Note that this path will also be used as the default
             Select Data/zip file directory when a cell's InfoInput context
             allows input from the NwN2 Data/zip resources
recent=      (integer) a count of recently opened file-paths to store. If left
             blank or a value less than 1 is specified, recently opened files
             will not be tracked, while 16 is the hardcoded upper limit. SETTING
             "recent=" TO A VALID VALUE ENABLES YATA TO WRITE THE FILE
             Recent.Cfg TO ITS APPLICATION DIRECTORY. Recent.Cfg stores
             filepaths (without quotes). THE WRITE-OPERATION CAN FAIL for a
             variety of reasons that I really don't want to be arshed with -
             hence the option so you can decide if it works on your OS
strict=      "true" (without quotes) to show extra warnings when loading a
             2da-file (default false). Strict is intended for users who want to
             notice stuff that is by and large safe to disregard:
             (1) warn about the existance of tab-characters if "alignoutput=" is
                 not set to "tabs". Note that tabs are generally safe for NwN2
                 but perhaps not for NwN ... there is at least one app for NwN2
                 that fails if tabs delimit the colhead labels (the Players
                 Handbook recompiler tool)
             (2) warn about suspicious (non-ASCII) characters in colhead labels.
                 When labeling a colhead "strict=true" allows only alpha-numeric
                 characters and the underscore character, otherwise any
                 printable ASCII character except the double-quote is allowed.
                 Note that Yata supports 2da-files with Unicode in their colhead
                 labels - but if you want to edit such labels use a decent
                 texteditor (or try a different 2da-editor)
             (3) warn about a garbage Default value on the 2nd line of a
                 2da-file. This will be autocorrected if the file is saved
                 regardless of the "strict=" setting
             (4) warn about a tab-character in the version header instead of a
                 space-character. This will be autocorrected if the file is
                 saved regardless of the "strict=" setting
             (5) "true" will also cause Yata to try to flag the table as Changed
                 as a notice that saving the table would result in output that
                 is different than the input file. But this is not rigorous.
                 Note that any silent changes shall be innocuous (eg. trimming
                 whitespace from the ends of row-lines, or clearing quotes from
                 celltexts that do not contain whitespace iff
                 "clearquotes=true"); setting the Changed flag like this is a
                 way to tell purists that a file is not quite at peak efficiency
             (6) and "true" also suppresses the tooltip that appears when a col
                 is sorted by anything other than ID-ascending ("warn : Table is
                 not sorted by ascending ID") - ie, persons who use
                 "strict=true" don't get a tooltip although the head of the
                 id-col still turns to a red color regardless of this setting
             (7) when applying celltext that has whitespace but is not double-
                 quoted Yata will add the quotes and issue a notice that the
                 text has been changed if "true"; otherwise Yata will add
                 double-quotes without bothering the user.
             (8) when applying celltext that does not have whitespace but is
                 double-quoted Yata will clear the quotes if "clearquotes=true"
                 and issue a notice that the text has been changed if "true";
                 otherwise Yata will clear the quotes without bothering the
                 user.

The dirpresets appear on the File menu (if specified) and are a quick way to
show an open-file-dialog at your frequently used directory(s).

The pathall directories are for gathering Info that will appear on the statusbar
if Crafting.2da, Spells.2da, Feat.2da, Classes.2da, or BaseItems.2da are loaded
as the cursor is moved over their cells. (Yata was designed with Crafting in
mind and can show stuff like Encoded IPs as readable strings on the statusbar,
eg.)

To bypass a setting without deleting it, prefix its line with any character you
want. The parser considers only lines that begin with the string-variables
above; any line that doesn't begin exactly with one of those strings is ignored.

The order of the settings in the file is arbitrary.

If the Settings.Cfg file does not exist Yata should run okay with its default
settings.


Appendix A: note on Load

Yata is pretty strict when loading a 2da file. If it detects anything awkward it
lets the user know and will try to automatically fix cell-fields. Such cells
should then appear highlighted with pink - these are called "loadchanged" cells,
since they were changed when the 2da was loaded. An option under the Cells menu
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
a green field at the far left of a row) and then the Shift key can be used to
select a range of rows. (Using the Control key to select multiple rows does not
work for this.)

A range of copied rows can be pasted/inserted at a currently selected row (as
indicated with a green field at the far left of a row) or to the end of the
table if no row is currently selected. Pasting a range does not overwrite any
row(s) - it rather inserts the copy-range, starting at the selected row.


Appendix C: edit operations on rows

Since editing by row(s) is not intuitive I should say a few words here.

Edits that involve only one row ought be straightforward; right-click on the
row at the far left and a popup will appear, and that row will be selected. All
edit-operations that are possible on a single row are shown on the popup.

Edits that involve more than one row are more complicated. First you need to get
familiar with these facts: (a) only one row can ever be currently selected (as
indicated with a green field at the far left of that row), (b) to flag multiple
rows for cutting/copying or deleting w/ Shift+Delete hold down Shift and click
at the far left of another row - Control will not work for this, although such
row(s)'s cells would appear to be selected; a row of selected cells is not
necessarily/technically a selected or flagged row itself! (c) to cut/copy/delete
a range of rows (a selected row along with its flagged rows) use the Rows menu
or its keyboard shortcuts.

Currently flagged rows are indicated with a pale green field at the far left of
their rows.

Cutting/copying/pasting rows does not use the Windows Clipboard. The copy of
such data is instead maintained internally by Yata. If you want this data on the
Clipboard for whatever reason choose "Export copied row(s)" on the Clipboard
menu. To get data that's on the Clipboard back into Yata's internal format
choose "Import clipboard row(s)" on the Clipboard menu - but be warned that such
data from the Clipboard is not checked and could be garbage as far as a 2da is
concerned, so it's up to you to decide whether or not to proceed at that point.
The contents of the Clipboard can be viewed and edited by choosing "Open clip
editor" on the Clipboard menu.

Once a range of rows has been copied it can be pasted into a table by choosing
"Paste" on the Rows menu. The first row will be pasted at the currently selected
row, pushing that row along with all following rows down such that they get
shifted beneath the range that is pasted. Note that pasting rows never replaces
any row(s). To replace rows with a copied range of rows, paste your copied rows
then delete the rows you want deleted, or vice versa. And to paste rows after
the last row, choose "Paste" without having a currently selected row - pressing
Escape will clear all selections of rows, cols, and cells.


Appendix D: output

Yata outputs 2da-files as UTF-8 encoded textfiles. It uses a single space (by
default) for the delimiter. It can however output files with aligned cols if
"alignoutput=" is set to "true" or "tabs" in Settings.Cfg - but neither is
recommended since tabs can break other applications while spaces will inflate
the filesize.

A third option "electron" can be specified if you want output that is identical
to what is generated by NwN2's Electron toolset. However this setting should be
used only if you want to maintain whitespace-compatibility when working with
version control.


Appendix E: how to use Info paths

IMPORTANT - Info (on the statusbar) and InfoInput dialogs are hardcoded to work
with 2das that ship with or are compliant with those that ship with NwN2 only.
NwN and NwN:EE users can get incorrect results. Info and InfoInput are not
waranteed (sic) for NwN or NwN:EE.

Yata is capable of displaying readable info about fields in Crafting.2da,
Spells.2da, Feat.2da, Classes.2da and BaseItems.2da. Paths to various other
2da-files need to be set first, then info ought be displayed on the statusbar
when the mouse-cursor is moved over the cells of certain cols like "CATEGORY"
(displays the label of a trigger-spell) or "EFFECTS" (displays a recipe's
itemproperty in a readable way) in Crafting.2da, etc. Note that pathing to
2da-files can also be termed, groping ... that is, when a 2da-file is pathed it
will be groped for relevant info.

There are three ways to get such info: (a) Using the Paths menu when
Crafting.2da, Spells.2da, Feat.2da, Classes.2da, or BaseItems.2da is loaded, (b)
Using "pathall=" entries in Settings.Cfg,* (c) Using the "pathzipdata=" entry in
Settings.Cfg.*

* 2da-info will not be groped until one of Crafting.2da, Spells.2da, Feat.2da,
Classes.2da, or BaseItems.2da is opened in Yata. After that the info will be
retained unless reset by using the Paths menu.

(a) Using the Paths menu

Paths appears on the menubar only when Crafting.2da, Spells.2da, Feat.2da,
Classes.2da, or BaseItems.2da are loaded - the filename without extension needs
to be "crafting", "spells", "feat", "classes", or "baseitems"
(case-insensitive).

Path all ... : this item opens a folder browser dialog to search for any/all
               applicable 2da-file(s). It can be used more than once and if so,
               each time another folder is selected any applicable 2da-file(s)
               will be additionally groped for info. In the case of two files
               with the same filename pathed in two different directories info
               from the latest groping will be used.

Other items that appear under Paths depend on the 2da that is currently active.
The items open a file browser dialog. Use them to path to a specific 2da-file. A
check will appear next to the entry on the menu if the file was successfully
groped; clicking on a checked item clears its info.

Crafting.2da

path BaseItems.2da       : these files contain basic Info for Crafting.2da
path Feat.2da
path ItemPropDef.2da
path Skills.2da
path Spells.2da

path Classes.2da         : these files contain Info that is used to decode
path Disease.2da           EncodedIPs for Crafting.2da. Note that
path Iprp_AmmoCost.2da     ItemPropDef.2da above interprets the base
path Iprp_Feats.2da        itemproperty; these files contain further details of
path Iprp_OnHitSpell.2da   the itemproperty. Raw values are displayed if these
path Iprp_Spells.2da       files have not been groped.
path RacialTypes.2da

Spells.2da

path Categories.2da      : these files contain Info for Spells.2da
path Classes.2da
path Feat.2da
path Ranges.2da
path Spells.2da
path SpellTarget.2da

Feat.2da

path Categories.2da      : these files contain Info for Feat.2da
path Classes.2da
path CombatModes.2da
path Feat.2da
path MasterFeats.2da
path Skills.2da
path Spells.2da

Classes.2da

path Feat.2da            : these files contain Info for Classes.2da
path Packages.2da

BaseItems.2da

path BaseItems.2da       : these files contain Info for BaseItems.2da
path Feat.2da
path InventorySnds.2da
path ItemProps.2da
path WeaponSounds.2da
path AmmunitionTypes.2da

(b) Using "pathall=" entries in Settings.Cfg

You can specify one (or more) "pathall=" directories in Settings.Cfg (without
quotes), and each will operate just like "Path all ..." does under the menubar.

eg.
pathall=C:\Users\User\Documents\Neverwinter Nights 2\override\2da
pathall=C:\Users\User\Documents\Neverwinter Nights 2\override\2da\classes

(subdirectories are *not* recursed)

Please note that this path-feature in Yata does not look inside zipped archives.
This is to say that the 2da-files that are part of the stock installation are
invalid targets; the files would need to be copied out of the /Data folder to a
different directory (ie, as individual unarchived/uncompressed 2da-files).

If you have custom versions of 2da-files they should be groped *after* the stock
files.

Note that Info can appear on the statusbar for various cols even if 2das have
not been groped; this is because standard info has been hardcoded in Yata.
Similarly, the InfoInput dialog (rightclick on a cell in any of Crafting.2da,
Spells.2da, Feat.2da, Classes.2da, or BaseItems.2da) might also be accessible
without any informational 2das having been groped.

Also note that the info from groped 2das is consistent across all loaded and
relevant 2das. Eg, if path Spells.2da is checked when Crafting.2da is loaded
then that info is used - its menuitem is checked - for Feat.2da also. Yata
retains the info from each groped 2da until it exits or the user dechecks a
path.

(c) Using the "pathzipdata=" entry in Settings.Cfg

As of Yata 5.3.0.0 the stock 2das that ship with NwN2 can also be groped. You
can specify a "pathzipdata=" directory in Settings.Cfg (without quotes).

eg.
pathzipdata=c:\Neverwinter Nights 2\Data

Routines from a 3rd party codec library, SharpZipLib, have been hardcoded in
Yata to extract info from the NwN2 data zips: 2DA.zip, 2DA_X1.zip, 2DA_X2.zip
(the OC, MotB, SoZ respectively). If "pathzipdata=" points to a valid directory
Yata will try to grope any relevant 2da-files; data that's found in later
expansions takes priority over the earlier ones.

Note that info groped from a "pathzipdata=" directory always has lower priority
than info found in "pathall=" directories.

Also note that Yata uses only very sparse code from SharpZipLib - the intent is
to read only the Zipfiles that ship with NwN2. So it probably wouldn't be wise
to package your own Zipfiles and expect Yata to read them ...


Appendix F: creating a 2da-file

Yata can create a new 2da-file from scratch w/ File|Create .... If you want to
create a new 2da-file yourself, at a minimum you should use a text-editor to
specify the 2da version header, followed by a blank line, followed by a line
with a space and a colhead label - look at the top of any valid 2da, in a
text-editor, to see what those first three lines should look like. Yata ought
then be able to open the 2da and assign a first blank row with default
cell-values.


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
open in the earliest instance. This is a .NET/Windows Explorer/Windows OS
limitation.

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

Yata can display the content of a selected row, or of a row with selected
cell(s), vertically in a panel that initially appears at the right. Click the
button - it has a treelike graphic - that's just beneath the Close button on
Yata's title bar to open and close the Property Panel.

Cell-values can be edited in the Property Panel by left-clicking a value field.
To accept an edit, press Enter or left-click on the panel; to cancel an edit,
press Escape or right-click on the panel. Note that clicking elsewhere causes
the panel to lose focus and cancel your current edit.

Advanced: Note that it is possible to change the ID-value of a row in the
Property Panel. It is recommended however to use 2da Ops|Order row ids instead
unless you have special reason not to. The value of a currently frozen col's
cell can also be changed this way (but, note that such a value could be
changed by unfreezing the col and editing the cell directly).

A right-click on the Property Panel button (or the Property Panel itself) will
cycle the panel's docked location through the four corners of the page in a
clockwise direction, or counter-clockwise if [Shift] is depressed. If the height
of the panel is greater than the height of the page, the panel simply shifts to
the left or right side instead.


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
re-selecting diff1 causes diff2 to be cleared; diff2 must be selected after
diff1. Reloading or sorting a 2da causes its diff to be cleared.

Select diff2 causes four noticable things to happen:
1. cells with texts that differ will be rendered with a teal background color
2. the tables' cols will be justified automatically
3. the tables will scroll in unison if possible
4. the DifferDialog will appear. It lists any differences between col headers
   and row counts. Additionally a Goto button in the lower left corner of the
   dialog can be used to cycle through cell texts that are different between the
   two tables (key Shift to goto previous, key Control to retain focus on the
   dialog). Note that closing the dialog with its Okay button (or its Close icon
   or the [Esc] key) does not reset diffs; the tables will still be sync'd and
   the backgrounds of any diff'd cells will still be colored teal unless cleared
   either by the Reset button or via the tab context.

A right-click on a diffed cell shows a popup with several operations including
these two:

- merge to other - Ce (copies the text in the selected cell to the other table)
- merge to other - Ro (copies the texts of the row of the selected cell to the
                       other table)

The selected cell's position (x/y) must be present in both tables. Rows do not
have to be the same length; any cells that overflow will be filled with ****.

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


Appendix N: Codepages

UTF-8 and ASCII encoded textfiles ought be interpreted by .NET easily. But if
text was encoded with any of the extended-ASCII (aka ANSI) charactersets then it
can become an issue since there is no way to rigorously determine what encoding
was used. Yata uses a very basic detection routine - it decodes text as UTF-8
and searches for � - if that character is found then a dialog appears asking
what codepage should be used to interpret the 2da's encoding. If a valid
codepage is specified in Settings.Cfg then that codepage will appear in the
dialog; if not then your machine's default encoding/characterset will appear in
the dialog instead.

In either case the desired codepage for loading a 2da file can be specified in
that dialog.

Note that Yata always encodes output as UTF-8.
