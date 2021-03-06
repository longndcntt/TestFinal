﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestFinal.Controls;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TestFinal.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Support.V4.Content;

[assembly: ExportRenderer(typeof(ImagePicker), typeof(ImagePickerRenderer))]
namespace TestFinal.Droid.Renderers
{
    public class ImagePickerRenderer : PickerRenderer
    {
        public ImagePickerRenderer(Context context) : base(context) { }

        ImagePicker element;
        
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            element = (ImagePicker)this.Element;
            var editText = this.Control;
            if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
            {
                editText.SetCompoundDrawablesWithIntrinsicBounds(AddPickerStyles(element.Image), null, null, null);
            }
            editText.CompoundDrawablePadding = 25;
        }

        public LayerDrawable AddPickerStyles(string imagePath)
        {
            Drawable[] layers = { GetDrawable(imagePath) };
            LayerDrawable layerDrawable = new LayerDrawable(layers);
            layerDrawable.SetLayerInset(0, 0, 0, 0, 0);

            return layerDrawable;
        }

        private BitmapDrawable GetDrawable(string imagePath)
        {
            int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            var bitmap = ((BitmapDrawable)drawable).Bitmap;

            var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
            result.Gravity = Android.Views.GravityFlags.Start;

            return result;
        }

    }
}