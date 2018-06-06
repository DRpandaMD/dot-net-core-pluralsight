using System;
using System.Runtime.InteropServices;

public class AppKitHelper 
{
    [DllImport("libobjc.dylib")]
    static extern IntPtr dlopen(string path, int flags);

    [DllImport("libobjc.dylib")]
    static extern IntPtr objc_getClass(string name);

    [DllImport("libobjc.dylib")]
    static extern IntPtr sel_registerName(string name);


    [DllImport("libobjc.dylib")]
    static extern IntPtr objc_msgSend(IntPtr reciever, IntPtr selector);

    [DllImport("libobjc.dylib")]
    static extern IntPtr objc_msgSend(IntPtr reciever, IntPtr selector, IntPtr arg1);

    [DllImport("libobjc.dylib")]
    static extern IntPtr objc_msgSend(IntPtr reciever, IntPtr selector, IntPtr arg1, IntPtr arg2);

    static IntPtr GetNSString(string str)
    {
        var strHandle = GCHandle.Alloc(str,GCHandleType.Pinned);
        var nsString = objc_msgSend(objc_msgSend(objc_getClass("NSString"),
            sel_registerName("alloc")), sel_registerName("initWithCharacters:length"),
            strHandle.AddrOfPinnedObject(), str.Length);
        strHandle.Free();
        return nsString;
    }

    public static void DisplayAlert(string message)
    {
        dlopen("System/Library/Framework/AppKit.framework/AppKit", 0);
        var nsAlert = objc_msgSend(objc_msgSend(objc_getClass("NSAlert"),
            sel_registerName("alloc")), sel_registerName("init"));
        objc_msgSend(nsAlert, sel_registerName("SetMessageText:"), GetNSString(message));
        objc_msgSend(nsAlert, sel_registerName("runModal"));
    }
}