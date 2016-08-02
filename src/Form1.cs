using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        private string DireccionGuardado;
        private string DireccionAbrir;
        public Bitmap Image;
        public Bitmap Image2;
        public Bitmap Image3;
        public int Acum;
        bool Act;
        int Zoom, ZoomAux;
        int[] histogram_a;
        int[] histogram_r;
        int[] histogram_g;
        int[] histogram_b;
        float[] Fa, Fr, Fg, Fb;


        public Form1()
        {
            InitializeComponent();
            Acum = 0;
            DireccionGuardado = "";
            DireccionAbrir = "";
            Act = false;
            Zoom = 0;
            ZoomAux = 0;
        }

        public void DibujarHistograma() {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            MessageBox.Show(" Espere unos segundos ... ");
            //chart1.Series.Clear();
            histogram_r = new int[256];
            histogram_g = new int[256];
            histogram_b = new int[256];
            histogram_a = new int[256];
            // Calcular la cantidad de colores

            for ( int i=0; i<Image.Width; i++ )
            {
                for ( int j=0; j<Image.Height; j++)
                {
                    Color Pixel = Image2.GetPixel(i,j);
                    histogram_b[Pixel.B]++;
                    histogram_r[Pixel.R]++;
                    histogram_g[Pixel.G]++;
                    histogram_a[Pixel.A]++;
                }
            }
            // Crea Histograma
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            for (int i=0; i < histogram_r.Length; i++)
            {
                chart1.Series["Red"].Points.AddXY(i, histogram_r[i]);
                chart1.Series["Blue"].Points.AddXY(i, histogram_b[i]);
                chart1.Series["Green"].Points.AddXY(i, histogram_g[i]);
            }
                
        }

        public string GetDireccionGuardar()
        {
            return DireccionGuardado;
        }

        public string GetDireccionAbrir()
        {
            return DireccionAbrir;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Transformar
        private void Transformar()
        {
            Image2 = new Bitmap(Image.Width, Image.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb );

            for ( int i=0; i<Image.Width; i++ )
            {
                for ( int j=0; j<Image.Height; j++ )
                {
                    Color Pixel = Image.GetPixel(i, j);
                    Image2.SetPixel(i,j,Pixel);
                }
            }

            Image = CopyBitmap(Image2);
        }
        // Size Image

        private int sizeImage()
        {
            return 0;
        }

        // Guardar
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            try
            {
                SaveFileDialog SFD = new SaveFileDialog();
                SFD.Filter = ".BMP|*.bmp";
                SFD.ShowDialog();
                DireccionGuardado = SFD.FileName;
                Image.Save(DireccionGuardado, System.Drawing.Imaging.ImageFormat.Bmp);

            }catch (System.ArgumentException) { return; } 
        }
        // Cerrar
        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);  
        }
        // Abrir
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        // BMP
        private void imagenBMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Filter = "BMP|*.bmp";
                OFD.ShowDialog();
                DireccionAbrir = OFD.FileName;
                Image = new Bitmap(DireccionAbrir);
                pictureBox1.Image = Image;
                Image2 = CopyBitmap(Image);
                label1.Text = "Ancho: " + (Image.Width).ToString();
                label2.Text = "Alto: " + (Image.Height).ToString();
                label3.Text = "Tamaño: " + (Image.Width * Image.Height).ToString();
                label4.Text = "Profundidad: " + (Image.PixelFormat).ToString();
                Act = true;
                DibujarHistograma();
                if ( Image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb ) {Transformar();}
            }
            catch (ArgumentException)
            {
                return;
            }

        }
        // Otros formatos
        private void imagenJPEGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { 
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "(JPEG, JPG, PNG)|*.jpeg;*.jpg;*.png";
            OFD.ShowDialog();
            DireccionAbrir = OFD.FileName;
            Image = new Bitmap(DireccionAbrir);

            pictureBox1.Image = Image;
            Image2 = CopyBitmap(Image);
            label1.Text = "Ancho: " + (Image.Width).ToString();
            label2.Text = "Alto: " + (Image.Height).ToString();
            label3.Text = "Cantidad de Pixeles: " + (Image.Width * Image.Height).ToString();
            label4.Text = "Profundidad: " + (Image.PixelFormat).ToString();
            textBox1.Text = "0";
            Act = true;
            if (Image.PixelFormat != System.Drawing.Imaging.PixelFormat.Format24bppRgb) { Transformar(); }
            }
            catch (ArgumentException)
            {
                return;
            }
}

        // Ecualización del histograma
        private void button11_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }

            int C = Image2.Width * Image2.Height;
            try
            {
                // Alpha
                Fa = new float[256];
                Fa[0] = 0;
                float aAcum = histogram_a[0];
                for (int i = 0; i < 255; i++)
                {
                    Fa[i] = (aAcum * 255) / C;
                    aAcum = aAcum + histogram_a[i];
                }
                Fa[255] = 255;

                // Red
                Fr = new float[256];
                Fr[0] = 0;
                float rAcum = histogram_r[0];
                for (int i = 0; i < 255; i++)
                {
                    Fr[i] = (rAcum * 255) / C;
                    rAcum = rAcum + histogram_r[i];
                }
                Fr[255] = 255;

                // Green
                Fg = new float[256];
                Fg[0] = 0;
                float gAcum = histogram_g[0];
                for (int i = 0; i < 255; i++)
                {
                    Fg[i] = (gAcum * 255) / C;
                    gAcum = gAcum + histogram_g[i];
                }
                Fg[255] = 255;

                // Blue
                Fb = new float[256];
                Fb[0] = 0;
                float bAcum = histogram_b[0];
                for (int i = 0; i < 255; i++)
                {
                    Fb[i] = (bAcum * 255) / C;
                    bAcum = bAcum + histogram_b[i];
                }
                Fb[255] = 255;

                for ( int i=0; i<Image2.Width; i++ )
                {
                    for ( int j=0; j<Image2.Height; j++ )
                    {
                        Color Pixel = Image2.GetPixel(i, j);
                        Image2.SetPixel(i, j, Color.FromArgb(Pixel.A,Clamp((int)Fr[Pixel.R]),Clamp((int)Fg[Pixel.G]),Clamp((int)Fb[Pixel.B])));
                    }
                }
                pictureBox1.Image = Image2;

            }
            catch (NullReferenceException)
            {
                MessageBox.Show(" No hay un histograma");
            }

        }


        // Deshacer
        private void deshacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            // Restablecer los valores de la interfaz y la acumulada.
            Image2 = CopyBitmap(Image);
            trackBar1.Value = 0;
            label6.Text = "V a l o r : 0 ";
            Acum = 0;
            trackBar2.Value = 1;
            label8.Text = "V a l o r : 0 ";
            pictureBox1.Image = Image;
            ZoomAux = Zoom;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

        }
        // Rehacer
        private void rehacerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            // Aplicar cambios a la Imagen
            Image = CopyBitmap(Image2);
            // Restablecer los valores de la interfaz y la acumulada.
            trackBar1.Value = 0;
            label6.Text = "V a l o r : 0 ";
            Acum = 0;
            trackBar2.Value = 1;
            label8.Text = "V a l o r : 0 ";
            pictureBox1.Image = Image;
            Image2 = CopyBitmap(Image);
            label1.Text = "Ancho: " + (Image.Width).ToString();
            label2.Text = "Alto: " + (Image.Height).ToString();
            label3.Text = "Tamaño: " + (Image.Width * Image.Height).ToString();
            label4.Text = "Profundidad: " + (Image.PixelFormat).ToString();
            textBox1.Text = "0";
            Zoom = ZoomAux;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

        }
        // Salir
        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        // Negativo
        private void button2_Click(object sender, EventArgs e)
        {
            if ( Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            for ( int i=0; i < Image2.Width; i++)
            {
                for ( int j=0; j<Image2.Height; j++)
                {
                    Color Pixel;
                    Pixel = Image2.GetPixel(i, j);
                    Image2.SetPixel(i, j, Color.FromArgb(Pixel.A,255 - Pixel.R, 255 - Pixel.G, 255 - Pixel.B));
                }
            }
            pictureBox1.Image = Image2;
        }
        // Funcion  Comparar
        private int Comparar ( int C, int U)
        {
            if ( C <= U)
            {
                return 0;
            }
            else
            {
                return 255;
            }
        }
        // Compiar bitmap
        protected Bitmap CopyBitmap(Bitmap source)
        {
            try
            {
                return new Bitmap(source);
            }catch(OutOfMemoryException)
            {
                MessageBox.Show("  Out of memory  ");
                Environment.Exit(0);
                return new Bitmap(source);
            }
        }

        // Clamp
        protected int Clamp ( int  P )
        {
            if (P > 255)
            {
                return 255;
            }
            if ( P < 0)
            {
                return 0;
            }
            return P;
        }
        // Umbralizar
        private void button1_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            try {
                int U = Int32.Parse(textBox1.Text);
                int A;
                for (int i = 0; i < Image2.Width; i++)
                {
                    for (int j = 0; j < Image2.Height; j++)
                    {
                        Color Pixel;
                        Pixel = Image2.GetPixel(i, j);
                        A = (Pixel.B + Pixel.G + Pixel.R) / 3;
                        A = Comparar(A, U);
                        Image2.SetPixel(i,j,Color.FromArgb(Pixel.A,A,A,A));
                    }
                }
                pictureBox1.Image = Image2;
            }
            catch (System.FormatException)
            {
                MessageBox.Show(" Error en los parametros ingresados ");
                return;
            }
}

        private void button4_Click(object sender, EventArgs e)
        {
            DibujarHistograma();
        }

        // Espejo Vertical
        private void button5_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            Color aux, color;
            int k;
            k = Image.Height - 1;

            for (int i = 0; i < Image2.Width; i++)
            {
                for (int j = 0; j <= (Image2.Height / 2); j++)
                {
                    aux = Image2.GetPixel(i, j);
                    color = Image2.GetPixel(i, k);
                    Image2.SetPixel(i, j, Color.FromArgb(color.A,color.R,color.G,color.B));
                    Image2.SetPixel(i, k, Color.FromArgb(aux.A,aux.R,aux.G,aux.B));
                    k = k - 1;
                }
                k = Image.Height - 1;
            }

            pictureBox1.Image = Image2;
        }

        // Espejo Horizontal
        private void button6_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            Color aux, color;
            int k;
            k = Image.Width - 1;
            for (int i = 0; i < Image2.Height; i++)
            {
                for (int j = 0; j <= (Image2.Width / 2); j++)
                {
                    aux = Image2.GetPixel(j, i);
                    color = Image2.GetPixel(k, i);
                    Image2.SetPixel(j, i,Color.FromArgb(color.A,color.R,color.G,color.B));
                    Image2.SetPixel(k, i,Color.FromArgb(aux.A,aux.R,aux.G,aux.B));
                    k = k - 1;
                }
                k = Image.Width - 1;
            }

            pictureBox1.Image = Image2;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label6.Text = "V a l o r : " + trackBar1.Value.ToString();
        }

        // Aceptar Brillo
        private void button7_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            Acum = Acum + trackBar1.Value;  // Acumular los valores del Trackbar
            for (int i = 0; i < Image2.Width; i++)
            {
                for (int j = 0; j < Image2.Height; j++)
                {
                    Color Pixel = Image2.GetPixel(i,j);
                    Image2.SetPixel(i, j, Color.FromArgb(Pixel.A,Clamp(Pixel.R + Acum),Clamp(Pixel.G + Acum), Clamp(Pixel.B + Acum) ) );
                }
            }
            // Restablecer
            pictureBox1.Image = Image2;
            trackBar1.Value = 0;
            label6.Text = "V a l o r : 0 ";
        }

        // Aceptar Contraste
        private void button10_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            double Contraste;
            Contraste = trackBar2.Value;
            Contraste = (100 + Contraste) / 100;
            double A, R, G, B;
            for ( int i=0; i<Image2.Width;i++)
            {
                for ( int j=0; j<Image2.Height; j++)
                {
                    Color Pixel = Image2.GetPixel(i, j);
                    A = Math.Abs( ((((Pixel.A / 255.0) - 0.5) * Contraste) + 0.5) * 255.0 );
                    R = Math.Abs( ((((Pixel.R / 255.0) - 0.5) * Contraste) + 0.5) * 255.0 );
                    G = Math.Abs( ((((Pixel.G / 255.0) - 0.5) * Contraste) + 0.5) * 255.0 );
                    B = Math.Abs( ((((Pixel.B / 255.0) - 0.5) * Contraste) + 0.5) * 255.0 );
                    Image2.SetPixel(i, j, Color.FromArgb( Pixel.A, Clamp( (int)R ), Clamp( (int)G ), Clamp( (int)B ) ) );
                }
            }
            // Restablecer
            pictureBox1.Image = Image2;
            trackBar2.Value = 1;
            label8.Text = "V a l o r : 0 ";
        }

        // Zoom out
        private void button13_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            if (ZoomAux < -2)
            {
                MessageBox.Show(" Ha llegado a la cantidad maxima de zoom posible ");
                return;
            }
            Bitmap Image3 = new Bitmap(Image2.Width / 2, Image2.Height / 2, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Color Pixel1, Pixel2, Pixel3, Pixel4;
            int A, R, G, B;
            int Alto = Image2.Height, Ancho = Image2.Width;
            if (Image2.Width % 2 != 0) {
                Ancho = Ancho - 1;
            }
            if ( Image2.Height % 2 != 0)
            {
                Alto = Alto - 1;
            } 
            
                for (int i = 0; i < Ancho; i = i + 2)
                {
                    for (int j = 0; j < Alto; j = j + 2)
                    {
                        Pixel1 = Image2.GetPixel(i, j);
                        Pixel2 = Image2.GetPixel(i + 1, j);
                        Pixel3 = Image2.GetPixel(i, j + 1);
                        Pixel4 = Image2.GetPixel(i + 1, j + 1);
                        A = (Pixel1.A + Pixel2.A + Pixel3.A + Pixel4.A) / 4;
                        B = (Pixel4.B + Pixel3.B + Pixel2.B + Pixel1.B) / 4;
                        G = (Pixel1.G + Pixel2.G + Pixel3.G + Pixel4.G) / 4;
                        R = (Pixel1.R + Pixel2.R + Pixel3.R + Pixel4.R) / 4;
                        Image3.SetPixel(i / 2, j / 2, Color.FromArgb(A, R, G, B));
                    }
                }
                Image2 = CopyBitmap(Image3);
                pictureBox1.Image = Image2;
                ZoomAux--;
            
        }

        public Bitmap Escalar(int Ancho, int Alto)
        {
            int w2 = Ancho;
            int h2 = Alto;
            Bitmap Image3 = new Bitmap(w2, h2, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            int x, y;
            float x_ratio = ((float)(Image2.Width - 1)) / w2;
            float y_ratio = ((float)(Image2.Height - 1)) / h2;
            float x_diff, y_diff;
            for (int i = 0; i < h2; i++)
            {
                for (int j = 0; j < w2; j++)
                {
                    x = (int)(x_ratio * j);
                    y = (int)(y_ratio * i);
                    x_diff = (x_ratio * j) - x;
                    y_diff = (y_ratio * i) - y;
                    Color Pixel1 = Image2.GetPixel(x, y);
                    Color Pixel2 = Image2.GetPixel(x + 1,y);
                    Color Pixel3 = Image2.GetPixel(x, y + 1);
                    Color Pixel4 = Image2.GetPixel(x + 1, y + 1);
                    float blue, red, green;

                    // blue element
                    // Yb = Ab(1-w)(1-h) + Bb(w)(1-h) + Cb(h)(1-w) + Db(wh)
                    blue = (Pixel1.B) * (1 - x_diff) * (1 - y_diff) + (Pixel2.B) * (x_diff) * (1 - y_diff) +
                           (Pixel3.B) * (y_diff) * (1 - x_diff) + (Pixel4.B) * (x_diff * y_diff);

                    // green element
                    // Yg = Ag(1-w)(1-h) + Bg(w)(1-h) + Cg(h)(1-w) + Dg(wh)
                    green = Pixel1.G * (1 - x_diff) * (1 - y_diff) + (Pixel2.G) * (x_diff) * (1 - y_diff) +
                            (Pixel3.G) * (y_diff) * (1 - x_diff) + (Pixel4.G) * (x_diff * y_diff);

                    // red element
                    // Yr = Ar(1-w)(1-h) + Br(w)(1-h) + Cr(h)(1-w) + Dr(wh)
                    red = (Pixel1.R) * (1 - x_diff) * (1 - y_diff) + (Pixel2.R) * (x_diff) * (1 - y_diff) +
                          (Pixel3.R) * (y_diff) * (1 - x_diff) + (Pixel4.R) * (x_diff * y_diff);

                    Image3.SetPixel(j, i, Color.FromArgb(Pixel1.A, Clamp((int)red), Clamp((int)green), Clamp((int)blue)));

                }
            }
            return Image3;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            try
            {
                int Alto, Ancho;
                Alto = Int32.Parse(textBox2.Text);
                Ancho = Int32.Parse(textBox3.Text);
                Image2 = Escalar(Ancho, Alto);
                pictureBox1.Image = Image2;
                textBox2.Text = "";
                textBox3.Text = "";
            }
            catch (System.FormatException)
            {
                MessageBox.Show(" Error en los parametros ingresados ");
                return;
            }

        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //Rotar
        private void button15_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            Color color;
            try
            {
                double t = Double.Parse(textBox4.Text);

            int x, y, x1, y1;
            int nr, nc;  //numero de renglones, numero de columnas
            nr = Image2.Height;
            nc = Image2.Width;
            t = t * Math.PI / 180;
            int min_x = (int)Math.Round(Math.Min(0.0, Math.Min(-Math.Sin(t) * (Image2.Width - 1), Math.Min(Math.Cos(t) * (Image2.Height - 1), Math.Cos(t) * (Image2.Height - 1) - Math.Sin(t) * (Image2.Width - 1)))));
            int max_x = (int)Math.Round(Math.Max(0.0, Math.Max(-Math.Sin(t) * (Image2.Width - 1), Math.Max(Math.Cos(t) * (Image2.Height - 1), Math.Cos(t) * (Image2.Height - 1) - Math.Sin(t) * (Image2.Width - 1)))));
            int min_y = (int)Math.Round(Math.Min(0.0, Math.Min(Math.Cos(t) * (Image2.Width - 1), Math.Min(Math.Sin(t) * (Image2.Height - 1), Math.Sin(t) * (Image2.Height - 1) + Math.Cos(t) * (Image2.Width - 1)))));
            int max_y = (int)Math.Round(Math.Max(0.0, Math.Max(Math.Cos(t) * (Image2.Width - 1), Math.Max(Math.Sin(t) * (Image2.Height - 1), Math.Sin(t) * (Image2.Height - 1) + Math.Cos(t) * (Image2.Width - 1)))));

            Image3 = new Bitmap(max_y - min_y + 1, max_x - min_x + 1);

            for (x1 = 0; x1 < max_x - min_x + 1; x1++)
            {
                for (y1 = 0; y1 < max_y - min_y + 1; y1++)
                {
                    x = (int)Math.Round(Math.Cos(t) * x1 - Math.Sin(t) * y1 - min_x);
                    y = (int)Math.Round(Math.Sin(t) * x1 + Math.Cos(t) * y1 - min_y);

                    if ((x >= 0) && (x < nr) && (y >= 0) && (y < nc))
                    {
                        color = Image2.GetPixel(y, x);
                        Image3.SetPixel(y1, x1, color);

                    }
                    /*else
                    {
                        color = Color.Black; //aqui le pones un valor que quieras
                    }*/

                }
            }
            textBox4.Text = "";
            Image2 = Image3;
            pictureBox1.Image = Image2;
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Error en la entrada");
                return;
            }
        }


        // Zoom in
        private void button12_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            if ( ZoomAux > 2 )
            {
                MessageBox.Show(" Ha llegado a la cantidad maxima de zoom posible ");
                return;
            }
            Bitmap Image3 = new Bitmap(Image2.Width*2, Image2.Height*2, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            for ( int i=0; i<Image2.Width; i++)
            {
                for ( int j=0; j<Image2.Height; j++)
                {
                    Color Pixel = Image2.GetPixel(i, j);
                    Image3.SetPixel(i*2,j*2,Pixel);
                    Image3.SetPixel((i*2)+1,j*2,Pixel);
                    Image3.SetPixel(i*2,(j*2)+1,Pixel);
                    Image3.SetPixel((i*2)+1,(j*2)+1,Pixel);
                }
            }
            Image2 = CopyBitmap(Image3);
            pictureBox1.Image = Image2;
            ZoomAux++;

        }

        // Aplicar
        private void button8_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            // Aplicar cambios a la Imagen
            Image =  CopyBitmap(Image2);
            // Restablecer los valores de la interfaz y la acumulada.
            trackBar1.Value = 0;
            label6.Text = "V a l o r : 0 ";
            Acum = 0;
            trackBar2.Value = 1;
            label8.Text = "V a l o r : 0 ";
            pictureBox1.Image = Image;
            Image2 = CopyBitmap(Image);
            label1.Text = "Ancho: " + (Image.Width).ToString();
            label2.Text = "Alto: " + (Image.Height).ToString();
            label3.Text = "Tamaño: " + (Image.Width * Image.Height).ToString();
            label4.Text = "Profundidad: " + (Image.PixelFormat).ToString();
            textBox1.Text = "0";
            Zoom = ZoomAux;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

        }

        // Cancelar
        private void button9_Click(object sender, EventArgs e)
        {
            if (Act == false)
            {
                MessageBox.Show(" No hay imagen ");
                return;
            }
            // Restablecer los valores de la interfaz y la acumulada.
            Image2 = CopyBitmap(Image);
            trackBar1.Value = 0;
            label6.Text = "V a l o r : 0 ";
            Acum = 0;
            trackBar2.Value = 1;
            label8.Text = "V a l o r : 0 ";
            pictureBox1.Image = Image;
            ZoomAux = Zoom;
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label8.Text = "V a l o r : " + trackBar2.Value.ToString();
        }


    }
}
