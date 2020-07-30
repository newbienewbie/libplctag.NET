﻿using libplctag;
using libplctag.Generic.DataTypes;
using System;
using System.Net;
using System.Threading;

namespace CSharpDotNetCore
{
    class ExampleGroups
    {
        public static void Run()
        {

            const int TIMEOUT = 5000;

            var myPlc = new AttributeGroup()
            {
                Gateway = "192.168.0.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };

            var myTag = new Tag()
            {
                Name = "MY_DINT_1D",
                ElementSize = DataType.DINT
            };
            
            myTag.Initialize(TIMEOUT);

            myTag.SetInt32(0, 3737);

            myTag.Write(TIMEOUT);

            myTag.Read(TIMEOUT);

            int myDint = myTag.GetInt32(0);

            Console.WriteLine(myDint);
        }

        public void MultiPlcGroup()
        {

            var myPlcA = new AttributeGroup()
            {
                Gateway = "192.168.0.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };

            var myPlcB = new AttributeGroup()
            {
                Gateway = "192.168.0.11",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };

            var tags = new TagGroup();
            var dint1a = tags.CreateTag("MY_DINT1", DataType.DINT, myPlcA);
            var dint2a = tags.CreateTag("MY_DINT2", DataType.DINT, myPlcA);
            var dint1b = tags.CreateTag("MY_DINT1", DataType.DINT, myPlcB);
            var dint2b = tags.CreateTag("MY_DINT2", DataType.DINT, myPlcB);


            var timeout = 1000;
            tags.InitializeAll(timeout);
            tags.ReadAll(timeout);

            var value = dint1a.GetInt32(0);

            Console.WriteLine(value);

        }

        public void GenericTagGroup()
        {

            var myPlcA = new AttributeGroup()
            {
                Gateway = "192.168.0.10",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };

            var myPlcB = new AttributeGroup()
            {
                Gateway = "192.168.0.11",
                Path = "1,0",
                PlcType = PlcType.ControlLogix,
                Protocol = Protocol.ab_eip
            };

            var tags = new TagGroup();
            var dint1a = tags.CreateGenericTag<PlcTypeDINT, int>("MY_DINT1", myPlcA);
            var dint2a = tags.CreateGenericTag<PlcTypeDINT, int>("MY_DINT2", myPlcA);
            var dint1b = tags.CreateGenericTag<PlcTypeDINT, int>("MY_DINT1", myPlcB);
            var dint2b = tags.CreateGenericTag<PlcTypeDINT, int>("MY_DINT2", myPlcB);

            var timeout = 1000;
            tags.InitializeAll(timeout);
            tags.ReadAll(timeout);

            var value = dint1a.Value;

            Console.WriteLine(value);

        }
    }
}