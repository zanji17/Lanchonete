﻿using System;
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
    public partial class FormUsuario : Form
    {
        SqlConnection con = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=C:\\Programas\\Lanchonete\\lanchonete.mdf;Integrated Security = True");
        public FormUsuario()
        {
            InitializeComponent();
        }
        public void CarregaDGV()
        {
            String str = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=C:\\Programas\\Lanchonete\\lanchonete.mdf;Integrated Security = True";
            String Query = "SELECT * FROM Usuario";
            SqlConnection con = new SqlConnection(str);
            SqlCommand cmd = new SqlCommand(Query, con);
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable usuario = new DataTable();
            da.Fill(usuario);
            dgvUsuario.DataSource = usuario;
            con.Close();
        }

        private void FormUsuario_Load(object sender, EventArgs e)
        {
            CarregaDGV();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("InserirUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nome", SqlDbType.NChar).Value = txtNome.Text.Trim();
                cmd.Parameters.AddWithValue("@cargo", SqlDbType.NChar).Value = txtCargo.Text.Trim();
                cmd.Parameters.AddWithValue("@admissao", SqlDbType.Date).Value = dtpAdmissao.Value;
                cmd.Parameters.AddWithValue("@login", SqlDbType.NChar).Value = txtLogin.Text.Trim();
                cmd.Parameters.AddWithValue("@senha", SqlDbType.NChar).Value = txtSenha.Text.Trim();
                cmd.ExecuteNonQuery();
                CarregaDGV();
                MessageBox.Show("Usuario Cadastrado com Sucesso!", "Cadastro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Text = "";
                txtCargo.Text = "";
                txtLogin.Text = "";
                txtSenha.Text = "";
                dtpAdmissao.Value = DateTime.Now;
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
                SqlCommand cmd = new SqlCommand("AtualizarUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Convert.ToInt32(txtId.Text.Trim());
                cmd.Parameters.AddWithValue("@nome", SqlDbType.NChar).Value = txtNome.Text.Trim();
                cmd.Parameters.AddWithValue("@cargo", SqlDbType.NChar).Value = txtCargo.Text.Trim();
                cmd.Parameters.AddWithValue("@admissao", SqlDbType.Date).Value = dtpAdmissao.Value;
                cmd.Parameters.AddWithValue("@login", SqlDbType.NChar).Value = txtLogin.Text.Trim();
                cmd.Parameters.AddWithValue("@senha", SqlDbType.NChar).Value = txtSenha.Text.Trim();
                cmd.ExecuteNonQuery();
                CarregaDGV();
                MessageBox.Show("Usuario Atualizado com Sucesso!", "Atualização", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtId.Text = "";
                txtNome.Text = "";
                txtCargo.Text = "";
                txtLogin.Text = "";
                txtSenha.Text = "";
                dtpAdmissao.Value = DateTime.Now;
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
            SqlCommand cmd = new SqlCommand("ExcluirUsuario", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Convert.ToInt32(txtId.Text.Trim());
            cmd.ExecuteNonQuery();
            CarregaDGV();
            MessageBox.Show("Usuario Apagado com Sucesso!", "Exclusão", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtId.Text = "";
            txtNome.Text = "";
            txtCargo.Text = "";
            txtLogin.Text = "";
            txtSenha.Text = "";
            dtpAdmissao.Value = DateTime.Now;
            con.Close();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLocalizar_Click(object sender, EventArgs e)
        {
            if (txtId.Text.Trim() != "")
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("LocalizarUsuario", con);
                cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Convert.ToInt32(txtId.Text.Trim());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    txtNome.Text = rd["nome"].ToString();
                    txtCargo.Text = rd["cargo"].ToString();
                    txtLogin.Text = rd["login"].ToString();
                    txtSenha.Text = rd["senha"].ToString();
                    dtpAdmissao.Value = Convert.ToDateTime(rd["admissao"]);
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Nenhum usuário localizado com este ID?", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("ID não foi informado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvUsuario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvUsuario.Rows[e.RowIndex];
                txtId.Text = row.Cells[0].Value.ToString();
                txtNome.Text = row.Cells[1].Value.ToString();
                txtCargo.Text = row.Cells[2].Value.ToString();
                dtpAdmissao.Value = Convert.ToDateTime(row.Cells[3].Value);
                txtLogin.Text = row.Cells[4].Value.ToString();
                txtSenha.Text = row.Cells[5].Value.ToString();
            }
        }

        private void txtLogin_Leave(object sender, EventArgs e)
        {
            string usu = "SELECT login FROM Usuario WHERE login = @login";
            SqlCommand cmd = new SqlCommand(usu, con);
            cmd.Parameters.AddWithValue("@login", SqlDbType.NChar).Value = txtLogin.Text.Trim();
            con.Open();
            cmd.CommandType = CommandType.Text;
            SqlDataReader usuario = cmd.ExecuteReader();
            if (usuario.HasRows)
            {
                MessageBox.Show("Login de usuário já cadastrado! Tente Novamente.", "Login já cadastrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
                txtLogin.Focus();
                txtLogin.Text = "";
            }
            else
            {
                con.Close();
            }
        }
    }
}