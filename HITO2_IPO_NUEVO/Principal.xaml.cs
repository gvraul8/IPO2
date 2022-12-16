﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml;

namespace HITO2_IPO_NUEVO
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
        private MainWindow login;
        List<Ruta> listadoRutas = new List<Ruta>();
        List<PuntoInteres> listadoPuntosInteres = new List<PuntoInteres>();
        List<Excursionista> listadoExcursionistas = new List<Excursionista>();
        List<Guia> listadoGuias = new List<Guia>();
        List<Oferta> listadoOfertas = new List<Oferta>();
        Usuario user;
        private BitmapImage imagCheck = new BitmapImage(new Uri("/Imagenes/check.png", UriKind.Relative)); 
        private BitmapImage imagCross = new BitmapImage(new Uri("/Imagenes/incorrect.png", UriKind.Relative));

        public Principal(Usuario u)
        {
            user = u;
            InitializeComponent();
            PrintUserData();


            listadoRutas = CargarContenidoRutasXML();
            imprimirNombreRutas();
            inicializaComponentesRutas();

            listadoExcursionistas = CargarContenidoExcursionistasXML();
            imprimirNombreExcursionistas();
            inicializaComponenentesExcursionistas();

            listadoGuias = CargarContenidoGuiasXML();
            imprimirNombreGuias();
            inicializaComponenentesGuias();

            listadoOfertas = CargarContenidoOfertasXML();
            imprimirNombreOfertas();
            inicializaComponenentesOfertas();

            listadoPuntosInteres = CargarContenidoPuntosInteresXML();
            inicializaComponenentesPuntosInteres();


        }

        void PrintUserData()
        {
            lbNombreUsuario.Content = user.Name.ToString(); ;
            lbApellidosUsuario.Content = user.LastName.ToString();
            lbEmailUsuario.Content=user.Email.ToString();  
           // lbUltimoAccesoUsuario.Content = user.LastLogin.ToString();

            var fullFilePath = user.ImgUrl.ToString();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            imgUsuario.Source = bitmap;
        }

        private void btCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            login= new MainWindow();
            login.Show();
            this.Close();
        }

        private void btn_Ayuda_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/gvraul8/IPO/wiki/AYUDA");
        }

        private void InsertarRutaEnXML(List<Ruta> listadoAux)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode cabecera = doc.CreateElement("Rutas");
            doc.AppendChild(cabecera);
            doc.InsertBefore(doc.CreateXmlDeclaration("1.0","UTF-8",null), cabecera);
           
            XmlNode rutaInsertar = doc.CreateElement("Ruta");

            foreach (Ruta rutaAux in listadoAux)
            {
                rutaInsertar = doc.CreateElement("Ruta");

                XmlAttribute nombre = doc.CreateAttribute("Nombre");
                nombre.Value = rutaAux.Nombre;
                rutaInsertar.Attributes.Append(nombre);

                XmlAttribute origen = doc.CreateAttribute("Origen");
                origen.Value = rutaAux.Origen;
                rutaInsertar.Attributes.Append(origen);

                XmlAttribute destino = doc.CreateAttribute("Destino");
                destino.Value = rutaAux.Destino;
                rutaInsertar.Attributes.Append(destino);

                XmlAttribute provincia = doc.CreateAttribute("Provincia");
                provincia.Value = rutaAux.Provincia;
                rutaInsertar.Attributes.Append(provincia);

                XmlAttribute fecha = doc.CreateAttribute("Fecha");
                fecha.Value = rutaAux.Fecha.ToString();
                rutaInsertar.Attributes.Append(fecha);

                XmlAttribute dificultad = doc.CreateAttribute("Dificultad");
                dificultad.Value = rutaAux.Dificultad;
                rutaInsertar.Attributes.Append(dificultad);

                XmlAttribute plazasDisponibles = doc.CreateAttribute("PlazasDisponibles");
                plazasDisponibles.Value = rutaAux.PlazasDisponibles.ToString();
                rutaInsertar.Attributes.Append(plazasDisponibles);

                XmlAttribute materialNecesario = doc.CreateAttribute("MaterialNecesario");
                materialNecesario.Value = rutaAux.MaterialNecesario;
                rutaInsertar.Attributes.Append(materialNecesario);

                XmlAttribute numeroRealizaciones = doc.CreateAttribute("NumeroDeRealizaciones");
                numeroRealizaciones.Value = rutaAux.NumeroDeRealizaciones.ToString();
                rutaInsertar.Attributes.Append(numeroRealizaciones);

                XmlAttribute url_Ruta = doc.CreateAttribute("URL_RUTA");
                url_Ruta.Value = rutaAux.URL_RUTA.ToString();
                rutaInsertar.Attributes.Append(url_Ruta);

                cabecera.AppendChild(rutaInsertar);
            }
            
            doc.Save("rutas2.xml");
            //doc.Save("Datos/rutas.xml");
        }

        private void bt_editars_Click(object sender, RoutedEventArgs e)
        {
            tcPestanas.SelectedIndex = 1;
        }

        private void edicionTextBox(Boolean bloqueado)
        {
            tb_nombre.IsReadOnly = bloqueado;
            tb_origen.IsReadOnly = bloqueado;
            tb_destino.IsReadOnly = bloqueado;
            tb_provincia.IsReadOnly = bloqueado;
            //dp_fecha.IsReadOnly = activado;
            tb_dificultad.IsReadOnly = bloqueado;
            tb_plazas.IsReadOnly = bloqueado;

            if (bloqueado == false)
            {
                tb_nombre.Text = "Escriba aqui el nombre de la ruta";
                bt_editars.Content = "Seleccionar guia";
                bt_consultarPDis.Content = "Añadir PDI";
                bt_consultarPDis.IsEnabled = false;
                tb_nombre.Focus();
            }

        }

    

















        //RUTAS --------------------------------------------------------------------------------



        private void imprimirNombreRutas()
        {
            foreach (Ruta ruta in listadoRutas)
            {
                ListBoxRutas.Items.Add(ruta.Nombre);
            }
        }

        private List<Ruta> CargarContenidoRutasXML()
        {
            List<Ruta> listado = new List<Ruta>();
            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();

            var fichero = Application.GetResourceStream(new Uri("Datos/rutas.xml", UriKind.Relative));
            //var fichero = Application.GetResourceStream(new Uri("bin/Debug/rutas.xml", UriKind.Relative));

            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                //Ruta(string nombre, string origen, string destino, string provincia, DateTime fecha, string dificultad, int plazasDisponibles, string material, int numRealizaciones)
                var nuevaRuta = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);
                nuevaRuta.Nombre = node.Attributes["Nombre"].Value;
                nuevaRuta.Origen = node.Attributes["Origen"].Value;
                nuevaRuta.Destino = node.Attributes["Destino"].Value;
                nuevaRuta.Provincia = node.Attributes["Provincia"].Value;
                nuevaRuta.Fecha = Convert.ToDateTime(node.Attributes["Fecha"].Value);
                nuevaRuta.Dificultad = node.Attributes["Dificultad"].Value;
                nuevaRuta.PlazasDisponibles = Convert.ToInt32(node.Attributes["PlazasDisponibles"].Value);
                nuevaRuta.MaterialNecesario = node.Attributes["MaterialNecesario"].Value;
                nuevaRuta.NumeroDeRealizaciones = Convert.ToInt32(node.Attributes["NumeroDeRealizaciones"].Value);
                nuevaRuta.URL_RUTA = new Uri(node.Attributes["URL_RUTA"].Value, UriKind.Absolute);
                //nuevaRuta.URL_INTERES = new Uri(node.Attributes["URL_INTERES"].Value, UriKind.Absolute);
                listado.Add(nuevaRuta);
            }
            return listado;
        }

        private void inicializaComponentesRutas()
        {
            tb_nombre.Text = "";
            tb_origen.Text = "";
            tb_destino.Text = "";
            tb_provincia.Text = "";
            tb_dificultad.Text = "";
            tb_plazas.Text = "";
            dp_fecha.Text = "";
            img_ruta.Source = new BitmapImage();


            bt_anadir.IsEnabled = true;
            bt_editar.IsEnabled = false;
            bt_eliminar.IsEnabled = false;

            bt_anadirs.IsEnabled = false;  //poner a true cuando se pulse el de añadir
            bt_consultarPDis.IsEnabled = false;

            //quedaría deshabilitar boton GUARDAR CUANDO LO CREEN (SE ACTIVARÁ SÓLO CUANDO E PULSE AÑADIR O EDITAR)
        }

        private void rellenaCasillasRuta(object sender, SelectionChangedEventArgs e)
        {


            if (ListBoxRutas.SelectedIndex > -1)
            {
                int index = ListBoxRutas.SelectedIndex;
                var rutaAux = listadoRutas[index];

                tb_nombre.Text = rutaAux.Nombre.ToString();
                tb_origen.Text = rutaAux.Origen.ToString();
                tb_destino.Text = rutaAux.Origen.ToString();
                tb_provincia.Text = rutaAux.Provincia.ToString();
                tb_dificultad.Text = rutaAux.Dificultad.ToString();
                tb_plazas.Text = rutaAux.PlazasDisponibles.ToString();
                //tb_material.Text = rutaAux.MaterialNecesario.ToString();
                //tb_realizaciones.Text = rutaAux.NumeroDeRealizaciones.ToString();
                dp_fecha.Text = Convert.ToDateTime(rutaAux.Fecha.ToString()).ToString();

                // https://stackoverflow.com/questions/18435829/showing-image-in-wpf-using-the-url-link-from-database
                var fullFilePath = rutaAux.URL_RUTA.ToString();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();

                img_ruta.Source = bitmap;

                //var fullFilePath2 = rutaAux.URL_INTERES.ToString();
                //BitmapImage bitmap2 = new BitmapImage();
                //bitmap2.BeginInit();
                //bitmap2.UriSource = new Uri(fullFilePath2, UriKind.Absolute);
                //bitmap2.EndInit();

                //img_interesRuta.Source = bitmap2;

                bt_anadir.IsEnabled = false;
                bt_editar.IsEnabled = true;
                bt_eliminar.IsEnabled = true;

                bt_consultarPDis.IsEnabled = true;
            }
            else
            {
                inicializaComponentesRutas();
            }

        }

        private void bt_consultarPDis_Click(object sender, RoutedEventArgs e)
        {
            bt_consultarPDis.IsEnabled = true;
            
            tcPestanas.SelectedIndex = 4;

            int index = ListBoxRutas.SelectedIndex;
            var rutaAux = listadoRutas[index];
            imprimirNombrePuntosInteres(rutaAux.Nombre);
        }

        private void click_añadir_Ruta(object sender, RoutedEventArgs e)
        {
            inicializaComponentesRutas();
        }



















        //EXCURSIONISTAS ---------------------------------------------------------


        private void imprimirNombreExcursionistas()
        {
            foreach (Excursionista excursionista in listadoExcursionistas)
            {
                ListBoxExcursionistas.Items.Add(excursionista.Name);
            }

        }

        private List<Excursionista> CargarContenidoExcursionistasXML()
        {
            listadoRutas = CargarContenidoRutasXML();
            List<Excursionista> listado = new List<Excursionista>();

            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/excursionistas.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var rutaAux = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);
                var nuevoExcursionista = new Excursionista("", "", "", "", "", DateTime.Today, "", 1, true, listadoRutas, listadoRutas);
                nuevoExcursionista.User = node.Attributes["User"].Value;
                nuevoExcursionista.Pass = node.Attributes["Pass"].Value;
                nuevoExcursionista.Name = node.Attributes["Name"].Value;
                nuevoExcursionista.LastName = node.Attributes["LastName"].Value;
                nuevoExcursionista.Email = node.Attributes["Email"].Value;
                nuevoExcursionista.LastLogin = Convert.ToDateTime(node.Attributes["LastLogin"].Value);
                nuevoExcursionista.Telefono = node.Attributes["Phone"].Value;
                nuevoExcursionista.ImgUrl = new Uri(node.Attributes["ImgUrl"].Value);
                nuevoExcursionista.Edad = int.Parse(node.Attributes["Edad"].Value);
                nuevoExcursionista.Notificaciones = Convert.ToBoolean(node.Attributes["Notificaciones"].Value);

                string[] rutasRealizadas = node.Attributes["RutasRealizadas"].Value.Split(',');
                List<Ruta> listadoRutasRealizadas = new List<Ruta>();
                foreach (string nombreRutaRealizadas in rutasRealizadas)
                {
                    for (int i = 0; i < listadoRutas.Count; i++)
                    {
                        if (listadoRutas[i].Nombre == nombreRutaRealizadas)
                        {
                            listadoRutasRealizadas.Add(listadoRutas[i]);
                        }
                    }
                }
                nuevoExcursionista.RutasRealizadas = listadoRutasRealizadas;

                string[] RutasFuturas = node.Attributes["RutasFuturas"].Value.Split(',');
                List<Ruta> listadoRutasFuturas = new List<Ruta>();
                foreach (string nombreRutaFuturas in RutasFuturas)
                {
                    for (int i = 0; i < listadoRutas.Count; i++)
                    {
                        if (listadoRutas[i].Nombre == nombreRutaFuturas)
                        {
                            listadoRutasFuturas.Add(listadoRutas[i]);
                        }
                    }
                }
                nuevoExcursionista.RutasFuturas = listadoRutasFuturas;

                listado.Add(nuevoExcursionista);
            }
            return listado;
        }

        void inicializaComponenentesExcursionistas()
        {
            cb_rutasEx.IsEnabled = false;
            cb_ofertas.IsEnabled = false;
            tb_nombre_excursionista.Text = "";
            tb_edad.Text =  "";
            tb_telefonoexcursionista.Text = "";
            tb_correoexcursionista.Text = "";
            bt_anadirExcursionista.IsEnabled = true;
            bt_editarExcursionista.IsEnabled = false;
            bt_eliminarExcursionista.IsEnabled = false;
            lb_rutasrealplaEx.Items.Clear();

            //quedaría deshabilitar boton GUARDAR CUANDO LO CREEN (SE ACTIVARÁ SÓLO CUANDO E PULSE AÑADIR O EDITAR)
        }

        private void rellenaCasillasExcursionista(object sender, SelectionChangedEventArgs e)
        {

            if (ListBoxExcursionistas.SelectedIndex > -1)
            {

                lb_rutasrealplaEx.Items.Clear();
                cb_ofertas.IsEnabled = true;
                cb_rutasEx.IsEnabled = true;

                int index = ListBoxExcursionistas.SelectedIndex;
                var excursionistaAux = listadoExcursionistas[index];

                tb_nombre_excursionista.Text = excursionistaAux.Name;
                tb_edad.Text = excursionistaAux.Edad.ToString();
                tb_telefonoexcursionista.Text = excursionistaAux.Telefono;
                tb_correoexcursionista.Text = excursionistaAux.Email;
                if (excursionistaAux.Notificaciones)
                {
                   
                }

                bt_anadirExcursionista.IsEnabled = false;
                bt_editarExcursionista.IsEnabled = true;
                bt_eliminarExcursionista.IsEnabled = true;
            }
            else 
            {
                inicializaComponenentesExcursionistas();
            }
        }

        private void ComboBoxRutasExcursionistas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = cb_rutasEx.SelectedValue.ToString();

            if (selectedItem.Equals("System.Windows.Controls.ComboBoxItem: Rutas Realizadas"))
            {
                imprimeRutasRealizarExcursionista();
            }
            else if (selectedItem.Equals("System.Windows.Controls.ComboBoxItem: Rutas Planificadas"))
            {
                imprimeRutasFuturasExcursionista();
            }
        }

        private void imprimeRutasRealizarExcursionista()
        {
            lb_rutasrealplaEx.Items.Clear();
            int index = ListBoxExcursionistas.SelectedIndex;
            var excursionistaAux = listadoExcursionistas[index];

            foreach (Ruta ruta in excursionistaAux.RutasRealizadas)
            {
                lb_rutasrealplaEx.Items.Add(ruta.Nombre + " (" + ruta.Fecha + ")");
            }
        }

        private void imprimeRutasFuturasExcursionista()
        {
            lb_rutasrealplaEx.Items.Clear();
            int index = ListBoxExcursionistas.SelectedIndex;
            var excursionistaAux = listadoExcursionistas[index];

            foreach (Ruta ruta in excursionistaAux.RutasFuturas)
            {
                lb_rutasrealplaEx.Items.Add(ruta.Nombre + " (" + ruta.Fecha + ")");
            }
        }











        //GUÍAS ------------------------------------------------------------------------


        private void imprimirNombreGuias()
        {
            foreach (Guia guia in listadoGuias)
            {
                ListBoxGuias.Items.Add(guia.Name);
            }

        }

        private List<Guia> CargarContenidoGuiasXML()
        {
            listadoRutas = CargarContenidoRutasXML();
            List<Guia> listado = new List<Guia>();

            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/guias.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var rutaAux = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);
                var nuevoGuia = new Guia("", "", "", "", "", DateTime.Today, "", "", "", listadoRutas, listadoRutas);
                nuevoGuia.User = node.Attributes["User"].Value;
                nuevoGuia.Pass = node.Attributes["Pass"].Value;
                nuevoGuia.Name = node.Attributes["Name"].Value;
                nuevoGuia.LastName = node.Attributes["LastName"].Value;
                nuevoGuia.Email = node.Attributes["Email"].Value;
                nuevoGuia.LastLogin = Convert.ToDateTime(node.Attributes["LastLogin"].Value);
                nuevoGuia.Telefono = node.Attributes["Phone"].Value;
                nuevoGuia.ImgUrl = new Uri(node.Attributes["ImgUrl"].Value);
                nuevoGuia.Idiomas = node.Attributes["Idiomas"].Value;
                nuevoGuia.Disponibilidad = node.Attributes["Disponibilidad"].Value;

                string[] rutasRealizadas = node.Attributes["RutasRealizadas"].Value.Split(',');
                List<Ruta> listadoRutasRealizadas = new List<Ruta>();
                foreach (string nombreRutaRealizadas in rutasRealizadas)
                {
                    for (int i = 0; i < listadoRutas.Count; i++)
                    {
                        if (listadoRutas[i].Nombre == nombreRutaRealizadas)
                        {
                            listadoRutasRealizadas.Add(listadoRutas[i]);
                        }
                    }
                }
                nuevoGuia.RutasRealizadas = listadoRutasRealizadas;

                string[] RutasFuturas = node.Attributes["RutasFuturas"].Value.Split(',');
                List<Ruta> listadoRutasFuturas = new List<Ruta>();
                foreach (string nombreRutaFuturas in RutasFuturas)
                {
                    for (int i = 0; i < listadoRutas.Count; i++)
                    {
                        if (listadoRutas[i].Nombre == nombreRutaFuturas)
                        {
                            listadoRutasFuturas.Add(listadoRutas[i]);
                        }
                    }
                }
                nuevoGuia.RutasFuturas = listadoRutasFuturas;

                listado.Add(nuevoGuia);
            }
            return listado;
        }

        void inicializaComponenentesGuias()
        {

            cb_rutasGuias.IsEnabled = false;

            tb_nombreguia.Text = "";
            tb_idiomas.Text = "";
            tb_correoguia.Text = "";
            tb_disponibilidad.Text = "";

            bt_anadirGuia.IsEnabled = true;
            bt_editarGuia.IsEnabled = false;
            bt_eliminarGuia.IsEnabled = false;
            lb_rutasrealplaGuias.Items.Clear();

            //quedaría deshabilitar boton GUARDAR CUANDO LO CREEN (SE ACTIVARÁ SÓLO CUANDO E PULSE AÑADIR O EDITAR)
        }

        private void rellenaCasillasGuias(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxGuias.SelectedIndex > -1)
            {
                lb_rutasrealplaGuias.Items.Clear();
                cb_rutasGuias.IsEnabled = true;

                int index = ListBoxGuias.SelectedIndex;
                var guiaAux = listadoGuias[index];

                tb_nombreguia.Text = guiaAux.Name;
                tb_idiomas.Text = guiaAux.Idiomas;
                tb_telefonoguia.Text = guiaAux.Telefono;
                tb_correoguia.Text = guiaAux.Email;
                tb_disponibilidad.Text = guiaAux.Disponibilidad;

                bt_anadirGuia.IsEnabled = false;
                bt_editarGuia.IsEnabled = true;
                bt_eliminarGuia.IsEnabled = true;
            }
            else
            {
                inicializaComponenentesGuias();
            }

        }


        private void ComboBoxRutasGuias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = cb_rutasGuias.SelectedValue.ToString();

            if (selectedItem.Equals("System.Windows.Controls.ComboBoxItem: Rutas Realizadas"))
            {
                imprimeRutasRealizarGuia();
            }
            else if (selectedItem.Equals("System.Windows.Controls.ComboBoxItem: Rutas Planificadas"))
            {
                imprimeRutasFuturasGuia();
            }
        }

        private void imprimeRutasRealizarGuia()
        {
            lb_rutasrealplaGuias.Items.Clear();
            int index = ListBoxGuias.SelectedIndex;
            var guiaAux = listadoGuias[index];

            foreach (Ruta ruta in guiaAux.RutasRealizadas)
            {
                lb_rutasrealplaGuias.Items.Add(ruta.Nombre + " (" + ruta.Fecha + ")");
            }
        }

        private void imprimeRutasFuturasGuia()
        {
            lb_rutasrealplaGuias.Items.Clear();
            int index = ListBoxGuias.SelectedIndex;
            var guiaAux = listadoGuias[index];

            foreach (Ruta ruta in guiaAux.RutasFuturas)
            {
                lb_rutasrealplaGuias.Items.Add(ruta.Nombre + " (" + ruta.Fecha + ")");
            }
        }

        private void click_añadir_Guia(object sender, RoutedEventArgs e)
        {
            inicializaComponenentesGuias();
        }












        //OFERTAS ---------------------------------------------------------------

        private void imprimirNombreOfertas()
        {
            foreach (Oferta oferta in listadoOfertas)
            {
                ListBoxOfertas.Items.Add(oferta.Id);
            }
        }

        private List<Oferta> CargarContenidoOfertasXML()
        {
            listadoRutas = CargarContenidoRutasXML();
            List<Oferta> listado = new List<Oferta>();

            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/ofertas.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                var rutaAux = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);
                var nuevaOferta = new Oferta(0, rutaAux, "");
                nuevaOferta.Id = int.Parse(node.Attributes["Id"].Value);
                nuevaOferta.Descripcion = node.Attributes["Descripcion"].Value;
                nuevaOferta.IMG_OFERTA = new Uri(node.Attributes["IMG_OFERTA"].Value);
                for (int i = 0; i < listadoRutas.Count; i++)
                {
                    if (listadoRutas[i].Nombre == node.Attributes["Ruta"].Value)
                    {
                        nuevaOferta.Ruta = listadoRutas[i];
                    }
                }
                listado.Add(nuevaOferta);
            }
            return listado;
        }

        void inicializaComponenentesOfertas()
        {
            tb_nombre_oferta.Text = "";
            tb_rutaOferta.Text = "";
            tb_descricpcionoferta.Text = "";
            img_oferta.Source = new BitmapImage();

            bt_anadirOferta.IsEnabled = true;
            bt_editarOferta.IsEnabled = false;
            bt_eliminarOferta.IsEnabled = false;

            //quedaría deshabilitar boton GUARDAR CUANDO LO CREEN (SE ACTIVARÁ SÓLO CUANDO E PULSE AÑADIR O EDITAR)
        }

        private void rellenaCasillasOferta(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxOfertas.SelectedIndex > -1)
            {
               
                int index = ListBoxOfertas.SelectedIndex;
                var ofertaAux = listadoOfertas[index];

                tb_nombre_oferta.Text = ofertaAux.Id.ToString();
                tb_rutaOferta.Text = ofertaAux.Ruta.Nombre;
                tb_descricpcionoferta.Text = ofertaAux.Descripcion;


                var fullFilePath = ofertaAux.IMG_OFERTA.ToString();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                bitmap.EndInit();
                img_oferta.Source = bitmap;

                bt_anadirOferta.IsEnabled = false;
                bt_editarOferta.IsEnabled = true;
                bt_eliminarOferta.IsEnabled = true;
            }
            else
            {
                inicializaComponenentesOfertas();
            }

        }











        //PUNTOS DE INTERES -------------------------------------------------------------------------------




        private List<PuntoInteres> CargarContenidoPuntosInteresXML()
        {

            listadoRutas = CargarContenidoRutasXML();

            List<PuntoInteres> listado = new List<PuntoInteres>();
            // Cargar contenido de prueba
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/puntosInteres.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                //Ruta(string nombre, string origen, string destino, string provincia, DateTime fecha, string dificultad, int plazasDisponibles, string material, int numRealizaciones)
                var rutaAux = new Ruta("", "", "", "", DateTime.Today, "", 0, "", 0);

                var nuevoPuntoInteres = new PuntoInteres("", "", rutaAux);
                nuevoPuntoInteres.Nombre = node.Attributes["Nombre"].Value;
                nuevoPuntoInteres.Descripcion = node.Attributes["Descripcion"].Value;
                for (int i = 0; i < listadoRutas.Count; i++)
                {
                    if (listadoRutas[i].Nombre == node.Attributes["NombreRuta"].Value)
                    {
                        nuevoPuntoInteres.Ruta = listadoRutas[i];
                    }
                }
                nuevoPuntoInteres.URL_IMAGEN = new Uri(node.Attributes["Imagen"].Value, UriKind.Absolute);

                listado.Add(nuevoPuntoInteres);

            }
            return listado;
        }

        private void imprimirNombrePuntosInteres(string nombreRuta)
        {
            ListBoxPDI.Items.Clear();

            foreach (PuntoInteres puntoInteres in listadoPuntosInteres)
            {
                if (puntoInteres.Ruta.Nombre == nombreRuta) {
                    ListBoxPDI.Items.Add(puntoInteres.Nombre);
                }
                
            }
        }

        private void inicializaComponenentesPuntosInteres()
        {
            lb_nombre_pdi.Content = "";
            tb_descricpcionpdi.Text = "";
            img_pdi.Source = new BitmapImage();

            bt_anadirPdi.IsEnabled = true;
            bt_editarPdi.IsEnabled = false;
            bt_eliminarPDI.IsEnabled = false;

        }

        private void rellenaCasillasPuntoInteres(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxPDI.SelectedIndex > -1)
            {
                int index = ListBoxPDI.SelectedIndex;
                
                foreach (PuntoInteres puntoInteres in listadoPuntosInteres)
                {
                    if (puntoInteres.Nombre == ListBoxPDI.SelectedItem.ToString())
                    {
                        lb_nombre_pdi.Content = "Ruta: " + puntoInteres.Nombre;
                        tb_descricpcionpdi.Text = puntoInteres.Descripcion;
                        var fullFilePath = puntoInteres.URL_IMAGEN.ToString();
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                        bitmap.EndInit();
                        img_pdi.Source = bitmap;

                    }

                }


                bt_anadirPdi.IsEnabled = false;
                bt_editarPdi.IsEnabled = true;
                bt_eliminarPDI.IsEnabled = true;
            }
            else
            {
                inicializaComponenentesPuntosInteres();
            }
        }



        //---------------  CONTROL DE ENTRADAS  -------------------------------


        private void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextNumeric(e.Text);
            if (e.Handled == false)
            {
                img_tb_plazas.Source=imagCheck;
                img_tb_plazas.Visibility = Visibility.Visible;
                img_tb_plazas.ToolTip="Formato adecuado";
            }
            else
            {
                img_tb_plazas.Source=imagCross;
                img_tb_plazas.Visibility = Visibility.Visible;
                img_tb_plazas.ToolTip="Debes introducir un formato numerico";
            }

        }

        private static bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);
        }
        private void tb_origen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                tb_destino.Focus();

            }
        }

        private void tb_destino_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                tb_provincia.Focus();
            }
        }

        private void tb_provincia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                dp_fecha.Focus();
            } 
        }

        private void dp_fecha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                tb_dificultad.Focus();
            }
        }

        private void tb_dificultad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                tb_plazas.Focus();
            }
        }

        private void bt_editar_Click(object sender, RoutedEventArgs e)
        {
            edicionTextBox(false);
            tb_nombre.Focus();
        }

        private void bt_anadir_Click(object sender, RoutedEventArgs e)
        {
            edicionTextBox(false);
            tb_nombre.Focus();
        }
        private void bt_consultarguia_Click(object sender, RoutedEventArgs e)
        {
            tcPestanas.SelectedIndex = 0;
        }
    }
}
    

