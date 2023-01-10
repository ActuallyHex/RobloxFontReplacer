// if any problems occur when trying to use this please build and run the program with admin perms.

using System;
using System.IO;
using System.Configuration;

string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
string systemFontFolder = @"C:\Windows\Fonts";
string appDataFontsFolder = appDataPath + @"\Microsoft\Windows\Fonts";
string robloxVersion = "";
string[] names = { "arial", "arialbd", "SourceSansPro-Regular", "SourceSansPro-Light", "SourceSansPro-Bold" };
string choice = "0";
string[] robloxVers = Directory.GetDirectories(appDataPath + @"\Roblox\Versions\");

for (int i = 0; i < robloxVers.Length; i++)
{
    string name = robloxVers[i];
    FileInfo fi = new FileInfo(name);
    DateTime creationTime = fi.CreationTime;

    var recent = new DirectoryInfo(appDataPath + @"\Roblox\Versions\").GetDirectories().OrderByDescending(d => d.LastWriteTimeUtc).First();

    string recentString = recent.ToString();

    if (!string.IsNullOrEmpty(recentString))
    {
        recentString = recentString.Replace(appDataPath, "");
        recentString = recentString.Replace(@"\Roblox\Versions\", "");
    }

    robloxVersion = recentString;
}

string robloxFontFolder = appDataPath + @"\Roblox\Versions\" + robloxVersion + @"\content\fonts";

var di = new DirectoryInfo(robloxFontFolder);
di.Attributes &= ~FileAttributes.ReadOnly; // I dont know if this really fixes the "file not accessible" error but it seemed to work for me

while (choice != "-1")
{
    Console.WriteLine("############################");
    Console.WriteLine("#   ROBLOX FONT REPLACER   #");
    Console.WriteLine("############################");

    Console.WriteLine("Change Roblox Font  : 1");
    Console.WriteLine("Reverse Roblox Font : 2");
    Console.WriteLine("List Fonts          : 3");
    Console.WriteLine("Exit               : -1");
    Console.Write("Choice: ");
    choice = Console.ReadLine();

    if (choice == "1")
    {
        Console.Write("System Font Folder (1) or App Data Font Folder (2): ");
        string fontOptions = Console.ReadLine();
        bool valid = false;
        string userFont = "";

        if (fontOptions == "2")
        {
            systemFontFolder = appDataFontsFolder;
            valid = true;
        }
        else if (fontOptions == "1")
        {
            appDataFontsFolder = appDataPath + @"\Microsoft\Windows\Fonts";
            valid = true;
        }
        else
        {
            Console.WriteLine("[!] Invalid Option!");

            Thread.Sleep(100);
 
            Console.Write("System Font Folder (1) or App Data Font Folder (2): ");
            fontOptions = Console.ReadLine();
        }

        Console.WriteLine("Input font name: ");
        userFont = Console.ReadLine();
        Console.WriteLine();
   
        if (Directory.Exists(Path.Combine(appDataPath + @"\Roblox\Versions\" + robloxVersion + @"\backup-fonts")) == false)
        {
            Console.WriteLine("[i] Backing up default fonts");
            Directory.CreateDirectory(Path.Combine(appDataPath + @"\Roblox\Versions\" + robloxVersion + @"\backup-fonts"));

            for (int i = 0; i < 5; i++)
            {
                File.Copy(Path.Combine(robloxFontFolder + @"\" + names[i] + ".ttf"), Path.Combine(appDataPath + @"\Roblox\Versions\" + robloxVersion + @"\backup-fonts\" + names[i] + ".ttf"));
            }

            Console.WriteLine("[i] Successfully backed up default fonts");
        }

        for (int i = 0; i < 5; i++)
        {
            try
            {
                File.Delete(Path.Combine(robloxFontFolder + @"\" + names[i] + ".ttf"));

                File.Copy(Path.Combine(systemFontFolder + @"\" + userFont + ".ttf"), Path.Combine(robloxFontFolder + @"\" + names[i] + ".ttf"));
                Console.WriteLine("[+] Replaced " + names[i] + " with " + userFont);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        Console.WriteLine("[+] All font files replaced successfully! ");
        Thread.Sleep(1000);
        Console.Clear();
    }
    else if (choice == "2")
    {
        if (Directory.Exists(Path.Combine(appDataPath + @"\Roblox\Versions\" + robloxVersion + @"\backup-fonts")) == true)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    File.Delete(Path.Combine(robloxFontFolder + @"\" + names[i] + ".ttf"));
                    File.Copy(Path.Combine(appDataPath + @"\Roblox\Versions\" + robloxVersion + @"\backup-fonts" + @"\" + names[i] + ".ttf"), Path.Combine(robloxFontFolder + @"\" + names[i] + ".ttf"));

                    Console.WriteLine("[+] Successfully restored fonts!");
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            Thread.Sleep(1000);
            Console.Clear();
        }
        else
        {
            Console.WriteLine("[!] Backup folder doesnt exist");
        }
    }
    else if (choice == "3")
    {
        string[] fontNames = Directory.GetFiles(systemFontFolder);
        string[] appdataFontName = Directory.GetFiles(appDataFontsFolder);

        for(int i = 0; i < fontNames.Length; i++)
        {
            string trimmedString = fontNames[i];

            trimmedString = trimmedString.Replace(@"C:\Windows\Fonts\", "");

            Console.WriteLine("System Font : " + trimmedString);
        }

        for(int i = 0; i < appdataFontName.Length; i++)
        {
            string trimmedString = appdataFontName[i];
            trimmedString = trimmedString.Replace(appDataFontsFolder + @"\", "");

            Console.WriteLine("App Data Font: " + trimmedString);
        }
    }

}

Environment.Exit(0);
