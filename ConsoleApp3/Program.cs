﻿using ConsoleApp3.Logics.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            FileDownloadBLL obj = new FileDownloadBLL();
            int j = obj.CkycFileDownload();

            Console.WriteLine("Schedular DONE");
            Console.ReadLine();
        }
    }
}
