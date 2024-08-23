using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace BulkOCR
{
    internal class Image
    {
        //properties - instance variables
        public string ImagePath { get; set; }
        public string FileName {  get; set; }
        public string Extension { get; set; }


        //constructor
        public Image(string input)
        {
            ImagePath = setFilePath(input);
            FileName = getFileName();
            Extension = getExtension();
        }

        //methods
        private string setFilePath(string imagePath)
        {
            //swap all "\" for "/"
            imagePath = imagePath.Replace("\\", "/");
            //remove any quotation marks
            imagePath = imagePath.Replace('"', ' ').Trim();

            WriteLine(imagePath);
            return imagePath;
        }
        private string getFileName()
        {
            //get the filename
            int extIndex = ImagePath.LastIndexOf('.');
            int divider = ImagePath.LastIndexOf('/');
            int subLength = extIndex - divider;
            string fileName = ImagePath.Substring(divider + 1, subLength - 1);
            WriteLine($"filename: {fileName}");
            return fileName;
        }

        private string getExtension()
        {
            //get the file extension
            int extIndex = ImagePath.LastIndexOf('.');
            string ext = ImagePath.Substring(extIndex + 1);
            WriteLine($"extension: {ext}");
            return ext;
        }

        public string saveToTxt(string text, string saveTo)
        {
            //attempt output of text to filename to downloads folder
            //get the current date
            string day = DateTime.Today.Day.ToString();
            string month = DateTime.Today.Month.ToString();
            string year = DateTime.Today.Year.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string dateDir = $"{day}{month}{year}";
            string timeDir = $"{hour}{minute}";
            string userProfile = "";
            

            if(saveTo == String.Empty || saveTo == "")
            {
                //get user profile path
                userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                saveTo = $"{userProfile}/Downloads";
            }

            //create new directory
            string directory = $"{saveTo}/{dateDir}/{timeDir}";
            Directory.CreateDirectory(directory);

            //save the text file in the new folder
            string file = directory + $"/{FileName}-{Extension}.txt";
            File.AppendAllText(file, text);

            return directory;
        }
    }
}
