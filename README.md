# OsawariRip
A ripping tool for the files used by "Ultra Adventure! Go Go - Osawari Island"

This tool can be used to extract the layers that make up most of the animated imagery used by Ultra Adventure! Go Go - Osawari Island (it will be refered to as Osawari for the rest of this readme).
The animated files can be recognised by their extention that is tipicaly a number. How you get the Osawari files is completely up to you.

## Arguments
The following arguments can be supplied in any order:
*  `-i <filename/filepath>` or `--inputfile <filename/filepath>`<br>Mark this file for processing by the tool. The filename should contain the extention in order to work. If the file is in a different folder than where the command window is run from the entire path should be supplied.
*  `-o <directory>` or `--outputdir <directory>` - This will be the folder the extracted files will be placed. Not using this will default to the folder the command console is running from.
*  `-t <type>` or `--type <type>` - The type of image the file contains. Valid types right now:
  *  `test` - Do a test run. Extracted images will be numbered in the order they exist in the source file. This in the default.
  *  `catalog` - Meant for files found in the catalog (often also used for the move animation during quests). Names the base, alternative faces and accent layers.
  *  More comming in the later updates
*  `-f` or `--filter` - Filter out garbage/accent layers such as tranparent canvases, tears and sweat droplets.
*  `-n` or `--no-filter` - Don't filter out garbage/accent layers. This is the default.

## Examples
`osawari some_file.3` or `osawari -i some_file.3`<br>
Do a test run on `some_file.3`. This will extract all files to the folder the command console is working from. Use this if you are unsure what the contents of a file are.<br>This can also be achieved by dragging and dropping the file on the extractor. In this case the images will be extracted to the folder the test file is in.

`osawari -i some_file.3 --filter`<br>
Do a test run on `some_file.3` and remove all garbage/accent files from the listing.

`osawari --inputfile some_file.3 --type catalog -f -o C:\path\to\output\dir`<br>
Extract non garbage/accent files in `some_file.3` to `C:\path\to\output\dir` and name them according to the layer types of catalog images.