﻿using System;
using CodeHub.Core.ViewModels.Accounts;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using System.Reactive.Linq;
using SDWebImage;

namespace CodeHub.iOS.Cells
{
    public class AccountCellView : ReactiveTableViewCell<AccountItemViewModel>
    {
        public static NSString Key = new NSString("ProfileCell");
        public new readonly UIImageView ImageView;
        public readonly UILabel TitleLabel;
        public readonly UILabel SubtitleLabel;

        public AccountCellView(IntPtr handle)
            : base(handle)
        {
            ImageView = new UIImageView();
            ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            ImageView.Layer.MinificationFilter = MonoTouch.CoreAnimation.CALayer.FilterTrilinear;
            ImageView.Layer.MasksToBounds = true;

            TitleLabel = new UILabel();
            TitleLabel.TextColor = UIColor.FromWhiteAlpha(0.0f, 1f);
            TitleLabel.Font = UIFont.FromName("HelveticaNeue", 17f);

            SubtitleLabel = new UILabel();
            SubtitleLabel.TextColor = UIColor.FromWhiteAlpha(0.1f, 1f);
            SubtitleLabel.Font = UIFont.FromName("HelveticaNeue-Thin", 14f);

            ContentView.Add(ImageView);
            ContentView.Add(TitleLabel);
            ContentView.Add(SubtitleLabel);

            this.WhenViewModel(x => x.Username).Subscribe(x => TitleLabel.Text = x);
            this.WhenViewModel(x => x.Domain).Subscribe(x => SubtitleLabel.Text = x);
            this.WhenViewModel(x => x.AvatarUrl).Where(x => !string.IsNullOrEmpty(x)).Subscribe(x => ImageView.SetImage(new NSUrl(x), Images.LoginUserUnknown));
            this.WhenViewModel(x => x.AvatarUrl).Where(x => string.IsNullOrEmpty(x)).Subscribe(_ => ImageView.Image = Images.LoginUserUnknown);
            this.WhenViewModel(x => x.Selected).Subscribe(x => Accessory = x ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var imageSize = this.Bounds.Height - 30f;
            ImageView.Layer.CornerRadius = imageSize / 2;
            ImageView.Frame = new RectangleF(15, 15, imageSize, imageSize);

            var titlePoint = new PointF(ImageView.Frame.Right + 15f, 19f);
            TitleLabel.Frame = new RectangleF(titlePoint.X, titlePoint.Y, this.ContentView.Bounds.Width - titlePoint.X - 10f, TitleLabel.Font.LineHeight);
            SubtitleLabel.Frame = new RectangleF(titlePoint.X, TitleLabel.Frame.Bottom, this.ContentView.Bounds.Width - titlePoint.X - 10f, SubtitleLabel.Font.LineHeight + 1);
        }
    }
}

