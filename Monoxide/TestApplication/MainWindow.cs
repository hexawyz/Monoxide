using System;
using System.ComponentModel;
using System.MacOS.AppKit;
using System.MacOS.WebKit;
using System.MacOS.CoreGraphics;

namespace TestApplication
{
	public class MainWindow : Window
	{
		Button button1;
		Button button2;
		Button checkBox;
		WebView webView;
		DrawableView paintedView;
		
		public MainWindow()
		{
			CreateToolbarTemplate();
			Style |= WindowStyle.UnifiedTitleAndToolbar;
			Toolbar = new Toolbar() { TemplateName = "Main", Customizable = true };
			button1 = new Button() { Title = "Click me \u263A", Width = 100, Margin = new Thickness(0, 0, 200, 0), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom };
			button1.Action += HandleButton1Action;
			button2 = new Button() { Title = "\u26A0 Don't click me \u2620", Width = 200, Margin = new Thickness(0, 0, 0, 0), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom };
			button2.Action += HandleButton2Action;
			checkBox = new Button() { Title = "Closable", ButtonType = ButtonType.Switch, Width = 100, Height = 24, Margin = new Thickness(120, 0, 0, 100), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom };
			paintedView = new DrawableView() { Height = 100, Margin = new Thickness(0, 0, 0, 160), VerticalAlignment = VerticalAlignment.Bottom };
			paintedView.Draw += HandlePaintedViewDraw;
			webView = new WebView() { Margin = new Thickness(0, 0, 0, 260), VerticalAlignment = VerticalAlignment.Top };
#if DOCUMENT
//			checkBox.Checked = true;
#endif
			Title = "Hello From C#";
			Content.Children.AddRange
			(
				button1,
				button2,
				checkBox,
				new ColorWell() { Width = 100, Height = 100, Margin = new Thickness(10, 0, 0, 32), HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Bottom },
				new ColorWell() { Width = 100, Height = 100, Margin = new Thickness(0, 0, 10, 32), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom },
				new SearchField() { Width = double.NaN, Height = 22, Margin = new Thickness(120, 0, 120, 32), HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Bottom },
				new TextField() { Height = 22, Margin = new Thickness(120, 0, 120, 64), HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Bottom },
				new ComboBox() { Height = 22, Margin = new Thickness(10, 0, 10, 132), HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Bottom },
				paintedView,
				webView
			);
		}
		
		private void CreateToolbarTemplate()
		{
			if (ToolbarTemplate.IsDefined("Main")) return;
			
			var segmentedControl = new SegmentedControl() { Style = SegmentStyle.TextureRounded, SelectionMode = ItemSelectionMode.None };
			segmentedControl.Segments.Add(new Segment() { Image = Image.GoLeftTemplate });
			segmentedControl.Segments.Add(new Segment() { Image = Image.GoRightTemplate });
			var myItem1 = new ImageToolbarItem("Foo") { Label = "Foo", Image = Image.Info };
			var myItem2 = new ImageToolbarItem("Bar") { Label = "Bar", Image = Image.UserAccounts };
			var myItem3 = new ViewToolbarItem("FooBar") { Label = "FooBar", View = segmentedControl };
			var myItem4 = new ViewToolbarItem("Search") { Label = "Search", View = new SearchField() };
			
			ToolbarTemplate.TryDefine
			(
				"Main",
				new []
				{
					ToolbarItem.ColorsToolbarItem,
					ToolbarItem.FontsToolbarItem,
					ToolbarItem.SeparatorToolbarItem,
					ToolbarItem.SpaceToolbarItem,
					ToolbarItem.FlexibleSpaceToolbarItem,
					myItem1,
					myItem2,
					myItem3,
					myItem4
				},
				new []
				{
					ToolbarItem.ColorsToolbarItem,
					myItem1,
					myItem3,
					ToolbarItem.FlexibleSpaceToolbarItem,
					myItem4
				}
			);
		}

		private void HandleButton1Action(object sender, EventArgs e)
		{
			Console.WriteLine("Button 1 Clicked");
			//throw new InvalidOperationException("This is a dummy error to scare you !");
			Application.Current.ShowStandardAboutPanel();
		}
		
