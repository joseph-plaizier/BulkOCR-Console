# BulkOCR
The intent of this project is that you can select either a folder or multiple image files and this code will process each of them, outputting a .txt file.  I am currently a student and still learning and this was a project to help fill a personal need and to practice.

## To Run
In order to run the console application, 
1. Navigate to: Code\BulkOCR\bin\Debug\net8.0 and download this folder.
2. Find the BulkOCR.exe file and run it.

You can alternatively dowload all the code and run it from Visual Studio.

## Input Expectations
1. The first input in the application will ask which files or directories you'd like processed.  Please enter a full direct path to the image(s) or directory that you'd like to include.  Example: C:\users\Me\Downloads\image1.png or C:\users\Me\Downloads.

Please note the following:
+ If a directory path is entered, sub directories will also be scanned for image files.
+ Accepted images file types include the following: **.jpg**, **.jpeg**, **.tiff**, **.png**

2. The console application will scan the file(s) or directory (including sub directories) for supported file types.  Found files will be output to the console.  The console will then ask if you'd like a .txt report of the text from image extraction.  Please enter y for yes and n for no.

Please note that it is **highly** recommended that if multiple files are going to be processed that reports are saved to .txt files.

3. If you enter n, the text from image extraction will initiate.  Results from the extraction will appear in the console for each file.  Press any key to close the console window.  Please note that once the console window closes, all data from the extraction will be lost.

If you enter y, the console will ask for a directory path to store the .txt files.  Please enter a full directory path where you'd like the .txt files to be saved.  Example: C:\Users\Me\MyImageScans.  

By default, if no directory is provided (just leave it blank and press Enter) the .txt file will be stored in the users downloads folder.

Text from Image extraction .txt reports are stored in a particular file structure.  The root folder is saved with the current date with the format, 'ddMMyyyy'.  Example: 23082024 is August 23, 2024.  

Within this folder are additional folders based on what time the text was extracted from the image file.  Example: 1215 is 12:15 PM, while 123 could be 12:03 PM or 1:23 AM.  

Within each folder, a list of .txt files will be found with the name of the image that was used to extract text from with a dash and the extension type of the image file.  Example: MyPhoto-jpg.txt.

4. Extraction data will appear in the console regardless of whether a .txt report is generated or not.  Once the application completes processing all of the images, press any key to close the console window.
