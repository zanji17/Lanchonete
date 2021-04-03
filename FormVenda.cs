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
                btnFinalizarVenda.Enabled = false;
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
            btnFinalizarVenda.Enabled = true;
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

        private void txtQuantidade_Leave(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("Estoque", con);
            cmd.Parameters.AddWithValue("@Id", txtIdProduto.Text);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rd = cmd.ExecuteReader();
            int valor1 = 0;
            bool conversaoSucedida = int.TryParse(txtQuantidade.Text, out valor1);
            if (rd.Read())
            {
                int valor2 = Convert.ToInt32(rd["quantidade"].ToString());
                if (valor1 > valor2)
                {
                    MessageBox.Show("Não tem quantidade suficiente em estoque!", "Estoque Insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQuantidade.Text = "";
                    txtQuantidade.Focus();
                    con.Close();
                }
                else
                {
                    con.Close();
                }
            }
        }
        private void btnLocalizarVenda_Click(object sender, EventArgs e)
        {
            CarregaCbProduto();
            txtTotal.Text = "";
            dgvVenda.Columns.Clear();
            dgvVenda.Rows.Clear();
            con.Open();
            SqlCommand cmd = new SqlCommand("LocalizarVenda", con);
            cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = Convert.ToInt32(txtIdVenda.Text.Trim());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int linhas = dt.Rows.Count;
            if (dt.Rows.Count > 0)
            {
                cbCliente.Enabled = true;
                cbCliente.Text = "";
                cbCliente.Text = dt.Rows[0]["nomecliente"].ToString();
                txtTotal.Text = dt.Rows[0]["total_venda"].ToString();
                cbProduto.Enabled = true;
                txtIdProduto.Enabled = true;
                txtQuantidade.Enabled = true;
                txtValor.Enabled = true;
                dgvVenda.Enabled = true;
                btnNovoItem.Enabled = true;
                btnEditarItem.Enabled = true;
                btnExcluirItem.Enabled = true;
                txtTotal.Enabled = true;
                btnFinalizarVenda.Enabled = true;
                dgvVenda.Columns.Add("ID", "ID");
                dgvVenda.Columns.Add("Produto", "Produto");
                dgvVenda.Columns.Add("Quantidade", "Quantidade");
                dgvVenda.Columns.Add("Valor", "Valor");
                dgvVenda.Columns.Add("Total", "Total");
                for (int i = 0; i < linhas; i++)
                {
                    DataGridViewRow item = new DataGridViewRow();
                    item.CreateCells(dgvVenda);
                    item.Cells[0].Value = dt.Rows[i]["id_produto"].ToString();
                    item.Cells[1].Value = dt.Rows[i]["nomeproduto"].ToString();
                    item.Cells[2].Value = dt.Rows[i]["quantidade"].ToString();
                    item.Cells[3].Value = dt.Rows[i]["valor_unitario"].ToString();
                    item.Cells[4].Value = dt.Rows[i]["valor_total"].ToString();
                    dgvVenda.Rows.Add(item);
                }
            }
            else
            {
                MessageBox.Show("Nenhum pedido localizado com este ID!", "Não localizado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            con.Close();
        }

        private void btnFinalizarPedido_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("InserirVenda", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id_pessoa", SqlDbType.NChar).Value = cbCliente.SelectedValue;
            cmd.Parameters.AddWithValue("@total_venda", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTotal.Text);
            cmd.Parameters.AddWithValue("@data_venda", SqlDbType.Date).Value = DateTime.Now;
            cmd.Parameters.AddWithValue("@situacao", SqlDbType.NChar).Value = "aberto";
            cmd.ExecuteNonQuery();
            string idvenda = "SELECT IDENT_CURRENT('Venda') AS Id_venda";
            SqlCommand cmdvenda = new SqlCommand(idvenda, con);
            Int32 idvenda2 = Convert.ToInt32(cmdvenda.ExecuteScalar());
            foreach (DataGridViewRow dr in dgvVenda.Rows)
            {
                SqlCommand cmditens = new SqlCommand("InserirItens", con);
                cmditens.CommandType = CommandType.StoredProcedure;
                cmditens.Parameters.AddWithValue("@Id_venda", SqlDbType.Int).Value = idvenda2;
                cmditens.Parameters.AddWithValue("@Id_produto", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[0].Value);
                cmditens.Parameters.AddWithValue("@quantidade", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[2].Value);
                cmditens.Parameters.AddWithValue("@valor_unitario", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[3].Value);
                cmditens.Parameters.AddWithValue("@valor_total", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[4].Value);
                cmditens.ExecuteNonQuery();
            }
            con.Close();
            dgvVenda.Rows.Clear();
            dgvVenda.Refresh();
            txtTotal.Text = "";
            MessageBox.Show("Pedido salvo com sucesso!", "Pedido Armazenado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgvVenda.Rows)
            {
                con.Open();
                String venda = "UPDATE Venda SET total_venda = @total WHERE Id_venda = @IdVenda";
                SqlCommand cmdvenda = new SqlCommand(venda, con);
                cmdvenda.Parameters.AddWithValue("@total", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTotal.Text);
                cmdvenda.Parameters.AddWithValue("@IdVenda", SqlDbType.Int).Value = Convert.ToInt32(txtIdVenda.Text.Trim());
                cmdvenda.ExecuteNonQuery();
                String pedido = "SELECT * FROM ItensVenda WHERE Id_produto = @IdProduto AND Id_venda=@IdVenda";
                SqlCommand cmd = new SqlCommand(pedido, con);
                cmd.Parameters.AddWithValue("@IdVenda", SqlDbType.Int).Value = Convert.ToInt32(txtIdVenda.Text.Trim());
                cmd.Parameters.AddWithValue("@IdProduto", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[0].Value);
                SqlDataReader item = cmd.ExecuteReader();
                if (item.Read())
                {
                    con.Close();
                    con.Open();
                    String att = "UPDATE ItensVenda SET quantidade = @quantidade, valor_unitario = @valor_unitario, valor_total = @valor_total WHERE Id_produto=@IdProduto AND Id_venda = @IdVenda";
                    SqlCommand cmdatt = new SqlCommand(att, con);
                    cmdatt.Parameters.AddWithValue("@quantidade", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[2].Value);
                    cmdatt.Parameters.AddWithValue("@valor_unitario", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[3].Value);
                    cmdatt.Parameters.AddWithValue("@valor_total", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[4].Value);
                    cmdatt.Parameters.AddWithValue("@IdVenda", SqlDbType.Int).Value = Convert.ToInt32(txtIdVenda.Text.Trim());
                    cmdatt.Parameters.AddWithValue("@IdProduto", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[0].Value);
                    cmdatt.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    con.Close();
                    con.Open();
                    SqlCommand cmdinserir = new SqlCommand("InserirItens", con);
                    cmdinserir.CommandType = CommandType.StoredProcedure;
                    cmdinserir.Parameters.AddWithValue("@Id_venda", SqlDbType.Int).Value = Convert.ToInt32(txtIdVenda.Text.Trim());
                    cmdinserir.Parameters.AddWithValue("@Id_produto", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[0].Value);
                    cmdinserir.Parameters.AddWithValue("@quantidade", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[2].Value);
                    cmdinserir.Parameters.AddWithValue("@valor_unitario", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[3].Value);
                    cmdinserir.Parameters.AddWithValue("@valor_total", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[4].Value);
                    cmdinserir.ExecuteNonQuery();
                    con.Close();
                }
            }
            MessageBox.Show("Pedido atualizado com sucesso!", "Atualizar Pedido", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dgvVenda.Rows.Clear();
            
            dgvVenda.Refresh();
        }

        private void btnFinalizarVenda_Click(object sender, EventArgs e)
        {
            if (txtIdVenda == null)
            {
                con.Open();
                string idvenda = "SELECT IDENT_CURRENT('Venda') AS Id_venda";
                SqlCommand cmdvenda = new SqlCommand(idvenda, con);
                Int32 idvenda2 = Convert.ToInt32(cmdvenda.ExecuteScalar());
                con.Close();
                con.Open();
                String venda = "UPDATE Venda SET situacao = @situacao data_pagamento = @data_pagamento WHERE Id_venda = @Id_venda";
                SqlCommand cmd = new SqlCommand(venda, con);
                cmd.Parameters.AddWithValue("@situacao", SqlDbType.NChar).Value = "Fechado";
                cmd.Parameters.AddWithValue("@data_pagamento", SqlDbType.NChar).Value = Convert.ToString(DateTime.Now);
                cmd.Parameters.AddWithValue("@Id_venda", SqlDbType.Int).Value = idvenda2;
                cmd.ExecuteNonQuery();
                con.Close();
                foreach (DataGridViewRow dr in dgvVenda.Rows)
                {
                    con.Open();
                    String vendido = "INSERT INTO ItensVendidos(Id_venda, Id_produto, quantidade, valor_unitario, valor_total) VALUES (@Id_venda, @Id_produto, @quantidade, @valor_unitario, @valor_total)";
                    SqlCommand cmdvendido = new SqlCommand(vendido, con);
                    cmdvendido.Parameters.AddWithValue("@Id_venda", SqlDbType.Int).Value = idvenda2;
                    cmdvendido.Parameters.AddWithValue("@Id_produto", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[0].Value);
                    cmdvendido.Parameters.AddWithValue("@quantidade", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[2].Value);
                    cmdvendido.Parameters.AddWithValue("@valor_unitario", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[3].Value);
                    cmdvendido.Parameters.AddWithValue("@valor_total", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[4].Value);
                    cmdvendido.ExecuteNonQuery();
                    con.Close();
                }
                con.Open();
                String LimparItens = "DELETE FROM ItensVenda WHERE Id_venda = @Id_venda";
                SqlCommand cmdLimpar = new SqlCommand(LimparItens, con);
                cmdLimpar.Parameters.AddWithValue("@Id_venda", SqlDbType.Int).Value = idvenda2;
                cmdLimpar.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                con.Open();
                String venda = "UPDATE Venda SET situacao = @situacao, data_pagamento = @data_pagamento WHERE Id_venda = @Id_venda";
                SqlCommand cmd = new SqlCommand(venda, con);
                cmd.Parameters.AddWithValue("@situacao", SqlDbType.NChar).Value = "Fechado";
                cmd.Parameters.AddWithValue("@data_pagamento", SqlDbType.Date).Value = DateTime.Now;
                cmd.Parameters.AddWithValue("@Id_venda", SqlDbType.Int).Value = Convert.ToInt32(txtIdVenda.Text.Trim());
                cmd.ExecuteNonQuery();
                con.Close();
                foreach (DataGridViewRow dr in dgvVenda.Rows)
                {
                    con.Open();
                    String vendido = "INSERT INTO ItensVendidos(Id_venda, Id_produto, quantidade, valor_unitario, valor_total) VALUES (@Id_venda, @Id_produto, @quantidade, @valor_unitario, @valor_total)";
                    SqlCommand cmdvendido = new SqlCommand(vendido, con);
                    cmdvendido.Parameters.AddWithValue("@Id_venda", SqlDbType.Int).Value = Convert.ToInt32(txtIdVenda.Text.Trim());
                    cmdvendido.Parameters.AddWithValue("@Id_produto", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[0].Value);
                    cmdvendido.Parameters.AddWithValue("@quantidade", SqlDbType.Int).Value = Convert.ToInt32(dr.Cells[2].Value);
                    cmdvendido.Parameters.AddWithValue("@valor_unitario", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[3].Value);
                    cmdvendido.Parameters.AddWithValue("@valor_total", SqlDbType.Decimal).Value = Convert.ToDecimal(dr.Cells[4].Value);
                    cmdvendido.ExecuteNonQuery();
                    con.Close();
                }
                con.Open();
                String LimparItens = "DELETE FROM ItensVenda WHERE Id_venda = @Id_venda";
                SqlCommand cmdLimpar = new SqlCommand(LimparItens, con);
                cmdLimpar.Parameters.AddWithValue("@Id_venda", SqlDbType.Int).Value = Convert.ToInt32(txtIdVenda.Text.Trim());
                cmdLimpar.ExecuteNonQuery();
                con.Close();
            }
            dgvVenda.Rows.Clear();
            dgvVenda.Refresh();
            txtTotal.Text = "";
            MessageBox.Show("Venda realizada com sucesso!", "Venda Sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
