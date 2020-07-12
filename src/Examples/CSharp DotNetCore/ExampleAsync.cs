﻿using libplctag;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class ExampleAsync
    {
        public static async void Run()
        {
            var myTag = new Tag(IPAddress.Parse("10.10.10.10"), "1,0", CpuType.Logix, DataType.DINT, "PROGRAM:SomeProgram.SomeDINT", 5000);

            while (myTag.GetStatus() == StatusCode.StatusPending)
                Thread.Sleep(100);
            if (myTag.GetStatus() != StatusCode.StatusOk)
                throw new LibPlcTagException(myTag.GetStatus());

            myTag.SetInt32(0, 3737);

            await myTag.WriteAsync();
            if (myTag.GetStatus() != StatusCode.StatusOk)
                throw new LibPlcTagException(myTag.GetStatus());

            await myTag.ReadAsync();
            if (myTag.GetStatus() != StatusCode.StatusOk)
                throw new LibPlcTagException(myTag.GetStatus());

            int myDint = myTag.GetInt32(0);

            Console.WriteLine(myDint);
        }
    }
}