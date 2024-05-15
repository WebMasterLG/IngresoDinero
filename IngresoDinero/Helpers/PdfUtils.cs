using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace IngresoDinero.Helpers
{
    public class PdfUtils
    {
        private static int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }

        private static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length - search.Length + repl.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + search.Length));
            }
            return dst;
        }

        public static byte[] RemovePdfMark(byte[] src)
        {
            // Quita la marca de iText
            byte[] producerMetadataBytes = new byte[] { 0x2f, 0x50, 0x72, 0x6f, 0x64, 0x75, 0x63, 0x65, 0x72, 0x28, 0x69, 0x54, 0x65, 0x78, 0x74, 0x53, 0x68, 0x61, 0x72, 0x70, 0x92, 0x20, 0x35, 0x2e, 0x35, 0x2e, 0x31, 0x33, 0x2e, 0x32, 0x20, 0xa9, 0x32, 0x30, 0x30, 0x30, 0x2d, 0x32, 0x30, 0x32, 0x30, 0x20, 0x69, 0x54, 0x65, 0x78, 0x74, 0x20, 0x47, 0x72, 0x6f, 0x75, 0x70, 0x20, 0x4e, 0x56, 0x20, 0x5c, 0x28, 0x41, 0x47, 0x50, 0x4c, 0x2d, 0x76, 0x65, 0x72, 0x73, 0x69, 0x6f, 0x6e, 0x5c, 0x29, 0x29 };
            return ReplaceBytes(src, producerMetadataBytes, new byte[0]);
        }
    }
}
