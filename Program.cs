using System.Timers;
using Timers = System.Timers;
namespace TrayHeartRate
{
    internal class Program
    {

        private static readonly Font font = new("Arial Narrow", 27, FontStyle.Bold);
        private static readonly Brush textBrush = Brushes.White;
        private static readonly Brush backgroundBrush = Brushes.Black;
        private const int iconSize = 32;
        private static int lastHour = -1;

        private static readonly NotifyIcon trayIcon = new()
        {
            BalloonTipIcon = ToolTipIcon.Info,
            Visible = true,
            ContextMenuStrip = new ContextMenuStrip(),
        };

        private static void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            RereshIcon();
        }

        private static void RereshIcon()
        {
            var now = DateTime.UtcNow;

            if (lastHour == now.Hour)
            {
                return;
            }

            lastHour = now.Hour;

            var bmp = new Bitmap(iconSize, iconSize);
            using var img = Graphics.FromImage(bmp);

            img.FillRectangle(backgroundBrush, 0, 0, iconSize, iconSize);

            img.DrawString($"{now:HH}", font, textBrush, new PointF(-6, -4));

            trayIcon.Text = $"{now:HH} hours UTC";

            var icon = Icon.FromHandle(bmp.GetHicon());

            trayIcon.Icon?.Dispose();

            trayIcon.Icon = icon;
        }

        static void Main()
        {
            trayIcon.ContextMenuStrip!.Items.Add("Quit", null, (s, e) => Application.Exit());

            RereshIcon();

            var refreshInterval = TimeSpan.FromMinutes(1);
            var timer = new Timers.Timer(refreshInterval);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            Application.Run();

            trayIcon.Dispose();
        }
    }
}