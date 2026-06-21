using System;
using System.Data;
using System.Data.SqlClient;

namespace CRUDMahasiswaADO
{
    internal class DAL
    {
        static string connectionString = @"Data Source=.\ACHELL;Initial Catalog=DBAkademikADO;User ID=sa;Password=Rachel18*";

        public string GetConnectionString()
        {
            return connectionString;
        }

        SqlConnection conn = new SqlConnection(connectionString);
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataTable dtProdi;

        // PERBAIKAN: Menggunakan parameter @Total agar tidak error
        public int CountMhs()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_CountMahasiswa", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Pastikan nama parameter di sini SAMA dengan nama parameter di SQL Server
                SqlParameter outputParam = new SqlParameter("@Total", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                cmd.ExecuteNonQuery();
                return Convert.ToInt32(outputParam.Value);
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable GetMhs()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetMahasiswa", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);
                return dtMahasiswa;
            }
            finally { conn.Close(); }
        }

        public void InsertMhs(string nim, string nama, string alamat, string jenisKelamin, DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            SqlTransaction trans = conn.BeginTransaction();
            try
            {
                SqlCommand command = new SqlCommand("sp_InsertMahasiswa", conn, trans);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pNIM", nim);
                command.Parameters.AddWithValue("pNama", nama);
                command.Parameters.AddWithValue("pAlamat", alamat);
                command.Parameters.AddWithValue("pTanggalLahir", tanggalLahir);
                command.Parameters.AddWithValue("pJenisKelamin", jenisKelamin);
                command.Parameters.AddWithValue("pNmProdi", kodeProdi);
                command.Parameters.AddWithValue("pFoto", foto);
                command.ExecuteNonQuery();
                trans.Commit();
            }
            catch (Exception) { trans.Rollback(); throw; }
            finally { conn.Close(); }
        }

        public void UpdateMhs(string nim, string nama, string alamat, string jenisKelamin, DateTime tanggalLahir, string kodeProdi, byte[] foto)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand command = new SqlCommand("sp_UpdateMahasiswa", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("pNIM", nim);
                command.Parameters.AddWithValue("pNama", nama);
                command.Parameters.AddWithValue("pAlamat", alamat);
                command.Parameters.AddWithValue("pJenisKelamin", jenisKelamin);
                command.Parameters.AddWithValue("pTanggalLahir", tanggalLahir);
                command.Parameters.AddWithValue("pNmProdi", kodeProdi);
                command.Parameters.AddWithValue("pFoto", foto);
                command.ExecuteNonQuery();
            }
            finally { conn.Close(); }
        }

        public void DeleteMhs(string nim)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DeleteMahasiswa", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("pNIM", nim);
                cmd.ExecuteNonQuery();
            }
            finally { conn.Close(); }
        }
        public DataTable getAllDataChart()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DashBoard", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);
                return dtMahasiswa;
            }
            finally { conn.Close(); }
        }

        public DataTable getDataChartByTahun(DateTime thMasuk)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DashBoardByTahun", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inTglMsuk", thMasuk.Year);
                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);
                return dtMahasiswa;
            }
            finally { conn.Close(); }
        }

        public void resetData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmdDelete = new SqlCommand("DELETE FROM mahasiswa;", conn);
                cmdDelete.ExecuteNonQuery();
                SqlCommand cmdInsert = new SqlCommand("INSERT INTO mahasiswa SELECT * FROM mahasiswa_backup;", conn);
                cmdInsert.ExecuteNonQuery();
            }
            finally { conn.Close(); }
        }

        public void testInject(string nim)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("Update mahasiswa set nama = 'HACKED' where NIM = " + nim, conn);
                cmd.ExecuteNonQuery();
            }
            finally { conn.Close(); }
        }

        public DataTable getDataRekap(string prodi, DateTime tanggalMasuk)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inProdi", prodi);
                cmd.Parameters.AddWithValue("@inTglMsuk", tanggalMasuk.Year.ToString());
                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();
                da.Fill(dtMahasiswa);
                return dtMahasiswa;
            }
            finally { conn.Close(); }
        }

        // Tambahkan method lainnya (resetData, testInject, dll) di sini sesuai kebutuhan Anda
    }
}