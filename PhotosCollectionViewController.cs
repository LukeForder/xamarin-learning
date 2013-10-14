using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.AssetsLibrary;
using MonoTouch.CoreFoundation;

namespace CollectionViewLearningIPhone
{
	public class PhotoCollectionViewCell : UICollectionViewCell 
	{
		UIImageView _image;

		[Export("initWithFrame:")]
		public PhotoCollectionViewCell (RectangleF frame) : base(frame)
		{
			BackgroundView = new UIView { BackgroundColor = UIColor.Clear };

			SelectedBackgroundView = new UIView { BackgroundColor =  UIColor.Green, t };

			//ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
			//ContentView.Layer.BorderWidth = 2.0f;
			ContentView.BackgroundColor = UIColor.Clear;
			//ContentView.Transform = CGAffineTransform.MakeScale (0.95f, 0.95f);

			_image = new UIImageView (new RectangleF(0,0,75,75));
			_image.Center = ContentView.Center;
			_image.ClipsToBounds = true;
			_image.ContentMode = UIViewContentMode.ScaleAspectFill;
			//_image.SizeThatFits ();

			ContentView.AddSubview (_image);

		}

		public UIImage Image {
			set {
				this._image.Image
					= value;
			}
		}

	}

	public class PhotosCollectionViewController : UICollectionViewController
	{
		NSString photoCellId = new NSString ("photoCellId");

		List<UIImage> images = new List<UIImage>();

		public PhotosCollectionViewController (UICollectionViewLayout layout) : base(layout)
		{

		}

		public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var photoCell = (PhotoCollectionViewCell)collectionView.DequeueReusableCell (photoCellId, indexPath);

			photoCell.Image = images [indexPath.Item];

			return photoCell;
		}

		public override int GetItemsCount (UICollectionView collectionView, int section)
		{
			return images.Count;
		}

		public override int NumberOfSections (UICollectionView collectionView)
		{
			return 1;
		}

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			base.ItemSelected (collectionView, indexPath);
		}

		public override void ItemDeselected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			base.ItemDeselected (collectionView, indexPath);
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			CollectionView.RegisterClassForCell (typeof(PhotoCollectionViewCell), photoCellId);

			// next we need to load the images from the photo gallery
			ALAssetsLibrary library = new ALAssetsLibrary ();

			library.Enumerate (
				ALAssetsGroupType.SavedPhotos, 
				(ALAssetsGroup assetGroup,ref bool stop) => {
				if (assetGroup != null) {
					assetGroup.Enumerate (NSEnumerationOptions.Reverse,
					                     (ALAsset asset, int index, ref bool s) => {
						if (asset != null){
							var cgImage = asset.DefaultRepresentation.GetImage();
							var uiImage = UIImage.FromImage(cgImage);
							images.Add (uiImage);
							CollectionView.InsertItems(
								new NSIndexPath[] {
								NSIndexPath.FromItemSection(images.Count - 1, 0)
							});
						}

					});
				}

			},
				(NSError error) => {
				Console.WriteLine (error.Domain); });
		}
	}
}