		private void HandleButton2Action(object sender, EventArgs e)
		{
			Console.WriteLine("Button 2 Clicked");
			using (var alert = new Alert())
			{
				alert.Style = AlertStyle.Critical;
				alert.ShowSuppressionButton = true;
				alert.MessageText = "\u2623 You are doomed ! \u2623";
				alert.InformativeText = "You're a bad guy ! \u2639" + Environment.NewLine + "You shouldn't have clicked this button." + Environment.NewLine + "Enjoy you fate. \u2604";
				alert.AddButton("Whatever");
				while (!alert.Suppress)
				{
					Application.Beep();
					alert.ShowDialog();
				}
			}
		}

		private void HandlePaintedViewDraw (object sender, DrawEventArgs e)
		{
			e.Context.FillColor = new RGBColor(1, 0, 0, 1);
			e.Context.FillRectangle(e.Bounds);
			e.Context.FillColor = new RGBColor(0, 0, 1, 1);
			e.Context.FillRectangle(paintedView.ActualWidth / 4, paintedView.ActualHeight / 4, paintedView.ActualWidth / 2, paintedView.ActualHeight / 2);
			e.Context.FillColor = new RGBColor(0, 1, 0, 1);
			e.Context.FillRectangle(paintedView.ActualWidth / 8, paintedView.ActualHeight / 4, paintedView.ActualWidth / 8, paintedView.ActualHeight / 2);
			e.Context.FillRectangle(6 * paintedView.ActualWidth / 8, paintedView.ActualHeight / 4, paintedView.ActualWidth / 8, paintedView.ActualHeight / 2);
			e.Context.FillColor = new RGBColor(1, 1, 0, 1);
			e.Context.FillEllipse(3 * paintedView.ActualWidth / 8, paintedView.ActualHeight / 8, paintedView.ActualWidth / 4, 6 * paintedView.ActualHeight / 8);
			e.Context.StrokeColor = new RGBColor(0, 0, 0, 1);
			e.Context.FillColor = new RGBColor(1, 1, 1, 1);
			e.Context.BeginPath();
			e.Context.MoveTo(10, 20);
			e.Context.AddArcTo(10, 10, 20, 10, 10);
			e.Context.AddLineTo(40, 10);
			e.Context.AddArcTo(50, 10, 50, 20, 10);
			e.Context.AddLineTo(50, 40);
			e.Context.AddArcTo(50, 50, 40, 50, 10);
			e.Context.AddLineTo(20, 50);
			e.Context.AddArcTo(10, 50, 10, 40, 10);
			e.Context.ClosePath();
			e.Context.DrawPath(PathDrawingMode.FillStroke);
			e.Context.FillColor = new RGBColor(0.1, 0.1, 0.1, 0.5);
			if (paintedView.ActualWidth > 40 && paintedView.ActualHeight > 40)
			{
				var half = paintedView.ActualWidth / 2;
				e.Context.FillRoundedRectangle(new Rectangle(half - 40, paintedView.ActualHeight / 16, 80, 14 * paintedView.ActualHeight / 16), 20);
			}
		}
		
		public bool CanClose { get { return checkBox.Checked; } }
		
		protected override void OnLoad (EventArgs e)
		{
			webView.SetMainFrameUrl("http://www.perdu.com/");
			base.OnLoad(e);
		}
		
		protected override void OnMinimizing(EventArgs e)
		{
			Console.WriteLine("Window Minimizing");
			base.OnMinimizing(e);
		}
		
		protected override void OnMinimized(EventArgs e)
		{
			Console.WriteLine("Window Minimized");
			base.OnMinimizing(e);
		}
		
		protected override void OnClosed(EventArgs e)
		{
			Console.WriteLine("Window Closed");
			base.OnClosed (e);
		}
		
		protected override void OnClosing(CancelEventArgs e)
		{
			Console.WriteLine("Window Closing");
			if (e.Cancel = !CanClose)
				Application.Beep();
			base.OnClosing (e);
		}

	}
}
