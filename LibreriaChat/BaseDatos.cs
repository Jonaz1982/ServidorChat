using System;
using System.Data;
using System.Data.SqlClient;


public class BaseDatos
{

    private SqlConnection cn;
    private String strConexion;

    public BaseDatos()
    {
        strConexion = "";
    }

    public BaseDatos(String strConexion)
    {
        this.strConexion = strConexion;
    }

    public String CadenaConexion
    {
        get
        {
            return strConexion;
        }
        set
        {
            strConexion = value;
            cn = null;
        }
    }

    public bool Conectar()
    {
        if (cn == null)
        {
            if (strConexion != null && strConexion.Length > 0)
            {
                try
                {
                    cn = new SqlConnection(strConexion);
                    cn.Open();
                    return true;
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Error al conectarse a la base de datos:\n" + e.Message);
                }
            }
            else return false;
        }
        else return true;
    }

    public bool Cerrar()
    {
        try { cn.Close(); return true; }
        catch (Exception e)
        {
            throw new ArgumentException("Error al cerrar la conexión a la base de datos:\n" + e.Message);
        }
    }


    public SqlDataReader ConsultarDR(String strConsulta)
    {
        if (Conectar())
            try
            {
                SqlCommand cmd = new SqlCommand(strConsulta, cn);
                return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error al consultar la base de datos:\n" + e.Message);
            }
        else return null;
    }

    public DataTable Consultar(String strConsulta)
    {
        if (Conectar())
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(strConsulta, cn);
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error al consultar la base de datos:\n" + e.Message);
            }
        else return null;
    }

    public bool Actualizar(String strConsulta)
    {
        if (Conectar())
            try
            {
                SqlCommand cmd = new SqlCommand(strConsulta, cn);
                cmd.CommandTimeout = 1200;
                cmd.ExecuteNonQuery();
                cmd = null;
                return true;
            }
            catch (Exception e)
            {
                throw new ArgumentException("Error al consultar la base de datos:\n" + e.Message);
            }
        else return false;
    }

    //Metodo para Obtener la imagen binaria de un campo tipo imagen
    public byte[] ObtenerImagen(String strConsulta)
    {
        //Ejecutar la consulta
        DataTable tbl = Consultar(strConsulta);

        //verificar que la consulta devuelve registros
        if (tbl != null && tbl.Rows.Count > 0 &&
            !Convert.IsDBNull(tbl.Rows[0][0]))
        {

            return (byte[])tbl.Rows[0][0];
        }
        else
            return null;

    }

}