using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using NUCCPL;
using static NUCCPL.NativeMethods;
using static NUCCPL.WindowPosTypes;
using static NUCCPL.SWPFlags;
using static NUCCPL.MKs;
using static NUCCPL.WM;
using System.Threading;

namespace NUCCPL
{

    public partial class FormControl : Form
    {
        IntPtr LyricHandle;
        //IntPtr NetEaseMainHandle;
        List<IntPtr> AllNetEaseWindowHandles = new List<IntPtr>();

        ~FormControl (){
            SetWindowPos(LyricHandle, WindowPosHandles[HWND_NOTOPMOST], 0, 0, 0, 0, (uint)(SWP_NOMOVE | SWP_NOSIZE));
        }
        
        public FormControl()
        {
            InitializeComponent();
        }

        private void FormControl_Load(object sender, EventArgs e)
        {
            Process[] NEPs = Process.GetProcessesByName("cloudmusic");
            foreach(Process p in NEPs)
            {
                
                foreach (var window in GetAllWindowsByPID(p.Id))
                {
                    var ClassNameStr = new StringBuilder("", 2053);
                    GetClassName(window, ClassNameStr, 2050);

                    if (ClassNameStr.ToString().Trim() == "DesktopLyrics")
                    {
                        LyricHandle = p.MainWindowHandle;
                    }
                    else
                    {
                        AllNetEaseWindowHandles.Add(window);
                    }
                }
            }
            timerTopMost.Start();
        }

        private void timerTopMost_Tick(object sender, EventArgs e)
        {
            TopMost = true;
            SetWindowPos(LyricHandle, WindowPosHandles[HWND_TOPMOST], 0, 0, 0, 0, (uint)(SWP_NOMOVE | SWP_NOSIZE));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
            Environment.Exit(0);
        }

        private void btnPrevMusic_Click(object sender, EventArgs e)
        {
            foreach(var Window in AllNetEaseWindowHandles)
            {
                Points p;
                p.x = 45;
                p.y = 745;
                SendMessage(Window, (int)WM_MOUSEMOVE, 0, MakePoints(p));
                var ret = SendMessage(Window, (int)WM_LBUTTONDOWN, (int)MK_LBUTTON, MakePoints(p));
                var lerr = GetLastError();
                Debug.WriteLine(ret);
                Debug.WriteLine(lerr);
                Thread.Sleep(5);
                ret = SendMessage(Window, (int)WM_LBUTTONUP, (int)MK_LBUTTON, MakePoints(p));
                Debug.WriteLine(ret);
                Debug.WriteLine(lerr);
            }
        }
    }
}
