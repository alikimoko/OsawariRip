using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace OsawariRip
{
    public class Program
    {

        static string inputFile = "", fullInputFile;
        static string outputDirectory = "";
        static string standardDirectory = Environment.CurrentDirectory;
        static string type = "test";
        static int garbageFiles = 0;
        static bool filter = false;

        static void Main(string[] args)
        {

            // argument checking
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify an input file");
                return;
            }
            
            if (args.Length == 1)
                inputFile = args[0];
            else
            {
                // argument parsing
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = args[i].ToLower();
                    switch (args[i])
                    {
                        case "-i": case "--inputfile": i++; inputFile = args[i]; break;
                        case "-o": case "--outputdir": i++; outputDirectory = args[i]; i++; break;
                        case "-f": case "--filter": filter = true; break;
                        case "-n": case "--no-filter": filter = false; break;
                        case "-t": case "--type": i++; type = args[i]; break;
                        default: Console.WriteLine("Unable to read argument " + args[i]); break;
                    }
                }
            }

            if (inputFile == "")
            {
                Console.WriteLine("Please specify an input file");
                return;
            }
            else
            {
                // file processing
                if (inputFile.Contains('/') || inputFile.Contains('\\'))
                    fullInputFile = inputFile;
                else
                    fullInputFile = standardDirectory + inputFile;
                
                byte[] data = ReadFile();

                if (data != null)
                {
                    if (outputDirectory == "")
                        outputDirectory = standardDirectory;
                    if (outputDirectory[outputDirectory.Length - 1] != '/' && outputDirectory[outputDirectory.Length - 1] != '\\')
                        outputDirectory += "\\";

                    List<byte[]> files = Split(data);

                    if (files.Count == 0)
                    {
                        Console.WriteLine("Invalid file");
                        return;
                    }

                    for (int i = 0; i < files.Count; i++)
                        WriteFile(files[i], i);
                }
            }
        }

        static byte[] ReadFile()
        {
            try
            {
                FileStream file = new FileStream(inputFile, FileMode.Open);
                byte[] data = new byte[file.Length];
                file.Read(data, 0, (int)file.Length);
                file.Close();
                return data;
            }
            catch
            {
                Console.WriteLine("Unable to open file: \"" + inputFile + "\"");
                return null;
            }
        }

        static List<byte[]> Split(byte[] data)
        {
            bool writing = false;
            List<byte[]> files = new List<byte[]>();
            List<byte> file = new List<byte>();

            for (int i = 0; i < data.Length; i++)
            {
                if (writing)
                {
                    file.Add(data[i]);
                    try
                    {
                        if (data[i] == 130 && data[i - 1] == 96 && data[i - 2] == 66 && data[i - 3] == 174 &&
                            data[i - 4] == 68 && data[i - 5] == 78 && data[i - 6] == 69 && data[i - 7] == 73)
                        {
                            writing = false;
                            files.Add(file.ToArray());
                        }
                    }
                    catch
                    {
                        // still at start of file
                    }
                }
                else
                {
                    try
                    {
                        if (data[i] == 137 && data[i + 1] == 80 && data[i + 2] == 78 && data[i + 3] == 71)
                        {
                            writing = true;
                            file = new List<byte>();
                            file.Add(data[i]);
                            file.Add(data[i + 1]);
                            file.Add(data[i + 2]);
                            file.Add(data[i + 3]);
                            i = i + 3;
                        }
                    }
                    catch
                    {
                        // end of file reached
                    }
                }
            }
            
            if (files.Count == 0 || // invalid file
                !filter) // filter not wanted
                return files;
            return Filter(files);
        }

        static List<byte[]> Filter(List<byte[]> files)
        {
            List<byte[]> filtered = new List<byte[]>();
            for (int i = 0; i < files.Count; i++)
                if (files[i].Length > 2048) // garbage images are exactly 170 byte large, exept for one that is not in every file, some margin
                    filtered.Add(files[i]);
            return filtered;
        }

        static void WriteFile(byte[] file, int index)
        {
            string path = outputDirectory;

            if (type == "test")
            {
                path += index + ".png";
            }
            else if (file.Length <= 2048)
            {
                garbageFiles++;
                path += "accent " + garbageFiles + ".png";
            }
            else
            {
                index -= garbageFiles;
                switch (type)
                {
                    case "catalog":
                        if (index == 0)
                            path += "base.png";
                        else
                        {
                            index--;
                            path += "alt face " + index + ".png";
                        }
                        break;
                }
            }

            FileStream output = new FileStream(path, FileMode.Create, FileAccess.Write);
            output.Write(file, 0, file.Length);
            output.Close();
        }
    }
}
