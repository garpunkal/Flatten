﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flatten
{
    public class Program
    {
        static void Main(string[] args)
        {
            var destination = new DirectoryInfo(Directory.GetCurrentDirectory());

            FlattenFiles(destination, destination);
            CleanUp(destination);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("fin.");
            Console.ResetColor();

            Console.ReadKey();
        }


        static void CleanUp(DirectoryInfo root)
        {
            var removedExtensions = ".txt,.nfo,.jpg,.png";

            foreach (FileInfo fi in GetFiles(root)
                .Where(x => removedExtensions.Contains(x.Extension.ToLower())))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{fi.FullName}");
                Console.ResetColor();

                try
                {
                    File.Delete(fi.FullName);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }
        }

        static void FlattenFiles(DirectoryInfo root, DirectoryInfo destination)
        {
            if (root.FullName != destination.FullName)
                foreach (FileInfo fi in GetFiles(root))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{fi.FullName}");
                    Console.ResetColor();

                    try
                    {
                        File.Move(fi.FullName, destination.FullName + "\\" + fi.Name);
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e.Message);
                        Console.ResetColor();
                    }
                }


            foreach (DirectoryInfo dirInfo in root.GetDirectories())
                FlattenFiles(dirInfo, destination);


            if (root.FullName != destination.FullName)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(root.FullName);
                Console.ResetColor();

                try
                {
                    root.Delete();
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }
            }
        }

        private static FileInfo[] GetFiles(DirectoryInfo root, string filter = "*.*")
        {
            FileInfo[] files = null;

            try
            {
                files = root.GetFiles(filter);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
            catch (DirectoryNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }

            return files;
        }
    }
}