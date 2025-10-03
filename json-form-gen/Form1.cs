using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace json_form_gen {
    public partial class Form1 : Form {
        class Field {
            public string Name;
            public Label Label;
        }
        class FieldText : Field {
            public string DefaultText;
            public TextBox TextBox;
        }
        class FieldCheckBox : Field {
            public bool Checked;
            public CheckBox CheckBox;
        }
        class FieldNumber : Field {
            public int DefaultNumber;
            public NumericUpDown NumericUpDown;
        }
        class FieldArray : Field {
            public Field[] fields;
        }
        Field[] fields;
        public Form1() {
            InitializeComponent();
            CreateFields(new Field[] {
                new FieldNumber{Name="Number"},
                new FieldArray{Name="Object",fields=new Field[]{
                    new FieldText{Name="Name"},
                    new FieldNumber{Name="Age"},
                    new FieldArray{Name="Object",fields=new Field[]{
                        new FieldText{Name="Name"},
                        new FieldNumber{Name="Age"},
                    }},
                }},
                new FieldNumber{Name="Number"},
            });
            Console.WriteLine(GetJson(fields));
        }
        int yPos;
        int _xPos;
        int tab;
        const int tabSize = 32;
        int xPos {
            get {
                return _xPos + (tab*tabSize);
            }
            set {
                _xPos = value;
            }
        }
        void CreateFields(Field[] fields) {
            this.fields = fields;
            foreach (Field f in fields) {
                CreateField(f);
            }
        }
        string GetJson(Field[] fields) {
            string txt = "";
            foreach (var field in fields) {
                txt += field.Name;
                switch (field) {
                    case FieldText f:
                        txt += ": \"" + f.TextBox.Text + "\"";
                        break;
                    case FieldCheckBox f:
                        txt += ": " + f.CheckBox.Checked;
                        break;
                    case FieldNumber f:
                        txt += ": " + f.NumericUpDown.Value;
                        break;
                    case FieldArray f:
                        txt += "{\n" + GetJson(f.fields) + "\n}";
                        break;
                }
                txt += ",\n";
            }
            return txt;
        }
        void CreateField(Field field) {
            field.Label = new Label();
            field.Label.Text = field.Name;
            field.Label.Location = new Point(xPos, yPos);
            xPos += field.Label.Width;
            Controls.Add(field.Label);

            switch (field) {
                case FieldText f:
                    f.TextBox = new TextBox();
                    f.TextBox.Location = new Point(xPos, yPos);
                    f.TextBox.Text = f.DefaultText;
                    Controls.Add(f.TextBox);

                    xPos = 0;
                    yPos += field.Label.Size.Height;
                    break;
                case FieldCheckBox f:
                    f.CheckBox = new CheckBox();
                    f.CheckBox.Location = new Point(xPos, yPos);
                    f.CheckBox.Checked = f.Checked;
                    Controls.Add(f.CheckBox);

                    xPos = 0;
                    yPos += field.Label.Size.Height;
                    break;
                case FieldNumber f:
                    f.NumericUpDown = new NumericUpDown();
                    f.NumericUpDown.Location = new Point(xPos, yPos);
                    f.NumericUpDown.Value = f.DefaultNumber;
                    Controls.Add(f.NumericUpDown);

                    xPos = 0;
                    yPos += field.Label.Size.Height;
                    break;
                case FieldArray f:
                    tab++;
                    xPos = 0;
                    yPos += field.Label.Size.Height;
                    foreach (Field subF in f.fields) {
                        CreateField(subF);
                    }
                    tab--;
                    break;
            }
        }
    }
}
