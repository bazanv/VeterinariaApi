using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using veterNetFram.AccesoDatos;

namespace veterNetFram.Presentacion.Reportes
{
    public partial class frmReporte : Form
    {
        public frmReporte()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            var filters = new Dictionary<string, object>();
            String strSql = string.Concat(" select  p.p_nombre as Producto, sum(d.cantidad) as Unidades,sum(d.cantidad*d.p_facturado) as Importes  " +
                " from Detalles d join Productos p on d.id_producto=p.id_producto join Atencion a on a.id_atencion=d.id_atencion " +
                " where  a.borrado=0 " +
                " and a.fecha between @desde and @hasta " + 
                " group by p.p_nombre ");
            filters.Add("desde", dtpDesde.Value);
            filters.Add("hasta", dtpHasta.Value);
            var resultado = ClienteSingleton.GetInstance().ConsultaSQL(strSql, filters);
            rvReporte.LocalReport.DataSources.Clear();
            rvReporte.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", resultado));
            rvReporte.RefreshReport();

       
        }

        private void frmReporte_Load(object sender, EventArgs e)
        {
     
            dtpDesde.Text = DateTime.Today.ToShortDateString();
            dtpHasta.Text = DateTime.Today.ToShortDateString();

          //  this.rvReporte.RefreshReport();
        }
    }
}
