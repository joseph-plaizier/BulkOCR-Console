/// instructions received from https://github.com/charlesw/tesseract and example .sln file at https://github.com/charlesw/tesseract-samples/tree/master
/// Add the Tesseract NuGet Package by running Install-Package Tesseract from the Package Manager Console.
///(Optional) Add the Tesseract.Drawing NuGet package to support interop with System.Drawing in .NET Core, for instance to allow passing Bitmap to Tesseract
///Ensure you have Visual Studio 2019 x86 & x64 runtimes installed (see note above).
///Download language data files for tesseract 4.00 from the tessdata repository and add them to your project, ensure 'Copy to output directory' is set to Always.
///Check out the Samples solution ~/src/Tesseract.Samples.sln in the tesseract-samples repository for a working example.

/// add a folder called "tessdata" to your project
/// copy the applicable language trained data files to the tessdata folder

// TODO: create queue structure to capture multiple image paths
// TODO: use folder paths or multiple images seperated by comma for multiple images

// TODO: create web interface GUI
// TODO: create branches to store dev code, final exectuble version of console, final webpage code for web

//add System and System.Diagnostics references to the project
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection.Metadata;
using Tesseract;
using static System.Console;
using static System.Net.Mime.MediaTypeNames;

namespace BulkOCR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            bool repeat = true;

            WriteLine("\nWhat is the path to the image file(s)?  Directory paths are accepted.");
            string input = ReadLine();
            Queue<Image> paths = ProcessInput(input);

            //ask if the user wants an .txt version of the output
            Write("\nDo you want a .txt file of the conversion to be created? (y/n) ");
            string create = ReadLine();
            create = create.Trim().ToLower().Trim(['e', 's', 'o']);

            string saveTo = String.Empty;
            //ask the user where to save files
            if(create == "y")
            {
                WriteLine("\nPlease enter a directory where the .txt files should be saved.");
                WriteLine("If no directory is entered the default location of the Downloads folders will be used.");
                saveTo = ReadLine();
                //swap all "\" for "/"
                saveTo = saveTo.Replace("\\", "/");
                //remove any quotation marks
                saveTo = saveTo.Replace('"', ' ').Trim();
                
            }

            //start try block
            try
            {
                //set a IDisposable instance with a "using" code block
                //https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/using
                //basic text from image filepath - https://github.com/charlesw/tesseract/blob/master/docs/examples.md
                //foreach (Image image in paths)
                Parallel.ForEach(paths, i =>
                {
                    WriteLine($"\nProcessing image {i.ImagePath}");
                    ProcessImage(i, create, saveTo);
                });
            } //end try
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                Console.WriteLine("Unexpected Error: " + ex.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(ex.ToString());
            }
        } //end main method

        public static Queue<Image> ProcessInput(string input)
        {
            //create queue for image paths
            Queue<Image> paths = new Queue<Image>();

            //process the input so it detects single file or directory
            //swap all "\" for "/"
            input = input.Replace("\\", "/");
            //remove any quotation marks
            input = input.Replace('"', ' ').Trim();

            //determine if it's a directory or a file
            FileAttributes attr = File.GetAttributes(input);
            if (attr.HasFlag(FileAttributes.Directory)) //directory
            {
                //get all image files recusively in the folder
                string[] extensions = { "jpg", "jpeg", "png", "tiff" };
                string[] imageFiles = System.IO.Directory.GetFiles(input, String.Format("*.{0}", extensions), SearchOption.AllDirectories);
                foreach(string image in imageFiles)
                {
                    Image img = new Image(image);
                    paths.Enqueue(img);
                }
            }
            else //single file path
            {
                //create a new image object
                Image image = new Image(input);

                //add the image object to the queue
                paths.Enqueue(image);
            }
            return paths;
        }

        public static void ProcessImage(Image image, string create, string saveTo = "")
        {
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromFile(image.ImagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        var text = page.GetText();
                        Console.WriteLine("Mean confidence: {0}", page.GetMeanConfidence());

                        Console.WriteLine("Text (GetText): \r\n{0}", text);

                        string directory = "";
                        if (create == "y")
                        {
                            directory = image.saveToTxt(text, saveTo);
                        }

                        Console.WriteLine("Text (iterator):");
                        //insert
                        using (var iter = page.GetIterator())
                        {
                            iter.Begin();

                            do
                            {
                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                            {
                                                Console.WriteLine("<BLOCK>");
                                            }

                                            Console.Write(iter.GetText(PageIteratorLevel.Word));
                                            Console.Write(" ");

                                            if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                            {
                                                Console.WriteLine();
                                            }
                                        } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                        if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                        {
                                            Console.WriteLine();
                                        }
                                    } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                            } while (iter.Next(PageIteratorLevel.Block));

                            if (create == "y")
                            {
                                //let user know where files were saved
                                WriteLine($"Output files saved to {directory}");
                            }
                        }

                    }
                }
            }
        }

    } //end class
}
