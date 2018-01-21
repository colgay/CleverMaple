using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
}

public class Win32
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetWindowDC(IntPtr window);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int ReleaseDC(IntPtr window, IntPtr dc);

    [DllImport("gdi32.dll", SetLastError = true)]
    public static extern uint GetPixel(IntPtr dc, int x, int y);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindow(IntPtr hWnd);

    public static Color GetPixelColor(IntPtr hWnd, int x, int y)
    {
        IntPtr hdc = GetWindowDC(hWnd);
        int pixel = (int)GetPixel(hdc, x, y);
        ReleaseDC(hWnd, hdc);

        return Color.FromArgb(255, (pixel >> 0) & 0xff, (pixel >> 8) & 0xff, (pixel >> 16) & 0xff);
    }

    public static Bitmap CopyFromScreen(int posX, int posY, int width, int height, PixelFormat format)
    {
        Bitmap bmp = new Bitmap(width, height, format);
        using (Graphics gr = Graphics.FromImage(bmp as Image))
            gr.CopyFromScreen(posX, posY, 0, 0, bmp.Size);

        return bmp;
    }

    public static bool FindPixelColor(Bitmap src, int posX, int posY, int width, int height, Color color, ref Point point)
    {
        bool result = false;

        int realWidth = posX + width;
        int realHeight = posY + height;

        FastBitmap srcData = new FastBitmap(src);
        srcData.Lock();

        for (int y = posY; y < realHeight; ++y)
        {
            for (int x = posX; x < realWidth; ++x)
            {
                if (srcData.GetPixel(x, y) == color)
                {
                    result = true;

                    point.X = x;
                    point.Y = y;

                    goto EndOfScan;
                }
            }
        }

        EndOfScan:
        srcData.Unlock();

        return result;
    }

    public static bool FindCharacterName(Bitmap bmp, ref Point point)
    {
        bool result = false;

        int moveWidth = bmp.Width - 51;
        int moveHeight = bmp.Height - 8;

        FastBitmap bmpData = new FastBitmap(bmp);
        bmpData.Lock();

        for (int y = 0; y < moveHeight; ++y)
        {
            for (int x = 0; x < moveWidth; ++x)
            {
                if (IsCharacterName(x, y, bmpData))
                {
                    // match!
                    result = true;

                    point.X = x;
                    point.Y = y;

                    goto EndOfScan;
                }
            }
        }

        EndOfScan:
        bmpData.Unlock();
        
        return result;
    }

    public static Bitmap CropImage(Bitmap source, Rectangle section)
    {
        Bitmap target = new Bitmap(section.Width, section.Height);

        using (Graphics gr = Graphics.FromImage(target))
        {
            gr.DrawImage(source, new Rectangle(0, 0, target.Width, target.Height), section, GraphicsUnit.Pixel);
        }

        return target;
    }
    
    public static bool FindImage(Bitmap src, Bitmap bmp, ref Point point)
    {
        bool result = false;

        int bmpwidth = bmp.Width;
        int bmpheight = bmp.Height;

        int movewidth = src.Width - bmpwidth;
        int moveheight = src.Height - bmpheight;

        FastBitmap srcData = new FastBitmap(src);
        FastBitmap bmpData = new FastBitmap(bmp);

        srcData.Lock();
        bmpData.Lock();

        for (int y = 0; y < moveheight; ++y)
        {
            for (int x = 0; x < movewidth; ++x)
            {
                // Compare the image
                for (int by = 0; by < bmpheight; ++by)
                {
                    for (int bx = 0; bx < bmpwidth; ++bx)
                    {
                        Color color = bmpData.GetPixel(bx, by);
                        Color color2 = srcData.GetPixel(x + bx, y + by);

                        if (color2 != Color.FromArgb(255, color.R, color.G, color.B))
                            goto TakeABreak;
                    }
                }

                result = true;

                point.X = x;
                point.Y = y;

                goto EndOfScan;

                TakeABreak:
                continue;
            }
        }

        EndOfScan:

        srcData.Unlock();
        bmpData.Unlock();

        return result;
    }

    private static bool IsCharacterName(int x, int y, FastBitmap bmpData)
    {
        Color white = Color.FromArgb(255, 255, 255, 255);

        // C
        if (bmpData.GetPixel(x, y) != white
            || bmpData.GetPixel(x + 4, y + 2) != white
            || bmpData.GetPixel(x + 3, y + 7) != white)
            return false;

        if (bmpData.GetPixel(x, y + 1) == white
            || bmpData.GetPixel(x + 4, y + 3) == white
            || bmpData.GetPixel(x + 2, y + 6) == white)
            return false;

        // 5
        if (bmpData.GetPixel(x + 50, y) != white 
            || bmpData.GetPixel(x + 51, y + 5) != white
            || bmpData.GetPixel(x + 49, y + 8) != white)
            return false;

        if (bmpData.GetPixel(x + 50, y + 1) == white
            || bmpData.GetPixel(x + 50, y + 6) == white
            || bmpData.GetPixel(x + 49, y + 7) == white)
            return false;

        return true;
    }
}
