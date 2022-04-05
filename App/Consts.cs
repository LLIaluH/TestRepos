using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace DataHSMforWeb.App
{
    public static class ConstantProvider
    {
        public const short PIECE_ID_LENGTH = 20;
        public const short FINISHER_STAND_NUM = 6;
        public const short FS_SAMPLE_NUM = 180;
    }

    public struct S_L2L2_FinisherData
    {
        [MarshalAsAttribute(UnmanagedType.ByValTStr, SizeConst = ConstantProvider.PIECE_ID_LENGTH)]
        public string pieceId;
        public byte descaler; // bool type is not compatible due to 4 bytes alignment during reading buffer of Oracle Blob    Sevidov 23.10.2020
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FINISHER_STAND_NUM)]
        public float[] pieceLength;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] relativeOffset;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] entryWidth;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] entryTemp;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] entryThick;
        public float edgerLength;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] edgerSampleTime;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] edgerWidth;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] edgerForce;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] edgerTorque;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] edgerSpeed;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FINISHER_STAND_NUM)]
        public int[] startRbl;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FINISHER_STAND_NUM)]
        public int[] stopRbl;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] SampleTimeF1;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] SampleTimeF2;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] SampleTimeF3;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] SampleTimeF4;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] SampleTimeF5;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] SampleTimeF6;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] exitThickF1;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] exitThickF2;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] exitThickF3;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] exitThickF4;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] exitThickF5;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] exitThickF6;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] forceF1;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] forceF2;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] forceF3;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] forceF4;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] forceF5;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] forceF6;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] torqueF1;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] torqueF2;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] torqueF3;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] torqueF4;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] torqueF5;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] torqueF6;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] speedF1;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] speedF2;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] speedF3;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] speedF4;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] speedF5;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] speedF6;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] bendingF1;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] bendingF2;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] bendingF3;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] bendingF4;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] bendingF5;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] bendingF6;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] interStandCool1;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] interStandCool2;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] interStandCool3;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] interStandCool4;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] interStandCool5;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] spInterstandTension1;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] spInterstandTension2;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] spInterstandTension3;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] spInterstandTension4;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] spInterstandTension5;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] dcSpTension;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] exitWidth;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] exitTemp;
        [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = ConstantProvider.FS_SAMPLE_NUM)]
        public float[] exitThick;
    }
}