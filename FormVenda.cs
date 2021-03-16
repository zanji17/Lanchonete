using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Lanchonete
{
    public partial class FormVenda : Form
    {
        SqlConnection con = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=C:\\Programas\\Lanchonete\\lanchonete.mdf;Integrated Security = True");
        public FormVenda()
        {
            InitializeComponent();
        }
        
        public void CarregaCbCliente()
        {
            String cli = "SELECT * FROM Cliente";
            SqlCommand cmd = new SqlCommand(cli, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cli, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Cliente");
            cbCliente.ValueMember = "cpf";
            cbCliente.DisplayMember = "nome";
            cbCliente.DataSource = ds.Tables["Cliente"];
            con.Close();
        }

        public void CarregaCbProduto()
        {
            String pro = "SELECT * FROM Produto";
            SqlCommand cmd = new SqlCommand(pro, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(pro, con);
            DataSet ds = new DataSet();
            da.Fill(ds, "Produto");
            cbProduto.ValueMember = "Id";
            cbProduto.DisplayMember = "nome";
            con.Close();
            cbProduto.DataSource = ds.Tables["Produto"];
        }

        private void FormVenda_Load(object sender, EventArgs e)
        {
            if (cbCliente.DisplayMember == "")
            {
                cbProduto.Enabled = false;
                txtIdProduto.Enabled = false;
                txtQuantidade.Enabled = false;
                txtValor.Enabled = false;
                dgvVenda.Enabled = false;
                btnNovoItem.Enabled = false;
                btnExcluirItem.Enabled = false;
                txtTotal.Enabled = false;
                btnFinalizar.Enabled = false;
            }
            CarregaCbCliente();
        }

        private void btnNovaVenda_Click(object sender, EventArgs e)
        {
            cbProduto.Enabled = true;
            CarregaCbProduto();
            txtIdProduto.Enabled = true;
            txtQuantidade.Enabled = true;
            txtValor.Enabled = true;
            dgvVenda.Enabled = true;
            btnNovoItem.Enabled = true;
            btnExcluirItem.Enabled = true;
            txtTotal.Enabled = true;
            btnFinalizar.Enabled = true;
            dgvVenda.Columns.Add("ID", "ID");
            dgvVenda.Columns.Add("Produto", "Produto");
            dgvVenda.Columns.Add("Quantidade", "Quantidade");
            dgvVenda.Columns.Add("Valor", "Valor");
            dgvVenda.Columns.Add("Total", "Total");
            txtQuantidade.Focus();
            txtQuantidade.Text = "";
        }

        private void cbProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            con.Open();
            string pro = "SELECT * FROM Produto WHERE nome = @nome";
            SqlCommand cmd = new SqlCommand(pro, con);
            cmd.Parameters.AddWithValue("@nome", SqlDbType.NChar).Value = cbProduto.Text.Trim();
            cmd.CommandType = CommandType.Text;
            SqlDataReader produto = cmd.ExecuteReader();
            if (produto.Read())
            {
                txtIdProduto.Text = produto["Id"].ToString();
                txtValor.Text = produto["valor"].ToString();
                txtQuantidade.Focus();
                txtQuantidade.Text = "";
                con.Close();
            }
            else
            {
                MessageBox.Show("Produto Não Identificado","Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }
    }
}
