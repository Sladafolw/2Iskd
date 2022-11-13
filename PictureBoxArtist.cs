using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2Iskd
{
    internal class PictureBoxArtist
    {
        private readonly PictureBox pictureBox;
        private Bitmap bitmap;
        private Graphics graphics;
        private readonly Pen pen;
        private Point lastPoint;
        private bool isDrawing;

        public PictureBoxArtist(PictureBox pictureBox, Pen pen)
        {
            this.pictureBox = pictureBox;
            this.pen = pen;
            isDrawing = false;
            bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(bitmap);
            lastPoint = new();
        }

        public void StartDrawing(Point startPoint)
        {
            lastPoint = startPoint;
            isDrawing = true;
            Drawing(lastPoint);
        }

        public void Clear()
        {
            graphics = Graphics.FromImage(bitmap = new Bitmap(pictureBox.Width, pictureBox.Height));
            pictureBox.Image = bitmap;
        }

        public void Drawing(Point toPoint)
        {
            if (!isDrawing)
            {
                return;
            }

            graphics.DrawLine(pen, lastPoint, toPoint);
            pictureBox.Image = bitmap;
            lastPoint = toPoint;
        }

        public void EndDrawing()
        {
            isDrawing = false;
        }

        public void SaveTo(string path)
        {
            bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }

    }
}
