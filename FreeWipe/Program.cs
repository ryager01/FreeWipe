using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace FreeWipe
{
    class Program
    {
        /// <summary>
        /// Flags used to empty recycle bin
        /// </summary>
        enum RecycleFlag : int
        {

            SHERB_NOCONFIRMATION = 0x00000001, // No confirmation, when emptying 
            SHERB_NOPROGRESSUI = 0x00000001, // No progress tracking window during the emptying of the recycle bin
            SHERB_NOSOUND = 0x00000004 // No sound when the emptying of the recycle bin is complete
        }

        /// <summary>
        /// System DLL used to empty the recycle bin
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="pszRootPath"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("Shell32.dll")]
        static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, RecycleFlag dwFlags);



        /// <summary>
        /// Application will :
        /// 1. Empty Recycle Bin
        /// 2. Clean Hard Drive
        /// 3. Defrag Hard Drive
        /// 4. Wipe Free Space
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string drive = "C";
            if (args.Length > 0)
                drive = args[0];

            Console.WriteLine("Connecting to " + drive + " Drive");

            if (!Directory.Exists(drive + ":\\"))
            {
                Console.WriteLine("Sorry this drive does not exist.");

            }
            else
            {

                Console.WriteLine("1. Emptying Recycle Bin.");
                /// 1. Empty Recycle Bin
                SHEmptyRecycleBin(IntPtr.Zero, null, RecycleFlag.SHERB_NOSOUND | RecycleFlag.SHERB_NOCONFIRMATION);

                /// 2. Clean Hard Drive
                Console.WriteLine("2. Clean Hard Drive");
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = "cleanmgr.exe";
                pInfo.Arguments = "/d" + drive + " /VERYLOWDISK";
                Process p = Process.Start(pInfo);
                p.WaitForInputIdle();
                p.WaitForExit();

                /// 3. Defrag Hard Drive
                Console.WriteLine("3. Defrag Hard Drive (Not Working)");
                //pInfo.UseShellExecute = false;
                //pInfo.RedirectStandardOutput = true;
                //pInfo.WorkingDirectory = Environment.SystemDirectory;
                //pInfo.Verb = "runas";
                //pInfo.FileName = System.IO.Path.Combine(Environment.SystemDirectory, "defrag.exe");
                //pInfo.Arguments = drive + ": /K /L /U /V /X";
                //p = Process.Start(pInfo);
                //p.BeginOutputReadLine();
                //p.WaitForInputIdle();
                //p.WaitForExit();


                /// 4. Wipe Free Space
                Console.WriteLine("4. Wipe Free Space");
                if (!Directory.Exists(drive + ":\\FreeWipe"))
                {
                    Directory.CreateDirectory(drive + ":\\FreeWipe");
                }
                int Data = 0;
                int fileid = 0;
                int Passes = 3;
                int Pass = 0;
                FileStream file;
                bool done = false;

                string fulldata = "";
                while (Pass < Passes)
                {
                    done = false;
                    Pass++;
                    if (Pass == 2) Data = 1; else Data = 0;
                    fulldata = Data.ToString();
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata; fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata;
                    fulldata = fulldata + fulldata;

                    fileid = 0;
                    while (!done)
                    {
                        fileid++;
                        Console.Clear();
                        Console.WriteLine("Pass " + Pass.ToString() + "/" + Passes.ToString() + " Wipe :" + drive + "\\FreeWipe\\" + fileid.ToString("X8") + ".txt");
                        file = File.OpenWrite(drive + ":\\FreeWipe\\" + fileid.ToString("X8") + ".txt");
                        try
                        {
                            file.Write(Encoding.ASCII.GetBytes(fulldata), 0, Encoding.ASCII.GetBytes(fulldata).Length);
                        }
                        catch
                        {
                            file.Close();
                            done = true;
                        }
                        file.Close();
                    }

                    int purge = 0;
                    while (purge <= fileid)
                    {
                        purge++;
                        Console.Clear();
                        Console.WriteLine("Pass " + Pass.ToString() + "/" + Passes.ToString() + " Purge :" + drive + "\\FreeWipe\\" + fileid.ToString("X8") + ".txt");
                        File.Delete(drive + ":\\FreeWipe\\" + purge.ToString("X8") + ".txt");
                    }
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Press any key to continue");
            Console.WriteLine(Console.ReadKey());
        }
    }
}
