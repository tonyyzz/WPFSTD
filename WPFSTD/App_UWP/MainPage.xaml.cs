using Microsoft.Graphics.Canvas.Effects;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App_UWP
{
	/// <summary>
	/// 可用于自身或导航至 Frame 内部的空白页。
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			this.InitializeComponent();
			initializeFrostedGlass(GlassHost);
		}
		private void initializeFrostedGlass(UIElement glassHost)
		{
			
			Visual hostVisual = ElementCompositionPreview.GetElementVisual(glassHost);
			Compositor compositor = hostVisual.Compositor;
			var glassEffect = new GaussianBlurEffect()
			{
				BlurAmount = 15.0f,
				BorderMode = EffectBorderMode.Hard,
				Source = new ArithmeticCompositeEffect
				{
					MultiplyAmount = 0,
					Source1Amount = 0.5f,
					Source2Amount = 0.5f,
					Source1 = new CompositionEffectSourceParameter("backdropBrush"),
					Source2 = new ColorSourceEffect
					{
						Color = Color.FromArgb(255, 245, 245, 245)
					}
				}
			};
			var effectFactory = compositor.CreateEffectFactory(glassEffect);

			var backdropBrush = compositor.CreateBackdropBrush();
			var effectBrush = effectFactory.CreateBrush();
			effectBrush.SetSourceParameter("backdropBrush", backdropBrush);
			var glassVisual = compositor.CreateSpriteVisual();
			glassVisual.Brush = backdropBrush;
			ElementCompositionPreview.SetElementChildVisual(glassHost, glassVisual);
			var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
			bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
			glassVisual.StartAnimation("Size", bindSizeAnimation);
		}
	}
}
