using System;
using System.Runtime.InteropServices;

namespace Agent.Core.Misc
{
    public static class Win32Api
    {
        public const Int32 NATIVE_ERROR_ALREADY_EXISTS = 183;

        [DllImport("coredll.dll", EntryPoint = "CreateMutex", SetLastError = true)]
        public static extern IntPtr CreateMutex(IntPtr lpMutexAttributes,
                                                bool InitialOwner,
                                                string MutexName);

        public static bool IsAnotherInstanceRunning(string applicationName)
        {
            IntPtr hMutex = CreateMutex(IntPtr.Zero, true, applicationName);

            if (hMutex == IntPtr.Zero)
                throw new ApplicationException("Failure creating mutex: "
                                               + Marshal.GetLastWin32Error().ToString("X"));

            return Marshal.GetLastWin32Error() == NATIVE_ERROR_ALREADY_EXISTS;
        }

        [DllImport("coredll.dll", SetLastError = true,
            CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]

        public static extern bool CloseHandle(IntPtr hObject);

        public static bool IsApplicationRunning(string applicationName)
        {
            //this doesn't exist in coredll.dll (or in CE?)
            //            IntPtr hMutex = OpenMutex(SYNCHRONIZE, false, applicationName);
            IntPtr hMutex = CreateMutex(IntPtr.Zero, false, applicationName);

            if (hMutex == IntPtr.Zero)
            {
                throw new ApplicationException("Failure creating mutex: "
                                               + Marshal.GetLastWin32Error().ToString("X"));
            }

            bool isRunning = Marshal.GetLastWin32Error() == NATIVE_ERROR_ALREADY_EXISTS;

            CloseHandle(hMutex);

            return isRunning;
        }

        //public static bool ApplicationExists(string applicationName, bool owner)
        //{
        //    //this doesn't exist in coredll.dll (or in CE?)
        //    //            IntPtr hMutex = OpenMutex(SYNCHRONIZE, false, applicationName);
        //    IntPtr hMutex = CreateMutex(IntPtr.Zero, false, applicationName);

        //    if (hMutex == IntPtr.Zero)
        //    {
        //        throw new ApplicationException("Failure creating mutex: "
        //                                       + Marshal.GetLastWin32Error().ToString("X"));
        //    }

        //    return  Marshal.GetLastWin32Error() == NATIVE_ERROR_ALREADY_EXISTS;



        //}
    }
}
