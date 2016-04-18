﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
///using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace RTC
{
    using Manager_t = Int32;
    using Result_t = Int32;
    using Port_t = Int32;
    using RTC_t = Int32;

    public abstract class RTComponent {

        const string rtmadapter_dll = OpenRTM_aist.Manager.rtmadapter_dll;

        [DllImport(rtmadapter_dll, CallingConvention = CallingConvention.Cdecl)]
        private static extern Result_t RTC_addInPort(RTC_t rtc, string name, Port_t port);

        [DllImport(rtmadapter_dll, CallingConvention = CallingConvention.Cdecl)]
        private static extern Result_t RTC_addOutPort(RTC_t rtc, string name, Port_t port);

        [DllImport(rtmadapter_dll, CallingConvention = CallingConvention.Cdecl)]
        private static extern Result_t RTC_deletePort(RTC_t rtc, Port_t port);

        /*
        private static extern Result_t RTC_onAborting_listen(RTC_t rtc, int(*callback)(int));

        private static extern Result_t RTC_onError_listen(RTC_t rtc, int(*callback)(int));

        private static extern Result_t RTC_onReset_listen(RTC_t rtc, int(*callback)(int));
        */

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate Int32 RtcCallback(Int32 ec_id);

        [DllImport(rtmadapter_dll, CallingConvention = CallingConvention.Cdecl)]
        private static extern Result_t RTC_onExecute_listen(RTC_t rtc, RtcCallback callback);

        [DllImport(rtmadapter_dll, CallingConvention = CallingConvention.Cdecl)]
        private static extern Result_t RTC_onActivate_listen(RTC_t rtc, RtcCallback callback);

        [DllImport(rtmadapter_dll, CallingConvention = CallingConvention.Cdecl)]
        private static extern Result_t RTC_onDeactivate_listen(RTC_t rtc, RtcCallback callback);

        RTC_t _r;

        public RTComponent(RTC_t r)
        {
            _r = r;
            RTC_onExecute_listen(_r, onExecute);
        }

        public void addOutPort(PortBase port)
        {
            RTC_addOutPort(_r, port.getName(), port.d());
        }

        public void addInPort(PortBase port)
        {
            RTC_addInPort(_r, port.getName(), port.d());
        }

        public void initialize()
        {
            onInitialize(0);
        }

        public abstract int onInitialize(int ec_id);
        public abstract int onActivated(int ec_id);
        public abstract int onDeactivated(int ec_id);
        public abstract int onExecute(int ec_id);
    }

}