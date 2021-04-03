using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using CodificacaoBase64;

namespace Lanchonete
{
    public partial class FormLogin : Form
    {
        private Cripto b;
        SqlConnection con = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=C:\\Programas\\Lanchonete\\lanchonete.mdf;Integrated Security = True");
        public FormLogin()
        {
            InitializeComponent();
            FormSplashScreen formSplash = new FormSplashScreen();
            formSplash.Show();
            Thread.Sleep(3000);
            formSplash.Close();
            b = new Cripto();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogar_Click(object sender, EventArgs e)
        {
            con.Open();
            String usu = "SELECT login, senha FROM usuario WHERE login = @login and senha = @senha";
            SqlCommand cmd = new SqlCommand(usu, con);
            cmd.Parameters.AddWithValue("@login", SqlDbType.NChar).Value = txtLogin.Text.Trim();
            cmd.Parameters.AddWithValue("@senha", SqlDbType.NChar).Value = b.Base64Encode(txtSenha.Text.Trim());
            SqlDataReader usuario = cmd.ExecuteReader();
            if (usuario.HasRows)
            {
                this.Hide();
                FormPrincipal principal = new FormPrincipal();
                principal.Show();
                con.Close();
            }
            else
            {
                MessageBox.Show("Login ou Senha incorretos! Tente Novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLogin.Text = "";
                txtSenha.Text = "";
                con.Close();
            }
        }
    }
}
