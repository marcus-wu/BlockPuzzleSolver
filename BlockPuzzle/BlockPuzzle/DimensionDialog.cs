using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace BlockPuzzle
{
    public partial class DimensionDialog : Form
    {
        public Vector3 Result { get; set; }
        public DimensionDialog()
        {
            InitializeComponent();
        }

        private void numericUpDown1_Enter(object sender, System.EventArgs e)
        {
            numericUpDown1.Select(0, numericUpDown1.Text.Length);
        }

        private void numericUpDown2_Enter(object sender, System.EventArgs e)
        {
            numericUpDown2.Select(0, numericUpDown2.Text.Length);
        }

        private void numericUpDown3_Enter(object sender, System.EventArgs e)
        {
            numericUpDown3.Select(0, numericUpDown3.Text.Length);
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Result = new Vector3((int)numericUpDown1.Value, (int)numericUpDown2.Value, (int)numericUpDown3.Value);
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
