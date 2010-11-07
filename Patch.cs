using System;
using System.IO;

namespace FlashPatch
{
    class Patch
    {
        public static bool apply(string filename)
        {

            var searchString = new Byte[] {0x74, 0x39, 0x83, 0xe8};
            var buff = new Byte[] { 0x90, 0x90 };

            var bytes = File.ReadAllBytes(filename);
            var location = -1;

            // Get the first location where file data matches search bytes
            // please forgive algorithm (or lack thereof), this was written at 2am
            bool found = false;
            for (int i = 0; i < bytes.Length; i++)
            {
                for (int j = 0; j < searchString.Length; j++)
                {
                    found = true;
                    if (bytes[i + j] != searchString[j])
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

            writeBytes(location, buff, filename);

            return true;
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
