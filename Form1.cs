using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MxMd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.DataSource = Country_name();
             
        }

        
        public string Country_name_selected_item;
        public string Subdivision_1_name_selected_item;
        public string City_name_selected_item;
        public HashSet<string> temp = new HashSet<string>();
        public string[] csvLines = File.ReadAllLines("GeoLite2-City-Locations-en.csv");
        //public string[] csvLines1 = File.ReadAllLines(@"" + GetExecutingAssembly() + "GeoLite2-City-Blocks-IPv4.csv");
        public string locations;
        public string blocks;
        public string Text1;
        





        
       
        List<string> Country_name()
        {
            
            temp.Clear();
            
            
            for (int i = 0; i < csvLines.Length; i++)
            {
                string[] rowData = csvLines[i].Split(',');
                temp.Add(rowData[5]);

            }
            List<string> ltemp = new List<string>();
            ltemp.Clear();
            ltemp = temp.ToList();
            ltemp.Sort();
            return ltemp;

        }

        List<string> Subdivision_1_name(string country_name_selected_item)
        {

            
            temp.Clear();
            for (int i = 0; i < csvLines.Length; i++)
            {
                string[] rowData = csvLines[i].Split(',');
                for (int j = 0; j < rowData.Length; j++)
                { if (rowData[j] == country_name_selected_item) 
                    {
                        temp.Add(rowData[7]);
                    }
                }
                

            }

            
            List<string> ltemp = new List<string>();
            ltemp.Clear();
            ltemp = temp.ToList();
            ltemp.Sort();
            return ltemp;
        }

        List<string> City_name( string subdivision_1_name_selected_item)
        {
            
            
            temp.Clear();
            for (int i = 0; i < csvLines.Length; i++)
            {
                string[] rowData = csvLines[i].Split(',');
                for (int j = 0; j < rowData.Length; j++)
                {
                    if (rowData[j] == subdivision_1_name_selected_item)
                    {
                        temp.Add(rowData[10]);
                    }
                }


            }

            List<string> ltemp = new List<string>();
            ltemp.Clear();
            ltemp = temp.ToList();
            ltemp.Sort();
            return ltemp;
        }
        List<string> Geoname_id_list(string country_name_selected_item)
         {
             List<string> ltemp = new List<string>();
             for (int i = 0; i < csvLines.Length; i++)
             {
                 string[] rowData = csvLines[i].Split(',');
                 for (int j = 0; j < rowData.Length; j++)
                 {
                     if (rowData[j] == country_name_selected_item)
                     {
                         ltemp.Add(rowData[0]);
                     }
                 }
             }
             ltemp.Sort();
             return ltemp;
         }
        List<string> ip_list(List<string> geoname_id_list)
        {

            List<string> ltemp = new List<string>();
            string line;
            using (var sr = new StreamReader(new BufferedStream(File.OpenRead("GeoLite2-City-Blocks-IPv4.csv"), 10 * 1024 * 1024)))
            {

                while ((line = sr.ReadLine()) != null)
                {
                    string[] line1 = line.Split(',');
                    for (int i = 0; i < geoname_id_list.Count; i++)
                    {
                        if (line1[1] == geoname_id_list[i])
                        {
                            ltemp.Add(line1[0]);
                        }
                    }

                }


            }

            ltemp.Sort();
            return ltemp;
        }
       

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            City_name_selected_item = null;
            string country_name_selected_item = comboBox1.SelectedItem as string;
            comboBox3.DataSource= Subdivision_1_name(country_name_selected_item);
            Country_name_selected_item = country_name_selected_item;
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string subdivision_1_name_selected_item = comboBox3.SelectedItem as string;
            comboBox5.DataSource = City_name(subdivision_1_name_selected_item);
            Subdivision_1_name_selected_item = subdivision_1_name_selected_item;
        }

        void comboBox5_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string city_name_selected_item = comboBox5.SelectedItem as string;
            City_name_selected_item = city_name_selected_item;
            
        }

        

        async private void generate_Click(object sender, EventArgs e)
        {
            Text1 = "";
            

            //await Task.Run(() =>
            //{
                if (Country_name_selected_item != null)
                {
                    if (Subdivision_1_name_selected_item != null)
                    {
                        if (City_name_selected_item != null)
                        { 
                            
                            
                            await Task.Run(() => { Text1 = string.Join("\n", (/**/ip_list(Geoname_id_list(City_name_selected_item)))); });
                        
                            
                        //else { await Task.Run(() => { Text2 = string.Join("\n", (/**/Postal_code(Geoname_id_list(City_name_selected_item)))); }); }

                            

                        }
                        else
                        {
                            await Task.Run(() => { Text1 = string.Join("\n", (/**/ip_list(Geoname_id_list(Subdivision_1_name_selected_item)))); });

                        }
                    }
                    else
                        await Task.Run(() => { Text1 = string.Join("\n", (/**/ip_list(Geoname_id_list(Country_name_selected_item)))); });


                }
            //});



            textBox1.Text =  Text1;
        }

      
    }

   
    
}
