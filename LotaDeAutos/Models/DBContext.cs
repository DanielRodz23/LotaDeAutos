using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotaDeAutos.Models
{
    public class DBContext
    {
        private const string conectionstring = "Data Source=loteautos.db";
        public DBContext()
        {
            using (var connection = new SqliteConnection(conectionstring))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"
                                        CREATE TABLE IF NOT EXISTS autos (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Marca TEXT NOT NULL,
                                        Modelo TEXT NOT NULL,
                                        Version TEXT NOT NULL,
                                        Anio INTEGER NOT NULL,
                                        Precio DECIMAL NOT NULL,
                                        Kilometraje INTEGER NOT NULL,
                                        Motor TEXT NOT NULL,
                                        Transmicion TEXT NOT NULL,
                                        Carroceria TEXT NOT NULL,
                                        Descripcion TEXT NOT NULL
                                        );
                                       ";
                command.ExecuteNonQuery(); 
                //No regresa registros
                //command.ExecuteReader(); Regresa registros
            }
        }
        public async Task Agregar(AutoModel auto)
        {
            using (var connection = new SqliteConnection(conectionstring))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO autos 
                                        (Marca, Modelo, Version, Anio, Precio, Kilometraje, Motor, Transmicion, Carroceria, Descripcion)
                                        values ($Marca, $Modelo, $Version, $Anio, $Precio, $Kilometraje, $Motor, $Transmicion, $Carroceria, $Descripcion)";
                command.Parameters.AddWithValue("$" + nameof(auto.Marca), auto.Marca);
                command.Parameters.AddWithValue("$" + nameof(auto.Modelo), auto.Modelo);
                command.Parameters.AddWithValue("$" + nameof(auto.Version), auto.Version);
                command.Parameters.AddWithValue("$" + nameof(auto.Anio), auto.Anio);
                command.Parameters.AddWithValue("$" + nameof(auto.Precio), auto.Precio);
                command.Parameters.AddWithValue("$" + nameof(auto.Kilometraje), auto.Kilometraje);
                command.Parameters.AddWithValue("$" + nameof(auto.Motor), auto.Motor);
                command.Parameters.AddWithValue("$" + nameof(auto.Transmicion), auto.Transmicion);
                command.Parameters.AddWithValue("$" + nameof(auto.Carroceria), auto.Carroceria);
                command.Parameters.AddWithValue("$" + nameof(auto.Descripcion), auto.Descripcion);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Actualizar(AutoModel auto)
        {
            using (var connection = new SqliteConnection(conectionstring))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"UPDATE autos 
                                        SET Marca = $Marca, 
                                            Modelo = $Modelo, 
                                            Version = $Version, 
                                            Anio = $Anio, 
                                            Precio = $Precio, 
                                            Kilometraje = $Kilometraje, 
                                            Motor = $Motor, 
                                            Transmicion = $Transmicion, 
                                            Carroceria = $Carroceria, 
                                            Descripcion = $Descripcion
                                        WHERE Id = $Id
                                        ";
                command.Parameters.AddWithValue("$" + nameof(auto.Marca), auto.Marca);
                command.Parameters.AddWithValue("$" + nameof(auto.Modelo), auto.Modelo);
                command.Parameters.AddWithValue("$" + nameof(auto.Version), auto.Version);
                command.Parameters.AddWithValue("$" + nameof(auto.Anio), auto.Anio);
                command.Parameters.AddWithValue("$" + nameof(auto.Precio), auto.Precio);
                command.Parameters.AddWithValue("$" + nameof(auto.Kilometraje), auto.Kilometraje);
                command.Parameters.AddWithValue("$" + nameof(auto.Motor), auto.Motor);
                command.Parameters.AddWithValue("$" + nameof(auto.Transmicion), auto.Transmicion);
                command.Parameters.AddWithValue("$" + nameof(auto.Carroceria), auto.Carroceria);
                command.Parameters.AddWithValue("$" + nameof(auto.Descripcion), auto.Descripcion);
                command.Parameters.AddWithValue("$" + nameof(auto.Id), auto.Id);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Eliminar(int id)
        {
            using (var connection = new SqliteConnection(conectionstring))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"DELETE from autos
                                        WHERE Id = $Id
                                        ";
                command.Parameters.AddWithValue("$Id", id);

                await command.ExecuteNonQueryAsync();
            }
        }


        public async Task<AutoModel> GetById(int id)
        {
            AutoModel? auto = null;
            if (id <= 0)
            {
                throw new ArgumentException("El id no debe ser mayor a cero.");
            }

            using (var connection = new SqliteConnection(conectionstring))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM autos
                                        WHERE Id = $Id";
                command.Parameters.AddWithValue("$Id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                    }

                    auto = new AutoModel
                    {
                        Id = reader.GetInt32(0),          //entre parentesis es el num de la columna correspondiente.
                        Marca = reader.GetString(1),
                        Modelo=reader.GetString(2),
                        Version= reader.GetString(3),
                        Anio=reader.GetInt16(4),
                        Precio=reader.GetDecimal(5),
                        Kilometraje=reader.GetInt32(6),
                        Motor= reader.GetString(7),
                        Transmicion=reader.GetString(8),
                        Carroceria=reader.GetString(9),
                        Descripcion=reader.GetString(10)
                    };
                }


            }

            return auto;
        }


        public async Task<IEnumerable<AutoModel>> GetAll()
        {
            List<AutoModel> listaautos = null;

            using (var connection = new SqliteConnection(conectionstring))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT Id, Marca, Modelo, Version, Anio, Precio, Kilometraje, Motor, Transmicion, Carroceria, Descripcion
                                        FROM autos";
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())//mientas existan mas filas
                {
                    if (listaautos == null)
                    {
                        listaautos = new();
                    }
                    listaautos.Add(new AutoModel
                    {
                        Id = reader.GetInt32(0),          //entre parentesis es el num de la columna correspondiente.
                        Marca = reader.GetString(1),
                        Modelo = reader.GetString(2),
                        Version = reader.GetString(3),
                        Anio = reader.GetInt16(4),
                        Precio = reader.GetDecimal(5),
                        Kilometraje = reader.GetInt32(6),
                        Motor = reader.GetString(7),
                        Transmicion = reader.GetString(8),
                        Carroceria = reader.GetString(9),
                        Descripcion = reader.GetString(10)
                    });
                }
            }

            return listaautos;
        }
    }
}
