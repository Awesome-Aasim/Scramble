using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scramble
{
    public partial class Scramble : Form
    {
        public static Timer timer1 = new Timer();
        public static int framenumber = 0;
        public static Random rand = new Random();
        public enum GWL
        {
            ExStyle = -20
        }

        public enum WS_EX
        {
            Transparent = 0x20,
            Layered = 0x80000
        }

        public enum LWA
        {
            ColorKey = 0x1,
            Alpha = 0x2
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        static readonly IntPtr HWND_TOP = new IntPtr(0);

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        const UInt32 SWP_NOSIZE = 0x0001;

        const UInt32 SWP_NOMOVE = 0x0002;

        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;


        [DllImport("user32.dll")]

        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            int wl = GetWindowLong(this.Handle, GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            SetWindowLong(this.Handle, GWL.ExStyle, wl);
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            //SetLayeredWindowAttributes(this.Handle, 0, 128, LWA.Alpha);
        }
        public Scramble()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            InitializeComponent();
            this.TransparencyKey = this.BackColor;
            this.Size = new Size(Screen.FromControl(this).Bounds.Width, Screen.FromControl(this).Bounds.Height);
            this.Closing += new CancelEventHandler(this.Scramble_Closing);
        }
        private void Scramble_Load(object sender, EventArgs e)
        {
            timer1.Tick += new EventHandler(timer1_tick);
            timer1.Interval = 100;
            timer1.Start();
            glitchScreenshot();
        }
        private void Scramble_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
        private void timer1_tick(object sender, EventArgs e)
        {
            if (framenumber % 2 == 0)
            {
                glitchScreenshot();
            }
            else
            {
                pictureBox1.Image = null;
            }
            this.Refresh();
        }

        private void glitchScreenshot()
        {
            timer1.Stop();
            var screen = System.Windows.Forms.Screen.PrimaryScreen;
            var rect = screen.Bounds;
            var size = new Size(rand.Next(screen.Bounds.Width - 1) + 1, rand.Next(screen.Bounds.Height - 1) + 1);

            Bitmap bmp = new Bitmap(rect.Size.Width, rect.Size.Height);
            /*
            var bmp = new Bitmap(size.Width, size.Height);
            // get random coordinates to transfer pixels to bitmap
            var x = rand.Next(bmp.Width);
            var y = rand.Next(bmp.Height);
            // get random coordinates
            var x0 = rand.Next(bmp.Width);
            var y0 = rand.Next(bmp.Height);
            */
            // replace the pixels with that color
            var x = rand.Next(screen.Bounds.Width);
            var y = rand.Next(screen.Bounds.Height);
            var x0 = rand.Next(screen.Bounds.Width);
            var y0 = rand.Next(screen.Bounds.Height);
            while (size.Width + x0 > rect.Size.Width)
            {
                size.Width--;
            }
            while (size.Height + y0 > rect.Size.Height)
            {
                size.Height--;
            }
            Bitmap bmp2 = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bmp);
            Graphics g2 = Graphics.FromImage(bmp2);
            //g.CopyFromScreen(x, y, rand.Next(screen.Bounds.Width), rand.Next(screen.Bounds.Height), size);
            g2.CopyFromScreen(x0, y0, 0, 0, size);
            var rect2 = new Rectangle(0, 0, size.Width, size.Height);
            // Lock the bitmap's bits.  
            BitmapData bmpData = bmp2.LockBits(rect2, ImageLockMode.ReadWrite, bmp2.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride) * bmp2.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            int[] val = { rand.Next(-255, 256), rand.Next(-255, 256), rand.Next(-255, 256), 0 };
            int temp;
            switch (rand.Next(100))
            {
                case 0 | 1 | 2 | 3:
                    for (int counter = 0; counter < rgbValues.Length; counter++)
                    {
                        temp = rgbValues[counter];
                        temp += val[counter % 4];
                        if (temp > 255) temp = 255;
                        if (temp < 0) temp = 0;
                        rgbValues[counter] = (byte)temp;
                    }
                    // Copy the RGB values back to the bitmap
                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
                    bmp2.UnlockBits(bmpData);
                    g.DrawImageUnscaled(bmp2, new Point(x, y));
                    break;
                case 4 | 5 | 6 | 7:
                    for (int counter = 0; counter < rgbValues.Length; counter++)
                    {
                        temp = val[counter % 4] / 2 + 255;
                        if (temp > 255) temp = 255;
                        if (temp < 0) temp = 0;
                        rgbValues[counter] = (byte)temp;
                    }
                    // Copy the RGB values back to the bitmap
                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
                    bmp2.UnlockBits(bmpData);
                    g.DrawImageUnscaled(bmp2, new Point(x, y));
                    break;
                case 8:
                    for (int counter = 0; counter < rgbValues.Length; counter++)
                    {
                        temp = rgbValues[counter];
                        temp += rand.Next(-255, 256);
                        if (temp > 255) temp = 255;
                        if (temp < 0) temp = 0;
                        rgbValues[counter] = (byte)temp;
                    }
                    // Copy the RGB values back to the bitmap
                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
                    bmp2.UnlockBits(bmpData);
                    g.DrawImageUnscaled(bmp2, new Point(x, y));
                    break;
                case 9:
                    for (int counter = 0; counter < rgbValues.Length; counter++)
                    {
                        temp = rand.Next(256);
                        if (temp > 255) temp = 255;
                        if (temp < 0) temp = 0;
                        rgbValues[counter] = (byte)temp;
                    }
                    // Copy the RGB values back to the bitmap
                    System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
                    bmp2.UnlockBits(bmpData);
                    g.DrawImageUnscaled(bmp2, new Point(x, y));
                    break;
                default:
                    break;
            }
            pictureBox1.Image = bmp;
            timer1.Start();
        }
    }
}
