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
    public partial class FormCliente : Form
    {
        SqlConnection con = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=C:\\Programas\\Lanchonete\\lanchonete.mdf;Integrated Security = True");
        public FormCliente()
        {
            InitializeComponent();
        }

        public void CarregaDGV()
        {
            String str = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=C:\\Programas\\Lanchonete\\lanchonete.mdf;Integrated Security = True";
            String Query = "SELECT * FROM Cliente";
            SqlConnection con = new SqlConnection(str);
            SqlCommand cmd = new SqlCommand(Query, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable cliente = new DataTable();
            da.Fill(cliente);
            dgvCliente.DataSource = cliente;
            con.Close();
        }

        private void FormCliente_Load(object sender, EventArgs e)
        {
            CarregaDGV();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InserirCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cpf", SqlDbType.NChar).Value = txtCpf.Text.Trim();
                cmd.Parameters.AddWithValue("@nome", SqlDbType.NChar).Value = txtNome.Text.Trim();
                cmd.Parameters.AddWithValue("@cidade", SqlDbType.NChar).Value = txtCidade.Text.Trim();
                cmd.Parameters.AddWithValue("@celular", SqlDbType.NChar).Value = txtCelular.Text.Trim();
                cmd.Parameters.AddWithValue("@email", SqlDbType.NChar).Value = txtEmail.Text.Trim();
                cmd.ExecuteNonQuery();
                CarregaDGV();
                MessageBox.Show("Cliente Cadastrado com Sucesso!", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCpf.Text = "";
                txtNome.Text = "";
                txtCidade.Text = "";
                txtCelular.Text = "";
                txtEmail.Text = "";
                con.Close();
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("AtualizarCliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cpf", SqlDbType.NChar).Value = txtCpf.Text.Trim();
                cmd.Parameters.AddWithValue("@nome", SqlDbType.NChar).Value = txtNome.Text.Trim();
                cmd.Parameters.AddWithValue("@cidade", SqlDbType.NChar).Value = txtCidade.Text.Trim();
                cmd.Parameters.AddWithValue("@celular", SqlDbType.NChar).Value = txtCelular.Text.Trim();
                cmd.Parameters.AddWithValue("@email", SqlDbType.NChar).Value = txtEmail.Text.Trim();
                cmd.ExecuteNonQuery();
                CarregaDGV();
                MessageBox.Show("Cliente Atualizado com Sucesso!", "Atualização", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCpf.Text = "";
                txtNome.Text = "";
                txtCidade.Text = "";
                txtCelular.Text = "";
                txtEmail.Text = "";
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
            SqlCommand cmd = new SqlCommand("ExcluirCliente", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cpf", SqlDbType.NChar).Value = txtCpf.Text.Trim();
            cmd.ExecuteNonQuery();
            CarregaDGV();
            MessageBox.Show("Cliente Apagado com Sucesso!", "Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtCpf.Text = "";
            txtNome.Text = "";
            txtCidade.Text = "";
            txtCelular.Text = "";
            txtEmail.Text = "";
            con.Close();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLocalizar_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("LocalizarCliente", con);
            cmd.Parameters.AddWithValue("@cpf", SqlDbType.NChar).Value = txtCpf.Text.Trim();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                txtNome.Text = rd["nome"].ToString();
                txtCidade.Text = rd["cidade"].ToString();
                txtCelular.Text = rd["celular"].ToString();
                txtEmail.Text = rd["email"].ToString();
                con.Close();
            }
            else
            {
                MessageBox.Show("Nenhum cliente localizado com este CPF?", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCliente_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvCliente.Rows[e.RowIndex];
                txtCpf.Text = row.Cells[0].Value.ToString();
                txtNome.Text = row.Cells[1].Value.ToString();
                txtCidade.Text = row.Cells[2].Value.ToString();
                txtCelular.Text = row.Cells[3].Value.ToString();
                txtEmail.Text = row.Cells[4].Value.ToString();
            }
        }

        private void txtCpf_Leave(object sender, EventArgs e)
        {
            string cli = "SELECT cpf FROM Cliente WHERE cpf = @cpf";
            SqlCommand cmd = new SqlCommand(cli, con);
            cmd.Parameters.AddWithValue("@cpf", SqlDbType.NChar).Value = txtCpf.Text.Trim();
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataReader cliente = cmd.ExecuteReader();
            if (cliente.HasRows) 
            {
                MessageBox.Show("CPF de cliente já cadastrado! Tente Novamente.", "Cliente já cadastrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
                txtCpf.Focus();
                txtCpf.Text = "";
            }
            else
            {
                con.Close();
            }
        }
    }
}
