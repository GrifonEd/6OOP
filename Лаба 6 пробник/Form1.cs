using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаба_6_пробник
{
    public partial class Form1 : Form
    {
        Color current_color = Color.DarkBlue;
        Color basic_color = Color.DarkBlue;
        Color selected_color = Color.Red;
        Color selected_color1 = Color.Brown;
        int kolvo_elem = 0;
        static int sizeStorage = 1;
        int index_sozdania;
        Storage storage = new Storage(sizeStorage);
        
        public Form1()
        {
            InitializeComponent();
           
        }


        public class Shape
        {
            public PointF[] polygonPoints = new PointF[3];
            private string ClassName = "Shape";
            public int R = 60;
            public Color color;
            public int x, y;
            public bool narisovana = true;
            public virtual string Class_Name()
            {
                return ClassName;
            }
        }
        public class Square : Shape
        {
            private string ClassName = "Square";
            new public int  R =  60;
            public Square(int x, int y, Color color)
            {
                this.x = x-R ;
                this.y = y-R ;
                this.color = color;
            }
            public override string Class_Name()
            {
                return ClassName;
            }
            ~Square()
            {
            }
        }
       
        public class Triangle : Shape
        {
            private string ClassName = "Triangle";
            new public int R = 60;
            

            public Triangle(int x, int y, Color color)
            {
                this.x = x ;
                this.y = y;
                this.color = color;
                polygonPoints[0] = new PointF(this.x, (int)(this.y - 2 * R * Math.Sqrt(3) / 3));
                polygonPoints[1] = new PointF(this.x -R, (int)(this.y +   R * Math.Sqrt(3) / 3));
                polygonPoints[2] = new PointF(this.x + R, (int)(this.y +   R * Math.Sqrt(3) / 3));
               
            }
            public override string Class_Name()
            {
                return ClassName;
            }
            ~Triangle()
            {
            }
        }
        public class CCircle :  Shape 
        {
            private string ClassName = "CCircle";
            new public int R = 60;
            public CCircle(int x, int y, Color color)
            {
                this.x = x - R;
                this.y = y - R;
                this.color = color;
            }
            public override string Class_Name()
            {
                return ClassName;
            }

            ~CCircle()
            {
            }
        }
        private int Check_In(ref Storage storage, int size, int x, int y)
        {
            if (storage.kolvo_zanyatix(size) != 0)
            {
                for (int i = 0; i < size; i++)
                {
                    if (!storage.proverka(i))
                    {
                        if (storage.objects[i].Class_Name() == "CCircle")
                        {
                            if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                return i;
                        }
                        else if(storage.objects[i].Class_Name() == "Square")
                        {
                            if (Math.Abs(storage.objects[i].x - x) < storage.objects[i].R && Math.Abs( storage.objects[i].y - y) < storage.objects[i].R)
                                return i;
                        }
                        else if (storage.objects[i].Class_Name() == "Triangle")
                        {

                            if (Math.Abs(storage.objects[i].x - x) < storage.objects[i].R && Math.Abs(storage.objects[i].y - y) < storage.objects[i].R)
                                return i;
                        }
                    }
                }

            }
            return -1;
        }
        private void MyPaint(int kolvo_elem, ref Storage storage)
        {
            if (storage.objects[kolvo_elem] != null)
            {
                if (storage.objects[kolvo_elem].x+2*storage.objects[kolvo_elem].R  > 711)
                    storage.objects[kolvo_elem].x = 711- 2*storage.objects[kolvo_elem].R;
                if (storage.objects[kolvo_elem].x  < 0)
                    storage.objects[kolvo_elem].x =  0;
                if (storage.objects[kolvo_elem].y + 2*storage.objects[kolvo_elem].R> 420)
                    storage.objects[kolvo_elem].y = 420- 2*storage.objects[kolvo_elem].R;
                if (storage.objects[kolvo_elem].y < 0)
                    storage.objects[kolvo_elem].y = 0;
                Pen pen = new Pen(storage.objects[kolvo_elem].color, 4);
                if (storage.objects[kolvo_elem].Class_Name() == "CCircle")
                    picture.CreateGraphics().DrawEllipse(pen, storage.objects[kolvo_elem].x, storage.objects[kolvo_elem].y, storage.objects[kolvo_elem].R * 2, storage.objects[kolvo_elem].R * 2);
                else if (storage.objects[kolvo_elem].Class_Name() == "Square")
                { 
                    picture.CreateGraphics().DrawRectangle(pen, storage.objects[kolvo_elem].x, storage.objects[kolvo_elem].y, storage.objects[kolvo_elem].R * 2, storage.objects[kolvo_elem].R * 2);
                }
                else if (storage.objects[kolvo_elem].Class_Name() == "Triangle")
                {
                    /* polygonPoints[0] = new PointF(this.x, (int)(this.y - 2 * R * Math.Sqrt(3) / 3));
                polygonPoints[1] = new PointF(this.x - R, (int)(this.y + R * Math.Sqrt(3) / 3));
                polygonPoints[2] = new PointF(this.x + R, (int)(this.y + R * Math.Sqrt(3) / 3));*/
                    Triangle Hi = (Triangle )storage.objects[kolvo_elem];
                    picture.CreateGraphics().DrawPolygon(pen, Hi.polygonPoints);
                }
            }
        }
        private void Remove_Selection(ref Storage storage)
        {
            for (int i = 0; i < sizeStorage; ++i)
                if (!storage.proverka(i))
                {
                   if(storage.objects[i].color==selected_color)
                    storage.objects[i].color = current_color;
                    if (storage.objects[i].narisovana == true)
                        MyPaint(i, ref storage);
                }
        }
        private void picture_MouseClick(object sender, MouseEventArgs e)
        {
          
            if (кругToolStripMenuItem.Checked)
            {
                
                Shape krug = new CCircle(e.X, e.Y, current_color);
                if (Check_In(ref storage, sizeStorage, krug.x, krug.y) != -1)
                {
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        int x = e.X - krug.R;
                        int y = e.Y - krug.R;
                        for (int i = 0; i < sizeStorage; i++)
                            if (!storage.proverka(i))
                            {
                                if (storage.objects[i].Class_Name() == "CCircle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Square")
                                {
                                    if (Math.Abs(storage.objects[i].x - x) < storage.objects[i].R && Math.Abs(storage.objects[i].y - y) < storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Triangle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                            }
                    }
                    else
                    {
                        int x = e.X - krug.R;
                        int y = e.Y - krug.R;
                        Remove_Selection(ref storage);
                        for (int i = 0; i < sizeStorage; i++)
                            if (!storage.proverka(i))
                            {
                                if (storage.objects[i].Class_Name() == "CCircle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color1;
                                        MyPaint(i, ref storage);
                                        break;
                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Square") 
                                {
                                    if (Math.Abs(storage.objects[i].x - x) < storage.objects[i].R && Math.Abs(storage.objects[i].y - y) < storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color1;
                                        MyPaint(i, ref storage);
                                        break;
                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Triangle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color1;
                                        MyPaint(i, ref storage);
                                        break;
                                    }
                                }
                            }
                        storage.objects[Check_In(ref storage, sizeStorage, krug.x, krug.y)].color = selected_color1;
                        MyPaint(Check_In(ref storage, sizeStorage, krug.x, krug.y), ref storage);
                    }
                    return;
                }
                storage.add_object(ref sizeStorage, ref krug, kolvo_elem, ref index_sozdania);
                Remove_Selection(ref storage);
                storage.objects[index_sozdania].color = selected_color;
                MyPaint(index_sozdania, ref storage);
                kolvo_elem++;

            }
            else if (квадратToolStripMenuItem.Checked)
            {
                Shape kvad = new Square(e.X, e.Y, basic_color);
                if (Check_In(ref storage, sizeStorage, kvad.x, kvad.y) != -1)
                {
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        int x = e.X-kvad.R;
                        int y = e.Y - kvad.R;
                        for (int i = 0; i < sizeStorage; i++)
                            if (!storage.proverka(i))
                            {
                                if (storage.objects[i].Class_Name() == "CCircle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Square")
                                {
                                    if (Math.Abs(storage.objects[i].x - x) < storage.objects[i].R && Math.Abs(storage.objects[i].y - y) < storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Triangle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                            }
                    }
                    else
                    {
                        int x = e.X-kvad.R ;
                        int y = e.Y-kvad.R ;
                        Remove_Selection(ref storage);
                        for (int i = 0; i < sizeStorage; i++)
                            if (!storage.proverka(i))
                            {
                                if (storage.objects[i].Class_Name() == "CCircle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);
                                        break;
                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Square")
                                {
                                    if (Math.Abs(storage.objects[i].x - x) < storage.objects[i].R && Math.Abs(storage.objects[i].y - y) < storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);
                                        break;
                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Triangle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                            }
                        storage.objects[Check_In(ref storage, sizeStorage, kvad.x, kvad.y)].color = selected_color;
                        MyPaint(Check_In(ref storage, sizeStorage, kvad.x, kvad.y), ref storage);
                    }
                    return;
                }
                storage.add_object(ref sizeStorage, ref kvad, kolvo_elem, ref index_sozdania);
                Remove_Selection(ref storage);
                storage.objects[index_sozdania].color = selected_color;
                MyPaint(index_sozdania, ref storage);
                kolvo_elem++;
            }
            if (треугольникToolStripMenuItem.Checked)
            {
                Shape treug = new Triangle(e.X, e.Y, basic_color);
                if (Check_In(ref storage, sizeStorage, treug.x, treug.y) != -1)
                {
                    if (Control.ModifierKeys == Keys.Control)
                    {
                        int x = e.X ;
                        int y = e.Y ;
                        for (int i = 0; i < sizeStorage; i++)
                            if (!storage.proverka(i))
                            {
                                if (storage.objects[i].Class_Name() == "CCircle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Square")
                                {
                                    if (Math.Abs(storage.objects[i].x - x) < storage.objects[i].R && Math.Abs(storage.objects[i].y - y) < storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Triangle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);

                                    }
                                }
                            }
                    }
                    else
                    {
                        int x = e.X ;
                        int y = e.Y ;
                        Remove_Selection(ref storage);
                        for (int i = 0; i < sizeStorage; i++)
                            if (!storage.proverka(i))
                            {
                                if (storage.objects[i].Class_Name() == "CCircle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);
                                        break;
                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Square")
                                {
                                    if (Math.Abs(storage.objects[i].x - x) < storage.objects[i].R && Math.Abs(storage.objects[i].y - y) < storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);
                                        break;
                                    }
                                }
                                else if (storage.objects[i].Class_Name() == "Triangle")
                                {
                                    if ((x - storage.objects[i].x) * (x - storage.objects[i].x) + (y - storage.objects[i].y) * (y - storage.objects[i].y) <= storage.objects[i].R * storage.objects[i].R)
                                    {
                                        storage.objects[i].color = selected_color;
                                        MyPaint(i, ref storage);
                                        break;
                                    }
                                }
                            }
                        storage.objects[Check_In(ref storage, sizeStorage, treug.x, treug.y)].color = selected_color;
                        MyPaint(Check_In(ref storage, sizeStorage, treug.x, treug.y), ref storage);
                    }
                    return;
                }
                storage.add_object(ref sizeStorage, ref treug, kolvo_elem, ref index_sozdania);
                Remove_Selection(ref storage);
                storage.objects[index_sozdania].color = selected_color;
                MyPaint(index_sozdania, ref storage);
                kolvo_elem++;

            }
        }
        public class Storage
        {

            public Shape[] objects;
            public Storage(int size)
            {
                objects = new Shape[size];
                for (int i = 0; i < size; i++)
                    objects[i] = null;
            }
            public void add_object(ref int size, ref Shape new_object, int ind, ref int index_sozdania)
            {
                Storage storage1 = new Storage(size + 1);
                for (int i = 0; i < size; i++)
                    storage1.objects[i] = objects[i];
                size = size + 1;
                videlenie(size);
                for (int i = 0; i < size; i++)
                    objects[i] = storage1.objects[i];
                objects[ind] = new_object;
                index_sozdania = ind;
            }
             public Shape GetObject(int index)
            {
                return objects[index];
            }
            public void videlenie(int size)
            {
                objects = new Shape[size];
                for (int i = 0; i < size; i++)
                    objects[i] = null;
            }
            public int kolvo_zanyatix(int size)
            {
                int count_zanyatih = 0;
                for (int i = 0; i < size; i++)
                {
                    if (!proverka(i))
                        count_zanyatih++;
                }
                return count_zanyatih;
            }
            public bool proverka(int kolvo_elem)
            {
                if (objects[kolvo_elem] == null)
                    return true;
                else return false;
            }
            public void Delte_obj(ref int kolvo_elem)
            {
                if (objects[kolvo_elem] != null)
                {
                    objects[kolvo_elem] = null;
                    kolvo_elem--;
                }
            }

        }



        private void button2_Click(object sender, EventArgs e)
        {
            picture.Refresh();
            for (int i = 0; i < sizeStorage; i++)
            {
                if (!storage.proverka(i))
                {
                    storage.objects[i].narisovana = false;
                    storage.objects[i].color = basic_color;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            picture.Refresh();
            if (storage.kolvo_zanyatix(sizeStorage) != 0)
                for (int i = 0; i < sizeStorage; ++i)
                {
                    MyPaint(i, ref storage); // Рисует круг
                    if (!storage.proverka(i))
                        storage.objects[i].narisovana = true; // Для элемента ставим флаг - нарисован
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
          
            for (int i = 0; i < sizeStorage; i++)
            {
                storage.Delte_obj(ref i);
            }
            sizeStorage = 1;
            storage = new Storage(sizeStorage);
            kolvo_elem = 0;
        }

        private void ButtonDelThis_Click(object sender, EventArgs e)
        {
            if (storage.kolvo_zanyatix(sizeStorage) != 0)
                for (int i = 0; i < sizeStorage; i++)
                    if (storage.proverka(i) == false && storage.objects[i].color == selected_color)
                        storage.Delte_obj(ref i);

            button2_Click(sender, e);
            button3_Click(sender, e);
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {

        }

        private void picture_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text =(e.X).ToString();
            label2.Text = (e.Y).ToString();
        }

        

        private void picture_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Right)
            {
                for (int i = 0; i < sizeStorage; i++)
                    if (storage.objects[i].color == selected_color)
                    {
                        storage.objects[i].x += 1;
                        MyPaint(i, ref storage);
                    }

            }
        }

        private void кругToolStripMenuItem_Click(object sender, EventArgs e)
        {
            кругToolStripMenuItem.Checked = true;
            квадратToolStripMenuItem.Checked = false;
            треугольникToolStripMenuItem.Checked = false;
        }

        private void квадратToolStripMenuItem_Click(object sender, EventArgs e)
        {
            кругToolStripMenuItem.Checked = false;
            квадратToolStripMenuItem.Checked = true;
            треугольникToolStripMenuItem.Checked = false;
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            кругToolStripMenuItem.Checked = false;
            квадратToolStripMenuItem.Checked = false;
            треугольникToolStripMenuItem.Checked = true;
        }

        
        

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
                int norm = 0;
                for (int i = 0; i < sizeStorage - 1; i++)
                {
                    if (!storage.proverka(i)) 
                    if (storage.objects[i].color == selected_color) 
                    norm = i;


                }
                if (!storage.proverka(norm)) 
                {
                    if (storage.objects[norm].Class_Name() != "Triangle")
                    {
                        if (e.KeyChar == (char)Keys.S)
                        {


                            {
                                storage.objects[norm].y += 3;


                                MyPaint(norm, ref storage);
                                //  button3_Click(sender, e);
                            }



                        }
                        else if (e.KeyChar == (char)Keys.A)
                        {


                            {
                                storage.objects[norm].x -= 3;


                                MyPaint(norm, ref storage);
                                //  button3_Click(sender, e);
                            }



                        }
                        else if (e.KeyChar == (char)Keys.D)
                        {


                            {
                                storage.objects[norm].x += 3;


                                MyPaint(norm, ref storage);
                                //  button3_Click(sender, e);
                            }



                        }
                        else if (e.KeyChar == (char)Keys.W)
                        {


                            {
                                storage.objects[norm].y -= 3;


                                MyPaint(norm, ref storage);
                                // button3_Click(sender, e);
                            }



                        }
                        else if (e.KeyChar == (char)Keys.L)
                        {
                           // storage.objects[norm].y -= 1;
                            //storage.objects[norm].x -= 1;
                            storage.objects[norm].R += 1;

                            MyPaint(norm, ref storage);
                        }

                        else if (e.KeyChar == (char)Keys.K)
                        {
                           // storage.objects[norm].y += 1;
                           // storage.objects[norm].x += 1;
                            storage.objects[norm].R -= 1;
                            if (storage.objects[norm].R < 1)
                                storage.objects[norm].R = 1;
                            MyPaint(norm, ref storage);
                        }
                    }
                    else
                    {
                        if (e.KeyChar == (char)Keys.S)
                        {


                            {
                                storage.objects[norm].y += 3;

                                storage.objects[norm].polygonPoints[0].Y += 3;
                                storage.objects[norm].polygonPoints[1].Y += 3;
                                storage.objects[norm].polygonPoints[2].Y += 3;

                                MyPaint(norm, ref storage);
                                // button3_Click(sender, e);
                            }



                        }
                        if (e.KeyChar == (char)Keys.A)
                        {


                            {
                                storage.objects[norm].x -= 3;
                                storage.objects[norm].polygonPoints[0].X -= 3;
                                storage.objects[norm].polygonPoints[1].X -= 3;
                                storage.objects[norm].polygonPoints[2].X -= 3;

                                MyPaint(norm, ref storage);
                                // button3_Click(sender, e);
                            }



                        }
                        if (e.KeyChar == (char)Keys.D)
                        {


                            {
                                storage.objects[norm].x += 3;
                                storage.objects[norm].polygonPoints[0].X += 3;
                                storage.objects[norm].polygonPoints[1].X += 3;
                                storage.objects[norm].polygonPoints[2].X += 3;

                                MyPaint(norm, ref storage);
                                // button3_Click(sender, e);
                            }



                        }
                        if (e.KeyChar == (char)Keys.W)
                        {


                            {
                                storage.objects[norm].y -= 3;
                                storage.objects[norm].polygonPoints[0].Y -= 3;
                                storage.objects[norm].polygonPoints[1].Y -= 3;
                                storage.objects[norm].polygonPoints[2].Y -= 3;

                                MyPaint(norm, ref storage);
                                // button3_Click(sender, e);
                            }



                        }
                    }
                }
                button3_Click(sender, e);
            }
            return;
        }

        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            синийToolStripMenuItem.Checked = true;
            желтыйToolStripMenuItem.Checked = false;
            черныйToolStripMenuItem.Checked = false;
            зеленыйToolStripMenuItem.Checked = false;
            selected_color = Color.DarkBlue;
            current_color = Color.DarkBlue;
        }

        private void желтыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            синийToolStripMenuItem.Checked = false;
            желтыйToolStripMenuItem.Checked = true;
            черныйToolStripMenuItem.Checked = false;
            зеленыйToolStripMenuItem.Checked = false;
            selected_color = Color.Yellow;
            current_color = Color.Yellow;
        }

        private void черныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            синийToolStripMenuItem.Checked = false;
            желтыйToolStripMenuItem.Checked = false;
            черныйToolStripMenuItem.Checked = true;
            зеленыйToolStripMenuItem.Checked = false;
            selected_color = Color.Black;
            current_color = Color.Black;
        }

        private void зеленыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            синийToolStripMenuItem.Checked = false;
            желтыйToolStripMenuItem.Checked = false;
            черныйToolStripMenuItem.Checked = false;
            зеленыйToolStripMenuItem.Checked = true;
            selected_color = Color.Green;
            current_color = Color.Green;
        }

        private void picture_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    
}
