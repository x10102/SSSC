using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmallSimpleSerialConsole
{
    [Serializable()]
    class Profile
    {
        public string Name;
        public string Port_Name;
        public int BaudRate;
        public Parity Parity;
        public int BitNumber;
        public bool IsDefault;
        public DataFormat Format;
        //List<List<string>> scripts;

        public Profile(string name, string portname = "COM1", int baud_rate = 9600, Parity parity = Parity.None, int bitNumber = 8, DataFormat format = DataFormat.Hex)
        {
            this.Name = name;
            this.BaudRate = baud_rate;
            this.Parity = parity;
            this.BitNumber = bitNumber;
            this.Format = format;
            this.Port_Name = portname;
        }

        public static Profile Load(string path)
        {
            Profile loaded_profile;

            try
            {
                Stream file_stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryFormatter formatter = new BinaryFormatter();
                loaded_profile = (Profile)formatter.Deserialize(file_stream);
                file_stream.Close();
                return loaded_profile;
            }
            catch (Exception e)
            {
                Console.WriteLine("I/O error while saving profile: {0}", e.Message);
                return null;
            }
            
        }

        public static void Save(Profile pf, string path)
        {
            try
            {
                Stream file_stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(file_stream, pf);
                file_stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("I/O error while saving profile: {0}", e.Message);
            } 
        }
    }
}
