﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace HITO2_IPO_NUEVO
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private Principal principal;
        String defaultImg = "https://cdn-icons-png.flaticon.com/512/74/74472.png";

        public MainWindow()
        {
            InitializeComponent();
  
        }

        private void tb_usuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {

                pb_contrasena.IsEnabled = true;
                pb_contrasena.Focus();
            }
        }

        private void pb_contrasena_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btn_iniciarSesion.Focus();

            }
        }

        private void btn_iniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            List<Usuario> listado = new List<Usuario>();
            listado = CargarContenidoXML();
            foreach (Usuario usuario in listado)
            {

                if (tb_usuario.Text.Equals(usuario.User) && pb_contrasena.Password.Equals(usuario.Pass))
                {
                    principal= new Principal(usuario);
                    principal.Show();
                    this.Close();
                }
                else
                {
                    lb_errorCombinacion.Visibility = Visibility.Visible;
                }
                
            }
        }

        private void lb_olvidocontrasena_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Se ha enviado un correo de recuperación al correo asociado con el nombre de usuario introducido.");
        }

        private void btn_idioma_Click(object sender, RoutedEventArgs e)
        {

        }

        private List<Usuario> CargarContenidoXML()
        {
            List<Usuario> listado = new List<Usuario>();
            XmlDocument doc = new XmlDocument();
            var fichero = Application.GetResourceStream(new Uri("Datos/usuarios.xml", UriKind.Relative));
            doc.Load(fichero.Stream);
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                //  Usuario(string user, string pass, string name, string lastName, string email, DateTime lastLogin, Uri imgUrl)
                var nuevoUsuario = new Usuario("", "","","","", DateTime.MinValue);
                nuevoUsuario.User = node.Attributes["User"].Value;
                nuevoUsuario.Pass = node.Attributes["Pass"].Value;
                nuevoUsuario.Name = node.Attributes["Name"].Value;
                nuevoUsuario.LastName = node.Attributes["LastName"].Value;
                nuevoUsuario.Email = node.Attributes["Email"].Value;
                nuevoUsuario.LastLogin = DateTime.Now;
                if ((node.Attributes["ImgUrl"].Value) is "")
                {
                    nuevoUsuario.ImgUrl =  new Uri(defaultImg, UriKind.Absolute);
                } else
                {
                    nuevoUsuario.ImgUrl = new Uri(node.Attributes["ImgUrl"].Value, UriKind.Absolute);
                }
                
                listado.Add(nuevoUsuario);
            }
            return listado;
        }

        private void cbIdioma_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
