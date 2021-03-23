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
            string pro = "SELECT * FROM Produto WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(pro, con);
            cmd.Parameters.AddWithValue("@Id", cbProduto.SelectedValue);
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

        private void btnNovoItem_Click(object sender, EventArgs e)
        {
            var repetido = false;
            foreach (DataGridViewRow dr in dgvVenda.Rows)
            {
                if (txtIdProduto.Text == Convert.ToString(dr.Cells[0].Value))
                {
                    repetido = true;
                }
            }
            if (repetido == false)
            {
                DataGridViewRow item = new DataGridViewRow();
                item.CreateCells(dgvVenda);
                item.Cells[0].Value = txtIdProduto.Text;
                item.Cells[1].Value = cbProduto.Text;
                item.Cells[2].Value = txtQuantidade.Text;
                item.Cells[3].Value = txtValor.Text;
                item.Cells[4].Value = Convert.ToDecimal(txtQuantidade.Text) * Convert.ToDecimal(txtValor.Text);
                dgvVenda.Rows.Add(item);
                cbProduto.Text = "";
                txtIdProduto.Text = "";
                txtQuantidade.Text = "";
                txtValor.Text = "";
                decimal soma = 0;
                foreach (DataGridViewRow dr in dgvVenda.Rows)
                {
                    soma += Convert.ToDecimal(dr.Cells[4].Value);
                }
                txtTotal.Text = Convert.ToString(soma);
            }
            else
            {
                MessageBox.Show("Item já está listado na venda!", "Repetição", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }  
        }

        private void dgvVenda_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dgvVenda.Rows[e.RowIndex];
            cbProduto.Text = row.Cells[1].Value.ToString();
            txtIdProduto.Text = row.Cells[0].Value.ToString();
            txtQuantidade.Text = row.Cells[2].Value.ToString();
            txtValor.Text = row.Cells[3].Value.ToString();
        }

        private void btnEditarItem_Click(object sender, EventArgs e)
        {
            int linha = dgvVenda.CurrentRow.Index;
            dgvVenda.Rows[linha].Cells[0].Value = txtIdProduto.Text;
            dgvVenda.Rows[linha].Cells[1].Value = cbProduto.Text;
            dgvVenda.Rows[linha].Cells[2].Value = txtQuantidade.Text;
            dgvVenda.Rows[linha].Cells[3].Value = txtValor.Text;
            dgvVenda.Rows[linha].Cells[4].Value = Convert.ToDecimal(txtQuantidade.Text) * Convert.ToDecimal(txtValor.Text);
            
            cbProduto.Text = "";
            txtIdProduto.Text = "";
            txtQuantidade.Text = "";
            txtValor.Text = "";
            decimal soma = 0;
            foreach (DataGridViewRow dr in dgvVenda.Rows)
            {
                soma += Convert.ToDecimal(dr.Cells[4].Value);
            }
            txtTotal.Text = Convert.ToString(soma);
        }

        private void btnExcluirItem_Click(object sender, EventArgs e)
        {
            int linha = dgvVenda.CurrentRow.Index;
            dgvVenda.Rows.RemoveAt(linha);
            dgvVenda.Refresh();

            cbProduto.Text = "";
            txtIdProduto.Text = "";
            txtQuantidade.Text = "";
            txtValor.Text = "";
            decimal soma = 0;
            foreach (DataGridViewRow dr in dgvVenda.Rows)
            {
                soma += Convert.ToDecimal(dr.Cells[4].Value);
            }
            txtTotal.Text = Convert.ToString(soma);
        }
    }
}
