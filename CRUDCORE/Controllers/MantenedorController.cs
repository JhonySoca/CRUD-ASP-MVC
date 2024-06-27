using Microsoft.AspNetCore.Mvc;
using CRUDCORE.Datos;
using CRUDCORE.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;

namespace CRUDCORE.Controllers
{
    public class MantenedorController : Controller
    {

        ContactoDatos _ContactoDatos = new ContactoDatos();
        public IActionResult Listar()
        { //MOSTRARA UNA LISTA DE CONTACTOS
            var oLista = _ContactoDatos.Listar();

            return View(oLista);
        }

        public IActionResult Guardar()
        {//DEVOLVER SOLO LA VISTA

            return View();
        }


        [HttpPost]
        //
        public IActionResult Guardar(ContactoModel oContacto)
        {//RECIBIR UN OBJETO Y GUARDARLO EN LA BASE DE DATOS
            if (!ModelState.IsValid)
                return View();

            var respuesta = _ContactoDatos.Guardar(oContacto);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }


        public IActionResult Editar(int IdContacto)
        {//DEVOLVER SOLO LA VISTA
            var ocontacto = _ContactoDatos.Obtener(IdContacto);
            return View(ocontacto);
        }


        [HttpPost]
        public IActionResult Editar(ContactoModel oContacto)
        {//DEVOLVER SOLO LA VISTA
            if (!ModelState.IsValid)
                return View();

            var respuesta = _ContactoDatos.Editar(oContacto);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }



        public IActionResult Eliminar(int IdContacto)
        {//DEVOLVER SOLO LA VISTA
            var ocontacto = _ContactoDatos.Obtener(IdContacto);
            return View(ocontacto);
        }


        [HttpPost]
        public IActionResult Eliminar(ContactoModel oContacto)
        {//DEVOLVER SOLO LA VISTA
            

            var respuesta = _ContactoDatos.Eliminar(oContacto.IdContacto);
            if (respuesta)
                return RedirectToAction("Listar");
            else
                return View();
        }






        public IActionResult ExportarPDF()
        {
            var contactos = _ContactoDatos.Listar();

            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
            document.Open();

            Paragraph title = new Paragraph("Lista de Contactos", FontFactory.GetFont(FontFactory.HELVETICA, 18, Font.BOLD));
            title.SpacingAfter = 20f; // Añadir espacio después del título
            document.Add(title);
            PdfPTable table = new PdfPTable(3);
            table.AddCell("Nombre");
            table.AddCell("Telefono");
            table.AddCell("Correo");

            foreach (var item in contactos)
            {
                table.AddCell(item.Nombre);
                table.AddCell(item.Telefono);
                table.AddCell(item.correo);
            }

            document.Add(table);
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return File(workStream, "application/pdf", "ListaContactos.pdf");
        }


    }
}


