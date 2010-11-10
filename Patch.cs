using System;
using System.IO;

namespace FlashPatch
{
    class Patch
    {
        internal static bool apply(string filename)
        {
            Byte[] searchBytes = new Byte[] { 0x74, 0x39, 0x83, 0xe8 };
            Byte[] replacementBytes = new Byte[] { 0x90, 0x90 };

            var bytes = File.ReadAllBytes(filename);
            var location = -1;

            // Get the first location where file data matches search bytes
            // please forgive algorithm (or lack thereof), this was written at 2am
            bool found = false;
            for (int i = 0; i < bytes.Length; i++)
            {
                for (int j = 0; j < searchBytes.Length; j++)
                {
                    found = true;
                    if (bytes[i + j] != searchBytes[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    location = i;
                    break;
                }
            }

            if (!found)
            {
                return false;
            }

            // Backup
            string backupFilename = filename + "_backup";
            File.Delete(backupFilename);
            File.Copy(filename, backupFilename);

            writeBytes(location, replacementBytes, filename);

            return true;
        }

        internal static bool restore(string filename)
        {
            string backupFilename = filename + "_backup";

            if (File.Exists(filename))
            {
                File.Delete(filename);
                File.Copy(backupFilename, filename);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void writeBytes(int location, byte[] buff, string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            fs.Position = location;

            foreach (var b in buff)
            {
                fs.WriteByte(b);
            }
           
            fs.Close();
        }




    }
}
