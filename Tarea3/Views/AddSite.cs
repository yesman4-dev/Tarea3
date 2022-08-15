using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public class AddSite : ContentPage
    {
        byte[] cam_image;

        private Entry nombre;
        private Entry Apellidos;
        private Entry edad;
        private Entry direccion;
        private Entry puesto;

        private Image imagePreview;

        private Button cam_imgButton;
        private Button saveButton;

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "myDbSite.db3");

        public AddSite()
        {
            this.Title = "Agregar nuevo Empleado";
            StackLayout stackLayout = new StackLayout();

            //Camera Btn
            cam_imgButton = new Button();
            cam_imgButton.Text = "Tomar Foto";
            cam_imgButton.Clicked += cam_imgButton_Clicked;
            stackLayout.Children.Add(cam_imgButton);

            //Image Preview
            imagePreview = new Image();
            stackLayout.Children.Add(imagePreview);

            // nombre Entry
            nombre = new Entry();
            nombre.Keyboard = Keyboard.Text;
            nombre.Placeholder = "Nombre";
            stackLayout.Children.Add(nombre);

            // Apellidos Entry
            Apellidos = new Entry();
            Apellidos.Keyboard = Keyboard.Text;
            Apellidos.Placeholder = "Apellidos";
            stackLayout.Children.Add(Apellidos);

            // Edad Entry
            edad = new Entry();
            edad.Keyboard = Keyboard.Numeric;
            edad.Placeholder = "edad";
            stackLayout.Children.Add(edad);

            // direccion Entry
            direccion = new Entry();
            direccion.Keyboard = Keyboard.Text;
            direccion.Placeholder = "direccion";
            stackLayout.Children.Add(direccion);

            // puesto Entry
            puesto = new Entry();
            puesto.Keyboard = Keyboard.Text;
            puesto.Placeholder = "puesto";
            stackLayout.Children.Add(puesto);

            //Save Btn
            saveButton = new Button();
            saveButton.Text = "Guardar";
            saveButton.Clicked += saveButton_Clicked;
            stackLayout.Children.Add(saveButton);

            Content = stackLayout;
            
            
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

       

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Sitio>();

            var maxPK = db.Table<Sitio>().OrderByDescending(c => c.Id).FirstOrDefault();

            if (!string.IsNullOrEmpty(nombre.Text))
            {
                Sitio sitio = new Sitio()
                {
                    Id = (maxPK == null ? 1 : maxPK.Id + 1),
                    Nombre = nombre.Text,
                    Apellidos = Apellidos.Text,
                    Edad = edad.Text,
                    Direccion = direccion.Text,
                    Puesto = puesto.Text,
                    save_image = cam_image
                };

                db.Insert(sitio);
                await DisplayAlert(null, "Empleado: " + sitio.Nombre + " Guardado!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "OK");
            }
        }
    }
}