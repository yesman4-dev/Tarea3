using Tarea3.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tarea3.Views
{
    public class GetSite : ContentPage
    {
        private ListView listView;
        byte[] cam_image;

        private Entry idEntry;
        private Entry nombre;
        private Entry apellidos;
        private Entry edad;
        private Entry direccion;
        private Entry puesto;

        private Image imagePreview;
        private Button editButton;
        private Button deleteButton;

        Sitio sitio = new Sitio();

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDbSite.db3");
        public GetSite()
        {
            this.Title = " Editar Empleados";

            var db = new SQLiteConnection(dbPath);

            StackLayout stackLayout = new StackLayout();

            listView = new ListView();
            listView.ItemsSource = db.Table<Sitio>().OrderBy(x => x.Id).ToList();
            listView.ItemSelected += listViewItem;
            stackLayout.Children.Add(listView);

            //Image Preview
            imagePreview = new Image();
            stackLayout.Children.Add(imagePreview);

            // ID Entry
            idEntry = new Entry();
            idEntry.Placeholder = "ID";
            idEntry.IsVisible = false;
            stackLayout.Children.Add(idEntry);

            // Nombre Entry
            nombre = new Entry();
            nombre.Keyboard = Keyboard.Text;
            nombre.Placeholder = "nombre";
            stackLayout.Children.Add(nombre);

            // Apellidos Entry
            apellidos = new Entry();
            apellidos.Keyboard = Keyboard.Text;
            apellidos.Placeholder = "Apellidos";
            stackLayout.Children.Add(apellidos);

            // Edad Entry
            edad = new Entry();
            edad.Keyboard = Keyboard.Text;
            edad.Placeholder = "edad";
            stackLayout.Children.Add(edad);

            // Direccion Entry
            direccion = new Entry();
            direccion.Keyboard = Keyboard.Text;
            direccion.Placeholder = "direccion";
            stackLayout.Children.Add(direccion);

            // puesto Entry
            puesto = new Entry();
            puesto.Keyboard = Keyboard.Text;
            puesto.Placeholder = "puesto";
            stackLayout.Children.Add(puesto);


            //Edit Btn
            editButton = new Button();
            editButton.Text = "Actulizar datos";
            editButton.Clicked += editButton_Clicked;
            stackLayout.Children.Add(editButton);
            
            //Delete Btn
            deleteButton = new Button();
            deleteButton.Text = "Eliminar empleado";
            deleteButton.Clicked += deleteButton_Clicked;
            stackLayout.Children.Add(deleteButton);
            
            Content = stackLayout;
        }        

        private void listViewItem(object sender, SelectedItemChangedEventArgs e)
        {
            sitio = (Sitio)e.SelectedItem;

            idEntry.Text = sitio.Id.ToString();
            nombre.Text = sitio.Nombre.ToString();
            apellidos.Text = sitio.Apellidos.ToString();
            edad.Text = sitio.Edad.ToString();
            direccion.Text = sitio.Direccion.ToString();
            puesto.Text = sitio.Puesto.ToString();
           // imagePreview = sitio.save_imag
        }

        private async void cam_imgButton_Clicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.CapturePhotoAsync();
            var stream = await result.OpenReadAsync();
            imagePreview.Source = ImageSource.FromStream(() => stream);

            using (MemoryStream memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                cam_image = memory.ToArray();
            }
        }


        private async void editButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);

            if (!string.IsNullOrEmpty(nombre.Text) &&
                !string.IsNullOrEmpty(apellidos.Text) &&
                !string.IsNullOrEmpty(edad.Text) &&
                !string.IsNullOrEmpty(direccion.Text) &&
                !string.IsNullOrEmpty(puesto.Text))
            {
                Sitio sitio = new Sitio()
                {
                    Id = Convert.ToInt32(idEntry.Text),
                    Nombre = nombre.Text,
                    Apellidos = apellidos.Text,
                    Edad = edad.Text,
                    Direccion = direccion.Text,
                    Puesto = puesto.Text,
                   
                };

                db.Update(sitio);
                await DisplayAlert(null, "Empleado: " + sitio.Nombre + " Editado!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "OK");
            }
        }
        
        private async void deleteButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);
            db.Table<Sitio>().Delete(x => x.Id == sitio.Id);
            await DisplayAlert(null, "Empleado: " + sitio.Nombre + " Eliminado", "OK");
            await Navigation.PopAsync();
        }
        
        
    }
}