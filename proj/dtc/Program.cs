using DTBTool;
using System;
using System.Collections.Generic;
using System.IO;

namespace dtc
{
    class Program
    {
        static DTBTreeRoot Root;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("dtc - DTA compiler / CLI for DTBTool");
                Console.WriteLine("Original frontend and library by GameZelda");
                Console.WriteLine("Modified by donnaken15");
            }
            for (int i = 0; i < args.Length; i++)
            {
                if (File.Exists(args[0]))
                {
                    Root = new DTBTreeRoot();
                    StreamReader streamReader = new StreamReader(args[i], System.Text.Encoding.GetEncoding(1252));
                    try
                    {
                        List<DTAParserWarning> list = DTAReader.ReadRootNode(Root, streamReader);
                        BinaryWriter binaryWriter = new BinaryWriter(File.Create(Path.GetFileNameWithoutExtension(args[i]) + ".dtb"));
                        try
                        {
                            DTBWriter.WriteRootNode(Root, binaryWriter);
                        }
                        catch (DTBException ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        binaryWriter.Close();
                    }
                    catch (DTBException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    streamReader.Close();
                }
                else
                    Console.WriteLine("File not found: "+args[i]);
            }
        }
    }
}
