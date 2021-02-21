using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Reflection;
//using System.Diagnostics;

namespace SmallSimpleSerialConsole //The name for this was "CringeRetardSerialConsole" at some point but I couldn't upload that to GitHub :)
{
    class Program
    {
        static string[] rates = { "2400", "4800", "9600", "14400", "19200", "38400", "57600", "115200" };
        static int recv_delay = 100;
        static ConsoleColor default_color_fg = Console.ForegroundColor;
        static ConsoleColor default_color_bg = Console.BackgroundColor;
        static SerialPort sp = new SerialPort();
        public static string profile_path = AppDomain.CurrentDomain.BaseDirectory + "\\profiles\\";
        static Profile loaded_prf;
        static bool script_recording;
        static void Main(string[] args)
        {
            if(File.Exists(profile_path + "default.sscp"))
            {
                try
                {
                    loaded_prf = Profile.Load(profile_path + "default.sscp");
                    Console.WriteLine("Profile Loaded: " + loaded_prf.Name + ".sscp");
                } catch(Exception e)
                {
                    Console.WriteLine("Error loading the default profile: {0}", e.Message);
                }
            } else
            {
                Console.WriteLine("No default profile found.");
                loaded_prf = new Profile("default");
            }


            Console.Title = "SSSC";
            sp.PortName = loaded_prf.Port_Name;
            sp.BaudRate = loaded_prf.BaudRate;
            sp.Parity = loaded_prf.Parity;
            sp.DataBits = loaded_prf.BitNumber;
            sp.DataReceived += DataRecievedHandler;


            while (true)
            {
                Console.Write("> ");               
                string[] input = Console.ReadLine().Split(' ');

                switch (input[0]) {

                    case "setup":
                        SPsetup();
                        continue;

                    case "exit":
                        Console.WriteLine("Closing port...");
                        if (sp.IsOpen)
                        {
                            sp.Close();
                        }
                        Console.WriteLine("Exiting program, bye!");
                        Environment.Exit(0);
                        break;

                    case "close":
                        System.Console.WriteLine("Closing port...");
                        if(sp.IsOpen) { sp.Close(); }
                        Console.WriteLine("Port closed.");
                        continue;
                        
                    case "open":
                        Console.WriteLine("Opening port...");
                        if (!sp.IsOpen) {
                            try
                            {
                                sp.Open();
                            } catch(Exception e)
                            {
                                Console.WriteLine("Error opening port: {0}", e.Message);
                                continue;
                            }
                        }
                        Console.WriteLine("Port Open.");
                        continue;

                    case "save":
                        if(input.Length != 2)
                        {
                            Console.WriteLine("Invalid parameters");
                        } else
                        {
                            Profile.Save(loaded_prf, AppDomain.CurrentDomain.BaseDirectory + "\\profiles\\" + input[1] + ".sscp");
                            Console.WriteLine("Current settings saved as {0}", input[1] + ".sscp");
                            continue;
                        }
                        break;

                    case "load":
                        if (input.Length != 2)
                        {
                            Console.WriteLine("Invalid parameters");
                        }
                        else
                        {
                            string filepath = profile_path + input[1] + ".sscp";
                            if (File.Exists(filepath))
                            {
                                Profile.Load(filepath);
                            } else
                            {
                                Console.WriteLine("Invalid profile name");
                            }
                            continue;
                        }
                        continue;

                    case "clear":
                        Console.Clear();
                        continue;

                    case "record":

                        if (input.Length != 2)
                        {
                            Console.WriteLine("Invalid parameters");
                        }
                        else
                        {
                            Script.RecordScript(input[1]);
                            script_recording = true;
                            Console.WriteLine("Now recording script: {0}", input[1]);
                        }        
                        continue;

                    case "run":
                        if (input.Length != 2)
                        {
                            Console.WriteLine("Invalid parameters");
                        }
                        else
                        {
                            int err = Script.ValidateScript(input[1]);
                            if (err == 0)
                            {
                                Console.WriteLine("Executing {0}...", input[1] + ".sscs");
                            }
                            else
                            {
                                Console.WriteLine("Failed to execute: {0}", err == -1 ? "Script file missing or corrupted" : "Error at line " + err); //Oh yeah I do love the conditional operator, how could you tell?
                            }
                        }
                         
                        continue;
                }

                    if (CheckInput(input, loaded_prf.Format))
                    {
                        SendData(input, input.Length, loaded_prf.Format);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input!");
                    }
          
            }
        }

        static void DataRecievedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(recv_delay); 
            while(sp.BytesToRead > 0)
            {
                Console.WriteLine("Data recieved: {0}",sp.ReadLine());
            }
            Console.Write("> ");
        }

        static bool CheckInput(string[] input, DataFormat DataType)
        {
            foreach (string a in input)
            {
                switch (DataType)
                {
                    case DataFormat.Hex:
                        if (!HelperClass.IsHexByte(a))
                            {
                            return false;
                            }
                        break;
                }
            }
     
            return true;
        }
        static void SendData(string[] hexbytes, int length, DataFormat DataType)
        {
            switch(DataType) {
                case DataFormat.Hex:
                    byte[] send = new byte[length];
                    for (int i = 0; i < length; i++)
                    {
                        send[i] = Convert.ToByte(hexbytes[i], 16);
                    }
                    try
                    {
                        sp.Write(send, 0, length);
                    } catch (Exception e)
                    {
                        Console.WriteLine("Error sending data: {0}", e.Message);
                    }
                    break;
            }
        }

        static void SPsetup()
        {
            SelectionList sel_port = new SelectionList("Select a serial port");
            foreach (var port in SerialPort.GetPortNames())
            {
                sel_port.addOption(port);
            }

            sp.PortName = sel_port.DisplayList().text;

            SelectionList sel_baud = new SelectionList(rates, "Select a baud rate");

            sp.BaudRate = int.Parse(sel_baud.DisplayList().text);
            Console.WriteLine("Opening serial port...");

            try
            {
                sp.Open();
                Console.WriteLine("Port Open.");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error opening port: {0}", e.Message);
                Console.ReadLine();
            }
        }

    }

}
