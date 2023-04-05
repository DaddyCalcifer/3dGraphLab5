using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphSprite
{
    public partial class Form1 : Form
    {
        private Rectangle Hero;
        Image[] run_left,run_right,idle_,att_l,att_r;
        private int FrameIndex = 0;
        readonly string AnimationPath = @"C:\Users\Nikita\Desktop\Animations\";
        Animation CurrentAnimation = Animation.idle;
        string last_slide;
        int AttackFramesLeft = 0;

        private const int Speed = 7, IdleTick=90, RunTick=55;

        Image background;
        enum Animation { left, right, idle };

        public Form1()
        {
            this.DoubleBuffered = true;
            InitializeComponent();

            Hero = new Rectangle(new Point((int)this.Width/2,(int)this.Height/2), new Size(77, 100));
            background = this.BackgroundImage;

            timer_update.Interval = IdleTick;
            timer_update.Tick += new EventHandler(Update);
            timer_update.Start();

            this.KeyDown += new KeyEventHandler(KeyDownned);
            this.KeyUp += new KeyEventHandler(KeyUpped);
            //
            String[] pathes;
            run_left = new Image[8];
            pathes = System.IO.Directory.GetFiles(AnimationPath + "\\left\\");
            for(int i=0; i < run_left.Length;i++)
            {
                run_left[i] = Image.FromFile(pathes[i]);
            }
            //
            run_right = new Image[8];
            pathes = System.IO.Directory.GetFiles(AnimationPath + "\\right\\");
            for (int i = 0; i < run_right.Length; i++)
            {
                run_right[i] = Image.FromFile(pathes[i]);
            }
            //
            idle_ = new Image[8];
            pathes = System.IO.Directory.GetFiles(AnimationPath + "\\idle\\");
            for (int i = 0; i < idle_.Length; i++)
            {
                idle_[i] = Image.FromFile(pathes[i]);
            }
            //
            att_l = new Image[4];
            pathes = System.IO.Directory.GetFiles(AnimationPath + "\\left\\attack\\");
            for (int i = 0; i < att_l.Length; i++)
            {
                att_l[i] = Image.FromFile(pathes[i]);
            }
            //
            att_r = new Image[4];
            pathes = System.IO.Directory.GetFiles(AnimationPath + "\\right\\attack\\");
            for (int i = 0; i < att_r.Length; i++)
            {
                att_r[i] = Image.FromFile(pathes[i]);
            }
        }

        private void KeyUpped(object sender, KeyEventArgs e)
        {
            CurrentAnimation = Animation.idle;
            timer_update.Interval = IdleTick;
        }

        private void KeyDownned(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "S":
                    CurrentAnimation = Animation.right;
                    Hero.Y += Speed;
                    break;
                case "A":
                    CurrentAnimation = Animation.left;
                    last_slide = "left";
                    Hero.X -= Speed;
                    break;
                case "W":
                    CurrentAnimation = Animation.right;
                    Hero.Y -= Speed;
                    break;
                case "D":
                    CurrentAnimation = Animation.right;
                    last_slide = "right";
                    Hero.X += Speed;
                    break;
                case "Space":
                    if (AttackFramesLeft == 0)
                    {
                        timer_update.Interval = IdleTick;
                        AttackFramesLeft = 4;
                        Attack();
                    }
                    break;
            }
            timer_update.Interval = RunTick;
        }

        private void Update(object sender, EventArgs e)
        {
            if (FrameIndex == 8) FrameIndex = 0;

            if (AttackFramesLeft == 0)
                PlayAnimation();
            else
            {
                Attack();
                AttackFramesLeft--;
            }

            if (Hero.X > this.Width + 77)
                Hero.X = -50;
            if (Hero.X < -77)
                Hero.X = this.Width + 50;

            if (Hero.Y > this.Height - 100)
                Hero.Y = -80;
            if (Hero.Y < -100)
                Hero.Y = this.Height - 90;

            FrameIndex++;
        }
        private void Attack()
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            Graphics cf = Graphics.FromImage(bitmap);
            cf.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            cf.DrawImage(background, new Rectangle(0, 0, this.Width, this.Height));
            if (last_slide == "right") cf.DrawImage(att_r[4-AttackFramesLeft], new Rectangle(Hero.X, Hero.Y, Hero.Width + 10, Hero.Height + 10));
            else cf.DrawImage(att_l[4-AttackFramesLeft], new Rectangle(Hero.X,Hero.Y,Hero.Width+10,Hero.Height+10));
            this.BackgroundImage = bitmap;
        }

        private void PlayAnimation()
        {
            Bitmap bitmap = new Bitmap(this.Width,this.Height);
            Graphics cf = Graphics.FromImage(bitmap);
            cf.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            cf.DrawImage(background,new Rectangle(0,0,this.Width,this.Height));
            switch(CurrentAnimation)
            {
                case Animation.left:
                    cf.DrawImage(run_left[FrameIndex], Hero);
                    break;
                case Animation.right:
                    cf.DrawImage(run_right[FrameIndex], Hero);
                    break;
               default:
                    cf.DrawImage(idle_[FrameIndex], Hero);
                    break;
            }
            
            this.BackgroundImage = bitmap;
        }
    }
}
