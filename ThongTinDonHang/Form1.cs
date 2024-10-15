using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThongTinDonHang
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void txb_TongDonHang_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void LoadInvoiceData(DateTime fromDate, DateTime toDate)
        {
            using (var context = new Model1())
            {
                // Lấy danh sách hóa đơn có ngày giao hàng nằm trong khoảng thời gian từ fromDate đến toDate
                var invoices = context.Invoice
                    .Where(i => i.DeliveryDate >= fromDate && i.DeliveryDate <= toDate)
                    .Select(i => new
                    {
                        STT = i.InvoiceNo,
                        NgayDatHang = i.OrderDate,
                        NgayGiaoHang = i.DeliveryDate,
                        ThanhTien = i.Order.Sum(o => o.Price * o.Quantity) // Tính tổng tiền mỗi hóa đơn
                    })
                    .ToList();

                // Hiển thị dữ liệu trong DataGridView
                dataGridView1.DataSource = invoices;

                // Tính tổng cộng đơn hàng và hiển thị trong textbox txb_TongDonHang
                decimal total = invoices.Sum(i => i.ThanhTien);
                txb_TongDonHang.Text = total.ToString("C"); // Định dạng tiền tệ
            }
        }


        private void cb_XemTatCaDonHang_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_XemTatCaDonHang.Checked)
            {
                // Lấy ngày đầu tháng và cuối tháng
                DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                // Cập nhật DateTimePicker và tải dữ liệu
                dateTimePicker1.Value = firstDayOfMonth;
                dateTimePicker2.Value = lastDayOfMonth;
            }

            // Tải lại dữ liệu
            LoadInvoiceData(dateTimePicker1.Value, dateTimePicker2.Value);
        }

   
       
        private void Form1_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;

            // Gọi hàm để tải dữ liệu
            LoadInvoiceData(dateTimePicker1.Value, dateTimePicker2.Value);
        }

       


        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            LoadInvoiceData(dateTimePicker1.Value, dateTimePicker2.Value);
        }
       


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadInvoiceData(dateTimePicker1.Value, dateTimePicker2.Value);
        }
    }
}
