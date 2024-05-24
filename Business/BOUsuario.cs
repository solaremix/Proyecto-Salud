using Interface.Dto.Request;
using Interface.Dto.Response;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Interface.Dto;
using System.IO;

namespace Business
{
    public class BOUsuario
    {
        private readonly DOProyectoSalud _dataAccess;

        public BOUsuario()
        {
            _dataAccess = new DOProyectoSalud();
        }

        public void RegistrarUsuario(UsuarioDto usuario)
        {
            // Insertar datos del padre
            int padreId = _dataAccess.InsertarPadre(usuario.padre);

            // Establecer el id del padre en el objeto UsuarioDto
            usuario.padre.id = padreId;

            // Insertar datos del usuario con el id del padre
            _dataAccess.InsertarUsuario(usuario);
        }
    }
}
