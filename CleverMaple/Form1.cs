using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace CleverMaple
{
    public partial class Form1 : Form
    {
        private bool m_working;
        private IntPtr m_hWnd;
        private Bitmap m_monsterImg;
        private Bitmap m_hpbarImg;
        private float m_shouldClimb;
        private KeyboardSimulator m_keyboard;
        private DirectInputKeyCode m_moveKey = DirectInputKeyCode.DIK_DELETE;
        private int m_shoudTeleport = 0;
        private Point m_oldPosition;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HotKeyManager.RegisterHotKey(Keys.F2, KeyModifiers.Control);
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKey_TogglePressed);

            m_monsterImg = new Bitmap("ghost.bmp");
            m_hpbarImg = new Bitmap("hpbar.bmp");

            m_keyboard = new KeyboardSimulator();
        }

        private void buttonToggle_Click(object sender, EventArgs e)
        {
            if (!m_working)
                StartProgram();
            else
                StopProgram();
        }

        private void HotKey_TogglePressed(object sender, HotKeyEventArgs e)
        {
            if (!m_working)
                StartProgram();
            else
                StopProgram();
        }

        private void StartProgram()
        {
            m_hWnd = Win32.FindWindow("MapleStoryClass", null);
            
            if (CheckGameWindow())
            {
                m_working = true;
                toggleButton.Text = "Stop";

                SetControlsEnabled(false);
                HealTimer.Start();
                timer1.Start();
                timer2.Start();
            }
        }

        private void StopProgram()
        {
            m_working = false;
            toggleButton.Text = "Start";

            SetControlsEnabled(true);
            HealTimer.Stop();
            timer1.Stop();
            timer2.Stop();
        }

        private void HealTimerNum_Changed(object sender, EventArgs e)
        {
            HealTimer.Interval = (int)HealTimerNum.Value;
        }

        private void SetControlsEnabled(bool value)
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Enabled = value;
            }

            toggleButton.Enabled = true;
        }

        private void KeyBox_Focus(object sender, EventArgs e)
        {
            var box = sender as TextBox;
            box.Text = null;
        }

        private void HealTimer_Tick(object sender, EventArgs e)
        {
            HealTimer.Stop();

            if (CheckGameWindow())
            {
                HealTimer.Enabled = true;

                RECT rct = new RECT();
                Win32.GetWindowRect(m_hWnd, ref rct);

                rct.Left += 3;
                rct.Top += 22;
                rct.Right -= 3;
                rct.Bottom -= 3;

                using (Bitmap screen = Win32.CopyFromScreen(rct.Left + 218, rct.Top + 580, 217, 18,
                    PixelFormat.Format32bppArgb))
                {
                    if (screen.GetPixel(53, 0) == Color.FromArgb(255, 255, 0, 0)
                        || screen.GetPixel(53, 6) == Color.FromArgb(255, 190, 190, 190))
                    {
                        RestoreHp();
                    }

                    if (screen.GetPixel(160, 0) == Color.FromArgb(255, 255, 0, 0) 
                        || screen.GetPixel(161, 6) == Color.FromArgb(255, 190, 190, 190))
                    {
                        RestoreMp();
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            
            if (CheckGameWindow())
            {
                timer1.Enabled = true;

                RECT rct = new RECT();
                Win32.GetWindowRect(m_hWnd, ref rct);

                rct.Left += 3;
                rct.Top += 22;
                rct.Right -= 3;
                rct.Bottom -= 3 + 68;

                using (Bitmap screen = Win32.CopyFromScreen(rct.Left, rct.Top, (rct.Right - rct.Left), (rct.Bottom - rct.Top),
                    PixelFormat.Format32bppArgb))
                {
                    Point point = new Point();

                    // Find character name postion
                    if (Win32.FindCharacterName(screen, ref point))
                    {
                        Point characterPos = new Point(point.X + 26, point.Y - 7);

                        using (Bitmap image = Win32.CropImage(screen, new Rectangle(0, characterPos.Y - 80,
                            screen.Width, 80)))
                        {
                            // Find monster position
                            if (Win32.FindImage(image, m_monsterImg, ref point) || Win32.FindImage(image, m_hpbarImg, ref point))
                            {
                                // Stop action
                                StopAction();

                                DirectInputKeyCode oldMoveDir = m_moveKey;

                                // Monster on left
                                if (point.X < characterPos.X)
                                    MoveCharacter(DirectInputKeyCode.DIK_LEFT);
                                else // On right
                                    MoveCharacter(DirectInputKeyCode.DIK_RIGHT);

                                // Add some delay when changing arrow direction
                                if (oldMoveDir != m_moveKey)
                                    Thread.Sleep(200);

                                Random rand = new Random();
                                if (rand.NextDouble() < 0.9) // 90%
                                    m_keyboard.KeyPress(DirectInputKeyCode.DIK_S); // Fire arrow
                                else
                                    m_keyboard.KeyPress(DirectInputKeyCode.DIK_A); // Magic cross

                                m_shoudTeleport = 0;
                            }
                            else // No monsters were found
                            {
                                m_shoudTeleport += 200;

                                float teleportMax;
                                if (isCharacterOnUpperFloor(screen))
                                    teleportMax = 1500;
                                else
                                    teleportMax = 4000;

                                // Should teleport?
                                if (m_shoudTeleport >= teleportMax)
                                {
                                    m_shoudTeleport = 0;

                                    TeleportCharacter(DirectInputKeyCode.DIK_UP, 25);

                                    // Randomly move left or right
                                    Random rand = new Random();
                                    if (rand.NextDouble() >= 0.5)
                                        MoveCharacter(DirectInputKeyCode.DIK_LEFT);
                                    else
                                        MoveCharacter(DirectInputKeyCode.DIK_RIGHT);
                                }

                                m_keyboard.KeyDown(DirectInputKeyCode.DIK_Z);
                            }
                        }
                    }
                    else
                    {
                        m_keyboard.KeyDown(DirectInputKeyCode.DIK_Z);
                    }

                    // Evade the doors
                    EvadeTheDoors(screen);

                    // Unstuck from a ladder
                    UnstuckFromLadder(screen, 200);
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();

            if (CheckGameWindow())
            {
                timer2.Enabled = true;

                StopAction();

                Thread.Sleep(1000);
                m_keyboard.KeyPress(DirectInputKeyCode.DIK_R);

                Thread.Sleep(1000);
                m_keyboard.KeyPress(DirectInputKeyCode.DIK_T);
            }
        }

        private bool CheckGameWindow(bool msg=true)
        {
            if (!Win32.IsWindow(m_hWnd))
            {
                if (msg)
                    MessageBox.Show("Could not find MapleStory game client.");

                return false;
            }

            return true;
        }
        
        private void RestoreHp()
        {
            m_keyboard.KeyPress(DirectInputKeyCode.DIK_NEXT);
        }

        private void RestoreMp()
        {
            m_keyboard.KeyPress(DirectInputKeyCode.DIK_END);
        }

        private void EvadeTheDoors(Bitmap screen)
        {
            if (m_moveKey == DirectInputKeyCode.DIK_LEFT)
            {
                Point point = new Point();
                if (Win32.FindPixelColor(screen, 26, 71, 11, 12, Color.FromArgb(255, 255, 255, 136), ref point))
                    MoveCharacter(DirectInputKeyCode.DIK_RIGHT);
            }
            else if (m_moveKey == DirectInputKeyCode.DIK_RIGHT)
            {
                Point point = new Point();
                if (Win32.FindPixelColor(screen, 234, 70, 13, 12, Color.FromArgb(255, 255, 255, 136), ref point))
                    MoveCharacter(DirectInputKeyCode.DIK_LEFT);
            }
        }

        private void UnstuckFromLadder(Bitmap screen, int interval)
        {
            Point pos = new Point();

            if (Win32.FindPixelColor(screen, 25, 48, 222, 43, Color.FromArgb(255, 255, 255, 136), ref pos))
            {
                if (m_oldPosition == pos)
                    m_shouldClimb += interval;
                else
                    m_shouldClimb = 0;

                if (m_shouldClimb >= 4000)
                {
                    StopAction();
                    StopMoving();
                    Thread.Sleep(1000);
                    
                    // Climb up!
                    m_keyboard.KeyDown(DirectInputKeyCode.DIK_UP);
                    Thread.Sleep(2000);
                    m_keyboard.KeyUp(DirectInputKeyCode.DIK_UP);

                    // Randomly move left or right
                    Random rand = new Random();
                    if (rand.NextDouble() >= 0.5)
                        MoveCharacter(DirectInputKeyCode.DIK_LEFT);
                    else
                        MoveCharacter(DirectInputKeyCode.DIK_RIGHT);
                }
            }

            m_oldPosition = pos;
        }

        private void StopAction(bool attack = true, bool teleport = true, bool pickup = true)
        {
            if (attack)
            {
                m_keyboard.KeyUp(DirectInputKeyCode.DIK_A);
                m_keyboard.KeyUp(DirectInputKeyCode.DIK_S);
            }

            if (teleport)
                m_keyboard.KeyUp(DirectInputKeyCode.DIK_Z);

            if (pickup)
                m_keyboard.KeyUp(DirectInputKeyCode.DIK_C);
        }

        private void StopMoving()
        {
            m_keyboard.KeyUp(DirectInputKeyCode.DIK_UP);
            m_keyboard.KeyUp(DirectInputKeyCode.DIK_DOWN);
            m_keyboard.KeyUp(DirectInputKeyCode.DIK_LEFT);
            m_keyboard.KeyUp(DirectInputKeyCode.DIK_RIGHT);

            m_moveKey = DirectInputKeyCode.DIK_DELETE;
        }

        private void MoveCharacter(DirectInputKeyCode key)
        {
            StopMoving();

            m_keyboard.KeyDown(key);
            m_moveKey = key;
        }

        private void TeleportCharacter(DirectInputKeyCode key, int delay)
        {
            StopAction();
            StopMoving();

            m_keyboard.KeyDown(key);
            m_keyboard.KeyPress(DirectInputKeyCode.DIK_C);
            Thread.Sleep(delay);
            m_keyboard.KeyUp(key);
        }

        private bool isCharacterOnUpperFloor(Bitmap screen)
        {
            Point point = new Point();
            if (Win32.FindPixelColor(screen, 47, 62, 183, 12, Color.FromArgb(255, 255, 255, 136), ref point))
                return true;

            return false;
        }
    }
}
