using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using TwainScanning;
using TwainScanning.Collectors;
using TwainScanning.NativeStructs;
using TwainScanning.Capability;
using System.Windows.Forms;

namespace ConsoleSample
{
    class ParseArgs
    {
        public bool showGui = false;
        public bool scan = false;
        public bool selectSourceGui = false;
        public string bmpPath = null;
        public string pdfPath = null;
        public string tiffPath = null;
        public string source = null;
        public bool showSources = false;
        public bool showSupportedCaps = false;
        public bool showCapValues = false;
        public List<string> capValuesToShow = new List<string>();
        public bool displayHelp = false;
        public bool setCapValues = false;
        public List<string> capValuesToSet = new List<string>();

        public ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string a = args[i];
                string next = ((i + 1) < args.Length) ? args[i + 1] : "";
                switch (a)
                {
                    case ("-scan"):
                        {
                            scan = true;
                            break;
                        }
                    case ("-help"):
                        {
                            displayHelp = true;
                            break;
                        }
                    case ("-showGui"):
                        {
                            showGui = true;
                            break;
                        };
                    case ("-selectSourceGui"):
                        {
                            selectSourceGui = true;
                            break;
                        };
                    case ("-bmpPath"):
                        {
                            bmpPath = next;
                            i++;
                            break;
                        };
                    case ("-pdfPath"):
                        {
                            pdfPath = next;
                            i++;
                            break;
                        };
                    case ("-tiffPath"):
                        {
                            tiffPath = next;
                            i++;
                            break;
                        };
                    case ("-source"):
                        {
                            source = next;
                            i++;
                            break;
                        };
                    case ("-showSources"):
                        {
                            showSources = true;
                            break;
                        };
                    case ("-showSupportedCaps"):
                        {
                            showSupportedCaps = true;
                            break;
                        };
                    case ("-showCapValues"):
                        {
                            showCapValues = true;
                            i++;
                            for (; i < args.Length; i++)
                            {
                                capValuesToShow.Add(args[i]);
                            }
                            break;
                        };
                    case ("-setCapValues"):
                        {
                            setCapValues = true;
                            i++;
                            for (; i < args.Length; i++)
                            {
                                capValuesToSet.Add(args[i]);
                            }
                            break;
                        };
                }
            }
        }
    };



    class Program
    {

        static void DisplayHelp()
        {
            Console.WriteLine("options for scanning from defined source: -scan [-source NAME] [-showGui] [-selectSourceGui] [-bmpPath PATH] [-pdfPath PATH] [-tiffPath PATH] [-setCapValues CAP1=VAL1 [CAP2=VAL2] ... ]");
            Console.WriteLine("options to show all source names: -showSources");
            Console.WriteLine("options to show capabilities of specific source: -source NAME -showSupportedCaps");
            Console.WriteLine("options to show capabilities of specific source: -source NAME -showCapValues [CAPNAME1, CAPNAME2]");
            Console.WriteLine("options to show help: -help");
            Console.WriteLine("");
            Console.WriteLine("-scan                             :should it scan");
            Console.WriteLine("-source NAME                      :if defined opens named source, if not opens default");
            Console.WriteLine("-showGui                          :show scanner gui");
            Console.WriteLine("-selectSourceGui                  :show select source gui");
            Console.WriteLine("-setCapValues NAME1=VALUE1 [NAME1=VALUE1]...  :sets capability named NAMEX to value VALUEX, commonly used capabilities are XResolution, YResolution, IPixelType, TransferMethod, ... All capabilities for specific source can be seen by using -showSupportedCaps flag. MUST BE USED AS LAST FLAG");
            Console.WriteLine("-bmpPath PATH                     :sets where to save bitmaps");
            Console.WriteLine("-pdfPath PATH                     :sets where to save pdf");
            Console.WriteLine("-tiffPath PATH                    :sets where to save multipage tiff");
            Console.WriteLine("-showSupportedCaps                :prints all supported capabilities for specific source");
            Console.WriteLine("-showCapValues NAME1 [NAME2] ...  :prints all available values for the specified capability of specific source, and marks current value. MUST BE USED AS LAST FLAG");
            Console.WriteLine("-help                             :displays this message");
        }
        //Show available twain device's.
        static void DisplaySources()
        {
            using (DataSourceManager dsm = new DataSourceManager(IntPtr.Zero))
            {
                //Get identity of default source's.
                try
                {
                    TwIdentity defaultDeviceIdentity = dsm.DefaultSource();
                    foreach (TwIdentity id in dsm.AvailableSources())
                    {
                        if (((string)defaultDeviceIdentity.ProductName) == ((string)id.ProductName))
                            Console.Write("-> ");
                        else
                            Console.Write("   ");
                        Console.Write(id.ProductName);
                        Console.WriteLine();
                    }
                }
                catch (BadRcTwainException ex)
                {
                    Console.Write("Bad twain return code: " + ex.ReturnCode.ToString() + "\nCondition code: " + ex.ConditionCode.ToString() + "\n" + ex.Message);
                }
            }
        }
        //Show capability values.
        static void DisplayCapValues(ParseArgs pargs)
        {
            using (DataSourceManager dsm = new DataSourceManager(IntPtr.Zero))
            {
                using (DataSource ds = dsm.OpenSource(pargs.source))
                {
                    Console.WriteLine("Source: " + (string)ds.Identity.ProductName);
                    foreach (var c in ds.Settings.GetSupportedCapabilities())
                    {
                        if (!pargs.capValuesToShow.Contains(c.Cap.ToString()))
                            continue;

                        if (c.IsReadOnly)
                            Console.Write("Readonly ");
                        Console.WriteLine("Capability: " + c.Cap.ToString());

                        List<string> vals = new List<string>();
                        foreach (var v in c.GetCurrentValuesObj())
                            vals.Add(v.ToString());
                        vals.Reverse();

                        int limit = 10;

                        List<string> possibleVals = new List<string>();
                        foreach (var v in c.GetSupportedValuesObj())
                            possibleVals.Add(v.ToString());

                        if (possibleVals.Count > limit)
                        {
                            possibleVals.RemoveRange(limit / 2, possibleVals.Count - limit);
                            possibleVals.Insert(limit / 2, "...");
                        }

                        foreach (var v in vals)
                            if (!possibleVals.Contains(v))
                                possibleVals.Insert(limit / 2, v);

                        if (possibleVals.Count > limit + 1)
                            possibleVals.Insert(limit / 2, "...");

                        foreach (var v in possibleVals)
                        {
                            if (vals.Contains(v))
                                Console.Write("  -> ");
                            else
                                Console.Write("     ");
                            Console.WriteLine(v);
                        }
                    }
                }
            }
        }

        static void SetCapValues(DataSource ds, ParseArgs pargs)
        {
            Console.WriteLine("Source: " + (string)ds.Identity.ProductName);

            foreach (var ent in pargs.capValuesToSet)
            {
                string[] capVal = ent.Split(new char[] { '=' }, 2);
                string capName = capVal[0];
                string capValue = capVal[1];
                foreach (var c in ds.Settings.GetSupportedCapabilities())
                {
                    if (capName != c.Cap.ToString())
                        continue;
                    foreach (var v in c.GetSupportedValuesObj())
                    {
                        c.SetCurrentValuesStr(new string[] { capValue });
                    }
                }
            }
        }

        static void DisplaySupportedCaps(ParseArgs pargs)
        {
            try
            {
                using (DataSourceManager dsm = new DataSourceManager(IntPtr.Zero))
                {
                    using (DataSource ds = dsm.OpenSource(pargs.source))
                    {
                        if (ds == null)
                        {
                            Console.WriteLine("Unable to open source");
                            return;
                        }
                        Console.WriteLine("Source: " + (string)ds.Identity.ProductName);
                        foreach (var c in ds.Settings.GetSupportedCapabilities())
                        {

                            Console.Write(c.Cap.ToString().PadRight(40));

                            List<object> vals = new List<object>(c.GetCurrentValuesObj());

                            if (c.IsReadOnly)
                                Console.Write("    readonly:");
                            else
                            {
                                List<object> supportedVals = new List<object>(c.GetSupportedValuesObj());
                                if (supportedVals.Count == 1)
                                    Console.Write("single value:");
                                else if (supportedVals.Count == vals.Count)
                                    Console.Write("  all values:");
                                else
                                    Console.Write("       value:");
                            }
                            if (vals.Count > 10)
                            {
                                vals.RemoveRange(5, vals.Count - 10);
                                vals.Insert(5, "...");
                            }
                            foreach (var val in vals)
                            {
                                Console.Write(" " + val.ToString());
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (BadRcTwainException ex)
            {
                Console.Write("Bad twain return code: " + ex.ReturnCode.ToString() + "\nCondition code: " + ex.ConditionCode.ToString() + "\n" + ex.Message);
            }
        }

        static void Main(string[] args)
        {
            ConfigFile configFile = new ConfigFile();
            ConfigFile.SetConfigFromIni(configFile);
            bool isValid = TwainScanning.GlobalConfig.SetLicenseKey(configFile.Company, configFile.LicenseKey);

            TwainScanning.GlobalConfig.Force64BitDriver = false; //False is default, set to true to use 64-bit device drivers in 64-bit applications

            IImageCollector collector = null;
            if (args.Length == 0)
                collector = ScanMinimal();
            else
            {
                var pargs = new ParseArgs(args);
                if (pargs.displayHelp)
                {
                    DisplayHelp();
                    return;
                }
                else if (pargs.showSources)
                {
                    DisplaySources();
                    return;
                }
                else if (pargs.showSupportedCaps)
                {
                    DisplaySupportedCaps(pargs);
                    return;
                }
                else if (pargs.showCapValues)
                {
                    DisplayCapValues(pargs);
                    return;
                }

                if (pargs.scan)
                    collector = Scan(pargs);
            }
            if (collector != null)
                collector.Dispose();

        }

        static ImageCollector ScanMinimal()
        {
            AppInfo info = new AppInfo();
            info.name = "Terminal";
            info.manufacturer = "terminalworks";
            try
            {
                using (DataSourceManager dsm = new DataSourceManager(IntPtr.Zero, info))
                {
                    dsm.SelectDefaultSourceDlg();
                    using (var ds = dsm.OpenSource())
                    {
                        if (ds == null)
                        {
                            Console.WriteLine("Unable to open source");
                            return null;
                        }
                        DataSource.ErrorInfo ei = new DataSource.ErrorInfo();
                        var collector = ds.Acquire(false, true, ei, TwSX.Native);
                        return collector;
                    }
                }
            }
            catch (BadRcTwainException ex)
            {
                Console.Write("Bad twain return code: " + ex.ReturnCode.ToString() + "\nCondition code: " + ex.ConditionCode.ToString() + "\n" + ex.Message);
            }
            return new ImageCollector();
        }



        static IImageCollector Scan(ParseArgs args)
        {

            var collector = new ImageMultiCollector();
            AppInfo info = new AppInfo();
            info.name = "Terminal";
            info.manufacturer = "terminalworks";
            try
            {
                using (DataSourceManager dsm = new DataSourceManager(IntPtr.Zero, info))
                {
                    if (args.selectSourceGui)
                        dsm.SelectDefaultSourceDlg();

                    using (var ds = dsm.OpenSource(args.source))
                    {
                        if (ds == null)
                        {
                            Console.WriteLine("Unable to open source");
                            return null;
                        }
                        SetCapValues(ds, args);

                        ImageCollector imgCol = new ImageCollector();
                        collector.AddCollector(imgCol);
                        //To get output as PDF and Tiff format, imageCollectorPdf and imageCollectorTiff need to be added to imageMultiCollector.
                        if (args.pdfPath != null)
                            collector.AddCollector(new ImageCollectorPdf(args.pdfPath));
                        if (args.tiffPath != null)
                            collector.AddCollector(new ImageCollectorTiffMultipage((string)args.tiffPath));

                        ds.Acquire(collector, args.showGui, true);


                        if (args.bmpPath != null)
                        {
                            imgCol.SaveAllToBitmaps(NameGenerator.DefaultNameGenerator(args.bmpPath));
                        }
                    }
                }
            }
            catch (TwainException ex)
            {
                Console.WriteLine("Something went wrong! " + ex.Message);
            }
            return collector;
        }
    }
}
