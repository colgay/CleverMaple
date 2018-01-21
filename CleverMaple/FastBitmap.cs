using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class FastBitmap
{
    private Bitmap m_bitmap = null;
    private IntPtr m_pointer = IntPtr.Zero;
    private BitmapData m_bmpData = null;

    public byte[] Pixels { get; set; }
    public int Bytes { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public FastBitmap(Bitmap bitmap)
    {
        this.m_bitmap = bitmap;
    }

    public void Lock()
    {
        try
        {
            // Lock bitmap
            m_bmpData = m_bitmap.LockBits(new Rectangle(0, 0, m_bitmap.Width, m_bitmap.Height), 
                ImageLockMode.ReadWrite, m_bitmap.PixelFormat);

            // Get bytes per pixel
            Bytes = Bitmap.GetPixelFormatSize(m_bitmap.PixelFormat) / 8;

            // Create pixels array
            int byteCount = m_bmpData.Stride * m_bitmap.Height;
            Pixels = new byte[byteCount];

            // Get width and height
            Height = m_bmpData.Height;
            Width = m_bmpData.Width * Bytes;

            // Copy data from pointer to array
            m_pointer = m_bmpData.Scan0;
            Marshal.Copy(m_pointer, Pixels, 0, Pixels.Length);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void Unlock()
    {
        try
        {
            Marshal.Copy(Pixels, 0, m_pointer, Pixels.Length);
            m_bitmap.UnlockBits(m_bmpData);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public Color GetPixel(int x, int y)
    {
        int i = (y * m_bmpData.Stride) + (x * Bytes);

        byte b = Pixels[i];
        byte g = Pixels[i + 1];
        byte r = Pixels[i + 2];
        byte a = Pixels[i + 3];

        return Color.FromArgb(a, r, g, b);
    }
}