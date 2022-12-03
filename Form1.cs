using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;

namespace _2Iskd
{
    public partial class Form1 : Form
    {
        private readonly SingleLayerNeuralNetwork network;
        private readonly PictureBoxArtist boxArtist;

        public Form1()
        {
            InitializeComponent();
            network = new(new char[] { 'П', 'Е', 'Т', 'Л' }, new double[] { 4664.570633, 6125.233988, 6104.869437, 5010.279034 });
            pictureBox1.BackColor = Color.Gray;
            boxArtist = new(pictureBox1, new Pen(Color.Black, 20f)
            {
                StartCap = System.Drawing.Drawing2D.LineCap.Round,
                EndCap = System.Drawing.Drawing2D.LineCap.Round
            });

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Clear_Click(object sender, EventArgs e)
        {
            boxArtist.Clear();
        }

        private void Save_Click(object sender, EventArgs e)
        {
           string myLetter = letter.Text;
            int id = Directory.GetFiles(@$"C:\Users\slava\source\repos\2Iskd\2Iskd\letters\{myLetter}").Length;

            boxArtist.SaveTo(@$"C:\Users\slava\source\repos\2Iskd\2Iskd\letters\{myLetter}\{myLetter}_{id + 1}.png");

    
        }

        private void Select_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new();
            string path;

            fileDialog.ShowDialog();
            path = fileDialog.FileName;

            pictureBox1.Image = new Bitmap(path);

        }

        private void Recognize_Click(object sender, EventArgs e)
        {
            var answer = network.Identify(new Bitmap(pictureBox1.Image));
            char letter = answer.letter;

            if (answer.isSure)
            {
                MessageBox.Show($"Это буква {letter}.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            MessageBox.Show($"Вероятно, что это буква {letter}.", "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    

    private void Learning_Click(object sender, EventArgs e)
        {
            network.Learning();

        }

        private void Exit_Click(object sender, EventArgs e)
    {
        Close();

    }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            boxArtist.StartDrawing(e.Location);

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            boxArtist.EndDrawing();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            boxArtist.Drawing(e.Location);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}