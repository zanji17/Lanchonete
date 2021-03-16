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
    public partial class FormProduto : Form
    {
        SqlConnection con = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=C:\\Programas\\Lanchonete\\lanchonete.mdf;Integrated Security = True");
        public FormProduto()
        {
            InitializeComponent();
        }

        public void CarregaDGV()
        {
            String str = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=C:\\Programas\\Lanchonete\\lanchonete.mdf;Integrated Security = True";
            String Query = "SELECT * FROM Produto";
            SqlConnection con = new SqlConnection(str);
            SqlCommand cmd = new SqlCommand(Query, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable produto = new DataTable();
            da.Fill(produto);
            dgvProduto.DataSource = produto;
            con.Close();
        }

        private void FormProduto_Load(object sender, EventArgs e)
        {
            CarregaDGV();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InserirProduto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nome", SqlDbType.NChar).Value = txtNome.Text.Trim();
                cmd.Parameters.AddWithValue("@tipo", SqlDbType.NChar).Value = txtTipo.Text.Trim();
                cmd.Parameters.AddWithValue("@quantidade", SqlDbType.NChar).Value = txtQuantidade.Text.Trim();
                cmd.Parameters.AddWithValue("@valor", SqlDbType.Decimal).Value = Convert.ToDecimal(txtValor.Text.Trim());
                cmd.ExecuteNonQuery();
                CarregaDGV();
                MessageBox.Show("Produto Cadastrado com Sucesso!", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Text = "";
                txtTipo.Text = "";
                txtQuantidade.Text = "";
                txtValor.Text = "";
                con.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("AtualizarProduto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Convert.ToInt32(txtId.Text.Trim());
                cmd.Parameters.AddWithValue("@nome", SqlDbType.NChar).Value = txtNome.Text.Trim();
                cmd.Parameters.AddWithValue("@tipo", SqlDbType.NChar).Value = txtTipo.Text.Trim();
                cmd.Parameters.AddWithValue("@quantidade", SqlDbType.NChar).Value = txtQuantidade.Text.Trim();
                cmd.Parameters.AddWithValue("@valor", SqlDbType.Decimal).Value = Convert.ToDecimal(txtValor.Text.Trim());
                cmd.ExecuteNonQuery();
                CarregaDGV();
                MessageBox.Show("Produto Atualizado com Sucesso!", "Atualização", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtId.Text = "";
                txtNome.Text = "";
                txtTipo.Text = "";
                txtQuantidade.Text = "";
                txtValor.Text = "";
                con.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("ExcluirProduto", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Convert.ToInt32(txtId.Text.Trim());
            cmd.ExecuteNonQuery();
            CarregaDGV();
            MessageBox.Show("Usuario Apagado com Sucesso!", "Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtId.Text = "";
            txtNome.Text = "";
            txtTipo.Text = "";
            txtQuantidade.Text = "";
            txtValor.Text = "";
            con.Close();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLocalizar_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("LocalizarProduto", con);
            cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Convert.ToInt32(txtId.Text.Trim());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                txtNome.Text = rd["nome"].ToString();
                txtTipo.Text = rd["tipo"].ToString();
                txtQuantidade.Text = rd["quantidade"].ToString();
                txtValor.Text = rd["valor"].ToString();
                con.Close();
            }
            else
            {
                MessageBox.Show("Nenhum Produto localizado com este ID?", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvProduto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvProduto.Rows[e.RowIndex];
                txtId.Text = row.Cells[0].Value.ToString();
                txtNome.Text = row.Cells[1].Value.ToString();
                txtTipo.Text = row.Cells[2].Value.ToString();
                txtQuantidade.Text = row.Cells[3].Value.ToString();
                txtValor.Text = row.Cells[4].Value.ToString();
            }
        }

        private void txtNome_Leave(object sender, EventArgs e)
        {
            string pro = "SELECT nome FROM Produto WHERE nome = @nome";
            SqlCommand cmd = new SqlCommand(pro, con);
            cmd.Parameters.AddWithValue("@nome", SqlDbType.NChar).Value = txtNome.Text.Trim();
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataReader produto = cmd.ExecuteReader();
            if (produto.HasRows)
            {
                MessageBox.Show("Nome de Produto já cadastrado! Tente Novamente.", "Nome já cadastrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
                txtNome.Focus();
                txtNome.Text = "";
            }
            else
            {
                con.Close();
            }
        }
    }
}
